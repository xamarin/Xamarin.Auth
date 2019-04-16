using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace AccountStoreTest
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            SaveCredentials("moljac", "test");

            return;
		}

        void OnSaveClicked(object sender, EventArgs args)
        {
            SaveCredentials(UserName, Password);

            return;
        }

        void OnLoadClicked(object sender, EventArgs args)
        {
            (string username, string password) data = LoadCredentials(AppName);

            UserName = UserName;
            Password = Password;

            return;
        }

        void OnClearClicked(object sender, EventArgs args)
        {

        }

        public string UserName
        {
            get;
            set;
        } = "user";

        public string Password
        {
            get;
            set;
        } = "password";

        public string AppName
        {
            get;
            set;
        } = "AccountStoreTest";


        public void SaveCredentials(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                AccountStore.Create("StorePasswordUsedOnlyOnAndroid").Save(account, AppName);
            }

            return;
        }

        public (string username, string password) LoadCredentials(string appname)
        {
            var account = AccountStore.Create().FindAccountsForService(appname).FirstOrDefault();
            string u = (account != null) ? account.Username : null;
            string p = (account != null) ? account.Properties["Password"] : null;

            return (username: u, password: p);
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create("StorePasswordUsedOnlyOnAndroid").Delete(account, AppName);
            }
        }
    }
}
