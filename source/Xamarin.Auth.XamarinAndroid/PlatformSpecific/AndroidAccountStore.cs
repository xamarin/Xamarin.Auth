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
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Java.IO;
using Java.Security;
using Javax.Security.Auth.Callback;
using Javax.Crypto;

namespace Xamarin.Auth
{
	/// <summary>
	/// AccountStore that uses a KeyStore of PrivateKeys protected by a fixed password
	/// in a private region of internal storage.
	/// </summary>
	internal partial class AndroidAccountStore : AccountStore
	{
		Context context;
		KeyStore ks;
		KeyStore.PasswordProtection prot;

		static readonly object fileLock = new object ();

		const string FileName = "Xamarin.Social.Accounts";
		static readonly char[] Password = "3295043EA18CA264B2C40E0B72051DEF2D07AD2B4593F43DDDE1515A7EC32617".ToCharArray ();

		public AndroidAccountStore (Context context)
		{
			this.context = context;

			ks = KeyStore.GetInstance (KeyStore.DefaultType);

			prot = new KeyStore.PasswordProtection (Password);

			try {
				lock (fileLock) {
					if (! System.IO.File.Exists(FileName))
					{
						LoadEmptyKeyStore (Password);
					}
					else
					{
						using (var s = context.OpenFileInput (FileName)) {
							ks.Load (s, Password);
						}
					}
				}
			}
			catch (System.IO.FileNotFoundException) {
				System.Diagnostics.Debug.WriteLine("System.IO.FileNotFoundException caught for AccountStore");
				//ks.Load (null, Password);
				LoadEmptyKeyStore (Password);
			}
			catch (Java.IO.FileNotFoundException) {
				System.Diagnostics.Debug.WriteLine("Java.IO.FileNotFoundException caught for AccountStore");
				//ks.Load (null, Password);
				LoadEmptyKeyStore (Password);
			}
		}

		public override IEnumerable<Account> FindAccountsForService (string serviceId)
		{
			return FindAccountsForServiceAsync(serviceId).Result;
		}

		public override void Save (Account account, string serviceId)
		{
			SaveAsync(account, serviceId);

			return;
		}

		public override void Delete (Account account, string serviceId)
		{
			DeleteAsync(account, serviceId);

			return;
		}

		protected void Save()
		{
			lock (fileLock) {
				using (var s = context.OpenFileOutput (FileName, FileCreationMode.Private)) {
					ks.Store (s, Password);
				}
			}
		}

		static string MakeAlias (Account account, string serviceId)
		{
			return account.Username + "-" + serviceId;
		}

		class SecretAccount : Java.Lang.Object, ISecretKey
		{
			byte[] bytes;
			public SecretAccount (Account account)
			{
				bytes = System.Text.Encoding.UTF8.GetBytes (account.Serialize ());
			}
			public byte[] GetEncoded ()
			{
				return bytes;
			}
			public string Algorithm {
				get {
					return "RAW";
				}
			}
			public string Format {
				get {
					return "RAW";
				}
			}
		}

		static IntPtr id_load_Ljava_io_InputStream_arrayC;

		/// <summary>
		/// Work around Bug https://bugzilla.xamarin.com/show_bug.cgi?id=6766
		/// </summary>
		void LoadEmptyKeyStore (char[] password)
		{
			if (id_load_Ljava_io_InputStream_arrayC == IntPtr.Zero) {
				id_load_Ljava_io_InputStream_arrayC = JNIEnv.GetMethodID (ks.Class.Handle, "load", "(Ljava/io/InputStream;[C)V");
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = JNIEnv.NewArray (password);
			JNIEnv.CallVoidMethod (ks.Handle, id_load_Ljava_io_InputStream_arrayC, new JValue[]
			{
				new JValue (intPtr),
				new JValue (intPtr2)
			});
			JNIEnv.DeleteLocalRef (intPtr);
			if (password != null)
			{
				JNIEnv.CopyArray (intPtr2, password);
				JNIEnv.DeleteLocalRef (intPtr2);
			}
		}
	}
}
