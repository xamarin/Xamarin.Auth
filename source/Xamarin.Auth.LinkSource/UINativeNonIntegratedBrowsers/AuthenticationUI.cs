using System;

namespace Xamarin.Auth
{
    public class AuthenticationUI
    {
        public static AuthenticationUIType AuthenticationUIType 
        { 
            get; 
            set; 
        } = AuthenticationUIType.EmbeddedBrowser;
    }
}
