using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ComicBook
{
	public partial class MainPage : ContentPage
	{
		const string ServiceId = "ComicBook";
		const string Scope = "profile";

		Account account;
		AccountStore store;

		public MainPage ()
		{
			InitializeComponent ();

			implicitButton.Clicked += ImplicitButtonClicked;
			authorizationCodeButton.Clicked += AuthorizationCodeButtonClicked;
			getProfileButton.Clicked += GetProfileButtonClicked;
			refreshButton.Clicked += RefreshButtonClicked;

			store = AccountStore.Create ();
			account = store.FindAccountsForService (ServiceId).FirstOrDefault ();

			if (account != null)
			{
				statusText.Text = "Restored previous session";
				getProfileButton.IsEnabled = true;
				refreshButton.IsEnabled = true;
			}
		}

		void ImplicitButtonClicked (object sender, EventArgs e)
		{
			var authenticator = new OAuth2Authenticator
				(
					ServerInfo.ClientId,
					Scope,
					ServerInfo.AuthorizationEndpoint,
					ServerInfo.RedirectionEndpoint
				);

			authenticator.Completed += OnAuthCompleted;
			authenticator.Error += OnAuthError;

			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login (authenticator);
		}
			
		void AuthorizationCodeButtonClicked (object sender, EventArgs e)
		{
			var authenticator = new OAuth2Authenticator
				(
					ServerInfo.ClientId,
					ServerInfo.ClientSecret,
					Scope,
					ServerInfo.AuthorizationEndpoint,
					ServerInfo.RedirectionEndpoint,
					ServerInfo.TokenEndpoint
				);

			authenticator.Completed += OnAuthCompleted;
			authenticator.Error     += OnAuthError;

			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login (authenticator);
		}

		async void GetProfileButtonClicked (object sender, EventArgs e)
		{
			try
			{
				var request = new OAuth2Request("GET", ServerInfo.ApiEndpoint, null, account);
				var response = await request.GetResponseAsync();

				var text = response.GetResponseText();

				var json = JObject.Parse(text);

				var name = (string)json["Name"];
				var email = (string)json["Email"];
				var imageUrl = (string)json["ImageUrl"];

				nameText.Text  = name;
				emailText.Text = email;

				var imageRequest = new OAuth2Request("GET", new Uri(imageUrl), null, account);
				var stream = await (await imageRequest.GetResponseAsync()).GetResponseStreamAsync();

				profileImage.Source = ImageSource.FromStream ( ()=> stream );

				statusText.Text = "Get data succeeded";
			}
			catch (Exception x)
			{
				getProfileButton.IsEnabled = false;
				statusText.Text = "Get data failure: " + x.Message + "\r\nHas the access token expired?";
			}
		}

		async void RefreshButtonClicked (object sender, EventArgs e)
		{
			var refreshToken = account.Properties["refresh_token"];
			
			if (string.IsNullOrWhiteSpace(refreshToken))
				return;
			
			var queryValues = new Dictionary<string, string> 
			{
				{"refresh_token", refreshToken},
				{"client_id", ServerInfo.ClientId},
				{"grant_type", "refresh_token"},
				{"client_secret", ServerInfo.ClientSecret},
			};
			
			var authenticator = new OAuth2Authenticator
				(
					ServerInfo.ClientId,
					ServerInfo.ClientSecret,
					"profile",
					ServerInfo.AuthorizationEndpoint,
					ServerInfo.RedirectionEndpoint,
					ServerInfo.TokenEndpoint
				);

			try
			{
				var result = await authenticator.RequestAccessTokenAsync(queryValues);

				if (result.ContainsKey("access_token"))
					account.Properties["access_token"] = result["access_token"];

				if (result.ContainsKey("refresh_token"))
					account.Properties["refresh_token"] = result["refresh_token"];

				store.Save(account, ServiceId);

				statusText.Text = "Refresh succeeded";
			}
			catch (Exception ex) 
			{
				statusText.Text = "Refresh failed " + ex.Message;
			}
		}

		void OnAuthCompleted (object sender, AuthenticatorCompletedEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;

			if (authenticator != null) 
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error 	-= OnAuthError;
			}

			if (e.IsAuthenticated)
			{
				getProfileButton.IsEnabled = true;

				if (this.account != null)
					store.Delete (this.account, ServiceId);

				store.Save(account = e.Account, ServiceId);

				getProfileButton.IsEnabled = true;

				if (account.Properties.ContainsKey ("expires_in")) 
				{
					var expires = int.Parse (account.Properties ["expires_in"]);
					statusText.Text = "Token lifetime is: " + expires + "s";
				} 
				else 
				{
					statusText.Text = "Authentication succeeded";
				}

				if(account.Properties.ContainsKey("refresh_token"))
					refreshButton.IsEnabled = true;
			}
			else
			{
				statusText.Text = "Authentication failed";
			}
		}

		void OnAuthError (object sender, AuthenticatorErrorEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;

			if (authenticator != null) 
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error 	-= OnAuthError;
			}

			statusText.Text = "Authentication error: " + e.Message;
		}
	}
}