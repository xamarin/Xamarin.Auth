using Android.App;
using Android.Webkit;

namespace Xamarin.Auth
{
    public static class WebViewConfiguration
    {
        static WebViewConfiguration()
        {
            using (var webView = new WebView(Application.Context))
            {
                UserAgent = webView.Settings.UserAgentString;
            }
        }

        public static string UserAgent { get; set; }
    }
}
