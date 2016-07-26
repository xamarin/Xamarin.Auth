using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class SlackOAuth2 : Xamarin.Auth.Helpers.OAuth2
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
			HowToMarkDown = 
			@"
https://api.slack.com/docs/oauth


https://slack.com/oauth/authorize

https://api.slack.com/docs/oauth-scopes

			";
			Description = "Slack OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "users:read"; // 
			OAuth_UriAuthorization = new Uri("https://slack.com/oauth/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

