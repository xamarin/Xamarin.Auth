using System;
using System.Text;

namespace Xamarin.Auth.Providers.Facebook
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
                    clientId: client_id,
                    authorizeUrl: new Uri("https://www.facebook.com/v2.9/dialog/oauth"),
                    redirectUrl: new Uri($"fb{client_id}://authorize"),
                    scope: scope,
                    getUsernameAsync: usernamer_getter_async,
                    isUsingNativeUI: true
                )
        {
        }
    }
}
