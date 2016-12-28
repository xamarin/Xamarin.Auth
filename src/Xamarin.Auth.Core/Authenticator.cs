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

namespace Xamarin.Auth
{
	/// <summary>
	/// A process and user interface to authenticate a user.
	/// </summary>
	public abstract class Authenticator
	{
		/// <summary>
		/// Gets or sets the title of any UI elements that need to be presented for this authenticator.
		/// </summary>
		/// <value><c>"Authenticate" by default.</c></value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets whether to allow user cancellation.
		/// </summary>
		/// <value><c>true</c> by default.</value>
		public bool AllowCancel { get; set; }

		/// <summary>
		/// Occurs when authentication has been successfully or unsuccessfully completed.
		/// Consult the <see cref="AuthenticatorCompletedEventArgs.IsAuthenticated"/> event argument to determine if
		/// authentication was successful.
		/// </summary>
		public event EventHandler<AuthenticatorCompletedEventArgs> Completed;
        
        public event EventHandler<EventArgs> Completing;

        /// <summary>
        /// Occurs when there an error is encountered when authenticating.
        /// </summary>
        public event EventHandler<AuthenticatorErrorEventArgs> Error;

		/// <summary>
		/// Gets whether this authenticator has completed its interaction with the user.
		/// </summary>
		/// <value><c>true</c> if authorization has been completed, <c>false</c> otherwise.</value>
		public bool HasCompleted { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.Authenticator"/> class.
		/// </summary>
		public Authenticator ()
		{
			Title = "Authenticate";
			HasCompleted = false;
			AllowCancel = true;
		}

	    public Action<Action> MainThreadInvoker { get; set; }


		/// <summary>
		/// Implementations must call this function when they have successfully authenticated.
		/// </summary>
		/// <param name='account'>
		/// The authenticated account.
		/// </param>
		public void OnSucceeded (Account account)
		{
			if (HasCompleted)
				return;

			HasCompleted = true;

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

        public void OnSucceeding()
        {
            BeginInvokeOnUIThread(delegate {
                var ev = Completing;
                if (ev != null)
                {
                    ev(this, EventArgs.Empty);
                }
            });
        }

        /// <summary>
        /// Implementations must call this function when they have cancelled the operation.
        /// </summary>
        public void OnCancelled ()
		{
			if (HasCompleted)
				return;

			HasCompleted = true;

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
		    if (MainThreadInvoker != null)
		        MainThreadInvoker(action);
            else
		        Platform.Engine.InvokeOnMainThread(action);
		}
	}

	/// <summary>
	/// Authenticator completed event arguments.
	/// </summary>
	public class AuthenticatorCompletedEventArgs : EventArgs
	{
		/// <summary>
		/// Whether the authentication succeeded and there is a valid <see cref="Account"/>.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if the user is authenticated; otherwise, <see langword="false"/>.
		/// </value>
		public bool IsAuthenticated { get { return Account != null; } }

		/// <summary>
		/// Gets the account created that represents this authentication.
		/// </summary>
		/// <value>
		/// The account.
		/// </value>
		public Account Account { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.AuthenticatorCompletedEventArgs"/> class.
		/// </summary>
		/// <param name='account'>
		/// The account created or <see langword="null"/> if authentication failed or was canceled.
		/// </param>
		public AuthenticatorCompletedEventArgs (Account account)
		{
			Account = account;
		}
	}

	/// <summary>
	/// Authenticator error event arguments.
	/// </summary>
	public class AuthenticatorErrorEventArgs : EventArgs
	{
		/// <summary>
		/// Gets a message describing the error.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message { get; private set; }

		/// <summary>
		/// Gets the exception that signaled the error if there was one.
		/// </summary>
		/// <value>
		/// The exception or <see langword="null"/>.
		/// </value>
		public Exception Exception { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.AuthenticatorErrorEventArgs"/> class
		/// with a message but no exception.
		/// </summary>
		/// <param name='message'>
		/// A message describing the error.
		/// </param>
		public AuthenticatorErrorEventArgs (string message)
		{
			Message = message;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.AuthenticatorErrorEventArgs"/> class with an exception.
		/// </summary>
		/// <param name='exception'>
		/// The exception signaling the error. The message of this object is retrieved from this exception or
		/// its inner exceptions.
		/// </param>
		public AuthenticatorErrorEventArgs (Exception exception)
		{
			Message = exception.GetUserMessage ();
			Exception = exception;
		}
	}
}

