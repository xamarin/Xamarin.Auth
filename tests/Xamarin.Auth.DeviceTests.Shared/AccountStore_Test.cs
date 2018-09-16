using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Xamarin.Auth.DeviceTests
{
    public class AccountStore_Test
    {
        [Theory]
        [InlineData("moljac", "test", "AccountStoreTest")]
        [InlineData("moljac", "different", "DifferentStoreTest")]
        [InlineData("mattleibow", "anothertest123", "DifferentStoreTest")]
        public void Save_And_Load(string username, string password, string appname)
        {
            // save an entry
            var account = new Account(username);
            account.Properties.Add("Password", password);
            AccountStore.Create().Save(account, appname);

            // now load it again
            var loadedAccount = AccountStore.Create().FindAccountsForService(appname).FirstOrDefault();
            Assert.Equal(username, account?.Username);
            Assert.Equal(password, account?.Properties["Password"]);

            // delete it
            AccountStore.Create().Delete(loadedAccount, appname);

            // make sure it is gone
            Assert.Empty(AccountStore.Create().FindAccountsForService(appname));
        }
    }
}
