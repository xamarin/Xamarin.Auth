using System;

using AuthenticateUIType = System.Object;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// Web authenticator.
    /// Native login implementation[s].
    /// </summary>
    /// <see cref="https://github.com/GoogleChrome/custom-tabs-client/tree/master/demos/src/main/java/org/chromium/customtabsdemos"/>
    /// <see cref="https://developer.chrome.com/multidevice/android/customtabs"/>
    #if XAMARIN_AUTH_INTERNAL
    internal abstract partial class WebAuthenticator
    #else
    public abstract partial class WebAuthenticator
	#endif
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Xamarin.Auth.WebAuthenticator"/> 
        /// is using native user interface.
        /// </summary>
        /// <value><c>true</c> if is using native user interface; otherwise, <c>false</c>.</value>
        public bool IsUsingNativeUI
        {
            get
            {
                return is_using_native_ui;
            }
            /*
                Fork users - do NOT add set here!

                If set is used users can set Native UI after ctor and it will be ignored.
                In Xamarin.Auth a lot of things happen already in ctor.... Even first requests...
            */ 
        }

        protected bool is_using_native_ui = false;

        protected void ShowErrorForNativeUI(string v)
        {
            ShowErrorForNativeUIDebug(v);
            ShowErrorForNativeUIAlert(v);

            return;
        }

        protected void ShowErrorForNativeUIDebug(string v)
        {
            System.Diagnostics.Debug.WriteLine(v);

            return;
        }

    }
}
