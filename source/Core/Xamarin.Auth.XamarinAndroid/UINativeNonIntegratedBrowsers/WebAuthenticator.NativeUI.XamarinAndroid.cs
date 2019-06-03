using System;

using AuthenticateUIType =
            Android.Content.Intent
            //global::Android.Support.CustomTabs.CustomTabsIntent.Builder // not an Intent
            //System.Object
            ;
using UIContext =
            Android.Content.Context
            //Android.App.Activity
            ;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    #if XAMARIN_AUTH_INTERNAL
    internal partial class WebAuthenticator
    #else
    public partial class WebAuthenticator
    #endif
    {

        public delegate AuthenticateUIType PlatformUIMethodDelegate(UIContext context);

        /// <summary>
        /// Gets or sets the get platform UIMethod.
        /// Func (delegate) pointing to the method that generates authentication UI
        /// </summary>
        /// <value>The get platform UI Method.</value>
        public event PlatformUIMethodDelegate PlatformUIMethod;

        /// <summary>
        /// Gets the platform Native UI (Android - [Chrome] Custom Tabs).
        /// </summary>
        /// <returns>
        /// The platform Native UI (non-embeded/non-integrated Browser Control/Widget/View (WebView).
        /// Android.Support.CustomTabs.CustomTabsIntent
        /// </returns>
        /// <see cref="https://components.xamarin.com/gettingstarted/xamandroidsupportcustomtabs"/>
        protected virtual AuthenticateUIType GetPlatformUINative(UIContext context)
        {
            System.Uri uri_netfx = this.GetInitialUrlAsync().Result;
            global::Android.Net.Uri uri_android = global::Android.Net.Uri.Parse(uri_netfx.AbsoluteUri);
            CustomTabsConfiguration.UriAndroidOS = uri_android;
            AuthenticateUIType ui = new AuthenticateUIType(context, typeof(WebAuthenticatorNativeBrowserActivity));
            ui.PutExtra("ClearCookies", ClearCookiesBeforeLogin);
            var state = new WebAuthenticatorNativeBrowserActivity.State
			{
				Authenticator = this,
			};
            ui.PutExtra("StateKey", WebAuthenticatorNativeBrowserActivity.StateRepo.Add(state));

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

        protected void ShowErrorForNativeUIAlert(string v)
        {
            global::Android.Content.Context c = global::Android.App.Application.Context;
            new Plugin.Threading.UIThreadRunInvoker(c).BeginInvokeOnUIThread
                                (
                                    () =>
                                    {
                                        var b = new global::Android.App.AlertDialog.Builder(c);
                                        b.SetMessage(v);
                                        b.SetTitle("Warning");
                                        b.SetNeutralButton
                                         (
                                             "OK", 
                                             (s, e) =>
                                            {
                                                ((global::Android.App.AlertDialog)s).Cancel();
                                            }
                                        );
                                        var alert = b.Create();
                                        alert.Show();

                                    }
                                );
            return;
        }
    }
}
