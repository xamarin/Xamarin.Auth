using System;

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
        public override Uri AccessTokenUri => new Uri("https://api.fitbit.com/oauth2/token");
    }
}
