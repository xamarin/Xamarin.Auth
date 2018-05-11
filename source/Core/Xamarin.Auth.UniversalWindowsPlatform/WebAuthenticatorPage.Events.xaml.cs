using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    public sealed partial class WebAuthenticatorPage
    {

        Uri url_initial = null;
        Uri url_args_redirect = null;

        WebAuthenticator authenticator = null;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (null == authenticator)
            {
                authenticator = e.Parameter as WebRedirectAuthenticator;
                if (authenticator == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid Authenticator {e.Parameter?.GetType().ToString() ?? "null"}");
                    throw new Xamarin.Auth.AuthException($"Invalid Authenticator {e.Parameter?.GetType().ToString() ?? "null"}");
                }
            }

            url_initial = this.authenticator.GetInitialUrlAsync().Result;
            this.browser.Navigate(url_initial);

            System.Diagnostics.Debug.WriteLine("OnNavigatedTo authenticator = " + authenticator.Title);

            authenticator.Completed += auth_Completed;
            authenticator.Error += auth_Error;

            url_initial = await authenticator.GetInitialUrlAsync();

            base.OnNavigatedTo(e);

            return;
        }

        private async void Browser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Uri uri_navigated = args.Uri;

            System.Diagnostics.Debug.WriteLine("Browser_NavigationStarting uri_navigated = " + uri_navigated.OriginalString);



            // Warning CS1998  This async method lacks 'await' operators and will run 
            // synchronously.Consider using the 'await' operator to await non - blocking API calls, or 
            // 'await Task.Run(...)' to do CPU - bound work on a background thread.	
            System.Threading.Tasks.Task t = null;
            t = System.Threading.Tasks.Task.Run
                                        (
                                            () => 
                                            {
                                                this.authenticator.OnPageLoading(uri_navigated);
                                            }
                                        );

            return;
        }

        private async void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            url_args_redirect = args.Uri;
            Windows.Web.WebErrorStatus status = args.WebErrorStatus;

            System.Diagnostics.Debug.WriteLine("Browser_NavigationCompleted = " + url_args_redirect.OriginalString);

            return;
        }

        private async void Browser_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            Uri uri_failed = e.Uri;

            System.Diagnostics.Debug.WriteLine("Browser_NavigationFailed = " + uri_failed.OriginalString);

            return;
        }

        private void auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new Xamarin.Auth.AuthException("Auth Error");
        }

        private async void auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("auth_Completed Username = " + e?.Account?.Username);

            if (this.Frame.CanGoBack)
            {
                await
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                    (
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        () =>
                        {
                            this.Frame.GoBack();
                        }
                    );
            }

            return;
        }
    }
}
