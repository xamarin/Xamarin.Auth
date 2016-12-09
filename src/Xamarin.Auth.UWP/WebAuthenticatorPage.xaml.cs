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

namespace Xamarin.Auth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebAuthenticatorPage : Page
    {
        private OAuth2Authenticator auth;

        public WebAuthenticatorPage()
        {
            this.InitializeComponent();
        }

     
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            auth = (OAuth2Authenticator) e.Parameter;

            auth.Completed += OnAuthCompleted;
            auth.Error += OnAuthError;

            Uri uri = await auth.GetInitialUrlAsync();
            this.browser.Source = uri;

            auth.Completed += (sender, args) =>  // throws on BackButton
            auth.Completed += OnAuthCompleted;
            auth.Error += OnAuthError;
           
            

            base.OnNavigatedTo(e);
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();

            return;
        }
    }
}
