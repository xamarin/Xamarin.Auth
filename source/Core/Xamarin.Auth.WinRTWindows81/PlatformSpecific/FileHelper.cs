using System;

namespace Diary.WindowsPhone8Silverlight
{
    public static class FileHelper
    {
        public static string GetLocalStoragePath()
        {
            return
                //Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                Windows.Storage.ApplicationData.Current.LocalFolder.Path
                ;
        }
    }
}