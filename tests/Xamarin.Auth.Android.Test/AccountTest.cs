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
using System;
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Auth;
using System.Threading.Tasks;
using Android.NUnit;

namespace Xamarin.Auth.Android.Test
{
	[TestFixture]
	public class AccountTest
	{
		[Test]
		public void Delete ()
		{
			// Make sure that a new store gets created to avoid key clashes
			TestRunner.Shared.DeleteFile ("Xamarin.Social.Accounts");

			var store = AccountStore.Create (TestRunner.Shared, "delete test");

			// Store a test account
			var account = new Account ("xamarin_delete");
			store.Save (account, "test");

			// Make sure it was stored
			var saccount = store.FindAccountsForService ("test").FirstOrDefault (a => a.Username == "xamarin_delete");
			Assert.That (saccount, Is.Not.Null ());

			// Delete it
			store.Delete (saccount, "test");

			// Make sure it was deleted
			var daccount = store.FindAccountsForService ("test").FirstOrDefault (a => a.Username == "xamarin_delete");
			Assert.That (daccount, Is.Null ());
		}

		[Test]
		public void MigrationSucceeds ()
		{
			// Reset
			TestRunner.Shared.DeleteFile ("Xamarin.Social.Accounts");

			// Create a keystore with the old default password
			var hardCodedPassword = "3295043EA18CA264B2C40E0B72051DEF2D07AD2B4593F43DDDE1515A7EC32617";
			var store = AccountStore.Create (TestRunner.Shared, hardCodedPassword);

			// Store a test account
			var account = new Account ("xamarin_migration");
			store.Save (account, "test");

			// Make sure it was stored
			var saccount = store.FindAccountsForService ("test").FirstOrDefault (a => a.Username == "xamarin_migration");
			Assert.That (saccount, Is.Not.Null ());

			// Now create a new one using a different password, to trigger the migration
			store = AccountStore.Create(TestRunner.Shared, "a different password");

			// We should be able to find the previously stored test account
			saccount = store.FindAccountsForService ("test").FirstOrDefault (a => a.Username == "xamarin_migration");
			Assert.That (saccount, Is.Not.Null ());
		}

		[Test]
		public void MigrationFails ()
		{
			// Reset
			TestRunner.Shared.DeleteFile ("Xamarin.Social.Accounts");

			// Create a keystore with some password
			var store = AccountStore.Create (TestRunner.Shared, "some old password");

			// Store a test account
			var account = new Account ("xamarin_migration");
			store.Save (account, "test");

			// Make sure it was stored
			var saccount = store.FindAccountsForService ("test").FirstOrDefault (a => a.Username == "xamarin_migration");
			Assert.That (saccount, Is.Not.Null ());

			bool exceptionHappened = false;
			try
			{
				// Now create a new one using a different password, to trigger the migration
				// This should lead to an exception, since the hard coded password won't match
				store = AccountStore.Create(TestRunner.Shared, "a different password");
			}
			catch (Java.IO.IOException ex) {
				Assert.That (ex.Message == "KeyStore integrity check failed.");
				exceptionHappened = true;
			}

			Assert.That (exceptionHappened);

			// Check that the migration did not touch the original keystore
			store = AccountStore.Create (TestRunner.Shared, "some old password");

			// Try to read a previously stored key
			saccount = store.FindAccountsForService ("test").FirstOrDefault (a => a.Username == "xamarin_migration");
			Assert.That (saccount, Is.Not.Null ());
		}
	}
}

