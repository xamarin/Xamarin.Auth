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

2.  Creating and optionally customising UI      

3.  Presenting/Lunching UI and authenticating user	

	1.	Detecting/Fetching/Intercepting URL change - redirect_url and  

		This substep is step needed for NativeUI and requires that custom scheme
		registration together for redirect_url intercepting mechanism. This step	
		is actually App Linking (Deep Linking) concept in mobile applications.

    2.	Parsing OAuth data from redirect_url

		In order to obtain OAuth data returned redirect_url must be parsed and the	
		best way is to let Xamarin.Auth do it automatically by parsing redirect_url 
		
	3.	Triggering Events based on OAuth data 
	
		Parsing subsytem of the Authenticator will parse OAuth data and raise	
		appropriate events based on returned data

4.  Using identity 

	1.	Using protected resources (making calls)	
	
	2.	Saving account info
	
	3.	Retrieving account info
  

  
Those steps and (substeps) which will be used in detailed documentation. 
See [./Details.md](./Details.md).


## 0 Server Side 

Server side setup of the particular OAuth provider like Google, Facebook or Microsoft Live
is the source of misunderstandings and errors. This setup differs from provider to provider,
especially nomenclature (naming).

In general there are 2 common types of "apps", "projects" or "credentials":

1.  Server or Web Application

    Server (Fitbit naming) or Web (Google and Facebook terms) application is considered to be 
    secure, i.e. client_secret is secure and can be stored and not easily accessed/retrieved 
    by malicious user.
    
    Server/Web app uses http[s] schemes for redirect_url, because it loads real web page 
    (url-authority can be localhost or real hostname like http://xamarin.com).
    
    Xamarin.Auth prior to version 1.4.0 used to support only http[s] url-scheme with real   
    url-authority (existing host, no localhost) and arbitrary url-path. 
    
2.  Native or Installed (mobile or desktop) apps    
    
    This group is usually divided into Android, iOS, Chrome (javascript) and other (.net)   
    subtypes. Each subtype can have different setup. In most cases developer must submit    
    for Android package id with SHA1 key and for iOS BundleID. Custom schemes can be predefined 
    (generated) by provider (Google or Facebook) or defined by user (Fitbit). Generated schemes     
    are usually based on data submitted (package id, bundle id).
    
    Xamarin Components Team is working on the doc with minimal info for common used providers 
    and how to setup server side.
    
    Xamarin.Auth implements requirements for native/installed apps since nuget version 1.4.0, 
    but the API was broken (`GetUI()` returned `System.Object`, so cast was necessary).

Server side setup details is explained in separate documents in Xamarin.Auth repository. 
See (./details/setup-server-side-oauth-provider.md)[./details/setup-server-side-oauth-provider.md].  



## 1 Client Side Initialization

Client (mobile) application initialization is based on Oauth Grant (flow) in use which is determined 
by OAuth  provider and it's server side setup.

Initialization is performed thorugh `Authenticator` constructors for:

### OAuth2 Implicit Grant flow 

With parameters:    

    *   clientId        
    *   scope       
    *   authorizeUrl        
    *   redirectUrl     
    
[TODO Link to code]
    
### OAuth2 Authorization Code Grant flow 

With parameters:       

    *   clientId   	
    *   clientSecret	
	*	scope       
    *   authorizeUrl  	      
    *   redirectUrl 	
    *   accessTokenUrl	

[TODO Link to code]
    
    
OAuth details and how Xamarin.Auth implements OAuth is described in documentation in
Xamarin.Auth repo.
See [./details/oauth.md](./details/oauth.md)


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

[TODO Link to code]

Facebook uses OAuth 2.0 authentication, so we create an `OAuth2Authenticator`. 
Authenticators are responsible for managing the user interface and communicating with 
authentication services.

Authenticators take a variety of parameters; in this case, the application's ID, its 
authorization scope, and Facebook's various service locations are required.


### 1.2 Setup Authentication Event Handlers

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

[TODO Link to code]


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

[TODO Link to code]

## 2. Create Login UI and authenticate user

Creating/Launching UI is platform specific and while authenticators manage their own UI, 
it's up to user to initially present the authenticator's UI on the screen. This lets one 
control how the authentication UI is displayed - modally, in navigation controllers, in 
popovers, etc.

Before the UI is presented, user needs to start listening to the `Completed` event which fires 
when the user successfully authenticates or cancels. One can find out if the authentication 
succeeded by testing the `IsAuthenticated` property of `eventArgs`:

All the information gathered from a successful authentication is available in 
`eventArgs.Account`.

### 2.1 Creating Login UI 

Now, the login UI can be obtained using `GetUI()` method and afterwards login screen is 
ready to be presented.  

The `GetUI()` method returns: 

*   `UINavigationController` on iOS, and 
*   `Intent` on Android.  
*   `System.Type` on WinRT (Windows 8.1 and Windows Phone 8.1)    
*   `Syste.Uri` on Windows Phone 8.x Silverlight

Android:

```csharp
global::Android.Content.Intent ui_object = Auth1.GetUI(this);
```

[TODO Link to code]

iOS:

```csharp
UIKit.UIViewController ui_object = Auth1.GetUI();
```

[TODO Link to code]

### 2.2 Customizing the UI - Native UI [OPTIONAL]

Some users will want to customize appearance of the Native UI (Custom Tabs on Android and/or 
SFSafariViewController on iOS) there is extra step needed - cast to appropriate type, so the   
API can be accessed (more in Details).


## 3 Present/Launch the Login UI

This step is platform specific and it is almost impossible to share it accross platforms.

On Android, user would write the following code to present the UI.

```csharp
StartActivity (ui_object);  // ui_object is Android.Content.Intent
// or
StartActivity (auth.GetUI (this));
```

[TODO Link to code]

On iOS, one would present UI in following way (with differences fromold API)

```csharp
PresentViewController(ui_object, true, null);
//or
PresentViewController (auth.GetUI ());
```

[TODO Link to code]

On Windows [TODO] 


## 4 Using identity 

### 4.1 Making requests to protected resources

With obtained access_token (identity) user can now access protected ressources.

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
 
Binary form is deployable as nuget from nuget.org or Xamarin Component 
from component store:

*	nuget 
*	Component [UPDATE INPROGRESS]


More details about how to compile Xamarin.Auth library and samples can be found in the docs
in repository on github.
See [./details/installation-and-compilation.md](./details/installation-and-compilation.md).
