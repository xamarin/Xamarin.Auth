# Xamarin.Auth Roadmap 

## Upcomming Changes

### v.1.6.0.0

*   .NET Standard support (1.0 and 1.6)

*   Azure Mobile Services Client changes

*   iOS check for SafariViewController and WKWebView availability

*   two properties to allow a developer to specify when to call OnPageLoading

*   conditional compiles XAMARIN_AUTH_INTERNAL

*   AZURE_MOBILE_SERVICES preprocessor define for namespaces

*   CustomTabs closing


This doc is subject to discussion (personal roadmap based on user feedback):

## 1 OAuth features (`refresh_token`, other grant flow implementations)

###	1.1	Custom Parameters		

	
https://stackoverflow.com/questions/40787981/c-sharp-xamarin-ios-add-a-nonce-to-xamarin-auth-request/44792934#44792934

The user is talking about `nonce` parameter, but there is need for more general API
for custom parameters.

User suggests:
	
```
auth.RequestParameters.Add("nonce", Guid.NewGuid().ToString("N"));
```

But the problem is that request to the authorisation endpoint is launched during
construction of the object, so adding `RequestParameters` after `auth` object 
construction will not affect parameters. (`IsUsingNativeUI` propert setter was removed
because setter confused users which tried to set Native UI in initializer);
	
Proposed solution - is to add another ctor parameter `IDictionary<string, string>)` 
which would add parameters to the query.
		
TODO -  more links
	
#### Some provider samples:

##### **Google**
	
https://developers.google.com/identity/protocols/OAuth2InstalledApp

Authorization Code leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `redirect_uri`           |  required-standard    | endpoint for sending responses                 |
| `response_type`          |  required-standard    | `code` for authorization flow installed apps   |
| `scope`                  |  required-standard    | scope user consents                            |
| `state`                  |  recommended-standard | random string to prevent XSS forgery           |
| `code_challenge_method`  |  optional-custom      |                                                |
| `code_challenge`         |  optional-custom      |                                                |
| `login_hint`             |  optional-custom      |                                                |


TODO: `redirect_url` reqired by Google and recommended by Azure Active Directory ?????

Token leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `code`                   |  recommended-standard | endpoint for sending responses                 |
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `client_secret`          |  standard             | required for web apps, not for native          |
| `redirect_uri`           |  recommended-standard | random string to prevent XSS forgery           |
| `grant_type`             |  required-standard    | `authorization_code`                           |
| `code_verifier`          |  required-standard    | scope user consents                            |

TODO: ??? no `scope` parameter??

Refresh Token leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `refresh_token`          |  required-standard    | `refresh_token` obtained in Token 2nd leg/step |
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `client_secret`          |  standard             | required for web apps, not for native          |
| `grant_type`             |  required-standard    | `authorization_code` for authorization flow    |


TODO: 

*	no `redirect_url` and `scope` ???

##### **Google OpenId**
	
https://developers.google.com/identity/protocols/OpenIDConnect
	
Authorization Code leg/step:
	
| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `response_type`          |  required-standard    | `code` for authorization flow installed apps   |
| `scope`                  |  required-standard    | scope user consents                            |
| `redirect_uri`           |  required-standard    | endpoint for sending responses                 |
| `state`                  |  recommended-standard | random string to prevent XSS forgery           |
| `prompt`                 |  optional-custom      |                                                |
| `display`                |  optional-custom      |                                                |
| `login_hint`             |  optional-custom      |                                                |
| `access_type`            |  optional-custom      |                                                |
| `include_granted_scopes` |  optional-custom      |                                                |
| `openid.realm`           |  optional-custom      |                                                |
| `hd`                     |  optional-custom      |                                                |
	
Token leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `code`                   |  recommended-standard | endpoint for sending responses                 |
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `client_secret`          |  standard             | required for web apps, not for native          |
| `redirect_uri`           |  recommended-standard | random string to prevent XSS forgery           |
| `grant_type`             |  required-standard    | `authorization_code`                           |
	
	
##### **Azure Active Directory**
	
https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-protocols-oauth-code
	
Authorization Code leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `response_type`          |  required-standard    | `code` for authorization flow                  |
| `scope`                  |  required-standard    | scope user consents                            |
| `redirect_uri`           |  recommended-standard | endpoint for sending responses                 |
| `state`                  |  recommended-standard | random string to prevent XSS forgery           |
| `response_mode`          |  optional-custom      |                                                |
| `prompt`                 |  optional-custom      |                                                |
| `login_hint`             |  optional-custom      |                                                |
| `domain_hint`            |  optional-custom      |                                                |
		
Token leg/step:

| Parameter name           |      Type             |  Description                                           |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `grant_type`             |  required-standard    | `refresh_token`                                |
| `scope`                  |  required-standard    | scope user consents                            |
| `code`                   |  recommended-standard | endpoint for sending responses                 |
| `redirect_uri`           |  recommended-standard | random string to prevent XSS forgery           |
| `client_secret`          |  standard             | required for web apps, not for native          |

TODO:
 
*	no `state` check??
	
Refresh Token leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `grant_type`             |  required-standard    | `authorization_code` for authorization flow    |
| `scope`                  |  required-standard    | scope user consents                            |
| `refresh_token`          |  required-standard    | `refresh_token` obtained in Token 2nd leg/step |
| `redirect_uri`           |  recommended-standard | random string to prevent XSS forgery           |
| `client_secret`          |  standard             | required for web apps, not for native          |
	
