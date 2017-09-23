#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth.Presenters.Windows81Universal
#else
namespace Xamarin.Auth._MobileServices.Presenters.Windows81Universal
#endif
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