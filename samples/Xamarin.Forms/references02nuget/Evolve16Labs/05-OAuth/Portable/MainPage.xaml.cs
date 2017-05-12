using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using ComicBookPCL;
using System.Text;
using Xamarin.Auth.XamarinForms;

namespace ComicBook
{
    public partial class MainPage : ContentPage
    {
        const string ServiceId = "ComicBook";
        const string Scope = "profile";

        Account account;
        AccountStore store;

        public MainPage()
        {
            InitializeComponent();

            implicitButton.Clicked += ImplicitButtonClicked;
            authorizationCodeButton.Clicked += AuthorizationCodeButtonClicked;
            getProfileButton.Clicked += GetProfileButtonClicked;
            refreshButton.Clicked += RefreshButtonClicked;
            /*
            store = AccountStore.Create();
            account = store.FindAccountsForService(ServiceId).FirstOrDefault();

            if (account != null)
            {
                statusText.Text = "Restored previous session";
                getProfileButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
            }
            */
            this.BindingContext = this;

            this.pickerUIFrameworks.SelectedIndex = 0;
            this.pickerFormsImplementations.SelectedIndex = 0;

            Device.OnPlatform
                  (
                      iOS: () => this.pickerViews.SelectedIndex = 0
                  );

            buttonGoogle.Clicked += ButtonGoogle_Clicked;

            return;
        }

        void ImplicitButtonClicked(object sender, EventArgs e)
        {
            OAuth2Authenticator authenticator = new OAuth2Authenticator
                (
                    ServerInfo.ClientId,
                    Scope,
                    ServerInfo.AuthorizationEndpoint,
                    ServerInfo.RedirectionEndpoint,
                    null,
                    isUsingNativeUI: native_ui
                );

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            if (forms_implementation_renderers)
            {
                // Renderers Implementaion
                //Navigation.PushModalAsync(new Xamarin.Auth.XamarinForms.AuthenticatorPage());
            }
            else
            {
                // Presenters Implementation
                Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);
            }

            return;
        }

        void AuthorizationCodeButtonClicked(object sender, EventArgs e)
        {
            OAuth2Authenticator authenticator = new OAuth2Authenticator
                (
                    ServerInfo.ClientId,
                    ServerInfo.ClientSecret,
                    Scope,
                    ServerInfo.AuthorizationEndpoint,
                    ServerInfo.RedirectionEndpoint,
                    ServerInfo.TokenEndpoint,
                    null,
                    isUsingNativeUI: true
                );

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            if (forms_implementation_renderers)
            {
                // Renderers Implementaion
                //Navigation.PushModalAsync(new Xamarin.Auth.XamarinForms.AuthenticatorPage());
            }
            else
            {
                // Presenters Implementation
                Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);
            }

