using System;
using System.Collections.Generic;
using System.Drawing;
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
			ClearStatus();
			
			var auth = new OAuth2Authenticator (
				clientId: "App ID from https://developers.facebook.com/apps",
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			auth.AllowCancel = allowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.dom	
			auth.Completed += async (s, e) =>
			{
				// We presented the UI, so it's up to us to dismiss it.
				dialog.DismissViewController (true, null);

				if (!e.IsAuthenticated) {
					SetStatus ("Not authorized");
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, e.Account);
				try {
					Response response = await request.GetResponseAsync();
					var obj = JsonValue.Parse (await response.GetResponseTextAsync());
					SetStatus ("Logged in as " + obj["name"]);
				} catch (OperationCanceledException) {
					SetStatus ("Canceled");
				} catch (Exception ex) {
					SetStatus ("Error: " + ex.Message);
				}
			};

			UIViewController vc = auth.GetUI ();
			dialog.PresentViewController (vc, true, null);
		}

		void ClearStatus()
		{
			activity.StartAnimating();
			activity.Hidden = false;
			facebookStatus.Caption = String.Empty;
			dialog.ReloadData();
		}

		void SetStatus (string status)
		{
			activity.StopAnimating();
			facebookStatus.Caption = status;
			dialog.ReloadData();
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			facebook = new Section ("Facebook");
			facebook.Add (new StyledStringElement ("Log in", () => LoginToFacebook (true)));			
			facebook.Add (new StyledStringElement ("Log in (no cancel)", () => LoginToFacebook (false)));
			facebook.Add (facebookStatus = new StringElement (String.Empty));

			dialog = new DialogViewController (new RootElement ("Xamarin.Auth Sample") {
				facebook,
			});

			activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White) {
				Frame = new RectangleF (0, 11.5f, 21, 21),
				HidesWhenStopped = true,
				Hidden = true,
			};

			dialog.NavigationItem.RightBarButtonItem = new UIBarButtonItem (activity);

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (dialog);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		UIWindow window;
		DialogViewController dialog;
		UIActivityIndicatorView activity;

		Section facebook;
		StringElement facebookStatus;

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

