using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Xamarin.Auth
{
    internal partial class UWPAccountStore : AccountStore
    {
        public override IEnumerable<Account> FindAccountsForService(string serviceId)
        {
            return FindAccountsForServiceAsync(serviceId).Result;
        }

        public override void Save(Account account, string serviceId)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SaveAsync(account, serviceId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public override void Delete(Account account, string serviceId)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DeleteAsync(account, serviceId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public override async Task<IEnumerable<Account>> FindAccountsForServiceAsync(string serviceId)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var files = await localFolder.GetFilesAsync().AsTask().ConfigureAwait(false);

            var accounts = new List<Account>();

            foreach (var file in files.Where(x => x.Name.StartsWith("xamarin.auth.") && x.Name.EndsWith("." + serviceId)))
            {
                using (var stream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
                using (var reader = new BinaryReader(stream))
                {
                    var length = reader.ReadInt32();
                    var data = reader.ReadBytes(length);

                    var unprot = await DataProtectionExtensions.UnprotectAsync(data.AsBuffer()).ConfigureAwait(false);
                    var account = Account.Deserialize(Encoding.UTF8.GetString(unprot.ToArray()));
                    accounts.Add(account);
                }
            }

            return accounts;
        }

        public override async Task SaveAsync(Account account, string serviceId)
        {
            var data = Encoding.UTF8.GetBytes(account.Serialize());
            var prot = await DataProtectionExtensions.ProtectAsync(data.AsBuffer()).ConfigureAwait(false);

            var localFolder = ApplicationData.Current.LocalFolder;
            var path = GetAccountPath(account, serviceId);
            var file = await localFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((int)prot.Length);
                writer.Write(prot.ToArray());
            }
        }

        public async Task SaveAsync(Account account, string serviceId, Uri uri)
        {
            var data = Encoding.UTF8.GetBytes(account.Serialize(uri));
            var prot = await DataProtectionExtensions.ProtectAsync(data.AsBuffer()).ConfigureAwait(false);

            var localFolder = ApplicationData.Current.LocalFolder;
            var path = GetAccountPath(account, serviceId);
            var file = await localFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((int)prot.Length);
                writer.Write(prot.ToArray());
            }
        }

        public override async Task DeleteAsync(Account account, string serviceId)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var path = GetAccountPath(account, serviceId);
                var file = await localFolder.GetFileAsync(path).AsTask().ConfigureAwait(false);
                await file.DeleteAsync().AsTask().ConfigureAwait(false);
            }
            catch
            {
                // Ignore this error if file doesn't exist
            }
        }

        private static string GetAccountPath(Account account, string serviceId)
        {
            return $"xamarin.auth.{account.Username}.{serviceId}";
        }
    }
}
