using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


using Xamarin.Auth.SampleData;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Xamarin.Auth.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            itemList.ItemsSource = null;
            itemList.ItemsSource = provider_list;

            return;
        }

        string[] provider_list = new string[] 
        { 
            "Facebook OAuth2",
            "Twitter OAuth1",
            "Google OAuth2",
            "Microsoft Live OAuth2",
            "LinkedIn OAuth1",
            "LinkedIn OAuth2",
            "Github OAuth2",
            "Amazon OAuth2", 
            "Dropbox OAuth2", 
            "Meetup OAuth1", 
            "Meetup OAuth2", 
            "Paypal OAuth2", 
            "Stackoverflow OAuth2", 
        };
        string provider = null;

        private void itemList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //TextView tv = v as TextView;
            string si = ((ListBox)sender).SelectedItem.ToString();
            string provider = si;

            switch (provider)
            {
                case "Facebook OAuth2":
                    Authenticate(Data.TestCases["Facebook OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Twitter OAuth1":
                    Authenticate(Data.TestCases["Twitter OAuth1"] as Xamarin.Auth.Helpers.OAuth1);
                    break;
                case "Google OAuth2":
                    Authenticate(Data.TestCases["Google OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Microsoft Live OAuth2":
                    Authenticate(Data.TestCases["Microsoft Live OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "LinkedIn OAuth1":
                    Authenticate(Data.TestCases["LinkedIn OAuth1"] as Xamarin.Auth.Helpers.OAuth1);
                    break;
                case "LinkedIn OAuth2":
                    Authenticate(Data.TestCases["LinkedIn OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Github OAuth2":
                    Authenticate(Data.TestCases["Github OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Instagram OAuth2":
                    Authenticate(Data.TestCases["Instagram OAuth2"] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Amazon OAuth2":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
                    break;
                case "Meetup OAuth1":
                    Authenticate(Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2);
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
                    //Toast.MakeText(this, "Unknown OAuth Provider!", ToastLength.Long);
                    break;
            };
            var list = Data.TestCases;

            return;
        }

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
            EventHandler<AuthenticatorErrorEventArgs> Auth_Error = null;
            auth.Error += Auth_Error;
            EventHandler Auth_BrowsingCompleted = null;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;

            //Uri uri = auth.GetUI();
            Type page_type = auth.GetUI();

            //(System.Windows.Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);
            this.Frame.Navigate(page_type, auth);

            return;
        }

        private void Authenticate(Xamarin.Auth.Helpers.OAuth2 oauth2)
        {
            OAuth2Authenticator auth = new OAuth2Authenticator
                (
                    clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    scope: oauth2.OAuth2_Scope,
                    authorizeUrl: oauth2.OAuth_UriAuthorization,
                    redirectUrl: oauth2.OAuth_UriCallbackAKARedirect
                );

            auth.AllowCancel = oauth2.AllowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;
            auth.Error += Auth_Error;
            auth.BrowsingCompleted += Auth_BrowsingCompleted;

            //Uri uri = auth.GetUI();
            Type page_type = auth.GetUI();

            //(System.Windows.Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);
            this.Frame.Navigate(page_type, auth);

            return;
        }

        private async void Auth_Error(object sender, AuthenticatorErrorEventArgs ee)
        {
            string title = "OAuth Error";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            sb.Append("Message  = ").Append(ee.Message)
                                    .Append(System.Environment.NewLine);
            msg = sb.ToString();


            MessageDialog md = new MessageDialog(msg, title);
            await md.ShowAsync();

            return;

        }

        private async void Auth_BrowsingCompleted(object sender, EventArgs ee)
        {
            string title = "OAuth Browsing Completed";
            string msg = "";

            StringBuilder sb = new StringBuilder();
            msg = sb.ToString();

            return;
        }

        public async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs ee)
        {
            string title = "OAuth Completed";
            string msg = "";

            if (!ee.IsAuthenticated)
            {
                msg = "Not authincated";
            }
            else
            {
                AccountStoreTests(sender, ee);

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

            MessageDialog md = new MessageDialog(msg, title);
            await md.ShowAsync();

            return;
        }


        private async void AccountStoreTests(object authenticator, AuthenticatorCompletedEventArgs ee)
        {
            string title = "AccountStore tests";
            string msg = "";

            AccountStore account_store = AccountStore.Create();
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
                MessageDialog md = new MessageDialog(msg, title);
                await md.ShowAsync();

            }
            //------------------------------------------------------------------

            AccountStore.Create().Save(ee.Account, provider + ".v.2");

            //------------------------------------------------------------------
            // throws on iOS
            //
            Account account2 = AccountStore.Create().FindAccountsForService(provider + ".v.2").FirstOrDefault();
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
                MessageDialog md = new MessageDialog
                                        (
                                          "access_token = " + token,
                                          "Access Token"
                                        );
                await md.ShowAsync();
            }
            //------------------------------------------------------------------

            return;
        }
    }
}
