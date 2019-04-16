using System;

using AuthenticateUIType = System.Object;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    public partial class WebAuthenticator
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

        /// <summary>
        /// Gets the platform Native UI (Android - [Chrome] Custom Tabs).
        /// </summary>
        /// <returns>
        /// The platform Native UI (non-embeded/non-integrated Browser Control/Widget/View (WebView).
        /// Android.Support.CustomTabs.CustomTabsIntent
        /// </returns>
        /// <see cref="https://components.xamarin.com/gettingstarted/xamandroidsupportcustomtabs"/>
        protected virtual AuthenticateUIType GetPlatformUINative()
        {
            throw new NotImplementedException("PCL bite-n-switch");

            System.Uri uri_netfx = this.GetInitialUrlAsync().Result;

            // System.Object
            AuthenticateUIType ui = null;

            return ui;
        }

        public AuthenticateUIType AuthenticationUIPlatformSpecificNative()
        {
            return GetPlatformUINative();
        }

        protected void ShowErrorForNativeUIAlert(string v)
        {
            throw new NotImplementedException("Ups not on Portable");
        }
    }
}
