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

#if ! __UNIFIED__
using MonoTouch.Accounts;
#else
using Accounts;
#endif

namespace Xamarin.Auth
{
    #if XAMARIN_AUTH_INTERNAL
    internal class ACAccountWrapper : Account
    #else
    public class ACAccountWrapper : Account
    #endif
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

