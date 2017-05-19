using System;

using AuthenticateUIType =
            SafariServices.SFSafariViewController
            //System.Object
            ;

namespace Xamarin.Auth
{
    public partial class WebAuthenticator
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

            sfvc = new global::SafariServices.SFSafariViewController(url_ios, false);
            #if DEBUG
            this.Title = "Auth " + sfvc.GetType().ToString();
            System.Diagnostics.Debug.WriteLine($"SFSafariViewController.Title = {this.Title}");
            #endif

            sfvc.Delegate = new NativeAuthSafariViewControllerDelegate(this);
            sfvc.Title = this.Title;

            ui = sfvc;
            
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

