using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xamarin.Auth.XamarinForms
{
	/// <summary>
	/// Page login.
	/// </summary>
	public partial class PageLogin : ContentPage
	{

		// TODO: replace with Messaging
		//-------------------------------------------------------------------------
		# region  Property OAuth OAuth w Event pre and post
		/// <summary>
		/// OAuth
		/// </summary>
		public Xamarin.Auth.Helpers.OAuth OAuth
		{
			get
			{
				return oauth;
			} // OAuth.get
			set
			{
				// for multi threading apps uncomment lines beginnig with //MT:
				//if (oauth != value)		// do not write if equivalent/equal/same
				{
					// raise/trigger Event if somebody has subscribed to the event
					if (null != OAuthChanging)
					{
						// raise/trigger Event
						OAuthChanging(this, new EventArgs());
					}

					//MT: lock(oauth) // MultiThread safe
					{
						// Set the property value
						oauth = value;
						// raise/trigger Event if somebody has subscribed to the event
						if (null != OAuthChanged)
						{
							// raise/trigger Event
							OAuthChanged(this, new EventArgs());
						}
					}

					// raise/trigger Event if somebody has subscribed to the event
					if (null != OAuthChangePerformed)
					{
						// raise/trigger Event
						OAuthChangePerformed(this, new EventArgs());
					}
				}

				return;
			} // OAuth.set
		} // OAuth

		private Xamarin.Auth.Helpers.OAuth oauth;

		public event EventHandler OAuthChanged;

		public event EventHandler OAuthChanging;

		public event EventHandler OAuthChangePerformed
			;
		# endregion Property OAuth OAuth w Event pre and post
		//-------------------------------------------------------------------------
			
		public PageLogin 
					(
					string application_id_aka_client_id, 
					string scope, 
					Uri uri_authorization, 
					Uri uri_callback_redirect
					)
				: this()
		{
			this.OAuth = new Xamarin.Auth.Helpers.OAuth2()
			{
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = application_id_aka_client_id,
				OAuth2_Scope = scope,
				OAuth_UriAuthorization = uri_authorization,
				OAuth_UriCallbackAKARedirect = uri_callback_redirect,
			};

			return;
		}

		public PageLogin (Xamarin.Auth.Helpers.OAuth2 oauth2)
			: this
					(
					oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					oauth2.OAuth2_Scope,
					oauth2.OAuth_UriAuthorization,
					oauth2.OAuth_UriCallbackAKARedirect
					)
		{
			return;
		}

		protected PageLogin ()
		{
			this.OAuthChanged += PageLogin_OAuthChanged;
			DefineUserInterface();

			return;
		}

		void PageLogin_OAuthChanged(object sender, EventArgs e)
		{
			DefineUserInterface();

			return;
		}

		public PageLogin 
					(
					string application_id_aka_client_id, 
					string client_customer_secret, 
					Uri uri_request_token, 
					Uri uri_authorization, 
					Uri uri_access_token, 
					Uri uri_callback_redirect
					)
				: this()
		{
			this.OAuth =  new Xamarin.Auth.Helpers.OAuth1()
			{
				OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = application_id_aka_client_id,
				OAuth1_SecretKey_ConsumerSecret_APISecret = client_customer_secret,
				OAuth1_UriRequestToken = uri_request_token,
				OAuth_UriAuthorization = uri_authorization,
				OAuth1_UriAccessToken = uri_access_token,
				OAuth_UriCallbackAKARedirect = uri_callback_redirect,
			};

			return;
		}

		public PageLogin (Xamarin.Auth.Helpers.OAuth1 oauth1)
			: this
					(
					oauth1.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
					oauth1.OAuth1_SecretKey_ConsumerSecret_APISecret,
					oauth1.OAuth1_UriRequestToken,
					oauth1.OAuth_UriAuthorization,
					oauth1.OAuth1_UriAccessToken,
					oauth1.OAuth_UriCallbackAKARedirect
					)
		{
			return;
		}


		private void DefineUserInterface ()
		{
			if 
				(
				null == this.OAuth
				||
				null == this.OAuth.AccountProperties
				)
			{
				return;
			}

			TableView table_view = new TableView
			{
				Intent = TableIntent.Form,
			};

			TableRoot root = new TableRoot ("Xamarin.Auth Authenticated")
			{
			};

			TableSection section = new TableSection ("Account Properties")
			{
			};

			List<TextCell> cells_account_properties = new List<TextCell>();

			foreach (var p in this.OAuth.AccountProperties)
			{
				TextCell tc =  new TextCell()
				{
					Text = p.Key,
					Detail = p.Value
				};

				cells_account_properties.Add(tc);
			}

			section.Add(cells_account_properties);
			root.Add(section);
			table_view.Root = root;

			this.Content = table_view;

			return;
		}
	}
}


