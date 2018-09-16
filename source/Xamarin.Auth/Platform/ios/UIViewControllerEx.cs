using System;
using Foundation;
using UIKit;

namespace Xamarin.Auth
{
    internal static class UIViewControllerEx
    {
        public static void ShowError(this UIViewController controller, string title, Exception error, Action continuation = null)
        {
            ShowError(controller, title, error.GetUserMessage(), continuation);
        }

        public static void ShowError(this UIViewController controller, string title, string message, Action continuation = null)
        {
            var mainBundle = NSBundle.MainBundle;

            var alert = new UIAlertView(
                mainBundle.LocalizedString(title, "Error message title"),
                mainBundle.LocalizedString(message, "Error"),
                (IUIAlertViewDelegate)null,
                mainBundle.LocalizedString("OK", "Dismiss button title for error message"));

            if (continuation != null)
            {
                alert.Dismissed += delegate
                {
                    continuation();
                };
            }

            alert.Show();
        }
    }
}

