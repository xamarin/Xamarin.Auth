# Xamarin.Auth Details

WORK IN PROGRESS

[![Components-XamarinAuth][7]][8]

[7]: https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth/badge/icon
[8]: https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth

Xamarin.Auth helps developers authenticate users via standard authentication mechanisms 
(e.g. OAuth 1.0 and 2.0), and store user credentials. It's also straightforward  to add 
support for non-standard authentication schemes. 

## Current version and status

*   nuget version 1.5.0
    *   Native UI (CustomTabs on Android and SFSafariViewController on iOS)
	*	Xamarin.Forms support	
		*	Xamarin.Android (tested)	
		*	Xamarin.iOS (tested)
		*	Windows platforms (tests in progress)	
    *   Xamarin.iOS Embedded Browser WKWebView support as alternative
        WKWebView instead of UIWebView  

		
[Change Log](./docs/details/change-log.md)        

### Status

CI servers:

*	MacOSX builds

	https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth/
	
*	Windows builds		

	https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth-Windows/
    
	
Xamarin.Auth has grown into fully fledged cross platform library supporting:

*   Xamarin.Android
*   Xamarin.iOS (Unified only, Classic Support is removed)
*   Windows Phone Silverlight 8 (8.1 redundant)
*   Windows Store 8.1 WinRT
*   Windows Phone 8.1 WinRT
*   Universal Windows Platform (UWP)

The library is cross-platform, so once user learns how to use it on one platform,
it is  fairly simple to use it on other platforms.

Recent changes in Xamarin.Auth brought in new functionalities which caused minor
breaking changes. Version 1.4.x broke `GetUI()` API, because it returned `System.Object`	
instead of `Intent` on Android and `UIViewController` on iOS. Efforts to add Xamarin.Forms
support led to more refactoring and pushing functionality deeper into Xamarin.Auth, so
version 1.5.0 reverted `GetUI()` API to original flavor returning UI object for each 
platform.

So, in version 1.5.0 `GetUI()` returns

*	on Android:

	*	`Android.Content.Intent` for embedded WebViews and NativeUI
	
*	on iOS: 

	*	`UIKit.UIViewController` for embedded WebViews and
	
	*	`SafariServices.SFSafariViewController` for Native UI
	


[TODO API design and breaking changes]

## Work in progress and plans


*   Xamarin.Forms 
	*	Windows support - testing/samples
	*	different navigation
*	more samples
	*	Azure AD B2C
	*	Azure ADAL
	*	Flickr
*	restructuring samples
*   UserAgent API     
	[DEPRECATED] [NOT RECOMMENDED] ToS violations
    workaround for attempts to fake UserAgent for Embedded Browsers to fool	
	Google

## Support 

If there is need for real-time support use Xamarin Chat (community slack team) and go to
\#xamarin-auth-social channel where help from experienced users can be obtained.
For all users without account for community slack team, please, go to self-invite link
first.

### Documentation - Github Wiki pages


### Github

Issues

Samples (with nuget references) from the repo separated for faster development:

https://github.com/moljac/Xamarin.Auth.Samples.NugetReferences/


### Xamarin Forums

https://forums.xamarin.com/search?search=auth

https://forums.xamarin.com/search?search=xamarin.auth

### Stackoverflow

http://stackoverflow.com/search?q=xamarin.auth

### Xamarin Chat - Community Slack Team (xamarin-auth-social room)

For those that need real-time help (hand-in-hand leading through implementation) the 
best option is to use community slack channel. There are numerous people that have
implemented Xamarin.Auth with Native UI and maintainers/developers of the library.

https://xamarinchat.slack.com/messages/C4TD1NHPT/
    
For those without Xamarin Chat account please visit this page and generate 
self-invitation:

https://xamarinchat.herokuapp.com/


## OAuth 

OAuth flow (process) is setup in 4 major steps:

0.  **Server side setup for OAuth service provider** 

	To name some:
	
	1.	Google
	
		Google introduced mandatory use of Native UI for security reasons because	
		Android CustomTabs and iOS SFSafariViewController are based on Chrome and	
		Safari code-bases thus thoroughly tested and regulary updated. Moreover 	
		Native UI (System Browsers) have reduced API, so malicious users have less	
		possibilities to retrieve and use sensitive data.
		
		[Google](./details/setup-server-side-oauth-providers/google.md)
	
	2.	Facebook
	
		[Facebook](./details/setup-server-side-oauth-providers/facebook.md)

	3.	Microsoft
	
		[Microsoft](./details/setup-server-side-oauth-providers/microsoft.md)
	
	4.	Fitbit	
		
		Fitbit is good for testing, because it allows arbitrary values for		
		redirect_url.
		
		[Fitbit](./details/setup-server-side-oauth-providers/fitbit.md)
		
	5.

