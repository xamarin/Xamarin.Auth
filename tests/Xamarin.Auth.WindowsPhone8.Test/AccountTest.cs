using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Xamarin.Auth.WP8.Test
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void Delete()
        {
            var store =AccountStore.Create();

            // Store a test account
            var account = new Account("xamarin_delete");
            store.Save(account, "test");

            // Make sure it was stored
            var saccount = store.FindAccountsForService("test").FirstOrDefault(a => a.Username == "xamarin_delete");
            Assert.IsNotNull(saccount);

            // Delete it
            store.Delete(saccount, "test");

            // Make sure it was deleted
            var daccount = store.FindAccountsForService("test").FirstOrDefault(a => a.Username == "xamarin_delete");
            Assert.IsNull(daccount);
        }
    }
}
