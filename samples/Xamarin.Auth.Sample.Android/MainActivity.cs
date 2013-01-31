using System;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;

namespace Xamarin.Auth.Sample.Android
{
	[Activity (Label = "Xamarin.Auth Sample (Android)", MainLauncher = true)]
	public class MainActivity : Activity
	{
		void LoginToFacebook (object sender, EventArgs e)
		{
			var auth = new OAuth2Authenticator (
				clientId: "App ID from https://developers.facebook.com/apps",
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += (s, ee) => {
				if (!ee.IsAuthenticated) {
					this.facebookStatus.Text = "Not Authenticated";
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, ee.Account);
				request.GetResponseAsync().ContinueWith (t => {
					if (t.IsFaulted)
						this.facebookStatus.Text = "Error: " + t.Exception.InnerException.Message;
					else if (t.IsCanceled)
						this.facebookStatus.Text = "Canceled";
					else
					{
						var obj = JsonValue.Parse (t.Result.GetResponseText());
						this.facebookStatus.Text = "Logged in as " + obj["name"];
					}
				}, uiScheduler);
			};

			var intent = auth.GetUI (this);
			StartActivityForResult (intent, 42);
		}

		private TextView facebookStatus;
		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			this.facebookStatus = FindViewById<TextView> (Resource.Id.FacebookTextView);
			var facebook = FindViewById<Button> (Resource.Id.FacebookButton);			
			facebook.Click += LoginToFacebook;
		}
	}
}