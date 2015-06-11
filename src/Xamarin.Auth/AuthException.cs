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

namespace Xamarin.Auth
{
	/// <summary>
	/// An exception generated by the Xamarin.Auth library.
	/// </summary>
#if !PLATFORM_WINPHONE
	[Serializable]
#endif
#if XAMARIN_AUTH_INTERNAL
	internal class AuthException : Exception
#else
	public class AuthException : Exception
#endif
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthException"/> class.
		/// </summary>
		public AuthException ()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthException"/> class.
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public AuthException (string message) : base (message)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthException"/> class.
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public AuthException (string message, Exception inner) : base (message, inner)
		{
		}
		
#if !PLATFORM_WINPHONE	
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthException"/> class.
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected AuthException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
#endif
	}
}

