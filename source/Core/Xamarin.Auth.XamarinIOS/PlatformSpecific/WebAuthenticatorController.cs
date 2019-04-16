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
using System.Text;

#if !__UNIFIED__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#else
using Foundation;
using UIKit;
using WebKit;
#endif

#if ! AZURE_MOBILE_SERVICES
using Xamarin.Utilities.iOS;
using Xamarin.Controls;
#else
using Xamarin.Utilities._MobileServices.iOS;
using Xamarin.Controls._MobileServices;
#endif

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// The ViewController that the WebAuthenticator presents to the user.
    /// </summary>
    internal partial class WebAuthenticatorController : UIViewController
    {
        protected WebAuthenticator authenticator;

        UIWebView ui_web_view;
        WKWebView wk_web_view;
        UIView web_view = null;

        UIActivityIndicatorView activity;
        UIView authenticatingView;
        ProgressLabel progress;
        bool webViewVisible = true;

        const double TransitionTime = 0.25;

        bool keepTryingAfterError = true;

        public WebAuthenticatorController(WebAuthenticator authenticator)
            : this(authenticator, WebViewConfiguration.IOS.IsUsingWKWebView)
        {
            return;
        }

        public WebAuthenticatorController(WebAuthenticator authenticator, bool is_using_wkwebview)
        {
            WebViewConfiguration.IOS.IsUsingWKWebView = is_using_wkwebview;

            this.authenticator = authenticator;

            authenticator.Error += HandleError;
            authenticator.BrowsingCompleted += HandleBrowsingCompleted;

            //
            // Create the UI
            //
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

            if (WebViewConfiguration.IOS.IsUsingWKWebView == false)
            {
                // UIWebView

                #if DEBUG
                StringBuilder sb1 = new StringBuilder();
                sb1.Append("Embedded WebView using - UIWebView");
                System.Diagnostics.Debug.WriteLine(sb1.ToString());
                #endif

                web_view = PrepareUIWebView();

                this.View.AddSubview((UIWebView)web_view);
             }
            else
            {
                // WKWebView - availabilty and fallback is done in PrepareWKWebView()

                #if DEBUG
                StringBuilder sb1 = new StringBuilder();
                sb1.Append("Embedded WebView using - WKWebView");
                System.Diagnostics.Debug.WriteLine(sb1.ToString());
                #endif

                web_view = PrepareWKWebView();

                this.View.AddSubview((WKWebView)web_view);
            }

            #if DEBUG
			authenticator.Title = "Auth " + web_view.GetType().ToString();
            #endif

			Title = authenticator.Title;
			View.BackgroundColor = UIColor.Black;

            // InvalidOperation - either delegates or events!
            //this.webView.LoadFinished += WebView_LoadFinished;
            //
            // Locate our initial URL
            //
            BeginLoadingInitialUrl();

            #if DEBUG
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"WebAuthenticatorController ");
			sb.AppendLine($"        WebViewConfiguration.IsUsingWKWebView = {WebViewConfiguration.IOS.IsUsingWKWebView}");
			sb.AppendLine($"        authenticator.IsUsingNativeUI         = {authenticator.IsUsingNativeUI}");
			sb.AppendLine($"        authenticator.Title                   = {authenticator.Title}");
			System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

			return;
        }

        protected UIView PrepareUIWebView()
        {
            ui_web_view = new UIWebView(View.Bounds)
            {
                Delegate = new UIWebViewDelegate(this),
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
            };
            web_view = ui_web_view;
            View.AddSubview((UIWebView)web_view);

            return web_view;
        }

        protected UIView PrepareWKWebView()
        {
            WebKit.WKWebViewConfiguration wk_web_view_configuration = null;

            if
                (
                    ObjCRuntime.Class.GetHandle("WKWebView") != IntPtr.Zero
                    &&
                    UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                )
            {
                wk_web_view_configuration = new WebKit.WKWebViewConfiguration();

                if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    wk_web_view_configuration.WebsiteDataStore = WKWebsiteDataStore.NonPersistentDataStore;
                }

                wk_web_view = new WebKit.WKWebView(View.Frame, wk_web_view_configuration)
                {
                    UIDelegate = new WKWebViewUIDelegate(this),
                    NavigationDelegate = new WKWebViewNavigationDelegate(this),
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                };

                if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    //  cheating!
                    //  http://www.useragentstring.com/pages/useragentstring.php?typ=Browser
                    wk_web_view.CustomUserAgent = WebViewConfiguration.IOS.UserAgent;
                }

                web_view = wk_web_view;
            }
            else
            {
                // Fallback to Embedded WebView
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("WKWebView not available!");
                msg.AppendLine("Fallback to UIWebView");

				this.ShowErrorForNativeUIAlert(msg.ToString());

                web_view = PrepareUIWebView();
            }


            return web_view;
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
                    Uri uri = t.Result;
                    LoadInitialUrl(uri);
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
                                toView: web_view,
                                duration: TransitionTime,
                                options: UIViewAnimationOptions.TransitionCrossDissolve,
                                completion: null
                             );
            }

            if (url != null)
            {
                var request = new NSUrlRequest(new NSUrl(url.AbsoluteUri));
                NSUrlCache.SharedCache.RemoveCachedResponse(request); // Always try
                if (WebViewConfiguration.IOS.IsUsingWKWebView == false)
                {
                    ui_web_view.LoadRequest(request);
                }
                else
                {
                    wk_web_view.LoadRequest(request);
				}
            }

            return;
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
                            fromView: web_view,
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

            if ( !authenticator.ShowErrors ) {
                after();
                return;
            }

            if ( e.Exception != null)
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

        protected void ShowErrorForNativeUIAlert(string v)
        {
        	new Plugin.Threading.UIThreadRunInvoker().BeginInvokeOnUIThread
        						(
        							() =>
        							{
        								UIKit.UIAlertView alert = null;
        								alert = new UIKit.UIAlertView
        												(
        													"WARNING",
        													v,
        													null,
        													"Ok",
        													null
        												);
        								alert.Show();
        							}
        						);
        	return;
        }
    }
}

