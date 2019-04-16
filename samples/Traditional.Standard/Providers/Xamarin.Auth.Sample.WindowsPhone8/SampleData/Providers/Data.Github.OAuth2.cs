using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class Data 
	{
		static partial void SetPublicDemoDataGithubOAuth2()
		{
			/// https://developer.github.com/v3/oauth/#scopes
			oauth2 = new Xamarin.Auth.Helpers.OAuth2()
			{
				Description = "Github OAuth2",
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "",
				OAuth2_Scope = "", // "", "user",
				OAuth_UriAuthorization = new Uri("https://github.com/login/oauth/authorize"), 
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
	}
}

