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
        /// Create an account store.
        /// </summary>
        /// <summary>
        /// Create an account store.
        /// </summary>
        public static AccountStore Create()
        {
            return new KeyChainAccountStore();
        }
    }
}

