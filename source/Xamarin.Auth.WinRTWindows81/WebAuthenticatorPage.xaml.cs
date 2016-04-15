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
        public WebAuthenticatorPage()
        {
            this.InitializeComponent();
        }

     
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            OAuth2Authenticator auth = (OAuth2Authenticator) e.Parameter;

            auth.Completed += auth_Completed;
            auth.Error += OnAuthError;

            Uri uri = await auth.GetInitialUrlAsync();
            this.browser.Source = uri;

            /*
            string key = NavigationContext.QueryString["key"];

            this.auth = (WebAuthenticator)PhoneApplicationService.Current.State[key];
            //this.auth.Completed += (sender, args) => NavigationService.GoBack(); // throws on BackButton
            this.auth.Completed += auth_Completed;
            this.auth.Error += OnAuthError;

            PhoneApplicationService.Current.State.Remove(key);

            if (this.auth.ClearCookiesBeforeLogin)
                await this.browser.ClearCookiesAsync();

            Uri uri = await this.auth.GetInitialUrlAsync();
            this.browser.Source = uri;
            */
            base.OnNavigatedTo(e);
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();

            return;
        }
    }
}
