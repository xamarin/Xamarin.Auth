using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebAuthenticatorPage : Page
    {
        public WebAuthenticatorPage()
        {
            this.InitializeComponent();

            this.browser.NavigationCompleted += Browser_NavigationCompleted;
            this.browser.NavigationStarting += Browser_NavigationStarting;
            this.browser.NavigationFailed += Browser_NavigationFailed;

            return;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.Frame.GoBack();
                    }
                );

            return;
        }
    }
}
