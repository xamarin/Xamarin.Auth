using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class Data 
	{
		static partial void SetPublicDemoDataGoogleOAuth2()
		{
			// https://www.snip2code.com/Snippet/245686/Xamarin-Google-and-Facebook-authenticati
			/*
				clientId: "123456789.apps.googleusercontent.com", 
				scope: "https://www.googleapis.com/auth/userinfo.email", 
				authorizeUrl: new Uri ("https://accounts.google.com/o/oauth2/auth"),
				redirectUrl: new Uri ("http://bunchy.com/oauth2callback"), 
				getUsernameAsync: null)
			*/
			oauth2 = new Xamarin.Auth.Helpers.OAuth2()
			{
				Description = "Google OAuth2",
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "",
				OAuth2_Scope = "https://www.googleapis.com/auth/userinfo.email",
				OAuth_UriAuthorization = new Uri("https://accounts.google.com/o/oauth2/auth"), 
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

