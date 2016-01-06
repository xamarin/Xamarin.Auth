using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Auth.SampleData;

namespace Xamarin.Auth.Sample.XamarinAndroid
{
	[Activity (Label = "Xamarin.Auth.Sample.XamarinAndroid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ListActivity 
	{
		string[] provider_list;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			provider_list = new string[] 
				{ 
					"Facebook OAuth2",
					"Twitter OAuth1",
					"Google OAuth2",
					"Microsoft Live OAuth2",
					"LinkedIn OAuth1",
					"LinkedIn OAuth2",
					"Github OAuth2",
					"Instagram OAuth2", 
				};

			ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, provider_list);

			return;
		}

		string provider = null;

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			TextView tv = v as TextView;
			provider = tv.Text;

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
					Toast.MakeText(this, "Unknown OAuth Provider!", ToastLength.Long);
					break;
			};

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
            auth.Error += Auth_Error;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;

			var intent = auth.GetUI (this);
			StartActivity (intent);

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
            auth.Error += Auth_Error;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;

			var intent = auth.GetUI (this);
			StartActivity (intent);

			return;
		}

        private void Auth_Error (object sender, AuthenticatorErrorEventArgs ee)
        {
            string title = "OAuth Error";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            sb.Append("Message  = ").Append(ee.Message)
                                    .Append(System.Environment.NewLine);
            msg = sb.ToString();

            Toast.MakeText
                        (
                            this,
                            "Message = " + msg,
                            ToastLength.Short
                        ).Show();

            return;

        }

        private void Auth_BrowsingCompleted (object sender, EventArgs ee)
        {
            string title = "OAuth Browsing Completed";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            msg = sb.ToString();

            Toast.MakeText
                        (
                            this,
                            "Message = " + msg,
                            ToastLength.Short
                        ).Show();

            return;
        }

		public void Auth_Completed (object sender, AuthenticatorCompletedEventArgs ee)
		{
			var builder = new AlertDialog.Builder (this);

			if (!ee.IsAuthenticated)
			{
				builder.SetMessage ("Not Authenticated");
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

					builder.SetTitle (ar.Title);
					builder.SetMessage (sb.ToString());
				} 
				catch (Android.OS.OperationCanceledException) 
				{
					builder.SetTitle ("Task Canceled");
				} 
				catch (Exception ex) 
				{
					builder.SetTitle ("Error");
					builder.SetMessage (ex.ToString());
				}
			}

			//ee.Account

			builder.SetPositiveButton ("Ok", (o, e) => { });
			builder.Create().Show();

			return;	
		}

		private void AccountStoreTests (AuthenticatorCompletedEventArgs ee)
		{
			AccountStore account_store = AccountStore.Create(this);
			account_store.Save (ee.Account, provider);	

            //------------------------------------------------------------------
            // Android
            // https://kb.xamarin.com/agent/case/225411
            // cannot reproduce 
			Account account1 = account_store.FindAccountsForService(provider).FirstOrDefault();
            if( null != account1 )
            {
                string token = account1.Properties["access_token"].ToString();
                Toast.MakeText
                        (
                            this,
                            "acces_token = " + token,
                            ToastLength.Short
                        ).Show();
            }
            //------------------------------------------------------------------

			AccountStore.Create(this).Save(ee.Account, provider + ".v.2");

            //------------------------------------------------------------------
			// throws on iOS
			//
			Account account2 = AccountStore.Create(this).FindAccountsForService(provider+ ".v.2").FirstOrDefault();
            if( null != account2 )
            {
                string token = account2.Properties["access_token"].ToString();
                Toast.MakeText
                        (
                            this,
                            "acces_token = " + token,
                            ToastLength.Short
                        ).Show();
            }
            //------------------------------------------------------------------

			return;
		}
	}
}


