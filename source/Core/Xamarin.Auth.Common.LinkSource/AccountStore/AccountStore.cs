//
//  Copyright 2012-2016, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Collections.Generic;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// A persistent storage for <see cref="Account"/>s. This storage is encrypted.
    /// Accounts are stored using a service ID and the username of the account
    /// as a primary key.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal abstract partial class AccountStore
    #else
    [Obsolete("Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide")]
    public abstract partial class AccountStore
    #endif
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
        [Obsolete("Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide")]
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
        [Obsolete("Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide")]
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
        [Obsolete("Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide")]
        public abstract void Delete(Account account, string serviceId);
    }
}
