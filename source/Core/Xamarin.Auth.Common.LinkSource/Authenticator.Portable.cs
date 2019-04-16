using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Utilities;


using AuthenticateUIType = System.Object;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// A process and user interface to authenticate a user.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal abstract partial class Authenticator
    #else
    public abstract partial class Authenticator
    #endif
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
    }
}

