using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class MicrosoftLiveOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public MicrosoftLiveOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Microsoft";
            OrderUI = "2";
            Description = "Microsoft Live OAuth2";
			HowToMarkDown = 
@"
	https://msdn.microsoft.com/en-us/library/bb676633.aspx
	Live connect
		https://account.live.com/developers/applications/index
		
		https://account.live.com/developers/applications/summary/0000000040150A7D
		

	Basic Information
		Application name:
			Xamarin.Auth.Component
		Default language:
			English (United States)
		Application logo:
			Image of a 48 x 48 logo.
		Terms of service URL:
		Privacy URL:
	API Settings
		Mobile or desktop client app:
			No
		Restrict JWT issuing:
			Yes
		Enhanced redirection security:
			Enabled
		Target domain:
		Redirect URLs:
			http://xamarin.com/
	App Settings
		Client ID:
			0000000040150A7D
		Client secret:
			y3FsUfKKEvp4R3bOcfFikS9jhQghqNff
	Localization
		Language:
			English (United States)
		Application name:
			Xamarin.Auth.Component

	clientId: '<MyclientI>',
	scope: 'wl.basic, wl.signin, wl.offline_access',
	authorizeUrl: new Uri('https://login.live.com/oauth20_authorize.srf'),
	redirectUrl: new Uri('https://login.live.com/oauth20_desktop.srf'))
";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "wl.basic, wl.signin, wl.offline_access";
			OAuth_UriAuthorization = new Uri("https://login.live.com/oauth20_authorize.srf");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			AllowCancel = true;

			return;
		}
	}
}

