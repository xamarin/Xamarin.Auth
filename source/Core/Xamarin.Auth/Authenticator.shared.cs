using System;
using System.Collections.Generic;

namespace Xamarin.Auth
{
    public abstract partial class Authenticator
    {
        public string Title { get; set; }

        public bool AllowCancel { get; set; }

        public bool ShowErrors { get; set; }

        public event EventHandler<AuthenticatorCompletedEventArgs> Completed;

        public event EventHandler<AuthenticatorErrorEventArgs> Error;

        public bool HasCompleted { get; private set; }

        protected bool IgnoreErrorsWhenCompleted { get; set; }

        public Authenticator()
        {
            Title = "Authenticate";
            HasCompleted = false;
            AllowCancel = true;
            ShowErrors = true;
            IgnoreErrorsWhenCompleted = false;
            IsAuthenticated = () => false;
        }

        public void OnSucceeded(Account account)
        {
            if (HasCompleted)
                return;

            HasCompleted = true;

            MainThread.BeginInvokeOnMainThread(() => Completed?.Invoke(this, new AuthenticatorCompletedEventArgs(account)));
        }

        public void OnSucceeded(string username, IDictionary<string, string> accountProperties)
        {
            OnSucceeded(new Account(username, accountProperties));
        }

        public void OnCancelled()
        {
            if (HasCompleted)
                return;

            HasCompleted = true;

            MainThread.BeginInvokeOnMainThread(() => Completed?.Invoke(this, new AuthenticatorCompletedEventArgs(null)));
        }

        public void OnError(string message)
        {
            if (HasCompleted && IgnoreErrorsWhenCompleted)
                return;

            MainThread.BeginInvokeOnMainThread(() => Error?.Invoke(this, new AuthenticatorErrorEventArgs(message)));
        }

        public void OnError(Exception exception)
        {
            if (HasCompleted && IgnoreErrorsWhenCompleted)
                return;

            MainThread.BeginInvokeOnMainThread(() => Error?.Invoke(this, new AuthenticatorErrorEventArgs(exception)));
        }

        public Func<bool> IsAuthenticated { get; set; }

        public Func<Account, AccountResult> GetAccountResult { get; set; }
    }
}
