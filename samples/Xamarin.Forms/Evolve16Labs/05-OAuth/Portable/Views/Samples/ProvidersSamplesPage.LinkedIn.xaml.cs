using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Xamarin.Forms;
using Xamarin.Auth.XamarinForms;

using Xamarin.Auth;

using ComicBook;
using ComicBook.Utilities;

namespace ComicBook
{
    public partial class ProvidersSamplesPage
    {

        protected void LinkedIn()
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
                                    if (Settings.IsUsingNativeUI)
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
                                    if (Settings.IsUsingNativeUI)
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
                     isUsingNativeUI: Settings.IsUsingNativeUI
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
            AuthenticationState.Authenticator = authenticator;

            this.PresentUILoginScreen(authenticator);

            return;
        }


    }
}