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

using System;
using System.Collections.Generic;

using Android.App;
using Android.Net;
using Android.OS;
using Android.Widget;
using Android.Support.CustomTabs;
using Xamarin.Auth;

#if !AZURE_MOBILE_SERVICES
namespace Android.Support.CustomTabs.Chromium.SharedUtilities
#else
namespace Android.Support.CustomTabs.Chromium.SharedUtilities._MobileServices
#endif
{
    /// <summary>
    /// This is a helper class to manage the connection from Activity to the CustomTabs 
    /// Service.
    /// </summary>
    #if XAMARIN_CUSTOM_TABS_INTERNAL
    internal partial class CustomTabActivityHelper : Java.Lang.Object, IServiceConnectionCallback
    #else
    public partial class CustomTabActivityHelper : Java.Lang.Object, IServiceConnectionCallback
    #endif
    {
        private CustomTabsSession custom_tabs_session;
        private CustomTabsClient custom_tabs_client;
        private CustomTabsServiceConnection custom_tabs_service_connection;
        private CustomTabsActivityManager custom_tabs_activity_manager;
        private IConnectionCallback connection_callback;
        private CustomTabsIntent.Builder custom_tabs_intent_builder = null;
        private Activity activity;
        private Android.Net.Uri uri = null;
        private bool service_bound = false;

        private string packageName = null;

        public CustomTabActivityHelper()
        {
            this.NavigationEventHandler = NavigationEventHandlerDefaultImplementation;

            return;
        }

		/// <summary>
		/// Opens the URL on a Custom Tab if possible. Otherwise fallsback to opening it on a WebView.
		/// </summary>
		/// <param name="activity"> The host activity. </param>
		/// <param name="custom_tabs_intent"> a CustomTabsIntent to be used if Custom Tabs is available. </param>
		/// <param name="uri"> the Uri to be opened. </param>
		/// <param name="fallback"> a CustomTabFallback to be used if Custom Tabs is not available. </param>
		public /*static*/ void LaunchUrlWithCustomTabsOrFallback
                                (
                                    Activity a,
                                    CustomTabsIntent custom_tabs_intent,
                                    string package_name_for_custom_tabs,
                                    Android.Net.Uri u,
                                    ICustomTabFallback fallback
                                )
        {
			uri = u;
			activity = a;
            bool fallback_neccessary = false;

			//If we cant find a package name, it means theres no browser that supports
			//Chrome Custom Tabs installed. So, we fallback to the webview
			if (package_name_for_custom_tabs == null)
            {
                fallback_neccessary = true;
            }
            else
            {
                custom_tabs_activity_manager = new CustomTabsActivityManager(this.activity);

                custom_tabs_activity_manager.BindService(package_name_for_custom_tabs);
                //custom_tabs_intent.Intent.SetPackage(package_name_for_custom_tabs);

                custom_tabs_session = custom_tabs_activity_manager.Session;

                custom_tabs_intent_builder = new CustomTabsIntent.Builder(custom_tabs_session);


                // direct call to CustomtTasIntent.LaunchUrl was ported from java samples and refactored
                // seems like API changed
                //------------------------------------------------------------------------------
                //custom_tabs_intent.LaunchUrl(activity, uri);
                //return;
                //------------------------------------------------------------------------------
                custom_tabs_activity_manager = new CustomTabsActivityManager(activity);
				custom_tabs_intent = custom_tabs_intent_builder.Build();

				CustomTabsHelper.AddKeepAliveExtra(activity, custom_tabs_intent.Intent);

                //custom_tabs_intent.LaunchUrl(activity, uri);
				custom_tabs_activity_manager.CustomTabsServiceConnected +=
                                // custom_tabs_activit_manager_CustomTabsServiceConnected
                                delegate
                                    {
                                        System.Diagnostics.Debug.WriteLine("CustomTabsActivityManager.CustomTabsServiceConnected");

                                        custom_tabs_activity_manager.LaunchUrl(uri.ToString());

                                        System.Diagnostics.Debug.WriteLine("CustomTabsActivityManager.LaunchUrl");

                                        return;
                                    }
                                ;
                System.Diagnostics.Debug.WriteLine($"CustomTabsActivityManager.BindService({package_name_for_custom_tabs})");
                service_bound = custom_tabs_activity_manager.BindService(package_name_for_custom_tabs);
                //custom_tabs_activity_manager.LaunchUrl(uri.ToString());
                //------------------------------------------------------------------------------

                if (service_bound == false)
                {
                    System.Diagnostics.Debug.WriteLine($"FALLBACK: No Packages that support CustomTabs");
					// No Packages that support CustomTabs
					fallback_neccessary = true;
                }

            }

            if (fallback_neccessary == true && fallback != null)
            {
                fallback.OpenUri(activity, uri);
            }

            return;
        }

        protected void custom_tabs_activit_manager_CustomTabsServiceConnected 
                                    (
                                        Content.ComponentName name, 
                                        CustomTabsClient client
                                    )
        {
            custom_tabs_intent_builder = new CustomTabsIntent.Builder(custom_tabs_activity_manager.Session);

            custom_tabs_intent_builder.EnableUrlBarHiding();

            if (CustomTabsConfiguration.IsWarmUpUsed)
            {
                System.Diagnostics.Debug.WriteLine("CustomTabsActivityManager.WarmUp()");
                client.Warmup(0);
                //custom_tabs_activity_manager.Warmup();
            }

			if (CustomTabsConfiguration.IsPrefetchUsed)
			{
				System.Diagnostics.Debug.WriteLine("CustomTabsActivityManager PREFETCH");
				custom_tabs_activity_manager.MayLaunchUrl(uri.ToString(), null, null);
			}

			if (CustomTabsConfiguration.AreAnimationsUsed)
            {
                custom_tabs_intent_builder.SetStartAnimations
                            (
                                activity,
                                Xamarin.Auth.Resource.Animation.slide_in_right,
                                Xamarin.Auth.Resource.Animation.slide_out_left
                            );
                custom_tabs_intent_builder.SetExitAnimations
                            (
                                activity,
                                global::Android.Resource.Animation.SlideInLeft,
                                global::Android.Resource.Animation.SlideOutRight
                            );
            }

            custom_tabs_activity_manager.LaunchUrl(uri.ToString(), custom_tabs_intent_builder.Build());
            
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
                                                                    HandleOnNavigationEventDelegate
                                                                );
                }

                return custom_tabs_session;
            }
        }

        public CustomTabsClient.OnNavigationEventDelegate NavigationEventHandler
        {
            get;
            set;
        }

        protected void NavigationEventHandlerDefaultImplementation(int navigationEvent, Bundle extras)
        {
            return;
        }

		void HandleOnNavigationEventDelegate(int navigationEvent, Bundle extras)
		{
			return;
		}

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
            };

            List<string> packages = PackageManagerHelper.GetPackageNameToUse(activity, this.UriTest);
            string packageName = "";

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
        public virtual bool MayLaunchUrl
                                (
                                    Android.Net.Uri uri, 
                                    Bundle extras, 
                                    IList<Bundle> other_likely_bundles
                                )
        {
            if (custom_tabs_client == null)
            {
                return false;
            }

            custom_tabs_session = custom_tabs_activity_manager.Session;

            if (custom_tabs_session == null)
            {
                return false;
            }

            return custom_tabs_session.MayLaunchUrl(uri, extras, other_likely_bundles);
        }

        public virtual void OnServiceConnected(CustomTabsClient client)
        {
            System.Diagnostics.Debug.WriteLine("CustomTabsActivityHelper.OnServiceConnected");

            custom_tabs_client = client;
            if (CustomTabsConfiguration.IsWarmUpUsed)
            {
                System.Diagnostics.Debug.WriteLine("    warmup");

                custom_tabs_client.Warmup(0L);
            }
            if (connection_callback != null)
            {
                System.Diagnostics.Debug.WriteLine("    connection_callback.OnCustomTabsConnected()");

                connection_callback.OnCustomTabsConnected();
            }

            return;
        }

        public virtual void OnServiceDisconnected()
        {
            System.Diagnostics.Debug.WriteLine("CustomTabsActivityHelper.OnServiceConnected");

            custom_tabs_client = null;
            custom_tabs_session = null;
            if (connection_callback != null)
            {
                System.Diagnostics.Debug.WriteLine("    connection_callback.OnCustomTabsDisconnected()");

                connection_callback.OnCustomTabsDisconnected();
            }

            return;
        }
    }
}