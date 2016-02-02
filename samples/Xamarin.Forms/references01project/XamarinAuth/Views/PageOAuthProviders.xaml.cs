using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Xamarin.Forms;

using Xamarin.Auth;
using Xamarin.Auth.XamarinForms;

using Xamarin.Auth.SampleData;

namespace XamarinAuth.Views
{
    public partial class PageOAuthProviders : ContentPage
	{
        public PageOAuthProviders ()
		{
			InitializeComponent ();
		}



		Page page = null;
				
		string provider = null;

		public void OnItemSelected (object sender, ItemTappedEventArgs args_tapped)
		{
			object item = args_tapped.Item;

			KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

			string provider = kvp.Value;

			switch (provider)
			{
				case "Facebook OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_facebook = null;
                    oauth_facebook = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
                    page = new PageOAuth
                                (
                                    oauth_facebook.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_facebook.OAuth2_Scope,
                                    oauth_facebook.OAuth_UriAuthorization,
                                    oauth_facebook.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
					break;
				case "Twitter OAuth1":
                    Xamarin.Auth.Helpers.OAuth1 oauth_twitter = null;
                    oauth_twitter = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1;  
                    page = new PageOAuth
                                (
                                    oauth_twitter.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_twitter.OAuth1_SecretKey_ConsumerSecret_APISecret,
                                    oauth_twitter.OAuth1_UriRequestToken,
                                    oauth_twitter.OAuth_UriAuthorization,
                                    oauth_twitter.OAuth_UriAccessToken,
                                    oauth_twitter.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
                	break;
				case "Google OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_google = null;
                    oauth_google = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
                    page = new PageOAuth
                                (
                                    oauth_google.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_google.OAuth2_Scope,
                                    oauth_google.OAuth_UriAuthorization,
                                    oauth_google.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
                    break;
				case "Microsoft Live OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_mslive = null;
                    oauth_mslive = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
                    page = new PageOAuth
                                (
                                    oauth_mslive.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_mslive.OAuth2_Scope,
                                    oauth_mslive.OAuth_UriAuthorization,
                                    oauth_mslive.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
                    break;
				case "LinkedIn OAuth1":
                    Xamarin.Auth.Helpers.OAuth1 oauth_linkedin1 = null;
                    oauth_linkedin1 = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth1;  
                    page = new PageOAuth
                                (
                                    oauth_linkedin1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_linkedin1.OAuth1_SecretKey_ConsumerSecret_APISecret,
                                    oauth_linkedin1.OAuth1_UriRequestToken,
                                    oauth_linkedin1.OAuth_UriAuthorization,
                                    oauth_linkedin1.OAuth_UriAccessToken,
                                    oauth_linkedin1.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
                    break;
				case "LinkedIn OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_linkedin2 = null;
                    oauth_linkedin2 = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
                    page = new PageOAuth
                                (
                                    oauth_linkedin2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_linkedin2.OAuth2_Scope,
                                    oauth_linkedin2.OAuth_UriAuthorization,
                                    oauth_linkedin2.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
                    break;
				case "Github OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_github = null;
                    oauth_github = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
                    page = new PageOAuth
                                (
                                    oauth_github.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_github.OAuth2_Scope,
                                    oauth_github.OAuth_UriAuthorization,
                                    oauth_github.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
                    break;
				case "Instagram OAuth2":
                    Xamarin.Auth.Helpers.OAuth2 oauth_instagram = null;
                    oauth_instagram = Data.TestCases[provider] as Xamarin.Auth.Helpers.OAuth2;  
                    page = new PageOAuth
                                (
                                    oauth_instagram.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                                    oauth_instagram.OAuth2_Scope,
                                    oauth_instagram.OAuth_UriAuthorization,
                                    oauth_instagram.OAuth_UriCallbackAKARedirect,
                                    null,
                                    true
                                );
                    break;
				default:
					DisplayAlert ("Error", "Unknown OAuth Provider!", "OK");
					break;
			};

			this.Navigation.PushAsync(page);

			return;

		}

		public async void Auth_Completed (object sender, AuthenticatorCompletedEventArgs ee)
		{
			string title = "OAuth Results";
			string msg = "";

			if (!ee.IsAuthenticated)
			{
				msg = "Not Authenticated";
			}
			else 
			{
				AccountStoreTests (ee);

				try 
				{
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

				} 
				catch (Exception ex) 
				{
					msg = ex.Message;
				}
			}

			DisplayAlert (title, msg, "OK");

			return;
		}

		private void AccountStoreTests (AuthenticatorCompletedEventArgs ee)
		{
			AccountStore account_store = AccountStore.Create();
			account_store.Save (ee.Account, provider);	
			Account account1 = account_store.FindAccountsForService(provider).FirstOrDefault();

			AccountStore.Create().Save(ee.Account, provider + ".v.2");
			// throws on iOS
			//
			Account account2 = AccountStore.Create().FindAccountsForService(provider+ ".v.2").FirstOrDefault();

			return;
		}
	}
}

