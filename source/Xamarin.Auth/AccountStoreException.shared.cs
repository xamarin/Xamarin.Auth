using System;
using System.Collections.Generic;

namespace Xamarin.Auth
{
    /// <summary>
    /// A persistent storage for <see cref="Account"/>s. This storage is encrypted.
    /// Accounts are stored using a service ID and the username of the account
    /// as a primary key.
    /// </summary>
    public partial class AccountStoreException : AuthException
    {
        public string Operation
        {
            get;
            set;
        } = "N/A";

        public AccountStoreException(string operation)
            : base(operation)
        {
            this.Operation = operation;
        }

        public AccountStoreException(string operation, System.Exception exc)
            : base(operation, exc)
        {
        }
    }
}

