# Google Setup

WORK IN PROGRESS - CONSTRUCTION_SITE/BAUSTELLE

https://developers.google.com/identity/protocols/OAuth2InstalledApp#choosingredirecturi
https://developers.google.com/identity/protocols/OAuth2InstalledApp#request-parameter-redirect_uri


https://developers.google.com/api-client-library/python/auth/installed-app#Configuring%20the%20client%20object

The URI urn:ietf:wg:oauth:2.0:oob is a special URI used to identify out-of-browser 
applications, i.e. non-web applications (desktop, mobile, command line, etc.).

When you create the credentials in the APIs Console, make sure you select 
"Installed Application" as the application type and the redirect URI will automatically 
be set as 

    urn:ietf:wg:oauth:2.0:oob

and prevent the "redirect_uri_mismatch" when making a request.

    com.xamarin.xamarin-auth-test:/oauth2redirect

'redirect_uri' value of the Token request need to be the same as the 'redirect_uri' value 
of the Authorization request.

must have the 'Platform' set to 'Native (Windows Mobile, Blackberry, desktop, devices, and more)' 
when registering your app in the Google Cloud Console, otherwise, it will not let you use 
'urn:ietf:wg:oauth:2.0:oob' as the redirect URI.




Turns out the above code is correct. My issue was with setting the custom url scheme in my 
info.plist. The url scheme needs to be the bundle id only(i.e. com.example.myexample). 
I had the ':/oauth2callback' appended to the end of it, which is incorrect.


https://plus.google.com/+NaveenAgarwal/posts/AztHNnQh7w6


https://github.com/doorkeeper-gem/doorkeeper/issues/514




	Technology = Traditional.Standard

        Sample = Providers

            Android     
                com.xamarin.traditional.standard.samples.oauth.providers.android
                1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn.apps.googleusercontent.com

				redirect_url[s]
				{
				com.xamarin.traditional.standard.samples.oauth.providers.android:/oauth2redirect		
				com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn:/oauth2redirect		
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				
				
            iOS 
                com.xamarin.traditional.standard.samples.oauth.providers.ios
                1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh.apps.googleusercontent.com
            
				redirect_url[s]
				{
				com.xamarin.traditional.standard.samples.oauth.providers.ios:/oauth2redirect		
				com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh:/oauth2redirect
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				

	Technology =  Xamarin.Forms 

        Sample = Samples.NativeUI

            Android     
                com.xamarin.xamarinforms.samples.oauth.nativeui.android         
                1093596514437-og84g9cig4h3gn09ju12oqd84svs4u8f.apps.googleusercontent.com       

				redirect_url[s]
				{
				com.xamarin.xamarinforms.samples.oauth.nativeui.android:/oauth2redirect		
				com.googleusercontent.apps.1093596514437-og84g9cig4h3gn09ju12oqd84svs4u8f:/oauth2redirect
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				
				
            iOS         
                com.xamarin.xamarinforms.samples.oauth.nativeui.ios         
                1093596514437-7o2bm07prpmuf8c5qgs5bnik3saiafe0.apps.googleusercontent.com       

				redirect_url[s]
				{
				com.xamarin.xamarinforms.samples.oauth.nativeui.ios:/oauth2redirect		
				com.googleusercontent.apps.1093596514437-7o2bm07prpmuf8c5qgs5bnik3saiafe0:/oauth2redirect
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				
				
		Sample = Evolve16Labs.ComicBook		
		
            Android     
                com.xamarin.xamarinforms.samples.oauth.evolve16labs.comicbook.android       
                1093596514437-dbvffhvihnst5j2ujtn86a26g5cbf60k.apps.googleusercontent.com       
				
				
				redirect_url[s]
				{
				com.xamarin.xamarinforms.samples.oauth.evolve16labs.comicbook.android:/oauth2redirect		
				com.googleusercontent.apps.1093596514437-dbvffhvihnst5j2ujtn86a26g5cbf60k:/oauth2redirect
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				
				
            iOS         
                com.xamarin.xamarinforms.samples.oauth.evolve16labs.comicbook.ios              
                1093596514437-5f7295ts2k1ic7r082ufralpj28eb1bj.apps.googleusercontent.com       
				
				redirect_url[s]
				{
				com.xamarin.xamarinforms.samples.oauth.evolve16labs.comicbook.ios:/oauth2redirect		
				com.googleusercontent.apps.1093596514437-5f7295ts2k1ic7r082ufralpj28eb1bj:/oauth2redirect
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				
				
                
		Sample = Providers

			Android     
				com.xamarin.xamarinforms.samples.oauth.providers.android        

				redirect_url[s]
				{
				com.xamarin.xamarinforms.samples.oauth.providers.android:/oauth2redirect		
				com.googleusercontent.apps.
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				
				
			iOS         
				com.xamarin.xamarinforms.samples.oauth.providers.ios        

				
				redirect_url[s]
				{
				com.xamarin.xamarinforms.samples.oauth.providers.ios:/oauth2redirect		
				com.googleusercontent.apps.
				urn:ietf:wg:oauth:2.0:oob
				urn:ietf:wg:oauth:2.0:oob:auto
				http://localhost:PORT
				https://localhost:PORT
				http://127.0.0.1:PORT
				https://127.0.0.1:PORT				
				http://[::1]:PORT 
				https://[::1]:PORT 
				}				
				