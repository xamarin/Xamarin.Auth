# Embedded WebViews (Browser Controls) vs Native UI

embedded-webviews-aka-browser-controls-vs-native-ui.md


## TL;DR - Embedded WebViews vs Native UI

*   Embedded WebViews have bigger API surface 

    Bigger API surface means more opportunities for malicious software/users, from ability
    to decompile application, to ability to read loaded URL, manipulate cache and cookies.

*   Embedded WebViews have seamless UX

    Embedded WebViews usage does not leave application and launches external system browser,
    meaning user stays in the application.

*   Embedded WebViews do not require advanced programming techniques

    Leaving application to perform authentication in external browser means the callback
    (return to the application) from browser must be implemented. The only and proper 
    technique for that is called "Deep App Linking" (or just Deep Linking or App Linking).
    This technique is everything, but trivial, because developer needs to register callback
    custom schemes and implement scheme intercepting which is platform specific.

*   Native UI has 'reduced API surface'

    'Reduced API surface' means that in most cases (most meaning **all** - for Android 
	CustomTabs and iOS SFSafariWebView) developer is **not** able to:
    
    *   retrieve URL loaded in the (system, external) browser

    *   manipulate Cookies and Cache 

*   Native UI is based on maintained codebase of system browser

    *   Android - Chromium/Chrome browser
    *   iOS - Safari browser

*   Native UI is regularly updated by system updates (browser updates)

*   Native UI utilizes browser identity (password/account) management (built-in or as plugin)


## Native external system browser

*	Pros
	*	security
		*	proven codebase
		*	regular system updates
	*	use of browser features and API
		*	use of system 
			*	account store (might be implemented as plugin/extension)
			*	cookie store, so it is able to take advantage of active sessions
			*	chrome which can display important security information about loaded 
				page
		* 	plugins/extensions	
*	Cons		
	*	external process - app logic is leaving app and switching to system browser
		*	custom schemes used	
			*	additional coding needed		
			*	debugging could be non-trivial	
	*	version problems (browser versions)		
	*	fragmentation (Android)		
		4 versions of Chrome Browser, Opera, Firefox, Samsung browsers that support 		
		CustomTabs
			


## 
		
## Details 


In 2016-08 (2016-06-22) on Google's develoers' blog Google announced ban for OAuth 
implementations that use Embedded WebViews (Browser Controls on Windows platforms) for 
security reasons.

https://developers.googleblog.com/2016/08/modernizing-oauth-interactions-in-native-apps.html

The document is available here:

https://developers.google.com/identity/protocols/OAuth2InstalledApp#request-parameter-redirect_uri

NOTE: 2017-05-20 The document it is changing day by day a lot of information not available 2-3
weeks ago have made it to the doc.

## Remarks

### `redirect_url`




## TL;DR

Few facts for Google:

*   Google generates custom scheme[s] for `redirect_url` 

    *   based on app info which is provided to Google in developer console:

        *   Android package name (from AndroidManifest.xml)     

            In Visual Studio and Xamarin.Studio go to 
            
            Android App +/ Project Options/Properties +/ Android Application +/ Package name

            and copy the value for Package name (something like `com.xamarin.comicbook`)

        *   iOS Bundle Identifier
        
            iOS App +/ Open Info.plist +/ Identity +/ Bundle Identifier

            and copy the value for Bundle Identifier (something like `com.xamarin.comicbook`)

### Facebook

https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow

Security
    Server IP Whitelist

    App requests using the app secret must originate from these IP addresses.
    Update Settings IP Whitelist

    App Settings can only be updated from these IP addresses.

    Require App Secret
    Only allow calls from a server and require app secret or app secret proof for all API calls.

Facebook login

    Settings

    Client OAuth Settings
YesNo
Client OAuth Login
Enables the standard OAuth client token flow. Secure your application and prevent abuse by locking down which token redirect URIs are allowed with the options below. Disable globally if not used.

