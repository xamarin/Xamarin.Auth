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

namespace Xamarin.Auth
{
    /// <summary>
    /// An authenticator that presents a form to the user.
    /// </summary>
    public abstract partial class FormAuthenticator : Authenticator
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

