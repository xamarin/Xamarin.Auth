using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
	public class WindowsPhoneAccountStore : AccountStore
	{
		public override IEnumerable<Account> FindAccountsForService(string serviceId)
		{
			using (var store = IsolatedStorageFile.GetUserStoreForApplication())
			{
				string[] auths = store.GetFileNames("xamarin.auth.*");
				foreach (string path in auths)
				{
					using (var stream = new BinaryReader(new IsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, store)))
					{
						int length = stream.ReadInt32();
						byte[] data = stream.ReadBytes(length);

						byte[] unprot = ProtectedData.Unprotect(data, null);
						yield return Account.Deserialize(Encoding.UTF8.GetString(unprot, 0, unprot.Length));
					}
				}
			}
		}

		public override void Save(Account account, string serviceId)
		{
			byte[] data = Encoding.UTF8.GetBytes(account.Serialize());
			byte[] prot = ProtectedData.Protect(data, null);

			var path = GetAccountPath(account, serviceId);

			using (var store = IsolatedStorageFile.GetUserStoreForApplication())
			using (var stream = new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, store))
			{
				stream.WriteAsync(BitConverter.GetBytes(prot.Length), 0, sizeof(int)).Wait();
				stream.WriteAsync(prot, 0, prot.Length).Wait();
			}
		}

		public override void Delete(Account account, string serviceId)
		{
			var path = GetAccountPath(account, serviceId);
			using (var store = IsolatedStorageFile.GetUserStoreForApplication())
			{
				store.DeleteFile(path);
			}
		}

		private static string GetAccountPath(Account account, string serviceId)
		{
			return String.Format("xamarin.auth.{0}.{1}", account.Username, serviceId);
		}
	}
}
