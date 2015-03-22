using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Xamarin.Auth.Store
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
            //this.browser.Navigating += OnBrowserNavigating;
            this.browser.LoadCompleted += browser_LoadCompleted;
            this.browser.NavigationFailed += browser_NavigationFailed;
        }

        WebAuthenticator _auth;
        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetAuth(e.Parameter as WebAuthenticator);
        }

        /// <summary>
        /// pages with SwapChainBackgroundPanel don't support frame navigation
        /// so need this to manually start process
        /// </summary>
        /// <param name="auth"></param>
        public async void SetAuth(WebAuthenticator auth)
        {
            _auth = auth;
            _auth.Completed += (sender, args) =>
            {
                try
                {
                    //try to manually go back, might not work based on navigation method
                    if (this.Frame !=null && this.Frame.CanGoBack)
                        this.Frame.GoBack();
                }
                catch { }
            };
            _auth.Error += _auth_Error;
            this.progress.IsActive = true;
            Uri uri = await _auth.GetInitialUrlAsync();
            browser.Source = uri;
            browser.Navigate(uri);

            _auth.OnPageLoading(uri);
            _auth.OnPageLoaded(uri);
        }
        
        void _auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            var t = "";
        }

        
        void browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            this.progress.IsActive = false;
            _auth.OnPageLoaded(e.Uri);
        }

        void browser_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            this.progress.IsActive = false;
            //_auth.OnError(e.WebErrorStatus.ToString());
        }


    }
}
