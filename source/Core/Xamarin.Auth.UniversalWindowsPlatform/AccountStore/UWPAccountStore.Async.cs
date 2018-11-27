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
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using Windows.Security.Cryptography;
using Windows.Storage;

namespace Xamarin.Auth.WindowsUWP
{
    internal partial class UWPAccountStore
    {
        public override async Task<List<Account>> FindAccountsForServiceAsync(string serviceId)
        {
            if (serviceId?.Length == 0)
                throw new ArgumentNullException(nameof(serviceId));

            var localFolder = ApplicationData.Current.LocalFolder;

            var files = await localFolder.GetFilesAsync().AsTask().ConfigureAwait(false);

            var names = files.Select(x => x.Name);
            var accounts = new List<Account>();

            foreach (var file in files.Where(x => x.Name.StartsWith("xamarin.auth.") &&
                                                  x.Name.EndsWith("." + serviceId))
                                      .ToList())
            {
                using (var stream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
                using (var reader = new BinaryReader(stream))
                {
                    int length = reader.ReadInt32();
                    byte[] data = reader.ReadBytes(length);

                    byte[] unprot = (await DataProtectionExtensions.UnprotectAsync(data.AsBuffer()).ConfigureAwait(false)).ToArray();
                    accounts.Add(Account.Deserialize(Encoding.UTF8.GetString(unprot, 0, unprot.Length)));
                }
            }

            return accounts;
        }

        public override async Task DeleteAsync(Account account, string serviceId)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (account.Username?.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(account), "Username cannot be empty");
            if (serviceId?.Length == 0)
                throw new ArgumentNullException(nameof(serviceId));

            var path = GetAccountPath(account, serviceId);
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync(path).AsTask().ConfigureAwait(false);
                await file.DeleteAsync().AsTask().ConfigureAwait(false);
            }
            catch
            {
                // Ignore this error if file doesn't exist
            }
        }

        public override Task SaveAsync(Account account, string serviceId)
        {
            return SaveAsync(account, serviceId, null);
        }

        public async Task SaveAsync(Account account, string serviceId, Uri uri)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (account.Username?.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(account), "Username cannot be empty");
            if (serviceId?.Length == 0)
                throw new ArgumentNullException(nameof(serviceId));

            byte[] data = Encoding.UTF8.GetBytes(uri == null ? account.Serialize() : account.Serialize(uri));
            byte[] prot = (await DataProtectionExtensions.ProtectAsync(data.AsBuffer()).ConfigureAwait(false)).ToArray();

            var path = GetAccountPath(account, serviceId);

            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((Int32)prot.Length);
                writer.Write(prot);
            }
        }

        private static string GetAccountPath(Account account, string serviceId)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (account.Username?.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(account), "Username cannot be empty");
            if (serviceId?.Length == 0)
                throw new ArgumentNullException(nameof(serviceId));

            string fileName = account.Username;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            return $"xamarin.auth.{fileName}.{serviceId}";
        }
    }
}
