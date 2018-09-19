using System;
using System.Threading.Tasks;
using System.Text;
using Foundation;
using UIKit;
using WebKit;

namespace Xamarin.Auth
{
    internal partial class WebAuthenticatorController : UIViewController
    {
      private  const double TransitionTime = 0.25;

        private WebAuthenticator authenticator;

       private UIWebView ui_web_view;
       private WKWebView wk_web_view;
       private UIView web_view = null;

        private UIActivityIndicatorView activity;
        private UIView authenticatingView;
        private ProgressLabel progress;
        private bool webViewVisible = true;

        private bool keepTryingAfterError = true;

        public WebAuthenticatorController(WebAuthenticator authenticator)
            : this(authenticator, WebViewConfiguration.IsUsingWKWebView)
        {
        }

        public WebAuthenticatorController(WebAuthenticator authenticator, bool is_using_wkwebview)
        {
            WebViewConfiguration.IsUsingWKWebView = is_using_wkwebview;

            this.authenticator = authenticator;

            authenticator.Error += HandleError;
            authenticator.BrowsingCompleted += HandleBrowsingCompleted;

            // Create the UI
            if (authenticator.AllowCancel)
            {
                NavigationItem.LeftBarButtonItem =
                    new UIBarButtonItem
                    (
                        UIBarButtonSystemItem.Cancel,
                        delegate
                        {
                            Cancel();

                            DismissViewControllerAsync(true);
                        }
                );
            }

            activity = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(activity);

            if (WebViewConfiguration.IsUsingWKWebView == false)
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
			sb.AppendLine($"        WebViewConfiguration.IsUsingWKWebView = {WebViewConfiguration.IsUsingWKWebView}");
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
                    wk_web_view.CustomUserAgent = WebViewConfiguration.UserAgent;
                }

                web_view = wk_web_view;
            }
            else
            {
                // Fallback to Embedded WebView
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
                if (WebViewConfiguration.IsUsingWKWebView == false)
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

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (authenticator.AllowCancel && authenticator.IsAuthenticated())
            {
                Cancel();
            }
        }

        void WebView_LoadFinished(object sender, EventArgs e)
		{
            if (((WebRedirectAuthenticator)authenticator).IsLoadableRedirectUri == false)
            {
                this.DismissViewControllerAsync(true);
            }

            return;
		}
    }
}

