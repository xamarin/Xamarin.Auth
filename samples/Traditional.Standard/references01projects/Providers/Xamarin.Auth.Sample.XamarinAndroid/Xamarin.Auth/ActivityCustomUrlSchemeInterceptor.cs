
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
                        "com.xamarin.traditional.standard.samples.oauth.providers.android",
                        "com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn",
                        /*
                        "urn:ietf:wg:oauth:2.0:oob",
                        "urn:ietf:wg:oauth:2.0:oob.auto",
                        "http://localhost:PORT",
                        "https://localhost:PORT",
                        "http://127.0.0.1:PORT",
                        "https://127.0.0.1:PORT",              
                        "http://[::1]:PORT", 
                        "https://[::1]:PORT", 
                        */
                    },
            //DataHost = "localhost",
            DataPath = "/oauth2redirect"
        )
    ]
    public class ActivityCustomUrlSchemeInterceptor : Activity
    //=================================================================
    {
        string message;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

#if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("ActivityCustomUrlSchemeInterceptor.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(uri_android.ToString());

            // load redirect_url Page
            MainActivity.Auth2?.OnPageLoading(uri_netfx);
            MainActivity.Auth1?.OnPageLoading(uri_netfx);

            this.Finish();

            return;
        }
    }
}
