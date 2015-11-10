using System;

namespace Xamarin.Auth.Helpers
{
	/*
			WORKING!!
				Facebook
					OAuth2
						scope = "";				// email, basic, ...
						uri_authorize =  new Uri("https://m.facebook.com/dialog/oauth/");
						uri_callback_redirect = new Uri("http://xamarin.com");

				Twitter
					OAuth1
						  "DynVhdIjJDZSdQMb8KSLTA",
						  "REvU5dCUQI4MvjV6aWwUWVUqwObu3tvePIdQVBhNo",
						  new Uri("https://api.twitter.com/oauth/request_token"),
						  new Uri("https://api.twitter.com/oauth/authorize"),
						  new Uri("https://api.twitter.com/oauth/access_token"),
						  new Uri("http://twitter.com"

			LINKS / REFERENCES:

				https://forums.xamarin.com/discussion/3420/xamarin-auth-with-twitter
				https://forums.xamarin.com/discussion/16100/oauth-twitter-and-xamarin-article-authenticate-with-xamarin-auth
				http://visualstudiomagazine.com/articles/2014/04/01/using-oauth-twitter-and-async-to-display-data.aspx?m=2
				https://forums.xamarin.com/discussion/15869/xamarin-auth-twitter-authentication-process-failing
				https://forums.xamarin.com/discussion/4178/does-twitter-oauth-work-with-xamarin-auth
				http://www.codeproject.com/Tips/852742/Simple-Twitter-client-using-Xamarin-Forms-Xamarin

	*/
	public partial class OAuth2 : OAuth
	{
		public OAuth2 ()
		{
		}

		public string OAuth2_Scope
		{
			get;
			set;
		}

		public string OAuth1_SecretKey_ConsumerSecret_APISecret
		{
			get;
			set;
		}

		/*
		not available in OAuth2 ?!?!

		public Uri OAuth1_UriRequestToken
		{
			get;
			set;
		}
		*/

		public Uri OAuth1_UriAccessToken
		{
			get;
			set;
		}

		public virtual void Login ()
		{			
		}
	}
}

