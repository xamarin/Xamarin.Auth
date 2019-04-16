//
//  Copyright 2012-2016, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
#if ! PORTABLE && ! NETFX_CORE
using System.Security.Cryptography;
#endif
using System.Net;
using System.Globalization;
using Xamarin.Utilities;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// A collection of utility functions for signing OAuth 1.0 requests.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal static class OAuth1
    #else
    public static class OAuth1
    #endif
    {
        /// <summary>
        /// Encodes a string according to: http://tools.ietf.org/html/rfc5849#section-3.6
        /// </summary>
        /// <returns>
        /// The encoded string.
        /// </returns>
        /// <param name='unencoded'>
        /// The string to encode.
        /// </param>
        public static string EncodeString(string unencoded)
        {
            var utf8 = Encoding.UTF8.GetBytes(unencoded);
            var sb = new StringBuilder();

            for (var i = 0; i < utf8.Length; i++)
            {
                var v = utf8[i];
                if ((0x41 <= v && v <= 0x5A) || (0x61 <= v && v <= 0x7A) || (0x30 <= v && v <= 0x39) ||
                    v == 0x2D || v == 0x2E || v == 0x5F || v == 0x7E)
                {
                    sb.Append((char)v);
                }
                else
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "%{0:X2}", v);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the signature base string according to: http://tools.ietf.org/html/rfc5849#section-3.4.1
        /// </summary>
        /// <returns>
        /// The signature base string.
        /// </returns>
        /// <param name='method'>
        /// HTTP request method.
        /// </param>
        /// <param name='uri'>
        /// The request resource URI.
        /// </param>
        /// <param name='parameters'>
        /// Parameters covered by: http://tools.ietf.org/html/rfc5849#section-3.4.1.3
        /// </param>
        public static string GetBaseString(string method, Uri uri, IDictionary<string, string> parameters)
        {
            var baseBuilder = new StringBuilder();
            baseBuilder.Append(method);
            baseBuilder.Append("&");
            baseBuilder.Append(EncodeString(uri.AbsoluteUri));
            baseBuilder.Append("&");
            var head = "";
            foreach (var key in parameters.Keys.OrderBy(x => x))
            {
                var p = head + EncodeString(key) + "=" + EncodeString(parameters[key]);
                baseBuilder.Append(EncodeString(p));
                head = "&";
            }
            return baseBuilder.ToString();
        }

        /// <summary>
        /// Gets the signature of a request according to: http://tools.ietf.org/html/rfc5849#section-3.4
        /// </summary>
        /// <returns>
        /// The signature.
        /// </returns>
        /// <param name='method'>
        /// HTTP request method.
        /// </param>
        /// <param name='uri'>
        /// The request resource URI.
        /// </param>
        /// <param name='parameters'>
        /// Parameters covered by: http://tools.ietf.org/html/rfc5849#section-3.4.1.3
        /// </param>
        /// <param name='consumerSecret'>
        /// Consumer secret.
        /// </param>
        /// <param name='tokenSecret'>
        /// Token secret.
        /// </param>
        public static string GetSignature(string method, Uri uri, IDictionary<string, string> parameters, string consumerSecret, string tokenSecret)
        {
#if !PORTABLE && !NETFX_CORE
            var baseString = GetBaseString(method, uri, parameters);
            var key = EncodeString(consumerSecret) + "&" + EncodeString(tokenSecret);
            var hashAlgo = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var hash = hashAlgo.ComputeHash(Encoding.UTF8.GetBytes(baseString));
#else
			var hash = new byte[] { };
			string msg = 
				@"
					TODO: 
					Get-Project Xamarin.Auth.Portable | Install-Package System.Security.Cryptography.Hashing -Pre
					Get-Project Xamarin.Auth.Portable | Install-Package System.Security.Cryptography.Hashing.Algorithms -Pre
				";
			System.Diagnostics.Debug.WriteLine("Xamarin.Auth: " + msg);
            #endif
            var sig = Convert.ToBase64String(hash);

            return sig;
        }

        static Dictionary<string, string> MixInOAuthParameters(string method, Uri url, IDictionary<string, string> parameters, string consumerKey, string consumerSecret, string tokenSecret)
        {
            var ps = new Dictionary<string, string>(parameters);

            var nonce = new Random().Next().ToString();
            var timestamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            ps["oauth_nonce"] = nonce;
            ps["oauth_timestamp"] = timestamp;
            ps["oauth_version"] = "1.0";
            ps["oauth_consumer_key"] = consumerKey;
            ps["oauth_signature_method"] = "HMAC-SHA1";

            var sig = GetSignature(method, url, ps, consumerSecret, tokenSecret);
            ps["oauth_signature"] = sig;

            return ps;
        }

        /// <summary>
        /// Creates an OAuth 1.0 signed request.
        /// </summary>
        /// <returns>
        /// The request.
        /// </returns>
        /// <param name='method'>
        /// HTTP request method.
        /// </param>
        /// <param name='uri'>
        /// The request resource URI.
        /// </param>
        /// <param name='parameters'>
        /// Parameters covered by: http://tools.ietf.org/html/rfc5849#section-3.4.1.3
        /// </param>
        /// <param name='consumerKey'>
        /// Consumer key.
        /// </param>
        /// <param name='consumerSecret'>
        /// Consumer secret.
        /// </param>
        /// <param name='tokenSecret'>
        /// Token secret.
        /// </param>
        public static HttpWebRequest CreateRequest(string method, Uri uri, IDictionary<string, string> parameters, string consumerKey, string consumerSecret, string tokenSecret)
        {
            Dictionary<string, string> ps = MixInOAuthParameters(method, uri, parameters, consumerKey, consumerSecret, tokenSecret);

            string realUrl = uri.AbsoluteUri + "?" + ps.FormEncode();

            HttpWebRequest req = 
                    // (HttpWebRequest)WebRequest.Create(realUrl)   // WebRequest - no go for .NetStandard 1.6
                    HttpWebRequest.CreateHttp(realUrl)
                    ;
            req.Method = method;

            return req;
        }

        /// <summary>
        /// Gets the authorization header for a signed request.
        /// </summary>
        /// <returns>
        /// The authorization header.
        /// </returns>
        /// <param name='method'>
        /// HTTP request method.
        /// </param>
        /// <param name='uri'>
        /// The request resource URI.
        /// </param>
        /// <param name='parameters'>
        /// Parameters covered by: http://tools.ietf.org/html/rfc5849#section-3.4.1.3
        /// </param>
        /// <param name='consumerKey'>
        /// Consumer key.
        /// </param>
        /// <param name='consumerSecret'>
        /// Consumer secret.
        /// </param>
        /// <param name='token'>
        /// Token.
        /// </param>
        /// <param name='tokenSecret'>
        /// Token secret.
        /// </param>
        public static string GetAuthorizationHeader(string method, Uri uri, IDictionary<string, string> parameters, string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            var ps = new Dictionary<string, string>(parameters);
            ps["oauth_token"] = token;
            ps = MixInOAuthParameters(method, uri, ps, consumerKey, consumerSecret, tokenSecret);

            var sb = new StringBuilder();

            var head = "";
            foreach (var p in ps)
            {
                if (p.Key.StartsWith("oauth_"))
                {
                    sb.Append(head);
                    sb.AppendFormat("{0}=\"{1}\"", EncodeString(p.Key), EncodeString(p.Value));
                    head = ",";
                }
            }

            return sb.ToString();
        }
    }
}

