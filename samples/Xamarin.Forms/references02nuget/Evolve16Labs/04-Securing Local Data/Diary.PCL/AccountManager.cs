using Xamarin.Auth;
using System.Linq;
using System;

namespace Diary.Shared
{
	public class AccountManager
	{
		const string serviceID 	= "Diary";
		const string pwKey = "password";
		const string kmKey = "keymaterial";
		const string saltKey = "salt";

 		public bool CreateAndSaveAccount (string username, string password)
		{
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            byte[] salt = CryptoUtilities.Get256BitSalt();
            byte[] hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(password), salt);

			AccountStore store = AccountStore.Create ();
            if (GetAccountFromStore(store, username) != null)
                return false;

            Account account = new Account(username);
            account.Properties.Add(pwKey, Convert.ToBase64String(hashedPassword));
            account.Properties.Add(saltKey, Convert.ToBase64String(salt));
            account.Properties.Add(kmKey, Convert.ToBase64String(
                CryptoUtilities.GetAES256KeyMaterial()));

            store.Save(account, serviceID);

            return true;
		}

		public bool LoginToAccount (string username, string password)
		{
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;
            
			AccountStore store = AccountStore.Create ();
            Account account = GetAccountFromStore(store, username);
            if (account == null)
                return false;

            byte[] salt, hashedPassword;

            // Upgrade existing passwords to our new format
            if (!account.Properties.ContainsKey(saltKey))
            {
                salt = CryptoUtilities.Get256BitSalt();
                hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(account.Properties[pwKey]), salt);
                account.Properties[pwKey] = Convert.ToBase64String(hashedPassword);
                account.Properties.Add(saltKey, Convert.ToBase64String(salt));
                store.Save(account, serviceID);
            }

            salt = Convert.FromBase64String(account.Properties[saltKey]);
            hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(password), salt);

            return account.Properties[pwKey] == Convert.ToBase64String(hashedPassword);
		}
			
		public Account GetAccount (string username)
		{
			return GetAccountFromStore (AccountStore.Create (), username);
		}

		Account GetAccountFromStore (AccountStore store, string username)
		{
			if (store == null || username == null)
				return null;

            var accounts = store.FindAccountsForService(serviceID);
            return accounts.FirstOrDefault(a => a.Username == username);
        }
	}
}