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

		public string OAuth_SecretKey_ConsumerSecret_APISecret
		{
			get;
			set;
		}

		public virtual void Login ()
		{			
		}

		public OAuth2Authenticator OAuthAuthenticator
		{
			get;
			set;
		}
	}
}

