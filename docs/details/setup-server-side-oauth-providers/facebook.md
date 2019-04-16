# Facebook Setup

WORK IN PROGRESS - CONSTRUCTION_SITE/BAUSTELLE

## `redirect_url`

```csharp
	string redirect_url = $"fb{fb_app_id}://authorize";
```

The URL can be found in Facebook SDK implementations

*	Android

	https://github.com/facebook/facebook-android-sdk/blob/45482361eb182bb3392386182f8f4e6c4896c2b9/facebook-common/src/main/java/com/facebook/CustomTabMainActivity.java#L45

*	iOS

	https://github.com/facebook/facebook-ios-sdk/blob/f7531a838a1ec3308721f335e801ac9f508feee4/FBSDKLoginKit/FBSDKLoginKitTests/FBSDKLoginManagerTests.m#L51

	https://github.com/facebook/facebook-ios-sdk/blob/4b4bd9504d70d99d6c6b1ca670f486ac8f494f17/FBSDKLoginKit/FBSDKLoginKit/Internal/FBSDKLoginUtility.m#L43-L47




## Console - Server Side Setup 

### Setup 1

1.	Dasboard
	1.	API version = 2.9
2.	Settings / Basic
	1.	Display Name = Xamarin.Auth.Native.ComicBk
	2.	App Domains = xamarin.com
		most likeky irrelevant
	3.	Website (Platform)	
		Site url = https://xamarin.com (edited)
	4.	iOS (Platform)	
		Bundle ID = com.xamarin.comicbook.ios
		Single Sign On = yes
		Deep Linking = yes
	5.	Android
		Google Play Package Name = com.xamarin.comicbook.android
		Class Name = com.xamarin.comicbook.ActivityCustomUrlSchemeInterceptor
		Key Hashes = …
		Single Sign On = yes
		Deep Linking = yes 
3.	Settings Advanced
	1.	Native or desktop app? = yes
	2.	Is App Secret embedded in the client? = No
4.	Facebook Login	
	1.	Settings - Client OAuth Settings
		1.	Client OAuth Login =  yes
		2.	Web OAuth Login =  yes
		3.	Embedded Browser OAuth Login = yes
		4.	Valid OAuth redirect URIs = http://localhost/somepath
			most likely - not relevant
		5.	Login from Devices  = no

### Setup 2

*	Client OAuth Settings

	*	Client OAuth Login = yes	
	
	*	Web OAuth Login = no	
	
	*	Embedded Browser OAuth Login = yes	
	
	*	Force Web OAuth Reauthentication = no	
	
	*	Login from Devices = no	
		
		
## Errors

### Not Logged In

Error

	Not Logged In: You are not logged in. Please log in and try again.
	
with:
	
```
Url =  https://m.facebook.com/dialog/oauth/
```

### Can't Load Url

Error

	Can't Load Url: The domain of this URL isn't included in the app's domains.
	To be able to load this URL, add all domains and subdomains of your app to
	the App Domain field in your app settings. 

with:

```
	url fb1889013594699403://localhost/path
	url fb1889013594699403://xamarin.com
```

https://stackoverflow.com/questions/37063685/facebook-oauth-the-domain-of-this-url-isnt-included-in-the-apps-domain
https://stackoverflow.com/questions/37652221/facebook-login-cant-load-url-the-domain-of-this-url-isnt-included-in-the-app

### App domains must match the domain		

Error 

	App Domains add localhost

    App domains must match the domain of the Facebook Web Games URL (https), Mobile Site URL, 
    Unity Binary URL, Site URL or Secure Page Tab URL. Please correct these domains: 
    localhost        

	
### cannot add custom scheme URL

Error
	
	web app

	cannot add custom scheme URL

### Given URL is not allowed by the Application configuration

Error

	Given URL is not allowed by the Application configuration.: One or more of the given URLs is 
	not allowed by the App's settings. It must match the Website URL or Canvas URL, or the domain 
	must be a subdomain of one of the App's domains.


	
	
