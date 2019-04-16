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

#if __ANDROID__
using AuthenticateUIType = Android.Content.Intent;
#elif __IOS__ && __UNIFIED__
using AuthenticateUIType = UIKit.UIViewController;
#elif __IOS__ && ! __UNIFIED__
using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
#elif WINDOWS_PHONE && SILVERLIGHT
using AuthenticateUIType = System.Uri;
#elif PORTABLE
using AuthenticateUIType = System.Object;
#elif NETSTANDARD1_6
using AuthenticateUIType = System.Object;
#endif

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
        /// Clears all cookies.
        /// </summary>
        /// <seealso cref="ClearCookiesBeforeLogin"/>
        public new static void ClearCookies()
        {
            throw new NotImplementedException(LibraryUtilities.MessageNotImplementedException);
        }

        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        protected override AuthenticateUIType GetPlatformUI()
        {
            // System.Object
            AuthenticateUIType ui = PlatformUIMethod();

            return ui;
        }

        protected AuthenticateUIType GetPlatformUIEmbeddedBrowser()
        {
            throw new NotImplementedException("PCL bite-n-switch");

            // System.Uri uri_netfx = this.GetInitialUrlAsync().Result;

            // System.Object
            // AuthenticateUIType ui = null;

            // return ui;
        }

        public AuthenticateUIType AuthenticationUIPlatformSpecificEmbeddedBrowser()
        {
            return GetPlatformUIEmbeddedBrowser();
        }
    }
}

