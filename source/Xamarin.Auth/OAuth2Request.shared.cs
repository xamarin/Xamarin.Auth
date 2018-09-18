using System;
using System.Collections.Generic;

namespace Xamarin.Auth
{
    public class OAuth2Request : Request
    {
        public OAuth2Request(string method, Uri url)
            : this(method, url, null, null)
        {
        }

        public OAuth2Request(string method, Uri url, IDictionary<string, string> parameters)
            : this(method, url, parameters, null)
        {
        }

        public OAuth2Request(string method, Uri url, Account account)
            : this(method, url, null, account)
        {
        }

        public OAuth2Request(string method, Uri url, IDictionary<string, string> parameters, Account account)
            : base(method, url, parameters, account)
        {
        }

        public string AccessTokenParameterName { get; set; } = "access_token";

        protected override Uri GetPreparedUrl()
        {
            return GetAuthenticatedUrl(Account, base.GetPreparedUrl(), AccessTokenParameterName);
        }

        public static Uri GetAuthenticatedUrl(Account account, Uri unauthenticatedUrl, string accessTokenParameterName = "access_token")
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (accessTokenParameterName == null)
                throw new ArgumentNullException(nameof(accessTokenParameterName));
            if (unauthenticatedUrl == null)
                throw new ArgumentNullException(nameof(unauthenticatedUrl));
            if (!account.Properties.ContainsKey("access_token"))
                throw new ArgumentException("OAuth2 account is missing required access_token property.", nameof(account));

            var url = unauthenticatedUrl.AbsoluteUri;
            var join = url.Contains("?") ? "&" : "?";
            url += join + accessTokenParameterName + "=" + account.Properties["access_token"];

            return new Uri(url);
        }

        public static string GetAuthorizationHeader(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (!account.Properties.ContainsKey("access_token"))
                throw new ArgumentException("OAuth2 account is missing required access_token property.", nameof(account));

            return "Bearer " + account.Properties["access_token"];
        }
    }
}
