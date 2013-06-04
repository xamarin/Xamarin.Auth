//
//  Copyright 2013, Stampsy Ltd.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Xamarin.Auth
{
	/// <summary>
	/// Implements custom URL handler for iOS.
	/// Call <see cref="Instance"/>'s <see cref="HandleOpenUrl"/> and <see cref="WillEnterForeground"/> methods from respective <c>AppDelegate</c> methods in your iOS application.
	/// </summary>
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class SafariUrlHandler : ICustomUrlHandler
#else
	public class SafariUrlHandler : ICustomUrlHandler
#endif
	{
		static readonly Lazy<SafariUrlHandler> instance = new Lazy<SafariUrlHandler> (() => new SafariUrlHandler ());

		public static SafariUrlHandler Instance {
			get { return instance.Value; }
		}


		TaskCompletionSource<Uri> tcs;
		string callbackScheme;

		private SafariUrlHandler ()
		{
		}

		/// <summary>
		/// Opens <paramref name="url"/> in Safari and waits for user to return to the app.
		/// 
		/// If the user returns to the app by opening custom URL that starts from <paramref name="callbackScheme"/> from browser,
		/// this custom URL will be the result of the task.
		/// 
		/// If custom URL doesn't match <paramref name="callbackScheme"/>, or if the app went foreground
		/// without receiving a custom URL, the task will be cancelled.
		/// </summary>
		/// <remarks>
		/// You need to call <see cref="HandleOpenUrl"/> and <see cref="WillEnterForeground"/> on this instance
		/// from your <c>AppDelegate</c>'s respective methods.
		/// </remarks>
		public Task<Uri> OpenUrl (Uri url, string callbackScheme)
		{
			var tcs = new TaskCompletionSource<Uri> ();

			UIApplication.SharedApplication.BeginInvokeOnMainThread (() => {
				CancelTask ();

				this.tcs = tcs;
				this.callbackScheme = callbackScheme;

				UIApplication.SharedApplication.OpenUrl (new NSUrl (url.ToString ()));
			});

			return tcs.Task;
		}

		void CancelTask ()
		{
			if (this.tcs != null)
				this.tcs.TrySetCanceled ();

			this.tcs = null;
			this.callbackScheme = null;
		}

		/// <summary>
		/// This method needs to be called from <c>AppDelegate.HandleOpenUrl</c>.
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
		/// This method needs to be called from <c>AppDelegate.WillEnterForeground.</c>
		/// </summary>
		public void WillEnterForeground ()
		{
			UIApplication.SharedApplication.BeginInvokeOnMainThread (() => {
				// By the time we get here, if there has been no HandleOpenUrl call, consider it cancelled
				CancelTask ();
			});
		}
	}
}