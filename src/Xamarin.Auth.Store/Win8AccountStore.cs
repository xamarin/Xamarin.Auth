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
using System.Linq;
using Windows.Security.Cryptography.DataProtection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Auth.Store;

namespace Xamarin.Auth.WindowsStore
{
	internal class WindowsAccountStore : AccountStore
	{
        StorageFolder storage = Windows.Storage.ApplicationData.Current.LocalFolder;

		public override IEnumerable<Account> FindAccountsForService (string serviceId)
		{
            return AsyncHelpers.RunSync<IEnumerable<Account>>(() => FindAccountsForServiceTask(serviceId));
			
		}

		public override void Delete (Account account, string serviceId)
		{
            DeleteTask(account, serviceId).Wait();
		}

		public override void Save (Account account, string serviceId)
		{
            SaveTask(account, serviceId).Wait();
		}

        

		private static string GetAccountPath (Account account, string serviceId)
		{
			return String.Format ("xamarin.auth.{0}.{1}", account.Username, serviceId);
		}

        #region Tasks
        async Task SaveTask(Account account, string serviceId)
        {
            byte[] data = Encoding.UTF8.GetBytes(account.Serialize());
            var protector = new Windows.Security.Cryptography.DataProtection.DataProtectionProvider("Local=user");
            byte[] prot = (await protector.ProtectAsync(data.AsBuffer())).ToArray();

            var path = GetAccountPath(account, serviceId);

            var file = await storage.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
            var writeStream = await file.OpenStreamForWriteAsync() as Stream;
            await writeStream.WriteAsync(BitConverter.GetBytes(prot.Length), 0, sizeof(int));
            await writeStream.WriteAsync(prot, 0, prot.Length);
            writeStream.Flush();
            writeStream.Dispose();
        }

        async Task<IEnumerable<Account>> FindAccountsForServiceTask(string serviceId)
        {
            var allFiles = await storage.GetFilesAsync();

            var authFiles = allFiles.Where(f => f.Name.Contains("xamarin.auth.")).ToArray();

            List<Account> accounts = new List<Account>();

            foreach (var file in authFiles)
            {
                var fas = await file.OpenAsync(FileAccessMode.Read);
                using (Stream str = fas.AsStreamForRead())
                {
                    using (var stream = new BinaryReader(str))
                    {
                        int length = stream.ReadInt32();
                        byte[] data = stream.ReadBytes(length);
                        var protector = new Windows.Security.Cryptography.DataProtection.DataProtectionProvider("Local=user");
                        byte[] unprot = (await protector.UnprotectAsync(data.AsBuffer())).ToArray();
                        accounts.Add( Account.Deserialize(Encoding.UTF8.GetString(unprot, 0, unprot.Length)));
                    }
                }
            }

            return accounts;
        }

        async Task DeleteTask(Account account, string serviceId)
        {
            var path = GetAccountPath(account, serviceId);
            var file = await storage.GetFileAsync(path);
            await file.DeleteAsync();
        }

        #endregion
    }
}