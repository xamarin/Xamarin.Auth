
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xamarin.Auth.Sample.XamarinAndroid
{
    [Activity(Label = "ActivityCustomUrlSchemeInterceptor")]
	[
        // App Linking - custom url schemes
        IntentFilter
        (
		    actions: new[] { Intent.ActionView },
		    Categories = new[] 
                    { 
                        Intent.CategoryDefault, 
                        Intent.CategoryBrowsable 
                    },
            DataSchemes = new[]
                    {
                        "xamarinauth",
						"xamarin-auth",
						"xamarin.auth",
					},
		    DataHost = "localhost"
        )
    ]
    public class ActivityCustomUrlSchemeInterceptor : Activity
    {
        string message;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            global::Android.Net.Uri uri_android = Intent.Data;


			System.Uri uri = new Uri(uri_android.ToString());
			IDictionary<string, string> fragment = Utilities.WebEx.FormDecode(uri.Fragment);

			Account account = new Account
									(
										"username",
										new Dictionary<string, string>(fragment)
									);

			AuthenticatorCompletedEventArgs args_completed = new AuthenticatorCompletedEventArgs(account);

			if (MainActivity.Auth2 != null)
			{
				// call OnSucceeded to trigger OnCompleted event
				MainActivity.Auth2.OnSucceeded(account);
			}
			else if (MainActivity.Auth1 != null)
			{
				// call OnSucceeded to trigger OnCompleted event
				MainActivity.Auth1.OnSucceeded(account);
			}
			else
			{
			}

            this.Finish();

			return;
		}
    }
}
