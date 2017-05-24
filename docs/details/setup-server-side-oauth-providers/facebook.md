# Facebook Setup

WORK IN PROGRESS - CONSTRUCTION_SITE/BAUSTELLE

https://www.facebook.com/help/community/search/?query=oauth


    1.  url https://m.facebook.com/dialog/oauth/

        Not Logged In: You are not logged in. Please log in and try again.

    2.  url fb1889013594699403://localhost/path

        Can't Load Url: The domain of this URL isn't included in the app's domains.
        To be able to load this URL, add all domains and subdomains of your app to
        the App Domain field in your app settings. 

    2.  url fb1889013594699403://xamarin.com

        Can't Load Url: The domain of this URL isn't included in the app's domains.
        To be able to load this URL, add all domains and subdomains of your app to
        the App Domain field in your app settings. 

        https://stackoverflow.com/questions/37063685/facebook-oauth-the-domain-of-this-url-isnt-included-in-the-apps-domain
        https://stackoverflow.com/questions/37652221/facebook-login-cant-load-url-the-domain-of-this-url-isnt-included-in-the-app


App Domains add localhost

    App domains must match the domain of the Facebook Web Games URL (https), Mobile Site URL, 
    Unity Binary URL, Site URL or Secure Page Tab URL. Please correct these domains: 
    localhost        

web app

    cannot add custom scheme URL

Android

iOS



## Letter for Facebok Developers' Group


Xamarin.Auth is a cross platform library for authentication purposes with OAuth for Xamarin 
and Windows platforms. It does not use native OAuth providers' SDKs like Facebook SDK, but
plain http requests and responses.

Recently Xamarin.Auth was extended with implementation for Native UI support for Installed
apps (on Android CustomTabs and on iOS SFSafariViewController). Users can easily switch between
Embedded WebView (Browser control) and Native UI

In order to use Native UI Deep/App Linking must be implemented by the app to enable callbacks into the app by using 
custom scheme[s].

Google Server side OAuth generates custom schemes in 2 different ways:

1. based on Android's package name or iOS Bundle Id. (package.name:/oauth2redirect)
2. based on App Id (com.googleusercontent.apps.<appid>:/oauth2redirect)



	
	
	
 


and now there are few

 