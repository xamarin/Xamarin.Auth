using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Auth.SampleData;
using System.Text;

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

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			TextView tv = v as TextView;
			string provider = tv.Text;

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
				default:
					Toast.MakeText(this, "Unknown OAuth Provider!", ToastLength.Long);
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

			var intent = auth.GetUI (this);
			StartActivity (intent);

			return;
		}

		public async void Auth_Completed (object sender, AuthenticatorCompletedEventArgs ee)
		{
			var builder = new AlertDialog.Builder (this);

			if (!ee.IsAuthenticated)
			{
				builder.SetMessage ("Not Authenticated");
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

			builder.SetPositiveButton ("Ok", (o, e) => { });
			builder.Create().Show();

			return;	
		}
	}
}


