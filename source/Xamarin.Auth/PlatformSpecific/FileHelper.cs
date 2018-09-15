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
#if __ANDROID__
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#elif __IOS__
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = System.IO.Path.Combine(docFolder, "..", "Library", "Databases");

            if (!System.IO.Directory.Exists(libFolder))
            {
                System.IO.Directory.CreateDirectory(libFolder);
            }

            return libFolder;
#elif WINDOWS_UWP
            return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
            throw new PlatformNotSupportedException();
#endif
        }
    }
}
