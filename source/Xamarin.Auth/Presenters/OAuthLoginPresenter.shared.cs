using System;

namespace Xamarin.Auth.Presenters
{
    public partial class OAuthLoginPresenter
    {
        public event EventHandler<AuthenticatorCompletedEventArgs> Completed;

        [Obsolete]
        public static Action<Authenticator> PlatformLogin;

        public void Login(Authenticator authenticator)
        {
            if (authenticator == null)
                throw new ArgumentNullException(nameof(authenticator));

            authenticator.Completed += OnAuthCompleted;

#pragma warning disable CS0612 // Type or member is obsolete
            // legacy event - just in case it is being used somewhere in the world
            PlatformLogin?.Invoke(authenticator);
#pragma warning restore CS0612 // Type or member is obsolete

#if !__NETSTANDARD__
            PlatformLoginImplementation(authenticator);
#endif
        }

        private void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            ((Authenticator)sender).Completed -= OnAuthCompleted;

            Completed?.Invoke(sender, e);
        }
    }
}
