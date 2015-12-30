using System;
using System.Collections.Generic;
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
	    const string HOST = "http://192.168.0.2";

        void LoginToFacebook (bool allowCancel)
		{
			var auth = new DelegatedOAuth2Authenticator (
				authorizeUrl: new Uri($"{HOST}/Account/Login"),
                redirectUrl: new Uri(HOST),
                clientId: "<theclientid>",
                clientSecret:"<theclientsecret>",
                getUsernameAsync: GetUserNameAsync
                );

			auth.AllowCancel = allowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += (s, ee) => {
				if (!ee.IsAuthenticated) {
					var builder = new AlertDialog.Builder (this);
					builder.SetMessage ("Not Authenticated");
					builder.SetPositiveButton ("Ok", (o, e) => { });
					builder.Create().Show();
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = new OAuth2Request("GET", new Uri($"{HOST}/api/accountapi/userinfo"), null, ee.Account);
				request.GetResponseAsync().ContinueWith (t => {
					var builder = new AlertDialog.Builder (this);
					if (t.IsFaulted) {
						builder.SetTitle ("Error");
						builder.SetMessage (t.Exception.Flatten().InnerException.ToString());
					} else if (t.IsCanceled)
						builder.SetTitle ("Task Canceled");
					else {
						var obj = JsonValue.Parse (t.Result.GetResponseText());

						builder.SetTitle ("Logged in");
						builder.SetMessage ("Name: " + obj["userName"]);
					}

					builder.SetPositiveButton ("Ok", (o, e) => { });
					builder.Create().Show();
				}, UIScheduler);
			};

			var intent = auth.GetUI (this);
			StartActivity (intent);
		}

	    private Task<string> GetUserNameAsync(IDictionary<string, string> accountproperties)
	    {
            var request = new OAuth2Request("GET", new Uri($"{HOST}/api/accountapi/userinfo"), null, new Account(string.Empty, accountproperties));

            var tcs = new TaskCompletionSource<string>();
            request.GetResponseAsync().ContinueWith(t => {
                if (t.IsFaulted) tcs.SetException(t.Exception.Flatten().InnerException);
                else if (t.IsCanceled) tcs.SetCanceled();
                else 
                {
                    var obj = JsonValue.Parse(t.Result.GetResponseText());

                    // also add additional account properties
                    accountproperties.Add("TenancyName", obj["tenancyName"]);
                    accountproperties.Add("Tenant", obj["tenant"]);
                    accountproperties.Add("TenantId", obj["tenantId"]?.ToString());

                    tcs.SetResult(obj["userName"]);
                }
            });

	        return tcs.Task;
	    }

	    private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			var facebook = FindViewById<Button> (Resource.Id.FacebookButton);			
			facebook.Click += delegate { LoginToFacebook(true);};

			var facebookNoCancel = FindViewById<Button> (Resource.Id.FacebookButtonNoCancel);
			facebookNoCancel.Click += delegate { LoginToFacebook(false);};
		}
	}
}