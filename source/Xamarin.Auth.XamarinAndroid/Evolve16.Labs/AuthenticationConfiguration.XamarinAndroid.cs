using Android.Content;
using Android.OS;

using Xamarin.Auth;
using Xamarin.Auth.Presenters.XamarinAndroid;

namespace Xamarin.Auth.Presenters.XamarinAndroid
{
    public static class AuthenticationConfiguration
    {
        internal static Context Context
        {
            get;
            set;
        }

        public static void Init(Context context, Bundle bundle)
        {
            AuthenticationConfiguration.Context = context;

            OAuthLoginPresenter.PlatformLogin = (authenticator) =>
            {
                PlatformOAuthLoginPresenter oauthLogin = new PlatformOAuthLoginPresenter();
                oauthLogin.Login(authenticator);
            };
        }
    }
}