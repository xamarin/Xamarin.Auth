using System;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;

using global::Xamarin.Auth;

namespace Xamarin.Auth.Sample.Android
{
    [Activity(Label = "Xamarin.Auth Sample (Android)", MainLauncher = true)]
    public class MainActivity : Activity
    {
        void LoginToFacebook(bool allowCancel)
        {
            var auth = new OAuth2Authenticator
                            (
                                clientId: "App ID from https://developers.facebook.com/apps",
                                scope: "",
                                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html")
                            );

            auth.AllowCancel = allowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += async (s, ee) =>
            {
                var builder = new AlertDialog.Builder(this);

                if (!ee.IsAuthenticated)
                    builder.SetMessage("Not Authenticated");
                else
                {
                    // Now that we're logged in, make a OAuth2 request to get the user's info.
                    var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, ee.Account);
                    try
                    {
                        Response response = await request.GetResponseAsync();
                        var obj = JsonValue.Parse(await response.GetResponseTextAsync());
                        builder.SetTitle("Logged in");
                        builder.SetMessage("Name: " + obj["name"]);
                    }
                    catch (System.OperationCanceledException)
                    {
                        builder.SetTitle("Task Canceled");
                    }
                    catch (Exception ex)
                    {
                        builder.SetTitle("Error");
                        builder.SetMessage(ex.ToString());
                    }
                }

                builder.SetPositiveButton("Ok", (o, e) => { });
                builder.Create().Show();
            };

            object intent = auth.GetUI(this);
            StartActivity((global::Android.Content.Intent)intent);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var facebook = FindViewById<Button>(Resource.Id.FacebookButton);
            facebook.Click += delegate { LoginToFacebook(true); };

            var facebookNoCancel = FindViewById<Button>(Resource.Id.FacebookButtonNoCancel);
            facebookNoCancel.Click += delegate { LoginToFacebook(false); };
        }
    }
}