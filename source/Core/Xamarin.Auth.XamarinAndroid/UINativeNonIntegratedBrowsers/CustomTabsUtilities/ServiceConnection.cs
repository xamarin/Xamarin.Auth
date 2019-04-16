// Copyright 2015 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if ! AZURE_MOBILE_SERVICES
namespace Android.Support.CustomTabs.Chromium.SharedUtilities
#else
namespace Android.Support.CustomTabs.Chromium.SharedUtilities._MobileServices
#endif
{

    using ComponentName = Android.Content.ComponentName;
    using CustomTabsClient = Android.Support.CustomTabs.CustomTabsClient;
    using CustomTabsServiceConnection = Android.Support.CustomTabs.CustomTabsServiceConnection;

    /// <summary>
    /// Implementation for the CustomTabsServiceConnection that avoids leaking the
    /// ServiceConnectionCallback
    /// </summary>
    #if XAMARIN_CUSTOM_TABS_INTERNAL
    internal class ServiceConnection : CustomTabsServiceConnection
    #else
    public class ServiceConnection : CustomTabsServiceConnection
    #endif
    {
        // A weak reference to the ServiceConnectionCallback to avoid leaking it.
        private System.WeakReference<IServiceConnectionCallback> mConnectionCallback;

        public ServiceConnection(IServiceConnectionCallback connectionCallback)
        {
            mConnectionCallback = new System.WeakReference<IServiceConnectionCallback>(connectionCallback);
        }

        public override void OnCustomTabsServiceConnected(ComponentName name, CustomTabsClient client)
        {
            IServiceConnectionCallback connectionCallback = null; //mConnectionCallback.Get();
            mConnectionCallback.TryGetTarget(out connectionCallback);//.Get();

            if (connectionCallback != null)
            {
                connectionCallback.OnServiceConnected(client);
            }
        }

        public override void OnServiceDisconnected(ComponentName name)
        {
            IServiceConnectionCallback connectionCallback = null; // mConnectionCallback.Get();
            mConnectionCallback.TryGetTarget(out connectionCallback);

            if (connectionCallback != null)
            {
                connectionCallback.OnServiceDisconnected();
            }

            return;
        }
    }

}