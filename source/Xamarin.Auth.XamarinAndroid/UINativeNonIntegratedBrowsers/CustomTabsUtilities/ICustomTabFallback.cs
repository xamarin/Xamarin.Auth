using System;

using Android.App;

namespace Android.Support.CustomTabs.Chromium.SharedUtilities
{
	/// <summary>
	/// To be used as a fallback to open the Uri when Custom Tabs is not available.
	/// </summary>
	public interface ICustomTabFallback
	{
		/// 
		/// <param name="activity"> The Activity that wants to open the Uri. </param>
		/// <param name="uri"> The uri to be opened by the fallback. </param>
		void OpenUri(Activity activity, Android.Net.Uri uri);
	}
}
