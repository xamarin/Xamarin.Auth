using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class InstagramOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public InstagramOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Instagram";
            OrderUI = "4";
			Description = "Instagram OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "basic";
			OAuth_UriAuthorization = new Uri("https://api.instagram.com/oauth/authorize/");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			AllowCancel = true;
            HowToMarkDown =
@"
    https://instagram.com/developer/
    https://www.instagram.com/developer/clients/manage/
    Xamarin.Auth.Component
    CLIENT INFO
    CLIENT ID       
    WEBSITE URL     http://xamarin.com
    REDIRECT URI    http://xamarin.com/
    SUPPORT EMAIL   None
    Sample for Xamarin.Auth component

";

            return;
		}
	}
}

