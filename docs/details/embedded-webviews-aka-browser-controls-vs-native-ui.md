# Embedded WebViews (Browser Controls) vs Native UI

embedded-webviews-aka-browser-controls-vs-native-ui.md

In 2016-08 (2016-06-22) on Google's develoers' blog Google announced ban for OAuth implementations
that use Embedded WebViews (Browser Controls on Windows platforms) for security reasons.

https://developers.googleblog.com/2016/08/modernizing-oauth-interactions-in-native-apps.html


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

