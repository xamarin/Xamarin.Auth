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

using Xamarin.Auth.SampleData;

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

            // [Chrome] Custom Tabs WarmUp and prefetch
            custom_tab_activity_helper = new global::Android.Support.CustomTabs.Chromium.SharedUtilities.CustomTabActivityHelper();


            return;
        }

        string[] provider_list = Data.TestCases.Keys.ToArray();

        string provider = null;

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            provider = provider_list[position];

            Xamarin.Auth.Helpers.OAuth auth;
            if (!Data.TestCases.TryGetValue(provider, out auth))
            {
                Toast.MakeText(this, "Unknown OAuth Provider!", ToastLength.Long);
            }
            if (auth is Xamarin.Auth.Helpers.OAuth1)
            {
                Authenticate(auth as Xamarin.Auth.Helpers.OAuth1);
            }
            else
            {
                Authenticate(auth as Xamarin.Auth.Helpers.OAuth2);
            }

            return;
        }

        global::Android.Support.CustomTabs.Chromium.SharedUtilities.CustomTabActivityHelper custom_tab_activity_helper = null;

        protected override void OnStart()
        {
            base.OnStart();

            // [Chrome] Custom Tabs WarmUp and prefetch
            custom_tab_activity_helper.BindCustomTabsService(this);

            return;
        }

        protected override void OnStop()
        {
            base.OnStop();

            // [Chrome] Custom Tabs WarmUp and prefetch
            custom_tab_activity_helper.UnbindCustomTabsService(this);

            return;
        }

        public static OAuth1Authenticator Auth1 = null;

        private void Authenticate(Xamarin.Auth.Helpers.OAuth1 oauth1)
        {
            //-------------------------------------------------------------
            // WalkThrough Step 1
            //      setting up Authenticator object
            Auth1 = new OAuth1Authenticator
                (
                    consumerKey: oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    consumerSecret: oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
                    requestTokenUrl: oauth1.OAuth1_UriRequestToken,
                    authorizeUrl: oauth1.OAuth_UriAuthorization,
                    accessTokenUrl: oauth1.OAuth_UriAccessToken,
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
            //-------------------------------------------------------------

            // If authorization succeeds or is canceled, .Completed will be fired.
            Auth1.Completed += Auth_Completed;
            Auth1.Error += Auth_Error;
            Auth1.BrowsingCompleted += Auth_BrowsingCompleted;

            //#####################################################################
            // WalkThrough Step 2
            //      creating Presenter (UI) for specific platform
            // Xamarin.Auth API - Breaking Change
            //      old API returned global::Android.Content.Intent
            //Intent ui_intent_as_object = auth.GetUI ();
            //      new API returns System.Object
            global::Android.Content.Intent ui_object = Auth1.GetUI(this);

            if (Auth1.IsUsingNativeUI == true)
            {
                //=================================================================
                // WalkThrough Step 2.1
                //      casting UI object to proper type to work with
                //
                // Xamarin.Auth API - Native UI support 
                //      *   Android - [Chrome] Custom Tabs on Android       
                //          Android.Support.CustomTabs      
                //          and 
                //      *   iOS -  SFSafariViewController     
                //          SafariServices.SFSafariViewController
                // on 2014-04-20 google (and some other providers) will work only with this API
                //  
                //
                //  2017-03-25
                //      NEW UPCOMMING API undocumented work in progress
                //      soon to be default
                //      optional API in the future (for backward compatibility)
                //
                //  required part
                //  add 
                //     following code:

                //  add custom schema (App Linking) handling
                //      1.  add Activity with IntentFilter to the app
                //          1.1. Define sheme[s] and host[s] in the IntentFilter
                //          1.2. in Activity's OnCreate extract URL with custom schema from Intent
                //          1.3. parse OAuth data from URL obtained in 1.2.
                //  NOTE[s]
                //  *   custom scheme support only
                //      xamarinauth://localhost
                //      xamarin-auth://localhost
                //      xamarin.auth://localhost
                //  *   no http[s] scheme support
                //------------------------------------------------------------

            }

			//------------------------------------------------------------
			// WalkThrough Step 3
			//      Launching UI
			//      [REQUIRED] 
			StartActivity(ui_object);
            //------------------------------------------------------------

            return;
        }

        public static OAuth2Authenticator Auth2 = null;

        private void Authenticate(Xamarin.Auth.Helpers.OAuth2 oauth2)
        {
            if (oauth2.OAuth2_UriRequestToken == null || string.IsNullOrEmpty(oauth2.OAuth_SecretKey_ConsumerSecret_APISecret))
            {
                //-------------------------------------------------------------
                // WalkThrough Step 1
                //      setting up Authenticator object
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
                    AllowCancel = oauth2.AllowCancel,
                };
                //-------------------------------------------------------------
            }
            else
            {
                //-------------------------------------------------------------
                // WalkThrough Step 1
                //      setting up Authenticator object
                Auth2 = new OAuth2Authenticator
                    (
                        clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                        clientSecret: "93e7f486b09bd1af4c38913cfaacbf8a384a50d2",
                        scope: oauth2.OAuth2_Scope,
                        authorizeUrl: oauth2.OAuth_UriAuthorization,
                        redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
                        accessTokenUrl: oauth2.OAuth2_UriRequestToken,
                        // Native UI API switch
                        //      true    - NEW native UI support 
                        //      false   - OLD embedded browser API [DEFAULT]
                        // DEFAULT will be switched to true in the near future 2017-04
                        isUsingNativeUI: test_native_ui
                    )
                {
                    AllowCancel = oauth2.AllowCancel,
                };
                //-------------------------------------------------------------
            }


            // If authorization succeeds or is canceled, .Completed will be fired.
            Auth2.Completed += Auth_Completed;
            Auth2.Error += Auth_Error;
            Auth2.BrowsingCompleted += Auth_BrowsingCompleted;

            //#####################################################################
            // WalkThrough Step 2
            //      creating Presenter (UI) for specific platform
            // Xamarin.Auth API - Breaking Change
            //      old API returned global::Android.Content.Intent
            //Intent ui_intent_as_object = auth.GetUI ();
            //      new API returns System.Object
            Intent ui_object = Auth2.GetUI(this);

            if (Auth2.IsUsingNativeUI == true)
            {
                //=================================================================
                // WalkThrough Step 2.1
                //      casting UI object to proper type to work with
                //
                // Xamarin.Auth API - Native UI support 
                //      *   Android - [Chrome] Custom Tabs on Android       
                //          Android.Support.CustomTabs      
                //          and 
                //      *   iOS -  SFSafariViewController     
                //          SafariServices.SFSafariViewController
                // on 2014-04-20 google (and some other providers) will work only with this API
                //  
                //
                //  2017-03-25
                //      NEW UPCOMMING API undocumented work in progress
                //      soon to be default
                //      optional API in the future (for backward compatibility)
                //
                //  required part
                //  add 
                //     following code:

                //------------------------------------------------------------
                //  add custom schema (App Linking) handling
                //      1.  add Activity with IntentFilter to the app
                //          1.1. Define sheme[s] and host[s] in the IntentFilter
                //          1.2. in Activity's OnCreate extract URL with custom schema from Intent
                //          1.3. parse OAuth data from URL obtained in 1.2.
                //  NOTE[s]
                //  *   custom scheme support only
                //      xamarinauth://localhost
                //      xamarin-auth://localhost
                //      xamarin.auth://localhost
                //  *   no http[s] scheme support
                //------------------------------------------------------------

            }

            //------------------------------------------------------------
            // WalkThrough Step 3
            //      Launching UI
            //      [REQUIRED] 
            StartActivity(ui_object);
            //------------------------------------------------------------

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
            AccountStore account_store = AccountStore.Create(this);
            account_store.Save(ee.Account, provider);

            //------------------------------------------------------------------
            // Android
            // https://kb.xamarin.com/agent/case/225411
            // cannot reproduce 
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


