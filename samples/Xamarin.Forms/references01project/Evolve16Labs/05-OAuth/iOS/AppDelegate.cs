using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace ComicBook.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //-----------------------------------------------------------------------------------------------
            // Xamarin.Auth initialization

            // Presenters Initialization
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();

            // User-Agent tweaks for Embedded WebViews (UIWebView and WKWebView)
            global::Xamarin.Auth.WebViewConfiguration.IOS.UserAgent = "moljac++";

            //-----------------------------------------------------------------------------------------------


            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

