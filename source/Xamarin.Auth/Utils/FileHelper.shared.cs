using System;
using System.IO;

namespace Xamarin.Auth
{
    internal static class FileHelper
    {
        public static string GetLocalStoragePath()
        {
#if __ANDROID__
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#elif __IOS__
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
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
