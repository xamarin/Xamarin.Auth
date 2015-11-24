using System;
using Xamarin.Auth;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Xamarin.Auth.XamarinForms;
using Xamarin.Auth.Helpers;
using Xamarin.Auth.XamarinForms.iOS;
using System.Collections.Generic;

[assembly: Xamarin.Forms.ExportRenderer (typeof(LoginPage), typeof(LoginPageRenderer) )]

namespace Xamarin.Auth.XamarinForms.iOS
{
	public partial class LoginPageRenderer : global::Xamarin.Forms.Platform.iOS.PageRenderer
	{
		bool hasBeenShown;//TODO is this nessesary?

		public Helpers.OAuth OAuth { get; set; }

		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);

			this.OAuth = (e.NewElement as LoginPage).OAuth;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			if (hasBeenShown == true || this.OAuth == null)
				return;

			hasBeenShown = true;
		
			if (this.OAuth is Helpers.OAuth1) 
			{
				Login (this.OAuth as Helpers.OAuth1);
			} 
			else if (this.OAuth is Helpers.OAuth2) 
			{
				Login (this.OAuth as Helpers.OAuth2);
			} 
			else 
			{
				throw new ArgumentOutOfRangeException ("Unknown OAuth");
			}
		}

		void Login (Helpers.OAuth1 oauth1)
		{
			var auth = new global::Xamarin.Auth.OAuth1Authenticator 
				(
					consumerKey: oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					consumerSecret: oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
					requestTokenUrl: oauth1.OAuth1_UriRequestToken,
					authorizeUrl: oauth1.OAuth_UriAuthorization, 
					accessTokenUrl: oauth1.OAuth1_UriAccessToken, 
					callbackUrl: oauth1.OAuth_UriCallbackAKARedirect, 
					getUsernameAsync: null
				);

			auth.Completed += OnAuthCompleted;

			PresentViewController (auth.GetUI (), true, null);
		}

		void Login (Helpers.OAuth2 oauth2)
		{
			global::Xamarin.Auth.OAuth2Authenticator auth;

			//implicit grant
			if (oauth2.OAuth1_UriAccessToken == null)
			{
	
				auth = new global::Xamarin.Auth.OAuth2Authenticator (
					clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					scope: oauth2.OAuth2_Scope,
					authorizeUrl: oauth2.OAuth_UriAuthorization,
					redirectUrl: oauth2.OAuth_UriCallbackAKARedirect);
			}
			//authorization code grant
			else
			{
				auth = new global::Xamarin.Auth.OAuth2Authenticator (
					clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer, 
					clientSecret: oauth2.OAuth1_SecretKey_ConsumerSecret_APISecret,
					scope: oauth2.OAuth2_Scope,
					authorizeUrl: oauth2.OAuth_UriAuthorization,
					redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
					accessTokenUrl: oauth2.OAuth1_UriAccessToken);
			}

			auth.Completed += OnAuthCompleted;

			PresentViewController (auth.GetUI (), true, null);
		}

		void OnAuthCompleted(object sender, global::Xamarin.Auth.AuthenticatorCompletedEventArgs e)
		{
			DismissViewController(true, null);        

			//return the account object
			MessagingCenter.Send<LoginPage, AuthenticatorCompletedEventArgs> (this.Element as LoginPage, "LoginCompleted", e);
		}
	}
}