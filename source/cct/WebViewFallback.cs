using Android.App;
using Android.Content;
using Android.Net;

namespace Xamarin.Auth
{
    internal class WebViewFallback : ICustomTabFallback
    {
        public virtual void OpenUri(Activity activity, Uri uri)
        {
            OpenUriXamarinAuthWebViewActivity(activity, uri);
        }

        protected void OpenUriSimple(Activity activity, Uri uri)
        {
            var intent = new Intent(Intent.ActionView);
            intent.SetData(uri);
            activity.StartActivity(intent);
        }

        protected void OpenUriXamarinAuthWebViewActivity(Activity activity, Uri uri)
        {
            activity.StartActivity(WebViewActivity.CreateIntent(activity, uri));
        }
    }
}
