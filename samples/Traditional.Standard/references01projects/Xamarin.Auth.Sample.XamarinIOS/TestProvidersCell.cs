﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

#if ! __CLASSIC__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

using global::Xamarin.Auth.SampleData;

namespace Xamarin.Auth.Sample.XamarinIOS
{
    public class TestProvidersCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString ("TestProvidersCell");

        public TestProvidersCell () : base (UITableViewCellStyle.Value1, Key)
        {
            // TODO: add subviews to the ContentView, set various colors, etc.
            TextLabel.Text = "TextLabel";
        }

        public override void TouchesBegan (NSSet touches, UIEvent evt)
        {
            provider = this.TextLabel.Text;

            switch (provider)
            {
                case "Facebook OAuth2":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Twitter OAuth1":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1);
                    break;
                case "Google OAuth2":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Microsoft Live OAuth2":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "LinkedIn OAuth1":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1);
                    break;
                case "LinkedIn OAuth2":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Github OAuth2":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Instagram OAuth2":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
                    break;
				case "Amazon OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Meetup OAuth1":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1);
					break;
				case "Meetup OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Dropbox OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Paypal OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
				case "Stackoverflow OAuth2":
					Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
					break;
                default:
                    UIAlertView _error = new UIAlertView ("Error", "Unknown OAuth Provider!", null, "Ok", null);
                    _error.Show ();
                    break;
            };

