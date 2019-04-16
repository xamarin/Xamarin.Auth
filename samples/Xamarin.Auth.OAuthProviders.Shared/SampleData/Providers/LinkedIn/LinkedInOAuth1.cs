using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class LinkedInOAuth1 : Helpers.OAuth1
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public LinkedInOAuth1()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "LinkedIn";
            OrderUI = "4";
            Description = "LinkedIn OAuth1";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth1_SecretKey_ConsumerSecret_APISecret = "";
			OAuth1_UriRequestToken = new Uri("https://api.linkedin.com/uas/oauth/requestToken");
			OAuth_UriAuthorization = new Uri("https://api.linkedin.com/uas/oauth/authorize");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			OAuth_UriAccessToken_UriRequestToken = new Uri("https://api.linkedin.com/uas/oauth/accessToken");
			AllowCancel = true;
			HowToMarkDown =
			@"
	https://developer.linkedin.com/
	https://www.linkedin.com/developer/apps/3679273/auth
	
	Authentication Keys
		Client ID:		
		Client Secret:	
	Default Application Permissions
		r_basicprofile	r_contactinfo	r_emailaddress
		r_fullprofile	r_network	rw_company_admin
		rw_groups	rw_nus	w_messages
		w_share

	OAuth 2.0
		Authorized Redirect URLs: 
			https://xamarin.com 		
			http://xamarin.com 		
			https://xamarin.com/
			http://xamarin.com/

	OAuth 1.0a
		Default 'Accept' Redirect URL:
			http://xamarin.com
		Default 'Cancel' Redirect URL:
			http://xamarin.com
";
			return;
		}
	}
}

