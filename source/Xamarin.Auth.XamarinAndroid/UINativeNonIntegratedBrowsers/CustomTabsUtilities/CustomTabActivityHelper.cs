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

using System.Collections.Generic;

using Android.App;
using Android.Net;
using Android.OS;
using Android.Support.CustomTabs;
using Android.Widget;

namespace Android.Support.CustomTabs.Chromium.SharedUtilities
{
    /// <summary>
    /// This is a helper class to manage the connection from Activity to the CustomTabs 
    /// Service.
    /// </summary>
    public partial class CustomTabActivityHelper : Java.Lang.Object, IServiceConnectionCallback
    {
        private CustomTabsSession custom_tabs_session;
        private CustomTabsClient custom_tabs_client;
        private CustomTabsServiceConnection custom_tabs_service_connection;
        private IConnectionCallback connection_callback;

		/// <summary>
		/// Opens the URL on a Custom Tab if possible. Otherwise fallsback to opening it on a WebView.
		/// </summary>
		/// <param name="activity"> The host activity. </param>
		/// <param name="custom_tabs_intent"> a CustomTabsIntent to be used if Custom Tabs is available. </param>
		/// <param name="uri"> the Uri to be opened. </param>
		/// <param name="fallback"> a CustomTabFallback to be used if Custom Tabs is not available. </param>
		public /*static*/ void LaunchUrlWithCustomTabsOrFallback
								(
									Activity activity,
									CustomTabsIntent custom_tabs_intent,
									Uri uri,
									ICustomTabFallback fallback
								)
		{
			string packageName = PackageManagerHelper.GetPackageNameToUse(activity, uri.ToString());

			//If we cant find a package name, it means theres no browser that supports
			//Chrome Custom Tabs installed. So, we fallback to the webview
			if (packageName == null)
			{
				if (fallback != null)
				{
					fallback.OpenUri(activity, uri);
				}
			}
			//else
			{
				custom_tabs_intent.Intent.SetPackage(packageName);
				custom_tabs_intent.LaunchUrl(activity, uri);
			}

			return;
		}

		/// <summary>
		/// Creates or retrieves an exiting CustomTabsSession.
		/// </summary>
		/// <returns> a CustomTabsSession. </returns>
		public virtual CustomTabsSession Session
        {
            get
            {
                if (custom_tabs_client == null)
                {
                    custom_tabs_session = null;
                }
                else if (custom_tabs_session == null)
                {
                    /*
                    public CustomTabsSession NewSession(OnNavigationEventDelegate onNavigationEventHandler);
                    public virtual CustomTabsSession NewSession(CustomTabsCallback callback);
                    public CustomTabsSession NewSession
                                                (
                                                    OnNavigationEventDelegate onNavigationEventHandler,
                                                    CustomTabsClient.ExtraCallbackDelegate extraCallbackHandler
                                                );
                    */
                    custom_tabs_session = custom_tabs_client.NewSession
                                                (
                                                    // OnNavigationEventDelegate onNavigationEventHandler
                                                    default(CustomTabsClient.OnNavigationEventDelegate)
                                                    // not available in 23.3.0
                                                    // downgraded from 25.1.1. because of Xamarin.Forms support 23.3.0 
                                                    // CustomTabsClient.ExtraCallbackDelegate extraCallbackHandler
                                                    //null
                                                );
                }
                return custom_tabs_session;
            }
        }

        public CustomTabsClient.OnNavigationEventDelegate NavigationEventHandler
        {
            get;
            set;
        } = null;

        /*
            not available in 23.3.0
            downgraded from 25.1.1. because of Xamarin.Forms support 23.3.0 
        <!--
            <package id = "Xamarin.Android.Support.CustomTabs" version="23.3.0" targetFramework="monoandroid71" />
        -->        
        public CustomTabsClient.ExtraCallbackDelegate ExtraCallback
        {
            get;
            set;
        } = null;
        */

        /// <summary>
        /// Register a Callback to be called when connected or disconnected from the Custom Tabs Service. 
        /// </summary>
        public virtual IConnectionCallback ConnectionCallback
        {
            get;
            set;
        }

        public string UriTest
        {
            get;
            set;
        } = "http://xamarin.com";

        /// <summary>
        /// Binds the Activity to the Custom Tabs Service. </summary>
        /// <param name="activity"> the activity to be binded to the service. </param>
        public virtual void BindCustomTabsService(Activity activity)
        {
            if (custom_tabs_client != null)
            {
                return;
            }

            string packageName = PackageManagerHelper.GetPackageNameToUse(activity, this.UriTest);

            if (packageName == null)
            {
                Toast.MakeText
                        (
                            activity, 
                            "No packages supporting CustomTabs found!", 
                            ToastLength.Short
                        ).Show();

                return;
            }

            custom_tabs_service_connection = new ServiceConnection(this);
            CustomTabsClient.BindCustomTabsService(activity, packageName, custom_tabs_service_connection);

            return;
        }

        /// <summary>
        /// Unbinds the Activity from the Custom Tabs Service. </summary>
        /// <param name="activity"> the activity that is connected to the service. </param>
        public virtual void UnbindCustomTabsService(Activity activity)
        {
            if (custom_tabs_service_connection == null)
            {
                return;
            }
            activity.UnbindService(custom_tabs_service_connection);
            custom_tabs_client = null;
            custom_tabs_session = null;
            custom_tabs_service_connection = null;

            return;
        }

        /// <summary>
        /// Warmup 
        /// Tells the browser of a likely future navigation to a URL.
        /// </summary>
        /// <seealso cref= <seealso cref="CustomTabsSession#mayLaunchUrl(Uri, Bundle, List)"/>. </seealso>
        /// <returns> true if call to mayLaunchUrl was accepted. </returns>
        public virtual bool MayLaunchUrl(Uri uri, Bundle extras, IList<Bundle> other_likely_bundles)
        {
            if (custom_tabs_client == null)
            {
                return false;
            }

            CustomTabsSession cts = this.Session;

            if (cts == null)
            {
                return false;
            }

            return cts.MayLaunchUrl(uri, extras, other_likely_bundles);
        }

        public virtual void OnServiceConnected(CustomTabsClient client)
        {
            custom_tabs_client = client;
            custom_tabs_client.Warmup(0L);
            if (connection_callback != null)
            {
                connection_callback.OnCustomTabsConnected();
            }

            return;
        }

        public virtual void OnServiceDisconnected()
        {
            custom_tabs_client = null;
            custom_tabs_session = null;
            if (connection_callback != null)
            {
                connection_callback.OnCustomTabsDisconnected();
            }

            return;
        }
    }
}