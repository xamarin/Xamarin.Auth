using System;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class StackoverflowOAuth2Implicit : Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public StackoverflowOAuth2Implicit()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
            ProviderName = "Stackoverflow";
            OrderUI = "4";
            Description = "Stackoverflow OAuth2 Implicit";
			HowToMarkDown = 
@"
Application management (dashboard)

http://stackapps.com/


https://api.stackexchange.com/docs/authentication

The explicit OAuth 2.0 flow consists of the following steps:

	Send a user to https://stackexchange.com/oauth, with these query string parameters
		client_id
		scope (details)
		redirect_uri - must be under an apps registered domain
		state - optional
	The user approves your app
	The user is redirected to redirect_uri, with these query string parameters
		code
		state - optional, only returned if provided in the first step
	POST (application/x-www-form-urlencoded) the following parameters to 
		https://stackexchange.com/oauth/access_token
		client_id
		client_secret
		code - from the previous step
		redirect_uri - must be the same as the provided in the first step
		This request is responded to with either an error (HTTP status code 400) 
		or an access token of the form access_token=...&expires=1234. expires will only be set if scope does not include no_expiry, the use of which is strongly advised against unless your app truly needs perpetual access.

The implicit OAuth 2.0 flow consists of the following steps:

	Open a new window at https://stackexchange.com/oauth/dialog, with these query string parameters
		client_id
		scope (details)
		redirect_uri - must be under an apps registered domain
		state - optional
	The user approves your app
	The user is redirected to redirect_uri, with these parameters in the hash
		access_token
		expires - optional, only if scope doesn't contain no_expiry
	The explicit flow should be used by server-side applications, with special care taken 
	to never leak client_secret. Client side applications should use the implicit flow.

https://api.stackexchange.com/docs/authentication#scope

With an empty scope, authentication will only allow an application to identify a user via the 
/me method. In order to access other information, different scope values must be sent. 
Multiple values may be sent in scope by comma or space delimitting them.

read_inbox - access a user's global inbox
no_expiry - access_token's with this scope do not expire
write_access - perform write operations as a user 2.1
private_info - access full history of a user's private actions on the site 2.1


";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = ""; // "", "read_inbox", "no_expiry", "write_access", ""
			OAuth_UriAuthorization = new Uri("https://stackexchange.com/oauth/dialog");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			AllowCancel = true;

			return;
		}

	}
}

