using AuthenticateUIType = Android.Content.Intent;
using Context = Android.Content.Context;

namespace Xamarin.Auth
{
    partial class Authenticator
    {
        public AuthenticateUIType GetUI(Context context)
        {
            return GetPlatformUI(context);
        }

        protected abstract AuthenticateUIType GetPlatformUI(Context context);
    }
}
