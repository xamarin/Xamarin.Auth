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
    public class TestUri
    {
        [Test()]
        public void TestAbsoluteVSOriginal()
        {
            Uri uri_01 = new Uri("http://xamarin.com/");
            Uri uri_02 = new Uri("http://xamarin.com");

            if (uri_01 == uri_02)
            {
                Console.WriteLine(" uri_01 == uri_02");
            }

            Console.WriteLine($" uri_01.Absolute       = {uri_01.AbsoluteUri}");
            Console.WriteLine($" uri_02.Absolute       = {uri_02.AbsoluteUri}");
            Console.WriteLine($" uri_01.OriginalString = {uri_01.OriginalString}");
            Console.WriteLine($" uri_02.OriginalString = {uri_02.OriginalString}");

            /*
             uri_01 == uri_02
             uri_01.Absolute       = http://xamarin.com/
             uri_02.Absolute       = http://xamarin.com/
             uri_01.OriginalString = http://xamarin.com/
             uri_02.OriginalString = http://xamarin.com
            */  
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
