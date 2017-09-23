#if !AZURE_MOBILE_SERVICES
namespace Xamarin.Auth.Presenters.WinPhone
#else
namespace Xamarin.Auth._MobileServices.Presenters.WinPhone
#endif
{
    public class PlatformOAuthLoginPresenter
    {
        
        public void Login(Authenticator authenticator)
        {
            authenticator.Completed += AuthenticatorCompleted;

            //rootViewController = UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController;
            //rootViewController.PresentViewController(authenticator.GetUI(), true, null);

            System.Uri uri = authenticator.GetUI();
            // this == Microsoft.Phone.Controls.PhoneApplicationPage
            Microsoft.Phone.Controls.PhoneApplicationPage this_page = null;
            this_page.NavigationService.Navigate(uri);

            return;
        }

        void AuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            //rootViewController.DismissViewController(true, null);

            ((Authenticator)sender).Completed -= AuthenticatorCompleted;
        }
    }
}