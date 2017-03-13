//
//  Copyright 2012-2016, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Threading.Tasks;
using Xamarin.Utilities.iOS;
using Xamarin.Controls;

#if ! __UNIFIED__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#else
using Foundation;
using UIKit;
#endif

namespace Xamarin.Auth
{
    /// <summary>
    /// The ViewController that the WebAuthenticator presents to the user.
    /// </summary>
    internal class WebAuthenticatorController : UIViewController
    {
        protected WebAuthenticator authenticator;

        UIWebView webView;
        UIActivityIndicatorView activity;
        UIView authenticatingView;
        ProgressLabel progress;
        bool webViewVisible = true;

        const double TransitionTime = 0.25;

        bool keepTryingAfterError = true;

        public WebAuthenticatorController(WebAuthenticator authenticator)
        {
            this.authenticator = authenticator;

            authenticator.Error += HandleError;
            authenticator.BrowsingCompleted += HandleBrowsingCompleted;

            //
            // Create the UI
            //
            Title = authenticator.Title;

            if (authenticator.AllowCancel)
            {
                NavigationItem.LeftBarButtonItem = 
                    new UIBarButtonItem
                    (
                        UIBarButtonSystemItem.Cancel,
                        delegate
                        {
                            Cancel();
                            #region
                            //---------------------------------------------------------------------------------------
                            /// Pull Request - manually added/fixed
                            ///		OAuth2Authenticator changes to work with joind.in OAuth #91
                            ///		https://github.com/xamarin/Xamarin.Auth/pull/91
                            ///		
                            DismissViewControllerAsync(true);
                            ///---------------------------------------------------------------------------------------
                            #endregion
                        }
                );
            }

            activity = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(activity);

            webView = new UIWebView(View.Bounds)
            {
                Delegate = new WebViewDelegate(this),
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
            };
            View.AddSubview(webView);
            View.BackgroundColor = UIColor.Black;

            // InvalidOperation - either delegates or events!
			//this.webView.LoadFinished += WebView_LoadFinished;
			//
			// Locate our initial URL
			//
			BeginLoadingInitialUrl();

            return;
        }

        void Cancel()
        {
            authenticator.OnCancelled();
        }

        void BeginLoadingInitialUrl()
        {
            authenticator.GetInitialUrlAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    keepTryingAfterError = false;
                    authenticator.OnError(t.Exception);
                }
                else
                {
                    // Delete cookies so we can work with multiple accounts
                    if (this.authenticator.ClearCookiesBeforeLogin)
                        WebAuthenticator.ClearCookies();

                    //
                    // Begin displaying the page
                    //
                    LoadInitialUrl(t.Result);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void LoadInitialUrl(Uri url)
        {
            if (!webViewVisible)
            {
                progress.StopAnimating();
                webViewVisible = true;
                UIView.Transition
                            (
                                fromView: authenticatingView,
                                toView: webView,
                                duration: TransitionTime,
                                options: UIViewAnimationOptions.TransitionCrossDissolve,
                                completion: null
                             );
            }

            if (url != null)
            {
                var request = new NSUrlRequest(new NSUrl(url.AbsoluteUri));
                NSUrlCache.SharedCache.RemoveCachedResponse(request); // Always try
                webView.LoadRequest(request);
            }
        }

        void HandleBrowsingCompleted(object sender, EventArgs e)
        {
            if (!webViewVisible) return;

            if (authenticatingView == null)
            {
                authenticatingView = new UIView(View.Bounds)
                {
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                    BackgroundColor = UIColor.FromRGB(0x33, 0x33, 0x33),
                };
                progress = new ProgressLabel("Authenticating...");
                var f = progress.Frame;
                var b = authenticatingView.Bounds;
                f.X = (b.Width - f.Width) / 2;
                f.Y = (b.Height - f.Height) / 2;
                progress.Frame = f;
                authenticatingView.Add(progress);
            }
            else
            {
                authenticatingView.Frame = View.Bounds;
            }

            webViewVisible = false;

            progress.StartAnimating();

            UIView.Transition
                        (
                            fromView: webView,
                            toView: authenticatingView,
                            duration: TransitionTime,
                            options: UIViewAnimationOptions.TransitionCrossDissolve,
                            completion: null
                        );
        }

        void HandleError(object sender, AuthenticatorErrorEventArgs e)
        {
            var after = keepTryingAfterError ?
                (Action)BeginLoadingInitialUrl :
                (Action)Cancel;

            if ( authenticator.ShowErrors == true && e.Exception != null)
            {
                this.ShowError("Authentication Error e.Exception = ", e.Exception, after);
            }
            else
            {
                this.ShowError("Authentication Error e.Message = ", e.Message, after);
            }

            return;
        }

        #region
        ///-------------------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		Added IsAuthenticated check #88
        ///		https://github.com/xamarin/Xamarin.Auth/pull/88
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (authenticator.AllowCancel && authenticator.IsAuthenticated())
            {
                Cancel();
            }
        }
        ///---------------------------------------------------------------------------------------
        #endregion

