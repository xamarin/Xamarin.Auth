using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Auth.XamarinForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageOAuth : ContentPage
    {
        public PageOAuth()
        {
            InitializeComponent();
        }

        public string oauth1_application_id_aka_client_id = null;
        public string oauth1_consumer_secret = null;
        public Uri oauth1_uri_reuest_token = null;
        public Uri oauth1_uri_authorize = null;
        public Uri oauth1_uri_access_token = null;
        public Uri oauth1_uri_callback_redirect = null;
        public GetUsernameAsyncFunc oauth1_func_get_username = null;
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
            bool allow_cncl = true
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
            allow_cancel = allow_cncl;

            return;
        }


        public string oauth2_application_id_aka_client_id = null;
        public string oauth2_scope = null;
        public Uri oauth2_uri_authorization = null;
        public Uri oauth2_uri_callback_redirect = null;
        public GetUsernameAsyncFunc oauth2_func_get_username = null;

        public PageOAuth
        (
            string application_id_aka_client_id,
            string scope,
            Uri uri_authorization,
            Uri uri_callback_redirect,
            GetUsernameAsyncFunc func_get_username,
            bool allow_cncl = true
        )
            : this()
        {
            oauth2_application_id_aka_client_id = application_id_aka_client_id;
            oauth2_scope = scope;
            oauth2_uri_authorization = uri_authorization;
            oauth2_uri_callback_redirect = uri_callback_redirect;
            oauth2_func_get_username = func_get_username;
            allow_cancel = allow_cncl;

            return;
        }
        public OAuth1Authenticator OAuth1Authenticator = null;
        public OAuth2Authenticator OAuth2Authenticator = null;

        public void OnItemSelected(object sender, ItemTappedEventArgs args_tapped)
        {
            object item = args_tapped.Item;

            KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

            string authentication_provider = kvp.Value;

            //switch (authentication_provider)
            //{
            //	default:
            //		string msg = "Unknown Authentication Provider: " + authentication_provider;
            //		//throw new NotImplementedException(msg);
            //}

            this.Navigation.PushAsync(this);

            return;

        }

    }
}
