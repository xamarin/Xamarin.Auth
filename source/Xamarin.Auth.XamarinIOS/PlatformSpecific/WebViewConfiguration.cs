using System;

namespace Xamarin.Auth
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

            public static string UserAgent
            {
                get;
                set;
            }

            static IOS()
            {
                UserAgent =
					"Mozilla/5.0 (iPhone; CPU iPhone OS 9_2 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13C75 Safari/601.1"
					//"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A"
                    //"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36"
                    ;

                return;
            }
        }

    }
}
