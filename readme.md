# Xamarin.Auth

The valid documentation is still documentation for Component 

*	[./component/GettingStarted.md](./component/GettingStarted.md)
*	[./component/Details.md](./component/Details.md)
*	[./component/License.md](./component/License.md)

## Release Notes

### v.1.3.2.6

### v.1.3.2.5

### v.1.3.2.3

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
		
### v.1.3.0+

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
		