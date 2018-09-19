using System;

namespace Xamarin.Auth
{
    public class AuthenticatorCompletedEventArgs : EventArgs
    {
        public bool IsAuthenticated => Account != null;

        public Account Account { get; }

        public AuthenticatorCompletedEventArgs(Account account)
        {
            Account = account;
        }
    }
}
