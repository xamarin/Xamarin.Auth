# Details

[![Components-Mono.Posix][7]][8]

[7]: https://jenkins.mono-project.com/view/Components/job/Components-Mono.Posix/badge/icon
[8]: https://jenkins.mono-project.com/view/Components/job/Components-Mono.Posix

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

		
[Change Log](./details/change-log.md)        

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


*   Xamarin.Forms Windows support	     
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
the Native UI step 3.1 Deep Linking must be manually implemented by the user.


1.	Android's Activity with IntentFilter OnCreated.		

	[TODO add url]		

2.	iOS' AppDelegate.OpenUrl method

	[TODO add url]		

User will need to expose Authenticator object via public field or property.

### 1. Initialization

#### 1.1 Creating and configuring an Authenticator

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


#### 1.1 Subscribing to Authenticator events

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


### 2. Creating/Preparing UI

#### 2.1 Creating Login UI

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


#### 2.2 Customizing the UI - Native UI [OPTIONAL]

Embedded Browser API has limited API for UI customizations, while
Native UI API is essentially more complex especially on Android.

##### Xamarin.Android 

Native UI on Android exposes several objects to the end user which 
enable UI customisations like adding menus, toolbars and performance 
optimisations like WarmUp (preloading of the browser in the memory) 
and prefetching (preloading of the web site before rendering).

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

##### Xamarin.iOS 

Native UI on iOS exposes SFSafariViewController and customizations
are performed on that object.


```csharp
```

[TODO Link to code]


### 3 Present/Launch the Login UI

This step will open a page of OAuth provider enabling user to enter the
credentials and authenticate.


NOTE: there is still discussion about API and returning object, so
this might be subject to change.

##### Xamarin.Android 

```csharp
// Step 3 Present/Launch the Login UI
StartActivity(ui_object);
```

##### Xamarin.iOS 

```csharp
// Step 3 Present/Launch the Login UI
PresentViewController(ui_object, true, null);
```

##### Universal Windows Platform

```csharp
this.Frame.Navigate(page_type, auth);
```

[TODO Link to code]


##### Windows Store 8.1 WinRT and Windows Phone 8.1 WinRT

```csharp
this.Frame.Navigate(page_type, auth);
```

[TODO Link to code]

##### Windows Phone Silverlight 8.x 

```csharp
this.NavigationService.Navigate(uri);
```

[TODO Link to code]

### 4. 

### Native UI support - Parsing URL fragment data

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

#### Preparing app for the Native UI support
    
For Android app add Xamarin.Android.Support.CustomTabs package through nuget
package manager.

For iOS apps - NOOP - nothing needs to be done.

#### Adding URL custom schema intercepting utility for parsing

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
[Activity(Label = "ActivityCustomUrlSchemeInterceptor")]
[
    // App Linking - custom url schemes
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
                    "xamarinauth",
                    "xamarin-auth",
                    "xamarin.auth",
                },
        DataHost = "localhost"
    )
]
public class ActivityCustomUrlSchemeInterceptor : Activity
{
    string message;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Create your application here
        global::Android.Net.Uri uri_android = Intent.Data;


        System.Uri uri = new Uri(uri_android.ToString());
        IDictionary<string, string> fragment = Utilities.WebEx.FormDecode(uri.Fragment);

        Account account = new Account
                                (
                                    "username",
                                    new Dictionary<string, string>(fragment)
                                );

        AuthenticatorCompletedEventArgs args_completed = new AuthenticatorCompletedEventArgs(account);

        if (MainActivity.Auth2 != null)
        {
            // call OnSucceeded to trigger OnCompleted event
            MainActivity.Auth2.OnSucceeded(account);
        }
        else if (MainActivity.Auth1 != null)
        {
            // call OnSucceeded to trigger OnCompleted event
            MainActivity.Auth1.OnSucceeded(account);
        }
        else
        {
        }

        this.Finish();

        return;
    }
}
```

[TODO Link to code]


IntentFilter attribute will modify AndroidManifest.xml adding following node (user
could have added this node manually to application node):

```
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

This will result in following XML snippet in Info.plist (again user can add it 
manually):

```
    <!--
        Info.plist
    -->
        <key>CFBundleURLTypes</key>
       <array>
           <dict>
               <key>CFBundleURLName</key>
               <string>com.example.store</string>
               <key>CFBundleURLTypes</key>
               <string>Viewer</string>
               <key>CFBundleURLSchemes</key>
               <array>
                   <string>xamarinauth</string>
                   <string>xamarin-auth</string>
                   <string>xamarin.auth</string>
                </array>
           </dict>
       </array>
```

[TODO Link to code]


NOTE:
When editing Info.plist take care if it is auto-opened in the generic plist editor.
Generic plist editor shows "CFBundleURLSchemes" as simple "URL Schemes"
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
    System.Uri uri = new Uri(url.AbsoluteString);
    IDictionary<string, string> fragment = Utilities.WebEx.FormDecode(uri.Fragment);

    Account account = new Account
                            (
                                "username",
                                new Dictionary<string, string>(fragment)
                            );

    AuthenticatorCompletedEventArgs args_completed = new AuthenticatorCompletedEventArgs(account);

    if (TestProvidersController.Auth2 != null)
    {
        // call OnSucceeded to trigger OnCompleted event
        TestProvidersController.Auth2.OnSucceeded(account);
    }
    else if (TestProvidersController.Auth1 != null)
    {
        // call OnSucceeded to trigger OnCompleted event
        TestProvidersController.Auth1.OnSucceeded(account);
    }
    else
    {
    }

    return true;
}
```

[TODO Link to code]


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

