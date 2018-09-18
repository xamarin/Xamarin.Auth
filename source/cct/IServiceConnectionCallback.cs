using Android.Support.CustomTabs;

namespace Xamarin.Auth
{
    internal interface IServiceConnectionCallback
    {
        void OnServiceConnected(CustomTabsClient client);

        void OnServiceDisconnected();
    }
}
