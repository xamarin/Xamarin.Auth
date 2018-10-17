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
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Xamarin.Auth.WindowsUWP
{
    internal partial class DataContainerAccountStore
    {
        public override Task DeleteAsync(Account account, string serviceId)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (account.Username?.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(account), "Username cannot be empty");

            ApplicationDataContainer container = GetDataContainer(serviceId);
            if (container.Values.ContainsKey(account.Username))
                container.Values.Remove(account.Username);

            return Task.CompletedTask;
        }

        public override async Task<List<Account>> FindAccountsForServiceAsync(string serviceId)
        {
            ApplicationDataContainer container = GetDataContainer(serviceId);
            List<Account> accounts = new List<Account>();
            foreach (byte[] data in container.Values.Values.ToArray())
            {
                byte[] unprot = (await DataProtectionExtensions.UnprotectAsync(data.AsBuffer()).ConfigureAwait(false)).ToArray();
                accounts.Add(Account.Deserialize(Encoding.UTF8.GetString(unprot, 0, unprot.Length)));
            }

            return accounts;
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

            ApplicationDataContainer container = GetDataContainer(serviceId);

            byte[] data = Encoding.UTF8.GetBytes(uri == null ? account.Serialize() : account.Serialize(uri));
            byte[] prot = (await DataProtectionExtensions.ProtectAsync(data.AsBuffer())).ToArray();

            container.Values[account.Username] = prot;
        }

        private static ApplicationDataContainer GetDataContainer(string serviceId)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string name = $"xamarin.auth.accountstore.{serviceId}";

            if (!localSettings.Containers.ContainsKey(name))
                localSettings.CreateContainer(name, ApplicationDataCreateDisposition.Always);
            return localSettings.Containers[name];
        }
    }
}
