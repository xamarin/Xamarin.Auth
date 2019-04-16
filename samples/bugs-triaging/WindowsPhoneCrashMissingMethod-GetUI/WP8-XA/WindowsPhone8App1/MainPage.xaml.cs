using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhone8App1.Resources;
using Xamarin.Auth;

namespace WindowsPhone8App1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            OAuth2Authenticator auth = null;

            auth = new OAuth2Authenticator(
                clientId: "1093596514437-ibfmn92v4bf27tto068heesgaohhto7n.apps.googleusercontent.com",
                scope: "https://www.googleapis.com/auth/userinfo.email",
                authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                redirectUrl: new Uri("http://xamarin.com")
            );

            auth.AllowCancel = auth.AllowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;
            auth.Error += Auth_Error;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;

            Uri uri = auth.GetUI();
            this.NavigationService.Navigate(uri);
        }

        private void Auth_BrowsingCompleted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            string t = e.Account.Properties[((OAuth2Authenticator)sender).AccessTokenName];

            MessageBox.Show("Token = " + t, "Auth OK", MessageBoxButton.OK);

            return;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}