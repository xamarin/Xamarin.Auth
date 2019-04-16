using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// Authenticator completed event arguments.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
	internal class AuthenticatorCompletedEventArgs : EventArgs
    #else
    public class AuthenticatorCompletedEventArgs : EventArgs
    #endif
    {
        /// <summary>
        /// Whether the authentication succeeded and there is a valid <see cref="Account"/>.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user is authenticated; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsAuthenticated { get { return Account != null; } }

        /// <summary>
        /// Gets the account created that represents this authentication.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public Account Account { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Auth.AuthenticatorCompletedEventArgs"/> class.
        /// </summary>
        /// <param name='account'>
        /// The account created or <see langword="null"/> if authentication failed or was canceled.
        /// </param>
        public AuthenticatorCompletedEventArgs(Account account)
        {
            Account = account;
        }
    }
}
