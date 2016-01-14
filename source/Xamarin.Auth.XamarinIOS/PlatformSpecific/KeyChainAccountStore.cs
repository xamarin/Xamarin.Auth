//
//  Copyright 2012, Xamarin Inc.
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
#define TEST_MARK_T

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

#if ! __UNIFIED__
using MonoTouch.Security;
using MonoTouch.Foundation;
#else
using Security;
using Foundation;
#endif

namespace Xamarin.Auth
{
	internal class KeyChainAccountStore : AccountStore
	{
		public override IEnumerable<Account> FindAccountsForService(string serviceId)
        {
            var query = new SecRecord(SecKind.GenericPassword);
            query.Service = serviceId;

            SecStatusCode result;
            SecRecord[] records = SecKeyChain.QueryAsRecord(query, 1000, out result);

            IEnumerable<Account> accounts_found = null;
            IEnumerable<Account> ienumerable_accounts = null;
            if (records != null)
            {
                /*
                moljac note:
                ienumerable_accounts    
                    {
                        System.Linq.Enumerable.WhereSelectArrayIterator
                                                <
                                                    MonoTouch.Security.SecRecord,
                                                    Xamarin.Auth.Account
                                                >
                    }    
                    {
                        System.Linq.Enumerable.WhereSelectArrayIterator
                                                <
                                                    Security.SecRecord,
                                                    Xamarin.Auth.Account
                                                >
                    }    
                */
                ienumerable_accounts = records.Select(GetAccountFromRecord);

                /*
                    must check for empty IEnumerable
                    IEnumerable ToList()
                    Value cannot be null.
                    Parameter name: data
                */
                try
                {
                    /*
                        Classic
                        accessing throws
                        > ienumerable_accounts.Count()
                        System.ArgumentNullException: Value cannot be null.
                        Parameter name: data
                        > ienumerable_accounts.LongCount()
                        System.ArgumentNullException: Value cannot be null.
                        Parameter name: data 
                    */
                    if (ienumerable_accounts.Count() > 0 && ienumerable_accounts.LongCount() > 0)
                    {
                        /*
                            Unified enters
                            method call ToList() throws

                            > ienumerable_accounts.Count()
                            System.ArgumentNullException: Value cannot be null.
                            Parameter name: data
                            > ienumerable_accounts.LongCount()
                            System.ArgumentNullException: Value cannot be null.
                            Parameter name: data
                            > ienumerable_accounts.ToList()
                            System.ArgumentNullException: Value cannot be null.
                            Parameter name: data
                        */
                         accounts_found = ienumerable_accounts.ToList();
                    }
                    else
                    {
                        accounts_found = new List<Account> ();
                    }
                }
                catch(System.Exception exc)
                {
                    string msg = exc.Message;
                    Debug.WriteLine("IEnumerable access excption = " + msg);
                }
            }
            else
            {
                accounts_found = new List<Account> ();
            }

			return accounts_found;
		}

		Account GetAccountFromRecord (SecRecord r)
		{
			#if ! TEST_MARK_T
            NSData data = r.Generic;
			#else
            NSData data = r.ValueData;
			#endif

            var serializedData = NSString.FromData (data, NSStringEncoding.UTF8);

            Account a = Account.Deserialize (serializedData); 

			return a;
		}

		Account FindAccount (string username, string serviceId)
		{
			var query = new SecRecord (SecKind.GenericPassword);
			query.Service = serviceId;
			query.Account = username;

			SecStatusCode result;
			var record = SecKeyChain.QueryAsRecord (query, out result);

			return record != null ?	GetAccountFromRecord (record) : null;
		}

		public override void Save (Account account, string serviceId)
		{
			var statusCode = SecStatusCode.Success;
			var serializedAccount = account.Serialize ();
			var data = NSData.FromString (serializedAccount, NSStringEncoding.UTF8);

			//
			// Remove any existing record
			//
			var existing = FindAccount (account.Username, serviceId);

			if (existing != null) {
				var query = new SecRecord (SecKind.GenericPassword);
				query.Service = serviceId;
				query.Account = account.Username;

				statusCode = SecKeyChain.Remove (query);
				if (statusCode != SecStatusCode.Success) {
					throw new Exception ("Could not save account to KeyChain: " + statusCode);
				}
			}

			//
			// Add this record
			//
			var record = new SecRecord (SecKind.GenericPassword);
			record.Service = serviceId;
			record.Account = account.Username;
			//------------------------------------------------------
			// mc++ mc#
			// Mark Taparauskas suggetsion:
			//		.Generic is not encrypted
			#if ! TEST_MARK_T
			record.Generic = data;
			#else
			record.ValueData = data;
			#endif
			//------------------------------------------------------
			record.Accessible = SecAccessible.WhenUnlocked;

			statusCode = SecKeyChain.Add (record);

			if (statusCode != SecStatusCode.Success) {
				throw new Exception ("Could not save account to KeyChain: " + statusCode);
			}
		}

		public override void Delete (Account account, string serviceId)
		{
			var query = new SecRecord (SecKind.GenericPassword);
			query.Service = serviceId;
			query.Account = account.Username;
			
			var statusCode = SecKeyChain.Remove (query);

			if (statusCode != SecStatusCode.Success) {
				throw new Exception ("Could not delete account from KeyChain: " + statusCode);
			}
		}
	}
}

