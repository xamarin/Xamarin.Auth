using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Text;

//using System.Windows.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

using Xamarin.Auth.ProviderSamples;

namespace Xamarin.Auth.Sample
{
    public partial class MainPage
    {
		string[] provider_list = Data.TestCases.Keys.ToArray ();
		string provider = null;

        private void itemList_Tap(object sender, TappedRoutedEventArgs e)
		{
			//TextView tv = v as TextView;
			string si = ((ListBox)sender).SelectedItem.ToString();
			string provider = si;

			Xamarin.Auth.ProviderSamples.Helpers.OAuth auth;
			if (!Data.TestCases.TryGetValue (provider, out auth)) 
            {
				//TODO: MessageBox.Show("Unknown OAuth Provider!");
			}
			if (auth is Xamarin.Auth.ProviderSamples.Helpers.OAuth1) 
            {
				Authenticate (auth as Xamarin.Auth.ProviderSamples.Helpers.OAuth1);
			} else 
            {
				Authenticate (auth as Xamarin.Auth.ProviderSamples.Helpers.OAuth2);
			}
			var list = Data.TestCases;

			return;

		}

		private void Authenticate(Xamarin.Auth.ProviderSamples.Helpers.OAuth1 oauth1)
		{
			OAuth1Authenticator auth = new OAuth1Authenticator 
				(
					consumerKey: oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					consumerSecret: oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
					requestTokenUrl: oauth1.OAuth1_UriRequestToken,
					authorizeUrl: oauth1.OAuth_UriAuthorization,
					accessTokenUrl: oauth1.OAuth_UriAccessToken_UriRequestToken,
					callbackUrl: oauth1.OAuth_UriCallbackAKARedirect
				);

			auth.AllowCancel = oauth1.AllowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += Auth_Completed;
			auth.Error += Auth_Error;
			auth.BrowsingCompleted += Auth_BrowsingCompleted;

            //Uri uri = auth.GetUI();
            Type page_type = auth.GetUI();

            //(System.Windows.Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);
            Windows.UI.Xaml.Controls.Page this_page = this;
            this_page.Frame.Navigate(page_type, auth);

            return;
		}

		private void Authenticate(Xamarin.Auth.ProviderSamples.Helpers.OAuth2 oauth2)
		{
			OAuth2Authenticator auth = null;

			if (oauth2.OAuth_UriAccessToken_UriRequestToken == null || string.IsNullOrEmpty (oauth2.OAuth_SecretKey_ConsumerSecret_APISecret)) 
            {
				auth = new OAuth2Authenticator 
                (
					clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					scope: oauth2.OAuth2_Scope,
					authorizeUrl: oauth2.OAuth_UriAuthorization,
					redirectUrl: oauth2.OAuth_UriCallbackAKARedirect
				);
			} 
            else 
            {
				auth = new OAuth2Authenticator 
                (
					clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					clientSecret: "93e7f486b09bd1af4c38913cfaacbf8a384a50d2",
					scope: oauth2.OAuth2_Scope,
					authorizeUrl: oauth2.OAuth_UriAuthorization,
					redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
					accessTokenUrl: oauth2.OAuth_UriAccessToken_UriRequestToken
				);
			}

			auth.AllowCancel = oauth2.AllowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += Auth_Completed;
			auth.Error += Auth_Error;
			auth.BrowsingCompleted += Auth_BrowsingCompleted;

            //Uri uri = auth.GetUI();
            Type page_type = auth.GetUI();

            //(System.Windows.Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);
            this.Frame.Navigate(page_type, auth);


            return;
		}

		private void Auth_Error (object sender, AuthenticatorErrorEventArgs ee)
		{
			string msg = "";

			StringBuilder sb = new StringBuilder();
			sb.Append("Message  = ").Append(ee.Message)
				.Append(System.Environment.NewLine);
			msg = sb.ToString();

            //TODO: MessageBox.Show("Message = " + msg);
            
			return;

		}

		private void Auth_BrowsingCompleted (object sender, EventArgs ee)
		{
			string msg = "";

			StringBuilder sb = new StringBuilder();
			msg = sb.ToString();

            //TODO: MessageBox.Show("Message = " + msg);

			return;
		}

		public async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs ee)
		{
			string msg = "";

			if (!ee.IsAuthenticated)
			{
				msg = "Not Authenticated";
			}
			else
			{
				try
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("IsAuthenticated  = ").Append(ee.IsAuthenticated)
						.Append(System.Environment.NewLine);
					sb.Append("Account.UserName = ").Append(ee.Account.Username)
						.Append(System.Environment.NewLine);

				}
				catch (Exception ex)
				{
					msg = ex.Message;
				}
			}

            //ee.Account.Properties;

            return;
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