using System;
using Xamarin.Auth;

namespace AuthExample.OAuth
{
    public static class OAuthAuthenticatorHelper
    {
        private static OAuth2Authenticator oAuth2Authenticator;
        private static OAuth1Authenticator oAuth1Authenticator;

        public static OAuth2Authenticator CreateOAuth2(OAuth2ProviderType socialLoginProvider)
        {
            switch (socialLoginProvider)
            {
                case OAuth2ProviderType.FACEBOOK:
                    oAuth2Authenticator = new OAuth2Authenticator(
                        clientId: FacebookConfiguration.ClientId,
                        scope: FacebookConfiguration.Scope,
                        authorizeUrl: new Uri(FacebookConfiguration.AuthorizeUrl),
                        redirectUrl: new Uri(FacebookConfiguration.RedirectUrl),
                        getUsernameAsync: null,
                        isUsingNativeUI: FacebookConfiguration.IsUsingNativeUI)
                    {
                        AllowCancel = true,
                        ShowErrors = false,
                        ClearCookiesBeforeLogin = true
                    };
                    break;
                case OAuth2ProviderType.GOOGLE:
                    oAuth2Authenticator = new OAuth2Authenticator(
                        clientId: GoogleConfiguration.ClientId,
                        clientSecret: GoogleConfiguration.ClientSecret,
                        scope: GoogleConfiguration.Scope,
                        authorizeUrl: new Uri(GoogleConfiguration.AuthorizeUrl),
                        redirectUrl: new Uri(GoogleConfiguration.RedirectUrl),
                        getUsernameAsync: null,
                        isUsingNativeUI: GoogleConfiguration.IsUsingNativeUI,
                        accessTokenUrl: new Uri(GoogleConfiguration.AcessTokenUrl))
                    {
                        AllowCancel = true,
                        ShowErrors = false,
                        ClearCookiesBeforeLogin = true
                    };
                    break;
                case OAuth2ProviderType.MICROSOFT:
                    oAuth2Authenticator = new OAuth2Authenticator(
                        clientId: MicrosoftConfiguration.ClientId,
                        clientSecret: MicrosoftConfiguration.ClientSecret,
                        scope: MicrosoftConfiguration.Scope,
                        authorizeUrl: new Uri(MicrosoftConfiguration.AuthorizeUrl),
                        redirectUrl: new Uri(MicrosoftConfiguration.RedirectUrl),
                        getUsernameAsync: null,
                        isUsingNativeUI: MicrosoftConfiguration.IsUsingNativeUI,
                        accessTokenUrl: new Uri(MicrosoftConfiguration.AcessTokenUrl))
                    {
                        AllowCancel = true,
                        ShowErrors = false,
                        ClearCookiesBeforeLogin = true
                    };
                    break;
            }

            AuthenticationState = oAuth2Authenticator;
            return oAuth2Authenticator;
        }

        public static OAuth1Authenticator CreateOAuth1(OAuth1ProviderType socialLoginProvider)
        {
            //TODO 
            switch (socialLoginProvider)
            {
                case OAuth1ProviderType.TWITTER:
                    oAuth1Authenticator = new OAuth1Authenticator(
                        consumerKey: "",
                        consumerSecret: "",
                        requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
                        authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
                        accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
                        callbackUrl: new Uri("http://mobile.twitter.com")
                        );
                    break;
            }

            return oAuth1Authenticator;
        }

        public static OAuth2Authenticator AuthenticationState { get; private set; }
    }
}