##### **Azure Active Directory B2C**

https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-reference-oauth-code
https://github.com/Azure-Samples/active-directory-b2c-xamarin-native
	
Authorization Code leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `response_type`          |  required-standard    | `code` for authorization flow                  |
| `redirect_uri`           |  recommended-standard | endpoint for sending responses                 |
| `scope`                  |  required-standard    | scope user consents                            |
| `response_mode`          |  optional-custom      |                                                |
| `state`                  |  recommended-standard | random string to prevent XSS forgery           |
| `p`                      |  required             |                                                |
| `prompt`                 |  optional-custom      |                                                |
	
	
Token leg/step:

| Parameter name           |      Type             |  Description                                           |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  required-standard    | app id assigned by registration portal         |
| `grant_type`             |  required-standard    | `refresh_token`                                |
| `scope`                  |  recommended          | scope user consents                            |
| `code`                   |  required-standard    | endpoint for sending responses                 |
| `redirect_uri`           |  required-standard    | random string to prevent XSS forgery           |
| `p`                      |  required             | required for web apps, not for native          |
	
TODO: 

*	`client_secret` not required? 
*	`p` required?

Refresh Token leg/step:

| Parameter name           |      Type             |  Description                                   |
|--------------------------|-----------------------|------------------------------------------------|
| `client_id`              |  recommended-standard | app id assigned by registration portal         |
| `grant_type`             |  required-standard    | `authorization_code` for authorization flow    |
| `scope`                  |  recommended-standard | scope user consents                            |
| `redirect_uri`           |  optional-standard    | random string to prevent XSS forgery           |
| `refresh_token`          |  required-standard    | `refresh_token` obtained in Token 2nd leg/step |
| `p`                      |  required             | required for web apps, not for native          |

NOTE: 

*	`redirect_url` recommended??


###	1.2	`refresh_token`
	
Currently in Xamarin.Auth.Extensions based on University team suggestion that refreshing 
tokens in implicit flow is not recommended.		

##	2 Building for Azure Mobile Services Client with XAMARIN_AUTH_INTERNAL

Cake scripts must be updated to compile Xamarin.Auth with and without definition
for XAMARIN_AUTH_INTERNAL which is used by Azure Mobile Service Client.


## 3 Security Issues

### 3.1	Logout Token Revocation

https://tools.ietf.org/html/rfc7009

https://developers.google.com/identity/protocols/OAuth2WebServer#tokenrevoke

To programmatically revoke a token, your application makes a request to 
https://accounts.google.com/o/oauth2/revoke and includes the token as a parameter:

	curl -H "Content-type:application/x-www-form-urlencoded" \
			https://accounts.google.com/o/oauth2/revoke?token={token}

The token can be an access token or a refresh token. If the token is an access token 
and it has a corresponding refresh token, the refresh token will also be revoked.

If the revocation is successfully processed, then the status code of the response is 200. 
For error conditions, a status code 400 is returned along with an error code.
	
	
## 4 Documentation

###	4.1	Xamarin.Forms support 
	
###	4.2	Native UI Details		
	
## 5 Oauth and Identity (OpenId) Service provider samples

### OAuth

RequestParameters

### OpenId

1.	Google Identity Provider

	https://developers.google.com/identity/protocols/OpenIDConnect#authenticatingtheuser
	
	JSON Discovery Metadata
	
	https://accounts.google.com/.well-known/openid-configuration
	

2.	Microsoft 

	https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-openid-connect-code
	
	JSON Discovery Metadata
	
	https://login.microsoftonline.com/fabrikamb2c.onmicrosoft.com/v2.0/.well-known/openid-configuration
	
3.	Yahoo

	https://developer.yahoo.com/oauth2/guide/openid_connect/
	
	JSON Discovery Metadata
	
	https://login.yahoo.com/.well-known/openid-configuration	

4.	PayPal

	https://developer.paypal.com/docs/integration/direct/identity/log-in-with-paypal/

	JSON Discovery Metadata
	
	https://www.paypalobjects.com/.well-known/openid-configuration
	
5.	Amazon

	https://images-na.ssl-images-amazon.com/images/G/01/lwa/dev/docs/website-developer-guide._TTH_.pdf
	
6.	SalesForce

	https://login.salesforce.com/
	
	JSON Discovery Metadata
	
	https://login.salesforce.com/.well-known/openid-configuration
	
7.	IdentityServer

	https://demo.identityserver.io/
	
	JSON Discovery Metadata
	
	https://demo.identityserver.io/.well-known/openid-configuration
	
More links

http://openid.net/developers/specs/
https://stackoverflow.com/questions/22501565/list-of-openid-connect-providers
https://stackoverflow.com/questions/1116743/where-can-i-find-a-list-of-openid-provider-urls
http://www.digitalenginesoftware.com/blog/archives/24-OpenID-Provider-URL-Formatting.html
http://web.archive.org/web/20091230182837/http://spreadopenid.org/provider-comparison

	
## 6 Custom Tabs API refactoring and improvements	
	
## 7 Component packaging		

## 8 .NET Standard

9.	Code clean up based on coverage
