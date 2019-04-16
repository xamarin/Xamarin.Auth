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
using Xamarin.Utilities;

//--------------------------------------------------------------------
//	Original defines
//		usings are in Authenticator.<Platform>.cs
//
//#if PLATFORM_IOS
//using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
//#elif PLATFORM_ANDROID
//using AuthenticateUIType = Android.Content.Intent;
//using UIContext = Android.Content.Context;
//#elif PLATFORM_WINPHONE
//using AuthenticateUIType = System.Uri;
//#else
using AuthenticateUIType = System.Object;
//#endif
//--------------------------------------------------------------------

namespace Xamarin.Auth
{
    /// <summary>
    /// A process and user interface to authenticate a user.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
	internal abstract partial class Authenticator
    #else
    public abstract partial class Authenticator
    #endif
    {
        public AuthenticateUIType GetUI()
        {
            return GetPlatformUI();
        }

        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        protected abstract AuthenticateUIType GetPlatformUI();

    }
}

