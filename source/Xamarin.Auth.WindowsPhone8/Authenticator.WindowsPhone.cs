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
using Xamarin.Utilities;


using AuthenticateUIType = System.Uri;

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

        protected abstract AuthenticateUIType GetPlatformUI();
    }
}

