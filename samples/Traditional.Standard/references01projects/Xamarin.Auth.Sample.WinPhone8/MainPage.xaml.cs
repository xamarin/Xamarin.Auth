using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Auth.Sample.WinPhone8.Resources;


using System.Text;

using Xamarin.Auth.SampleData;

namespace Xamarin.Auth.Sample.WinPhone8
{
    public partial class MainPage : PhoneApplicationPage
    {
        string[] provider_list;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();


            provider_list = new string[]
                {
                    "Facebook OAuth2",
                    "Twitter OAuth1",
                    "Google OAuth2",
                    "Microsoft Live OAuth2",
                    "LinkedIn OAuth1",
                    "LinkedIn OAuth2",
                    "Github OAuth2",
                    "Instagram OAuth2",
                };

            itemList.ItemsSource = null;
            itemList.ItemsSource = provider_list;
        }

        private void itemList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //TextView tv = v as TextView;
            string si = ((ListBox)sender).SelectedItem.ToString();
            string provider = si;

            switch (provider)
            {
                case "Facebook OAuth2":
                    Authenticate(Data.TestCases["Facebook OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Twitter OAuth1":
                    Authenticate(Data.TestCases["Twitter OAuth1"] as Xamarin.Auth.Helpers.OAuth1);
                    break;
                case "Google OAuth2":
                    Authenticate(Data.TestCases["Google OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Microsoft Live OAuth2":
                    Authenticate(Data.TestCases["Microsoft Live OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "LinkedIn OAuth1":
                    Authenticate(Data.TestCases["LinkedIn OAuth1"] as Xamarin.Auth.Helpers.OAuth1);
                    break;
                case "LinkedIn OAuth2":
                    Authenticate(Data.TestCases["LinkedIn OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Github OAuth2":
                    Authenticate(Data.TestCases["Github OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Instagram OAuth2":
                    Authenticate(Data.TestCases["Instagram OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                default:
                    //Toast.MakeText(this, "Unknown OAuth Provider!", ToastLength.Long);
                    break;
            };
            var list = Data.TestCases;

            return;

        }

        private void Authenticate(Xamarin.Auth.Helpers.OAuth1 oauth1)
        {
            return;
        }

        private void Authenticate(Xamarin.Auth.Helpers.OAuth2 oauth2)
        {
            OAuth2Authenticator auth = new OAuth2Authenticator
                (
                    clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    scope: oauth2.OAuth2_Scope,
                    authorizeUrl: oauth2.OAuth_UriAuthorization,
                    redirectUrl: oauth2.OAuth_UriCallbackAKARedirect
                );

            auth.AllowCancel = oauth2.AllowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;

			Uri uri = auth.GetUI();
			(System.Windows.Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);

            return;
        }

        public async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs ee)
        {
            string title = "OAuth Results";
            string msg = "";

            if (!ee.IsAuthenticated)
            {
                msg = "Not Authenticated";
            }
            else
            {
                try
                {
                    AuthenticationResult ar = new AuthenticationResult()
                    {
                        Title = "n/a",
                        User = "n/a",
                    };

                    StringBuilder sb = new StringBuilder();
                    sb.Append("IsAuthenticated  = ").Append(ee.IsAuthenticated)
                                                    .Append(System.Environment.NewLine);
                    sb.Append("Name             = ").Append(ar.User)
                                                    .Append(System.Environment.NewLine);
                    sb.Append("Account.UserName = ").Append(ee.Account.Username)
                                                    .Append(System.Environment.NewLine);

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }


            //UIAlertView _error = new UIAlertView(title, msg, null, "Ok", null);
            //_error.Show();

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