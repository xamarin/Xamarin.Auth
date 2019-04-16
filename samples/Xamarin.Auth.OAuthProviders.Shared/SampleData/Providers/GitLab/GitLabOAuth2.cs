using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class GitLabOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public GitLabOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Github";
            OrderUI = "4";
			HowToMarkDown =
            @"
http://doc.gitlab.com/ce/integration/oauth_provider.html

https://gitlab.com/oauth/authorize

			";
			Description = "Gitlab OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "api"; // 
			OAuth_UriAuthorization = new Uri("https://gitlab.com/oauth/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

