using System;

namespace Xamarin.Auth
{
    /// <summary>
    /// Web view configuration.
    /// </summary>
    public static class WebViewConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Xamarin.Auth.WebAuthenticatorController"/> 
        /// is using WKWebView or by default UIWebView
        /// </summary>
        /// <value><c>true</c> if is using WKW eb view; otherwise, <c>false</c>.</value>
        public static bool IsUsingWKWebView
        {
            get;
            set;
        } = false;

    }
}
