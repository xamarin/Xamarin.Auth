using System;
using System.Text;
using System.Threading.Tasks;

using Foundation;
using NetworkExtension;
using WebKit;

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

                    return;
                }
            }

            public static string UserAgentUIWebViewDefault
            {
            	get
            	{
            		return useragent_uiwebview;
            	}
            }

            public static string UserAgentWKWebViewDefault
            {
            	get
            	{
            		return useragent_wkwebview;
            	}
            }

            private static NSOperatingSystemVersion os_ver = NSProcessInfo.ProcessInfo.OperatingSystemVersion;
            private static string sys_ver = UIKit.UIDevice.CurrentDevice.SystemVersion;


            static IOS()
            {
                UserAgentFromWKWebView();
                UserAgentFromUIWebView();

                #if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Xamarin.Auth.WebViewConfiguration.IOS");
                sb.AppendLine($"         NSOperatingSystemVersion = {os_ver.Major}.{os_ver.Minor}");
                sb.AppendLine($"         SystemVersion            = {sys_ver}");
                sb.AppendLine($"WARNING");
                sb.AppendLine($"    in IOS 9.x+ use new WKWebView().wkwv.CustomUserAgent");
                sb.AppendLine($"    new WKWebView().CustomUserAgent");
                sb.AppendLine($"    in IOS 8.x use");
                sb.AppendLine($"    new WKWebView().CustomUserAgent");
                #endif


                UserAgent =
                    UserAgentUIWebViewDefault
                    // "Mozilla/5.0 (iPhone; CPU iPhone OS 9_2 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13C75 Safari/601.1"
                    // "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A"
                    // "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36"
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

                    useragent_wkwebview = err.ToString();
                }
                if (result != null)
                {
                    System.Diagnostics.Debug.WriteLine($"User-Agent API result = {result}");
 
                    useragent_wkwebview = result.ToString();
                }

                System.Diagnostics.Debug.WriteLine($"User-Agent API useragent_wkwebview = {useragent_wkwebview}");

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
                if (os_ver.Major >= 9 && os_ver.Major < 10)
                {

                    return;
                }

                return;
            }

            public static new string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine($"Xamarin.Auth.WebViewConfiguration.IOS");
                sb.Append    ($"         UserAgentUIWebViewDefault = ");
                sb.AppendLine($"{Xamarin.Auth.WebViewConfiguration.IOS.UserAgentUIWebViewDefault}");
                sb.Append    ($"         UserAgentWKWebViewDefault = ");
                sb.AppendLine($"{Xamarin.Auth.WebViewConfiguration.IOS.UserAgentWKWebViewDefault}");
                sb.Append    ($"         UserAgent                 = ");

                return sb.ToString();
            }
        }
    }
}
