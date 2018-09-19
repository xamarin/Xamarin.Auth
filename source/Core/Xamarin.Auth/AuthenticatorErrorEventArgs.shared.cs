using System;

namespace Xamarin.Auth
{
    public class AuthenticatorErrorEventArgs : EventArgs
    {
        public string Message { get; }

        public Exception Exception { get; }

        public AuthenticatorErrorEventArgs(string message)
        {
            Message = message;
        }

        public AuthenticatorErrorEventArgs(Exception exception)
        {
            Message = exception.GetInitialMessage();
            Exception = exception;
        }
    }
}
