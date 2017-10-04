# OAuth in Xamarin.Auth

## Definition

*	open authorization protocol 	
	[Jenkov]
*	delegation protocol 
	[]
	

### Roles

*	Resource Owner
*	Protected Resource on Resource Server
*	Client Application
*	Authorization Server

### Authorization Grant Types

*	Authorization Code 

	2 steps
	
	1.	Authorization [Enpoint] Request/Response
	
	2.	Token [Endpoint] Request/Response
	
	
*	Implicit

	1 step
	
*	Resource Owner Password Credentials

*	Client Credentials


### Endpoints

*	Authorization Endpoint 
*	Token Endpoint
*	Redirection Endpoint (`redirect_url`)

## Parameters comparison

### [Authorization Code Grant]

[Authorization Code Grant] https://tools.ietf.org/html/rfc6749#section-4.1

Notetion:

*	[R] Required
*	[O] Optional
*	[X]	eXtension 
1.	Authorization Request Comparison

| [RFC]                | [Google]     | [Facebook]   |
|:--------------------:|:------------:|:------------:|
| [R] `response_type`  |              |              |
| [R] `client_id`      |              |              |
| [O] `redirect_uri`   |              |              |
| [R] `response_type`  |              |              |

[RFC]: https://tools.ietf.org/html/rfc6749#section-4.1.1


## References

*	https://tools.ietf.org/html/draft-ietf-oauth-native-apps-10
*	http://wiki.oauth.net/w/page/27249271/OAuth%202%20for%20Native%20Apps
*	https://developers.googleblog.com/2016/08/modernizing-oauth-interactions-in-native-apps.html
*	https://www.iana.org/assignments/oauth-parameters/oauth-parameters.xhtml
*	https://www.oauth.com/
*	https://www.oauth.com/oauth2-servers/redirect-uris/redirect-uris-native-apps/
*	http://wiki.oauth.net/w/page/27249271/OAuth%202%20for%20Native%20Apps
*	tutorials

	*	https://aaronparecki.com/oauth-2-simplified/
	*	https://developer.chrome.com/extensions/tut_oauth
	*	http://tutorials.jenkov.com/oauth2/
	*	http://rasmustc.com/blog/Custom-Facebook-Authentication-with-webapi/

## Authorization Server


## Diverse 

https://github.com/IdentityModel/AuthorizationServer
https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection
https://github.com/IdentityModel/IdentityModel.AspNetCore.ScopeValidation
https://github.com/IdentityModel/IdentityModel.Owin.ClaimsTransformation

### Advanced reading (Standards/RFCs)

https://tools.ietf.org/html/rfc6749
https://tools.ietf.org/html/rfc6750
https://tools.ietf.org/html/rfc6819
https://tools.ietf.org/html/rfc7636
https://tools.ietf.org/html/draft-ietf-oauth-native-apps-10

### Libraries implementing OAuth 

https://oauth.net/code/

### Samples

*	http://ngiriraj.com/socialMedia/oauthlogin/
*	https://github.com/googlesamples/oauth-apps-for-windows

### Videos

### Server

https://www.youtube.com/watch?v=hnFW65ErJSY

## CustomTabs

https://developer.android.com/reference/android/support/customtabs/package-summary.html

https://github.com/GoogleChrome/custom-tabs-client
https://github.com/sushihangover/SushiHangover.GoogleChrome.CustomTabs.Shared
https://github.com/moljac/HolisticWare.Android.Support.CustomTabs.Chromium.SharedUtilities
https://labs.ribot.co.uk/exploring-chrome-customs-tabs-on-android-ef427effe2f4


https://github.com/SURFnet/nonweb-demo/wiki/Windows


## iOS Embedded WebView Implementations

http://nshipster.com/wkwebkit/

