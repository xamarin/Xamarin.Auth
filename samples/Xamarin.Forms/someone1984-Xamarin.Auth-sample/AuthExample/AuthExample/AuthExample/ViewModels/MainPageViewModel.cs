using AuthExample.OAuth;
using AuthExample.OAuth.Tokens;
using Newtonsoft.Json;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AuthExample.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        string userEmail;
        string providerName;
        OAuth2ProviderType provider;
        string providerToken;

        public string UserEmail
        {
            get
            {
                return userEmail;
            }

            set
            {
                SetProperty(ref userEmail, value);
            }
        }

        public string ProviderName
        {
            get
            {
                return providerName;
            }
            set
            {
                SetProperty(ref providerName, value);
            }
        }

        public OAuth2ProviderType OAuth2ProviderType
        {
            get
            {
                return provider;
            }
            set
            {
                SetProperty(ref provider, value);
            }
        }

        public string ProviderToken
        {
            get
            {
                return providerToken;
            }
            set
            {
                SetProperty(ref providerToken, value);
            }
        }

        public GoogleToken GoogleCredentials { get; set; }
        public FacebookToken FacebookCredentials { get; set; }
        public MicrosoftToken MicrosoftCredentials { get; set; }

        public Command OnPageAppearingCommand => new Command(ExecuteOnPageAppearing);

        public async void ExecuteOnPageAppearing()
        {
            UserEmail = await SecureStorage.GetAsync("Email");
            string socialProvider = await SecureStorage.GetAsync("Provider");

            OAuth2ProviderType = (OAuth2ProviderType)Enum.Parse(typeof(OAuth2ProviderType), socialProvider);

            var token = string.Empty;

            switch (OAuth2ProviderType)
            {
                case OAuth2ProviderType.NONE:
                    break;
                case OAuth2ProviderType.TRADITIONAL:
                    break;
                case OAuth2ProviderType.FACEBOOK:
                    token = await SecureStorage.GetAsync(OAuth2ProviderType.ToString());
                    FacebookCredentials = JsonConvert.DeserializeObject<FacebookToken>(token);
                    ProviderToken = FacebookCredentials.AccessToken;
                    ProviderName = OAuth2ProviderType.FACEBOOK.ToString();
                    break;
                case OAuth2ProviderType.GOOGLE:
                    token = await SecureStorage.GetAsync(OAuth2ProviderType.ToString());
                    GoogleCredentials = JsonConvert.DeserializeObject<GoogleToken>(token);
                    ProviderToken = GoogleCredentials.AccessToken;
                    ProviderName = OAuth2ProviderType.GOOGLE.ToString();
                    break;
                case OAuth2ProviderType.MICROSOFT:
                    token = await SecureStorage.GetAsync(OAuth2ProviderType.ToString());
                    MicrosoftCredentials = JsonConvert.DeserializeObject<MicrosoftToken>(token);
                    ProviderToken = MicrosoftCredentials.AccessToken;
                    ProviderName = OAuth2ProviderType.MICROSOFT.ToString();
                    break;

            }

        }
    }
}
