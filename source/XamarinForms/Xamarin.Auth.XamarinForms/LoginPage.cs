using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Auth;
using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.XamarinForms
{
	public class LoginPage : ContentPage
	{
		public EventHandler<AuthenticatorCompletedEventArgs> Completed { get; set; }

		public OAuth OAuth { get; set; }

		public LoginPage (string clientId, string scope, Uri authorizationUri, Uri redirectUri )
		{
			this.OAuth = new Helpers.OAuth2()
			{
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer 	= clientId,
				OAuth2_Scope 											= scope,
				OAuth_UriAuthorization 									= authorizationUri,
				OAuth_UriCallbackAKARedirect 							= redirectUri,
			};

			MessagingCenter.Subscribe<LoginPage, AuthenticatorCompletedEventArgs> (this, "LoginCompleted", (LoginPage sender, AuthenticatorCompletedEventArgs args) => {
				if (Completed != null)
					Completed(this, args);
			});
		}

		public LoginPage (Helpers.OAuth2 oauth2)
			: this (
			oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
			oauth2.OAuth2_Scope,
			oauth2.OAuth_UriAuthorization,
			oauth2.OAuth_UriCallbackAKARedirect )
		{
		}

		public LoginPage (string clientId, string secret, Uri requestTokenUri, 
						  Uri authorizationUri, Uri accessTokenUri, Uri redirectUri )
		{
			this.OAuth = new Helpers.OAuth1()
			{
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer 	= clientId,
				OAuth1_SecretKey_ConsumerSecret_APISecret 				= secret,
				OAuth1_UriRequestToken 									= requestTokenUri,
				OAuth_UriAuthorization 									= authorizationUri,
				OAuth1_UriAccessToken 									= accessTokenUri,
				OAuth_UriCallbackAKARedirect 							= redirectUri,
			};
		}

		public LoginPage (Helpers.OAuth1 oauth1)
			: this (
			oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
			oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
			oauth1.OAuth1_UriRequestToken,
			oauth1.OAuth_UriAuthorization,
			oauth1.OAuth1_UriAccessToken,
			oauth1.OAuth_UriCallbackAKARedirect)
		{
		}
	}
}