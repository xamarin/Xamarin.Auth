﻿using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class GithubOAuth2 : Xamarin.Auth.Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public GithubOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
			HowToMarkDown = 
@"
https://github.com/settings/applications
https://developer.github.com/v3/oauth/#scopes
https://github.com/settings/developers
";
			Description = "Github OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = ""; // "", "user",
			OAuth_UriAuthorization = new Uri("https://github.com/login/oauth/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			AllowCancel = true;

			return;
		}
	}
}

