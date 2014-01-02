using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace Xamarin.Auth.WP8.Test
{
    [TestClass]
    public class WebUtilitiesTest
    {
        [TestMethod]
        public void FormEncodeInvalid()
        {
            Assert.ThrowsException<ArgumentNullException>(() => WebUtilities.EncodeString(null));
        }

        [TestMethod]
        public void FormEncodeEmpty()
        {
            string result = new Dictionary<string, string>().FormEncode();
            Assert.AreEqual<string>(result, String.Empty);
        }

        [TestMethod]
        public void EncodeStringInvalid()
        {
            Assert.ThrowsException<ArgumentNullException>(() => WebUtilities.FormEncode(null));
        }

        [TestMethod]
        public void EncodeStringEmpty()
        {
            string unencoded = String.Empty;
            string result = WebUtilities.EncodeString(unencoded);
            Assert.AreEqual<string>(result, unencoded);
        }
    }
}
