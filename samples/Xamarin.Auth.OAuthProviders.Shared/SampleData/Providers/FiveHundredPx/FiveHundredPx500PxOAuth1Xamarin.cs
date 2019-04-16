using System;
using System.Threading.Tasks;

namespace Xamarin.Auth.ProviderSamples
{
	public partial class FiveHundredPx500PxOAuth1Xamarin : Helpers.OAuth1
    {
        partial void SetPublicNonSensitiveData();
        partial void SetPrivateSensitiveData();

		public FiveHundredPx500PxOAuth1Xamarin()
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
Consumer Key: TS6iuH5csR2GIiIGlUhR1iyJsrPZSYkLVOhu9NYt
Consumer Secret: zDIVU9fXE3a6Ntoy4iMi0ZXsoI2nGF8GHjCcJW0f
JavaScript SDK Key: 330e8a22ca929e2316de224231c6dca5f79a2a31
Description: Xamarin.Auth http://xamarin.com
Application URL: http://xamarin.com
Callback URL: http://xamarin.com
JavaScript SDK Callback URL:
Support URL: http://xamarin.com
Developer's Email: 
			";

            return;
        }

    }
}

