using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using global::Xamarin.Auth;

[assembly: UsesPermission(Android.Manifest.Permission.Internet)]

namespace Xamarin.Auth.Sample.XamarinAndroid
{
    [Activity
        (
            Label = "Xamarin.Auth.Sample.XamarinAndroid",
            MainLauncher = true,
            Icon = "@drawable/icon"
        )
    ]
    public class MainActivity : ListActivity
    {
        //=================================================================
        // Xamarin.Auth API test switch
        //  true    - Native UI
        //            Android - [Chrome] Custom Tabs
        //            iOS - Safari View Controller
        //  false   - embedded WebViews
        //            Android - WebView
        //            iOS - UIWebView or WKWebView
        bool test_native_ui = true;
        //=================================================================

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //=================================================================
            //  switching between 
            //      embbedded browsers (WebView)
            //  and
            //      Native UI ([Chrome] Custom Tabs)
            //  read the docs about pros and cons
            test_native_ui = true;
            //=================================================================

            ListAdapter = new ArrayAdapter<String>(this, global::Android.Resource.Layout.SimpleListItem1, provider_list);

            InitializeNativeUICustomTabs();

            return;
        }

        string[] provider_list = ProviderSamples.Data.TestCases.Keys.ToArray();

        string provider = null;

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            provider = provider_list[position];

            Xamarin.Auth.ProviderSamples.Helpers.OAuth auth;
            if (!ProviderSamples.Data.TestCases.TryGetValue(provider, out auth))
            {
                Toast.MakeText(this, "Unknown OAuth Provider!", ToastLength.Long);
            }
            if (auth is Xamarin.Auth.ProviderSamples.Helpers.OAuth1)
            {
                Authenticate(auth as Xamarin.Auth.ProviderSamples.Helpers.OAuth1);
            }
            else
            {
                Authenticate(auth as Xamarin.Auth.ProviderSamples.Helpers.OAuth2);
            }

