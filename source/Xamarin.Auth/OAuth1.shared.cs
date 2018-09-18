using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Net;
using System.Globalization;

namespace Xamarin.Auth
{
    public static class OAuth1
    {
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

        public static string GetBaseString(string method, Uri uri, IDictionary<string, string> parameters)
        {
            var baseBuilder = new StringBuilder();
            baseBuilder.Append(method);
            baseBuilder.Append("&");
            baseBuilder.Append(EncodeString(uri.AbsoluteUri));
            baseBuilder.Append("&");

            if (parameters != null)
            {
                var head = "";
                foreach (var key in parameters.Keys.OrderBy(x => x))
                {
                    var p = head + EncodeString(key) + "=" + EncodeString(parameters[key]);
                    baseBuilder.Append(EncodeString(p));
                    head = "&";
                }
            }

            return baseBuilder.ToString();
        }

        public static string GetSignature(string method, Uri uri, IDictionary<string, string> parameters, string consumerSecret, string tokenSecret)
        {
            var baseString = GetBaseString(method, uri, parameters);
            var key = EncodeString(consumerSecret) + "&" + EncodeString(tokenSecret);
            var hashAlgo = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var hash = hashAlgo.ComputeHash(Encoding.UTF8.GetBytes(baseString));
            var sig = Convert.ToBase64String(hash);
            return sig;
        }

        public static HttpWebRequest CreateRequest(string method, Uri uri, IDictionary<string, string> parameters, string consumerKey, string consumerSecret, string tokenSecret)
        {
            var ps = MixInOAuthParameters(method, uri, parameters, consumerKey, consumerSecret, tokenSecret);

            var realUrl = uri.AbsoluteUri + "?" + ps.FormEncode();
            var req = HttpWebRequest.CreateHttp(realUrl);
            req.Method = method;

            return req;
        }

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

        private static Dictionary<string, string> MixInOAuthParameters(string method, Uri url, IDictionary<string, string> parameters, string consumerKey, string consumerSecret, string tokenSecret)
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
    }
}
