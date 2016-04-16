using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
	public partial class MeetupOAuth2 : Xamarin.Auth.Helpers.OAuth2
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
			HowToMarkDown = 
			@"
			";
			Description = "Meetup OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = ""; // "", "basic", "email",
			OAuth_UriAuthorization = new Uri("https://secure.meetup.com/oauth2/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
			AllowCancel = true;
						
			return;
		}

	}
}

