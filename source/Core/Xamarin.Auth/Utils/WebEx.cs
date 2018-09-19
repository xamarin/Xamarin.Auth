using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Xamarin.Auth
{
    internal static class WebEx
    {
        public static string GetCookie(this CookieContainer containers, Uri domain, string name)
        {
            return containers
                .GetCookies(domain)
                .Cast<Cookie>()
                .FirstOrDefault(x => x.Name == name)
                ?.Value ?? string.Empty;
        }

        public static Encoding GetEncodingFromContentType(string contentType)
        {
            // TODO: Parse the Content-Type

            return Encoding.UTF8;
        }

        public static string GetResponseText(this WebResponse response)
        {
            var encoding = GetEncodingFromContentType(response.ContentType);

            using (var s = response.GetResponseStream())
            using (var r = new StreamReader(s, encoding))
            {
                return r.ReadToEnd();
            }
        }

        public static Task<WebResponse> GetResponseAsync(this WebRequest request)
        {
            return Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
        }

        public static IDictionary<string, string> FormDecode(string encodedString)
        {
            var inputs = new Dictionary<string, string>();

            if (encodedString.Length > 0)
            {
                var firstChar = encodedString[0];
                if (firstChar == '?' || firstChar == '#')
                    encodedString = encodedString.Substring(1);

                if (encodedString.Length > 0)
                {
                    var parts = encodedString.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in parts)
                    {
                        var equalsIndex = part.IndexOf('=');

                        string key;
                        string value;
                        if (equalsIndex >= 0)
                        {
                            key = Uri.UnescapeDataString(part.Substring(0, equalsIndex));
                            value = Uri.UnescapeDataString(part.Substring(equalsIndex + 1));
                        }
                        else
                        {
                            key = Uri.UnescapeDataString(part);
                            value = string.Empty;
                        }

                        inputs[key] = value;
                    }
                }
            }

            return inputs;
        }

        public static Dictionary<string, string> JsonDecode(string encodedString)
        {
            var inputs = new Dictionary<string, string>();
            var json = JObject.Parse(encodedString);

            foreach (var kv in json)
            {
                var v = kv.Value;
                if (v != null)
                    inputs[kv.Key] = (string)v;
            }

            return inputs;
        }

        public static string FormEncode(this IDictionary<string, string> inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));

            var sb = new StringBuilder();
            var head = "";
            foreach (var p in inputs)
            {
                sb.Append(head);
                sb.Append(EncodeString(p.Key));
                sb.Append("=");
                sb.Append(EncodeString(p.Value));
                head = "&";
            }
            return sb.ToString();
        }

        public static string EncodeString(string unencoded)
        {
            if (unencoded == null)
                throw new ArgumentNullException(nameof(unencoded));

            var utf8 = Encoding.UTF8.GetBytes(unencoded);
            var sb = new StringBuilder();

            for (var i = 0; i < utf8.Length; i++)
            {
                var v = utf8[i];
                if ((0x41 <= v && v <= 0x5A) || (0x61 <= v && v <= 0x7A) || (0x30 <= v && v <= 0x39) ||
                    v == 0x2D || v == 0x2E || v == 0x5F || v == 0x7E)
                    sb.Append((char)v);
                else
                    sb.AppendFormat(CultureInfo.InvariantCulture, "%{0:X2}", v);
            }

            return sb.ToString();
        }
    }
}
