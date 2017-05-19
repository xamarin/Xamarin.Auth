using System;

namespace Android.Support.CustomTabs.Chromium.SharedUtilities
{
	/// <summary>
	/// A Callback for when the service is connected or disconnected. Use those callbacks to
	/// handle UI changes when the service is connected or disconnected.
	/// </summary>
	public interface IConnectionCallback
	{
		/// <summary>
		/// Called when the service is connected.
		/// </summary>
		void OnCustomTabsConnected();

		/// <summary>
		/// Called when the service is disconnected.
		/// </summary>
		void OnCustomTabsDisconnected();
	}
}
