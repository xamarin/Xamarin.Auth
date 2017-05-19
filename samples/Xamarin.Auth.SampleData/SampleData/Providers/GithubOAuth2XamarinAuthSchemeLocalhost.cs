using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
    public partial class GithubOAuth2XamarinAuthSchemeLocalhost : Xamarin.Auth.Helpers.OAuth2
    {
        partial void SetPublicNonSensitiveData();
        partial void SetPrivateSensitiveData();

        public GithubOAuth2XamarinAuthSchemeLocalhost()
        {
            SetPublicNonSensitiveData();
            SetPrivateSensitiveData();

            return;
        }

        partial void SetPublicNonSensitiveData()
        {
            HowToMarkDown =
@"
https://github.com/settings/applications
https://developer.github.com/v3/oauth/#scopes
https://github.com/settings/developers
";
            Description = "Github OAuth2 xamarin-auth://localhost";
            OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
            OAuth2_Scope = ""; // "", "user",
            OAuth_UriAuthorization = new Uri("https://github.com/login/oauth/authorize");
            OAuth_UriAccessToken_UriRequestToken = new Uri("https://github.com/login/oauth/access_token");
            OAuth_UriCallbackAKARedirect = new Uri("xamarin-auth://localhost");
            AllowCancel = true;

            return;
        }
    }
}

