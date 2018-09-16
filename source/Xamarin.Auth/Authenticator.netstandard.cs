using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

#if __ANDROID__
using AuthenticateUIType = Android.Content.Intent;
#elif __IOS__
using AuthenticateUIType = UIKit.UIViewController;
#elif WINDOWS_UWP
using AuthenticateUIType = System.Uri;
#elif __NETSTANDARD__
using AuthenticateUIType = System.Object;
#endif

namespace Xamarin.Auth
{
    /// <summary>
    /// An authenticator that displays a web page.
    /// </summary>
    public abstract partial class Authenticator
    {
        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        public AuthenticateUIType GetUI()
        {
            return GetPlatformUI();
        }

        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        protected abstract AuthenticateUIType GetPlatformUI();

        /// <summary>
        /// Clears all cookies.
        /// </summary>
        /// <seealso cref="ClearCookiesBeforeLogin"/>
        public async static Task ClearCookiesAsync()
        {
            await Task.Run( () => ClearCookies() );

            return;
        }

        public static void ClearCookies()
        {
            throw new PlatformNotSupportedException();
        }
    }
}

