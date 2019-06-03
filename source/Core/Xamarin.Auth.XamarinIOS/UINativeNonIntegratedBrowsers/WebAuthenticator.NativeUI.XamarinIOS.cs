using System;

using AuthenticateUIType =
            SafariServices.SFSafariViewController
            //System.Object
            ;
using System.Text;

namespace Xamarin.Auth
{
    #if XAMARIN_AUTH_INTERNAL
    internal partial class WebAuthenticator
    #else
    public partial class WebAuthenticator
    #endif
    {
        /// <summary>
        /// Gets or sets the get platform UIMethod.
        /// Func (delegate) pointing to the method that generates authentication UI
        /// </summary>
        /// <value>The get platform UIM ethod.</value>
        public Func<AuthenticateUIType> PlatformUIMethod
        {
            get;
            set;
        }

        protected AuthenticateUIType GetPlatformUINative()
        {
            System.Uri uri_netfx = this.GetInitialUrlAsync().Result;
            Foundation.NSUrl url_ios = new Foundation.NSUrl(uri_netfx.AbsoluteUri);

            // SafariServices.SFSafariViewController 
            AuthenticateUIType ui = null;

            global::SafariServices.SFSafariViewController sfvc = null;

            if 
                ( 
                    // double check (trying to lookup class and check iOS version)
                    ObjCRuntime.Class.GetHandle("SFSafariViewController") != IntPtr.Zero
                    &&
                    UIKit.UIDevice.CurrentDevice.CheckSystemVersion (9, 0)
                )
            {
                sfvc = new global::SafariServices.SFSafariViewController(url_ios, false);

                #if DEBUG
                this.Title = "Auth " + sfvc.GetType().ToString();
                System.Diagnostics.Debug.WriteLine($"SFSafariViewController.Title = {this.Title}");
                #endif

                sfvc.Delegate = new NativeAuthSafariViewControllerDelegate(this);
                sfvc.Title = this.Title;

                ui = sfvc;
            }
            else
            {
                // Fallback to Embedded WebView
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("SafariViewController not available!");
                msg.AppendLine("Fallback to embbeded web view ");
                this.ShowErrorForNativeUIAlert(msg.ToString());

                this.GetPlatformUIEmbeddedBrowser();
            }

            return ui;
        }

        protected void ShowErrorForNativeUIAlert(string v)
        {
            new Plugin.Threading.UIThreadRunInvoker().BeginInvokeOnUIThread
                                (
                                    () =>
                                    {
                                        UIKit.UIAlertView alert = null;
                                        alert = new UIKit.UIAlertView
                                                        (
                                                            "WARNING", 
                                                            v, 
                                                            null, 
                                                            "Ok", 
                                                            null
                                                        );
                        				alert.Show();                                        
                                    }
                                );
            return;
        }
    }
}

