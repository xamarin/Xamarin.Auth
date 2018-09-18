using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Linq;

#if !__PORTABLE__
using System.Runtime.Serialization;
#endif

#if !WINDOWS_UWP && !__PORTABLE__
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Xamarin.Auth
{
    public class Account
    {
        private const string AccountUsernameKey = "__username__";
        private const string AccountCookiesKey = "__cookies__";

        public Account()
            : this(string.Empty, null, null)
        {
        }

        public Account(string username)
            : this(username, null, null)
        {
        }

        public Account(string username, CookieContainer cookies)
            : this(username, null, cookies)
        {
        }

        public Account(IDictionary<string, string> properties)
            : this(string.Empty, properties, null)
        {
        }

        public Account(string username, IDictionary<string, string> properties)
            : this(username, properties, null)
        {
        }

        public Account(string username, IDictionary<string, string> properties, CookieContainer cookies)
        {
            Username = username;
            Properties = (properties == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(properties);
            Cookies = cookies ?? new CookieContainer();
        }

        public virtual string Username { get; set; }

        public virtual Dictionary<string, string> Properties { get; set; }

        public virtual CookieContainer Cookies { get; set; }

        public string Serialize()
        {
            var sb = new StringBuilder();

            sb.Append(AccountUsernameKey);
            sb.Append("=");
            sb.Append(Uri.EscapeDataString(Username));

            foreach (var p in Properties)
            {
                sb.Append("&");
                sb.Append(Uri.EscapeDataString(p.Key));
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(p.Value));
            }

            if (Cookies.Count > 0)
            {
                sb.Append("&");
                sb.Append(AccountCookiesKey);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(SerializeCookies()));
            }

            return sb.ToString();
        }

        public string Serialize(Uri uri)
        {
            var sb = new StringBuilder();

            sb.Append(AccountUsernameKey);
            sb.Append("=");
            sb.Append(Uri.EscapeDataString(Username));

            foreach (var p in Properties)
            {
                sb.Append("&");
                sb.Append(Uri.EscapeDataString(p.Key));
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(p.Value));
            }

            if (Cookies.Count > 0)
            {
                sb.Append("&");
                sb.Append(AccountCookiesKey);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(SerializeCookies(uri)));
            }

            return sb.ToString();
        }

        public static Account Deserialize(string serializedString)
        {
            var acct = new Account();

            foreach (var p in serializedString.Split('&'))
            {
                var kv = p.Split('=');

                var key = Uri.UnescapeDataString(kv[0]);
                var val = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : "";

                if (key == AccountCookiesKey)
                    acct.Cookies = DeserializeCookies(val);
                else if (key == AccountUsernameKey)
                    acct.Username = val;
                else
                    acct.Properties[key] = val;
            }

            return acct;
        }

        public override string ToString()
        {
            return Serialize();
        }

        private string SerializeCookies()
        {
#if WINDOWS_UWP || __PORTABLE__
            throw new PlatformNotSupportedException();
#else
            using (MemoryStream stream = new MemoryStream())
            {
                var f = new BinaryFormatter();
                f.Serialize(stream, Cookies);
                return Convert.ToBase64String(stream.GetBuffer());
            }
#endif
        }

        private string SerializeCookies(Uri uri)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serialize(Cookies.GetCookies(uri), uri, stream);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private static CookieContainer DeserializeCookies(string cookiesString)
        {
#if WINDOWS_UWP || __PORTABLE__
            throw new PlatformNotSupportedException();
#else
            using (var s = new MemoryStream(Convert.FromBase64String(cookiesString)))
            {
                var f = new BinaryFormatter();
                return (CookieContainer)f.Deserialize(s);
            }
#endif
        }

        private static CookieContainer DeserializeCookies(string cookiesString, Uri uri)
        {
            CookieContainer cc = null;

            using (var s = new MemoryStream(Convert.FromBase64String(cookiesString)))
            {
                cc = Deserialize(s, uri);
            }

            return cc;
        }

        private static void Serialize(CookieCollection cookies, Uri address, Stream stream)
        {
#if __PORTABLE__
            throw new PlatformNotSupportedException();
#else
            var cookieList = new List<Cookie>();
            foreach (var cookie in cookies.OfType<Cookie>())
            {
                cookieList.Add(cookie);
            }

            var formatter = new DataContractSerializer(typeof(List<Cookie>));
            formatter.WriteObject(stream, cookieList);
#endif
        }

        private static CookieContainer Deserialize(Stream stream, Uri uri)
        {
#if __PORTABLE__
            throw new PlatformNotSupportedException();
#else
            var formatter = new DataContractSerializer(typeof(List<Cookie>));
            var cookies = (List<Cookie>)formatter.ReadObject(stream);
            var cc = new CookieCollection();

            foreach (Cookie cookie in cookies)
            {
                cc.Add(cookie);
            }

            var container = new CookieContainer();
            container.Add(uri, cc);
            return container;
#endif
        }
    }
}
