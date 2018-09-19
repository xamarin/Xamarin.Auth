using AuthenticateUIType = System.Type;

namespace Xamarin.Auth
{
    partial class Authenticator
    {
        public AuthenticateUIType GetUI()
        {
            return GetPlatformUI();
        }

        protected abstract AuthenticateUIType GetPlatformUI();
    }
}
