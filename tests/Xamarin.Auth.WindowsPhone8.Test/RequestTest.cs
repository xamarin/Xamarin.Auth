using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace Xamarin.Auth.WP8.Test
{
    [TestClass]
    public class RequestTest
    {
        [TestMethod]
        public void Ctor()
        {
            const string method = "POST";
            Uri uri = new Uri("http://xamarin.com");

            var ps = new Dictionary<string, string> { { "foo", "bar " } };

            var account = new Account("username");

            var request = new Request(method, uri, ps, account);

            Assert.AreEqual<string>(request.Method,method);
            Assert.AreEqual<Uri>(request.Url, uri);

            Assert.AreSame(request.Account, account);
        }
    }
}
