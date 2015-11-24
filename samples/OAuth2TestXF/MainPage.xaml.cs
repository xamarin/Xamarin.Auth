using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Auth.XamarinForms;
using Xamarin.Auth;

namespace OAuth2TestXF
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			await AuthorizeAsync ();
		}

		private async Task AuthorizeAsync ()
		{
			var clientId = "65e36yb9emmjws4"; 	
			var scope = "";	
			var authorizeUrl = new Uri("https://www.dropbox.com/1/oauth2/authorize"); 	
			var redirectUrl = new Uri("https://www.xamarin.com/login/callback"); 

			//login page
			var login = new Xamarin.Auth.XamarinForms.LoginPage (clientId, scope, authorizeUrl, redirectUrl);

			login.Completed += OnLoginCompleted;

			await Navigation.PushAsync (login);
		}

		void OnLoginCompleted (object sender, AuthenticatorCompletedEventArgs args)
		{
			bool success = args.IsAuthenticated;

			var acount = args.Account;
		}
	}
}

