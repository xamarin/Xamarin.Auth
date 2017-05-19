using System;

using AuthenticateUIType = System.Object;

namespace Xamarin.Auth
{
    /// <summary>
    /// Web authenticator.
    /// Native login implementation[s].
    /// </summary>
    /// <see cref="https://github.com/GoogleChrome/custom-tabs-client/tree/master/demos/src/main/java/org/chromium/customtabsdemos"/>
    /// <see cref="https://developer.chrome.com/multidevice/android/customtabs"/>
    /// <see cref=""/>
    /// <see cref=""/>
    /// <see cref=""/>
    public partial class WebAuthenticator
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
