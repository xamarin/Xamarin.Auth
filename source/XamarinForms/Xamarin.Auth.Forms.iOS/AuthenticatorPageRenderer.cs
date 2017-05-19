using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:
    ExportRenderer
        (
            typeof(Xamarin.Auth.XamarinForms.AuthenticatorPage),
            typeof(Xamarin.Auth.XamarinForms.XamarinIOS.AuthenticatorPageRenderer)
        )
]
namespace Xamarin.Auth.XamarinForms.XamarinIOS
{
    [Preserve(AllMembers = true)]
    public class AuthenticatorPageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public override void ViewDidAppear(bool animated)
        {
            if (!was_shown)
            {
                was_shown = true;

                base.ViewDidAppear(animated);
                authenticator_page = (AuthenticatorPage)base.Element;

                Authenticator = authenticator_page.Authenticator;
                Authenticator.Completed += Authentication_Completed;
                Authenticator.Error += Authentication_Error;

    			PresentViewController(Authenticator.GetUI(), true, null);
            }


            return;
        }

        bool was_shown = false;

        /*
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            authenticator_page = (AuthenticatorPage)base.Element;

            Authenticator = authenticator_page.Authenticator;
            Authenticator.Completed += Authentication_Completed;
            Authenticator.Error += Authentication_Error;

            // create a new window instance based on the screen size
            window = new UIKit.UIWindow(UIKit.UIScreen.MainScreen.Bounds);

            var ui = Authenticator.GetUI();
            // If you have defined a view, add it here:
            // window.RootViewController  = navigationController;
            window.RootViewController = ui;

            // make the window visible
            window.MakeKeyAndVisible();

            return;
        }
        
        UIKit.UIWindow window;
        */

        public static Authenticator Authenticator = null;
        public AuthenticatorPage authenticator_page = null;

        protected void Authentication_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            authenticator_page.Authentication_Completed(sender, e);

            return;
        }

        protected void Authentication_Error(object sender, AuthenticatorErrorEventArgs e)
        {

            authenticator_page.Authentication_Error(sender, e);

            return;
        }

        protected void Authentication_BrowsingCompleted(object sender, EventArgs e)
        {
            authenticator_page.Authentication_BrowsingCompleted(sender, e);

            return;
        }
    }
}

