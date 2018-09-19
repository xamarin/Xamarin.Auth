using Android.Content;
using Java.IO;
using Java.Security;
using Javax.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    internal partial class AndroidAccountStore : AccountStore
    {
        private const string FileName = "Xamarin.Social.Accounts";

        // NB: security hole! Left for backwards compatibility
        private static readonly char[] PasswordHardCoded = "3295043EA18CA264B2C40E0B72051DEF2D07AD2B4593F43DDDE1515A7EC32617".ToCharArray();

        private static readonly object fileLock = new object();

        private Context storeContext;
        private KeyStore keystore;
        private KeyStore.PasswordProtection protection;
        private char[] keystorePassword;

        [Obsolete("Use AndroidAccountStore(string) or AndroidAccountStore(char[]) instead.")]
        public AndroidAccountStore(Context context)
            : this(context, PasswordHardCoded)
        {
        }

        public AndroidAccountStore(Context context, string password)
            : this(context, password?.ToCharArray())
        {
        }

        public AndroidAccountStore(Context context, char[] password)
        {
            storeContext = context ?? throw new ArgumentNullException(nameof(context));
            keystorePassword = password ?? throw new ArgumentNullException(nameof(password));

            keystore = KeyStore.GetInstance(KeyStore.DefaultType);
            protection = new KeyStore.PasswordProtection(keystorePassword);

            try
            {
                lock (fileLock)
                {
                    if (!FileExists(storeContext, FileName))
                    {
                        LoadEmptyKeyStore(keystorePassword);
                    }
                    else
                    {
                        using (var s = storeContext.OpenFileInput(FileName))
                        {
                            keystore.Load(s, keystorePassword);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                LoadEmptyKeyStore(keystorePassword);
            }
            catch (IOException ex)
            {
                if (ex.Message == "KeyStore integrity check failed.")
                {
                    // Migration scenario: this exception means that the keystore could not be opened
                    // with the app provided password, so there is probably an existing keystore
                    // that was encoded with the old hard coded password, which was deprecated.
                    // We'll try to open the keystore with the old password, and migrate the contents
                    // to a new one that will be encoded with the new password.
                    MigrateKeyStore(storeContext);
                }
            }
        }

        public override IEnumerable<Account> FindAccountsForService(string serviceId)
        {
            var accounts = new List<Account>();

            var postfix = "-" + serviceId;

            var aliases = keystore.Aliases();
            while (aliases.HasMoreElements)
            {
                var alias = aliases.NextElement().ToString();
                if (alias.EndsWith(postfix))
                {
                    if (keystore.GetEntry(alias, protection) is KeyStore.SecretKeyEntry entry)
                    {
                        var bytes = entry.SecretKey.GetEncoded();
                        var serialized = System.Text.Encoding.UTF8.GetString(bytes);
                        var acct = Account.Deserialize(serialized);
                        accounts.Add(acct);
                    }
                }
            }

            accounts.Sort((a, b) => a.Username.CompareTo(b.Username));

            return accounts;
        }

        public override void Save(Account account, string serviceId)
        {
            var alias = MakeAlias(account, serviceId);

            var secretKey = new SecretAccount(account);
            var entry = new KeyStore.SecretKeyEntry(secretKey);

            keystore.SetEntry(alias, entry, protection);

            Save();
        }

        public override void Delete(Account account, string serviceId)
        {
            var alias = MakeAlias(account, serviceId);

            keystore.DeleteEntry(alias);

            Save();
        }

        public override Task<IEnumerable<Account>> FindAccountsForServiceAsync(string serviceId)
        {
            return Task.FromResult(FindAccountsForService(serviceId));
        }

        public override Task SaveAsync(Account account, string serviceId)
        {
            Save(account, serviceId);
            return Task.CompletedTask;
        }

        public override Task DeleteAsync(Account account, string serviceId)
        {
            Delete(account, serviceId);
            return Task.CompletedTask;
        }

        private void Save()
        {
            lock (fileLock)
            {
                using (var s = storeContext.OpenFileOutput(FileName, FileCreationMode.Private))
                {
                    keystore.Store(s, keystorePassword);
                    s.Flush();
                    s.Close();
                }
            }
        }

        static string MakeAlias(Account account, string serviceId)
        {
            return account.Username + "-" + serviceId;
        }

        private class SecretAccount : Java.Lang.Object, ISecretKey
        {
            private readonly byte[] bytes;

            public SecretAccount(Account account)
            {
                bytes = Encoding.UTF8.GetBytes(account.Serialize());
            }

            public byte[] GetEncoded() => bytes;

            public string Algorithm => "RAW";

            public string Format => "RAW";
        }

        private void LoadEmptyKeyStore(char[] password)
        {
            keystore.Load(null, password);
        }

        private bool FileExists(Context context, string filename)
        {
            var file = context.GetFileStreamPath(filename);
            return file?.Exists() == true;
        }

        private void MigrateKeyStore(Context context)
        {
            // Moves aside the old keystore, opens it with the old hard coded password
            // and copies all entries to the new keystore, secured with the app provided password

            lock (fileLock)
            {
                // First: attempt to open the keystore with the old password
                // If that succeeds, the store can be migrated
                lock (fileLock)
                {
                    using (var s = context.OpenFileInput(FileName))
                    {
                        keystore.Load(s, PasswordHardCoded);
                    }
                }

                MoveKeyStoreFile(context, FileName, FileName + "Old");
                LoadEmptyKeyStore(keystorePassword);
                CopyKeyStoreContents();

                context.DeleteFile(FileName + "Old");
            }
        }

        private void MoveKeyStoreFile(Context context, string source, string destination)
        {
            using (var input = context.OpenFileInput(source))
            using (var output = context.OpenFileOutput(destination, FileCreationMode.Private))
            {
                input.CopyTo(output);
            }

            context.DeleteFile(FileName);
        }

        private void CopyKeyStoreContents()
        {
            var oldKeyStore = KeyStore.GetInstance(KeyStore.DefaultType);
            var oldProtection = new KeyStore.PasswordProtection(PasswordHardCoded);

            using (var s = storeContext.OpenFileInput(FileName + "Old"))
            {
                oldKeyStore.Load(s, PasswordHardCoded);

                // Copy all aliases to a new keystore, using a different password
                var aliases = oldKeyStore.Aliases();
                while (aliases.HasMoreElements)
                {
                    var alias = aliases.NextElement().ToString();
                    var e = oldKeyStore.GetEntry(alias, oldProtection) as KeyStore.SecretKeyEntry;
                    keystore.SetEntry(alias, e, protection);
                }
            }

            Save();
        }
    }
}