            return;
        }

        async void GetProfileButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var request = new OAuth2Request("GET", ServerInfo.ApiEndpoint, null, account);
                var response = await request.GetResponseAsync();

                var text = response.GetResponseText();

                var json = JObject.Parse(text);

                var name = (string)json["Name"];
                var email = (string)json["Email"];
                var imageUrl = (string)json["ImageUrl"];

                nameText.Text = name;
                emailText.Text = email;

                var imageRequest = new OAuth2Request("GET", new Uri(imageUrl), null, account);
                var stream = await (await imageRequest.GetResponseAsync()).GetResponseStreamAsync();

                profileImage.Source = ImageSource.FromStream(() => stream);

                statusText.Text = "Get data succeeded";
            }
            catch (Exception x)
            {
                getProfileButton.IsEnabled = false;
                statusText.Text = "Get data failure: " + x.Message + "\r\nHas the access token expired?";
            }
        }

        async void RefreshButtonClicked(object sender, EventArgs e)
        {
            var refreshToken = account.Properties["refresh_token"];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return;

            var queryValues = new Dictionary<string, string>
            {
                {"refresh_token", refreshToken},
                {"client_id", ServerInfo.ClientId},
                {"grant_type", "refresh_token"},
                {"client_secret", ServerInfo.ClientSecret},
            };

            var authenticator = new OAuth2Authenticator
                (
                    ServerInfo.ClientId,
                    ServerInfo.ClientSecret,
                    "profile",
                    ServerInfo.AuthorizationEndpoint,
                    ServerInfo.RedirectionEndpoint,
                    ServerInfo.TokenEndpoint,
                    null,
                    isUsingNativeUI: true
                );

            try
            {
                var result = await authenticator.RequestAccessTokenAsync(queryValues);

                if (result.ContainsKey("access_token"))
                    account.Properties["access_token"] = result["access_token"];

                if (result.ContainsKey("refresh_token"))
                    account.Properties["refresh_token"] = result["refresh_token"];

                store.Save(account, ServiceId);

                statusText.Text = "Refresh succeeded";
            }
            catch (Exception ex)
            {
                statusText.Text = "Refresh failed " + ex.Message;
            }
        }

        void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                getProfileButton.IsEnabled = true;

                if (this.account != null)
                    store.Delete(this.account, ServiceId);

                store.Save(account = e.Account, ServiceId);

                getProfileButton.IsEnabled = true;

                if (account.Properties.ContainsKey("expires_in"))
                {
                    var expires = int.Parse(account.Properties["expires_in"]);
                    statusText.Text = "Token lifetime is: " + expires + "s";
                }
                else
                {
                    statusText.Text = "Authentication succeeded";
                }

                if (account.Properties.ContainsKey("refresh_token"))
                    refreshButton.IsEnabled = true;
            }
            else
            {
                statusText.Text = "Authentication failed";
            }
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            statusText.Text = "Authentication error: " + e.Message;
        }

        // *
        public List<string> UIFrameworks => _UIFrameworks;

        List<string> _UIFrameworks = new List<string>()
        {
            "Native UI (Custom Tabs or SFSafariViewController",
            "Embedded WebView",
        };

        bool native_ui = true;

        protected void pickerUIFrameworks_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            if (((string)p.SelectedItem).Equals("Native UI (Custom Tabs or SFSafariViewController"))
            {
                native_ui = true;
                pickerViews.IsEnabled = false;
            }
            else if (((string)p.SelectedItem).Equals("Embedded WebView"))
            {
                native_ui = false;
                pickerViews.IsEnabled = true;
            }
            else
            {
                throw new ArgumentException("UIFramework error");
            }

            return;
        }

        bool forms_implementation_renderers = false;

        public List<string> FormsImplementations => _FormsImplementations;

        List<string> _FormsImplementations = new List<string>()
        {
            "Presenters (Dependency Service/Injection)",
            "Custom Renderers",
        };

        protected void pickerFormsImplementations_SelectedIndex(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            string implementation = ((string)p.SelectedItem);
            if (string.IsNullOrEmpty(implementation))
                return;

            if (implementation == "Presenters (Dependency Service/Injection)")
            {
                forms_implementation_renderers = false;
            }
            else if (implementation == "Custom Renderers")
            {
                forms_implementation_renderers = true;
            }
            else
            {
                throw new ArgumentException("FormsImplementation error");
            }

            return;
        }

        string web_view = null;

        public List<string> Views => _Views;

        List<string> _Views = new List<string>()
        {
            "UIWebView",
            "WKWebView",
        };

        protected void pickerViews_SelectedIndex(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            web_view = ((string)p.SelectedItem);

            if (web_view == "UIWebView")
            {
                DependencyService.Get<IEmbeddedWebViewConfiguration>().IsUsingWKWebView = false;
            }
            else if (web_view == "WKWebView")
            {
                DependencyService.Get<IEmbeddedWebViewConfiguration>().IsUsingWKWebView = true;
            }
            else
            {
                throw new ArgumentException("WebView error");
            }

            return;
        }

        Xamarin.Auth.OAuth2Authenticator authenticator = null;

        protected void ButtonGoogle_Clicked(object sender, EventArgs e)
        {
            authenticator
                 = new Xamarin.Auth.OAuth2Authenticator
                 (
                     clientId:
                         new Func<string>
                            (
                                 () =>
                                 {
                                     string retval_client_id = "oops something is wrong!";

                                     // some people are sending the same AppID for google and other providers
                                     // not sure, but google (and others) might check AppID for Native/Installed apps
                                     // Android and iOS against UserAgent in request from 
                                     // CustomTabs and SFSafariViewContorller
                                     // TODO: send deliberately wrong AppID and note behaviour for the future
                                     // fitbit does not care - server side setup is quite liberal
                                     switch (Xamarin.Forms.Device.RuntimePlatform)
                                     {
                                         case "Android":
                                             retval_client_id = "1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn.apps.googleusercontent.com";
                                             break;
                                         case "iOS":
                                             retval_client_id = "1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh.apps.googleusercontent.com";
                                             break;
                                         case "Windows":
                                             retval_client_id = "1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh.apps.googleusercontent.com";
                                             break;
                                     }
                                     return retval_client_id;
                                 }
                           ).Invoke(),
                     clientSecret: null,   // null or ""
                     authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                     accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                     redirectUrl:
                         new Func<Uri>
                            (
                                 () =>
                                 {

                                     string uri = null;

                                     // some people are sending the same AppID for google and other providers
                                     // not sure, but google (and others) might check AppID for Native/Installed apps
                                     // Android and iOS against UserAgent in request from 
                                     // CustomTabs and SFSafariViewContorller
                                     // TODO: send deliberately wrong AppID and note behaviour for the future
                                     // fitbit does not care - server side setup is quite liberal
                                     switch (Xamarin.Forms.Device.RuntimePlatform)
                                     {
                                         case "Android":
                                             uri =
                                                 "com.xamarin.traditional.standard.samples.oauth.providers.android:/oauth2redirect"
                                                 //"com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn:/oauth2redirect"
                                                 ;
                                             break;
                                         case "iOS":
                                             uri =
                                                 "com.xamarin.traditional.standard.samples.oauth.providers.ios:/oauth2redirect"
                                                 //"com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh:/oauth2redirect"
                                                 ;
                                             break;
                                         case "Windows":
                                             uri =
                                                 "com.xamarin.traditional.standard.samples.oauth.providers.ios:/oauth2redirect"
                                                 //"com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh:/oauth2redirect"
                                                 ;
                                             break;
                                     }

                                     return new Uri(uri);
                                 }
                             ).Invoke(),
                     scope:
                                  //"profile"
                                  "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/plus.login"
                                  ,
                     getUsernameAsync: null,
                     isUsingNativeUI: native_ui
                 )
                 {
                     AllowCancel = true,
                 };

            authenticator.Completed +=
                (s, ea) =>
                    {
                        StringBuilder sb = new StringBuilder();

                        if (ea.Account != null && ea.Account.Properties != null)
                        {
                            sb.Append("Token = ").AppendLine($"{ea.Account.Properties["access_token"]}");
                        }
                        else
                        {
                            sb.Append("Not authenticated ").AppendLine($"Account.Properties does not exist");
                        }

                        DisplayAlert
								(
                                    "Authentication Results",
									sb.ToString(),
                                    "OK"
                                );

                        return;
                    };

            authenticator.Error +=
                (s, ea) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Error = ").AppendLine($"{ea.Message}");

                        DisplayAlert
                                (
                                    "Authentication Error",
                                    sb.ToString(),
                                    "OK"
                                );
                        return;
                    };

            AuthenticationState.Authenticator = authenticator;

            if (forms_implementation_renderers)
            {
                // Renderers Implementaion
                Navigation.PushModalAsync(new Xamarin.Auth.XamarinForms.AuthenticatorPage());
            }
            else
            {
                // Presenters Implementation
                Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);
            }

            return;
        }


    }
}