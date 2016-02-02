using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xamarin.Auth.XamarinForms
{
	/// <summary>
	/// Page login.
	/// </summary>
    public partial class PageOAuth : ContentPage
	{
        public string oauth1_application_id_aka_client_id = null;
        public string oauth1_consumer_secret = null;
        public Uri oauth1_uri_reuest_token = null;
        public Uri oauth1_uri_authorize = null;
        public Uri oauth1_uri_access_token = null;
        public Uri oauth1_uri_callback_redirect = null;
        public GetUsernameAsyncFunc oauth1_func_get_username = null                                  			;
        public bool allow_cancel = true;

        public PageOAuth 
                    (
                        string application_id_aka_client_id, 
                        string consumer_secret, 
                        Uri uri_request_token, 
                        Uri uri_authorize,
                        Uri uri_access_token, 
                        Uri uri_callback_redirect,
                        GetUsernameAsyncFunc func_get_username,
                        bool allow_cancel = true
                    )
                : this()
        {
            oauth1_application_id_aka_client_id = application_id_aka_client_id;
            oauth1_consumer_secret = consumer_secret;
            oauth1_uri_reuest_token = uri_request_token;
            oauth1_uri_authorize = uri_authorize;
            oauth1_uri_access_token = uri_access_token;
            oauth1_uri_callback_redirect = uri_callback_redirect;
            oauth1_func_get_username = func_get_username;
            allow_cancel = allow_cancel;

            DefineUserInterface();

            return;
        }

        public string oauth2_application_id_aka_client_id = null;
        public string oauth2_scope = null;
        public Uri oauth2_uri_authorization = null;
        public Uri oauth2_uri_callback_redirect = null;
        public GetUsernameAsyncFunc oauth2_func_get_username = null                                             ;

        public PageOAuth 
					(
    					string application_id_aka_client_id, 
    					string scope, 
    					Uri uri_authorization, 
    					Uri uri_callback_redirect,
                        GetUsernameAsyncFunc func_get_username,
                        bool allow_cancel = true
					)
				: this()
		{
            oauth2_application_id_aka_client_id = application_id_aka_client_id;
            oauth2_scope = scope;
            oauth2_uri_authorization = uri_authorization;
            oauth2_uri_callback_redirect = uri_callback_redirect;
            oauth2_func_get_username = func_get_username;
            allow_cancel = allow_cancel;

            DefineUserInterface();

			return;
		}
        
        protected PageOAuth ()
		{
			return;
		}

		private void DefineUserInterface ()
		{
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
         

			section.Add(cells_account_properties);
			root.Add(section);
			table_view.Root = root;

			Button buttonOAuthProvider = new Button () 
			{
				Text = "Authenticate"
			};
			buttonOAuthProvider.Clicked += ButtonOAuthProvider_Clicked;
			StackLayout stack_layout = new StackLayout ();
			stack_layout.Children.Add (buttonOAuthProvider);
			stack_layout.Children.Add (table_view);

			this.Content = stack_layout;

			return;
		}

		protected void ButtonOAuthProvider_Clicked (object sender, EventArgs e)
		{			
			this.Navigation.PushAsync (new PageOAuth ());

			return;
		}
	}
}


