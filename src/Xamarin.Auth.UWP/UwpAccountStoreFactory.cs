using Xamarin.Auth;

[assembly: PlatformAccountStore(typeof(UwpAccountStoreFactory))]

namespace Xamarin.Auth
{
    public class UwpAccountStoreFactory : IAccountStoreFactory
    {
        public IAccountStore Create(char[] password = null)
        {
            return new UwpAccountStore(password);
        }
    }
}