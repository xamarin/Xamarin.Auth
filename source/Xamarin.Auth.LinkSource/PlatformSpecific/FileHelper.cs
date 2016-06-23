using System;

namespace Diary.iOS
{
	public static class FileHelper
	{
		public static string GetLocalStoragePath()
		{
			string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libFolder = System.IO.Path.Combine(docFolder, "..", "Library", "Databases");

			if (!System.IO.Directory.Exists(libFolder))
			{
				System.IO.Directory.CreateDirectory(libFolder);
			}

			return libFolder;
		}
	}
}