using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Xamarin.Utilities;

namespace Xamarin.Auth.WP8.Test
{
    [TestClass]
    public class WebExTest
    {
        [TestMethod]
        public void JsonDecodeTest()
        {
            JsonDecode("{\"string\": \"value\"}", "string", "value");
            JsonDecode("{\"int\": 5000}", "int", "5000");
            JsonDecode("{\"bool\": true }", "bool", "true");
        }

        void JsonDecode(string json, string arg, string value)
        {
            var dict = WebEx.JsonDecode(json);
            string v;
            Assert.IsTrue(dict.TryGetValue(arg, out v), "Dictionary did not contain argument '" + arg + "'");
            Assert.AreEqual<string>(v,value);
        }
    }
}
