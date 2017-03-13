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
            // UIKit.UIViewController;
            System.Object
            ;

namespace Xamarin.Auth
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
        }


        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        protected override AuthenticateUIType GetPlatformUI()
        {
            System.Object ui = null;
            if (this.IsUsingNativeUI == true)
            {
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
            UIKit.UINavigationController nc = new UIKit.UINavigationController(new WebAuthenticatorController(this));

            AuthenticateUIType ui = nc;

            return ui;
        }
    }
}

