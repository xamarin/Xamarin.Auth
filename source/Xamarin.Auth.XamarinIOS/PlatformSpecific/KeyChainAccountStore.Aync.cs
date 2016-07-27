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

		    List<Account> accounts_found;
		    try
		    {
		        accounts_found = records.Select(GetAccountFromRecord)
                    .ToList();
		    }
		    catch (Exception ex)
		    {
		        do
		        {
		            Debug.WriteLine("IEnumerable access excption = " + ex.Message);
		            Debug.WriteLine(ex);
                } while ((ex = ex.InnerException) != null);

                accounts_found = new List<Account>();
		    }

            return Task.FromResult(accounts_found);
		}

		Account GetAccountFromRecord (SecRecord r)
		{
            //This library used to store passwords in .Generic.
            // Mark Taparauskas suggested using ValueData because it's encrypted,
            // but we need to handle apps upgrading to this version.
            NSData data = r.ValueData;
            if (data == null)
            {
                data = r.Generic;
                if (data != null)
                {
                    //Migrate to ValueData and clear the unencrypted data
                    r.ValueData = data;
                    r.Generic = NSData.FromArray(Array.Empty<byte>());
                    SecKeyChain.Add(r);
                }
            }

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
			record.ValueData = data;
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

