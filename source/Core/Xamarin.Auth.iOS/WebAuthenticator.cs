using Foundation;
using SafariServices;
using UIKit;
using AuthenticateUIType = UIKit.UIViewController;

namespace Xamarin.Auth
{
    public abstract partial class WebAuthenticator
    {
        protected override AuthenticateUIType GetPlatformUI()
        {
            if (IsUsingNativeUI && UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                return GetPlatformUINative();

            return GetPlatformUIEmbeddedBrowser();
        }

        protected virtual AuthenticateUIType GetPlatformUINative()
        {
            var uri = GetInitialUrlAsync().Result;

            return new SFSafariViewController(new NSUrl(uri.AbsoluteUri), false)
            {
                Delegate = new NativeAuthSafariViewControllerDelegate(this),
                Title = Title
            };
        }

        protected virtual AuthenticateUIType GetPlatformUIEmbeddedBrowser()
        {
            return new UINavigationController(new WebAuthenticatorController(this));
        }

        public static void ClearCookies()
        {
            var store = NSHttpCookieStorage.SharedStorage;
            var cookies = store.Cookies;
            foreach (var c in cookies)
            {
                store.DeleteCookie(c);
            }
        }

        private class NativeAuthSafariViewControllerDelegate : SFSafariViewControllerDelegate
        {
            private WebAuthenticator authenticator = null;

            public NativeAuthSafariViewControllerDelegate(WebAuthenticator wa)
            {
                authenticator = wa;
            }

            public override void DidFinish(SFSafariViewController controller)
            {
                if (authenticator.AllowCancel)
                    authenticator.OnCancelled();
            }
        }
    }
}
