#if XUNIT
//https://xunit.github.io/docs/comparisons.html
using Xunit;

using Test              = Xunit.FactAttribute;

// Aliases
// dummy XUnit does need testFixture
using TestFixture       = System.ObsoleteAttribute;  
using TestFixtureSetUp  = System.ObsoleteAttribute;
#endif

#if NUNIT
using NUnit.Framework;
using Fact= NUnit.Framework.TestAttribute;
#endif

using System;
using System.Threading.Tasks;
using System.Net;

using HolisticWare.Net.Http;

namespace NUnit.Tests
{
    [TestFixture()]
    public class TestOAuth2Http
    {
        [Test()]
        public void TestGetString()
        {
            OAuth2Http oauth2 = new OAuth2Http();
            Task<string> content = oauth2.HttpGetStringAsync("http://xamarin.com");

            string s = content.Result;

            Console.WriteLine("Response: ");
            Console.WriteLine(s);

            return;
        }


        [Test()]
        public void TestGetWebResponse()
        {
            OAuth2Http oauth2 = new OAuth2Http();
            Task<HttpWebResponse> content = oauth2.HttpGetAsync("http://xamarin.com");

            HttpWebResponse r = content.Result;

            Console.WriteLine("Response: ");
            Console.WriteLine(r.Cookies.ToString());

            return;
        }
    }
}
