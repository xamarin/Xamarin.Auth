using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.SampleData
{
    public partial class YammerOAuth2 : Xamarin.Auth.Helpers.OAuth2
    {
        partial void SetPublicNonSensitiveData();
        partial void SetPrivateSensitiveData();

		public YammerOAuth2()
        {
            SetPublicNonSensitiveData();
            SetPrivateSensitiveData();

            return;
        }

        partial void SetPublicNonSensitiveData()
        {
            Description = "Yammer OAuth2";
            OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
            OAuth2_Scope = ""; // 
			OAuth_UriAuthorization = new Uri("https://www.yammer.com/oauth2/authorize");
            OAuth_UriCallbackAKARedirect = new Uri("xamarin-auth://localhost");
            AllowCancel = true;
            HowToMarkDown =
            @"
Xamarin.Auth Tests xamarin-auth://localhost
Expected redirect ?     xamarin-auth://localhost
https://www.yammer.com/oauth2/authorize
			";

            return;
        }

    }
}

