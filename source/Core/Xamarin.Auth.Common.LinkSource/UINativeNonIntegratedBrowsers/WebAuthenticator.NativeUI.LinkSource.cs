using System;

using AuthenticateUIType = System.Object;

namespace Xamarin.Auth
{
    #if XAMARIN_AUTH_INTERNAL
    internal abstract partial class WebAuthenticator
    #else
    public abstract partial class WebAuthenticator
    #endif
    {
        /// <summary>
        /// Gets or sets the get platform UIMethod.
        /// Func (delegate) pointing to the method that generates authentication UI
        /// </summary>
        /// <value>The get platform UIM ethod.</value>
        public Func<AuthenticateUIType> PlatformUIMethod
        {
            get;
            set;
        }

        protected virtual AuthenticateUIType GetPlatformUINative()
        {
            System.Uri uri_netfx = this.GetInitialUrlAsync().Result;
            System.Object ui = null;

            return ui;
        }

        public AuthenticateUIType AuthenticationUIPlatformSpecificNative()
        {
            return GetPlatformUINative();
        }

        protected void ShowErrorForNativeUIAlert(string v)
        {

        }
    }
}
