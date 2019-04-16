using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;
using Windows.UI.Core;

namespace Xamarin.Auth.Presenters.UWP
{
    public class PlatformOAuthLoginPresenter
    {
        private readonly bool _hasHardwareButton;
        private Frame _rootFrame;
        private Authenticator _authenticator;

        public PlatformOAuthLoginPresenter()
        {
        	_hasHardwareButton = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }

        public void Login(Authenticator authenticator)
        {
            _authenticator = authenticator;
            authenticator.Completed += AuthenticatorCompleted;
            System.Type pageType = authenticator.GetUI();

            _rootFrame = Window.Current.Content as Frame;
            _rootFrame.Navigate(pageType, authenticator);


            if (!_hasHardwareButton && _authenticator.AllowCancel)
            {
                var navManager = SystemNavigationManager.GetForCurrentView();
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                navManager.BackRequested += CustomPlatformOAuthLoginPresenter_BackRequested;
            }

            return;

            authenticator.Completed += AuthenticatorCompleted;

            System.Type page_type = authenticator.GetUI();

            Windows.UI.Xaml.Controls.Frame root_frame = null;
            root_frame = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            root_frame.Navigate(page_type, authenticator);

            return;
        }

        private void CustomPlatformOAuthLoginPresenter_BackRequested(object sender, BackRequestedEventArgs e)
        {
        	if (_rootFrame.CanGoBack)
        	{
        		_rootFrame.GoBack();
        		_authenticator.OnCancelled();
        	}

            return;
        }

        protected void AuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (!_hasHardwareButton && _authenticator.AllowCancel)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= CustomPlatformOAuthLoginPresenter_BackRequested;
            }

            ((Authenticator)sender).Completed -= AuthenticatorCompleted;

            return;
        }
    }
}