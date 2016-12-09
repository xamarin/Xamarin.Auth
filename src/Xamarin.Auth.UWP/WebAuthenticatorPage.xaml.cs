using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
namespace Xamarin.Auth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebAuthenticatorPage : Page
    {
        private WebAuthenticator _auth;

        public WebAuthenticatorPage()
        {
            this.InitializeComponent();
        }
     
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _auth = (WebAuthenticator) e.Parameter;

            if (_auth == null)
                throw new InvalidOperationException("Expected WebAuthenticator as NavigationEventArgs.Parameter");

            _auth.Completed += OnAuthCompleted;
            _auth.Error += OnAuthError;

            Uri uri = await _auth.GetInitialUrlAsync();
            this.browser.Source = uri;
            this.browser.Settings.IsJavaScriptEnabled = true;
            this.browser.NavigationStarting += Browser_NavigationStarting;
            this.browser.NavigationCompleted += Browser_NavigationCompleted;

            if (_auth.ClearCookiesBeforeLogin)
            {
                await Windows.UI.Xaml.Controls.WebView.ClearTemporaryWebDataAsync();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _auth.Completed -= OnAuthCompleted;
            _auth.Error -= OnAuthError;

            base.OnNavigatedFrom(e);
        }

        private void Browser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            _auth.OnPageLoading(args.Uri);
        }

        private void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _auth.OnPageLoaded(args.Uri);
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();

            return;
        }
    }
}
