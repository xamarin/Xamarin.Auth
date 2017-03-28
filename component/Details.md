# Details

Xamarin.Auth helps developers authenticate users via standard authentication 
mechanisms  (e.g. OAuth 1.0 and 2.0), and store user credentials. 
It's also straightforward  to add support for non-standard authentication 
schemes. 

Xamarin.Auth has grown into fully fledged cross platform library supporting:

*	Xamarin.Android
*	Xamarin.iOS (Unified only, Classic Support is removed)
*	Windows Phone Silverlight 8 and 8.1
*	Windows Store 8.1 WinRT
*	Windows Phone 8.1 WinRT
*	Universal Windows Platform (UWP)

The library is cross-platform, so once user learns how to use it on one platform,
it is  fairly simple to use it on other platforms.

Recent changes in Xamarin.Auth brought in new functionalities which caused minor
breaking changes.

## Usage

Basic usage steps:

1.	Initialization 
	1.	create Authenticator object (OAuth1Authenticator or OAuth2Authenticator)		
		using constructor with required parameters
	2.	setup events (OnCompleted, OnError, OnCanceled, OnBrowsingCompleted)
2.	Presenting UI 
	1.	authenticator.GetUI()
	2.	decorating UI

### 1. Initialization

Shared code accross all platforms:

```csharp
	var	auth = new OAuth2Authenticator
				(
					clientId: "",
					scope: oauth2.OAuth2_Scope,
					authorizeUrl: oauth2.OAuth_UriAuthorization,
					redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
					// Native UI API switch
					// Default - false
					// will be switched to true in the near future 2017-04
					//      true    - NEW native UI support 
					//              - Android - Chrome Custom Tabs 
					//              - iOS SFSafariViewController
					//              - WORK IN PROGRESS
					//              - undocumented
					//      false   - OLD embedded browser API 
					//              - Android - WebView 
					//              - iOS - UIWebView
					isUsingNativeUI: test_native_ui
				);

            auth.AllowCancel = oauth2.AllowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;
            auth.Error += Auth_Error;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;
						
```
### 2. Presenting UI

Xamarin.Android

```csharp
	// global::Android.Content.Intent intent_as_object = auth.GetUI(this);
	System.Object intent_as_object = auth.GetUI(this);
	if (auth.IsUsingNativeUI == true)
	{
		// NEW UPCOMMING API undocumented work in progress
		// using new Native UI API Chrome Custom Tabs on Android and SFSafariViewController on iOS
		// on 2014-04-20 google login (and some other providers) will work only with this API
		System.Uri uri_netfx = auth.GetInitialUrlAsync().Result;
		global::Android.Net.Uri uri_android = global::Android.Net.Uri.Parse(uri_netfx.AbsoluteUri);

		// Add Android.Support.CustomTabs package 
		global::Android.Support.CustomTabs.CustomTabsActivityManager ctam = null;
		ctam = new global::Android.Support.CustomTabs.CustomTabsActivityManager(this);

		global::Android.Support.CustomTabs.CustomTabsIntent cti = null;
		cti = (global::Android.Support.CustomTabs.CustomTabsIntent)intent_as_object;

		cti.LaunchUrl(this, uri_android);
	}
	else
	{
		// OLD API undocumented work in progress (soon to be deprecated)
		// set to false to use old embedded browser API WebView and UIWebView
		// on 2014-04-20 google login (and some other providers) will NOT work with this API
		// This will be left as optional API for some devices (wearables) which do not support
		// Chrome Custom Tabs on Android.
		global::Android.Content.Intent i = null;
		i = (global::Android.Content.Intent)intent_as_object;
		StartActivity(i);
	}
```

Xamarin.iOS

