using System;
using System.Collections.Generic;
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
    /// A process and user interface to authenticate a user.
    /// </summary>
    public abstract partial class Authenticator
    {
        UIContext context;
        public AuthenticateUIType GetUI(UIContext context)
        {
            this.context = context;
            return GetPlatformUI(context);
        }

        protected abstract AuthenticateUIType GetPlatformUI(UIContext context);
    }
}

