using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Xamarin.Forms;

using Xamarin.Auth;
using Xamarin.Auth.XamarinForms;

using Xamarin.Auth.SampleData;

namespace XamarinAuth.Views
{
    public partial class PageOAuthProviders : ContentPage
	{
        public PageOAuthProviders ()
		{
			InitializeComponent ();
		}

		Page page = null;
				
		string provider = null;

		public void OnItemSelected (object sender, ItemTappedEventArgs args_tapped)
		{
			object item = args_tapped.Item;

			KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

			string provider = kvp.Value;

			switch (provider)
			{
				case "Facebook OAuth2":
					Xamarin.Auth.Helpers.OAuth2 oauth_facebook = null;
					oauth_facebook = Data.TestCases [provider] as Xamarin.Auth.Helpers.OAuth2;
					AuthenticateOAuth2 (oauth_facebook);
					break;
				case "Twitter OAuth1":
                    Xamarin.Auth.Helpers.OAuth1 oauth_twitter = null;
                    oauth_twitter = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1;  
					AuthenticateOAuth1 (oauth_twitter);
                	break;
				case "Google OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_google = null;
                    oauth_google = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
					AuthenticateOAuth2 (oauth_google);
                    break;
				case "Microsoft Live OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_mslive = null;
                    oauth_mslive = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
					AuthenticateOAuth2 (oauth_mslive);
                    break;
				case "LinkedIn OAuth1":
                    Xamarin.Auth.Helpers.OAuth1 oauth_linkedin1 = null;
                    oauth_linkedin1 = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1;  
					AuthenticateOAuth1 (oauth_linkedin1);
                    break;
				case "LinkedIn OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_linkedin2 = null;
                    oauth_linkedin2 = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
					AuthenticateOAuth2 (oauth_linkedin2);
                    break;
				case "Github OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_github = null;
                    oauth_github = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
					AuthenticateOAuth2 (oauth_github);
                    break;
				case "Instagram OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_instagram = null;
                    oauth_instagram = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
					AuthenticateOAuth2 (oauth_instagram);
                    break;
				default:
					DisplayAlert ("Error", "Unknown OAuth Provider!", "OK");
					break;
			};

			this.Navigation.PushAsync(page);

			return;

		}

		public void AuthenticateOAuth2(Xamarin.Auth.Helpers.OAuth2 oauth2)
		{
			page = new PageOAuth
				(
					oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					oauth2.OAuth2_Scope,
					oauth2.OAuth_UriAuthorization,
					oauth2.OAuth_UriCallbackAKARedirect,
					null,
					true
				);

			Navigation.PushAsync (page);

			return;
		}

		public void AuthenticateOAuth1(Xamarin.Auth.Helpers.OAuth1 oauth1)
		{
			page = new PageOAuth
				(
					oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
					oauth1.OAuth1_UriRequestToken,
					oauth1.OAuth_UriAuthorization,
					oauth1.OAuth_UriAccessToken,
					oauth1.OAuth_UriCallbackAKARedirect,
					null,
					true
				);

			Navigation.PushAsync (page);

			return;
		}

		public async void Auth_Completed (object sender, AuthenticatorCompletedEventArgs ee)
		{
			string title = "OAuth Results";
			string msg = "";

			if (!ee.IsAuthenticated)
			{
				msg = "Not Authenticated";
			}
			else 
			{
				AccountStoreTests (ee);

				try 
				{
					AuthenticationResult ar = new AuthenticationResult()
					{
						Title = "n/a",
						User = "n/a",
					};

					StringBuilder sb = new StringBuilder();
					sb.Append("IsAuthenticated  = ").Append(ee.IsAuthenticated)
													.Append(System.Environment.NewLine);
					sb.Append("Name             = ").Append(ar.User)
													.Append(System.Environment.NewLine);
					sb.Append("Account.UserName = ").Append(ee.Account.Username)
													.Append(System.Environment.NewLine);

				} 
				catch (Exception ex) 
				{
					msg = ex.Message;
				}
			}

			DisplayAlert (title, msg, "OK");

			return;
		}

		private void AccountStoreTests (AuthenticatorCompletedEventArgs ee)
		{
			AccountStore account_store = AccountStore.Create();
			account_store.Save (ee.Account, provider);	
			Account account1 = account_store.FindAccountsForService(provider).FirstOrDefault();

			AccountStore.Create().Save(ee.Account, provider + ".v.2");
			// throws on iOS
			//
			Account account2 = AccountStore.Create().FindAccountsForService(provider+ ".v.2").FirstOrDefault();

			return;
		}
	}
}

