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

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// An authenticator that displays a web page.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal abstract partial class WebAuthenticator
    #else
    public abstract partial class WebAuthenticator
    #endif
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

