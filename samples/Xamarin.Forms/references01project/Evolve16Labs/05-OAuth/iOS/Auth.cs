using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace Xamarin.Auth
{
	public static class Auth
	{
		public static void Init()
		{
			OAuthLoginPresenter.PlatformLogin = (authenticator) => {
				var oauthLogin = new PlatformOAuthLoginPresenter ();
				oauthLogin.Login (authenticator);
			};
		}
	}
}