# iOS SafariViewController

SFSafariViewController offer seamles browser integration for iOS applications. It
provides secure browser experience through:

*	process separation  	
	
	Users' applications and web browsing are separated in 2 processes which incereases
	security. Anything done on the web by the user will not have impact on the app 
	itself. Browsing to malicious page will not have impact on the app and its data.
	
*	Indicators that browsing is done over secure connections

*	Security Warnings for registered malicious web pages

*	standard browser features:

	*	form auto-fill		

	*	ApplePay integration

	*	access to

		*	Share Sheet (Share Menu)
		
		*	System Activity
		
		*	3rd part extensions for Safari		
		
*	iOS 11 features:

	*	Drag n Drop

	*	StatusBar customisations
	
		Use UIViewContorller status bar appearence
		
		In Info.plist add:

```xml
	UIViewContorllerBasedStatusBarAppearnce = yes
```

	*	Other Views can be presented in SafariViewController
		
		Messages Extension and Full Screen Video

	*	support for `window.open()`	
	
	*	Privacy 
	
		*	browsing the web under different accounts from different apps
		
		*	different apps have different persistent stores (cookies, cache ...)
		
		*	prevention of cross app tracking
		
		*	no automatic login to whatever account found
		
		*	private browsing improvements
		
		*	new APIs
		
			*	Dismiss Button
			
				In addition to `Done` - `Cancel` and `Close` added.
			
			*	Share Sheet
			
			*	Bar Collapsing
			
				Bar Collapsing can be disabled - ideal for OAuth flows.
				
			*	
			
			
## References


*	https://developer.apple.com/documentation/safariservices/sfsafariviewcontroller
*	https://developer.apple.com/videos/play/wwdc2015/504/
*	https://developer.apple.com/videos/play/wwdc2017/225/
*	http://developer.outbrain.com/ios-best-practices-for-opening-a-web-page-within-an-app/
			