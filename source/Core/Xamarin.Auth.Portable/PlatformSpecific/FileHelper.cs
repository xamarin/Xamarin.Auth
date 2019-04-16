using System;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    public static class FileHelper
	{
		public static string GetLocalStoragePath()
        {
            throw new NotImplementedException("Portable");

            #pragma warning disable 0162
            return string.Empty;
		}
	}
}