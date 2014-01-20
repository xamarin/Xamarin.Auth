using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace Xamarin.Auth.WP8.Test
{
    [TestClass]
    public class OAuth1Test
    {
        [TestMethod]
        public void GetBaseString()
        {
            var baseString = OAuth1.GetBaseString(
                "GET",
                new Uri("http://www.flickr.com/services/oauth/request_token"),
                new Dictionary<string, string>() {
					{ "oauth_nonce", "89601180" },
					{ "oauth_timestamp", "1305583298" },
					{ "oauth_consumer_key", "653e7a6ecc1d528c516cc8f92cf98611" },
					{ "oauth_signature_method", "HMAC-SHA1" },
					{ "oauth_version", "1.0" },
					{ "oauth_callback", "http://www.example.com" },
				});
            Assert.AreEqual<string>(
                baseString,"GET&http%3A%2F%2Fwww.flickr.com%2Fservices%2Foauth%2Frequest_token&oauth_callback%3Dhttp%253A%252F%252Fwww.example.com%26oauth_consumer_key%3D653e7a6ecc1d528c516cc8f92cf98611%26oauth_nonce%3D89601180%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1305583298%26oauth_version%3D1.0");
        }

        [TestMethod]
        public void GetSignature()
        {
            var sig = OAuth1.GetSignature(
                "GET",
                new Uri("http://www.flickr.com/services/oauth/request_token"),
                new Dictionary<string, string>() {
					{ "oauth_nonce", "95613465" },
					{ "oauth_timestamp", "1305586162" },
					{ "oauth_consumer_key", "653e7a6ecc1d528c516cc8f92cf98611" },
					{ "oauth_signature_method", "HMAC-SHA1" },
					{ "oauth_version", "1.0" },
					{ "oauth_callback", "http://www.example.com" },
				},
                "34bf6099d244db6a",
                "");
            Assert.AreEqual<string>(
                sig,
                "gICkK2bXRMKVYCvwX8bGsC/QbKY=");
        }
    }
}
