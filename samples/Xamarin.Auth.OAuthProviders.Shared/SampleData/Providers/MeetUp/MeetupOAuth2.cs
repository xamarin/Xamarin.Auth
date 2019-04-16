using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class MeetupOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public MeetupOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "MeetUp";
            OrderUI = "4";
            Description = "MeetUp OAuth2";
			HowToMarkDown = 
			@"
			";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = ""; // "", "basic", "email",
			OAuth_UriAuthorization = new Uri("https://secure.meetup.com/oauth2/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

