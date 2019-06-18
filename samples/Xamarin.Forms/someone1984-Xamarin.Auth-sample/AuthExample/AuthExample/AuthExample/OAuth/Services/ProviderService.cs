using AuthExample.OAuth.Data;
using AuthExample.OAuth.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AuthExample.OAuth.Services
{
    public static class ProviderService
    {
        public static async Task<string> GetFacebookEmailAsync()
        {
            string facebookTokenString = await SecureStorage.GetAsync("FACEBOOK");
            string facebookToken = JsonConvert.DeserializeObject<FacebookToken>(facebookTokenString).AccessToken;

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync("https://graph.facebook.com/me?fields=email&access_token=" + facebookToken);

            if(!httpResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Could not get FACEBOOK email. Status: {httpResponse.StatusCode}");
            }
    
            string data = await httpResponse.Content.ReadAsStringAsync();
            FacebookData facebookData = JsonConvert.DeserializeObject<FacebookData>(data);

            return await Task.FromResult(facebookData.Email);
        }

        public static async Task<string> GetMicrosoftEmailAsync()
        {
            string microsoftTokenString = await SecureStorage.GetAsync("MICROSOFT");
            string microsoftToken = JsonConvert.DeserializeObject<MicrosoftToken>(microsoftTokenString).AccessToken;

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync("https://apis.live.net/v5.0/me?access_token=" + microsoftToken);

            if(!httpResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Could not get MICROSOFT email. Status: {httpResponse.StatusCode}");
            }

            string data = await httpResponse.Content.ReadAsStringAsync();
            MicrosoftData microsoftMail = JsonConvert.DeserializeObject<MicrosoftData>(data);

            return await Task.FromResult(microsoftMail.Emails.Account);
        }

        public static async Task<string> GetGoogleEmailAsync()
        {
            string googleTokenString = await SecureStorage.GetAsync("GOOGLE");
            string googleToken = JsonConvert.DeserializeObject<GoogleToken>(googleTokenString).AccessToken;

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={googleToken}" );

            if(!httpResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Could not get GOOGLE email. Status: {httpResponse.StatusCode}");
            }

            string data = await httpResponse.Content.ReadAsStringAsync();

            GoogleData googleData = JsonConvert.DeserializeObject<GoogleData>(data);
            return await Task.FromResult(googleData.Email);
        }
    }
}
