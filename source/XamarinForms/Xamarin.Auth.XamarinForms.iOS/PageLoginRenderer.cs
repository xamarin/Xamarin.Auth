using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Xamarin.Auth.XamarinForms;

[assembly: 
    Xamarin.Forms.ExportRenderer
            (
            // ViewElement to be rendered (from Portable/Shared)
            typeof(Xamarin.Auth.XamarinForms.PageOAuth),
            // platform specific Renderer : global::Xamarin.Forms.Platform.iOS.PageRenderer
            typeof(Xamarin.Auth.XamarinForms.XamarinIOS.PageOAuthRenderer)
            )
]

namespace Xamarin.Auth.XamarinForms.XamarinIOS
{
    public partial class PageOAuthRenderer : global::Xamarin.Forms.Platform.iOS.PageRenderer
    {
        PageOAuth e_new = null;

        bool IsShown;

        // public class VisualElementChangedEventArgs : ElementChangedEventArgs<VisualElement>
        protected override void OnElementChanged (VisualElementChangedEventArgs e)
        {
            base.OnElementChanged (e);

            // OnElementChanged is fired before ViewDidAppear, using it to pass data

            e_new = e.NewElement as PageOAuth;
                     
            return;
        }

        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);

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

            PresentViewController (auth.GetUI (), true, null);

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
            global::Xamarin.Auth.OAuth2Authenticator auth = null;

            if 
                (
                    //null == oauth2.OAuth1_UriAccessToken
                    true
                )
            {
                try
                {
                    auth = new global::Xamarin.Auth.OAuth2Authenticator 
                        (
                            application_id_aka_client_id,
                            scope,
                            uri_authorization,
                            uri_callback_redirect,
                            func_get_username
                        );
                }
                catch (System.Exception exc)
                {
					string msg = exc.Message;
					System.Diagnostics.Debug.WriteLine ("Xamarin.Auth.OAuth2Authenticator exception = {0}", msg);

                    throw;
                }
            }
            else
            {
                try
                {
                    auth = new global::Xamarin.Auth.OAuth2Authenticator 
                        (
                            application_id_aka_client_id, 
                            //clientSecret: oauth2.OAuth1_SecretKey_ConsumerSecret_APISecret,
                            scope,
                            uri_authorization,
                            uri_callback_redirect,
                            //accessTokenUrl: oauth2.OAuth1_UriAccessToken,
                            func_get_username
                        );
                }
                catch (System.Exception exc)
                {
                    throw;
                }
            }

            auth.AllowCancel = allow_cancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += Auth_Completed;

            PresentViewController (auth.GetUI (), true, null);

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
            DismissViewController(true, null);

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