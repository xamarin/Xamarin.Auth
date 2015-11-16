using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xamarin.Auth.XamarinForms.Views
{
	public partial class PageMain : ContentPage
	{
		public PageMain ()
		{
			InitializeComponent ();
		}


		Xamarin.Auth.Helpers.OAuth oauth = null;

		//---------------------------------------------------------------------------------
		// methods implemented in other file which is included in project through wildcard
		// in this case files are in folder
		//			SecureSensitivePrivateData/
		//	patterns
		//			*.secure.cs
		//			*.sensitive.cs
		//			*.sensitive.cs
		//
		//		<Compile Include="SecureSensitivePrivateData/*.secure.cs" />
	    //		<Compile Include="SecureSensitivePrivateData/*.secret.cs" />
    	//		<Compile Include="SecureSensitivePrivateData/*.sensitive.cs" />
		//
		// csproj include

		// samples for docs
		partial void SetPublicDemoDataGoogleOAuth2();
		partial void SetPublicDemoDataFacebookOAuth2();
		partial void SetPublicDemoDataGithubOAuth2();
		partial void SetPublicDemoDataTwitterOAuth1();
		partial void SetPublicDemoDataMicrosoftLiveOAuth2();
		partial void SetPublicDemoDataInstagramOAuth2();
		partial void SetPublicDemoDataLinkedInOAuth1();
		partial void SetPublicDemoDataLinkedInOAuth2();

		// real data (presonal, hidden, secret, sensitive)
		// in csproj included as wildcard:
		//			    <Compile Include="Data\SecurePrivateSecretHidden\*.cs" />
		// data set in PublicDemo is overriden in those methods
		partial void SetSensitiveDataGoogleOAuth2();
		partial void SetSensitiveDataFacebookOAuth2();
		partial void SetSensitiveDataGithubOAuth2();
		partial void SetSensitiveDataTwitterOAuth1();
		partial void SetSensitiveDataMicrosoftLiveOAuth2();
		partial void SetSensitiveDataInstagramOAuth2();
		partial void SetSensitiveDataLinkedInOAuth1();
		partial void SetSensitiveDataLinkedInOAuth2();
		//---------------------------------------------------------------------------------

		Page page = null;

		Xamarin.Auth.Helpers.OAuth1 oauth1 = null;
		Xamarin.Auth.Helpers.OAuth2 oauth2 = null;
		
		public void OnItemSelected (object sender, ItemTappedEventArgs args_tapped)
		{
			object item = args_tapped.Item;

			KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

			string authentication_provider = kvp.Value;

			oauth1 = null;
			oauth2 = null;

			switch (authentication_provider)
			{
				case "Google OAuth2":
					SetPublicDemoDataGoogleOAuth2();
					SetSensitiveDataGoogleOAuth2();

					page = new PageLogin(oauth2);

					break;
				case "Facebook OAuth2":
					SetPublicDemoDataFacebookOAuth2();
					SetSensitiveDataFacebookOAuth2();

					page = new PageLogin(oauth2);

					break;
				case "Twitter OAuth1":
					SetPublicDemoDataTwitterOAuth1();
					SetSensitiveDataTwitterOAuth1();

					page = new PageLogin(oauth1);

					break;
				case "Microsoft Live OAuth2":
					SetPublicDemoDataMicrosoftLiveOAuth2();
					SetSensitiveDataMicrosoftLiveOAuth2();

					page = new PageLogin(oauth2);

					break;
				case "Instagram OAuth2":
					SetPublicDemoDataInstagramOAuth2();
					SetSensitiveDataInstagramOAuth2();

					page = new PageLogin(oauth2);

					break;
				case "LinkedIn OAuth1":
					SetPublicDemoDataLinkedInOAuth1();
					SetSensitiveDataLinkedInOAuth1();

					page = new PageLogin(oauth1);

					break;
				case "LinkedIn OAuth2":
					SetPublicDemoDataLinkedInOAuth2();
					SetSensitiveDataLinkedInOAuth2();

					page = new PageLogin(oauth2);

					break;
				case "Github OAuth2":
					SetPublicDemoDataGithubOAuth2();
					SetSensitiveDataGithubOAuth2();

					page = new PageLogin(oauth2);

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

