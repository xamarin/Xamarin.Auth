using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;

namespace AndroidApp1
{
    [Activity(Label = "AndroidApp1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.buttonLogin);

			button.Click += buttonLogin_Click;

			return;
		}


        private void buttonLogin_Click(object sender, EventArgs e)
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

            Intent i = auth.GetUI(this);
			StartActivity(i);

			return;
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

			Toast.MakeText(this, "Token = " + t, ToastLength.Long).Show();

			return;
        }

    }
}

