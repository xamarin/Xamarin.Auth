using Android.App;
using Android.Net;

namespace Xamarin.Auth
{
    internal interface ICustomTabFallback
    {
        void OpenUri(Activity activity, Uri uri);
    }
}
