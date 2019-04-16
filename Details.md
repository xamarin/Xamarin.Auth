# Xamarin.Auth

Xamarin.Auth makes authenticating with OAuth easy!  Xamarin.Auth helps developers authenticate
users via standard authentication mechanisms (e.g. OAuth 1.0 and 2.0), and store user
credentials. It's also straightforward to add support for non-standard authentication schemes. 

OAuth flow (process) is setup in 5 major steps:

1.  Server side setup for OAuth service provider (Google, Facebook)
2.  Client side initialization of Authenticator object      
3.  Creating and optionally customizing UI      
4.  Presenting/Lunching UI and authenticating user
5.  Using identity


``` csharp
var auth = new OAuth2Authenticator (
	clientId: "App ID from https://developers.facebook.com/apps",
	scope: "",
	authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
	redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"),
	isUsingNativeUI: false);
	
auth.Completed += (sender, eventArgs) => {
	DismissViewController (true, null);
	if (eventArgs.IsAuthenticated) {
		// Use eventArgs.Account to do wonderful things
	}
}

PresentViewController (auth.GetUI (), true, null);
``` 


Xamarin.Auth is cross-platform, so once user learns how to use it on one platform, 
it is fairly simple to use it on other platforms.  Xamarin.Auth has grown into fully 
fledged cross platform library supporting:

 * Xamarin.Android
 * Xamarin.iOS
 * Windows Phone Silverlight 8
 * Windows Store 8.1 WinRT
 * Windows Phone 8.1 WinRT
 * Universal Windows Platform (UWP)


## Current version and status 

[![Xamarin.Auth][3]][4]
[![Components-XamarinAuth][1]][2]


**nuget version 1.5.0**

 * Native UI (CustomTabs on Android and SFSafariViewController on iOS)
 *	Xamarin.Forms support	
   * Xamarin.Android (tested)	
   * Xamarin.iOS (tested)
   * Windows platforms (tests in progress)
	
 *   Xamarin.iOS Embedded Browser WKWebView support as alternative
      WKWebView instead of UIWebView
      
## Support

Xamarin.Auth is open source and a part of the [.Net Foundation][5].  To open a pull 
request or file an issue please see visit the [Xamairn.Auth][6] GitHub repository.

[1]: https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth/badge/icon
[2]: https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth
[3]: https://img.shields.io/nuget/vpre/Xamarin.Auth.svg?maxAge=2592000&label=Xamarin.Auth%20nuget
[4]: https://www.nuget.org/packages/Xamarin.Auth
[5]: https://dotnetfoundation.org/projects
[6]: https://github.com/xamarin/Xamarin.Auth