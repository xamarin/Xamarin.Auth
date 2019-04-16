using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class Data 
	{
		static partial void SetPublicDemoDataMicrosoftLiveOAuth2()
		{
			oauth2 = new Xamarin.Auth.Helpers.OAuth2()
			{
				Description = "Microsoft Live OAuth2",
				/*
				clientId: "<MyclientI>",
				scope: "wl.basic, wl.signin, wl.offline_access",
				authorizeUrl: new Uri(""https://login.live.com/oauth20_authorize.srf"),
				redirectUrl: new Uri("https://login.live.com/oauth20_desktop.srf"))
				*/
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "",
				OAuth2_Scope = "wl.basic, wl.signin, wl.offline_access",
				OAuth_UriAuthorization = new Uri("https://login.live.com/oauth20_authorize.srf"), 
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

