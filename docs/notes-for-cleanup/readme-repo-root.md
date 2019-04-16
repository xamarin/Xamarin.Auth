# Xamarin.Auth Repository ReadMe

Xamarin.Auth helps developers authenticate users via standard authentication mechanisms 
(e.g. OAuth 1.0 and 2.0), and store user credentials. It's also straightforward  to add 
support for non-standard authentication schemes. 

## Current version and status

[![Components-XamarinAuth][7]][8]

[7]: https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth/badge/icon
[8]: https://jenkins.mono-project.com/view/Components/job/Components-XamarinAuth


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

### Documentation - Github Wiki pages

These are working docs and are copied to Wiki pages of the repo.

[./docs/readme.md](./docs/readme.md)

[WORK IN PROGRESS]

[./docs/readme-detailed.md](./docs/readme-detailed.md)


### Github

https://github.com/xamarin/Xamarin.Auth/

#### Wiki

[WORK IN PROGRESS]

https://github.com/xamarin/Xamarin.Auth/wiki

#### Issues

https://github.com/xamarin/Xamarin.Auth/issues


#### Samples (with nuget references) from the repo separated for faster development:

https://github.com/moljac/Xamarin.Auth.Samples.NugetReferences/


### Xamarin Forums

https://forums.xamarin.com/search?search=auth

https://forums.xamarin.com/search?search=xamarin.auth

### Xamarin Chat - Community Slack Team (xamarin-auth-social room)

For those that need real-time help (hand-in-hand leading through implementation) the 
best option is to use community slack channel. There are numerous people that have
implemented Xamarin.Auth with Native UI and maintainers/developers of the library.

https://xamarinchat.slack.com/messages/C4TD1NHPT/
    
For those without Xamarin Chat account please visit this page and generate 
self-invitation:

https://xamarinchat.herokuapp.com/


### Stackoverflow

http://stackoverflow.com/search?q=xamarin.auth

Suggestion: use Xamarin formus or even better Community Slack channel. SO moderation
policies make answering very difficult (pasting links and demanding that the person
providing the answer to search for duplicates).



# Xamarin.Auth

Xamarin.Auth is a cross platform library that helps developers authenticate 
users via OAuth protocol (OAuth1 and OAuth2). 

OAuth flow (process) with Xamarin.Auth is set up in 5 steps with 1 step performed 
on OAuth provider's server side (portal, console) and 4 steps performed in the 
client (application).

0.  Server side setup for OAuth service provider (Google, Facebook)

