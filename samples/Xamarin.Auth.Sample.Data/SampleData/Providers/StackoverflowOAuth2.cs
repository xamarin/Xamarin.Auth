using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class StackoverflowOAuth2 : Xamarin.Auth.Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public StackoverflowOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
			HowToMarkDown = 
			@"
https://stackapps.com/users/login?returnurl=/apps/oauth/register

https://stackexchange.com/oauth

			";
			Description = "Stackoverflow OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "6378";
			OAuth2_Scope = ""; // "", "basic", "email",
			OAuth_UriAuthorization = new Uri("https://stackexchange.com/oauth");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

