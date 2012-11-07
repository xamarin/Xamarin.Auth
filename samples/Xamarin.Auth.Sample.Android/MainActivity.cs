using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Xamarin.Auth.Sample.Android
{
	[Activity (Label = "Xamarin.Auth Sample (Android)", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var facebook = FindViewById<Button> (Resource.Id.FacebookButton);			
			facebook.Click += LoginToFacebook;

			var skydrive = FindViewById<Button> (Resource.Id.SkydriveButton);
			skydrive.Click += LoginToSkydrive;
		}

		void LoginToFacebook (object sender, EventArgs e)
		{
			var a = new OAuth2Authenticator (
				clientId: "346691492084618",
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			a.Completed += (s, ee) => {
				var tv = FindViewById<TextView> (Resource.Id.FacebookTextView);
				tv.Text = GetEventAsString (ee);
			};

			var intent = a.GetUI (this);
			StartActivityForResult (intent, 42);
		}

		void LoginToSkydrive (object sender, EventArgs e)
		{
			var a = new OAuth2Authenticator (
				clientId: "00000000440DC040",
				scope: "wl.basic,wl.share,wl.skydrive",
				authorizeUrl: new Uri ("https://login.live.com/oauth20_authorize.srf"),
				redirectUrl: new Uri ("https://login.live.com/oauth20_desktop.srf"));

			a.Completed += (s, ee) => {
				var tv = FindViewById<TextView> (Resource.Id.SkydriveTextView);
				tv.Text = GetEventAsString (ee);
			};

			var intent = a.GetUI (this);
			StartActivityForResult (intent, 42);
		}

		string GetEventAsString (AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated) {
				return e.Account.Serialize ();
			} else {
				return "Not Authenticated";
			}
		}
	}
}


