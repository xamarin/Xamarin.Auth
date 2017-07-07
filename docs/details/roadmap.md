# Xamarin.Auth Roadmap 

This doc is subject to discussion (personal roadmap based on user feedback):

1.	OAuth features (`refresh_token`, other grant flow implementations)

	1.	Custom Parameters		
	
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
	
	Some provider samples:

	*Google*
	
	https://developers.google.com/identity/protocols/OAuth2InstalledApp#request-parameter-redirect_uri
	
	Authorization Code leg/step:
	
	| Parameter name           |      Type             |  Description                           |
	|--------------------------|-----------------------|------------------------------------------------|
	| `client_id`              |  required-standard    | app id assigned by registration portal |
	| `redirect_uri`           |  required-standard    | endpoint for sending responses         |
	| `response_type`          |  required-standard    | `code` for authorization flow installed apps   |
	| `scope`                  |  required-standard    | scope user consents                    |
	| `state`                  |  recommended-standard | random string to prevent XSS forgery   |
	| `code_challenge_method`  |  custom               |                                        |
	| `code_challenge`         |  optional             |                                        |
	| `login_hint`     |  optional             |                                        |

	
	TODO: `redirect_url` reqired by Google and recommended by Azure Active Directory ?????

	Token leg/step:
	
	| Parameter name   |      Type             |  Description                                |
	|--------------------------|-----------------------|------------------------------------------------|
	| `code`           |  recommended-standard | endpoint for sending responses         |
	| `client_id`      |  required-standard    | app id assigned by registration portal      |
	| `client_secret`  |  standard             | required for web apps, not for native       |
	| `redirect_uri`   |  recommended-standard | random string to prevent XSS forgery   |
	| `grant_type`     |  required-standard    | `authorization_code`                             |
	| `code_verifier`  |  required-standard    | scope user consents                    |

	TODO: ??? no `scope` parameter??

	Refresh Token leg/step:
	
	| Parameter name   |      Type             |  Description                                |
	|--------------------------|-----------------------|------------------------------------------------|
	| `refresh_token`  |  required-standard    | `refresh_token` obtained in Token 2nd leg/step |
	| `client_id`      |  required-standard    | app id assigned by registration portal         |
	| `client_secret`  |  standard             | required for web apps, not for native       |
	| `grant_type`     |  required-standard    | `authorization_code` for authorization flow    |

	TODO: ??? no `redirect_url` and `scope` ???
	
	*Azure Active Directory*
	
	https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-protocols-oauth-code
	
	Authorization Code leg/step:
	
	| Parameter name   |      Type             |  Description                           |
	|------------------|:----------------------|------------------------------------------------|
	| `client_id`      |  required-standard    | app id assigned by registration portal |
	| `response_type`  |  required-standard    | `code` for authorization flow          |
	| `scope`          |  required-standard    | scope user consents                    |
	| `redirect_uri`   |  recommended-standard | endpoint for sending responses         |
	| `state`          |  recommended-standard | random string to prevent XSS forgery   |
	| `response_mode`  |  custom               |                                        |
	| `prompt`         |  optional             |                                        |
	| `login_hint`     |  optional             |                                        |
	| `domain_hint`    |  optional             |                                        |
	
		
	Token leg/step:
	
	| Parameter name   |      Type             |  Description                                |
	|--------------------------|-----------------------|------------------------------------------------|
	| `client_id`      |  required-standard    | app id assigned by registration portal      |
	| `grant_type`     |  required-standard    | `refresh_token`                             |
	| `scope`          |  required-standard    | scope user consents                    |
	| `code`           |  recommended-standard | endpoint for sending responses         |
	| `redirect_uri`   |  recommended-standard | random string to prevent XSS forgery   |
	| `client_secret`  |  standard             | required for web apps, not for native       |

	TODO: state?? check??
	
	Refresh Token leg/step:
	
	| Parameter name   |      Type             |  Description                                |
	|--------------------------|-----------------------|------------------------------------------------|
	| `client_id`      |  required-standard    | app id assigned by registration portal         |
	| `grant_type`     |  required-standard    | `authorization_code` for authorization flow    |
	| `scope`          |  required-standard    | scope user consents                            |
	| `refresh_token`  |  required-standard    | `refresh_token` obtained in Token 2nd leg/step |
	| `redirect_uri`   |  recommended-standard | random string to prevent XSS forgery   |
	| `client_secret`  |  standard             | required for web apps, not for native       |
	
	*Azure Active Directory B2C*

	https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-reference-oauth-code
	https://github.com/Azure-Samples/active-directory-b2c-xamarin-native
	
	
	
	
	2.	`refresh_token`
	
		currently in Xamarin.Auth.Extensions based on University team suggestion:
		

2.	Building for Azure Mobile Services Client with XAMARIN_AUTH_INTERNAL

	Cake scripts must be updated to compile Xamarin.Auth with and without definition
	for XAMARIN_AUTH_INTERNAL which is used by Azure Mobile Service Client.

3.	Security Issues

	1.	Logout
	
4.	Documentation

	1.	Xamarin.Forms support 
	
	2.	Native UI Details		
	
5.	Identity (OAuth) Service provider samples

6.	Custom Tabs API refactoring and improvements	
	
7.	Component packaging		

8.	.NET Standard

9.	Code clean up based on coverage