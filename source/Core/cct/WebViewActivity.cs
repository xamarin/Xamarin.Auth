using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace Xamarin.Auth
{
    [Activity(Label = "@string/authenticator_activity_label", Theme = "@android:style/Theme.DeviceDefault")]
    public class WebViewActivity : Activity
    {
        public const string EXTRA_URL = "extra.url";

        public static Intent CreateIntent(Context context, global::Android.Net.Uri uri)
        {
            var intent = new Intent(context, typeof(WebViewActivity));
            intent.PutExtra(EXTRA_URL, uri.ToString());
            return intent;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_webview);

            var url = Intent.GetStringExtra(EXTRA_URL);

            if (string.IsNullOrEmpty(url))
            {
                Finish();
                return;
            }

            Title = url;

            var webView = FindViewById<WebView>(Resource.Id.webview);
            webView.SetWebViewClient(new WebViewClient());
            var webSettings = webView.Settings;
            webSettings.JavaScriptEnabled = true;

            webView.LoadUrl(url);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // respond to the action bar's Up/Home button
                case global::Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
