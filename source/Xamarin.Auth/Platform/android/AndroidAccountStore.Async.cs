using System;
using System.Collections.Generic;
using System.Linq;
using Java.Security;
using Javax.Crypto;
using Javax.Security.Auth.Callback;
using Java.IO;
using Android.Content;
using Android.Runtime;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    /// <summary>
    /// AccountStore that uses a KeyStore of PrivateKeys protected by a fixed password
    /// in a private region of internal storage.
    /// </summary>
    internal partial class AndroidAccountStore : AccountStore
    {
        public override Task<List<Account>> FindAccountsForServiceAsync(string serviceId)
        {
            var r = new List<Account>();

            var postfix = "-" + serviceId;

            var aliases = ks.Aliases();
            while (aliases.HasMoreElements)
            {
                var alias = aliases.NextElement().ToString();
                if (alias.EndsWith(postfix))
                {
                    var e = ks.GetEntry(alias, prot) as KeyStore.SecretKeyEntry;
                    if (e != null)
                    {
                        var bytes = e.SecretKey.GetEncoded();
                        var serialized = System.Text.Encoding.UTF8.GetString(bytes);
                        var acct = Account.Deserialize(serialized);
                        r.Add(acct);
                    }
                }
            }

            r.Sort((a, b) => a.Username.CompareTo(b.Username));

            return Task.FromResult(r);
        }

        public override Task SaveAsync(Account account, string serviceId)
        {
            string alias = MakeAlias(account, serviceId);

            SecretAccount secretKey = new SecretAccount(account);
            Java.Security.KeyStore.SecretKeyEntry entry = new KeyStore.SecretKeyEntry(secretKey);
            ks.SetEntry(alias, entry, prot);

            Save();

            return Task.FromResult(true);
        }

        public override Task DeleteAsync(Account account, string serviceId)
        {
            var alias = MakeAlias(account, serviceId);

            ks.DeleteEntry(alias);
            Save();

            return Task.FromResult(true);
        }

    }
}

