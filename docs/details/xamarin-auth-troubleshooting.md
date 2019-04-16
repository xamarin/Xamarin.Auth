# Xamarin.Auth Troubleshooting 

## Control does not return to the app after login

If you are using NativeUI (SafariViewController and [Chrome] Custom Tabs) check 

1.	schema for `redirect_url` should not be http[s] schema!

	
2.	app-linking (deep-linking) implementation 

3.	on Android there have been reports that AntiVirus software might block
	IntentFilters.
	
	Reported by 
	
```bash
	adb shell am start \
		-a android.intent.action.VIEW \
		-d "com.googleusercontent.apps.<your-android-app-id>:/oauth2redirect"
```	

## The "LinkAssemblies" task failed unexpectedly - Failed to resolve - LaunchUrl

### Problem

Linking fails for Android applications.

	The "LinkAssemblies" task failed unexpectedly.
	Mono.Linker.MarkException: 
	Error processing method: 
		'System.Void Android.Support.CustomTabs.Chromium.SharedUtilities.CustomTabActivityHelper::LaunchUrlWithCustomTabsOrFallback(Android.App.Activity,Android.Support.CustomTabs.CustomTabsIntent,Android.Net.Uri,Android.Support.CustomTabs.Chromium.SharedUtilities.ICustomTabFallback)'
		in assembly: 'Xamarin.Auth.dll' ---> 
		Mono.Cecil.ResolutionException: 
		Failed to resolve System.Void Android.Support.CustomTabs.CustomTabsIntent::LaunchUrl(Android.Content.Context,Android.Net.Uri)
		
From github


> Confirm linker issues with Xamarin.Auth 1.5.0.3 + Xamarin.Android.Support.CustomTabs 23.4.0.1:

From MatkovIvan:

https://github.com/Azure/azure-mobile-apps-net-client/issues/361#issuecomment-315353068
https://github.com/Azure/azure-mobile-apps-net-client/issues/361
https://github.com/xamarin/Xamarin.Auth/issues/167


In Xamarin.Android.Support.CustomTabs version 23.3.0:

	[Register("launchUrl", "(Landroid/app/Activity;Landroid/net/Uri;)V", "")]
	public void LaunchUrl(Activity context, Uri url);

In Xamarin.Android.Support.CustomTabs version 25.3.1:

	[Register("launchUrl", "(Landroid/content/Context;Landroid/net/Uri;)V", "")]
	public void LaunchUrl(Context context, Uri url);

There is no info about it in change logs, since this is auto-generated from java code. 
But in Azure Mobile Client case this is breaking change.

This issue inherited from Xamarin.Auth. It uses CustomTabsIntent::LaunchUrl directly instead 
of CustomTabsActivityManager despite documentation of custom tabs.

PR to Azure Mobile Client Xamarin.Auth fork to fix this: 

https://github.com/Azure/azure-mobile-services-xamarin-auth#11

### Analysis

TODO

### [Re]Solution/Workaround

1.	simply update Xamarin.Android.Support nuget packages to 25.0.0+ version

2.	turn off linker

Xamarin.Auth lowest pinned) versionWhen Xamarin.Forms support was added to Xamarin.Auth in 2017-05 the 
Xamarin.Android.Support libraries were pinned to 23.0.0 version in Xamarin.Forms.

## unsuported_response_type error

http://stackoverflow.com/questions/23961734/how-to-respond-to-an-oauth-2-0-authorization-endpoint-request-that-fails-due-to
http://stackoverflow.com/questions/11696526/google-oauth-2-response-type-error-on-token-request


https://forums.xamarin.com/discussion/7551/x-auth-google-unsupported-response-type
https://forums.xamarin.com/discussion/comment/75297#Comment_75297


## Detecting Cancellation in CustomTabs on Android

Xamarin.Auth [curerntly] cannot handle certain events in CustomTabs, because there is no
such API provided from CustomTabs. Though this issue is under investigation for possible
solutions!

References/link for similar issue:

1.	AppAuth

	https://github.com/openid/AppAuth-Android/issues/102
	\#102 Detecting user initiated closing of the authorization flow. 

	https://github.com/openid/AppAuth-Android/pull/109
	\#109 Use an intermediary activity to manage the authorization flow 

	https://codelabs.developers.google.com/codelabs/appauth-android-codelab/#0
	
2.	StackOverflow

	https://stackoverflow.com/questions/41010017/callback-on-dismiss-of-chrome-custom-tabs
	
	
	