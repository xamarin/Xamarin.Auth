using System.Collections.Generic;

namespace Xamarin.Auth
{
    public interface IAccountStore
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
        IEnumerable<Account> FindAccountsForService (string serviceId);

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
        void Save (Account account, string serviceId);

        /// <summary>
        /// Deletes the account for a given serviceId.
        /// </summary>
        /// <param name='account'>
        /// Account to delete.
        /// </param>
        /// <param name='serviceId'>
        /// Service identifier.
        /// </param>
        void Delete (Account account, string serviceId);
    }
}