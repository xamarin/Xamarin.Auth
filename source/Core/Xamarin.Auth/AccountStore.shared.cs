using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public abstract partial class AccountStore
    {
        public static string StorePassword { get; set; }

        public abstract IEnumerable<Account> FindAccountsForService(string serviceId);

        public abstract void Save(Account account, string serviceId);

        public abstract void Delete(Account account, string serviceId);

        public abstract Task<IEnumerable<Account>> FindAccountsForServiceAsync(string serviceId);

        public abstract Task SaveAsync(Account account, string serviceId);

        public abstract Task DeleteAsync(Account account, string serviceId);

        public static AccountStore Create()
        {
#if __ANDROID__
            return PlatformCreate(null, null);
#elif __IOS__
            return new KeyChainAccountStore();
#elif WINDOWS_UWP
            return new UWPAccountStore();
#else
            throw new System.PlatformNotSupportedException();
#endif
        }
    }
}
