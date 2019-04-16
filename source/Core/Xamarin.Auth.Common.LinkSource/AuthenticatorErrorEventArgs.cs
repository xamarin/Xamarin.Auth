using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Utilities;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// Authenticator error event arguments.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal class AuthenticatorErrorEventArgs : EventArgs
    #else
    public class AuthenticatorErrorEventArgs : EventArgs
    #endif
    {
        /// <summary>
        /// Gets a message describing the error.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the exception that signaled the error if there was one.
        /// </summary>
        /// <value>
        /// The exception or <see langword="null"/>.
        /// </value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Auth.AuthenticatorErrorEventArgs"/> class
        /// with a message but no exception.
        /// </summary>
        /// <param name='message'>
        /// A message describing the error.
        /// </param>
        public AuthenticatorErrorEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Auth.AuthenticatorErrorEventArgs"/> class with an exception.
        /// </summary>
        /// <param name='exception'>
        /// The exception signaling the error. The message of this object is retrieved from this exception or
        /// its inner exceptions.
        /// </param>
        public AuthenticatorErrorEventArgs(Exception exception)
        {
            Message = exception.GetUserMessage();
            Exception = exception;
        }

    }
}
