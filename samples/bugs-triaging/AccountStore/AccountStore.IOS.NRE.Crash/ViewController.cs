using Foundation;
using System;
using UIKit;

namespace AccountStore.IOS.NRE.Crash
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            Xamarin.Auth.AccountStore accountStore = Xamarin.Auth.AccountStore.Create();
            var accounts = accountStore.FindAccountsForService("someid");

            return;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}