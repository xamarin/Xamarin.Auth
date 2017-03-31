using System;

using AuthenticateUIType =
            // global::Android.Support.CustomTabs.CustomTabsIntent.Builder
            System.Object
            ;
using UIContext =
            //Android.Content.Context
            Android.App.Activity
            ;

namespace Xamarin.Auth
{
    public partial class WebAuthenticator
    {

        public delegate AuthenticateUIType PlatformUIMethodDelegate(UIContext context);

        /// <summary>
        /// Gets or sets the get platform UIMethod.
        /// Func (delegate) pointing to the method that generates authentication UI
        /// </summary>
        /// <value>The get platform UIM ethod.</value>
        public event PlatformUIMethodDelegate PlatformUIMethod;

        global::Android.Graphics.Color color_xamarin_blue;

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
            global::Android.Support.CustomTabs.CustomTabsActivityManager ct_mgr = null;
            global::Android.Support.CustomTabs.CustomTabsIntent.Builder ct_builder = null;

            //global::Android.Support.CustomTabs.CustomTabsIntent.Builder 
            AuthenticateUIType ui = null;

            //global::Android.App.Activity activity = (global::Android.App.Activity)context;
            ct_mgr = new global::Android.Support.CustomTabs.CustomTabsActivityManager(context);
            ct_builder = new global::Android.Support.CustomTabs.CustomTabsIntent.Builder(ct_mgr.Session);


            // UI customization
            //      CustomTabsIntent - !OK == NOGO
            //      CustomTabsIntent.Builder - OK == GO
            /*

            color_xamarin_blue = new global::Android.Graphics.Color(0x34, 0x98, 0xdb);

            ui = new global::Android.Support.CustomTabs.CustomTabsIntent.Builder().Build();
			ui = ct_builder
				.SetToolbarColor(color_xamarin_blue)
				.SetShowTitle(true)
				.EnableUrlBarHiding()
				.Build();
            */

            return ct_builder;
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
