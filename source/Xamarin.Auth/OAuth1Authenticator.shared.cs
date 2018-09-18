using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    [Obsolete]
    public delegate Task<string> GetUsernameAsyncFunc(IDictionary<string, string> accountProperties);

    public class OAuth1Authenticator : WebRedirectAuthenticator
    {
        [Obsolete]
        private readonly GetUsernameAsyncFunc getUsernameAsync;

        private string token;
        private string tokenSecret;
        private string verifier;

        public OAuth1Authenticator(string consumerKey, string consumerSecret, Uri requestTokenUrl, Uri authorizeUrl, Uri accessTokenUrl, Uri callbackUrl)
            : this(consumerKey, consumerSecret, requestTokenUrl, authorizeUrl, accessTokenUrl, callbackUrl, false)
        {
        }

        public OAuth1Authenticator(string consumerKey, string consumerSecret, Uri requestTokenUrl, Uri authorizeUrl, Uri accessTokenUrl, Uri callbackUrl, bool isUsingNativeUI)
            : base(authorizeUrl, callbackUrl)
        {
            ConsumerKey = consumerKey ?? throw new ArgumentException("consumerKey must be provided", nameof(consumerKey));
            ConsumerSecret = consumerSecret ?? throw new ArgumentException("consumerSecret must be provided", nameof(consumerSecret));
            RequestTokenUrl = requestTokenUrl ?? throw new ArgumentNullException(nameof(requestTokenUrl));
            AuthorizeUrl = authorizeUrl ?? throw new ArgumentNullException(nameof(authorizeUrl));
            AccessTokenUrl = accessTokenUrl ?? throw new ArgumentNullException(nameof(accessTokenUrl));
            CallbackUrl = callbackUrl ?? throw new ArgumentNullException(nameof(callbackUrl));
            IsUsingNativeUI = isUsingNativeUI;
        }

        [Obsolete]
        public OAuth1Authenticator(string consumerKey, string consumerSecret, Uri requestTokenUrl, Uri authorizeUrl, Uri accessTokenUrl, Uri callbackUrl, GetUsernameAsyncFunc getUsernameAsync)
            : this(consumerKey, consumerSecret, requestTokenUrl, authorizeUrl, accessTokenUrl, callbackUrl, getUsernameAsync, false)
        {
        }

        [Obsolete]
        public OAuth1Authenticator(string consumerKey, string consumerSecret, Uri requestTokenUrl, Uri authorizeUrl, Uri accessTokenUrl, Uri callbackUrl, GetUsernameAsyncFunc getUsernameAsync, bool isUsingNativeUI)
            : this(consumerKey, consumerSecret, requestTokenUrl, authorizeUrl, accessTokenUrl, callbackUrl, isUsingNativeUI)
        {
            this.getUsernameAsync = getUsernameAsync;
        }

        public string ConsumerKey { get; }

        public string ConsumerSecret { get; }

        public Uri RequestTokenUrl { get; }

        public Uri AuthorizeUrl { get; }

        public Uri AccessTokenUrl { get; }

        public Uri CallbackUrl { get; }

        public override async Task<Uri> GetInitialUrlAsync(Dictionary<string, string> parameters = null)
        {
            var req = OAuth1.CreateRequest(
                "GET",
                RequestTokenUrl,
                new Dictionary<string, string> { { "oauth_callback", CallbackUrl.OriginalString } },
                ConsumerKey,
                ConsumerSecret,
                string.Empty);

            var response = await req.GetResponseAsync();
            var content = response.GetResponseText();
            var r = WebEx.FormDecode(content);

            token = r["oauth_token"];
            tokenSecret = r["oauth_token_secret"];

            var paramType = AuthorizeUrl.AbsoluteUri.IndexOf("?") >= 0 ? "&" : "?";
            var url = AuthorizeUrl.AbsoluteUri + paramType + "oauth_token=" + Uri.EscapeDataString(token);
            return new Uri(url);
        }

        public override async void OnPageLoaded(Uri url)
        {
            if (url.Authority == CallbackUrl.Authority && url.AbsolutePath == CallbackUrl.AbsolutePath)
            {
                try
                {
                    var r = WebEx.FormDecode(url.Query);
                    r.TryGetValue("oauth_verifier", out verifier);

                    await GetAccessTokenAsync().ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    OnCancelled();
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }

        private async Task GetAccessTokenAsync()
        {
            RequestParameters = new Dictionary<string, string>
            {
                { "oauth_token", token }
            };

            if (verifier != null)
                RequestParameters["oauth_verifier"] = verifier;

            var req = OAuth1.CreateRequest("GET", AccessTokenUrl, RequestParameters, ConsumerKey, ConsumerSecret, tokenSecret);

            var response = await req.GetResponseAsync().ConfigureAwait(false);
            var content = response.GetResponseText();

            var accountProperties = WebEx.FormDecode(content);

            accountProperties["oauth_consumer_key"] = ConsumerKey;
            accountProperties["oauth_consumer_secret"] = ConsumerSecret;

#pragma warning disable CS0612 // Type or member is obsolete
            if (getUsernameAsync != null)
            {
                try
                {
                    var username = await getUsernameAsync(accountProperties).ConfigureAwait(false);
                    OnSucceeded(username, accountProperties);
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
#pragma warning restore CS0612 // Type or member is obsolete
            else
            {
                OnSucceeded("", accountProperties);
            }
        }
    }
}
