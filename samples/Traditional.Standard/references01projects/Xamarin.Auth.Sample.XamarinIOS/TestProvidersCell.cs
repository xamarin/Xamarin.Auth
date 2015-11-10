
using System;
using System.Text;

#if ! __CLASSIC__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

using global::Xamarin.Auth.SampleData;

namespace Xamarin.Auth.Sample.XamarinIOS
{
	public class TestProvidersCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("TestProvidersCell");

		public TestProvidersCell () : base (UITableViewCellStyle.Value1, Key)
		{
			// TODO: add subviews to the ContentView, set various colors, etc.
			TextLabel.Text = "TextLabel";
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			string provider = this.TextLabel.Text;

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
					UIAlertView _error = new UIAlertView ("Error", "Unknown OAuth Provider!", null, "Ok", null);
					_error.Show ();
					break;
			};
			var list = Data.TestCases;
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

			UIViewController vc = auth.GetUI ();

			UIView sv1 = this.Superview;
			UITableView sv2 = (UITableView) sv1.Superview;
			UIWindow sv3 = (UIWindow) sv2.Superview;
			sv3.RootViewController.PresentViewController(vc, true, null);

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


			UIAlertView _error = new UIAlertView (title, msg, null, "Ok", null);
			_error.Show ();

		}
	}
}

