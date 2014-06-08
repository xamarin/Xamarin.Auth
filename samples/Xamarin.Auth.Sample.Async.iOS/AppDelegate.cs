using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Xamarin.Auth_Async_Sample;

namespace Xamarin.Auth_Async_Sample__IOS_
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        async void LoginToFacebook(bool allowCancel)
        {
            var facebookService = new FacebookService();
            var account = await facebookService.LoginAsync(dialog, allowCancel);

            if (account==null)
            {
                ShowMessage("Not Authenticated");
                return;
            }

            // in this step the username is always null, but i followed the original sample
            ShowMessage(string.Format("Authenticated {0}", account.Username));
            var userInfo = await facebookService.GetUserInfoAsync(account);
            ShowMessage(!string.IsNullOrEmpty(userInfo) ? string.Format("Logged as {0}", userInfo) : "Wasn´t possible to get the name.");
           
        }

        private void ShowMessage(string message)
        {
            facebookStatus.Caption = message;
            dialog.ReloadData();
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            facebook = new Section("Facebook")
            {
                new StyledStringElement("Log in", () => LoginToFacebook(true)),
                new StyledStringElement("Log in (no cancel)", () => LoginToFacebook(false))
            };
            facebook.Add(facebookStatus = new StringElement(String.Empty));

            dialog = new DialogViewController(new RootElement("Xamarin.Auth Async Sample") {
				facebook,
			});

            window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = new UINavigationController(dialog)
            };
            window.MakeKeyAndVisible();

            return true;
        }

        UIWindow window;
        DialogViewController dialog;

        Section facebook;
        StringElement facebookStatus;
    }
}