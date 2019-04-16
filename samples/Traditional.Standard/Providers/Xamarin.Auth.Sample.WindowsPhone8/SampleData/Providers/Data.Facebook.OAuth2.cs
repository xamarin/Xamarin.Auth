using System;
using System.Threading.Tasks;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class Data 
	{
		static partial void SetPublicDemoDataFacebookOAuth2()
		{
			oauth2 = new Xamarin.Auth.Helpers.OAuth2()
			{
				Description = "Facebook OAuth2",
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "",
				OAuth2_Scope = "", // "", "basic", "email",
				OAuth_UriAuthorization = new Uri("https://m.facebook.com/dialog/oauth/"), 
				OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com"),
				AllowCancel = true,
			};

			if (TestCases.ContainsKey(oauth2.Description))
			{
				TestCases[oauth2.Description] = oauth2;
			}
			else
			{
				TestCases.Add(oauth2.Description, oauth2);
			}
						
			return;
		}

		public async Task<AuthenticationResult> DetailsFacebookOAuth2 (AuthenticatorCompletedEventArgs ee)
		{
			// Now that we're logged in, make a OAuth2 request to get the user's info.
			var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, ee.Account);
			Response response = await request.GetResponseAsync();
			var obj = System.Json.JsonValue.Parse (await response.GetResponseTextAsync());

			return new AuthenticationResult()
			{
				Title = "Logged in",
				User = obj["name"],
			};
		}
	}
}

