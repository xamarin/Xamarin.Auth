using UIKit;

namespace Xamarin.Auth.Presenters
{
    partial class OAuthLoginPresenter
    {
        private UIViewController rootViewController;

        private void PlatformLoginImplementation(Authenticator authenticator)
        {
            authenticator.Completed += AuthenticatorCompleted;

            // TODO: find the root more reliably
            rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            rootViewController.PresentViewController(authenticator.GetUI(), true, null);
        }

        private void AuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            ((Authenticator)sender).Completed -= AuthenticatorCompleted;

            rootViewController.DismissViewController(true, null);
        }
    }
}
