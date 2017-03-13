using System;

using AuthenticateUIType =
            // SafariServices.SFSafariViewController
            System.Object
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

            ui = 
                new global::SafariServices.SFSafariViewController(url_ios, false)
                //new Xamarin.Auth.SafariServices.SFSafariViewController()
            {
                Delegate = new NativeAuthSafariViewControllerDelegate(this),
            };

            return ui;
        }
    }
}

