//
//  Copyright 2012-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

//--------------------------------------------------------------------
//	Original defines
//		usings are in WebAuthenticator.<Platform>.cs
//
//#if PLATFORM_IOS
//using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
//#elif PLATFORM_ANDROID
//using AuthenticateUIType = Android.Content.Intent;
//using UIContext = Android.Content.Context;
//#elif PLATFORM_WINPHONE
//using Microsoft.Phone.Shell;
//using AuthenticateUIType = System.Uri;
//#else
//using AuthenticateUIType = System.Object;
//#endif
//--------------------------------------------------------------------

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that displays a web page.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal abstract partial class WebAuthenticator : Authenticator
#else
    public abstract partial class WebAuthenticator : Authenticator
#endif
	{
		/// <summary>
		/// Gets or sets whether to automatically clear cookies before logging in.
		/// </summary>
		/// <seealso cref="ClearCookies"/>
		public bool ClearCookiesBeforeLogin
		{
			get { return this.clearCookies; }
			set { this.clearCookies = value; }
		}

		/// <summary>
		/// Method that returns the initial URL to be displayed in the web browser.
		/// </summary>
		/// <returns>
		/// A task that will return the initial URL.
		/// </returns>
		public abstract Task<Uri> GetInitialUrlAsync ();

		/// <summary>
		/// Event handler called when a new page is being loaded in the web browser.
		/// </summary>
		/// <param name='url'>
		/// The URL of the page.
		/// </param>
		public virtual void OnPageLoading (Uri url)
		{
		}

		/// <summary>
		/// Event handler called when a new page has been loaded in the web browser.
		/// Implementations should call <see cref="Authenticator.OnSucceeded(Xamarin.Auth.Account)"/> if this page
		/// signifies a successful authentication.
		/// </summary>
		/// <param name='url'>
		/// The URL of the page.
		/// </param>
		public abstract void OnPageLoaded (Uri url);

		/// <summary>
		/// Clears all cookies.
		/// </summary>
		/// <seealso cref="ClearCookiesBeforeLogin"/>
		//public static void ClearCookies()
		//{
//#if PLATFORM_IOS
			//var store = MonoTouch.Foundation.NSHttpCookieStorage.SharedStorage;
			//var cookies = store.Cookies;
			//foreach (var c in cookies) {
			//	store.DeleteCookie (c);
			//}
//#elif PLATFORM_ANDROID
			//Android.Webkit.CookieSyncManager.CreateInstance (Android.App.Application.Context);
			//Android.Webkit.CookieManager.Instance.RemoveAllCookie ();
//#endif
		//}

		/// <summary>
		/// Occurs when the visual, user-interactive, browsing has completed but there
		/// is more authentication work to do.
		/// </summary>
		public event EventHandler BrowsingCompleted;

		private bool clearCookies = true;

		/// <summary>
		/// Raises the browsing completed event.
		/// </summary>
		protected virtual void OnBrowsingCompleted ()
		{
			var ev = BrowsingCompleted;
			if (ev != null) {
				ev (this, EventArgs.Empty);
			}
		}

#if PLATFORM_IOS
		/// <summary>
		/// Gets the UI for this authenticator.
		/// </summary>
		/// <returns>
		/// The UI that needs to be presented.
		/// </returns>
		//protected override AuthenticateUIType GetPlatformUI ()
		//{
		//	return new MonoTouch.UIKit.UINavigationController (new WebAuthenticatorController (this));
		//}
#elif PLATFORM_ANDROID
		/// <summary>
		/// Gets the UI for this authenticator.
		/// </summary>
		/// <returns>
		/// The UI that needs to be presented.
		/// </returns>
		//protected override AuthenticateUIType GetPlatformUI (UIContext context)
		//{
		//	var i = new global::Android.Content.Intent (context, typeof (WebAuthenticatorActivity));
		//	i.PutExtra ("ClearCookies", ClearCookiesBeforeLogin);
		//	var state = new WebAuthenticatorActivity.State {
		//		Authenticator = this,
		//	};
		//	i.PutExtra ("StateKey", WebAuthenticatorActivity.StateRepo.Add (state));
		//	return i;
		//}
#elif PLATFORM_WINPHONE
		//protected override AuthenticateUIType GetPlatformUI()
		//{
		//	Random r = new Random();
		//	string key;
		//	do {
		//		key = "xamarin_auth_" + r.Next();
		//	} while (PhoneApplicationService.Current.State.ContainsKey (key));
		//
		//	PhoneApplicationService.Current.State[key] = this;
		//
		//	
		//	System.Reflection.Assembly assembly = typeof(Authenticator).Assembly;
		//	string assembly_name = assembly.GetName().Name;
		//	return new Uri ("/" + assembly_name + ";component/WebAuthenticatorPage.xaml?key=" + key, UriKind.Relative);
		//}
#else
        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        //protected override AuthenticateUIType GetPlatformUI ()
        //{
        //	throw new NotSupportedException ("WebAuthenticator not supported on this platform.");
        //}
#endif

    }
}

