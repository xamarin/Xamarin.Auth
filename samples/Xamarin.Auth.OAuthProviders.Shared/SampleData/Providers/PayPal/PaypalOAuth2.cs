using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class PaypalOAuth2 : Helpers.OAuth2
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
            ProviderName = "Paypal";
            OrderUI = "4";
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

