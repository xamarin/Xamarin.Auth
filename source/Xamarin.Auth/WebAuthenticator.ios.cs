using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AuthenticateUIType =
            UIKit.UIViewController
            //System.Object
            ;
using System.Text;
using Xamarin.Auth;

namespace Xamarin.Auth
{
    /// <summary>
    /// An authenticator that displays a web page.
    /// </summary>
    public abstract partial class WebAuthenticator
    {
        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        protected override AuthenticateUIType GetPlatformUI()
        {
            AuthenticateUIType ui = null;
            if (this.IsUsingNativeUI == true)
            {
                Uri uri = GetInitialUrlAsync().Result;
                IDictionary<string, string> query_parts = WebEx.FormDecode(uri.Query);
                if (query_parts.ContainsKey("redirect_uri"))
                {
                    Uri redirect_uri = new Uri(query_parts["redirect_uri"]);
                    string scheme = redirect_uri.Scheme;
                    if (scheme.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("WARNING");
                        sb.AppendLine($"Scheme = {scheme}");
                        sb.AppendLine($"Native UI used with http[s] schema!");
                        sb.AppendLine($"Redirect URL will be loaded in Native UI!");
                        sb.AppendLine($"OAuth Data parsing might fail!");

                        ShowErrorForNativeUI(sb.ToString());
                    }
                }
                ui = GetPlatformUINative();
            }
            else
            {
                ui = GetPlatformUIEmbeddedBrowser();
            }

            return ui;
        }

        /// <summary>
        /// Gets the platform  UI (Android - WebView).
        /// </summary>
        /// <returns>
        /// The platform Native UI (embeded/integrated Browser Control/Widget/View (WebView)).
        /// Android.Support.CustomTabs.CustomTabsIntent
        /// </returns>
        /// <see cref="https://components.xamarin.com/gettingstarted/xamandroidsupportcustomtabs"/>
        protected AuthenticateUIType GetPlatformUIEmbeddedBrowser()
        {
            // Embedded Browser - Deprecated
            UIKit.UINavigationController nc = null;
            nc = new UIKit.UINavigationController(new WebAuthenticatorController(this));

            AuthenticateUIType ui = nc;

            return ui;
        }

        /// <summary>
        /// Clears all cookies.
        /// </summary>
        /// <seealso cref="ClearCookiesBeforeLogin"/>
        public static void ClearCookies()
        {
            var store = Foundation.NSHttpCookieStorage.SharedStorage;
            var cookies = store.Cookies;
            foreach (var c in cookies)
            {
                store.DeleteCookie(c);
            }

#if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"WebAuthenticator.ClearCookies ");
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            return;
        }


    }
}

