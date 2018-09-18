using System;
using System.Collections.Generic;
using Xunit;

namespace Xamarin.Auth.DeviceTests
{
    public class OAuth1_Test
    {
        private const string UriString = "https://xamarin.com";

        [Fact]
        public void No_Parameters()
        {
            var expected = $"GET&{OAuth1.EncodeString(UriString)}&";

            var actual = OAuth1.GetBaseString("GET", new Uri(UriString), null);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void One_Parameter()
        {
            var parameter = $"{OAuth1.EncodeString("key")}={OAuth1.EncodeString("value")}";
            var expected = $"GET&{OAuth1.EncodeString(UriString)}&{OAuth1.EncodeString(parameter)}";

            var parameters = new Dictionary<string, string>
            {
                { "key", "value" }
            };
            var actual = OAuth1.GetBaseString("GET", new Uri(UriString), parameters);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiple_Parameter()
        {
            var parameter1 = $"{OAuth1.EncodeString("key")}={OAuth1.EncodeString("value")}";
            var parameter2 = $"{OAuth1.EncodeString("another")}={OAuth1.EncodeString("something")}";
            var expected = $"GET&{OAuth1.EncodeString(UriString)}&{OAuth1.EncodeString($"{parameter2}&{parameter1}")}";

            var parameters = new Dictionary<string, string>
            {
                { "key", "value" },
                { "another", "something" },
            };
            var actual = OAuth1.GetBaseString("GET", new Uri(UriString), parameters);

            Assert.Equal(expected, actual);
        }
    }
}
