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
using System.Collections.Generic;
using System.Linq;
using Java.Security;
using Javax.Crypto;
using Javax.Security.Auth.Callback;
using Java.IO;
using Android.Content;
using Android.Runtime;

namespace Xamarin.Auth
{
	/// <summary>
	/// AccountStore that uses a KeyStore of PrivateKeys protected by a fixed password
	/// in a private region of internal storage.
	/// </summary>
	internal class AndroidAccountStore : AccountStore
	{
		Context context;
		KeyStore ks;
		KeyStore.PasswordProtection prot;

		static readonly object fileLock = new object ();

		const string FileName = "Xamarin.Social.Accounts";
		static char[] Password;

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.AndroidAccountStore"/> class
		/// with a KeyStore password provided by the application.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="password">KeyStore Password.</param>
		public AndroidAccountStore (Context context, string password)
		{
			if (password == null)
				throw new ArgumentNullException ("password");

			this.context = context;

			Password = password.ToCharArray ();

			ks = KeyStore.GetInstance (KeyStore.DefaultType);

			prot = new KeyStore.PasswordProtection (Password);

			try {
				lock (fileLock) {
					using (var s = context.OpenFileInput (FileName)) {
						ks.Load (s, Password);
					}
				}
			}
			catch (FileNotFoundException) {
				LoadEmptyKeyStore (Password);
			}
			catch (Java.IO.IOException ex) {
				if (ex.Message == "KeyStore integrity check failed.") {
					// Migration scenario: this exception means that the keystore could not be opened
					// with the app provided password, so there is probably an existing keystore
					// that was encoded with the old hard coded password, which was deprecated.
					// We'll try to open the keystore with the old password, and migrate the contents
					// to a new one that will be encoded with the new password.
					MigrateKeyStore (context);
				}
			}
		}

		public override IEnumerable<Account> FindAccountsForService (string serviceId)
		{
			var r = new List<Account> ();

			var postfix = "-" + serviceId;

			var aliases = ks.Aliases ();
			while (aliases.HasMoreElements) {
				var alias = aliases.NextElement ().ToString ();
				if (alias.EndsWith (postfix)) {
					var e = ks.GetEntry (alias, prot) as KeyStore.SecretKeyEntry;
					if (e != null) {
						var bytes = e.SecretKey.GetEncoded ();
						var serialized = System.Text.Encoding.UTF8.GetString (bytes);
						var acct = Account.Deserialize (serialized);
						r.Add (acct);
					}
				}
			}

			r.Sort ((a, b) => a.Username.CompareTo (b.Username));

			return r;
		}

		public override void Save (Account account, string serviceId)
		{
			var alias = MakeAlias (account, serviceId);

			var secretKey = new SecretAccount (account);
			var entry = new KeyStore.SecretKeyEntry (secretKey);
			ks.SetEntry (alias, entry, prot);

			Save();
		}

		public override void Delete (Account account, string serviceId)
		{
			var alias = MakeAlias (account, serviceId);

			ks.DeleteEntry (alias);
			Save();
		}

		void Save ()
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

		#region Migration of key store with hard coded password

		static readonly char[] DefaultPassword = "3295043EA18CA264B2C40E0B72051DEF2D07AD2B4593F43DDDE1515A7EC32617".ToCharArray();

		void MigrateKeyStore (Context context)
		{
			// Moves aside the old keystore, opens it with the old hard coded password
			// and copies all entries to the new keystore, secured with the app provided password

			lock (fileLock) {

				// First: attempt to open the keystore with the old password
				// If that succeeds, the store can be migrated

				lock (fileLock) {
					using (var s = context.OpenFileInput (FileName)) {
						ks.Load (s, DefaultPassword);
					}
				}

				MoveKeyStoreFile (context, FileName, FileName + "Old");

				LoadEmptyKeyStore (Password);

				CopyKeyStoreContents ();

				context.DeleteFile (FileName + "Old");
			}
		}

		void MoveKeyStoreFile(Context context, string source, string destination)
		{
			var input = context.OpenFileInput (source);
			var output = context.OpenFileOutput(destination, FileCreationMode.Private);

			byte[] buffer = new byte[1024];
			int len;
			while ((len = input.Read(buffer, 0, 1024)) > 0) {
				output.Write(buffer, 0, len);
			}
			input.Close();
			output.Close();

			context.DeleteFile (FileName);
		}

		void CopyKeyStoreContents ()
		{
			var oldKeyStore = KeyStore.GetInstance (KeyStore.DefaultType);
			var oldProtection = new KeyStore.PasswordProtection (DefaultPassword);

			using (var s = context.OpenFileInput (FileName + "Old")) {
				oldKeyStore.Load (s, DefaultPassword);
				// Copy all aliases to a new keystore, using a different password
				var aliases = oldKeyStore.Aliases();
				while (aliases.HasMoreElements) {
					var alias = aliases.NextElement ().ToString ();
					var e = oldKeyStore.GetEntry (alias, oldProtection) as KeyStore.SecretKeyEntry;
					ks.SetEntry (alias, e, prot);
				}
			}
			Save ();
		}

		#endregion

	}
}

