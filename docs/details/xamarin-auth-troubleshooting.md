# Xamarin.Auth Troubleshooting 

## App 


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

### Analysis

TODO

### [Re]Solution/Workaround

1.	simply update Xamarin.Android.Support nuget packages to 25.0.0+ version

2.	turn off linker

Xamarin.Auth lowest pinned) versionWhen Xamarin.Forms support was added to Xamarin.Auth in 2017-05 the 
Xamarin.Android.Support libraries were pinned to 23.0.0 version in Xamarin.Forms.


