using UIKit;
using AuthenticateUIType = UIKit.UIViewController;

namespace Xamarin.Auth
{
    partial class FormAuthenticator : Authenticator
    {
        protected override AuthenticateUIType GetPlatformUI()
        {
            return new UINavigationController(new FormAuthenticatorController(this));
        }
    }
}
