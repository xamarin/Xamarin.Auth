using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;


[assembly: 
	Xamarin.Forms.ExportRenderer
			(
			// ViewElement to be rendered (from Portable/Shared)
			typeof(Xamarin.Auth.XamarinForms.PageLogin),
			// platform specific Renderer : global::Xamarin.Forms.Platform.iOS.PageRenderer
			typeof(Xamarin.Auth.XamarinForms.WindowsPhone.PageLoginRenderer)
			)
]
namespace Xamarin.Auth.XamarinForms.WindowsPhone
{
	public partial class PageLoginRenderer : global::Xamarin.Forms.Platform.WinPhone.PageRenderer
	{
		bool IsShown;

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);

			// OnElementChanged is fired before ViewDidAppear, using it to pass data

			PageLogin e_new = e.NewElement as PageLogin;

			this.OAuth = e_new.OAuth;

			Login();

			return;
		}

		public void Login()
		{
			base.UpdateLayout();

			if (!IsShown)
			{

				IsShown = true;

				// TODO: polymorfic
				HolisticWare.Auth.OAuth1 oauth1 = this.OAuth as HolisticWare.Auth.OAuth1;
				HolisticWare.Auth.OAuth2 oauth2 = this.OAuth as HolisticWare.Auth.OAuth2;

				if (null != oauth1)
				{
					Login(oauth1);
					return;
				}

				if (null != oauth2)
				{
					Login(oauth2);
					return;
				}

				throw new ArgumentOutOfRangeException("Unknown OAuth");
				/*

				*/
			}
			return;
		}

		private void Login (HolisticWare.Auth.OAuth1 oauth1)
		{
			global::Xamarin.Auth.OAuth1Authenticator auth = 
					new global::Xamarin.Auth.OAuth1Authenticator 
						(
						consumerKey: oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
						consumerSecret: oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
						requestTokenUrl: oauth1.OAuth1_UriRequestToken,
						authorizeUrl: oauth1.OAuth_UriAuthorization, 
						accessTokenUrl: oauth1.OAuth1_UriAccessToken, 
						callbackUrl: oauth1.OAuth_UriCallbackAKARedirect, 
						getUsernameAsync: null
						);

			auth.Completed += auth_Completed;

			//PresentViewController (auth.GetUI (), true, null);

			Uri uri = auth.GetUI();
			var rv = System.Windows.Application.Current.RootVisual;
			Microsoft.Phone.Controls.PhoneApplicationFrame paf = null;

			paf = rv as Microsoft.Phone.Controls.PhoneApplicationFrame;
			paf.Navigate(uri);

			return;
		}


		private void Login (HolisticWare.Auth.OAuth2 oauth2)
		{
			global::Xamarin.Auth.OAuth2Authenticator auth = null;

			if (
				null == oauth2.OAuth1_UriAccessToken)
			{
				try
				{
					auth = 
						new global::Xamarin.Auth.OAuth2Authenticator 
						(
						clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
						scope: oauth2.OAuth2_Scope,
						authorizeUrl: oauth2.OAuth_UriAuthorization,
						redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
						getUsernameAsync: null
					);
				}
				catch (System.Exception exc)
				{
					throw exc;
				}
			}
			else
			{
				auth = 
					new global::Xamarin.Auth.OAuth2Authenticator 
						(
						clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer, 
						clientSecret: oauth2.OAuth1_SecretKey_ConsumerSecret_APISecret,
						scope: oauth2.OAuth2_Scope,
						authorizeUrl: oauth2.OAuth_UriAuthorization,
						redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
						accessTokenUrl: oauth2.OAuth1_UriAccessToken,
						getUsernameAsync: null
						);

			}
			auth.Completed += auth_Completed;

			//PresentViewController (auth.GetUI (), true, null);

			Uri uri = auth.GetUI();
			var rv = System.Windows.Application.Current.RootVisual;
			Microsoft.Phone.Controls.PhoneApplicationFrame paf = null;

			paf = rv as Microsoft.Phone.Controls.PhoneApplicationFrame;
			paf.Navigate(uri);

			return;
		}

		private void auth_Completed(object sender, global::Xamarin.Auth.AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				// e.Account contains info:
				//		e.AccountProperties[""]
				//
				// use access tokenmore detailed user info from the API

				this.AccountProperties = e.Account.Properties;
			}
			else
			{
				// The user cancelled
			}

			// dismiss UI on iOS, because it was manually created
			//DismissViewController(true, null);

			// possibly do something to dismiss THIS viewcontroller, 
			// or else login screen does not disappear             

			var rv = System.Windows.Application.Current.RootVisual;
			Microsoft.Phone.Controls.PhoneApplicationFrame paf = null;

			paf = rv as Microsoft.Phone.Controls.PhoneApplicationFrame;
			//paf.GoBack();


			return;
		}

		public Xamarin.Auth.Helpers.OAuth OAuth
		{
			get;
			set;
		}

		protected Dictionary<string, string> account_properties;

		public Dictionary<string, string> AccountProperties
		{
			protected get
			{
				return account_properties;
			}
			set
			{
				this.OAuth.AccountProperties = account_properties = value;
			}
		}
	}
}