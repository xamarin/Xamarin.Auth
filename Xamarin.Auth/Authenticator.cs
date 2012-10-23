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
	/// A process and user interface to authenticate a user.
	/// </summary>
	public abstract class Authenticator
	{
		/// <summary>
		/// Title of any UI elements that need to be presented for this authenticator.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Occurs when authentication has been successfully or unsuccessfully completed.
		/// Consult the <see cref="AuthenticatorCompletedEventArgs.IsAuthenticated"/> event argument to determine if
		/// authentication was successful.
		/// </summary>
		public event EventHandler<AuthenticatorCompletedEventArgs> Completed;

		/// <summary>
		/// Occurs when there an error is encountered when authenticating.
		/// </summary>
		public event EventHandler<AuthenticatorErrorEventArgs> Error;

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Social.Authenticator"/> class.
		/// </summary>
		public Authenticator ()
		{
			Title = "Authenticate";
		}

#if PLATFORM_ANDROID
		UIContext context;
		public AuthenticateUIType GetUI (UIContext context)
		{
			this.context = context;
			return GetPlatformUI (context);
		}
		protected abstract AuthenticateUIType GetPlatformUI (UIContext context);
#else
		public AuthenticateUIType GetUI ()
		{
			return GetPlatformUI ();
		}
		protected abstract AuthenticateUIType GetPlatformUI ();
#endif

		/// <summary>
		/// Implementations must call this function when they have successfully authenticated.
		/// </summary>
		/// <param name='account'>
		/// The authenticated account.
		/// </param>
		public void OnSucceeded (Account account)
		{
			BeginInvokeOnUIThread (delegate {
				var ev = Completed;
				if (ev != null) {
					ev (this, new AuthenticatorCompletedEventArgs (account));
				}
			});
		}

		/// <summary>
		/// Implementations must call this function when they have successfully authenticated.
		/// </summary>
		/// <param name='username'>
		/// User name of the account.
		/// </param>
		/// <param name='accountProperties'>
		/// Additional data, such as access tokens, that need to be stored with the account. This
		/// information is secured.
		/// </param>
		public void OnSucceeded (string username, IDictionary<string, string> accountProperties)
		{
			OnSucceeded (new Account (username, accountProperties));
		}

		/// <summary>
		/// Implementations must call this function when they have cancelled the operation.
		/// </summary>
		public void OnCancelled ()
		{
			BeginInvokeOnUIThread (delegate {
				var ev = Completed;
				if (ev != null) {
					ev (this, new AuthenticatorCompletedEventArgs (null));
				}
			});
		}

		/// <summary>
		/// Implementations must call this function when they have failed to authenticate.
		/// </summary>
		/// <param name='message'>
		/// The reason that this authentication has failed.
		/// </param>
		public void OnError (string message)
		{
			BeginInvokeOnUIThread (delegate {
				var ev = Error;
				if (ev != null) {
					ev (this, new AuthenticatorErrorEventArgs (message));
				}
			});
		}

		/// <summary>
		/// Implementations must call this function when they have failed to authenticate.
		/// </summary>
		/// <param name='exception'>
		/// The reason that this authentication has failed.
		/// </param>
		public void OnError (Exception exception)
		{
			BeginInvokeOnUIThread (delegate {
				var ev = Error;
				if (ev != null) {
					ev (this, new AuthenticatorErrorEventArgs (exception));
				}
			});
		}

		void BeginInvokeOnUIThread (Action action)
		{
#if PLATFORM_IOS
			MonoTouch.UIKit.UIApplication.SharedApplication.BeginInvokeOnMainThread (delegate { action (); });
#elif PLATFORM_ANDROID
			var a = context as Android.App.Activity;
			if (a != null) {
				a.RunOnUiThread (action);
			}
			else {
				action ();
			}
#else
			action ();
#endif
		}
	}

	public class AuthenticatorCompletedEventArgs : EventArgs
	{
		public bool IsAuthenticated { get { return Account != null; } }
		public Account Account { get; private set; }

		public AuthenticatorCompletedEventArgs (Account account)
		{
			Account = account;
		}
	}

	public class AuthenticatorErrorEventArgs : EventArgs
	{
		public string Message { get; private set; }
		public Exception Exception { get; private set; }

		public AuthenticatorErrorEventArgs (string message)
		{
			Message = message;
		}

		public AuthenticatorErrorEventArgs (Exception exception)
		{
			Message = exception.GetUserMessage ();
			Exception = exception;
		}
	}
}

