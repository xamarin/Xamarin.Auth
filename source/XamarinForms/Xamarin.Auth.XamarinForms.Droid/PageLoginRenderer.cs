using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth.XamarinForms;
using Xamarin.Auth;


[assembly: 
    Xamarin.Forms.ExportRenderer
            (
            // ViewElement to be rendered (from Portable/Shared)
            typeof(Xamarin.Auth.XamarinForms.PageOAuth),
            // platform specific Renderer : global::Xamarin.Forms.Platform.XamarinIOS.PageRenderer
            typeof(Xamarin.Auth.XamarinForms.XamarinAndroid.PageOAuthRenderer)
            )
]
namespace Xamarin.Auth.XamarinForms.XamarinAndroid
{
    public partial class PageOAuthRenderer : global::Xamarin.Forms.Platform.Android.PageRenderer
    {
        PageOAuth e_new = null;

        bool IsShown;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // OnElementChanged using it to pass data

            e_new = e.NewElement as PageOAuth;

            // PageRenderer is a ViewGroup - so should be able to load an AXML file and FindView<>
            activity = this.Context as Activity;

            if (!IsShown)
            {

                IsShown = true;

                if 
                    (
                        null != e_new.oauth1_application_id_aka_client_id
                        &&
                        null != e_new.oauth1_consumer_secret
                        &&
                        null != e_new.oauth1_uri_reuest_token
                        &&
                        null != e_new.oauth1_uri_authorize
                        &&
                        null != e_new.oauth1_uri_access_token
                        &&
                        null != e_new.oauth1_uri_callback_redirect
                    )
                {
                    this.Authenticate
                            (
                                e_new.oauth1_application_id_aka_client_id,
                                e_new.oauth1_consumer_secret,
                                e_new.oauth1_uri_reuest_token,
                                e_new.oauth1_uri_authorize,
                                e_new.oauth1_uri_access_token,
                                e_new.oauth1_uri_callback_redirect,
                                e_new.oauth2_func_get_username,
                                e_new.allow_cancel
                            );
                    return;
                }
                else if 
                    (
                        null != e_new.oauth2_application_id_aka_client_id
                        &&
                        null != e_new.oauth2_scope
                        &&
                        null != e_new.oauth2_uri_authorization
                        &&
                        null != e_new.oauth2_uri_callback_redirect
                    )
                {
                    this.Authenticate
                            (
                                e_new.oauth2_application_id_aka_client_id,
                                e_new.oauth2_scope,
                                e_new.oauth2_uri_authorization,
                                e_new.oauth2_uri_callback_redirect,
                                e_new.oauth2_func_get_username,
                                e_new.allow_cancel
                            );
                    return;
                }
                else
                {
                    throw new ArgumentException("Invalid OAuthenticator");
                }
            }

            return;
        }

        global::Android.App.Activity activity = null;

        private void Authenticate
                        (
                            string application_id_aka_client_id, 
                            string consumer_secret, 
                            Uri uri_reuest_token, 
                            Uri uri_authorize,
                            Uri uri_access_token, 
                            Uri uri_callback_redirect,
                            GetUsernameAsyncFunc func_get_username,
                            bool allow_cancel = true
                        )
        {
            OAuth1Authenticator auth = new OAuth1Authenticator 
                (
                    application_id_aka_client_id,
                    consumer_secret,
                    uri_reuest_token,
                    uri_authorize,
                    uri_access_token,
                    uri_callback_redirect,
                    func_get_username
                );

            auth.AllowCancel = allow_cancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;

            activity.StartActivity (auth.GetUI(activity));

            return;
        }
        
        private void Authenticate
                        (
                            string application_id_aka_client_id, 
                            string scope, 
                            Uri uri_authorization, 
                            Uri uri_callback_redirect,
                            GetUsernameAsyncFunc func_get_username,
                            bool allow_cancel = true
                        )
        {
            OAuth2Authenticator auth = new OAuth2Authenticator 
                (
                    application_id_aka_client_id,
                    scope,
                    uri_authorization,
                    uri_callback_redirect,
                    func_get_username
                );

            auth.AllowCancel = allow_cancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;

            activity.StartActivity (auth.GetUI(activity));

            return;
        }
        
        private void Auth_Completed(object sender, global::Xamarin.Auth.AuthenticatorCompletedEventArgs e)
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
            // IOS
            // 			DismissViewController(true, null);
            // Android

            // possibly do something to dismiss THIS viewcontroller, 
            // or else login screen does not disappear             

            return;
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
                account_properties = value;
            }
        }
    }
}