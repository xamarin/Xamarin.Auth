using System;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    public enum AuthenticationUIType
    {
        EmbeddedBrowser,
        Native
    }
}
