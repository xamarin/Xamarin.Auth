using System;
namespace AuthExample.OAuth
{
    public static class MicrosoftConfiguration
    {
        public static readonly string ClientId = "<your client id>";
        public static readonly string Scope = "wl.offline_access wl.signin wl.basic wl.emails";//"openid email https://graph.microsoft.com/user.read";
        public static readonly string ClientSecret = "<your client secret>";
        public static readonly string AuthorizeUrl = "https://login.live.com/oauth20_authorize.srf";//"https://login.microsoftonline.com/common/oauth2/V2.0/authorize";
        public static readonly string RedirectUrl = "<your redirect url>";
        public static readonly string AcessTokenUrl = "https://login.live.com/oauth20_token.srf";//"https://login.live.com/adfs/oauth2/token";
        public static bool IsUsingNativeUI = false;
    }
}
