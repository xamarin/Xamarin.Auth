using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Xamarin.Auth
{
	public partial class LoginView : UserControl
	{
		private WebBrowser _browser;
		private WebAuthenticator _auth;

		public LoginView(WebAuthenticator auth)
		{
			_browser = new WebBrowser();
			_browser.Navigated += _browser_Navigated;
			_browser.Navigating += _browser_Navigating;
			_browser.NavigationFailed += _browser_NavigationFailed;
			_browser.IsScriptEnabled = true;

			_auth = auth;
			_auth.Completed += (sender, args) =>
			{
				// With current implementation this isn't needed
			};
			_auth.Error += OnAuthError;

			if (_auth.ClearCookiesBeforeLogin)
			{
				var cookieTask = Task.Run(async () => { await _browser.ClearCookiesAsync(); });
				cookieTask.Wait();
			}

			var uriTask = Task.Run<Uri>(async () => await _auth.GetInitialUrlAsync());
			uriTask.Wait();
			_browser.Source = uriTask.Result;

			Content = _browser;
		}

		private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
		{
			//temporary "fix" for invalid_grant error
			if (!e.Message.Equals("Error authenticating: invalid_grant"))
			{
				//TODO: check if this is needed
			}
		}

		void _browser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
		{
			if (_auth.ShowUIErrors)
			{
				if (e.Exception == null)
					_auth.OnError("Unknown"); // Shows up when not connected to the internet
				else
					_auth.OnError(e.Exception);
			}
		}

		void _browser_Navigating(object sender, NavigatingEventArgs e)
		{
			_auth.OnPageLoading(e.Uri);
		}

		void _browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
		{
			_auth.OnPageLoaded (e.Uri);
		}
	}
}