        void WebView_LoadFinished(object sender, EventArgs e)
		{
            if (((WebRedirectAuthenticator)authenticator).IsLoadableRedirectUri == false)
            {
                this.DismissViewControllerAsync(true);
            }

            return;
		}

		protected class WebViewDelegate : UIWebViewDelegate
        {
            protected WebAuthenticatorController controller;
            Uri lastUrl;


            public WebViewDelegate(WebAuthenticatorController controller)
            {
                this.controller = controller;
            }

            /// <summary>
            /// Whether the UIWebView should begin loading data.
            /// </summary>
            /// <returns><c>true</c>, if start load was shoulded, <c>false</c> otherwise.</returns>
            /// <param name="webView">Web view.</param>
            /// <param name="request">Request.</param>
            /// <param name="navigationType">Navigation type.</param>
			public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
            {
                NSUrl nsUrl = request.Url;

                string msg = null;
                #if DEBUG
				msg = String.Format("WebAuthenticatorController.ShouldStartLoad {0}", nsUrl.AbsoluteString);
				System.Diagnostics.Debug.WriteLine(msg);
                #endif

                WebRedirectAuthenticator wra = null;
				wra = (WebRedirectAuthenticator)this.controller.authenticator;

				if (nsUrl != null && !controller.authenticator.HasCompleted)
                {
                    Uri url;
					if (Uri.TryCreate(nsUrl.AbsoluteString, UriKind.Absolute, out url))
                    {
						string host = url.Host.ToLower();
                        string scheme = url.Scheme;

                        #if DEBUG
						msg = String.Format("WebAuthenticatorController.ShouldStartLoad {0}", url.AbsoluteUri);
						System.Diagnostics.Debug.WriteLine(msg);
                        msg = string.Format("                          Host   = {0}", host);
                        System.Diagnostics.Debug.WriteLine(msg);
						msg = string.Format("                          Scheme = {0}", scheme);
						System.Diagnostics.Debug.WriteLine(msg);
                        #endif

						if (host == "localhost" || host == "127.0.0.1" || host == "::1")
                        {
                            wra.IsLoadableRedirectUri = false;
                            this.controller.DismissViewControllerAsync(true);
                        }
                        else
                        {
                            wra.IsLoadableRedirectUri = true;
						}

                        controller.authenticator.OnPageLoading(url);
                    }
                }

				return wra.IsLoadableRedirectUri;
            }

            public override void LoadStarted(UIWebView webView)
            {
                controller.activity.StartAnimating();

                webView.UserInteractionEnabled = false;
            }

            public override void LoadFailed(UIWebView webView, NSError error)
            {
				if (error.Domain == "WebKitErrorDomain")
				{
                    if (error.Code == 102)
                    {
                        // 
                        // WebViewDelegate.ShouldStartLoad returned false
                        // localhost, 127.0.0.1, ::1
                        // TODO: custom uris
                        // No need to show error - return immediately
                        return;
                    }
				}
				else if (error.Domain == "NSURLErrorDomain")
                {
                    // {The operation couldn’t be completed. (NSURLErrorDomain error -999.)}
                    if (error.Code == -999)
                    {
                        // delegate is getting a "cancelled" (-999) failure, 
                        //      that might be originated in javascript or 
                        //      fast clicks!!
                        //      perhaps even in a UIWebView bug.
                        return;
                    }
                }
                else 

                controller.activity.StopAnimating();

                webView.UserInteractionEnabled = true;

                controller.authenticator.OnError(error.LocalizedDescription);

                return;
            }

            public override void LoadingFinished(UIWebView webView)
            {
                controller.activity.StopAnimating();

                webView.UserInteractionEnabled = true;

                var url = new Uri(webView.Request.Url.AbsoluteString);
                if (url != lastUrl && !controller.authenticator.HasCompleted)
                {
                    lastUrl = url;
                    controller.authenticator.OnPageLoaded(url);
                }

                return;
            }
        }
    }
}

