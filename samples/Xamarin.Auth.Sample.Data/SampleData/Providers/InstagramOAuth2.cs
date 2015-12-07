using System;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	public partial class InstagramOAuth2 : Xamarin.Auth.Helpers.OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public InstagramOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
			HowToMarkDown = 
@"
 	https://instagram.com/developer/
	https://www.instagram.com/developer/clients/manage/
	Xamarin.Auth.Component
	CLIENT INFO
	CLIENT ID		5a0590cb807045d68cbb2459ab336869
	WEBSITE URL		http://xamarin.com
	REDIRECT URI	http://xamarin.com/
	SUPPORT EMAIL	None
	Sample for Xamarin.Auth component

";
			Description = "Instagram OAuth2";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";
			OAuth2_Scope = "basic";
			OAuth_UriAuthorization = new Uri("https://api.instagram.com/oauth/authorize/");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
			AllowCancel = true;

			return;
		}
	}
}

