using System;
using System.Text;

namespace Xamarin.Auth.Providers.Google
{
    public partial class NativeUIAuthenticator : OAuth2Authenticator
    {
        public NativeUIAuthenticator
                    (
                        string client_id,
                        string scope,
                        GetUsernameAsyncFunc usernamer_getter_async
                    )
            : base
                (
                    clientId:
                         new Func<string>
                            (
                                () =>
                                {
                                    StringBuilder sb = new StringBuilder(client_id);

                                    if (client_id.Contains("-") && !client_id.Contains("apps.googleusercontent.com"))
                                    {
                                        sb.Append('.').Append("apps.googleusercontent.com");

                                    }

                                    return sb.ToString();
                                }
                           ).Invoke(),
                    clientSecret: null,  
                    authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                    accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                    redirectUrl:
                         new Func<Uri>
                            (
                                () =>
                                {
                                    StringBuilder sb = new StringBuilder();
                                    string cid = null;
                                    string path = "oauth2redirect";

                                    if (client_id.Contains("apps.googleusercontent.com"))
                                    {
                                        int idx = client_id.IndexOf('.');
                                        cid = client_id.Substring(0, idx);
                                    }
                                    sb.Append("com.googleusercontent.apps")
                                      .Append('.')
                                      .Append(cid)
                                      .Append(":/")
                                      .Append(path)
                                      ;

                                    return new Uri(sb.ToString());
                                }
                           ).Invoke(),
                    scope: scope,
                    getUsernameAsync: usernamer_getter_async,
                    isUsingNativeUI: true
                )
        {
        }
    }
}