            return;
        }

		string provider = null;

        private void Authenticate(Xamarin.Auth.Helpers.OAuth1 oauth1)
        {
            OAuth1Authenticator auth = new OAuth1Authenticator 
                (
                    consumerKey: oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    consumerSecret: oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
                    requestTokenUrl: oauth1.OAuth1_UriRequestToken,
                    authorizeUrl: oauth1.OAuth_UriAuthorization,
                    accessTokenUrl: oauth1.OAuth_UriAccessToken,
                    callbackUrl: oauth1.OAuth_UriCallbackAKARedirect
                );

            auth.AllowCancel = oauth1.AllowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;
            auth.Error += Auth_Error;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;

            UIViewController vc = auth.GetUI ();

            UIView sv1 = this.Superview;
            UITableView sv2 = (UITableView) sv1.Superview;
            UIWindow sv3 = (UIWindow) sv2.Superview;
            sv3.RootViewController.PresentViewController(vc, true, null);

            return;
        }

        private void Authenticate(Xamarin.Auth.Helpers.OAuth2 oauth2)
        {
            OAuth2Authenticator auth = null;

            if (!oauth2.Description.Equals ("Github OAuth2"))
            {
                // all but github
                auth = new OAuth2Authenticator 
                    (
                        clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                        scope: oauth2.OAuth2_Scope,
                        authorizeUrl: oauth2.OAuth_UriAuthorization,
                        redirectUrl: oauth2.OAuth_UriCallbackAKARedirect
                    );
            }
            else
            {
                auth = new OAuth2Authenticator 
                    (
                        clientId: "5b5c2d2d76e2fd9a804b",
                        clientSecret: "93e7f486b09bd1af4c38913cfaacbf8a384a50d2",
                        scope: "",
                        authorizeUrl: new Uri("https://github.com/login/oauth/authorize"),
                        redirectUrl: new Uri("http://xamarin.com"),
                        accessTokenUrl: new Uri("https://github.com/login/oauth/access_token")
                    );
            }

            auth.AllowCancel = oauth2.AllowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;
            auth.Error += Auth_Error;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;

            UIViewController vc = auth.GetUI ();

            UIView sv1 = this.Superview;
            UITableView sv2 = (UITableView) sv1.Superview;
            UIWindow sv3 = (UIWindow) sv2.Superview;
            sv3.RootViewController.PresentViewController(vc, true, null);

            return;
        }
        
        public void Auth_Completed (object sender, AuthenticatorCompletedEventArgs ee)
        {
            string title = "OAuth Results";
            string msg = "";

            if (!ee.IsAuthenticated)
            {
                msg = "Not Authenticated";
            }
            else 
            {
                AccountStoreTests (sender, ee);

                try 
                {
                    //------------------------------------------------------------------
                    Account account = ee.Account;
                    string token = default(string);
                    if (null != account)
                    {
                        string token_name = default(string);
                        Type t = sender.GetType();
                        if ( t == typeof(Xamarin.Auth.OAuth2Authenticator) )
                        {
                            token_name = "access_token";
                            token = account.Properties[token_name].ToString();
                        }
                        else if ( t == typeof(Xamarin.Auth.OAuth1Authenticator) )
                        {
                            token_name = "oauth_token";
                            token = account.Properties[token_name].ToString();
                        }
                    }
                    //------------------------------------------------------------------

                    AuthenticationResult ar = new AuthenticationResult()
                    {
                        Title = "n/a",
                        User = "n/a",
                    };

                    StringBuilder sb = new StringBuilder();
                    sb.Append("IsAuthenticated  = ").Append(ee.IsAuthenticated)
                                                    .Append(System.Environment.NewLine);
                    sb.Append("Name             = ").Append(ar.User)
                                                    .Append(System.Environment.NewLine);
                    sb.Append("Account.UserName = ").Append(ee.Account.Username)
                                                    .Append(System.Environment.NewLine);
                    sb.Append("token            = ").Append(token)
                                                    .Append(System.Environment.NewLine);
                    msg = sb.ToString();
                } 
                catch (Exception ex) 
                {
                    msg = ex.Message;
                }
            }

            InvokeOnMainThread 
                ( 
                    () => 
                    {
                        // manipulate UI controls
                        UIAlertView _error = new UIAlertView (title, msg, null, "Ok", null);
                        _error.Show ();
                    }
                );

            return;
        }

        private void Auth_Error (object sender, AuthenticatorErrorEventArgs ee)
        {
            string title = "OAuth Error";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            sb.Append("Message  = ").Append(ee.Message)
                                    .Append(System.Environment.NewLine);
            msg = sb.ToString();

            InvokeOnMainThread 
                ( 
                    () => 
                    {
                        // manipulate UI controls
                        UIAlertView _error = new UIAlertView (title, msg, null, "Ok", null);
                        _error.Show ();
                    }
                );

            return;

        }

        private void Auth_BrowsingCompleted (object sender, EventArgs ee)
        {
            string title = "OAuth Browsing Completed";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            msg = sb.ToString();

            InvokeOnMainThread 
                ( 
                    () => 
                    {
                        // manipulate UI controls
                        UIAlertView _error = new UIAlertView (title, msg, null, "Ok", null);
                        _error.Show ();
                    }
                );

            return;
        }

        private void AccountStoreTests(object authenticator, AuthenticatorCompletedEventArgs ee)
        {
            AccountStore account_store = AccountStore.Create();
            account_store.Save(ee.Account, provider);  

            //------------------------------------------------------------------
            // Android
            // https://kb.xamarin.com/agent/case/225411
            // cannot reproduce 
            try
            {
                //------------------------------------------------------------------
                // Xamarin.iOS - following line throws
                IEnumerable<Account> accounts = account_store.FindAccountsForService(provider);
                Account account1 = accounts.FirstOrDefault();
                //------------------------------------------------------------------
                if (null != account1)
                {
                    string token = default(string);
                    string token_name = default(string);
                    Type t = authenticator.GetType();
                    if ( t == typeof(Xamarin.Auth.OAuth2Authenticator) )
                    {
                        token_name = "access_token";
                        token = account1.Properties[token_name].ToString();
                    }
                    else if ( t == typeof(Xamarin.Auth.OAuth1Authenticator) )
                    {
                        token_name = "oauth_token";
                        token = account1.Properties[token_name].ToString();
                    }
                    UIAlertView alert = 
                                    new UIAlertView
                                        (
                                            "Token",
                                            "access_token = " + token,
                                            null,
                                            "OK",
                                            null
                                        );
                    alert.Show();
                }
            }
            catch (System.Exception exc)
            {
                // Xamarin.iOS
                // exc  {System.ArgumentNullException: Value cannot be null. 
                //  Parameter name: data   
                //      at Foundation.NSString.Fr…} System.ArgumentNullException
                // Value cannot be null.
                // Parameter name: data
                string msg = exc.Message;
                System.Diagnostics.Debug.WriteLine("Exception AccountStore: " + msg);
            }

            try
            {
                AccountStore.Create().Save(ee.Account, provider + ".v.2");
            }
            catch (System.Exception exc)
            {
                string msg = exc.Message;
                System.Diagnostics.Debug.WriteLine("Exception AccountStore: " + msg);
            }

            try
            {
                //------------------------------------------------------------------
                // Xamarin.iOS - throws
                IEnumerable<Account> accounts = account_store.FindAccountsForService(provider+ ".v.2");
                Account account2 = accounts.FirstOrDefault();
                //------------------------------------------------------------------
                if( null != account2 )
                {
                    string token = default(string);
                    string token_name = default(string);
                    Type t = authenticator.GetType();
                    if ( t == typeof(Xamarin.Auth.OAuth2Authenticator) )
                    {
                        token_name = "access_token";
                        token = account2.Properties[token_name].ToString();
                    }
                    else if ( t == typeof(Xamarin.Auth.OAuth1Authenticator) )
                    {
                        token_name = "oauth_token";
                        token = account2.Properties[token_name].ToString();
                    }
                    UIAlertView alert = new UIAlertView
                                                (
                                                    "Token",
                                                    "access_token = " + token,
                                                    null,
                                                    "OK",
                                                    null
                                                );
                    alert.Show();
                }
            }
            catch (System.Exception exc)
            {
                string msg = exc.Message;
                System.Diagnostics.Debug.WriteLine("Exception AccountStore: " + msg);
            }

            return;
        }
    }
}

