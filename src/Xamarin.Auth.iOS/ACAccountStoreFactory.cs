using Xamarin.Auth;
using Xamarin.Auth.iOS;


[assembly: PlatformAccountStore(typeof(KeyChainAccountStoreFactory))]

namespace Xamarin.Auth.iOS
{
    public class KeyChainAccountStoreFactory : IAccountStoreFactory
    {
        public IAccountStore Create(char[] password = null)
        {
            return new KeyChainAccountStore(password);
        }
    }
}