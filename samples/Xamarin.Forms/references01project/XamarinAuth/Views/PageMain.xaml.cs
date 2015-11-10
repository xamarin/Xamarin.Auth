using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Auth.SampleData;

namespace XamarinAuth.Views
{
	public partial class PageMain : ContentPage
	{
		public PageMain ()
		{
			InitializeComponent ();
		}



		Page page = null;

		
		public void OnItemSelected (object sender, ItemTappedEventArgs args_tapped)
		{
			object item = args_tapped.Item;

			KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

			string authentication_provider = kvp.Value;


			switch (authentication_provider)
			{
				case "Google OAuth2":
					Data.SetPublicDemoDataGoogleOAuth2();
					Data.SetSensitiveDataGoogleOAuth2();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth2);

					break;
				case "Facebook OAuth2":
					Data.SetPublicDemoDataFacebookOAuth2();
					Data.SetSensitiveDataFacebookOAuth2();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth2);

					break;
				case "Twitter OAuth1":
					Data.SetPublicDemoDataTwitterOAuth1();
					Data.SetSensitiveDataTwitterOAuth1();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth1);

					break;
				case "Microsoft Live OAuth2":
					Data.SetPublicDemoDataMicrosoftLiveOAuth2();
					Data.SetSensitiveDataMicrosoftLiveOAuth2();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth2);

					break;
				case "Instagram OAuth2":
					Data.SetPublicDemoDataInstagramOAuth2();
					Data.SetSensitiveDataInstagramOAuth2();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth2);

					break;
				case "LinkedIn OAuth1":
					Data.SetPublicDemoDataLinkedInOAuth1();
					Data.SetSensitiveDataLinkedInOAuth1();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth1);

					break;
				case "LinkedIn OAuth2":
					Data.SetPublicDemoDataLinkedInOAuth2();
					Data.SetSensitiveDataLinkedInOAuth2();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth2);

					break;
				case "Github OAuth2":
					SetPublicDemoDataGithubOAuth2();
					SetSensitiveDataGithubOAuth2();

					page = new Xamarin.XamarinForms.Authentication.PageLogin(oauth2);

					break;
				default:
					string msg = "Unknown Authentication Provider: " + authentication_provider;
					throw new NotImplementedException(msg);
			}

			this.Navigation.PushAsync(page);

			return;

		}

	}
}