1.  **Client side initialization of Authenticator object**
      
    This step prepares all relevant OAuth Data in Authenticator object (client_id,
	redirect_url, client_secret, OAuth provider's endpoints etc)

2.  **Creating and optionally customising UI**      

3.  **Presenting/Launching UI and authenticating user**	

	1.	Detecting/Fetching/Intercepting URL change - redirect_url and  

		This substep, often called "App Linking" or "Deep Linking", is needed for 
		NativeUI and requires a custom scheme registration for the redirect_url  
		intercepting mechanism.

    2.	Parsing OAuth data from redirect_url

		In order to obtain OAuth data returned redirect_url must be parsed and the	
		best way is to let Xamarin.Auth do it automatically by parsing redirect_url 
		
	3.	Triggering Events based on OAuth data 
	
		Parsing subsytem of the Authenticator will parse OAuth data and raise	
		appropriate events based on returned data

4.  **Using identity** 

	1.	Using protected resources (making calls)	
	
	2.	Saving account info
	
	3.	Retrieving account info
	

Xamarin.Auth with Embedded Browser API does a lot under the hood for users, but with 
the Native UI step 3.1 Deep Linking (App linking) must be manually implemented by the 
user.


## Xamarin.Auth usage 

Xamarin.Auth covers 2 Xamarin technologies - traditional/standard (Xamarin.Android, 
Xamarin.iOS) and Xamarin.Forms. The library implements nuget "bait and switch" 
technology.	

### Usage Xamarin Traditional (Standard)

Design of the library is aimed to reduce platform differences, but this is not possible
in all cases (most methods in Android API need Context as a parameter), so user must
be familiar with platform concepts and details. 

#### 1. Initialization

##### 1.1 Creating and configuring an Authenticator

The server side setup of the OAuth provider defines OAuth flow used which again
defines which Authenticator constructor will be used.

This code is shared accross all platforms:

```csharp
OAuth2Authenticator auth = new OAuth2Authenticator
                (
                    clientId: "",
                    scope: oauth2.OAuth2_Scope,
                    authorizeUrl: oauth2.OAuth_UriAuthorization,
                    redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
                    // Native UI API switch
                    //      true    - NEW Native UI support 
                    //      false   - OLD Embedded Browser API [DEFAULT]
                    // DEFAULT will be switched to true in the near future 2017-04
                    isUsingNativeUI: test_native_ui
                )
            {
                AllowCancel = oauth1.AllowCancel,
            };                        
```

[TODO Link to code]


##### 1.1 Subscribing to Authenticator events

In order to receive OAuth events Authenticator object must subscribe to the 
events.

```csharp
Auth1.Completed += Auth_Completed;
Auth1.Error += Auth_Error;
Auth1.BrowsingCompleted += Auth_BrowsingCompleted;
```

[TODO Link to code]

In those events user can close the Login UI or perform further actions, based on
event raised (Completed or Error) and information which can be extracted from
EventArgs.

The Authenticator object has three events: 

*	Completed - which is fired when an Authentication is successful, 
*	Error - which is fired if the Authentication fails, and 
*	BrowsingCompleted which is fired when when the browser window is closed 

User will need to subscribe to these event in order to work with the data received 
from the authentication response.


#### 2. Creating/Preparing UI

##### 2.1 Creating Login UI

Creating UI step will call `GetUI()` method on Authenticator object which
will return platform specific object to present UI for login.

This code cannot be shared for most of the platforms, because it returns platform
specific objects.

On Android:
	
```csharp
// Step 2.1 Creating Login UI 
Android.Content.Intent ui_object = Auth1.GetUI(this);
```

[TODO Link to code]

On iOS:
	
```csharp
// Step 2.1 Creating Login UI 
UIKit.UIViewController ui_object = Auth1.GetUI();
```

[TODO Link to code]

for new API (both Embedded Browsers and Native UI Support) user will need to
cast object to appropriate type:

NOTE: there is still discussion about API and returning object, so
this might be subject to change.

NOTE: 
Windows platforms currently do NOT support Native UI embedded browser support 
only. Work in progress.

On Windows - Universal Windows Platform

```csharp
// Step 2.1 Creating Login UI 
Type page_type = auth.GetUI();

this.Frame.Navigate(page_type, auth);
```

[TODO Link to code]


On Windows - WinRT - Windows Store 8.1 and Windows Phone 8.1

```csharp
// Step 2.1 Creating Login UI 
System.Type page_type = auth.GetUI();
```

[TODO Link to code]


On Windows Phone Silverlight 8.x 

```csharp
// Step 2.1 Creating Login UI 
System.Uri uri = auth.GetUI ();
```

[TODO Link to code]


##### 2.2 Customizing the UI - Native UI [OPTIONAL]

Embedded Browser API has limited API for UI customizations, while
Native UI API is essentially more complex especially on Android.

**Xamarin.Android**

Native UI on Android exposes several objects to the end user which enable UI 
customisations like adding menus, toolbars and performance optimisations like 
WarmUp (preloading of the browser in the memory) and prefetching (preloading 
of the web site before rendering).

Those exposed objects from simpler to more complex:

*	CustomTabsIntent object which is enough for simple (basic) launch	
	of Custom Tabs (System Browser)
*	CustomTabsIntent.Builder class which is intended for adding menus,	
	toolbars, backbuttons and more. 	
	This object is returned by GetUI() on Android 

```csharp
```

[TODO Link to code]

[TODO finish API and more info]

**Xamarin.iOS**

Native UI on iOS exposes SFSafariViewController and customizations are performed 
through the API of that object.

```csharp
```

[TODO Link to code]


#### 3 Present/Launch the Login UI

This step will open a page of OAuth provider enabling user to enter the
credentials and authenticate.


NOTE: there is still discussion about API and returning object, so this might be subject to change.

**Xamarin.Android**

```csharp
// Step 3 Present/Launch the Login UI
StartActivity(ui_object);
```

**Xamarin.iOS**

```csharp
// Step 3 Present/Launch the Login UI
PresentViewController(ui_object, true, null);
```

**Windows - Universal Windows Platform**

```csharp
this.Frame.Navigate(page_type, auth);
```

[TODO Link to code]


**Windows - WinRT - Windows Store 8.1 Windows Phone 8.1**

```csharp
this.Frame.Navigate(page_type, auth);
```

[TODO Link to code]

**Windows Phone Silverlight 8.x**

```csharp
this.NavigationService.Navigate(uri);
```

[TODO Link to code]

#### 3.1 


#### 3.2 Native UI support - Parsing URL fragment data

The main reason for introducing Native UI support for Installed Apps (mobile apps)
is security. Both Android's [Chrome] Custom Tabs and iOS SFSafariViewController
originate from (share the same codebase) the Google's Chrome browser and Apple's
Safari web browser. This codebase is constantly updated and fixed.
Furthemore both Custom Tabs and Safari View Controller have minimal API, so attacking
surface for potential attacker is smaller. Additionally, Custom Tabs have additional
features aimed at increasing performance - faster loading and prefetching.

Due to the fact that, it is impossible to obtain loaded URL from Custom Tab or 
Safari View Controller after redirecting to redirect url (callback) in order to 
parse OAuth data like auth token, user must use App Linking and custom url schemes
to intercept callback.

This has some repercusions that http and https schemes will not work anymore, because
Android and iOS will open default apps for those schemes and those are built in
browsers (Android Browser and Safari).

    NOTE: 
    Android docs are showing IntentFilters with http and https schema, but after
    several attempts to implement this was temporarily abandonded.
    iOS will most likely open those URLs in browser, except those that were
    registered with some apps based on host (Maps http://maps.google.com, 
    YouTube http://www.youtube.com/ etc).
    
    Some other schemes like mailto will open on 
        Android Intent picker to let user choose which Intent/App will handle 
        scheme
        iOS 

		
To enable Native UI support 3 steps are neccessary:

1.	add references to external utilities that implement NativeUI usually 	
	nuget packages.
	
	This step is neccessary for Android only, because CustomTabs implementation
	is in `Xamarin.Android.Support.CustomTabs` nuget. `SafariServices` are part
	of newer iOS operating systems.
	
2.	register custom scheme at OS level

	Operating system needs to know which application will open particular custom	
	url scheme. This concept is called App or Deep Linking. On Android this 
	registration is done via url handling Activity's IntentFilter and on iOS via
	
3.	implement platform specific code that intercepts redirect_url with custom scheme

	On Android Activity handles opening Url with custom scheme and this Activity
	was registered at OS level thorugh IntentFilter in step 2. On iOS user is 
	supposed to implement `OpenUrl` in the `AppDelegate` class.
	When browser tries to open Url with custom scheme and the browser itself is not
	registered to open that scheme it will raise event on OS level and OS will check
	application registrations for that specific scheme. If the application is found
	url will be passed to `Activity`'s `OnCreate()` and/or `AppDelegate`'s `OpenUrl()`.
	

##### Preparing app for the Native UI support
    
For Android app add Xamarin.Android.Support.CustomTabs package through nuget
package manager.

For iOS apps - NOOP - nothing needs to be done.

Adding URL custom schema intercepting utility for parsing

Next step is to define custome scheme[s] the app can handle.

    NOTE:
    In the samples 
        xamarinauth
        xamarin-auth
        xamarin.auth
    shemes are used.
    Do NOT use those schemes, because schemes might be opened by Xamarin.Auth
    sample app if they were installed (tested before).
    
Xamarin.Android 

Add Activity with IntentFilter to catch/intercept URLs
with user's custom schema:

```csharp
    [Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
                    {
                        Intent.CategoryDefault,
                        Intent.CategoryBrowsable
                    },
            DataSchemes = new[]
                    {
                        "com.xamarin.traditional.standard.samples.oauth.providers.android",
                        /*
                        "com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn",
                        "urn:ietf:wg:oauth:2.0:oob",
                        "urn:ietf:wg:oauth:2.0:oob.auto",
                        "http://localhost:PORT",
                        "https://localhost:PORT",
                        "http://127.0.0.1:PORT",
                        "https://127.0.0.1:PORT",              
                        "http://[::1]:PORT", 
                        "https://[::1]:PORT", 
                        */
                    },
            //DataHost = "localhost",
            DataPath = "/oauth2redirect"
        )
    ]
    public class ActivityCustomUrlSchemeInterceptor : Activity
    {
        string message;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(uri_android.ToString());

			// load redirect_url Page (send it back to Xamarin.Auth)
			//	for Url parsing and raising Complete or Error events
            AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

            this.Finish();

            return;
        }
    }
}
```

[TODO Link to code]


IntentFilter attribute will modify AndroidManifest.xml adding following node (user
could have added this node manually to application node):

```xml
<activity android:label="ActivityCustomUrlSchemeInterceptor" android:name="md5f8c707217af032b51f5ca5f983d46c8c.ActivityCustomUrlSchemeInterceptor">
  <intent-filter>
	<action android:name="android.intent.action.VIEW" />
	<category android:name="android.intent.category.DEFAULT" />
	<category android:name="android.intent.category.BROWSABLE" />
	<data android:host="localhost" />
	<data android:scheme="xamarinauth" />
	<data android:scheme="xamarin-auth" />
	<data android:scheme="xamarin.auth" />
  </intent-filter>
</activity>
```

[TODO Link to code]

Xamarin.iOS

Register custom schemes to Info.plist by opening editor in Advanced tab
and add schemes in URL types with Role Viewer.

This will result in following Info.plist snippet:

```xml
<!--
	Info.plist
-->
<key>CFBundleURLTypes</key>
<array>
	<dict>
		<key>CFBundleURLName</key>
		<string>Xamarin.Auth Google OAuth</string>
		<key>CFBundleURLSchemes</key>
		<array>
			<string>com.xamarin.traditional.standard.samples.oauth.providers.ios</string>
			<string>com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh</string>
		</array>
		<key>CFBundleURLTypes</key>
		<string>Viewer</string>
	</dict>
</array>
```

[TODO Link to code]


NOTE:
When editing Info.plist take care if it is auto-opened in the generic plist editor.
Generic plist editor shows "CFBundleURLSchemes" as simple "URL Schemes".
If user is using the plist editor to create the values and type in URL Schemes, 
it won't convert that to CFBundleURLSchemes.
Switching to the xml editor and user will be able to see the difference.


Add code to intercept opening URL with registered custom scheme by implementing
OpenUrl method override in AppDelegate:

```csharp
public override bool OpenUrl
		(
			UIApplication application,
			NSUrl url,
			string sourceApplication,
			NSObject annotation
		)
{
	// Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
	Uri uri_netfx = new Uri(url.AbsoluteString);

	// load redirect_url Page (send it back to Xamarin.Auth)
	//	for Url parsing and raising Complete or Error events
	AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

	return true;
}
```

[TODO Link to code]

#### 4 Using identity

[TODO]


### Usage Xamarin.Forms

Since version 1.5.0 Xamarin.Auth has built in support for Xamarin.Forms with 2 
different implementations:

*	with platform specific Presenters (Dependency Service, Dependency Injection)	

	This implementation has no dependencies on Xamarin.Forms, so it is in Xamarn.Auth	
	nuget package.
	
*	with Custom Renderers 

	This implementation dependens on Xamarin.Forms, so it is in separate nuget 
	package - Xamarn.Auth.XamarinForms	
	
Most of the important code is placed in portable class library (PCL) or shared 
project.

This includes:

1.	initialization of the Authenticator object 

	This step involves construction of the object and subscribing to the events.
	
2.	Launching UI screen

	UI can be launched via Custom Renderers or platform Presenters.
	
3.	Using identity (obtained OAuth) data


#### 1. Initialization

Initialization is almost the same like for Traditional/Standard, except platform
specific cases when required (different client_id for different apps).


##### 1.1 Creating and configuring an Authenticator

Creating Authenticator object in PCL:

```csharp
authenticator
	 = new Xamarin.Auth.OAuth2Authenticator
	 (
		 clientId:
			 new Func<string>
				(
					 () =>
					 {
						 string retval_client_id = "oops something is wrong!";

						 // some people are sending the same AppID for google and other providers
						 // not sure, but google (and others) might check AppID for Native/Installed apps
						 // Android and iOS against UserAgent in request from 
						 // CustomTabs and SFSafariViewContorller
						 // TODO: send deliberately wrong AppID and note behaviour for the future
						 // fitbit does not care - server side setup is quite liberal
						 switch (Xamarin.Forms.Device.RuntimePlatform)
						 {
							 case "Android":
								 retval_client_id = "1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn.apps.googleusercontent.com";
								 break;
							 case "iOS":
								 retval_client_id = "1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh.apps.googleusercontent.com";
								 break;
							 case "Windows":
								 retval_client_id = "1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh.apps.googleusercontent.com";
								 break;
						 }
						 return retval_client_id;
					 }
			   ).Invoke(),
		 clientSecret: null,   // null or ""
		 authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
		 accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
		 redirectUrl:
			 new Func<Uri>
				(
					 () =>
					 {

						 string uri = null;

						 // some people are sending the same AppID for google and other providers
						 // not sure, but google (and others) might check AppID for Native/Installed apps
						 // Android and iOS against UserAgent in request from 
						 // CustomTabs and SFSafariViewContorller
						 // TODO: send deliberately wrong AppID and note behaviour for the future
						 // fitbit does not care - server side setup is quite liberal
						 switch (Xamarin.Forms.Device.RuntimePlatform)
						 {
							 case "Android":
								 uri =
									 "com.xamarin.traditional.standard.samples.oauth.providers.android:/oauth2redirect"
									 //"com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn:/oauth2redirect"
									 ;
								 break;
							 case "iOS":
								 uri =
									 "com.xamarin.traditional.standard.samples.oauth.providers.ios:/oauth2redirect"
									 //"com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh:/oauth2redirect"
									 ;
								 break;
							 case "Windows":
								 uri =
									 "com.xamarin.traditional.standard.samples.oauth.providers.ios:/oauth2redirect"
									 //"com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh:/oauth2redirect"
									 ;
								 break;
						 }

						 return new Uri(uri);
					 }
				 ).Invoke(),
		 scope:
					  //"profile"
					  "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/plus.login"
					  ,
		 getUsernameAsync: null,
		 isUsingNativeUI: native_ui
	 )
	 {
		 AllowCancel = true,
	 };
```

[TODO Link to code]


##### 1.2 Subscribing to Authenticator events

```csharp
authenticator.Completed +=
	(s, ea) =>
		{
			StringBuilder sb = new StringBuilder();

			if (ea.Account != null && ea.Account.Properties != null)
			{
				sb.Append("Token = ").AppendLine($"{ea.Account.Properties["access_token"]}");
			}
			else
			{
				sb.Append("Not authenticated ").AppendLine($"Account.Properties does not exist");
			}

			DisplayAlert
					(
						"Authentication Results",
						sb.ToString(),
						"OK"
					);

			return;
		};

authenticator.Error +=
	(s, ea) =>
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Error = ").AppendLine($"{ea.Message}");

			DisplayAlert
					(
						"Authentication Error",
						sb.ToString(),
						"OK"
					);
			return;
		};

```

[TODO Link to code]


#### 2. Creating/Preparing UI

##### 2.1 Creating Login UI

Creating UI will be performed in PCL  

*	for Custom Renderers user must create  `AuthenticatorPage` object

*	for Presenters user must create OAuthLoginPresenter object

 must be created and on its instance 
	method `Login(authenticator)` must be called. This method calls platform code.


##### 2.2 Customizing the UI - Native UI [OPTIONAL]

Customizing UI must be performed in platform specific projects with platform specific
API

On Android in `MainActivity.OnCreate()`:

```csharp
Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
```

[TODO Link to code]


On iOS in `AppDelegate.FinishedLaunching()`: 

```csharp
Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
```

[TODO Link to code]


[TODO Windows]

[TODO customization]


#### 3 Present/Launch the Login UI

*	for Custom Renderers user must Navigate to `AuthenticatorPage` object

	Implementation is hidden in Custom Renderers inplatformspecific code
	
*	for Presenters user must call `Login(authenticator)` method on 
	`OAuthLoginPresenter` object

	This method calls platform specific code.

```csharp
if (forms_implementation_renderers)
{
	// Renderers Implementaion
	Navigation.PushModalAsync(new Xamarin.Auth.XamarinForms.AuthenticatorPage());
}
else
{
	// Presenters Implementation
	Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
	presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
	presenter.Login(authenticator);
}

```

[TODO Link to code]

	
### 3.1 Detecting/Fetching/Intercepting URL change (redirect_url)

After user authenticates on the authorization endpoint of the OAuth service provider, the
client app will receive response from server containing OAuth data, because OAuth exchanges
the data through client (user's app) requests and server responses (OAuth service provider).

#### 3.1.1 Embedded WebViews

For Embedded WebView implementation everything is done automatically by Xamarin.Auth. All user
needs to do is to subscribe to the events (3.3 Triggering Events based on OAuth data).

#### 3.1.2 Native UI

Native UI implementation requires more manual work by the user and understanding of the concepts
calles "App linking" or sometimes "Deep linking". "App linking" is considered to be advanced
topic in mobile app development, but gains traction for intra-application communication which is
needed for authentication with Native UI. 

Native UI is implemented on Android with CustomTabs (Chrome Custom Tabs) and on iOS through Safari
ViewController (SFSafariViewController). Both CustomTabs and SFSafariViewController are API for
communicating with OS/system browser. This API has reduced surface, again for security reasons, so
user is not able to access url loaded, cache, cookies, etc. Another reason for enforcing this concept
is the fact that the codebase of the system browser (Chrome and Safari) which is used by Native UI
is thoroughly tested, stable and regularly updated with OS updates.

NOTE: On Android there are 4 versions of Chrome browser that implement CustomTabs, Firefox, Opera
and Samsung Browser, which complicates implementation. Furthermore there is no guarantee that
CustomTabs compatible borwser is installed at all. Xamarin.Auth has code for detecting CustomTabs
compatible packages, but it is "work in progress". 

API itself launches authentication/login flow in external process (system browser), so after login
and server's response it is necessary to return to the application that launched the OAuth flow for
OAuth data parsing. This is done through "App Linking" in 2 steps:

1.  registering URL scheme for redirect_url at OS level
2.  Implementing the code which detects/fetches/intercepts returned redirect_url with the data

Scheme detection/interception is actually done by the operating system, because browser receives
response from server with custom scheme and will try to load this URL. If the scheme is not known
to the browser it will not load it, but report to the OS by raising event. Operating System checks
on system level for registered schemes and if scheme is found OS will launch registered/associated
application and send it original URL.

NOTE: this is the reason why http[s] schemes are discouraged for OAuth with Xamarin.Auth. If http[s]
scheme is used by redirect_url, it will be opened by system browser and user will not receive events
and has no ability to access and analyze/parse url. Again Xamarin.Auth has code to detect http[s]
schemes used in Native UI and will show Alert/PopUp. In the future versions this will be configurable,
but user will be responsible in the case of problems.

Registering URL in Android applications is done with IntentFilter[s] which is defined in conjunction
with Activity which will be called after scheme detection for URL parsing. The parsing is done in
`OnCreate()` method of the Activity. In Xamarin.Android IntentFilter is defined as an attribute to
Activity and it will modify `AndroidManifest.xml` by adding following xml code snippet:

```xml
    <activity android:label="ActivityCustomUrlSchemeInterceptor" 
    android:launchMode="singleTop" android:noHistory="true" android:name="md52ecc484fd43c6baf7f3301c3ba1d0d0c.ActivityCustomUrlSchemeInterceptor">
      <intent-filter>
        <action android:name="android.intent.action.VIEW" />
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        <data android:path="/oauth2redirect" />
        <data android:scheme="com.xamarin.traditional.standard.samples.oauth.providers.android" />
      </intent-filter>
    </activity>
```

On iOS registering is done through Info.plist by opening it, going to Advanced tab and in section
URL Types clicking on Add URL Type. The data supplied should be Identifier, Role is Viewer and
comma separated list of URL schemes - custom schemes for redirect_url. Info.plist opened in text 
editor should have similar xml code snippet:

```xml
<key>CFBundleURLTypes</key>
<array>
    <dict>
        <key>CFBundleURLName</key>
        <string>Xamarin.Auth Google OAuth</string>
        <key>CFBundleURLSchemes</key>
        <array>
            <string>com.xamarin.traditional.standard.samples.oauth.providers.ios</string>
            <string>com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh</string>
        </array>
        <key>CFBundleURLTypes</key>
        <string>Viewer</string>
    </dict>
</array>
```

Upon registered custom scheme detection by the browser and passed to OS, Android OS will start Activity 
with registered scheme and user can fetch URL in `OnCreate()` method, while iOS will call `OpenUrl()`
method of the application's `AppDelegate`.

### 3.2 Parsing OAuth data from redirect_url

The data from server response is in the key-value form and Xamarin.Auth  in Embedded WebView 
implementation does extracting (parsing) of the data automatically. User intervention is not necessary.
In Native UI the flow is leaving Xamarin.Auth with launching Native UI and after OS returns the URL
in `Activity,OnCreate()` or `AppDelegate.OpenUrl()` user needs to parse this URL or pass the URL to the
Xamarin.Auth's `Authenticator` object by calling  `OnPageLoading(Uri)` and passing redirect_url as
method argument.

### 3.3 Triggering Events based on OAuth data

Events are automatically raised by Xamarin.Auth after the process of parsing OAuth data. All user needs
to do is to subscribe to the events (`Completed` and `Error`):

```csharp
authenticator.Completed +=
    (s, ea) =>
        {
            StringBuilder sb = new StringBuilder();

            if (ea.Account != null && ea.Account.Properties != null)
            {
                sb.Append("Token = ").AppendLine($"{ea.Account.Properties["access_token"]}");
            }
            else
            {
                sb.Append("Not authenticated ").AppendLine($"Account.Properties does not exist");
            }

            DisplayAlert("Authentication Results", sb.ToString(), "OK");

            return;
        };

authenticator.Error +=
    (s, ea) =>
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Error = ").AppendLine($"{ea.Message}");

            DisplayAlert("Authentication Error", sb.ToString(), "OK");
            return;
        };
```

The only difference between Embedded WebView implementation and Native UI is that Embedded WebView
implementation allows use of local `Authenticator` object, while Native UI needs exposed public
object (static or not) in some state variable, so it can be accessed from intercepting Activity on
Android and `AppDelegate.OpenUrl()` on iOS.

```csharp
// after initialization (creation and event subscribing) exposing local object 
AuthenticationState.Authenticator = authenticator;
```

### Code

Android code showing how to register custom scheme with IntentFilter for some Activity that will
intercept and parse the URL:

```csharp
[Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
[
    IntentFilter
    (
        actions: new[] { Intent.ActionView },
        Categories = new[]
                {
                    Intent.CategoryDefault,
                    Intent.CategoryBrowsable
                },
        DataSchemes = new[]
                {
                    "com.xamarin.traditional.standard.samples.oauth.providers.android",
                    /*
                    TODO: test all these redirect urls
                    "com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn",
                    "urn:ietf:wg:oauth:2.0:oob",
                    "urn:ietf:wg:oauth:2.0:oob.auto",
                    "http://localhost:PORT",
                    "https://localhost:PORT",
                    "http://127.0.0.1:PORT",
                    "https://127.0.0.1:PORT",              
                    "http://[::1]:PORT", 
                    "https://[::1]:PORT", 
                    */
                },
        //DataHost = "localhost",
        DataPath = "/oauth2redirect"
    )
]
public class ActivityCustomUrlSchemeInterceptor : Activity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        global::Android.Net.Uri uri_android = Intent.Data;

        // Convert Android.Net.Url to C#/netxf/BCL System.Uri - common API
        Uri uri_netfx = new Uri(uri_android.ToString());

        // load redirect_url Page for parsing
        AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

        this.Finish();

        return;
    }
}
```

[TODO link to the code]

On iOS these steps are performed in `AppDelegate.OpenUrl()` method:

```csharp
public partial class AppDelegate
{
    public override bool OpenUrl
            (
                UIApplication application,
                NSUrl url,
                string sourceApplication,
                NSObject annotation
            )
    {
        // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
        Uri uri_netfx = new Uri(url.AbsoluteString);

        // load redirect_url Page for parsing
        AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

        return true;
    }
}
```

[TODO link to the code]


## 4 Using identity 

### 4.1 Making requests to protected resources

With obtained access_token (identity) user can now access protected resources.

Since Facebook is an OAuth2 service, user can make requests with `OAuth2Request` providing 
the account retrieved from the `Completed` event. Assuming user is authenticated, it is possible     
to grab the user's info:

```csharp
OAuth2Request request = new OAuth2Request 
                            (
                                "GET",
                                 new Uri ("https://graph.facebook.com/me"), 
                                 null, 
                                 eventArgs.Account
                            );
request.GetResponseAsync().ContinueWith 
    (
        t => 
        {
            if (t.IsFaulted)
                Console.WriteLine ("Error: " + t.Exception.InnerException.Message);
            else 
            {
                string json = t.Result.GetResponseText();
                Console.WriteLine (json);
            }
        }
    );
```

[TODO Link to code]


### 4.2 Store the account

Xamarin.Auth securely stores `Account` objects so that users don't always have to reauthenticate 
the user. The `AccountStore` class is responsible for storing `Account` information, backed by 
the 
[Keychain](https://developer.apple.com/library/ios/#documentation/security/Reference/keychainservices/Reference/reference.html) 
on iOS and a [KeyStore](http://developer.android.com/reference/java/security/KeyStore.html) on 
Android.

Creating `AccountStore` on Android:

```csharp
// On Android:
AccountStore.Create (this).Save (eventArgs.Account, "Facebook");
```

[TODO Link to code]


Creating `AccountStore` on iOS:

```csharp
// On iOS:
AccountStore.Create ().Save (eventArgs.Account, "Facebook");
```

[TODO Link to code]


Saved Accounts are uniquely identified using a key composed of the account's 
`Username` property and a "Service ID". The "Service ID" is any string that is 
used when fetching accounts from the store.

If an `Account` was previously saved, calling `Save` again will overwrite it. 
This is convenient for services that expire the credentials stored in the account 
object.


## 4.3 Retrieve stored accounts

Fetching all `Account` objects stored for a given service is possible with follwoing API:

Retrieving accounts on Android:

```csharp
// On Android:
IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("Facebook");
```

[TODO Link to code]


Retrieving accounts on iOS:

```csharp
// On iOS:
IEnumerable<Account> accounts = AccountStore.Create ().FindAccountsForService ("Facebook");
```

[TODO Link to code]


It's that easy.

Windows [TODO]

## Extending Xamarin.Auth - Make customized Authenticator

Xamarin.Auth includes OAuth 1.0 and OAuth 2.0 authenticators, providing support for thousands 
of popular services. For services that use traditional username/password authentication, one 
can roll own authenticator by deriving from `FormAuthenticator`.

If user wants to authenticate against an ostensibly unsupported service, fear not – Xamarin.Auth 
is extensible! It's very easy to create own custom authenticators – just derive from any of the 
existing authenticators and start overriding methods.

## Installation

Xamarin.Auth can be installed in binary form (compiled and packaged)
or compiled from source.
 
Binary form is deployable as Nu from nuget.org or Xamarin Component 
from component store:

*	nuget 
*	Component [UPDATE INPROGRESS]


More details about how to compile Xamarin.Auth library and samples can be found in the docs
in repository on github.
See [./details/installation-and-compilation.md](./details/installation-and-compilation.md).


#### More Information
    
https://developer.chrome.com/multidevice/android/customtabs
    
## Installing Xamarin.Auth

Xamarin.Auth can be used (installed) through

1.  nuget package v >= 1.4.0.0
2.  project reference (source code)

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
artifacts to be build first on Windows and then on MacOSX, so nuget target
(nuget packaging) will fail if script is executed 

#### Installing Cake

Installing Cake is pretty easy:

    Windows

        Invoke-WebRequest http://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1
        .\build.ps1

    Mac OSX 

        curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/osx
        chmod +x ./build.sh && ./build.sh

    Linux

        curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/linux
        chmod +x ./build.sh && ./build.sh

#### Running Cake to Build Xamarin.Auth targets

Run cake with following command[s] to build libraries and nuget locally.
For nuget run it 1st on Windows and then on Mac (Xamarin build bots do that
and expect artifacts from Windows to be ready before packaging).

Running these targets is important for automatic package restore.

    Windows

        tools\Cake\Cake.exe --verbosity=diagnostic --target=libs
        tools\Cake\Cake.exe --verbosity=diagnostic --target=nuget
        tools\Cake\Cake.exe --verbosity=diagnostic --target=samples

    Mac OSX 

        mono tools/Cake/Cake.exe --verbosity=diagnostic --target=libs
        mono tools/Cake/Cake.exe --verbosity=diagnostic --target=nuget

Now, samples based on project references are ready to be used!  
        
### Component

Xamarin.Auth Component support is currently under development. It is "empty shell"
component, i.e. component that uses nuget package as dependency and contains only
samples, documentation and artwork.



## Diverse

*Some screenshots assembled with [PlaceIt](http://placeit.breezi.com/).*

