using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class LinkedInOAuth2 : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public LinkedInOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "LinkedIn";
            OrderUI = "4";
            Description = "LinkedIn OAuth2";
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
		NOTE: trailing s
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
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "r_basicprofile";
			OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com/");
			OAuth_UriAuthorization = 
						new Uri
							(
								"https://www.linkedin.com/uas/oauth2/authorization?response_type=code"
								+
								"&client_id=" + OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer
								+
								"&scope=" + Uri.EscapeDataString(OAuth2_Scope)
								+ 
								"&redirect_uri=" + Uri.EscapeDataString(OAuth_UriCallbackAKARedirect.ToString())
							);
			AllowCancel = true;

			return;
		}
	}
}

