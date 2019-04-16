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

        /*
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            authenticator = (OAuth2Authenticator)e.Parameter;

            url_initial = this.authenticator.GetInitialUrlAsync().Result;
            this.browser.Navigate(url_initial);

            System.Diagnostics.Debug.WriteLine("OnNavigatedTo authenticator = " + authenticator.Title);

            authenticator.Completed += auth_Completed;
            authenticator.Error += auth_Error;

            url_initial = await authenticator.GetInitialUrlAsync();

            base.OnNavigatedTo(e);

            return;
        }

        private void Browser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Uri uri_navigated = args.Uri;

            System.Diagnostics.Debug.WriteLine("Browser_NavigationStarting = " + uri_navigated.OriginalString);

            this.authenticator.OnPageLoading(uri_navigated);

            return;
        }

        private void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            url_args_redirect = args.Uri;
            Windows.Web.WebErrorStatus status = args.WebErrorStatus;

            System.Diagnostics.Debug.WriteLine("Browser_NavigationCompleted = " + url_args_redirect.OriginalString);

            return;
        }

        private void Browser_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            Uri uri_failed = e.Uri;

            System.Diagnostics.Debug.WriteLine("Browser_NavigationFailed = " + uri_failed.OriginalString);

            return;
        }

        private void auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new Xamarin.Auth.AuthException("Auth Error");
        }

        private void auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("auth_Completed Username = " + e.Account.Username);

            return;
        }
        */
    }
}
