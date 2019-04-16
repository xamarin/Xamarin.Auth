# Change Log and Release Notes

## Changelog

Nuget Version[s]

## Curent Version

NuGet version: v.1.4.1

Component version:

## Change Log

### NuGet v.1.5.0-alpha-13	

2017-05-09

*	Xamarin.Forms CustomRenderers Windows Platforms
*	GitLink symbolsource PDB support
*	OAuth1Authenticator fixes (Twitter fixes)

### NuGet v.1.5.0-alpha-12

2017-05-07

### NuGet v.1.5.0-alpha-11	

2017-05-01

### NuGet v.1.5.0-alpha-10

2017-04-27

### NuGet v.1.5.0-alpha-09	

2017-04-26

### NuGet v.1.5.0-alpha-08	

2017-04-24

### NuGet v.1.5.0-alpha-07	

2017-04-24

### NuGet v.1.5.0-alpha-06	

2017-04-23

### NuGet v.1.5.0-alpha-05	

2017-04-21

### NuGet v.1.5.0-alpha-04	

2017-04-20

### NuGet v.1.5.0-alpha-03	

2017-04-19

### NuGet v.1.5.0-alpha-02	

2017-04-13

### NuGet v.1.5.0-alpha-01	

2017-04-12

### NuGet v.1.4.1 

2017-04-03

*	Native UI with checks and Warnings	
	if http[s] scheme is used
*	Xamarin.iOS embedded browser WKWebView support as alternative
	WKWebView instead of UIWebView	
*   nuget version 1.4.0.0   


### NuGet v.1.4.0.1	

2017-03-30

*	minor Android fixes
*   Embedded Browsers (Android WebView and iOS UIWebView)   
	NOTE: this support will be prohibited by some OAuth providers       
	DEFAULT 2017-03     
*   native UI (Android Custom Tabs and iOS Safari View Controller)      
	must be explicitly setup in Authenticator constructor!  

### NuGet v.1.4.0	

2017-03-30

*   embedded browsers (Android WebView and iOS UIWebView)   
	NOTE: this support will be prohibited by some OAuth providers       
	DEFAULT 2017-03     
*   native UI (Android Custom Tabs and iOS Safari View Controller)      
	must be explicitly setup in Authenticator constructor!  

NOTE: 2017-03-20 use nuget until Component is updated
	
	
	
### NuGet v.1.3.2.6

*	Xamarin.Core (Common)
	*	fixed stupid error (of course me, who else) where default API was 		
		new unfinished/untested/undocumented API
*	Xamarin.Android
	*	fixed stupid error (of course me, who else) where default API was 		
		new unfinished/untested/undocumented API
	*	new parsing token from redirect_urls like localhost and 127.0.0.1
		*	on Android Xamarin.Auth still raises OnError events, but token 		
			can be parsed
*	Xamarin.iOS
	*	fixed stupid error (of course me, who else) where default API was 		
		new unfinished/untested/undocumented API
	*	new parsing token from redirect_urls like localhost and 127.0.0.1
		*	no OnError, because attempt to load localhost or 127.0.0.1		
			is caught in catch block.
			

### NuGet v.1.3.2.5

### NuGet v.1.3.2.3

1.	fixes for nuget packaging for Windows	
	*.rd.xml Runtime Directive files BuildAction=EmbeddedResource and paths		
	1.	Windows Universal Platform		
	2.	Windows Store 8.1 (WinRT)		
	3.	Windows Phone 8.1 (WinRT)		
2.	Initial bits for Native App (Browser) support	
	1.	Android - [Chrome] Custom Tabs 		
		NOTE: braking changes GetUI() returns object instead of Intent!		
		Xamarin.Android.Support.CustomTabs package must be added to the
		application.
	2.	iOS - Safari ViewController		
		SafariServices.SFSafariViewController		
		NOTE: braking changes GetUI() returns object instead of UIViewController!		
		
### NuGet v.1.3.0+

1.	new platforms supported:
	1.	Windows Phone 8 		
		not experimental any more
	2.	WinRT Windows Phone 8.1
	3.	WinRT Windows 8.1
	4.	Universal Windows Platform UWP
	5.	Xamarin.Forms [IN PROGRESS]	
2.	new folder structure for Xamarin CI
3.	nuget support
	1.	Xamarin.Auth
	2.	Xamarin.Auth.Extensions	[IN PROGRESS]	
		for non standard features like refresh_token
	3.	Xamarin.Auth.XamarinForms [IN PROGRESS] 
4.	manually merged Pull Requests:
	1.	PR#57 OnCreatingInitialUrl virtual method
        https://github.com/xamarin/Xamarin.Auth/pull/57
	2.	PR#62 Add new property to disable the escaping of scope parameter.
		https://github.com/xamarin/Xamarin.Auth/pull/62
	3.	PR#94 Marshalled NavigationService.GoBack to UI Thread #94
		https://github.com/xamarin/Xamarin.Auth/pull/94
	4.	PR#88 Added IsAuthenticated check 	
		https://github.com/xamarin/Xamarin.Auth/pull/88
	5.	PR#63 Fix : Android password field now hides the user input in FormAuthenticatorActivity
		https://github.com/xamarin/Xamarin.Auth/pull/63
	6.	PR#58 IgnoreErrorsWhenCompleted
		https://github.com/xamarin/Xamarin.Auth/pull/58
	7.	PR#76 bug fix in SslCertificateEqualityComparer
		https://github.com/xamarin/Xamarin.Auth/pull/76
	8.	PR#79 Adding method to request a refresh token		
		https://github.com/xamarin/Xamarin.Auth/pull/79
	9.	PR#91 OAuth2Authenticator changes to work with joind.in OAuth 	
		https://github.com/xamarin/Xamarin.Auth/pull/91
	10.	PR#59 MakeSecretOptional 		
		https://github.com/xamarin/Xamarin.Auth/pull/59
	11.	PR#62 Add new property to disable the escaping of scope parameter.
		https://github.com/xamarin/Xamarin.Auth/pull/62
5.	bugs fixed (from Xamarin Bugzilla)
	[TODO]
		