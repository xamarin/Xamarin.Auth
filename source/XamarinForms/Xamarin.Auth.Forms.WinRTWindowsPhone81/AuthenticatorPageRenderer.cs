using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;

[assembly:
    ExportRenderer
        (
            typeof(Xamarin.Auth.XamarinForms.AuthenticatorPage),
            typeof(Xamarin.Auth.XamarinForms.XamarinAndroid.AuthenticatorPageRenderer)
        )
]
namespace Xamarin.Auth.XamarinForms.XamarinAndroid
{
    public class AuthenticatorPageRenderer : Xamarin.Forms.Platform.WinRT.PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            authenticator_page = (AuthenticatorPage)base.Element;

            Authenticator = authenticator_page.Authenticator;
            Authenticator.Completed += Authentication_Completed;
            Authenticator.Error += Authentication_Error;

            System.Type ui_object = Authenticator.GetUI();

            //activity.StartActivity(ui_object);

            return;
        }

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

