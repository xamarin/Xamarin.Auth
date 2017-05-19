using System;

using Foundation;
using UIKit;
using SafariServices;

namespace Xamarin.Auth//.SafariServices
{
	public partial class NativeAuthSafariViewControllerDelegate 
        :
			global::SafariServices.SFSafariViewControllerDelegate
            // to mimic SFSafariViewController
            //UIKit.UIViewController,
            //global::SafariServices.ISFSafariViewControllerDelegate
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
			return;
		}

		public override UIActivity[] GetActivityItems(SFSafariViewController controller, NSUrl url, string title)
        {
			return null;
		}

    }
}
