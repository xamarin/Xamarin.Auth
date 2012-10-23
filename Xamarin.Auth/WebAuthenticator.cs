//
//  Copyright 2012, Xamarin Inc.
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

#if PLATFORM_IOS
using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
#elif PLATFORM_ANDROID
using AuthenticateUIType = Android.Content.Intent;
using UIContext = Android.Content.Context;
#else
using AuthenticateUIType = System.Object;
#endif

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that displays a web page.
	/// </summary>
	public abstract class WebAuthenticator : Authenticator
	{
		public abstract Task<Uri> GetInitialUrlAsync ();

		public abstract void OnPageLoaded (Uri url);

#if PLATFORM_IOS
		protected override AuthenticateUIType GetPlatformUI ()
		{
			return new MonoTouch.UIKit.UINavigationController (new WebAuthenticatorController (this));
		}
#elif PLATFORM_ANDROID
		protected override AuthenticateUIType GetPlatformUI (UIContext context)
		{
			var i = new global::Android.Content.Intent (context, typeof (WebAuthenticatorActivity));
			var state = new WebAuthenticatorActivity.State {
				Authenticator = this,
			};
			i.PutExtra ("StateKey", WebAuthenticatorActivity.StateRepo.Add (state));
			return i;
		}
#else
		protected override AuthenticateUIType GetPlatformUI ()
		{
			throw new NotSupportedException ("WebAuthenticator not supported on this platform.");
		}
#endif
	}
}

