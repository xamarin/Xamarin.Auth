using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Xamarin.Auth.Sample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		DialogViewController dialog;

		Section facebook;
		Section skydrive;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			facebook = new Section ("Facebook");
			facebook.Add (new StyledStringElement ("Log in", LoginToFacebook));

			skydrive = new Section ("Skydrive");
			skydrive.Add (new StyledStringElement ("Log in", LoginToSkydrive));

			dialog = new DialogViewController (new RootElement ("Xamarin.Auth Sample") {
				facebook,
				skydrive,
			});

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (dialog);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		void LoginToFacebook ()
		{
			var a = new OAuth2Authenticator (
				clientId: "346691492084618",
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			var vc = a.GetUI ();
			dialog.PresentViewController (vc, true, null);
			a.Completed += (s, e) => {
				dialog.DismissViewController (true, null);
				facebook.Add (new StringElement (GetEventAsString (e)));
			};
		}

		void LoginToSkydrive ()
		{
			var a = new OAuth2Authenticator (
				clientId: "00000000440DC040",
				scope: "wl.basic,wl.share,wl.skydrive",
				authorizeUrl: new Uri ("https://login.live.com/oauth20_authorize.srf"),
				redirectUrl: new Uri ("https://login.live.com/oauth20_desktop.srf"));

			var vc = a.GetUI ();
			dialog.PresentViewController (vc, true, null);
			a.Completed += (s, e) => {
				dialog.DismissViewController (true, null);
				skydrive.Add (new StringElement (GetEventAsString (e)));
			};
		}

		string GetEventAsString (AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated) {
				return e.Account.Serialize ();
			} else {
				return "Not Authenticated";
			}
		}

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

