using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class MeetupOAuth1 : Xamarin.Auth.Helpers.OAuth1
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
			HowToMarkDown = 
			@"
			";
			Description = "Meetup OAuth1";
			Description = "Meetup OAuth1";
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

