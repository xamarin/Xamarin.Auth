using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class FiveHundredPx500PxOAuth1CustomSchemeLocalhost : Helpers.OAuth1
    {
        partial void SetPublicNonSensitiveData();
        partial void SetPrivateSensitiveData();

		public FiveHundredPx500PxOAuth1CustomSchemeLocalhost()
        {
            SetPublicNonSensitiveData();
            SetPrivateSensitiveData();

            return;
        }

        partial void SetPublicNonSensitiveData()
        {
            ProviderName = "500px";
            OrderUI = "5";
            Description = "500px OAuth1";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth1_SecretKey_ConsumerSecret_APISecret = "";
			OAuth_UriAuthorization = new Uri("");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			OAuth_UriAccessToken_UriRequestToken = new Uri("");
			AllowCancel = true;
			HowToMarkDown =
            @"
Consumer Key: yISR2TlMU0EpEAFUWQ4SC6UN7Dv81KvLiCk787Tl
Consumer Secret: UoLkwZILDloGxfTJYTKrWPysohQdSkllTGldVJ4v
JavaScript SDK Key: 65b350515e3993126760e1c39eec59dfee776cfd
Description: Xamarin.Auth xamarin-auth://xamarin.com
Application URL: http://xamarin.com
Callback URL: http://xamarin-auth://xamarin.com
JavaScript SDK Callback URL:
Support URL: http://xamarin.com
Developer's Email: 
			";

            return;
        }

    }
}

