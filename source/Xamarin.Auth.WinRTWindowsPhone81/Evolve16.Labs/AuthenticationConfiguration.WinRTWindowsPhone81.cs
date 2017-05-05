using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace Xamarin.Auth.Presenters.WinRTWindowsPhone81
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