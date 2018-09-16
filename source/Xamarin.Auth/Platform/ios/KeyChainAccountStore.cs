
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;


#if ! __UNIFIED__
using MonoTouch.Security;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
#else
using Security;
using Foundation;
using ObjCRuntime;
#endif

namespace Xamarin.Auth
{
    internal partial class KeyChainAccountStore : AccountStore
    {
        public override IEnumerable<Account> FindAccountsForService(string serviceId)
        {
            return FindAccountsForServiceAsync(serviceId).Result;
        }

        public override void Save(Account account, string serviceId)
        {
            SaveAsync(account, serviceId);

            return;
        }

        public override void Delete(Account account, string serviceId)
        {
            DeleteAsync(account, serviceId);

            return;
        }
    }
}

