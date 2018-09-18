using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace LoginAccounts
{
    public abstract class FacebookBase : OAuth2Provider
    {
        public override string ProviderName => "Facebook";
        public override string ClientId => "503449900169497";
        public override string Scope => "public_profile,email";
        public override Uri AuthorizationUri => new Uri("https://www.facebook.com/v3.1/dialog/oauth");

        public override async Task<string> RetriveUsernameAsync(Account account)
        {
            var token = account.Properties["access_token"];
            var requestUri = $"https://graph.facebook.com/v2.3/me?fields=name&access_token={token}";

            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(requestUri);
                var result = JObject.Parse(json);
                return (string)result["name"];
            }
        }
    }

    public class FacebookHttp : FacebookBase
    {
        public override string ProviderVariant => "Facebook (with HTTP)";
        public override string Description => "Login with Facebook and use an HTTP redirect uri";
        public override Uri RedirectUri => new Uri("http://auth.xamarin.com/login");
    }

    public class FacebookHttps : FacebookBase
    {
        public override string ProviderVariant => "Facebook (with HTTPS)";
        public override string Description => "Login with Facebook and use an HTTPS redirect uri";
        public override Uri RedirectUri => new Uri("https://auth.xamarin.com/login");
    }

    public class FacebookLocalhost : FacebookBase
    {
        public override string ProviderVariant => "Facebook (with localhost)";
        public override string Description => "Login with Facebook and use http://localhost/ as the redirect uri";
        public override Uri RedirectUri => new Uri("http://localhost/");
    }

    public class FacebookLocalhostIp : FacebookBase
    {
        public override string ProviderVariant => "Facebook (with localhost)";
        public override string Description => "Login with Facebook and use http://127.0.0.1/ as the redirect uri";
        public override Uri RedirectUri => new Uri("http://127.0.0.1/");
    }
}
