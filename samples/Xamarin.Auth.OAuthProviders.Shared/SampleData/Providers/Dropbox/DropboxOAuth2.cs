using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class DropboxOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public DropboxOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Dropbox";
            OrderUI = "5";
            Description = "Dropbox OAuth2";
			HowToMarkDown = 
			@"
			";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "";
			OAuth_UriAuthorization = new Uri("https://www.dropbox.com/1/oauth2/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com/");
			AllowCancel = true;
						
			return;
		}

	}
}