            return;
        }

        global::Android.Support.CustomTabs.Chromium.SharedUtilities.CustomTabActivityHelper custom_tab_activity_helper = null;

        protected override void OnStart()
        {
            base.OnStart();

            // Step 2.2 Customizing the UI - Native UI [OPTIONAL]
            // [Chrome] Custom Tabs WarmUp and prefetch
            custom_tab_activity_helper.BindCustomTabsService(this);

            return;
        }

        protected override void OnStop()
        {
            base.OnStop();

            // Step 2.2 Customizing the UI - Native UI [OPTIONAL]
            // [Chrome] Custom Tabs WarmUp and prefetch
            custom_tab_activity_helper.UnbindCustomTabsService(this);

            return;
        }


        protected void InitializeNativeUICustomTabs()
        {
            // Step 2.2 Customizing the UI - Native UI [OPTIONAL]
            // [Chrome] Custom Tabs WarmUp and prefetch
            custom_tab_activity_helper = new global::Android.Support.CustomTabs.Chromium.SharedUtilities.CustomTabActivityHelper();

            //-----------------------------------------------------------------------------------------------
            // Xamarin.Auth initialization

            // User-Agent tweaks for Embedded WebViews (UIWebView and WKWebView)
            global::Xamarin.Auth.WebViewConfiguration.Android.UserAgent = "moljac++";

            //................................................................
            // Xamarin.Auth CustomTabs Initialization/Customisation
            // Note this API is still under development and subject to changes!

            // CustomTabs closing Toast message (null to turn it of, otherwise define it here)
            global::Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;
            //global::Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = "Closing? Let us know";

            global::Xamarin.Auth.CustomTabsConfiguration.ActionLabel = null;
            global::Xamarin.Auth.CustomTabsConfiguration.MenuItemTitle = null;
            global::Xamarin.Auth.CustomTabsConfiguration.AreAnimationsUsed = true;
            global::Xamarin.Auth.CustomTabsConfiguration.IsShowTitleUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsUrlBarHidingUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsCloseButtonIconUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsActionButtonUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsActionBarToolbarIconUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsDefaultShareMenuItemUsed = false;

            global::Android.Graphics.Color color_xamarin_blue;
            color_xamarin_blue = new global::Android.Graphics.Color(0x34, 0x98, 0xdb);
            global::Xamarin.Auth.CustomTabsConfiguration.ToolbarColor = color_xamarin_blue;
            //................................................................


            // ActivityFlags for tweaking closing of CustomTabs
            // please report findings!
            global::Xamarin.Auth.CustomTabsConfiguration.
               ActivityFlags =
                    global::Android.Content.ActivityFlags.NoHistory
                    |
                    global::Android.Content.ActivityFlags.SingleTop
                    |
                    global::Android.Content.ActivityFlags.NewTask
                    ;

            global::Xamarin.Auth.CustomTabsConfiguration.IsWarmUpUsed = true;
            global::Xamarin.Auth.CustomTabsConfiguration.IsPrefetchUsed = true;
            //-----------------------------------------------------------------------------------------------

            return;
        }

        public static OAuth1Authenticator Auth1 = null;

        private void Authenticate(Xamarin.Auth.ProviderSamples.Helpers.OAuth1 oauth1)
        {
            // Step 1.1 Creating and configuring an Authenticator
            Auth1 = new OAuth1Authenticator
                (
                    consumerKey: oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    consumerSecret: oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
                    requestTokenUrl: oauth1.OAuth1_UriRequestToken,
                    authorizeUrl: oauth1.OAuth_UriAuthorization,
                    accessTokenUrl: oauth1.OAuth_UriAccessToken_UriRequestToken,
                    callbackUrl: oauth1.OAuth_UriCallbackAKARedirect,
                    // Native UI API switch
                    //      true    - NEW native UI support 
                    //      false   - OLD embedded browser API [DEFAULT]
                    // DEFAULT will be switched to true in the near future 2017-04
                    isUsingNativeUI: test_native_ui
                )
            {
                AllowCancel = oauth1.AllowCancel,
            };

            // Step 1.2 Subscribing to Authenticator events
            // If authorization succeeds or is canceled, .Completed will be fired.
            Auth1.Completed += Auth_Completed;
            Auth1.Error += Auth_Error;
            Auth1.BrowsingCompleted += Auth_BrowsingCompleted;

            // Step 2.1 Creating Login UI
            global::Android.Content.Intent ui_object = Auth1.GetUI(this);

            if (Auth2.IsUsingNativeUI == true)
            {
                // Step 2.2 Customizing the UI - Native UI [OPTIONAL]
                // In order to access CustomTabs API 
                InitializeNativeUICustomTabs();
            }

            // Step 3 Present/Launch the Login UI
            StartActivity(ui_object);

            return;
        }

        public static OAuth2Authenticator Auth2 = null;

        private void Authenticate(Xamarin.Auth.ProviderSamples.Helpers.OAuth2 oauth2)
        {
            if (string.IsNullOrEmpty(oauth2.OAuth_SecretKey_ConsumerSecret_APISecret))
            {
                if (oauth2.OAuth_UriAccessToken_UriRequestToken == null)
                {
                    // Step 1.1 Creating and configuring an Authenticator
                    Auth2 = new OAuth2Authenticator
                                    (
                                        clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                        scope: oauth2.OAuth2_Scope,
                                        authorizeUrl: oauth2.OAuth_UriAuthorization,
                                        redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
                                        // Native UI API switch
                                        //      true    - NEW native UI support 
                                        //      false   - OLD embedded browser API [DEFAULT]
                                        // DEFAULT will be switched to true in the near future 2017-04
                                        isUsingNativeUI: test_native_ui

                                    )
                    {
                        ShowErrors = false,
                        AllowCancel = oauth2.AllowCancel,
                    };
                }
                else //if (oauth2.OAuth_UriAccessToken_UriRequestToken != null)
                {
                    // Step 1.1 Creating and configuring an Authenticator                        
                    Auth2 = new OAuth2Authenticator
                                    (
                                        clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                        clientSecret: oauth2.OAuth_SecretKey_ConsumerSecret_APISecret,
                                        scope: oauth2.OAuth2_Scope,
                                        authorizeUrl: oauth2.OAuth_UriAuthorization,
                                        redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
                                        accessTokenUrl: oauth2.OAuth_UriAccessToken_UriRequestToken,
                                        // Native UI API switch
                                        //      true    - NEW native UI support 
                                        //      false   - OLD embedded browser API [DEFAULT]
                                        // DEFAULT will be switched to true in the near future 2017-04
                                        isUsingNativeUI: test_native_ui

                                    )
                    {
                        ShowErrors = false,
                        AllowCancel = oauth2.AllowCancel,
                    };
                }
            }
            else
            {
                // Step 1.1 Creating and configuring an Authenticator
                Auth2 = new OAuth2Authenticator
                                (
                                    clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    clientSecret: oauth2.OAuth_SecretKey_ConsumerSecret_APISecret,
                                    scope: oauth2.OAuth2_Scope,
                                    authorizeUrl: oauth2.OAuth_UriAuthorization,
                                    redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
                                    accessTokenUrl: oauth2.OAuth_UriAccessToken_UriRequestToken,
                                    // Native UI API switch
                                    //      true    - NEW native UI support 
                                    //      false   - OLD embedded browser API [DEFAULT]
                                    // DEFAULT will be switched to true in the near future 2017-04
                                    isUsingNativeUI: test_native_ui

                                )
                {
                    ShowErrors = false,
                    AllowCancel = oauth2.AllowCancel,
                };
            }


            // Step 1.2 Subscribing to Authenticator events
            // If authorization succeeds or is canceled, .Completed will be fired.
            Auth2.Completed += Auth_Completed;
            Auth2.Error += Auth_Error;
            Auth2.BrowsingCompleted += Auth_BrowsingCompleted;

            // Step 2.1 Creating Login UI 
            global::Android.Content.Intent ui_object = Auth2.GetUI(this);

            if (Auth2.IsUsingNativeUI == true)
            {
                InitializeNativeUICustomTabs();
            }

            // Step 3 Present/Launch the Login UI
            StartActivity(ui_object);

            return;
        }



        private void Auth_Error(object sender, AuthenticatorErrorEventArgs ee)
        {
            string title = "OAuth Error";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            sb.Append("Message  = ").Append(ee.Message)
                                    .Append(System.Environment.NewLine);
            msg = sb.ToString();

            Toast.MakeText
                        (
                            this,
                            "Message = " + msg,
                            ToastLength.Long
                        ).Show();

            return;

        }

        private void Auth_BrowsingCompleted(object sender, EventArgs ee)
        {
            string title = "OAuth Browsing Completed";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            msg = sb.ToString();

            Toast.MakeText
                        (
                            this,
                            "Message = " + msg,
                            ToastLength.Long
                        ).Show();

            return;
        }

        public void Auth_Completed(object sender, AuthenticatorCompletedEventArgs ee)
        {
            var builder = new AlertDialog.Builder(this);

            if (!ee.IsAuthenticated)
            {
                builder.SetMessage("Not Authenticated");
            }
            else
            {
                AccountStoreTests(sender, ee);
                AccountStoreTestsAsync(sender, ee);

                try
                {
                    //------------------------------------------------------------------
                    Account account = ee.Account;
                    string token = default(string);
                    if (null != account)
                    {
                        string token_name = default(string);
                        Type t = sender.GetType();
                        if (t == typeof(Xamarin.Auth.OAuth2Authenticator))
                        {
                            token_name = "access_token";
                            token = account.Properties[token_name].ToString();
                        }
                        else if (t == typeof(Xamarin.Auth.OAuth1Authenticator))
                        {
                            token_name = "oauth_token";
                            token = account.Properties[token_name].ToString();
                        }
                    }
                    //------------------------------------------------------------------

                    StringBuilder sb = new StringBuilder();
                    sb.Append("IsAuthenticated  = ").Append(ee.IsAuthenticated)
                                                    .Append(System.Environment.NewLine);
                    sb.Append("Account.UserName = ").Append(ee.Account.Username)
                                                    .Append(System.Environment.NewLine);
                    sb.Append("token            = ").Append(token)
                                                    .Append(System.Environment.NewLine);

                    builder.SetTitle("AuthenticationResults");
                    builder.SetMessage(sb.ToString());
                }
                catch (global::Android.OS.OperationCanceledException)
                {
                    builder.SetTitle("Task Canceled");
                }
                catch (Exception ex)
                {
                    builder.SetTitle("Error");
                    builder.SetMessage(ex.ToString());
                }
            }

            //ee.Account

            builder.SetPositiveButton("Ok", (o, e) => { });
            builder.Create().Show();

            return;
        }

        private void AccountStoreTests(object authenticator, AuthenticatorCompletedEventArgs ee)
        {
            // Step 4.2 Store the account
            AccountStore account_store = AccountStore.Create(this);
            account_store.Save(ee.Account, provider);

            //------------------------------------------------------------------
            // Android
            // https://kb.xamarin.com/agent/case/225411
            // cannot reproduce 
            // Step 4.3 Retrieve stored accounts
            Account account1 = account_store.FindAccountsForService(provider).FirstOrDefault();
            if (null != account1)
            {
                //------------------------------------------------------------------
                string token = default(string);
                if (null != account1)
                {
                    string token_name = default(string);
                    Type t = authenticator.GetType();
                    if (t == typeof(Xamarin.Auth.OAuth2Authenticator))
                    {
                        token_name = "access_token";
                        token = account1.Properties[token_name].ToString();
                    }
                    else if (t == typeof(Xamarin.Auth.OAuth1Authenticator))
                    {
                        token_name = "oauth_token";
                        token = account1.Properties[token_name].ToString();
                    }
                }
                //------------------------------------------------------------------
                Toast.MakeText
                (
                    this,
                    "access_token = " + token,
                    ToastLength.Long
                ).Show();
            }
            //------------------------------------------------------------------

            AccountStore.Create(this).Save(ee.Account, provider + ".v.2");

            //------------------------------------------------------------------
            // throws on iOS
            //
            Account account2 = AccountStore.Create(this).FindAccountsForService(provider + ".v.2").FirstOrDefault();
            if (null != account2)
            {
                //------------------------------------------------------------------
                string token = default(string);
                if (null != account2)
                {
                    string token_name = default(string);
                    Type t = authenticator.GetType();
                    if (t == typeof(Xamarin.Auth.OAuth2Authenticator))
                    {
                        token_name = "access_token";
                        token = account2.Properties[token_name].ToString();
                    }
                    else if (t == typeof(Xamarin.Auth.OAuth1Authenticator))
                    {
                        token_name = "oauth_token";
                        token = account2.Properties[token_name].ToString();
                    }
                }
                //------------------------------------------------------------------
                Toast.MakeText
                (
                    this,
                    "access_token = " + token,
                    ToastLength.Long
                ).Show();
            }
            //------------------------------------------------------------------

            return;
        }

        private async void AccountStoreTestsAsync(object authenticator, AuthenticatorCompletedEventArgs ee)
        {
            AccountStore account_store = AccountStore.Create(this);
            await account_store.SaveAsync(ee.Account, provider);

            //------------------------------------------------------------------
            // Android
            // https://kb.xamarin.com/agent/case/225411
            // cannot reproduce 
            Account account1 = (await account_store.FindAccountsForServiceAsync(provider)).FirstOrDefault();
            if (null != account1)
            {
                //------------------------------------------------------------------
                string token = default(string);
                if (null != account1)
                {
                    string token_name = default(string);
                    Type t = authenticator.GetType();
                    if (t == typeof(Xamarin.Auth.OAuth2Authenticator))
                    {
                        token_name = "access_token";
                        token = account1.Properties[token_name].ToString();
                    }
                    else if (t == typeof(Xamarin.Auth.OAuth1Authenticator))
                    {
                        token_name = "oauth_token";
                        token = account1.Properties[token_name].ToString();
                    }
                }
                //------------------------------------------------------------------
                Toast.MakeText
                (
                    this,
                    "access_token = " + token,
                    ToastLength.Long
                ).Show();
            }
            //------------------------------------------------------------------

            AccountStore.Create(this).Save(ee.Account, provider + ".v.2");

            //------------------------------------------------------------------
            // throws on iOS
            //
            IEnumerable<Account> accounts = await (AccountStore.Create(this).FindAccountsForServiceAsync(provider + ".v.2"));
            Account account2 = accounts.FirstOrDefault();
            if (null != account2)
            {
                //------------------------------------------------------------------
                string token = default(string);
                if (null != account2)
                {
                    string token_name = default(string);
                    Type t = authenticator.GetType();
                    if (t == typeof(Xamarin.Auth.OAuth2Authenticator))
                    {
                        token_name = "access_token";
                        token = account2.Properties[token_name].ToString();
                    }
                    else if (t == typeof(Xamarin.Auth.OAuth1Authenticator))
                    {
                        token_name = "oauth_token";
                        token = account2.Properties[token_name].ToString();
                    }
                }
                //------------------------------------------------------------------
                Toast.MakeText
                (
                    this,
                    "access_token = " + token,
                    ToastLength.Long
                ).Show();
            }
            //------------------------------------------------------------------

            return;
        }

    }
}


