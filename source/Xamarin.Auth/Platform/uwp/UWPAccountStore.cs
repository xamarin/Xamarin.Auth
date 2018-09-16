
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Storage;

namespace Xamarin.Auth
{
    internal partial class UWPAccountStore : AccountStore
    {
        public override IEnumerable<Account> FindAccountsForService(string serviceId)
        {
            return FindAccountsForServiceAsync(serviceId).Result;
        }

        public override void Save(Account account, string serviceId)
        {
            Task t  = SaveAsync(account, serviceId);

            return;
        }

        public override void Delete(Account account, string serviceId)
        {
            Task t = DeleteAsync(account, serviceId);

            return;
        }
    }
}
