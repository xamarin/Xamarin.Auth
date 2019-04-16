using Xamarin.Auth;
using Xamarin.Auth.Presenters;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth.Presenters.XamarinIOS
#else
namespace Xamarin.Auth._MobileServices.Presenters.XamarinIOS
#endif
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