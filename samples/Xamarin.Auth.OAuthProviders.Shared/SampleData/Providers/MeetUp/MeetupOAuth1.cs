using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class MeetupOAuth1 : Helpers.OAuth1
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public MeetupOAuth1()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "MeetUp";
            OrderUI = "4";
            Description = "MeetUp OAuth1";
			HowToMarkDown = 
			@"
			";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth1_SecretKey_ConsumerSecret_APISecret = "";
			OAuth1_UriRequestToken = new Uri("https://api.meetup.com/oauth/request");
			OAuth_UriAuthorization = new Uri("https://secure.meetup.com/authorize");
			OAuth_UriAccessToken_UriRequestToken = new Uri("https://api.meetup.com/oauth/access");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

