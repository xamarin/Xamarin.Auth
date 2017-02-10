using System;
namespace Xamarin.Auth
{
    public class NativeAuthSafariViewControllerDelegate : SafariServices.SFSafariViewControllerDelegate
    {
        WeakReference authenticator;
        public NativeAuthSafariViewControllerDelegate(WebAuthenticator authenticator)
        {
            this.authenticator = new WeakReference(authenticator);
        }
        public override void DidFinish(SafariServices.SFSafariViewController controller)
        {
            (authenticator?.Target as WebAuthenticator)?.OnCancelled();
        }
    }
}
