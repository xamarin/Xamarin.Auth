using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Xamarin.Auth
{
    public partial class WebAuthenticatorPage : Page
    {
        private WebAuthenticator authenticator;
        private Uri initialUrl;
        private Uri redirectUrlArgs;

        public WebAuthenticatorPage()
        {
            browser.NavigationCompleted += OnBrowserNavigationCompleted;
            browser.NavigationStarting += OnBrowserNavigationStarting;
            browser.NavigationFailed += OnBrowserNavigationFailed;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (authenticator == null)
                authenticator = e.Parameter as WebAuthenticator;

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            initialUrl = await authenticator.GetInitialUrlAsync();

            browser.Navigate(initialUrl);
        }

        private void OnBrowserNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            authenticator.OnPageLoading(args.Uri);
        }

        private void OnBrowserNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            redirectUrlArgs = args.Uri;
        }

        private void OnBrowserNavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            throw new AuthException("Browser navigation failed: " + e.WebErrorStatus);
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new AuthException("Authentication error: " + e.Message, e.Exception);
        }

        private void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
