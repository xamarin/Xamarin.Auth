using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class FitbitOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public FitbitOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Fitbit";
            OrderUI = "5";
            Description = "Fitbit OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "profile"; // 
			OAuth_UriAuthorization = new Uri("https://www.fitbit.com/oauth2/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("xamarin-auth://localhost");
			AllowCancel = true;
			HowToMarkDown = 
			@"
https://dev.fitbit.com/eu
https://dev.fitbit.com/apps

    App Name                App Type        Default Access Type
    Xamarin.Auth Tests      Browser         Read-Only
    Xamarin.Auth Tests 

OAuth 2.0 Client ID                             <6-alhanums>
OAuth 2.0: Authorization URI                    https://www.fitbit.com/oauth2/authorize
OAuth 2.0: Access/Refresh Token Request URI     https://api.fitbit.com/oauth2/token

			";
						
			return;
		}

	}
}

