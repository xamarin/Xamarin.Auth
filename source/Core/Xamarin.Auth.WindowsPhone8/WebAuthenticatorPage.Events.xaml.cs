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
    public partial class WebAuthenticatorPage
    {
        private WebAuthenticator auth;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string key = NavigationContext.QueryString["key"];

            this.auth = (WebAuthenticator)PhoneApplicationService.Current.State[key];
            //this.auth.Completed += (sender, args) => NavigationService.GoBack(); // throws on BackButton
            this.auth.Completed += auth_Completed;
            this.auth.Error += OnAuthError;

            PhoneApplicationService.Current.State.Remove(key);

            if (this.auth.ClearCookiesBeforeLogin)
                await this.browser.ClearCookiesAsync();

            Uri uri = await this.auth.GetInitialUrlAsync();
            this.browser.Source = uri;

            base.OnNavigatedTo(e);
        }

        void auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                // Pull Request - manually added/fixed
                //		Marshalled NavigationService.GoBack to UI Thread #94
                //		https://github.com/xamarin/Xamarin.Auth/pull/94
                new Plugin.Threading.UIThreadRunInvoker().BeginInvokeOnUIThread
                (
                    () =>
                    {
                        NavigationService.GoBack();
                    }
                );
            }

            return;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.auth.OnCancelled();
            base.OnNavigatedFrom(e);
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            //NavigationService.GoBack();
        }

        private void OnBrowserNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            this.progress.IsVisible = false;
            if (e.Exception == null)
            {
                this.auth.OnError("Unknown"); // Shows up when not connected to the internet
            }
            else
            {
                this.auth.OnError(e.Exception);
            }

            return;
        }

        private void OnBrowserNavigated(object sender, NavigationEventArgs e)
        {
            this.progress.IsVisible = false;
            this.auth.OnPageLoaded(e.Uri);
        }

        private void OnBrowserNavigating(object sender, NavigatingEventArgs e)
        {
            this.progress.IsVisible = true;
            this.auth.OnPageLoading(e.Uri);
        }
    }
}