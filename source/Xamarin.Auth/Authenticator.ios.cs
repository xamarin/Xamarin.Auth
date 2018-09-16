using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AuthenticateUIType =
            UIKit.UIViewController
            //System.Object
            ;

namespace Xamarin.Auth
{
    /// <summary>
    /// A process and user interface to authenticate a user.
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
    }
}

