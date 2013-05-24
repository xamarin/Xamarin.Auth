using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
	public interface IExternalUrlManager
	{
		/// <summary>
		/// Implement this method to open a URL and wait for the app to go foreground with a callback URL.
		/// </summary>
		Task<Uri> OpenUrl (Uri url, string callbackScheme);
	}
}