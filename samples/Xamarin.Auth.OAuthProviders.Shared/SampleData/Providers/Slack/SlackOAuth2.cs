using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class SlackOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public SlackOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Slack";
            OrderUI = "3";
            Description = "Slack OAuth2";
			HowToMarkDown = 
			@"
https://api.slack.com/docs/oauth


https://slack.com/oauth/authorize

https://api.slack.com/docs/oauth-scopes

			";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "users:read"; // 
			OAuth_UriAuthorization = new Uri("https://slack.com/oauth/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

