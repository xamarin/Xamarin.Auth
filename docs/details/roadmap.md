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
	
	https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-protocols-oauth-code
	
	| Parameter name   |      Type             |  Description                           |
	|------------------|:---------------------:|---------------------------------------:|
	| `client_id`      |  required-standard    | app id assigned by registration portal |
	| `response_type`  |  required-standard    | `code` for authorization flow          |
	| `scope`          |  required-standard    | scope user consents                    |
	| `redirect_uri`   |  recommended-standard | endpoint for sending responses         |
	| `state`          |  recommended-standard | random string to prevent XSS forgery   |
	| `response_mode`  |  custom               |                                        |
	| `prompt`         |  optional             |                                        |
	| `login_hint`     |  optional             |                                        |
	| `domain_hint`    |  optional             |                                        |
	
		
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