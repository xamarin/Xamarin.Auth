using System;
using Accounts;

namespace Xamarin.Auth
{
    public class ACAccountWrapper : Account
    {
        /// <summary>
        /// The store that this account came from. We need to keep this reference to prevent the
        /// store from getting collected. It's necessary to keep it in memory or else the
        /// ACAccount store will stop working.
        /// </summary>
        #pragma warning disable 414
        ACAccountStore store;
        #pragma warning restore 414

        public ACAccount ACAccount { get; private set; }

        public override string Username
        {
            get
            {
                return ACAccount.Username;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine("iOS Account.Username set - NoOP {0}", value);
            }
        }

        public ACAccountWrapper(ACAccount account, ACAccountStore store)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            this.ACAccount = account;

            this.store = store;
        }
    }
}

