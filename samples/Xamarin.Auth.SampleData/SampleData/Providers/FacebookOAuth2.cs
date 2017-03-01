using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class FacebookOAuth2 : Xamarin.Auth.Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public FacebookOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
			HowToMarkDown = 
@"
	https://developers.facebook.com/apps/
		Settings 
			Advanced
				https://developers.facebook.com/apps/523432231128758/settings/advanced/
			Valid OAuth redirect URIs
				https://xamarin.com
				https://www.xamarin.com
			using URI not listed here will cause:
				Error:
				Given URL is not allowed by the Application configuration.: 
				One or more of the given URLs is not allowed by the App's settings. 
				It must match the Website URL or Canvas URL, or the domain must be a 
				subdomain of one of the App's domains.
			";
			Description = "Facebook OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = ""; // "", "basic", "email",
			OAuth_UriAuthorization = new Uri("https://m.facebook.com/dialog/oauth/");
			OAuth_UriCallbackAKARedirect = new Uri("http://localhost/");
			AllowCancel = true;
						
			return;
		}

	}
}

