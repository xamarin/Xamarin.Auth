using AuthenticateUIType = Android.Content.Intent;
using Context = Android.Content.Context;

namespace Xamarin.Auth
{
    partial class FormAuthenticator : Authenticator
    {
        protected override AuthenticateUIType GetPlatformUI(Context context)
        {
            return FormAuthenticatorActivity.CreateIntent(context, this);
        }
    }
}
