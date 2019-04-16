using System;

using Foundation;
using UIKit;
using SafariServices;

namespace Xamarin.Auth//.SafariServices
{
    #if XAMARIN_AUTH_INTERNAL
    internal partial class NativeAuthSafariViewControllerDelegate
        :
        global::SafariServices.SFSafariViewControllerDelegate
        // to mimic SFSafariViewController
        //UIKit.UIViewController,
        //global::SafariServices.ISFSafariViewControllerDelegate
    #else
    public partial class NativeAuthSafariViewControllerDelegate
        :
        global::SafariServices.SFSafariViewControllerDelegate
        // to mimic SFSafariViewController
        //UIKit.UIViewController,
        //global::SafariServices.ISFSafariViewControllerDelegate
    #endif
    {
        WebAuthenticator authenticator = null;

        public NativeAuthSafariViewControllerDelegate(WebAuthenticator wa)
        {
            authenticator = wa;

            return;
        }

        public override void DidCompleteInitialLoad(SFSafariViewController controller, bool didLoadSuccessfully)
        {
			return;
        }

		public override void DidFinish(SFSafariViewController controller)
        {
            if (authenticator.AllowCancel)
            {
                authenticator.OnCancelled();
            }

			return;
		}

		public override UIActivity[] GetActivityItems(SFSafariViewController controller, NSUrl url, string title)
        {
			return null;
		}

    }
}
