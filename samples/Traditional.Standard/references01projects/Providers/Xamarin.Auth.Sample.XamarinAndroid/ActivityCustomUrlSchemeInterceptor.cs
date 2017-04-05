
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
    //=================================================================
    [Activity(Label = "ActivityCustomUrlSchemeInterceptor")]
    // Walthrough Step 4
    //      Intercepting/Catching/Detecting [redirect] url change 
    //      App Linking / Deep linking - custom url schemes
    //      
    // 
    [
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
    //=================================================================
    {
        string message;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Create your application here

            //=================================================================
            // Walthrough Step 4.1
            //      Parsing intercepted/caught/detected [redirect] url change 
            global::Android.Net.Uri uri_android = Intent.Data;

            System.Uri uri = new Uri(uri_android.ToString());
            IDictionary<string, string> fragment = Utilities.WebEx.FormDecode(uri.Fragment);

            Account account = new Account
                                    (
                                        "???",  // whatever
                                        new Dictionary<string, string>(fragment)
                                    );

            AuthenticatorCompletedEventArgs args_completed = new AuthenticatorCompletedEventArgs(account);
            //=================================================================

            //=================================================================
            // Walthrough Step 5
            //      Raise/Trigger Events 
            //          OnCompleted is triggered by OnSucceeded
            //          OnError
            //          OnCanceled
            if (MainActivity.Auth2 != null)
            {
                // call OnSucceeded to trigger OnCompleted event
                // CHECK: OnSucceeded loads redirect_url in a webview
                // TODO: stop loading redirect url in OnSucceeded
                MainActivity.Auth2.OnSucceeded(account);
            }
            else if (MainActivity.Auth1 != null)
            {
                // call OnSucceeded to trigger OnCompleted event
                // CHECK: OnSucceeded loads redirect_url in a webview
                // TODO: stop loading redirect url in OnSucceeded
                MainActivity.Auth1.OnSucceeded(account);
            }
            else
            {
                throw new ArgumentException("OAuth Helper Object not recognized");
            }
            //=================================================================

            //base.OnBackPressed();
            this.Finish();

            return;
        }
    }
}
