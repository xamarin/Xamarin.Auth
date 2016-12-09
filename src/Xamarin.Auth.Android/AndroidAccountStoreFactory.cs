using Xamarin.Auth;
using Xamarin.Auth.Android;


[assembly: PlatformAccountStore(typeof(AndroidAccountStoreFactory))]

namespace Xamarin.Auth.Android
{
    public class AndroidAccountStoreFactory : IAccountStoreFactory
    {
        public IAccountStore Create(char[] password = null)
        {
            return new AndroidAccountStore(password);
        }
    }
}