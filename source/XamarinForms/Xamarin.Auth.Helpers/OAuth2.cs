using System;

namespace Xamarin.Auth.Helpers
{
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

