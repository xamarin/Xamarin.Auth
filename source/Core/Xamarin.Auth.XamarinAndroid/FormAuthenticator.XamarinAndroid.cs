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
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using AuthenticateUIType =
            Android.Content.Intent
            //System.Object
            ;
using UIContext =
            Android.Content.Context
            //Android.App.Activity
            ;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// An authenticator that presents a form to the user.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal abstract partial class FormAuthenticator : Authenticator
    #else
    public abstract partial class FormAuthenticator : Authenticator
    #endif
    {
        /// <summary>
        /// Gets the UI to present this form.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
        protected override AuthenticateUIType GetPlatformUI(UIContext context)
        {
            var i = new global::Android.Content.Intent(context, typeof(FormAuthenticatorActivity));
            var state = new FormAuthenticatorActivity.State
            {
                Authenticator = this,
            };
            i.PutExtra("StateKey", FormAuthenticatorActivity.StateRepo.Add(state));
            return i;
        }
    }
}

