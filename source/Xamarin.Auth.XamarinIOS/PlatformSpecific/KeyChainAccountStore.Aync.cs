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
#define TEST_MARK_T

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;


#if ! __UNIFIED__
using MonoTouch.Security;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.CoreFoundation;
#else
using Security;
using Foundation;
using ObjCRuntime;
using CoreFoundation;
#endif

namespace Xamarin.Auth
{
	internal partial class KeyChainAccountStore : AccountStore
	{
		static Lazy <System.Reflection.MethodInfo> SecRecord_queryDictGetter = new Lazy<System.Reflection.MethodInfo> (() => {
			return typeof (SecRecord).GetProperty ("queryDict", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetMethod;
			});

		static Lazy <IntPtr> Security_ReturnData = new Lazy<IntPtr> (() => {
			return (IntPtr)typeof (SecRecord).Assembly.GetTypes ()
						.First (t => t.Name == "SecItem" 
						&& t.Namespace == (typeof (SecRecord)).Namespace /*"[MonoTouch.]Security"*/
						).GetProperty ("ReturnData").GetMethod.Invoke (null, new Object [] {});
			});

		static Lazy <INativeObject> CFBoolean_True = new Lazy<INativeObject> (() => {
			return typeof (SecRecord).Assembly.GetTypes ()
						.First (t => t.Name == "CFBoolean" 
						&& t.Namespace == (typeof (CFObject)).Namespace /* "[MonoTouch.]CoreFoundation" */
						).GetField ("True").GetValue (null) as INativeObject;
			});

		public override Task<List<Account>> FindAccountsForServiceAsync (string serviceId)
        {
            var query = new SecRecord(SecKind.GenericPassword);
            query.Service = serviceId;

			// Workaround for https://bugzilla.xamarin.com/show_bug.cgi?id=29977
			var queryDict = SecRecord_queryDictGetter.Value.Invoke (query, new object [] {}) as NSMutableDictionary;
			queryDict.LowlevelSetObject (CFBoolean_True.Value.Handle, Security_ReturnData.Value);

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

			List<Account> retval = new List<Account> (accounts_found);

			return Task.FromResult(retval);
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

		public override Task SaveAsync (Account account, string serviceId)
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

			return Task.FromResult (true);
		}

		public override Task DeleteAsync (Account account, string serviceId)
		{
			var query = new SecRecord (SecKind.GenericPassword);
			query.Service = serviceId;
			query.Account = account.Username;
			
			var statusCode = SecKeyChain.Remove (query);

			if (statusCode != SecStatusCode.Success) {
				throw new Exception ("Could not delete account from KeyChain: " + statusCode);
			}

			return Task.FromResult (true);
		}
	}
}

