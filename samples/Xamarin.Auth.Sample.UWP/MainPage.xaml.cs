using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Xamarin.Auth.Sample.UWP
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var auth = new OAuth2Authenticator(
                clientId: Constants.KEY,
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            //auth.AllowCancel = allowCancel;

            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();


            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += async (s, ee) => {
                if (!ee.IsAuthenticated)
                {
                    var dialog  =new Windows.UI.Popups.MessageDialog("Not Authenticated");
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Cancel") { Id = 1 });
                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;
                    var result = await dialog.ShowAsync();

                    return;
                }

                // Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, ee.Account);
                await request.GetResponseAsync().ContinueWith(async (t) => {
                    var dialog = new Windows.UI.Popups.MessageDialog("Not Authenticated");
                    if (t.IsFaulted)
                    {
                        dialog.Title = "Error";
                        dialog.Content = t.Exception.Flatten().InnerException.ToString();
                    }
                    else if (t.IsCanceled)
                        dialog.Title = "Task Canceled";
                    else
                    {
                        var obj = JsonValue.Parse(t.Result.GetResponseText());

                        dialog.Title = "Logged in";
                        dialog.Content = "Name: " + obj["name"];
                    }

                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                    dialog.DefaultCommandIndex = 0;
                    await dialog.ShowAsync();
                }, scheduler);
            };

            var loginPageType = auth.GetUI();
            this.Frame.Navigate(loginPageType, auth);
        }
    }
}
