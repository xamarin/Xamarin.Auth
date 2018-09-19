using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;
using Windows.UI.Core;

namespace Xamarin.Auth.Presenters
{
    partial class OAuthLoginPresenter
    {
        private readonly static bool hasHardwareButton;

        static OAuthLoginPresenter()
        {
            hasHardwareButton = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }

        private void PlatformLoginImplementation(Authenticator authenticator)
        {
            var rootFrame = Window.Current.Content as Frame;
            var canCancel = !hasHardwareButton && authenticator.AllowCancel;

            authenticator.Completed += OnAuthCompletedImplementation;

            var pageType = authenticator.GetUI();
            rootFrame.Navigate(pageType, authenticator);

            if (canCancel)
            {
                var navManager = SystemNavigationManager.GetForCurrentView();
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                navManager.BackRequested += OnBackRequestedImplementation;
            }

            void OnAuthCompletedImplementation(object sender, AuthenticatorCompletedEventArgs e)
            {
                ((Authenticator)sender).Completed -= OnAuthCompletedImplementation;
                if (canCancel)
                    SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequestedImplementation;
            }

            void OnBackRequestedImplementation(object sender, BackRequestedEventArgs e)
            {
                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();

                    authenticator.OnCancelled();
                }
            }
        }
    }
}
