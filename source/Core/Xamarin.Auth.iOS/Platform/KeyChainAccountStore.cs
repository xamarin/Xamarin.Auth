using Foundation;
using Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    internal partial class KeyChainAccountStore : AccountStore
    {
        public override Task<IEnumerable<Account>> FindAccountsForServiceAsync(string serviceId)
        {
            var accounts = FindAccountsForService(serviceId);
            return Task.FromResult(accounts);
        }

        public override Task SaveAsync(Account account, string serviceId)
        {
            Save(account, serviceId);
            return Task.CompletedTask;
        }

        public override Task DeleteAsync(Account account, string serviceId)
        {
            Delete(account, serviceId);
            return Task.CompletedTask;
        }

        public override IEnumerable<Account> FindAccountsForService(string serviceId)
        {
            var query = new SecRecord(SecKind.GenericPassword)
            {
                Service = serviceId
            };

            var records = SecKeyChain.QueryAsRecord(query, 1000, out SecStatusCode result);

            var accountsFound = new List<Account>();
            foreach (var rec in records)
            {
                var acc = GetAccountFromRecord(rec);
                if (acc != null)
                    accountsFound.Add(acc);
            }

            return accountsFound;
        }

        public override void Save(Account account, string serviceId)
        {
            var statusCode = SecStatusCode.Success;
            var serializedAccount = account.Serialize();
            var data = NSData.FromString(serializedAccount, NSStringEncoding.UTF8);

            // Remove any existing record
            var existing = FindAccount(account.Username, serviceId);
            if (existing != null)
            {
                var query = new SecRecord(SecKind.GenericPassword)
                {
                    Service = serviceId,
                    Account = account.Username
                };
                statusCode = SecKeyChain.Remove(query);

                if (statusCode != SecStatusCode.Success)
                    throw new AccountStoreException($"Could not remove account from KeyChain: {statusCode}");
            }

            // Add this record
            var record = new SecRecord(SecKind.GenericPassword)
            {
                Service = serviceId,
                Account = account.Username,
                ValueData = data,
                Accessible = SecAccessible.AfterFirstUnlock
            };

            statusCode = SecKeyChain.Add(record);

            if (statusCode != SecStatusCode.Success)
            {
                if ((int)statusCode == -34018)
                {
                    throw new AccountStoreException(
                        $"Could not save account to KeyChain. " +
                        $"Keychain Access Groups might not have been added to Entitlements.plist or the Keychain Sharing Capabilities was not enabled. " +
                        $"Error code {statusCode}.");
                }

                throw new AccountStoreException(
                    $"Could not save account to KeyChain. " +
                    $"Entitlements.plist might be missing. " +
                    $"Error code {statusCode}.");
            }
        }

        public override void Delete(Account account, string serviceId)
        {
            var query = new SecRecord(SecKind.GenericPassword)
            {
                Service = serviceId,
                Account = account.Username
            };

            var statusCode = SecKeyChain.Remove(query);

            if (statusCode != SecStatusCode.Success)
                throw new AuthException($"Could not delete account from KeyChain: {statusCode}");
        }

        private Account GetAccountFromRecord(SecRecord record)
        {
            if (record == null)
                return null;

            NSData data = null;
            if (record.Generic != null)
                data = record.Generic;
            else if (record.ValueData != null)
                data = record.ValueData;

            if (data == null)
                return null;

            var serializedData = NSString.FromData(data, NSStringEncoding.UTF8);
            return Account.Deserialize(serializedData);
        }

        private Account FindAccount(string username, string serviceId)
        {
            var query = new SecRecord(SecKind.GenericPassword)
            {
                Service = serviceId,
                Account = username
            };

            var record = SecKeyChain.QueryAsRecord(query, out SecStatusCode result);
            return GetAccountFromRecord(record);
        }
    }
}
