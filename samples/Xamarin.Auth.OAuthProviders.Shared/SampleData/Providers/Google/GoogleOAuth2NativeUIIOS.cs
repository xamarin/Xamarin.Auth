using System;

namespace Xamarin.Auth.ProviderSamples
{
    public partial class GoogleOAuth2NativeUIIOS : Helpers.OAuth2
    {
        partial void SetPublicNonSensitiveData();
        partial void SetPrivateSensitiveData();

        public GoogleOAuth2NativeUIIOS()
        {
            SetPublicNonSensitiveData();
            SetPrivateSensitiveData();

            return;
        }

        partial void SetPublicNonSensitiveData()
        {
            ProviderName = "Google";
            OrderUI = "1";
            Description = "Google OAuth2 NativeUI iOS";
            OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
            OAuth2_Scope = "https://www.googleapis.com/auth/userinfo.email";
            OAuth_UriAuthorization = new Uri("https://accounts.google.com/o/oauth2/auth");
            OAuth_UriAccessToken_UriRequestToken = new Uri("https://www.googleapis.com/oauth2/v4/token");

            this.OAuth_UriCallbackAKARedirectPort =
                                    80
                                    //8080
                                    ;
            this.OAuth_UriCallbackAKARedirectPath =
                                    "/oauth2redirect"
                                    ;

            OAuth_UriCallbackAKARedirect =
                new Uri
                    (
                        // cannot be used with installed apps since 2017-04-20
                        // $"http://xamarin.com"	
                        $"com.xamarin.traditional.standard.samples.oauth.providers.ios:{OAuth_UriCallbackAKARedirectPath}"
                    //$"com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn:{this.OAuth_UriCallbackAKARedirectPath}"		
                    //$"urn:ietf:wg:oauth:2.0:oob"
                    //$"urn:ietf:wg:oauth:2.0:oob:auto"
                    //$"http://localhost:{this.Port}"
                    //$"https://localhost:{this.Port}"
                    //$"http://127.0.0.1:{this.Port}"
                    //$"https://127.0.0.1:{this.Port}"				
                    //$"http://[::1]:{this.Port}"
                    //$"https://[::1]:{this.Port}" 
                    );
            AllowCancel = true;
            HowToMarkDown =
@"
https://developers.google.com/identity/protocols/OAuth2InstalledApp#request-parameter-redirect_uri

";

            return;
        }

    }
}

