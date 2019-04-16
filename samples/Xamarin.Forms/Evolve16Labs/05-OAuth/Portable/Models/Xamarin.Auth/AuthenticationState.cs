using System;

using Xamarin.Auth;

namespace ComicBook
{
    public class AuthenticationState
    {
        /// <summary>
        /// The authenticator.
        /// </summary>
        // TODO:
        // Oauth1Authenticator inherits from WebAuthenticator
        // Oauth2Authenticator inherits from WebRedirectAuthenticator
        public static Xamarin.Auth.WebAuthenticator Authenticator;
    }
}