## References


https://www.facebook.com/groups/fbdevelopers/permalink/1421835504526626/
https://www.facebook.com/help/community/search/?query=oauth


## Letter for Facebok Developers' Group


Xamarin.Auth is a cross platform library for authentication purposes with OAuth for Xamarin 
and Windows platforms. It does not use native OAuth providers' SDKs like Facebook SDK, but
plain http requests and responses.

Recently Xamarin.Auth was extended with implementation for Native UI support for Installed
apps (on Android CustomTabs and on iOS SFSafariViewController). Users can easily switch between
Embedded WebView (Browser control) and Native UI

In order to use Native UI Deep/App Linking must be implemented by the app to enable callbacks 
into the app by using custom scheme[s].

Google Server side OAuth generates custom schemes in 2 different ways:

1. based on Android's package name or iOS Bundle Id. (package.name:/oauth2redirect)
2. based on App Id (com.googleusercontent.apps.<appid>:/oauth2redirect)

So, basically it is URL without authority. Custom scheme is given by google and path is arbitrary.


So questions: 

1.	From Manually Build a Login Flow

	https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow

	> However, if you need to implement browser-based login for an app without using 
	> our SDKs, such as in a webview for a native desktop app (for example Windows 8), 
	> or a login flow using entirely server-side code, you can build a Login flow for 
	> yourself by using browser redirects
	
	This is what Xamarin.Auth does - uses browser redirects. Here native desktop apps
	are mentioned but only for Windows 8 (later in the docs  WebAuthenticationBroker
	is mentioned), but no notions of Android and iOS.

	Windows 8 code shows `redirect_uri=ms-app://{package-security-identifier}`, so 
	custom schemes must be supported (`ms-app` in this case).

	In the step "Invoking the Login Dialog and Setting the Redirect URL":
	
	> redirect_uri. The URL that you want to redirect the person logging in back to. 
	> This URL will capture the response from the Login Dialog. If you are using this 
	> in a webview within a desktop app, this must be set to 
	> https://www.facebook.com/connect/login_success.html. You can confirm that this 
	> URL is set for your app by going to the App Dashboard, clicking Facebook Login 
	> in the right-hand menu, and checking the Valid OAuth redirect URIs in the Client 
	> OAuth Settings section.
	
	Note: **must be set to https://www.facebook.com/connect/login_success.html**
	
	So, no custom URLs (schemes)? 
	
1. standard "native" URIs for redirect: 

	1.	urn:ietf:wg:oauth:2.0:oob 
		and 
	2.	urn:ietf:wg:oauth:2.0:oob:auto
	
	
### Question

https://www.facebook.com/groups/fbdevelopers/permalink/1421835504526626/
https://www.facebook.com/help/community/question/?id=760602314109549&added&rdrhc


Hi
I'm maintainer/developer of Xamarin.Auth Authentication library and recently it was 
extended with Native UI support (Android CustomTabs and iOS SFSafariViewController). 
This implementation was necessary due to google's new OAuth requirements/restrictions 
(Embedded WebViews are blocked).

Xamarin.Auth actually uses Manually Build Login flow, but from that doc, seems it is 
impossible to define redirect_url with custom scheme (there is explicitly stated that 
`redirect_url` must be https://www.facebook.com/connect/login_success.html)

From doc for Windows 8 seems like custom schemes are possible (ms-app) and FB SDK uses 
`fb<appid>`.

Can you help me to make minimal sample (server side setup for Facebook Login with custom 
scheme)?

Can I write a github gist in Markdown and post link here, so we can discuss it? I need 
formatting (quotes, code snippets etc)

Thanks

### Answer

OK. Solution found: The best concept for redirect_url is to use (C# 7 code):

```csharp
$"fb{fb_app_id}://authorize"
```

This works with Xamarin.Auth both Native UI and Embedded WebViews. This can be found in 
FB SDK, but seems like it is not documented.
