using System;
namespace AuthExample.OAuth
{
    public static class GoogleConfiguration
    {
        public static readonly string ClientId = "<your client id>";
        public static readonly string Scope = "email";
        public static readonly string ClientSecret = "";
        public static readonly string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static readonly string RedirectUrl = "<your redirect url>";
        public static readonly string AcessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static bool IsUsingNativeUI = true;
    }
}
