using System;

using AuthenticateUIType =
            // global::Android.Support.CustomTabs.CustomTabsIntent
            System.Object
            ;
using UIContext = Android.Content.Context;

namespace Xamarin.Auth
{
    public partial class WebAuthenticator
    {
 
        public delegate AuthenticateUIType PlatformUIMethodDelegate(UIContext context);

        /// <summary>
        /// Gets or sets the get platform UIMethod.
        /// Func (delegate) pointing to the method that generates authentication UI
        /// </summary>
        /// <value>The get platform UIM ethod.</value>
        public event PlatformUIMethodDelegate PlatformUIMethod;

        /// <summary>
        /// Gets the platform Native UI (Android - [Chrome] Custom Tabs).
        /// </summary>
        /// <returns>
        /// The platform Native UI (non-embeded/non-integrated Browser Control/Widget/View (WebView).
        /// Android.Support.CustomTabs.CustomTabsIntent
        /// </returns>
        /// <see cref="https://components.xamarin.com/gettingstarted/xamandroidsupportcustomtabs"/>
        protected AuthenticateUIType GetPlatformUINative(UIContext context)
        {
            //global::Android.Support.CustomTabs.CustomTabsIntent 
            AuthenticateUIType ui = null;
            ui = new global::Android.Support.CustomTabs.CustomTabsIntent.Builder().Build();

            return ui;
        }

        public WebAuthenticator()
        {
            PlatformUIMethod = AuthenticationUIPlatformSpecificEmbeddedBrowser;

            return;
        }

        public AuthenticateUIType AuthenticationUIPlatformSpecificNative(UIContext context)
        {
            return GetPlatformUINative(context);
        }
    }
}
