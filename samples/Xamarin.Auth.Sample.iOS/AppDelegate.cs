using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Xamarin.Auth.Sample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		void LoginToFacebook (bool allowCancel)
		{
			var auth = new OAuth2Authenticator (
				clientId: "App ID from https://developers.facebook.com/apps",
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			auth.AllowCancel = allowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += (s, e) =>
			{
				// We presented the UI, so it's up to us to dismiss it.
				dialog.DismissViewController (true, null);

				if (!e.IsAuthenticated) {
					facebookStatus.Caption = "Not authorized";
					dialog.ReloadData();
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, e.Account);
				request.GetResponseAsync().ContinueWith (t => {
					if (t.IsFaulted)
						facebookStatus.Caption = "Error: " + t.Exception.InnerException.Message;
					else if (t.IsCanceled)
						facebookStatus.Caption = "Canceled";
					else
					{
						var obj = JsonValue.Parse (t.Result.GetResponseText());
						facebookStatus.Caption = "Logged in as " + obj["name"];
					}

					dialog.ReloadData();
				}, uiScheduler);
			};

			UIViewController vc = auth.GetUI ();
			dialog.PresentViewController (vc, true, null);
		}

		void LoginToGoogle (bool allowCancel)
		{
			var auth = new OAuth2Authenticator(
				           clientId: "558242267480-46jj51j6dog6rib4nliio60uikkcbs5f.apps.googleusercontent.com",
				           clientSecret: "exQIN8toqUPH4flPxPNwyCER",
				           scope: "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile",
				           authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
				           redirectUrl: new Uri("http://localhost"),
				           accessTokenUrl: new Uri("https://accounts.google.com/o/oauth2/token")
			           );

			auth.AllowCancel = allowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += (s, e) =>
			{
				// We presented the UI, so it's up to us to dismiss it.
				dialog.DismissViewController (true, null);

				if (!e.IsAuthenticated) {
					googleStatus.Caption = "Not authorized";
					dialog.ReloadData();
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
				var request = new OAuth2Request ("GET", new Uri ("https://www.googleapis.com/oauth2/v2/userinfo"), null, e.Account);
				request.GetResponseAsync().ContinueWith (t => {
					if (t.IsFaulted)
						googleStatus.Caption = "Error: " + t.Exception.InnerException.Message;
					else if (t.IsCanceled)
						googleStatus.Caption = "Canceled";
					else
					{
						var obj = JsonValue.Parse (t.Result.GetResponseText());
						var Id = (String)obj["id"];
						var Email = (String)obj["email"];
						var FirstName = (String)obj["given_name"];
						var LastName = (String)obj["family_name"];
						var PictureUrl = GetOptionalValue(obj, "picture");
						var Gender = GetOptionalValue(obj, "gender");
						var Locale = GetOptionalValue(obj, "locale");
						var Domain = (String)obj["hd"];

						googleStatus.Caption = "Logged in as " + Email;

						userInfo.Clear();
						userInfo.Add(new StringElement("Id", Id));
						userInfo.Add(new StringElement("Email", Email));
						userInfo.Add(new StringElement("FirstName", FirstName));
						userInfo.Add(new StringElement("LastName", LastName));
						userInfo.Add(new StringElement("Gender", Gender));
						userInfo.Add(new StringElement("Locale", Locale));
						userInfo.Add(new StringElement("Domain", Domain));

						if(PictureUrl != null)
						{
							var image = ImageFromUrl(PictureUrl);
							if(image != null)
								userInfo.Add(new ImageStringElement("Picture", image));
						}
					}

					dialog.ReloadData();
				}, uiScheduler);
			};

			UIViewController vc = auth.GetUI ();
			dialog.PresentViewController (vc, true, null);
		}

		private String GetOptionalValue(JsonValue json, String propertyName)
		{
			if (json.ContainsKey(propertyName))
				return json[propertyName];
			else
				return null;
		}

		static UIImage ImageFromUrl (string uri)
		{
			using (var url = new NSUrl (uri))
			using (var data = NSData.FromUrl (url))
				return UIImage.LoadFromData (data);
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			facebook = new Section ("Facebook");
			facebook.Add (new StyledStringElement ("Log in", () => LoginToFacebook (true)));			
			facebook.Add (new StyledStringElement ("Log in (no cancel)", () => LoginToFacebook (false)));
			facebook.Add (facebookStatus = new StringElement (String.Empty));

			google = new Section ("Google Apps");
			google.Add (new StyledStringElement ("Log in", () => LoginToGoogle (true)));			
			google.Add (new StyledStringElement ("Log in (no cancel)", () => LoginToGoogle (false)));
			google.Add (googleStatus = new StringElement (String.Empty));
			google.Add (userInfo);

			userInfo = new Section ("User Info");

			dialog = new DialogViewController (new RootElement ("Xamarin.Auth Sample") {
				facebook,
				google,
				userInfo
			});

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (dialog);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		UIWindow window;
		DialogViewController dialog;

		Section facebook;
		StringElement facebookStatus;
		Section google;
		Section userInfo;
		StringElement googleStatus;

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

