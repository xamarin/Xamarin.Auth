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
        AccountStore store = null;

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

            this.pickerUIFrameworks.SelectedIndexChanged += pickerUIFrameworks_SelectedIndexChanged;
            this.pickerFormsImplementations.SelectedIndexChanged += pickerFormsImplementations_SelectedIndexChanged;
            this.pickerNavigationType.SelectedIndexChanged += pickerNavigationType_SelectedIndexChanged;
            this.pickerViews.SelectedIndexChanged += pickerViews_SelectedIndexChanged;

            this.pickerUIFrameworks.SelectedIndex = 0;
            this.pickerFormsImplementations.SelectedIndex = 0;
            this.pickerNavigationType.SelectedIndex = 0;

            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case "iOS":
                    this.pickerViews.SelectedIndex = 0;
                    break;
            }

            buttonGoogle.Clicked += ButtonGoogle_Clicked;
            buttonFacebook.Clicked += ButtonFacebook_Clicked;

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
                if (navigation_push_modal == true)
                {
                    Navigation.PushModalAsync(new Xamarin.Auth.XamarinForms.AuthenticatorPage());
                }
                else
                {
                    Navigation.PushAsync(new Xamarin.Auth.XamarinForms.AuthenticatorPage());
                }
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

            PresentUILoginScreen(authenticator);

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
            "Custom Renderers",
            "Presenters (Dependency Service/Injection)",
        };

        protected void pickerFormsImplementations_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            string implementation = ((string)p.SelectedItem);
            if (string.IsNullOrEmpty(implementation))
                return;

            if (implementation == "Presenters (Dependency Service/Injection)")
            {
                System.Diagnostics.Debug.WriteLine("Presenters (Dependency Service/Injection)");

                forms_implementation_renderers = false;
            }
            else if (implementation == "Custom Renderers")
            {
                System.Diagnostics.Debug.WriteLine("Custom Renderers");

                forms_implementation_renderers = true;
            }
            else
            {
                throw new ArgumentException("FormsImplementation error");
            }

            return;
        }

        public static bool navigation_push_modal = false;
        public List<string> NavigationTypes => _NavigationTypes;

        List<string> _NavigationTypes = new List<string>()
        {
            "PushModalAsync",
            "PushAsync",
        };

        protected void pickerNavigationType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            string navigation_type = ((string)p.SelectedItem);
            if (string.IsNullOrEmpty(navigation_type))
                return;

            if (navigation_type == "PushAsync")
            {
                System.Diagnostics.Debug.WriteLine("PushAsync");

                navigation_push_modal = false;
            }
            else if (navigation_type == "PushModalAsync")
            {
                System.Diagnostics.Debug.WriteLine("PushModalAsync");

                navigation_push_modal = true;
            }
            else
            {
                throw new ArgumentException("NavigationTypes error");
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

        protected void pickerViews_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            web_view = ((string)p.SelectedItem);

            IEmbeddedWebViewConfiguration cfg = DependencyService.Get<IEmbeddedWebViewConfiguration>();

            if (null == cfg)
            {
                //TODO: check dependency service

                return;
            }

            if (web_view == "UIWebView")
            {
                System.Diagnostics.Debug.WriteLine("UIWebView");

                cfg.IsUsingWKWebView = false;
            }
            else if (web_view == "WKWebView")
            {
                System.Diagnostics.Debug.WriteLine("WKWebView");

                cfg.IsUsingWKWebView = true;
            }
            else
            {
                throw new ArgumentException("WebView error");
            }

            return;
        }

        Xamarin.Auth.OAuth2Authenticator authenticator = null;

        private void PresentUILoginScreen(OAuth2Authenticator authenticator)
        {
            if (forms_implementation_renderers)
            {
                // Renderers Implementaion

                Xamarin.Auth.XamarinForms.AuthenticatorPage ap;
                ap = new Xamarin.Auth.XamarinForms.AuthenticatorPage()
                {
                    Authenticator = authenticator,
                };

                NavigationPage np = new NavigationPage(ap);

                if (navigation_push_modal == true)
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        PushModal");
                    System.Diagnostics.Debug.WriteLine("        Custom Renderers");

                    Navigation.PushModalAsync(np);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        Push");
                    System.Diagnostics.Debug.WriteLine("        Custom Renderers");

                    Navigation.PushAsync(np);
                }
            }
            else
            {
                // Presenters Implementation

                if (navigation_push_modal == true)
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        PushModal");
                    System.Diagnostics.Debug.WriteLine("        Presenters");

                    Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                    presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(authenticator);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        Push");
                    System.Diagnostics.Debug.WriteLine("        Presenters");

                    Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                    presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(authenticator);
                }
            }

            return;
        }

    }
}