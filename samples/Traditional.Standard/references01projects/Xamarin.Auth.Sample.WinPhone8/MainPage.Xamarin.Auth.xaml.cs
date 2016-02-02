using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Auth.Sample.WinPhone8.Resources;


using System.Text;

using Xamarin.Auth.SampleData;

namespace Xamarin.Auth.Sample
{
    public partial class MainPage
    {
		string[] provider_list = new string[] 
		{ 
			"Facebook OAuth2",
			"Twitter OAuth1",
			"Google OAuth2",
			"Microsoft Live OAuth2",
			"LinkedIn OAuth1",
			"LinkedIn OAuth2",
			"Github OAuth2",
			"Amazon OAuth2", 
			"Dropbox OAuth2", 
			"Meetup OAuth1", 
			"Meetup OAuth2", 
			"Paypal OAuth2", 
			"Stackoverflow OAuth2", 
		};
		string provider = null;

		private void itemList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			//TextView tv = v as TextView;
			string si = ((ListBox)sender).SelectedItem.ToString();
			string provider = si;

			switch (provider)
			{
			case "Facebook OAuth2":
				Authenticate(Data.TestCases["Facebook OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Twitter OAuth1":
				Authenticate(Data.TestCases["Twitter OAuth1"] as Xamarin.Auth.Helpers.OAuth1);
				break;
			case "Google OAuth2":
				Authenticate(Data.TestCases["Google OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Microsoft Live OAuth2":
				Authenticate(Data.TestCases["Microsoft Live OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "LinkedIn OAuth1":
				Authenticate(Data.TestCases["LinkedIn OAuth1"] as Xamarin.Auth.Helpers.OAuth1);
				break;
			case "LinkedIn OAuth2":
				Authenticate(Data.TestCases["LinkedIn OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Github OAuth2":
				Authenticate(Data.TestCases["Github OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Instagram OAuth2":
				Authenticate(Data.TestCases["Instagram OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Amazon OAuth2":
				Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Meetup OAuth1":
				Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Meetup OAuth2":
				Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Dropbox OAuth2":
				Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Paypal OAuth2":
				Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
				break;
			case "Stackoverflow OAuth2":
				Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
				break;
			default:
				//Toast.MakeText(this, "Unknown OAuth Provider!", ToastLength.Long);
				break;
			};
			var list = Data.TestCases;

			return;

		}

		private void Authenticate(Xamarin.Auth.Helpers.OAuth1 oauth1)
		{
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

			Uri uri = auth.GetUI();
			(System.Windows.Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);

			return;
		}

		public async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs ee)
		{
			string title = "OAuth Results";
			string msg = "";

			if (!ee.IsAuthenticated)
			{
				msg = "Not Authenticated";
			}
			else
			{
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


			//UIAlertView _error = new UIAlertView(title, msg, null, "Ok", null);
			//_error.Show();

		}

		private void AccountStoreTests(object authenticator, AuthenticatorCompletedEventArgs ee)
		{
			AccountStore account_store = AccountStore.Create();
			account_store.Save(ee.Account, provider);
			// https://kb.xamarin.com/agent/case/225411

			//------------------------------------------------------------------
			// Android
			// cannot reproduce 
			Account account1 = account_store.FindAccountsForService(provider).FirstOrDefault();
			if (null != account1)
			{
				//------------------------------------------------------------------
				string token = default(string);
				if (null != account1)
				{
					string token_name = default(string);
					Type t = authenticator.GetType();
					if (t == typeof(Xamarin.Auth.OAuth2Authenticator))
					{
						token_name = "access_token";
						token = account1.Properties[token_name].ToString();
					}
					else if (t == typeof(Xamarin.Auth.OAuth1Authenticator))
					{
						token_name = "oauth_token";
						token = account1.Properties[token_name].ToString();
					}
				}
				//------------------------------------------------------------------
			}
			//------------------------------------------------------------------

			AccountStore.Create().Save(ee.Account, provider + ".v.2");

			//------------------------------------------------------------------
			// throws on iOS
			//
			Account account2 = AccountStore.Create().FindAccountsForService(provider + ".v.2").FirstOrDefault();
			if (null != account2)
			{
				//------------------------------------------------------------------
				string token = default(string);
				if (null != account2)
				{
					string token_name = default(string);
					Type t = authenticator.GetType();
					if (t == typeof(Xamarin.Auth.OAuth2Authenticator))
					{
						token_name = "access_token";
						token = account2.Properties[token_name].ToString();
					}
					else if (t == typeof(Xamarin.Auth.OAuth1Authenticator))
					{
						token_name = "oauth_token";
						token = account2.Properties[token_name].ToString();
					}
				}
				//------------------------------------------------------------------
			}
			//------------------------------------------------------------------

			return;
		}
    }
}