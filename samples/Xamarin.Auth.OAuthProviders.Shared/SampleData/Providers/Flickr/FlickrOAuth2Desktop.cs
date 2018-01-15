using System;

namespace Xamarin.Auth.ProviderSamples
{
    public partial class FlickrOAuth2Desktop : Helpers.OAuth2
    {
        partial void SetPublicNonSensitiveData();
        partial void SetPrivateSensitiveData();

        public FlickrOAuth2Desktop()
        {
            SetPublicNonSensitiveData();
            SetPrivateSensitiveData();

            return;
        }

        partial void SetPublicNonSensitiveData()
        {
            Description = "Flickr OAuth2 App Type = Desktop";
            OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
            OAuth_SecretKey_ConsumerSecret_APISecret = "";
            OAuth_UriCallbackAKARedirect = new Uri("https://xamarin.com");
            AllowCancel = true;

            HowToMarkDown =
@"
Flickr Server side setup (3 App types). Seems like all are the same - use Authorization Code Flow

*   Desktop

    No Options only Key and Secret

*   Web 

    CallbackUrl (Key and Secret)

*   Mobile

    *   Mobile Permissions (Read, Write, Delete)

    *   authentication URL (provided by flickr)

    
```
    App Type
        Web Application   Desktop Application   Mobile Application
 
   Callback URL
        https://xamarin.com
    This is where we'll send people after they authenticate.


    Mobile Permissions
        Read   Write   Delete
    
    Your authentication URL is https://www.flickr.com/auth-72157689423725242

```


";

            return;
        }
    }
}

