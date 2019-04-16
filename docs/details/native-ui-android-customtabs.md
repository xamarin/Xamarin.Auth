# Android CustomTabs (Chrome Custom Tabs)

## Features

*   possible to save up to 700 ms when opening a link with the CustomTabs by 
    connecting to the service and pre-loading Chrome.

    The loading happens as a low priority process, meaning that it won’t have any 
    negative performance impact on the application, but will give a big performance 
    boost when loading a link.    

    *   Connect to the Custom Tabs service on the `OnStart()` method of the Activities 
        planed to launch a CustomTab from
    *   Upon connection, call `Warmup()`

*   Calling `MayLaunchUrl()`

    Pre-fetching will make external content open instantly. So, if user has higher 
    likelihood of clicking on the link (let say mor than 50%), use the `MayLaunchUrl()` 
    method.

    OAuth is ideal for this, because OAuth Endpoints are well known in advance.

    Calling `MayLaunchUrl()` will make Custom Tabs pre-fetch the main page with the 
    supporting content and pre-render. This will give the maximum speed up to the page 
    loading process, but comes with a network and battery cost.

    Custom Tabs is smart and knows if the user is using the phone on a metered network or 
    if it’s a low end device and pre-rendering will have a negative effect on the overall 
    performance of the device and won’t pre-fetch or pre-render on those scenarios. 
    So, there’s no need to optimize your application for those cases.

## Low level API

Although the recommended method to integrate your application with Custom Tabs is using the 
Custom Tabs Support Library, a low level implementation is also available.

The complete implementation of the Support Library is available on Github and an be used 
as a start point. It also contains the AIDL files required to connect to the service, as the 
ones contained in the Chromium repository are not directly usable with Android Studio.

Basics for Launching Custom Tabs using the Low Level API

## Chrome Browser versions

During tests few cases were hit when CustomTabs did not close automatically.
Seems like Chrome Browser version installed.

APKs with different Chrome versions for installing on devices or emulators:

http://www.apkmirror.com/uploads/?q=chrome


https://developer.android.com/reference/android/support/customtabs/CustomTabsSession.html

## Prefetching

https://groups.google.com/a/chromium.org/forum/#!topic/prerender/OlOYzPhcL78


## Troubleshooting

Testing IntentFilter and Activity with 

1.	adb

```		
	export REDIRECT_URI="xamarin-auth://localhost/oauth2redirect"	
	adb shell am start -W \
		-a android.intent.action.VIEW \
		-c android.intent.category.DEFAULT \
		-d $REDIRECT_URI		
```	

2.	html

```html
<html>
	<href a="xamarin-auth://localhost/oauth2redirect">Test Custom Scheme</href>
</html>
```	

## References

*	https://developer.android.com/reference/android/support/customtabs/package-summary.html
*	https://developer.chrome.com/multidevice/android/customtabs
*	https://medium.com/google-developers/best-practices-for-custom-tabs-5700e55143ee
*	https://www.captechconsulting.com/blogs/an-introduction-to-chrome-custom-tabs-for-android
*	https://segunfamisa.com/posts/chrome-custom-tabs
