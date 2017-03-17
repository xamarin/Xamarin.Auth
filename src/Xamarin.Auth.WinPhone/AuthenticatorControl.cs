//
// AuthenticatorControl.cs
//
// Author:
//    Gabor Nemeth (gabor.nemeth.dev@gmail.com)
//
//    Copyright (C) 2015, Gabor Nemeth
//
// Authentication control for Windows Phone 8.1 Runtime apps
//
        
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace Xamarin.Auth
{
    /// <summary>
    /// Authentication control, that presents a web view for the user to authenticate
    /// Can be embedded in page, or another control, it is templateable
    /// </summary>
    public class AuthenticatorControl : ContentControl
    {
        private WebAuthenticator authenticator;
        private WebView webView;

        public AuthenticatorControl(WebAuthenticator authenticator)
        {
            if (authenticator == null)
                throw new ArgumentNullException("Authenticator cannot be null.");

            this.authenticator = authenticator;
            this.webView = new WebView();
            webView.NavigationCompleted += webView_NavigationCompleted;
            Content = webView;
            Loaded += AuthenticatorControl_Loaded;
        }

        async void AuthenticatorControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var startUrl = await authenticator.GetInitialUrlAsync();
            // remove cookies - don't want to remember the user credentials
            Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookieManager = filter.CookieManager;
            HttpCookieCollection cookies = cookieManager.GetCookies(startUrl);
            foreach (HttpCookie cookie in cookies)
            {
                cookieManager.DeleteCookie(cookie);
            }
            // navigate to the authorization page
            webView.Navigate(startUrl);
        }

        void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            authenticator.OnPageLoaded(args.Uri);
        }
    }
}
