using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace MinimalTestCase
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            string fb_app_id = "1889013594699403";
            bool native_ui = true;

            var authenticator
                 = new Xamarin.Auth.OAuth2Authenticator
                 (
                     clientId:
                         new Func<string>
                            (
                                () =>
                                {
                                    string retval_client_id = "oops something is wrong!";

                                    retval_client_id = fb_app_id;
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
                                    if (native_ui)
                                    {
                                        uri = "https://www.facebook.com/v2.9/dialog/oauth";
                                    }
                                    else
                                    {
                                        // old
                                        uri = "https://m.facebook.com/dialog/oauth/";
                                    }
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
                                    if (native_ui)
                                    {
                                        uri =
                                            //"fb1889013594699403://localhost/path"
                                            //"fb1889013594699403://xamarin.com"
                                            $"fb{fb_app_id}://authorize"
                                            ;
                                    }
                                    else
                                    {
                                        uri =
                                            //"https://localhost/path"
                                            $"fb{fb_app_id}://authorize"
                                            ;
                                    }
                                    return new Uri(uri);
                                }
                            ).Invoke(),
                     scope: "", // "basic", "email",
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

            // after initialization (creation and event subscribing) exposing local object 
            //AuthenticationState.Authenticator = authenticator;

            PresentUILoginScreen(authenticator);

        }

        private void PresentUILoginScreen(OAuth2Authenticator authenticator)
        {
            bool navigation_push_modal = false;
            bool forms_implementation_renderers = false;

            if (forms_implementation_renderers)
            {
                // Renderers Implementaion

                /*
                Xamarin.Auth.XamarinForms.AuthenticatorPage ap;
                ap = new Xamarin.Auth.XamarinForms.AuthenticatorPage()
                {
                    Authenticator = authenticator
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
                */
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
