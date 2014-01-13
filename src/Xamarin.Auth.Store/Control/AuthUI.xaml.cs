using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Xamarin.Auth.Store.Control
{
    public sealed partial class AuthUI : UserControl
    {
        WebAuthenticator _auth;
        public WebAuthenticator Authenticator
        {
            get
            {
                return _auth;
            }
            set 
            { 
                _auth = value;
                _auth.Completed += (sender, args) => 
                {

                };
			_auth.Error+=_auth_Error;
                		

            }
        }

        public async void Navigate()
        {
                        
            Uri uri = await _auth.GetInitialUrlAsync();
            browser.Source = uri;
            browser.Navigate(uri);
        }

        void _auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        public AuthUI()
        {
            this.InitializeComponent();
            //this.browser.Navigating += OnBrowserNavigating;
            this.browser.LoadCompleted += browser_LoadCompleted;
            this.browser.NavigationFailed += browser_NavigationFailed;
        }

        void browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void browser_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
