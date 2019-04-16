using System;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
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
