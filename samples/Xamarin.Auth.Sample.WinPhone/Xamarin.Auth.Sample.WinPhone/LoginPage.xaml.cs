using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Json;

namespace Xamarin.Auth.Sample.WinPhone
{
	public partial class LoginPage : PhoneApplicationPage
	{
		public LoginPage()
		{
			InitializeComponent();

			ShowLogin();
		}

		public void ShowLogin()
		{

			var auth = new OAuth2Authenticator(
				clientId: "App ID from https://developers.facebook.com/apps",
				scope: "",
				authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += async (s, ee) =>
			{
				if (!ee.IsAuthenticated)
				{
					App.LoginStatus = "Not Authenticated";
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
				var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, ee.Account);
				try
				{
					Response response = await request.GetResponseAsync();
					var obj = JsonValue.Parse(response.GetResponseText());

					App.LoginStatus = "Name: " + obj["name"];

				}
				catch (OperationCanceledException)
				{
					App.LoginStatus = "Canceled";
				}
				catch (Exception ex)
				{
					App.LoginStatus = "Error: " + ex.Message;
				}

				if (NavigationService.CanGoBack)
					NavigationService.GoBack();
				else
					NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
			};

			var control = auth.GetUI();

			ContentPanel.Children.Add(control);
		}
	}
}