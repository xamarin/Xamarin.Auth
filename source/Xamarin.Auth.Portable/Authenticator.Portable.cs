//
//  Copyright 2012-2016, Xamarin Inc.
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

#if __ANDROID__
using AuthenticateUIType = Android.Content.Intent;
#elif __IOS__ && __UNIFIED__
using AuthenticateUIType = UIKit.UIViewController;
#elif __IOS__ && ! __UNIFIED__
using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
#elif WINDOWS_PHONE && SILVERLIGHT
using AuthenticateUIType = System.Uri;
#elif PORTABLE
using AuthenticateUIType = System.Object;
#endif

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that displays a web page.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal abstract partial class WebAuthenticator
#else
	public abstract partial class Authenticator 
#endif
	{
		/// <summary>
		/// Clears all cookies.
		/// </summary>
		/// <seealso cref="ClearCookiesBeforeLogin"/>
		public async static void ClearCookies()
		{
            throw new NotImplementedException(LibraryUtilities.MessageNotImplementedException);
        }

        protected virtual AuthenticateUIType GetPlatformUI()
		{
            throw new NotImplementedException(LibraryUtilities.MessageNotImplementedException);
        }
	}
}

