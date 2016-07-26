using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class DropboxOAuth2 : Xamarin.Auth.Helpers.OAuth2
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
			HowToMarkDown = 
			@"
			";
			Description = "Dropbox OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "";
			OAuth_UriAuthorization = new Uri("https://www.dropbox.com/1/oauth2/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com/");
			AllowCancel = true;
						
			return;
		}

	}
}

