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
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Xamarin.Utilities;

using AuthenticateUIType = System.Object;

namespace Xamarin.Auth
{
#if XAMARIN_AUTH_INTERNAL
	internal class partial OAuth1Authenticator : WebAuthenticator
#else
	public partial class OAuth1Authenticator : WebAuthenticator
#endif
	{
		/// <summary>
		/// Gets the UI for this authenticator.
		/// </summary>
		/// <returns>
		/// The UI that needs to be presented.
		/// </returns>
		protected override AuthenticateUIType GetPlatformUI ()
		{
			throw new NotImplementedException();
		}
	}
}

