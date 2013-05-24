using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Xamarin.Auth
{
#if PLATFORM_IOS
	/// <summary>
	/// Implements external URL manager for iOS.
	/// For this class to work, you need to call its HandleOpenUrl and WillEnterForeground from respective AppDelegate methods.
	/// </summary>
	public class HandleOpenUrlManager : IExternalUrlManager
	{
		static readonly Lazy<HandleOpenUrlManager> instance = new Lazy<HandleOpenUrlManager> (() => new HandleOpenUrlManager ());

		public static HandleOpenUrlManager Instance {
			get { return instance.Value; }
		}


		TaskCompletionSource<Uri> tcs;
		string callbackScheme;

		private HandleOpenUrlManager ()
		{ 
		}

		/// <summary>
		/// Call this method from AppDelegate.HandleOpenUrl.
		/// </summary>
		public bool HandleOpenUrl (Uri url)
		{
			if (url.Scheme != callbackScheme)
				return false;

			if (this.tcs != null)
				this.tcs.TrySetResult (url);

			return true;
		}

		/// <summary>
		/// Call this method from AppDelegate.WillEnterForeground.
		/// </summary>
		public void WillEnterForeground ()
		{
			UIApplication.SharedApplication.BeginInvokeOnMainThread (() => {
				// By the time we get here, if there has been no HandleOpenUrl call, consider it canceled
				ClearPreviousCallback ();
			});
		}

		public Task<Uri> OpenUrl (Uri url, string callbackScheme)
		{
			var tcs = new TaskCompletionSource<Uri> ();

			UIApplication.SharedApplication.BeginInvokeOnMainThread (() => {
				ClearPreviousCallback ();

				this.tcs = tcs;
				this.callbackScheme = callbackScheme;

				UIApplication.SharedApplication.OpenUrl (new NSUrl (url.ToString ()));
			});

			return tcs.Task;
		}

		void ClearPreviousCallback ()
		{
			if (this.tcs != null)
				this.tcs.TrySetCanceled ();

			this.tcs = null;
			this.callbackScheme = null;
		}
	}
#endif
}

