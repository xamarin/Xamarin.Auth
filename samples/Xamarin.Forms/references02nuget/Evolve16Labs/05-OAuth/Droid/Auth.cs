using Android.Content;
using Android.OS;
using Xamarin.Auth.Presenters;

namespace Xamarin.Auth
{
	public static class Auth
	{
		internal static Context Context { get; set; }

		public static void Init(Context context, Bundle bundle)
		{
			Auth.Context = context;

			OAuthLoginPresenter.PlatformLogin = (authenticator) => {
				var oauthLogin = new PlatformOAuthLoginPresenter ();
				oauthLogin.Login (authenticator);
			};
		}
	}
}