using System;
using System.Threading.Tasks;
using NetworkExtension;

using Foundation;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// Web view configuration.
    /// </summary>
    public static class WebViewConfiguration
    {
        public static class IOS
        {
            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="T:Xamarin.Auth.WebAuthenticatorController"/> 
            /// is using WKWebView or by default UIWebView
            /// </summary>
            /// <value><c>true</c> if is using WKW eb view; otherwise, <c>false</c>.</value>
            public static bool IsUsingWKWebView
            {
                get;
                set;
            } = false;

            static string user_agent = null;

            public static string UserAgent
            {
                get
                {
                    return user_agent;
                }
                set
                {
                    user_agent = value;
                    SetDefaultUserAgent();

                    return;
                }
            }

            static IOS()
            {
                UserAgentFromUIWebView();
                UserAgentFromWKWebView();

                UserAgent =
                    useragent_uiwebview
                    //"Mozilla/5.0 (iPhone; CPU iPhone OS 9_2 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13C75 Safari/601.1"
                    //"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A"
                    //"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36"
                    ;

                return;
            }


            static string app_name_uiwebview = null;
            static string useragent_uiwebview = null;

            static void UserAgentFromUIWebView()
            {
                UIKit.UIWebView wv = new UIKit.UIWebView(CoreGraphics.CGRect.Empty);
                wv.LoadHtmlString("<html></html>", null);

                // case sensitive stuff:
                //      navigator.* 
                // few SO posts with Pascal case will not work!
                app_name_uiwebview = wv.EvaluateJavascript("navigator.appName");
                useragent_uiwebview = wv.EvaluateJavascript("navigator.userAgent");
                wv = null;

                #if DEBUG
                System.Diagnostics.Debug.WriteLine($"User-Agent API useragent_uiwebview = {useragent_uiwebview}");
                #endif

                return;
            }

            //static string app_name_wkwebview = null;
            static string useragent_wkwebview = null;

            static void UserAgentFromWKWebView()
            {
                WebKit.WKWebViewConfiguration wkconf = new WebKit.WKWebViewConfiguration()
                {
                };
                WebKit.WKWebView wkwv = new WebKit.WKWebView(CoreGraphics.CGRect.Empty, wkconf);

                // TODO: WKWebKit UserAgent JavaScript handler not triggered from JavaScript
                WebKit.WKJavascriptEvaluationResult handler =
                    (NSObject result, NSError err) =>
                        {
                            useragent_wkwebview = null;

                            if (err != null)
                            {
                                System.Console.WriteLine(err);
                            }
                            if (result != null)
                            {
                                #if DEBUG
                                System.Diagnostics.Debug.WriteLine($"User-Agent API result");
                                #endif
                            }

                            return;
                        };

                // case sensitive stuff:
                //      navigator.* 
                // few SO posts with Pascal case will not work!
                wkwv.EvaluateJavaScript((NSString)"navigator.appName", handler);
                wkwv.EvaluateJavaScript((NSString)"navigator.userAgent", handler);

                wkwv.LoadHtmlString("<html></html>", null);

                #if DEBUG
                System.Diagnostics.Debug.WriteLine($"User-Agent API useragent_wkwebview = {useragent_wkwebview}");
                #endif
                      
                return;
            }


            static void HandleWKJavascriptEvaluationResult(Foundation.NSObject result, Foundation.NSError error)
            {

                return;
            }

            static void SetDefaultUserAgent()
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
