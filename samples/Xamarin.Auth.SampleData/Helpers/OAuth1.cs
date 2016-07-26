using System;

namespace Xamarin.Auth.Helpers
{
	public partial class OAuth1 : OAuth
	{
		public OAuth1 ()
		{
		}

		/// <summary>
		///		OAuth1
		///			Twitter:	Consumer Secret / API Secret
		///			LinkedIn:	Secret key
		///			google:		
		///		
		/// </summary>
		public string OAuth1_SecretKey_ConsumerSecret_APISecret
		{
			get;
			set;
		}
	
		public Uri OAuth1_UriRequestToken
		{
			get;
			set;
		}


		public OAuth1Authenticator OAuthAuthenticator
		{
			get;
			set;
		}
	}
}

