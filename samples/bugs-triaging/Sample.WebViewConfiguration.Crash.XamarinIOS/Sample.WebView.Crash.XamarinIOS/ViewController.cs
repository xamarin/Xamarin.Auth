using System;

using UIKit;

namespace Sample.WebView.Crash.XamarinIOS
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            Xamarin.Auth.WebViewConfiguration.IOS.IsUsingWKWebView = true;
            Xamarin.Auth.WebViewConfiguration.IOS.UserAgent = "moljac++";

            string dump = Xamarin.Auth.WebViewConfiguration.IOS.ToString();
            System.Console.WriteLine(dump);

            return;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
