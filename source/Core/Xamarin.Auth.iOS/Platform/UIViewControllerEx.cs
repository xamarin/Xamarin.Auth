using Foundation;
using System;
using UIKit;

namespace Xamarin.Auth
{
    internal static class UIViewControllerEx
    {
        public static void ShowError(this UIViewController controller, string title, Exception error, Action continuation = null)
        {
            ShowError(controller, title, error.GetInitialMessage(), continuation);
        }

        public static void ShowError(this UIViewController controller, string title, string message, Action continuation = null)
        {
            var mainBundle = NSBundle.MainBundle;

            var alert = new UIAlertView(
                mainBundle.GetLocalizedString(title),
                mainBundle.GetLocalizedString(message),
                (IUIAlertViewDelegate)null,
                mainBundle.GetLocalizedString("OK"));

            if (continuation != null)
                alert.Dismissed += delegate { continuation(); };

            alert.Show();
        }
    }
}
