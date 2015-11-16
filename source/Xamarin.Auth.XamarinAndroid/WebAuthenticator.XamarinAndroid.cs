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

using AuthenticateUIType = Android.Content.Intent;
using UIContext = Android.Content.Context;

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that displays a web page.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal abstract partial class WebAuthenticator
#else
	public abstract partial class WebAuthenticator 
#endif
	{
		/// <summary>
		/// Clears all cookies.
		/// </summary>
		/// <seealso cref="ClearCookiesBeforeLogin"/>
		public static void ClearCookies()
		{
			Android.Webkit.CookieSyncManager.CreateInstance (Android.App.Application.Context);
			Android.Webkit.CookieManager.Instance.RemoveAllCookie ();
		}


		/// <summary>
		/// Gets the UI for this authenticator.
		/// </summary>
		/// <returns>
		/// The UI that needs to be presented.
		/// </returns>
		protected override AuthenticateUIType GetPlatformUI(UIContext context)
		{
			var i = new global::Android.Content.Intent(context, typeof(WebAuthenticatorActivity));
			i.PutExtra("ClearCookies", ClearCookiesBeforeLogin);
			var state = new WebAuthenticatorActivity.State
			{
				Authenticator = this,
			};
			i.PutExtra("StateKey", WebAuthenticatorActivity.StateRepo.Add(state));
			return i;
		}
	}
}

