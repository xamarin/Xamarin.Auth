using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AuthenticateUIType =
            Android.Content.Intent
            //System.Object
            ;
using UIContext =
            Android.Content.Context
            //Android.App.Activity
            ;

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
        protected override AuthenticateUIType GetPlatformUI(UIContext context)
        {
            AuthenticateUIType ui = null;
            if (this.IsUsingNativeUI == true)
            {
                ui = GetPlatformUINative(context);
            }
            else
            {
                ui = GetPlatformUIEmbeddedBrowser(context);
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
        protected AuthenticateUIType GetPlatformUIEmbeddedBrowser(UIContext context)
        {
            // Embedded Browser - Deprecated
            AuthenticateUIType ui = new AuthenticateUIType(context, typeof(WebAuthenticatorActivity));
            ui.PutExtra("ClearCookies", ClearCookiesBeforeLogin);
            var state = new WebAuthenticatorActivity.State
            {
                Authenticator = this,
            };
            ui.PutExtra("StateKey", WebAuthenticatorActivity.StateRepo.Add(state));

            return ui;
        }

        public AuthenticateUIType AuthenticationUIPlatformSpecificEmbeddedBrowser(UIContext context)
        {
            return GetPlatformUIEmbeddedBrowser(context);
        }


        /// <summary>
        /// Clears all cookies.
        /// </summary>
        /// <seealso cref="ClearCookiesBeforeLogin"/>
        public static void ClearCookies()
        {
            global::Android.Webkit.CookieSyncManager.CreateInstance(global::Android.App.Application.Context);
            global::Android.Webkit.CookieManager.Instance.RemoveAllCookie();
        }


    }
}

