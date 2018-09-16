using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 AuthenticateUIType = System.Object;

namespace Xamarin.Auth
{
    /// <summary>
    /// An authenticator that displays a web page.
    /// </summary>
    public abstract partial class WebAuthenticator
    {
        protected override AuthenticateUIType GetPlatformUI()
        {
            AuthenticateUIType ui = null;

            ui = PlatformUIMethod();

            return ui;
        }

        protected AuthenticateUIType GetPlatformUIEmbeddedBrowser()
        {

            System.Uri uri_netfx = this.GetInitialUrlAsync().Result;
            System.Object ui = null;

            return ui;
        }

        public AuthenticateUIType AuthenticationUIPlatformSpecificEmbeddedBrowser()
        {
            return GetPlatformUIEmbeddedBrowser();
        }

    }
}