1.  Client side initialization of Authenticator object      
    This step prepares all relevant OAuth Data in Authenticator object (client_id,
	redirect_url, client_secret, OAuth provider's endpoints etc)

2.  Creating and optionally customizing UI      

3.  Presenting/Lunching UI and authenticating user	

	1.	Detecting/Fetching/Intercepting URL change (redirect_url)  

		This sub-step is step needed for NativeUI and requires that custom scheme
		registration together for redirect_url intercepting mechanism. This step	
		is actually App Linking (Deep Linking) concept in mobile applications.

    2.	Parsing OAuth data from redirect_url

		In order to obtain OAuth data returned redirect_url must be parsed and the	
		best way is to let Xamarin.Auth do it automatically by parsing redirect_url 
		
	3.	Triggering Events based on OAuth data 
	
		Parsing subsystem of the Authenticator will parse OAuth data and raise	
		appropriate events based on returned data

4.  Using identity 

	1.	Using protected resources (making calls)
	
	2.	Saving account info
	
	3.	Retrieving account info
  
Those steps and (sub-steps) which will be used in detailed documentation.


## 0 Server Side 

Server side setup of the particular OAuth provider like Google, Facebook or Microsoft Live
differs from provider to provider, especially nomenclature (naming).  In general there are 2 common types of "apps", "projects" or "credentials":

1.  Server or Web Application

    A server (Fitbit naming) or Web (Google and Facebook terms) application is considered to be 
    secure, i.e. client_secret is secure and can be stored and not easily accessed/retrieved 
    by malicious user.
    
    Server/Web app uses http[s] schemes for redirect_url, because it loads real web page 
    (url-authority can be localhost or real hostname like http://xamarin.com).
    
    Xamarin.Auth, prior to version 1.4.0, only supported http[s] url-scheme with real   
    url-authority (existing host, no localhost) and arbitrary url-path. 
    
2.  Native or Installed (mobile or desktop) apps    
    
    This group is usually divided into Android, iOS, Chrome (javascript) and other (.net)   
    subtypes. Each subtype can have different setup. In most cases developer must submit    
    for Android package id with SHA1 key and for iOS BundleID. Custom schemes can be predefined 
    (generated) by provider (Google or Facebook) or defined by user (Fitbit). Generated schemes     
    are usually based on data submitted (package id, bundle id).
    
    Xamarin Components Team is working on the doc with minimal info for common used providers 
    and how to setup server side.

Server side setup details is explained in separate documents in Xamarin.Auth repository. 



## 1 Client Side Initialization

Client (mobile) application initialization is based on OAuth Grant (flow) in use which is 
determined by OAuth provider and it's server side setup.

Initialization is performed through `Authenticator` constructors for:

### OAuth2 Implicit Grant flow 

With parameters:    

    *   clientId        
    *   scope       
    *   authorizeUrl        
    *   redirectUrl     
    
    
### OAuth2 Authorization Code Grant flow 

With parameters:       

    *   clientId   	
    *   clientSecret	
    *   scope       
    *   authorizeUrl  	      
    *   redirectUrl 	
    *   accessTokenUrl	

OAuth details and how Xamarin.Auth implements OAuth is described in documentation in
Xamarin.Auth repo.
	
### 1.1 Create and configure an Authenticator

Let's authenticate a user to access Facebook which uses OAuth2 Implicit flow:

```csharp
using Xamarin.Auth;
// ...
OAuth2Authenticator auth = new OAuth2Authenticator 
    (
        clientId: "App ID from https://developers.facebook.com/apps",
        scope: "",
        authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
        redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"),
        // switch for new Native UI API
        //      true = Android Custom Tabs and/or iOS Safari View Controller
        //      false = Embedded Browsers used (Android WebView, iOS UIWebView)
        //  default = false  (not using NEW native UI)
        isUsingNativeUI: use_native_ui
    );
```

Facebook uses OAuth 2.0 authentication, so we create an `OAuth2Authenticator`. 
Authenticators are responsible for managing the user interface and communicating with 
authentication services.

Authenticators take a variety of parameters; in this case, the application's ID, its 
authorization scope, and Facebook's various service locations are required.


### 1.2 Setup Authentication Event Handlers

Before the UI is presented, user needs to start listening to the `Completed` event which fires 
when the user successfully authenticates or cancels. One can find out if the authentication 
succeeded by testing the `IsAuthenticated` property of `eventArgs`:

All the information gathered from a successful authentication is available in 
`eventArgs.Account`.

To capture events and information in the OAuth flow simply subscribe to Authenticator
events (add event handlers):

Xamarin.Android

```csharp
auth.Completed += (sender, eventArgs) => 
{
    // UI presented, so it's up to us to dimiss it on Android
    // dismiss Activity with WebView or CustomTabs
    this.Finish();

    if (eventArgs.IsAuthenticated) 
    {
        // Use eventArgs.Account to do wonderful things
    } else 
    {
        // The user cancelled
    }
};
```

Xamarin.iOS

```csharp
auth.Completed += (sender, eventArgs) => 
{
    // UI presented, so it's up to us to dimiss it on iOS
    // dismiss ViewController with UIWebView or SFSafariViewController
    this.DismissViewController (true, null);

    if (eventArgs.IsAuthenticated) 
    {
        // Use eventArgs.Account to do wonderful things
    } else 
    {
        // The user cancelled
    }
};
```

## 2. Create Login UI and authenticate user

Creating/Launching UI is platform specific and while authenticators manage their own UI, 
it's up to user to initially present the authenticator's UI on the screen. This lets one 
control how the authentication UI is displayed - modally, in navigation controllers, in 
popovers, etc.

### 2.1 Creating Login UI 

Now, the login UI can be obtained using `GetUI()` method and afterwards login screen is 
ready to be presented.  

The `GetUI()` method returns: 

*   `UINavigationController` on iOS, and 
*   `Intent` on Android.  
*   `System.Type` on WinRT (Windows 8.1 and Windows Phone 8.1)    
*   `System.Uri` on Windows Phone 8.x Silverlight

Android:

```csharp
global::Android.Content.Intent ui_object = Auth1.GetUI(this);
```

iOS:

```csharp
UIKit.UIViewController ui_object = Auth1.GetUI();
```

### 2.2 Customizing the UI - Native UI [OPTIONAL]

Some users will want to customize appearance of the Native UI (Custom Tabs on Android and/or 
SFSafariViewController on iOS) there is extra step needed - cast to appropriate type, so the   
API can be accessed (more in Details).


## 3 Present/Launch the Login UI

This step is platform specific and it is almost impossible to share it across platforms.

On Android, user would write the following code to present the UI.

```csharp
StartActivity (ui_object);  // ui_object is Android.Content.Intent
// or
StartActivity (auth.GetUI (this));
```

On iOS, one would present UI in following way (with differences from old API)

```csharp
PresentViewController(ui_object, true, null);
//or
PresentViewController (auth.GetUI ());
```


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


### 4.2 Store the account

Xamarin.Auth securely stores `Account` objects so that users don't always have to re-authenticate 
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


Creating `AccountStore` on iOS:

```csharp
// On iOS:
AccountStore.Create ().Save (eventArgs.Account, "Facebook");
```


Saved Accounts are uniquely identified using a key composed of the account's 
`Username` property and a "Service ID". The "Service ID" is any string that is 
used when fetching accounts from the store.

If an `Account` was previously saved, calling `Save` again will overwrite it. 
This is convenient for services that expire the credentials stored in the account 
object.


## 4.3 Retrieve stored accounts

Fetching all `Account` objects stored for a given service is possible with following API:

Retrieving accounts on Android:

```csharp
// On Android:
IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("Facebook");
```


Retrieving accounts on iOS:

```csharp
// On iOS:
IEnumerable<Account> accounts = AccountStore.Create ().FindAccountsForService ("Facebook");
```

It's that easy.

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
 
Binary form is deployable as a NuGet from nuget.org or Xamarin Component 
from component store:

*	NuGet 
*	Component [UPDATE IN PROGRESS]

More details about how to compile Xamarin.Auth library and samples can be found in the docs
in repository on GitHub.


