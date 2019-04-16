#if ! AZURE_MOBILE_SERVICES
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
#else
using Xamarin.Auth._MobileServices;
using Xamarin.Auth._MobileServices.Presenters;
#endif

#if !AZURE_MOBILE_SERVICES
namespace Xamarin.Auth.Presenters.WinRT
#else
namespace Xamarin.Auth._MobileServices.Presenters.WinRT
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