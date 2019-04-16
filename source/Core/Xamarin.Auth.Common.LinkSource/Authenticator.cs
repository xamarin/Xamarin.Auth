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

using System.Linq;

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
//using AuthenticateUIType = System.Object;
//#endif
//--------------------------------------------------------------------

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
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
        /// Gets or sets whether Xamarin.Auth should display login errors.
        /// </summary>
        /// <value><c>true</c> by default.</value>
        public bool ShowErrors { get; set; }

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
        /// Gets whether this authenticator has completed its interaction with the user.
        /// </summary>
        /// <value><c>true</c> if authorization has been completed, <c>false</c> otherwise.</value>
        public bool HasCompleted { get; private set; }


#region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		IgnoreErrorsWhenCompleted #58
        ///		https://github.com/xamarin/Xamarin.Auth/pull/58
        /// <summary>
        /// Controls the behavior of the <see cref="OnError"/> method when <see cref="HasCompleted"/> 
        ///	is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> to raise the <see cref="Error" event/> , <c>false</c> to do nothing.</value>
        protected bool IgnoreErrorsWhenCompleted { get; set; }
        //---------------------------------------------------------------------------------------
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Auth.Authenticator"/> class.
        /// </summary>
        public Authenticator()
        {
            Title = "Authenticate";
            HasCompleted = false;
            AllowCancel = true;
            ShowErrors = true;
            #region
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		IgnoreErrorsWhenCompleted #58
            ///		https://github.com/xamarin/Xamarin.Auth/pull/58
            IgnoreErrorsWhenCompleted = false;
            //---------------------------------------------------------------------------------------
            #endregion

            #region
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		Added IsAuthenticated check #88
            ///		https://github.com/xamarin/Xamarin.Auth/pull/88
            ///		
            IsAuthenticated = () => false;
            //---------------------------------------------------------------------------------------
            #endregion

            return;
        }

        #if __ANDROID__
		//UIContext context;
		//public AuthenticateUIType GetUI (UIContext context)
		//{
		//	this.context = context;
		//	return GetPlatformUI (context);
		//}
		//protected abstract AuthenticateUIType GetPlatformUI (UIContext context);
        #else
        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        //public AuthenticateUIType GetUI ()
        //{
        //	return GetPlatformUI ();
        //}

        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        //protected abstract AuthenticateUIType GetPlatformUI ();
        #endif

        /// <summary>
        /// Implementations must call this function when they have successfully authenticated.
        /// </summary>
        /// <param name='account'>
        /// The authenticated account.
        /// </param>
        public void OnSucceeded(Account account)
        {
            string msg = null;

            #if DEBUG
            string d = string.Join("  ;  ", account.Properties.Select(x => x.Key + "=" + x.Value));
            msg = String.Format("Authenticator.OnSucceded {0}", d);
            System.Diagnostics.Debug.WriteLine(msg);
            #endif

            if (HasCompleted)
            {
                return;
            }

            HasCompleted = true;

            #if !__ANDROID__
            Plugin.Threading.UIThreadRunInvoker ri = new Plugin.Threading.UIThreadRunInvoker();
            #else
            Plugin.Threading.UIThreadRunInvoker ri = new Plugin.Threading.UIThreadRunInvoker(this.context);
            #endif
            ri.BeginInvokeOnUIThread
            (
                delegate
                {
                    var ev = Completed;
                    if (ev != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Authenticator.OnSucceded Completed Begin");
                        ev(this, new AuthenticatorCompletedEventArgs(account));
                        System.Diagnostics.Debug.WriteLine("Authenticator.OnSucceded Completed End");
                    }
                    else
                    {
                        msg = "No subscribers to Xamarin.Auth.Authenticator.Completed (OnCompleted) event";
                        System.Diagnostics.Debug.WriteLine(msg);
                    }
                }
            );

            return;
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
        public void OnSucceeded(string username, IDictionary<string, string> accountProperties)
        {
            OnSucceeded(new Account(username, accountProperties));

            return;
        }

        /// <summary>
        /// Implementations must call this function when they have cancelled the operation.
        /// </summary>
        public void OnCancelled()
        {
            if (HasCompleted)
                return;

            HasCompleted = true;

            #if !__ANDROID__
            new Plugin.Threading.UIThreadRunInvoker().BeginInvokeOnUIThread
            #else
            new Plugin.Threading.UIThreadRunInvoker(this.context).BeginInvokeOnUIThread
            #endif
                (
                    delegate
                    {
                        var ev = Completed;
                        if (ev != null)
                        {
                            ev(this, new AuthenticatorCompletedEventArgs(null));
                        }
                    }
                );
        }

        /// <summary>
        /// Implementations must call this function when they have failed to authenticate.
        /// </summary>
        /// <param name='message'>
        /// The reason that this authentication has failed.
        /// </param>
        public void OnError(string message)
        {
            #if !__ANDROID__
            new Plugin.Threading.UIThreadRunInvoker().BeginInvokeOnUIThread
            #else
            new Plugin.Threading.UIThreadRunInvoker(this.context).BeginInvokeOnUIThread
            #endif
                (
                    delegate 
                    {
				        var ev = Error;
				        if (ev != null)
                        {
					        ev (this, new AuthenticatorErrorEventArgs (message));
				        }
			        }
                );
            RaiseErrorEvent(new AuthenticatorErrorEventArgs(message));

            return;
        }

        /// <summary>
        /// Implementations must call this function when they have failed to authenticate.
        /// </summary>
        /// <param name='exception'>
        /// The reason that this authentication has failed.
        /// </param>
        public void OnError(Exception exception)
        {
            #region
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		IgnoreErrorsWhenCompleted #58
            ///		https://github.com/xamarin/Xamarin.Auth/pull/58
            /*
			BeginInvokeOnUIThread (delegate {
				var ev = Error;
				if (ev != null) {
					ev (this, new AuthenticatorErrorEventArgs (exception));
				}
			});
			*/
            RaiseErrorEvent(new AuthenticatorErrorEventArgs(exception));
            //---------------------------------------------------------------------------------------
            #endregion

            return;
        }

        #region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		IgnoreErrorsWhenCompleted #58
        ///		https://github.com/xamarin/Xamarin.Auth/pull/58
        void RaiseErrorEvent(AuthenticatorErrorEventArgs args)
        {
            if (!(HasCompleted && IgnoreErrorsWhenCompleted))
            {
                #if !__ANDROID__
                new Plugin.Threading.UIThreadRunInvoker().BeginInvokeOnUIThread
                #else
                new Plugin.Threading.UIThreadRunInvoker(this.context).BeginInvokeOnUIThread
                #endif
                        (
                                delegate
                            {
                                var ev = Error;
                                if (ev != null)
                                {
                                    ev(this, args);
                                }
                            }
                    );
            }

            return;
        }
        //---------------------------------------------------------------------------------------
        #endregion

        #region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		Added IsAuthenticated check #88
        ///		https://github.com/xamarin/Xamarin.Auth/pull/88
        ///		
        /// <summary>
        /// Used by the ui to determine if it should stop authenticating
        /// </summary>
        public Func<bool> IsAuthenticated { get; set; }

        /// <summary>
        /// Used by Android to fill in the result on the activity
        /// </summary>
        public Func<Account, AccountResult> GetAccountResult { get; set; }
        //---------------------------------------------------------------------------------------
        #endregion

        protected int classlevel_depth = 0;

        public override string ToString()
        {
            /*
            string msg = string.Format
                                (
                                    "[Authenticator: Title={0}, AllowCancel={1}, ShowErrors={2}, HasCompleted={3},"
                                    +
                                    "IsAuthenticated={4}, GetAccountResult={5}]", 
                                    Title, 
                                    AllowCancel, 
                                    ShowErrors, 
                                    HasCompleted, 
                                    IsAuthenticated, 
                                    GetAccountResult
                                );
            */
            System.Text.StringBuilder sb = new System.Text.StringBuilder(base.ToString());

            sb.AppendLine().AppendLine(this.GetType().ToString());
            classlevel_depth++;
            string prefix = new string('\t', classlevel_depth);
            sb.Append(prefix).AppendLine($"Title            = {Title}");
            sb.Append(prefix).AppendLine($"ShowErrors       = {ShowErrors}");
            sb.Append(prefix).AppendLine($"AllowCancel      = {AllowCancel}");
            sb.Append(prefix).AppendLine($"HasCompleted     = {HasCompleted}");
            sb.Append(prefix).AppendLine($"IsAuthenticated  = {IsAuthenticated}");
            sb.Append(prefix).AppendLine($"GetAccountResult = {GetAccountResult}");

            return sb.ToString();
        }
    }
}

