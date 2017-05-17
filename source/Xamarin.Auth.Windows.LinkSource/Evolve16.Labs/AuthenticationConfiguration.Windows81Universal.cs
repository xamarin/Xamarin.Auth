using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace Xamarin.Auth.Presenters.WinRT
{
    public static class AuthenticationConfiguration
    {
        public static void Init()
        {
            OAuthLoginPresenter.PlatformLogin = (authenticator) =>
            {
                var oauthLogin = new PlatformOAuthLoginPresenter();
                oauthLogin.Login(authenticator);
            };
        }
    }
}