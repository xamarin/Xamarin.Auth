using System;
using System.Collections.Generic;

namespace Xamarin.Auth
{
    /// <summary>
    /// A persistent storage for <see cref="Account"/>s. This storage is encrypted.
    /// Accounts are stored using a service ID and the username of the account
    /// as a primary key.
    /// </summary>
    public abstract partial class AccountStore
    {
        /// <summary>
        /// Finds the accounts for a given service.
        /// </summary>
        /// <returns>
        /// The accounts for the service.
        /// </returns>
        /// <param name='serviceId'>
        /// Service identifier.
        /// </param>
        public abstract IEnumerable<Account> FindAccountsForService(string serviceId);

        /// <summary>
        /// Save the specified account by combining its username and the serviceId
        /// to form a primary key.
        /// </summary>
        /// <param name='account'>
        /// Account to store.
        /// </param>
        /// <param name='serviceId'>
        /// Service identifier.
        /// </param>
        public abstract void Save(Account account, string serviceId);

        /// <summary>
        /// Deletes the account for a given serviceId.
        /// </summary>
        /// <param name='account'>
        /// Account to delete.
        /// </param>
        /// <param name='serviceId'>
        /// Service identifier.
        /// </param>
        public abstract void Delete(Account account, string serviceId);
    }
}
