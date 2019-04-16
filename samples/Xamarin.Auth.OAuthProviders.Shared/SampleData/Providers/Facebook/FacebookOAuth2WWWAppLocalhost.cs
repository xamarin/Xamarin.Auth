using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class FacebookOAuth2WWWAppLocalhost : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public FacebookOAuth2WWWAppLocalhost()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Facebook";
            OrderUI = "2";
            Description = "Facebook OAuth2 WWW App Type Callbackurl http[s]://localhost/";
            OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
            OAuth2_Scope = ""; // "", "basic", "email",
            OAuth_UriAuthorization = new Uri("https://m.facebook.com/dialog/oauth/");
            OAuth_UriCallbackAKARedirect = new Uri("http://localhost/");
            AllowCancel = true;
            HowToMarkDown =
@"
	https://developers.facebook.com/apps/
		Settings 
            Display Name = Xamarin.Auth.WWW.localhost
			Advanced
				https://developers.facebook.com/apps/<AppID>/settings/advanced/

                Client OAuth Login = true

                    Enables the standard OAuth client token flow. Secure your application 
                    and prevent abuse by locking down which token redirect URIs are allowed 
                    with the options below. Disable globally if not used.

                    Disable Client OAuth Login if your app does not use it. Client OAuth 
                    Login is the global on-off switch for using OAuth client token flows. 
                    If your app does not use any client OAuth flows, which include Facebook 
                    login SDKs, you should disable this flow. Note, though, that you can't 
                    request permissions for an access token if you have Client OAuth Login 
                    disabled. This setting is found in the 
                    Products > Facebook Login > Settings section.

                Web OAuth Login = true

                    Enables web based OAuth client login for building custom login flows.

                    Disable Web OAuth Flow or Specify a Redirect Whitelist. Web OAuth Login 
                    settings enables any OAuth client token flows that use the Facebook web 
                    login dialog to return tokens to your own website. This setting is in the 
                    Products > Facebook Login > Settings section. Disable this setting if you 
                    are not building a custom web login flow or using the Facebook Login SDK 
                    on the web.

                Force Web OAuth Reauthentication = false

                    When on, prompts people to enter their Facebook password in order to 
                    log in on the web.

                    When this setting is enabled you are required to specify a list of OAuth 
                    redirect URLs. Specify an exhaustive set of app URLs that are the only 
                    valid redirect URLs for your app for returning access tokens and codes 
                    from the OAuth flow.

                Embedded Browser OAuth Login = false
                    Enables browser control redirect uri for OAuth client login.

                    Disable embedded browser OAuth flow if your app does not use it. 
                    Some desktop and mobile native apps authenticate users by doing 
                    the OAuth client flow inside an embedded webview. 
                    If your app does not do this, then disable the setting in 
                    Products > Facebook Login > Settings section.

            Valid OAuth redirect URIs
                http://localhost/
                https://localhost/
			using URI not listed here will cause:
				Error:
				Given URL is not allowed by the Application configuration.: 
				One or more of the given URLs is not allowed by the App's settings. 
				It must match the Website URL or Canvas URL, or the domain must be a 
				subdomain of one of the App's domains.
			";

            return;
		}

	}
}

