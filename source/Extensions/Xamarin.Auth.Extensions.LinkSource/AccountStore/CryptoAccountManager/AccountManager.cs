using System;
using System.Linq;

#if ! AZURE_MOBILE_SERVICES
using Xamarin.Auth;
using Xamarin.Auth.Cryptography;
#else
using Xamarin.Auth._MobileServices;
using Xamarin.Auth._MobileServices.Cryptography;
#endif

#if !AZURE_MOBILE_SERVICES
namespace Xamarin.Auth.AccountUtilities
#else
namespace Xamarin.Auth._MobileServices.AccountUtilities
#endif
{
    /// <summary>
    /// Account manager.
    /// Evolve16 Training - class from Labs with modifications
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal class AccountManager
    #else
    public class AccountManager
    #endif
    {
        string service_id = "service_id";
        public string ServiceId
        {
            get
            {
                return service_id;
            }
        }

        string key_password = "password";
        public string KeyPassword
        {
            get
            {
                return key_password;
            }
        }

        string key_keymaterial = "keymaterial";
        public string KeyKeyMaterial
        {
            get
            {
                return key_keymaterial;
            }
        }

        string key_salt = "salt";
        public string KeySalt
        {
            get
            {
                return key_salt;
            }
        }

        public AccountManager()
        {
        }

        public AccountManager(string serviceid, string map_key_password, string map_key_keymaterial, string map_key_salt)
        {
            this.service_id = serviceid;
            this.key_password = map_key_password;
            this.key_keymaterial = map_key_keymaterial;
            this.key_salt = map_key_salt;

            return;
        }

        public bool CreateAndSaveAccount(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            byte[] salt = CryptoUtilities.Get256BitSalt();
            byte[] hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(password), salt);

            AccountStore store = AccountStore.Create();
            if (GetAccountFromStore(store, username) != null)
                return false;

            Account account = new Account(username);
            account.Properties.Add(key_password, Convert.ToBase64String(hashedPassword));
            account.Properties.Add(key_salt, Convert.ToBase64String(salt));
            account.Properties.Add(key_keymaterial, Convert.ToBase64String(
                CryptoUtilities.GetAES256KeyMaterial()));

            store.Save(account, service_id);

            return true;
        }

        public bool LoginToAccount(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            AccountStore store = AccountStore.Create();
            Account account = GetAccountFromStore(store, username);
            if (account == null)
                return false;

            byte[] salt, hashedPassword;

            // Upgrade existing passwords to our new format
            if (!account.Properties.ContainsKey(key_salt))
            {
                salt = CryptoUtilities.Get256BitSalt();
                hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(account.Properties[key_password]), salt);
                account.Properties[key_password] = Convert.ToBase64String(hashedPassword);
                account.Properties.Add(key_salt, Convert.ToBase64String(salt));
                store.Save(account, service_id);
            }

            salt = Convert.FromBase64String(account.Properties[key_salt]);
            hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(password), salt);

            return account.Properties[key_password] == Convert.ToBase64String(hashedPassword);
        }

        public Account GetAccount(string username)
        {
            return GetAccountFromStore(AccountStore.Create(), username);
        }

        Account GetAccountFromStore(AccountStore store, string username)
        {
            if (store == null || username == null)
                return null;

            var accounts = store.FindAccountsForService(service_id);
            return accounts.FirstOrDefault(a => a.Username == username);
        }
    }
}