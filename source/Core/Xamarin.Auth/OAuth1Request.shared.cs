using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public class OAuth1Request : Request
    {
        private readonly bool includeMultiparts;

        public OAuth1Request(string method, Uri url, Account account)
            : this(method, url, null, account, false)
        {
        }

        public OAuth1Request(string method, Uri url, IDictionary<string, string> parameters, Account account)
            : this(method, url, parameters, account, false)
        {
        }

        public OAuth1Request(string method, Uri url, IDictionary<string, string> parameters, Account account, bool includeMultipartsInSignature)
            : base(method, url, parameters, account)
        {
            includeMultiparts = includeMultipartsInSignature;
        }

        public override Task<Response> GetResponseAsync(CancellationToken cancellationToken = default)
        {
            if (Account == null)
                throw new InvalidOperationException("You must specify an Account for this request to proceed.");

            // Sign the request before getting the response
            var req = GetPreparedWebRequest();

            // Authorize it
            var authorization = GetAuthorizationHeader();

            req.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authorization);

            return base.GetResponseAsync(cancellationToken);
        }

        protected virtual string GetAuthorizationHeader()
        {
            var ps = new Dictionary<string, string>(Parameters);

            if (includeMultiparts)
            {
                foreach (var p in Multiparts)
                {
                    if (!string.IsNullOrEmpty(p.TextData))
                        ps[p.Name] = p.TextData;
                }
            }

            return OAuth1.GetAuthorizationHeader(
                Method,
                Url,
                ps,
                Account.Properties["oauth_consumer_key"],
                Account.Properties["oauth_consumer_secret"],
                Account.Properties["oauth_token"],
                Account.Properties["oauth_token_secret"]);
        }
    }
}
