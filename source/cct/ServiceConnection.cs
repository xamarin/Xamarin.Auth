using Android.Content;
using Android.Support.CustomTabs;
using System;

namespace Xamarin.Auth
{
    internal class ServiceConnection : CustomTabsServiceConnection
    {
        private WeakReference<IServiceConnectionCallback> callbackRef;

        public ServiceConnection(IServiceConnectionCallback connectionCallback)
        {
            callbackRef = new WeakReference<IServiceConnectionCallback>(connectionCallback);
        }

        public override void OnCustomTabsServiceConnected(ComponentName name, CustomTabsClient client)
        {
            if (callbackRef.TryGetTarget(out var callback))
                callback.OnServiceConnected(client);
        }

        public override void OnServiceDisconnected(ComponentName name)
        {
            if (callbackRef.TryGetTarget(out var connectionCallback))
                connectionCallback.OnServiceDisconnected();
        }
    }
}
