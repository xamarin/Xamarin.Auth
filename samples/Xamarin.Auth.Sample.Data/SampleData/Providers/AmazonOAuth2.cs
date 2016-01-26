using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class AmazonOAuth2 : Xamarin.Auth.Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public AmazonOAuth2()
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
			Description = "Amazon OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = ""; // "", "basic", "email",
			OAuth_UriAuthorization = new Uri("https://m.facebook.com/dialog/oauth/");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

