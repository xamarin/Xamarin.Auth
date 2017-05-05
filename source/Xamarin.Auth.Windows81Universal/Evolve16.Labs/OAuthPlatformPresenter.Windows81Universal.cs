namespace Xamarin.Auth.Presenters.Windows81Universal
{
    public class PlatformOAuthLoginPresenter
    {
        
        public void Login(Authenticator authenticator)
        {
            authenticator.Completed += AuthenticatorCompleted;

            //rootViewController = UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController;
            //rootViewController.PresentViewController(authenticator.GetUI(), true, null);
        }

        void AuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            //rootViewController.DismissViewController(true, null);

            ((Authenticator)sender).Completed -= AuthenticatorCompleted;
        }
    }
}