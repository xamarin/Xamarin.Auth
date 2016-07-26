﻿using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class TwitterOAuth1 : Xamarin.Auth.Helpers.OAuth1
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public TwitterOAuth1()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
			HowToMarkDown = 
@"
	https://dev.twitter.com/
	https://apps.twitter.com/

	Xamarin.Auth component tests
	http://components.xamarin.com/view/xamarin.auth
	
	Application Settings
		Your application's Consumer Key and Secret are used to authenticate 
		requests to the Twitter Platform.
		Access level				Read and write (modify app permissions)
		Consumer Key (API Key)		etd7kEcbqOGnCYKb9D1zoqqr7 
		Callback URL				http://components.xamarin.com/view/xamarin.auth
		Callback URL Locked			No
		Sign in with Twitter		Yes
		App-only authentication		https://api.twitter.com/oauth2/token
		Request token URL			https://api.twitter.com/oauth/request_token
		Authorize URL				https://api.twitter.com/oauth/authorize
		Access token URL			https://api.twitter.com/oauth/access_token
";
			Description = "Twitter OAuth1";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth1_SecretKey_ConsumerSecret_APISecret = "";
			OAuth1_UriRequestToken = new Uri("https://api.twitter.com/oauth/request_token");
			OAuth_UriAuthorization = new Uri("https://api.twitter.com/oauth/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			OAuth_UriAccessToken = new Uri("https://api.twitter.com/oauth/access_token");
			AllowCancel = true;

			return;
		}
	}
}

