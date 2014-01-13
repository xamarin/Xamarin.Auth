using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
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
using Xamarin.Auth.Store.Control;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Xamarin.Auth.Sample.Store
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void OnClickFacebook(object sender, RoutedEventArgs e)
        {
            var auth = new OAuth2Authenticator(
                clientId: "438894422873149",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += async (s, ee) =>
            {
                if (!ee.IsAuthenticated)
                {
                    this.facebookStatus.Text = "Not Authenticated";
                    return;
                }

                // Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, ee.Account);
                try
                {
                    Response response = await request.GetResponseAsync();
                    var obj = JsonValue.Parse(await response.GetResponseTextAsync());

                    this.facebookStatus.Text = "Name: " + obj["name"];

                }
                catch (OperationCanceledException)
                {
                    this.facebookStatus.Text = "Canceled";
                }
                catch (Exception ex)
                {
                    this.facebookStatus.Text = "Error: " + ex.Message;
                }
            };

            AuthUI authUI = (AuthUI)auth.GetUI();
            AuthPanel.Children.Clear();
            AuthPanel.Children.Add(authUI);
            AuthPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            authUI.Navigate();
            //NavigationService.Navigate(uri);
        }
    }
}