```csharp
	//#####################################################################
	// Xamarin.Auth API - Breaking Change
	//      old API returned UIKit.UIViewController
	//UIViewController ui_controller = auth.GetUI ();
	//      new API returns System.Object
	System.Object ui_controller_as_object = auth.GetUI();
	if (auth.IsUsingNativeUI == true)
	{
		//=================================================================
		// Xamarin.Auth API - Native UI support 
		//      *   Android - [Chrome] Custom Tabs on Android       
		//          Android.Support.CustomTabs      
		//          and 
		//      *   iOS -  SFSafariViewController     
		//          SafariServices.SFSafariViewController
		// on 2014-04-20 google (and some other providers) will work only with this API
		//  
		//
		//  2017-03-25
		//      NEW UPCOMMING API undocumented work in progress
		//      soon to be default
		//      optional API in the future (for backward compatibility)
		//
		//  required part
		//  add 
		//     following code:
		SafariServices.SFSafariViewController c = null;
		c = (SafariServices.SFSafariViewController)ui_controller_as_object;
		//  add custom schema (App Linking) handling
		//    in AppDelegate.cs
		//         public override bool OpenUrl
		//                                (
		//                                    UIApplication application, 
		//                                    NSUrl url, 
		//                                    string sourceApplication, 
		//                                    NSObject annotation
		//                                )
		//
		//  NOTE[s]
		//  *   custom scheme support only
		//      xamarinauth://localhost
		//      xamarin-auth://localhost
		//      xamarin.auth://localhost
		//  *   no http[s] scheme support
		//------------------------------------------------------------
		// UI customization [OPTIONAL]
		if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
		{
			c.PreferredBarTintColor = UIColor.FromRGB(0x34, 0x98, 0xdb);
			c.PreferredControlTintColor = UIColor.White;
		}
		else
		{
			c.View.TintColor = UIColor.FromRGB(0x34, 0x98, 0xdb);
		}

		Action view_controller_customization =
			() =>
			{
				UIColor xamarin_blue = UIColor.FromRGB(0x34, 0x98, 0xdb);
				c.NavigationController.NavigationBar.TintColor = xamarin_blue;
			};
		PresentViewController(c, true, view_controller_customization);
		//=================================================================
	}
	else
	{
		//=================================================================
		// Xamarin.Auth API - embedded browsers support 
		//     - Android - WebView 
		//     - iOS - UIWebView
		//
		// on 2014-04-20 google (and some other providers) will work only with this API
		//
		//  2017-03-25
		//      soon to be non-default
		//      optional API in the future (for backward compatibility)
		UIViewController c = (UIViewController)ui_controller_as_object;
		PresentViewController(c, true, null);
		//=================================================================
	}
	//#####################################################################
	
```

NOTE: 
Windows platforms currently do NOT support Native UI embedded browser support 
only. Work in progress.

Universal Windows Platform

```csharp
```

Windows Store 8.1 WinRT and Windows Phone 8.1 WinRT

```csharp
```

Windows Phone Silverlight 8.x 

```csharp
```

It's that easy to authenticate users!

## Installing Xamarin.Auth

Xamarin.Auth can be used (installed) through

1.	nuget package v >= 1.4.0.0
2.	project reference (source code)

NOTE: Xamarin Component for new nuget is not ready! 2017-03-28

### Nuget package

Xamarin.Auth nuget package:

https://www.nuget.org/packages/Xamarin.Auth/

Current Version:

https://www.nuget.org/packages/Xamarin.Auth/1.3.2.7

Xamarin.Auth nuget package specification (nuspec):

### Project reference

For debuging and contributing (bug fixing) contributions Xamarin.Auth can be
used as source code for github repo:

Xamarin.Auth project (and folder structure) is based on Xamarin Components Team
internal rules and recommendations.

Xamarin.Auth Cake script file is slightly modified to enable community members
willing to help to compile Xamarin.Auth from source. Compilation is possible
both on Windows and MacOSX. If working on both platforms Cake script expects
artifacts to be build forst on Windows and then on MacOSX, so nuget target
(nuget packaging) will fail if script is executed 

Installing Cake

Windows

	Invoke-WebRequest http://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1
	.\build.ps1

Mac OSX 

	curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/osx
	chmod +x ./build.sh && ./build.sh

Linux

	curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/linux
	chmod +x ./build.sh && ./build.sh

Running Cake to Build Xamarin.Auth targets

Windows

	tools\Cake\Cake.exe --verbosity=diagnostic --target=libs
	tools\Cake\Cake.exe --verbosity=diagnostic --target=nuget
	tools\Cake\Cake.exe --verbosity=diagnostic --target=samples

Mac OSX 

	mono tools/Cake/Cake.exe --verbosity=diagnostic --target=libs
	mono tools/Cake/Cake.exe --verbosity=diagnostic --target=nuget

### Component

Xamarin.Auth Component support is currently under development. It is "empty shell"
component, i.e. component that uses nuget package as dependency and contains only
samples, documentation and artwork.

## Diverse

*Some screenshots assembled with [PlaceIt](http://placeit.breezi.com/).*

