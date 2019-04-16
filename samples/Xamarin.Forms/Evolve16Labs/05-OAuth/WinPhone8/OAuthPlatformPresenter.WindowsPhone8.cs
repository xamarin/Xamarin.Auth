namespace Xamarin.Auth
{
	public class PlatformOAuthLoginPresenter
	{
        /*
        ANDROID:
        internal static Context Context { get; set; }

        IOS:
        UIKit.UIViewController rootViewController;


        */

        public void Login (Authenticator authenticator)
		{
			authenticator.Completed += AuthenticatorCompleted;

            /*
            ANDROID:
 			Auth.Context.StartActivity (authenticator.GetUI(Auth.Context));
            
            
            IOS:
            rootViewController = UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController;
            rootViewController.PresentViewController (authenticator.GetUI(), true, null);
            */

            System.Uri uri = authenticator.GetUI();
            Auth.Page.NavigationService.Navigate(uri);

		}

		void AuthenticatorCompleted (object sender, AuthenticatorCompletedEventArgs e)
		{
			//rootViewController.DismissViewController (true, null);

			((Authenticator)sender).Completed -= AuthenticatorCompleted;
		}
	}
}