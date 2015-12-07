using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Xamarin.Forms;

using Xamarin.Auth;
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
				
		string provider = null;

		public void OnItemSelected (object sender, ItemTappedEventArgs args_tapped)
		{
			object item = args_tapped.Item;

			KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

			string provider = kvp.Value;

			switch (provider)
			{
				case "Facebook OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Twitter OAuth1":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1);
					break;
				case "Google OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Microsoft Live OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "LinkedIn OAuth1":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1);
					break;
				case "LinkedIn OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Github OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Instagram OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				default:
					DisplayAlert ("Error", "Unknown OAuth Provider!", "OK");
					break;
			};

			this.Navigation.PushAsync(page);

			return;

		}

		private void Authenticate(Xamarin.Auth.Helpers.OAuth1 oauth1)
		{
			OAuth1Authenticator auth = new OAuth1Authenticator 
				(
					consumerKey: oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					consumerSecret: oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
					requestTokenUrl: oauth1.OAuth1_UriRequestToken,
					authorizeUrl: oauth1.OAuth_UriAuthorization,
					accessTokenUrl: oauth1.OAuth1_UriAccessToken,
					callbackUrl: oauth1.OAuth_UriCallbackAKARedirect
				);

			auth.AllowCancel = oauth1.AllowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += Auth_Completed;

			this.Navigation.PushAsync(page);

			return;
		}

		private void Authenticate(Xamarin.Auth.Helpers.OAuth2 oauth2)
		{
			OAuth2Authenticator auth = new OAuth2Authenticator 
				(
					clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					scope: oauth2.OAuth2_Scope,
					authorizeUrl: oauth2.OAuth_UriAuthorization,
					redirectUrl: oauth2.OAuth_UriCallbackAKARedirect
				);

			auth.AllowCancel = oauth2.AllowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += Auth_Completed;

			this.Navigation.PushAsync(page);

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

