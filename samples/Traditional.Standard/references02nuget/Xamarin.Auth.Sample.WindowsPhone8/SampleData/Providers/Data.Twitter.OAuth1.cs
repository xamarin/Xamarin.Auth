﻿using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class Data
	{
		static partial void SetPublicDemoDataTwitterOAuth1()
		{
			oauth1 = new Xamarin.Auth.Helpers.OAuth1()
			{
				Description = "Twitter OAuth1",
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "",
				OAuth1_SecretKey_ConsumerSecret_APISecret = "",
				OAuth1_UriRequestToken = new Uri("https://api.twitter.com/oauth/request_token"),
				OAuth_UriAuthorization = new Uri("https://api.twitter.com/oauth/authorize"), 
				OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com"),
				OAuth1_UriAccessToken = new Uri("https://api.twitter.com/oauth/access_token"),
				AllowCancel = true,
			};

			if (TestCases.ContainsKey(oauth1.Description))
			{
				TestCases[oauth1.Description] = oauth1;
			}
			else
			{
				TestCases.Add(oauth1.Description, oauth1);
			}

			return;
		}
	}
}

