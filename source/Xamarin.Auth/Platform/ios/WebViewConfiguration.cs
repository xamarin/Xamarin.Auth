using Foundation;

namespace Xamarin.Auth
{
    public static class WebViewConfiguration
    {
        public static class IOS
        {
            public static bool IsUsingWKWebView { get; set; }

            public static string UserAgent { get; set; }

            public static string UserAgentUIWebViewDefault { get; private set; }

            public static string UserAgentWKWebViewDefault { get; private set; }

            private static NSOperatingSystemVersion os_ver = NSProcessInfo.ProcessInfo.OperatingSystemVersion;
            private static readonly string sys_ver = UIKit.UIDevice.CurrentDevice.SystemVersion;


            static IOS()
            {
                UserAgentFromWKWebView();
                UserAgentFromUIWebView();

                UserAgent =
                    UserAgentUIWebViewDefault
                    // "Mozilla/5.0 (iPhone; CPU iPhone OS 9_2 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13C75 Safari/601.1"
                    // "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A"
                    // "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36"
                    ;
            }


            static string app_name_uiwebview = null;

            static void UserAgentFromUIWebView()
            {
                UIKit.UIWebView wv = new UIKit.UIWebView(CoreGraphics.CGRect.Empty);
                wv.LoadHtmlString("<html></html>", null);

                // case sensitive stuff:
                //      navigator.* 
                // few SO posts with Pascal case will not work!
                app_name_uiwebview = wv.EvaluateJavascript("navigator.appName");
                UserAgentUIWebViewDefault = wv.EvaluateJavascript("navigator.userAgent");
                wv = null;

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"User-Agent API useragent_uiwebview = {UserAgentUIWebViewDefault}");
#endif

                return;
            }

            static void UserAgentFromWKWebView()
            {
                WebKit.WKWebViewConfiguration wkconf = new WebKit.WKWebViewConfiguration()
                {
                };
                WebKit.WKWebView wkwv = new WebKit.WKWebView(CoreGraphics.CGRect.Empty, wkconf);

                // TODO: WKWebKit UserAgent JavaScript handler not triggered from JavaScript
                WebKit.WKJavascriptEvaluationResult handler = HandleWKJavascriptEvaluationResult;
                // case sensitive stuff:
                //      navigator.* 
                // few SO posts with Pascal case will not work!
                wkwv.EvaluateJavaScript((NSString)"navigator.userAgent", handler);
                //wkwv.EvaluateJavaScript((NSString)"navigator.appName", handler);

                wkwv.LoadHtmlString("<html></html>", null);

                return;
            }


            static void HandleWKJavascriptEvaluationResult(Foundation.NSObject result, Foundation.NSError err)
            {
                if (err != null)
                {
                    System.Diagnostics.Debug.WriteLine($"User-Agent API error = {err}");

                    UserAgentWKWebViewDefault = err.ToString();
                }
                if (result != null)
                {
                    System.Diagnostics.Debug.WriteLine($"User-Agent API result = {result}");

                    UserAgentWKWebViewDefault = result.ToString();
                }

                System.Diagnostics.Debug.WriteLine($"User-Agent API useragent_wkwebview = {UserAgentWKWebViewDefault}");

                return;
            }

            // must be called before WkWebView is created.
            static void SetDefaultUserAgent()
            {
                if (os_ver.Major >= 8 && os_ver.Major < 9)
                {
                    // set default useragent for UIWebView and WkWebView
                    NSDictionary dictionary = NSDictionary.FromObjectAndKey
                                                (
                                                    NSObject.FromObject(UserAgent),
                                                    NSObject.FromObject("UserAgent")
                                                );
                    NSUserDefaults.StandardUserDefaults.RegisterDefaults(dictionary);

                    return;
                }
            }
        }
    }
}
