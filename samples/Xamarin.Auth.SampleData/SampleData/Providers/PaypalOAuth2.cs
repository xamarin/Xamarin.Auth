using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class PaypalOAuth2 : Xamarin.Auth.Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public PaypalOAuth2()
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
			Description = "Paypal OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "openid"; // "", "basic", "email",
			OAuth_UriAuthorization = new Uri("https://www.paypal.com/webapps/auth/protocol/openidconnect/v1/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

