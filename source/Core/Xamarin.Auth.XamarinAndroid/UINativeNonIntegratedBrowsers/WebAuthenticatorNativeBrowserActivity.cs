//
//  Copyright 2012-2016, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;

using Android.OS;
using Android.App;
using Android.Widget;
using Android.Support.CustomTabs;
using Xamarin.Utilities.Android;

using Plugin.Threading;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    [Activity
        (
            Label = "Web Authenticator Native Browser",
            // NoHistory = true,
            LaunchMode = global::Android.Content.PM.LaunchMode.SingleTop
        )
    ]
    #if XAMARIN_AUTH_INTERNAL
    internal partial class WebAuthenticatorNativeBrowserActivity : global::Android.Accounts.AccountAuthenticatorActivity
    #else
    public partial class WebAuthenticatorNativeBrowserActivity : global::Android.Accounts.AccountAuthenticatorActivity
    #endif
    {
        internal class State : Java.Lang.Object
        {
            public WebAuthenticator Authenticator;
        }

        internal static readonly ActivityStateRepository<State> StateRepo = new ActivityStateRepository<State>();

        State state;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //--------------------------------------------------------------
            // Azure Mobile Services Team uses simplified code
            // this is for testing purposes of their launch only
            LaunchCunstomTabsWithUrl =
                    //LaunchCunstomTabsWithUrlDefault
                    LaunchCunstomTabsWithUrlAzureMobileServiceClientTeamCode
                    ;
            //--------------------------------------------------------------

            //
            // Load the state either from a configuration change or from the intent.
            //
            // *
            state = LastNonConfigurationInstance as State;
            if (state == null && Intent.HasExtra("StateKey"))
            {
                var stateKey = Intent.GetStringExtra("StateKey");
                state = StateRepo.Remove(stateKey);
            }

            if (state == null)
            {
                Finish();
                return;
            }

            //Title = state.Authenticator.Title;

            //
            // Watch for completion
            //
            state.Authenticator.Completed +=
                (s, e) =>
                {
                    SetResult(e.IsAuthenticated ? Result.Ok : Result.Canceled);

                    #region
                    ///-------------------------------------------------------------------------------------------------
                    /// Pull Request - manually added/fixed
                    ///		Added IsAuthenticated check #88
                    ///		https://github.com/xamarin/Xamarin.Auth/pull/88
                    if (e.IsAuthenticated)
                    {
                        if (state.Authenticator.GetAccountResult != null)
                        {
                            var accountResult = state.Authenticator.GetAccountResult(e.Account);

                            Bundle result = new Bundle();
                            result.PutString(global::Android.Accounts.AccountManager.KeyAccountType, accountResult.AccountType);
                            result.PutString(global::Android.Accounts.AccountManager.KeyAccountName, accountResult.Name);
                            result.PutString(global::Android.Accounts.AccountManager.KeyAuthtoken, accountResult.Token);
                            result.PutString(global::Android.Accounts.AccountManager.KeyAccountAuthenticatorResponse, e.Account.Serialize());

                            SetAccountAuthenticatorResult(result);
                        }
                    }
                    ///-------------------------------------------------------------------------------------------------
                    #endregion

                    CloseCustomTabs();
                };

            state.Authenticator.Error +=
            (s, e) =>
            {
                if (!state.Authenticator.ShowErrors)
                    return;

                if (e.Exception != null)
                {
                    this.ShowError("Authentication Error e.Exception = ", e.Exception);
                }
                else
                {
                    this.ShowError("Authentication Error e.Message = ", e.Message);
                }
                BeginLoadingInitialUrl();
            };

            // Build the UI
            CustomTabsConfiguration.Initialize(this);
            CustomTabsConfiguration.UICustomization();

            LaunchCunstomTabsWithUrl();

            return;
        }

        public Action LaunchCunstomTabsWithUrl
        {
            get;
            set;
        }

        private void LaunchCunstomTabsWithUrlAzureMobileServiceClientTeamCode()
        {
            CustomTabsConfiguration
                .CustomTabActivityHelper
                    .LaunchUrlWithCustomTabsOrFallback
                        (
                            // Activity/Context
                            this,
                            // CustomTabIntent
                            CustomTabsConfiguration.CustomTabsIntent,
                            CustomTabsConfiguration.PackageForCustomTabs,
                            CustomTabsConfiguration.UriAndroidOS,
                            //  Fallback if CustomTabs do not exist
                            CustomTabsConfiguration.WebViewFallback
                        );

            return;
        }

        public void LaunchCunstomTabsWithUrlDefault()
        {
            //.......................................................
            // Launching CustomTabs and url - minimal
            if
                (
                    CustomTabsConfiguration.CustomTabActivityHelper != null
                    &&
                    CustomTabsConfiguration.CustomTabsIntent != null
                    &&
                    CustomTabsConfiguration.UriAndroidOS != null
                )
            {
                CustomTabsConfiguration
                    .CustomTabsIntent
                    .Intent.AddFlags(CustomTabsConfiguration.ActivityFlags);

                CustomTabsConfiguration
                    .CustomTabActivityHelper
                        .LaunchUrlWithCustomTabsOrFallback
                            (
                                // Activity/Context
                                this,
                                // CustomTabInten
                                CustomTabsConfiguration.CustomTabsIntent,
                                CustomTabsConfiguration.PackageForCustomTabs,
                                CustomTabsConfiguration.UriAndroidOS,
                                //  Fallback if CustomTabs do not exis
                                CustomTabsConfiguration.WebViewFallback
                            );
            }
            else
            {
                // plain CustomTabs no customizations
                CustomTabsIntent i = new CustomTabsIntent.Builder().Build();
                i.Intent.AddFlags(CustomTabsConfiguration.ActivityFlags);

                i.LaunchUrl(this, CustomTabsConfiguration.UriAndroidOS);
            }
            //.......................................................
            // Launching CustomTabs and url - if WarmUp and Prefetching is used
            /*
            */
            //---------------------------------------------------------------------------------

            //
            // Restore the UI state or start over
            //
            /*
            if (savedInstanceState != null)
            {
                //webView.RestoreState(savedInstanceState);
            }
            else
            {
                if (Intent.GetBooleanExtra("ClearCookies", true))
                {
                    WebAuthenticator.ClearCookies();
                }
                BeginLoadingInitialUrl();
            }
            */

            return;
        }

        private bool customTabsShown = false;

        protected override void OnPause()
        {
            base.OnPause();
            customTabsShown = true;

            return;
        }

        #region
        ///-------------------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///     Added IsAuthenticated check #88
        ///     https://github.com/xamarin/Xamarin.Auth/pull/88
        protected override void OnResume()
        {
            base.OnResume();
            if
                (
                    state.Authenticator.AllowCancel
                    &&
                    // mc++ state.Authenticator.IsAuthenticated()   // Azure Mobile Services Client fix
                    customTabsShown                                 // Azure Mobile Services Client fix
                )
            {
                state.Authenticator.OnCancelled();
            }

            customTabsShown = false;

            return;
        }
        ///-------------------------------------------------------------------------------------------------
        #endregion
        protected void CloseCustomTabs()
        {
            UIThreadRunInvoker ri = new UIThreadRunInvoker(this);
            ri.BeginInvokeOnUIThread
                    (
                        () =>
                        {
                            string msg = CustomTabsConfiguration.CustomTabsClosingMessage;

                            if (msg != null)
                            {
                                Toast.MakeText(this, msg, ToastLength.Short).Show();
                            }
                        }
                    );

            #if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"      CloseCustomTabs");
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            this.Finish();
            //this.CloseCustomTabsProcessKill();

            return;
        }

        protected void CloseCustomTabsProcessKill()
        {
            System.Diagnostics.Debug.WriteLine($"      CloseCustomTabs");
            ;
            ActivityManager manager = GetSystemService(global::Android.Content.Context.ActivityService) as ActivityManager;
            List<ActivityManager.RunningAppProcessInfo> processes = manager.RunningAppProcesses.ToList();
            //List<ActivityManager.RunningTaskInfo> tasks = (manager.Get().ToList();

            foreach (ActivityManager.RunningAppProcessInfo process in processes)
            {
                String name = process.ProcessName;

                System.Diagnostics.Debug.WriteLine($"      process");
                System.Diagnostics.Debug.WriteLine($"          .Pid = {process.Pid}");
                System.Diagnostics.Debug.WriteLine($"          .ProcessName = {process.ProcessName}");

                if
                    (
                        name.Contains("com.android.browser")
                    )
                {
                    int pid = process.Pid;
                    Process.KillProcess(pid);
                }

            }

            return;
        }


        void BeginLoadingInitialUrl()
        {
            state.Authenticator.GetInitialUrlAsync().ContinueWith
                (
                    t =>
                    {
                        if (t.IsFaulted)
                        {
                            if (!state.Authenticator.ShowErrors)
                                return;

                            this.ShowError("Authentication Error t.Exception = ", t.Exception);
                        }
                        else
                        {
                            //TODO: webView.LoadUrl(t.Result.AbsoluteUri);
                        }
                    }, 
                    System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext()
                );
        }

        public override void OnBackPressed()
        {
            if (state.Authenticator.AllowCancel)
            {
                state.Authenticator.OnCancelled();
            }

            this.Finish();

            return;
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            return state;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            // TODO: webView.SaveState(outState);
        }

        void BeginProgress(string message)
        {
            // TODO: webView.Enabled = false;
        }

        void EndProgress()
        {
            // TODO: webView.Enabled = true;
        }

    }
}

