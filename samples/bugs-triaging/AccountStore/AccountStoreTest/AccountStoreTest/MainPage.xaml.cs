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

        }

        void OnLoadClicked(object sender, EventArgs args)
        {

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
                AccountStore.Create().Save(account, AppName);
            }

            return;
        }

        public void LoadCredentials(string userName, string password)
        {
            var account = AccountStore.Create().FindAccountsForService(AppName).FirstOrDefault();
            UserName = (account != null) ? account.Username : null;
            Password = (account != null) ? account.Properties["Password"] : null;

            return;
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, AppName);
            }
        }
    }
}
