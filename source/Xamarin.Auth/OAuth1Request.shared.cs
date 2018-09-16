using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using Xamarin.Auth;

namespace Xamarin.Auth
{
    /// <summary>
    /// Request that is authenticated using an account retrieved from an <see cref="OAuth1Authenticator"/>.
    /// </summary>
    public class OAuth1Request : Request
    {
        bool includeMultipartsInSignature;

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Auth.OAuth1Request"/> class.
        /// </summary>
        /// <param name='method'>
        /// The HTTP method.
        /// </param>
        /// <param name='url'>
        /// The URL.
        /// </param>
        /// <param name='parameters'>
        /// Parameters that will pre-populate the <see cref="Xamarin.Auth.Request.Parameters"/> property or <see langword="null"/>.
        /// </param>
        /// <param name='account'>
        /// The account used to authenticate this request.
        /// </param>
        /// <param name='includeMultipartsInSignature'>
        /// If set to <see langword="true"/> include multiparts when calculating the OAuth 1.0 signature.
        /// </param>
        public OAuth1Request(string method, Uri url, IDictionary<string, string> parameters, Account account, bool includeMultipartsInSignature = false)
            : base(method, url, parameters, account)
        {
            this.includeMultipartsInSignature = includeMultipartsInSignature;
        }

        /// <summary>
        /// Asynchronously gets the response.
        /// </summary>
        /// <returns>
        /// The response.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="Account"/> is <c>null</c>.</exception>
        public override Task<Response> GetResponseAsync(CancellationToken cancellationToken)
        {
            //
            // Make sure we have an account
            //
            if (Account == null)
            {
                throw new InvalidOperationException("You must specify an Account for this request to proceed");
            }

            //
            // Sign the request before getting the response
            //
            var req = GetPreparedWebRequest();

            //
            // Authorize it
            //
            var authorization = GetAuthorizationHeader();

            req.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authorization);

            return base.GetResponseAsync(cancellationToken);
        }

        /// <summary>
        /// Gets OAuth authorization header.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Make sure that the parameters array contains mulitpart keys if we're dealing with a buggy
        /// OAuth implementation (such as Flickr).
        /// </para>
        /// <para>
        /// These normally shouldn't be included: http://tools.ietf.org/html/rfc5849#section-3.4.1.3.1
        /// </para>
        /// </remarks>
        protected virtual string GetAuthorizationHeader()
        {

            var ps = new Dictionary<string, string>(Parameters);
            if (includeMultipartsInSignature)
            {
                foreach (var p in Multiparts)
                {
                    if (!string.IsNullOrEmpty(p.TextData))
                    {
                        ps[p.Name] = p.TextData;
                    }
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

