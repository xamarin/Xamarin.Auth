using Accounts;
using System;

namespace Xamarin.Auth
{
    public class ACAccountWrapper : Account
    {
        private readonly ACAccountStore accountStore;

        public ACAccountWrapper(ACAccount account, ACAccountStore store)
        {
            ACAccount = account ?? throw new ArgumentNullException(nameof(account));
            accountStore = store;
        }

        public ACAccount ACAccount { get; private set; }

        public override string Username
        {
            get => ACAccount.Username;
            set => base.Username = value;
        }
    }
}
