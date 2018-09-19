using System;

namespace Xamarin.Auth
{
    public class AuthException : Exception
    {
        public AuthException()
        {
        }

        public AuthException(string message)
            : base(message)
        {
        }

        public AuthException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
