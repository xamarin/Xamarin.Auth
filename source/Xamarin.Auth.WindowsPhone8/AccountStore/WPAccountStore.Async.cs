//
//  Copyright 2013, Xamarin Inc.
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
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Auth.WindowsPhone
{
    internal partial class WPAccountStore
    {
        public override Task<List<Account>> FindAccountsForServiceAsync (string serviceId)
        {
            var accounts = new List<Account>();
            using (var store = IsolatedStorageFile.GetUserStoreForApplication()) {
                string[] auths = store.GetFileNames ("xamarin.auth.*");
                foreach (string path in auths) {
                    if (!path.EndsWith("." + serviceId)) continue;

                    using (var stream = new BinaryReader (new IsolatedStorageFileStream (path, FileMode.Open, FileAccess.Read, FileShare.Read, store))) {
                        int length = stream.ReadInt32();
                        byte[] data = stream.ReadBytes (length);

                        byte[] unprot = ProtectedData.Unprotect (data, null);
                        accounts.Add(Account.Deserialize (Encoding.UTF8.GetString (unprot, 0, unprot.Length)));
                    }
                }
            }

            return Task.FromResult(accounts);
        }

        public override Task DeleteAsync (Account account, string serviceId)
        {
            var path = GetAccountPath (account, serviceId);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication()) {
                store.DeleteFile (path);
            }

            return Task.FromResult(true);
        }

        public override Task SaveAsync (Account account, string serviceId)
        {
            byte[] data = Encoding.UTF8.GetBytes (account.Serialize());
            byte[] prot = ProtectedData.Protect (data, null);

            var path = GetAccountPath (account, serviceId);

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = new IsolatedStorageFileStream (path, FileMode.Create, FileAccess.Write, FileShare.None, store)) {
                stream.WriteAsync (BitConverter.GetBytes (prot.Length), 0, sizeof (int)).Wait();
                stream.WriteAsync (prot, 0, prot.Length).Wait();
            }

            return Task.FromResult(true);
        }

    }
}