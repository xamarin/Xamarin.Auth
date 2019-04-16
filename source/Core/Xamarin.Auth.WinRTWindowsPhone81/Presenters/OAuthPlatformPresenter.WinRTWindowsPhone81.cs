namespace Xamarin.Auth.Presenters.WinRT
{
    public class PlatformOAuthLoginPresenter
    {
        
        public void Login(Authenticator authenticator)
        {
            authenticator.Completed += AuthenticatorCompleted;

            System.Type page_type = authenticator.GetUI();

            Windows.UI.Xaml.Controls.Frame root_frame = null;
            root_frame = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            root_frame.Navigate(page_type, authenticator);

            return;
        }

        void AuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            //rootViewController.DismissViewController(true, null);

            ((Authenticator)sender).Completed -= AuthenticatorCompleted;
        }
    }
}