https://developers.facebook.com/docs/facebook-login/security/#surfacearea

Disable Client OAuth Login if your app does not use it. Client OAuth Login is the global on-off switch for using OAuth client token flows. If your app does not use any client OAuth flows, which include Facebook login SDKs, you should disable this flow. Note, though, that you can't request permissions for an access token if you have Client OAuth Login disabled. This setting is found in the Products > Facebook Login > Settings section.


YesNo   Web OAuth Login
Enables web based OAuth client login for building custom login flows.

https://developers.facebook.com/docs/facebook-login/security/#surfacearea

Disable Web OAuth Flow or Specify a Redirect Whitelist. Web OAuth Login settings enables any OAuth client token flows that use the Facebook web login dialog to return tokens to your own website. This setting is in the Products > Facebook Login > Settings section. Disable this setting if you are not building a custom web login flow or using the Facebook Login SDK on the web.

When this setting is enabled you are required to specify a list of OAuth redirect URLs. Specify an exhaustive set of app URLs that are the only valid redirect URLs for your app for returning access tokens and codes from the OAuth flow.



YesNo
Force Web OAuth Reauthentication
When on, prompts people to enter their Facebook password in order to log in on the web.


YesNo
Embedded Browser OAuth Login
Enables browser control redirect uri for OAuth client login.

Disable embedded browser OAuth flow if your app does not use it. Some desktop and mobile native apps authenticate users by doing the OAuth client flow inside an embedded webview. If your app does not do this, then disable the setting in Products > Facebook Login > Settings section.


https://github.com/tschellenbach/Django-facebook/issues/376




### iOS

1. Download and Install the Facebook SDK for iOS

    No need. Xamarin.Auth is used.

2. Add Login Kit to your Xcode Project

    Not needed.

3. Add your Bundle Identifier

Bundle ID
You can change your bundle identifier in the future via the iOS section on the settings page.
com.xamarin.comicbook.iosRemove

4. Enable Single Sign On for Your App

Enable Single Sign On
Enable single sign on for your app by setting Single Sign On to Yes below.
YesNo
Single Sign On
Will launch from iOS Notifications

5. Configure Your info.plist

 Configure Your info.plist
Find the .plist file in the Supporting Files folder in your Xcode Project.
Right-click your .plist file and choose "Open As Source Code".
Copy and paste the following XML snippet into the body of your file ( <dict>...</dict>).


<key>CFBundleURLTypes</key>
<array>
  <dict>
  <key>CFBundleURLSchemes</key>
  <array>
    <string>fb1889013594699403</string>
  </array>
  </dict>
</array>
<key>FacebookAppID</key>
<string>1889013594699403</string>
<key>FacebookDisplayName</key>
<string>Xamarin.Auth.Native.ComicBk</string>


6. Connect App Delegate





## Detailed Recap

To recap and add some information not available in the doc. Some information is scattered in 
sample repos on github and in other docs.

The key point of Native UI is security, though Google mentions USability too. Web app (or some call 
it Server app) is considered to be secure. Namely it takes more to get to the filesystem of the 
server (well maintained) than to some random user smartphone - and to retrieve app id and `client_secret` 
(more important). 
Web/server apps usually use Authorization Grant flow (2 steps, some call it explicit) and in 
2nd step `client_secret` is sent.

Native apps (Installed apps, though some differ mobile and desktop) are considered to be insecure, 
because client_secret (and other data) cannot be stored securely (decompiling or anything). User doesn’t 
even need to lose your smartphone it can be in the hands of users less skilled in security - thus insecure.

Web/Server apps have redirect_url with http[s] scheme, because after login you (as web admin) 
would redirect user back to your web app to parse OAuth data and present something after 
successful login (might be data from database).

We “faked” server with Xamarin.Auth for long time (check redirect_url from forums for FB 
http://facebook.com/blahblah/login_succes.html
I used http[s]://xamarin.com or whatever…


Error: invalid_request

Custom scheme URIs are not allowed for WEB client type.

