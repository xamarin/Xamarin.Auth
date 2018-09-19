using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace LoginAccounts
{
    public class Fitbit : OAuth2Provider
    {
        public override string ProviderName => "Fitbit";
        public override string ProviderVariant => "Fitbit (with custom scheme)";
        public override string Description => "Log in to Fitbit and use a custom scheme";
        public override string ClientId => "22D6X4";
        public override string Scope => "profile";
        public override Uri AuthorizationUri => new Uri("https://www.fitbit.com/oauth2/authorize");
        public override Uri RedirectUri => new Uri("xamarin-auth://login");

        public override async Task<string> RetriveUsernameAsync(Account account)
        {
            var token = account.Properties["access_token"];
            var requestUri = $"https://api.fitbit.com/1/user/-/profile.json";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = await client.GetStringAsync(requestUri);
                var result = JObject.Parse(json);
                return (string)result["user"]["fullName"];
            }
        }
    }
}
