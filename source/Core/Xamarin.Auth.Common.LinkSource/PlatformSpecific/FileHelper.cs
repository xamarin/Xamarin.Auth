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
            string libFolder = null;

            if (!System.IO.Directory.Exists(libFolder))
            {
                System.IO.Directory.CreateDirectory(libFolder);
            }

            return libFolder;
        }
    }
}
