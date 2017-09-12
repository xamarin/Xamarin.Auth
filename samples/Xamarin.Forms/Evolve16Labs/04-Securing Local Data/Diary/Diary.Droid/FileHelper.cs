using System;

namespace Diary.Droid
{
	public static class FileHelper
	{
		public static string GetLocalStoragePath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}
	}
}