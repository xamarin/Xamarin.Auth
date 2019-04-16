using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Xamarin.Auth;
using Xamarin.Auth.XamarinForms;

using ComicBook.Utilities;

namespace ComicBook
{
    public partial class UserAgentTestPage : ContentPage
    {
        protected Xamarin.Auth.WebAuthenticator authenticator = null;
        
        public UserAgentTestPage()
        {
            InitializeComponent();

            buttonTestUserAgent.Clicked += ButtonTestUserAgent_Clicked;

            return;
        }

        private void ButtonTestUserAgent_Clicked(object sender, EventArgs e)
        {
            string user_agent = entryUserAgent.Text;

            EmbeddedWebViewConfiguration webview_conf;

            webview_conf = new EmbeddedWebViewConfiguration
            {
                UserAgent = user_agent,
                IsUsingWKWebView = Settings.IsIOSUsingWKWebView
            };

            authenticator
                 = new Xamarin.Auth.OAuth2Authenticator
                 (
                     clientId:
                         new Func<string>
                            (
                                () =>
                                {
                                    string retval_client_id = "not used!";

                                    return retval_client_id;
                                }
                            ).Invoke(),
                     //clientSecret: null,   // null or ""
                     authorizeUrl:
                         new Func<Uri>
                            (
                                () =>
                                {
                                    string uri = null;
                                    uri = "http://www.whatsmyua.info/";
                                    
                                    return new Uri(uri);
                                }
                            ).Invoke(),
                     //accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                     redirectUrl:
                         new Func<Uri>
                            (
                                () =>
                                {
                                    string uri = null;
                                    if (Settings.IsUsingNativeUI)
                                    {
                                        uri =
                                            //"fb1889013594699403://localhost/path"
                                            //"fb1889013594699403://xamarin.com"
                                            $"https://localhost"
                                            ;
                                    }
                                    else
                                    {
                                        uri = 
                                            //"https://localhost/path"
                                            $"https://localhost"
                                            ;
                                    }
                                    return new Uri(uri);
                                }
                            ).Invoke(),
                     scope: "", // "basic", "email",
                     getUsernameAsync: null,
                     isUsingNativeUI: Settings.IsUsingNativeUI
                 )
                 {
                     AllowCancel = true,
                 };

            this.PresentUILoginScreen(authenticator);

            return;
        }
    }
}
