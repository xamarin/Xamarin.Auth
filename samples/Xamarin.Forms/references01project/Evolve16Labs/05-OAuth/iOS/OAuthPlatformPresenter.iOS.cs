namespace Xamarin.Auth
{
	public class PlatformOAuthLoginPresenter
	{
		UIKit.UIViewController rootViewController;

		public void Login (Authenticator authenticator)
		{
			authenticator.Completed += AuthenticatorCompleted;

			rootViewController = UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController;
			rootViewController.PresentViewController (authenticator.GetUI(), true, null);
		}

		void AuthenticatorCompleted (object sender, AuthenticatorCompletedEventArgs e)
		{
			rootViewController.DismissViewController (true, null);
			((Authenticator)sender).Completed -= AuthenticatorCompleted;
		}
	}
}