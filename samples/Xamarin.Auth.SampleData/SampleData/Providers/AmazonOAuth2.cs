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
https://login.amazon.com/manageApps
https://login.amazon.com/app-console-login
https://sellercentral.amazon.com/gp/homepage.html

App ID 
	amzn1.application.3c0931e3458f4853a4565c23eea41e6f

Client ID
	amzn1.application-oa2-client.c722345968574fb49c83dbaff46482c5

	Error Summary
	400 Bad Request
	The redirect URI you provided has not been whitelisted for your application
	Request Details

Whitelisting in App Console +/ Web Settings +/ Allowed Return Urls

	https://xamarin.com/
	https://xamarin.com

NOTE: only https!
			";
			Description = "Amazon OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "profile"; // 
			OAuth_UriAuthorization = new Uri("https://www.amazon.com/ap/oa");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

