using System;
#if !AZURE_MOBILE_SERVICES
using Xamarin.Auth;
#else
using Xamarin.Auth._MobileServices;
#endif

#if !AZURE_MOBILE_SERVICES
namespace Xamarin.Auth.Presenters
#else
namespace Xamarin.Auth._MobileServices.Presenters
#endif
{
#if XAMARIN_AUTH_INTERNAL
    internal class OAuthLoginPresenter
#else
    public class OAuthLoginPresenter
#endif
    {
        public event EventHandler<AuthenticatorCompletedEventArgs> Completed;

        public static Action<Authenticator> PlatformLogin;

        public void Login(Authenticator authenticator)
        {
            authenticator.Completed += OnAuthCompleted;

            PlatformLogin(authenticator);
        }

        void OnAuthCompleted(object sender, global::Xamarin.Auth.AuthenticatorCompletedEventArgs e)
        {
            if (Completed != null)
            {
                Completed(sender, e);
            }

            ((Authenticator)sender).Completed -= OnAuthCompleted;
        }
    }
}