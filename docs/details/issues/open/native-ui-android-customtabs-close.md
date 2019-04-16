# Native UI Android - CustomTabs closing

## 

@nickl

https://xamarinchat.slack.com/archives/C4TD1NHPT/p1496735906461957

```
if anyone here will find this interesting, but on Android I’ve managed to get the 
custom tab to open (in the app) and close automatically after the authentication 
(for Google) has been received.

LaunchMode on my activity to SingleTask and NoHistory to false.
```


https://stackoverflow.com/questions/45350947/xamarin-auth-android-chrome-custom-tabs-doesnt-close-on-redirect

You can go back to your app if you add this code to the end of OnCreate method in 
the class that captures the Redirect (CustomUrlSchemeInterceptorActivity) in 
Xamarin.Auth example in Android

	new Task
	(
		() =>
		{
			 StartActivity(new Intent(Application.Context,typeof(MainActivity)));
		}
	).Start();
	
Where MainActivity is the name of your main Activity class in Android.
