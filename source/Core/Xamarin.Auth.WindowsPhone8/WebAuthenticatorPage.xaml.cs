//
//  Copyright 2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth.WindowsPhone
#else
namespace Xamarin.Auth._MobileServices.WindowsPhone
#endif
{
    public partial class WebAuthenticatorPage : PhoneApplicationPage
    {
        public WebAuthenticatorPage()
        {
            InitializeComponent();

            this.browser.Navigating += OnBrowserNavigating;
            this.browser.Navigated += OnBrowserNavigated;
            this.browser.NavigationFailed += OnBrowserNavigationFailed;

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            return;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            /*
			System.Windows.Controls.Frame rootFrame = this.Content as Frame;
			if (rootFrame != null && rootFrame.CanGoBack && MainWebView.CanGoBack)
			{
				e.Handled = true;
				MainWebView.GoBack();
			}
			*/
            return;
        }

        protected override async void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);


            if (browser.CanGoBack)
            {
                e.Cancel = true;
                await
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                    (
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        () =>
                        {
                            browser.GoBack();
                        }
                    );
            }
            else
            {
                return;
            }

            return;
        }

      }
}