using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public partial class OAuth2Authenticator : WebRedirectAuthenticator
    {
        [Obsolete]
        private readonly GetUsernameAsyncFunc getUsernameAsync;

        private string requestState = Guid.NewGuid().ToString("N");
        private bool reportedForgery = false;

        public OAuth2Authenticator(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl)
            : this(clientId, null, scope, authorizeUrl, redirectUrl, null, false)
        {
        }

        public OAuth2Authenticator(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl, bool isUsingNativeUI)
            : this(clientId, null, scope, authorizeUrl, redirectUrl, null, false)
        {
        }

        public OAuth2Authenticator(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl)
            : this(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, false)
        {
        }

        public OAuth2Authenticator(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, bool isUsingNativeUI)
            : base(redirectUrl, redirectUrl)
        {
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            ClientSecret = clientSecret;
            Scope = scope ?? string.Empty;
            AuthorizeUrl = authorizeUrl ?? throw new ArgumentNullException(nameof(authorizeUrl));
            AccessTokenUrl = accessTokenUrl;
            IsUsingNativeUI = isUsingNativeUI;
        }

        [Obsolete]
        public OAuth2Authenticator(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl, GetUsernameAsyncFunc getUsernameAsync)
            : this(clientId, null, scope, authorizeUrl, redirectUrl, null, getUsernameAsync, false)
        {
        }

        [Obsolete]
        public OAuth2Authenticator(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl, GetUsernameAsyncFunc getUsernameAsync, bool isUsingNativeUI)
            : this(clientId, null, scope, authorizeUrl, redirectUrl, null, getUsernameAsync, isUsingNativeUI)
        {
        }

        [Obsolete]
        public OAuth2Authenticator(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, GetUsernameAsyncFunc getUsernameAsync)
            : this(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, getUsernameAsync, false)
        {
        }

        [Obsolete]
        public OAuth2Authenticator(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, GetUsernameAsyncFunc getUsernameAsync, bool isUsingNativeUI)
            : this(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, isUsingNativeUI)
        {
            this.getUsernameAsync = getUsernameAsync;
        }

        public bool DoNotEscapeScope { get; set; }

        public string AccessTokenName { get; set; } = "access_token";

        public string Scope { get; private set; }

        public Uri AuthorizeUrl { get; private set; }

        public Uri AccessTokenUrl { get; private set; }

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        protected bool IsImplicitFlow => AccessTokenUrl == null;

        protected bool IsAuthorizationCodeFlow => AccessTokenUrl != null;

        protected bool IsProofKeyCodeForExchange => AccessTokenUrl != null && string.IsNullOrWhiteSpace(ClientSecret);

        public override Task<Uri> GetInitialUrlAsync(Dictionary<string, string> custom_query_parameters = null)
        {
            RequestParameters = CreateRequestQueryParameters(custom_query_parameters);

            OnCreatingInitialUrl(RequestParameters);

            var queryString = string.Join("&", RequestParameters.Select(i => i.Key + "=" + i.Value));
            var url = string.IsNullOrEmpty(queryString) ? AuthorizeUrl : new Uri(AuthorizeUrl.AbsoluteUri + "?" + queryString);

            return Task.FromResult(url);
        }

        public Dictionary<string, string> CreateRequestQueryParameters(Dictionary<string, string> parameters = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", EnsureUriEncodedDataString(ClientId) },
                { "scope", DoNotEscapeScope ? Scope : EnsureUriEncodedDataString(Scope) },
                { "response_type", string.Join(" ", OAuthFlowResponseTypeVerification()) },
                { "state", requestState }
            };
            if (RedirectUrl != null)
                queryParams["redirect_uri"] = Uri.EscapeDataString(RedirectUrl.OriginalString);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    queryParams[param.Key] = EnsureUriEncodedDataString(param.Value);
                }
            }

            return queryParams;
        }

        protected List<string> OAuthFlowResponseTypeVerification()
        {
            List<string> responseTypes = null;

            if (IsImplicitFlow)
            {
                responseTypes = new List<string>
                {
                    Uri.EscapeDataString("token")
                };
            }
            else if (IsAuthorizationCodeFlow)
            {
                responseTypes = new List<string>
                {
                    Uri.EscapeDataString("code")
                };
            }
            else if (IsProofKeyCodeForExchange)
            {
                //
            }
            else
            {
                throw new InvalidOperationException("Unkown response type.");
            }

            return responseTypes;
        }

        protected virtual void OnCreatingInitialUrl(IDictionary<string, string> query)
        {
        }

        protected override void OnPageEncountered(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            // check for forgeries
            if (query.TryGetValue("state", out var state) && state != requestState && !reportedForgery)
            {
                reportedForgery = true;
                OnError("Invalid state from server. Possible forgery!");
                return;
            }

            base.OnPageEncountered(url, query, fragment);
        }

        protected override async void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            // access_token parameter lookup
            if (fragment.ContainsKey("access_token"))
            {
                // access_token found
                OnRetrievedAccountProperties(fragment);
            }
            else if (!IsImplicitFlow)
            {
                // code parameter lookpup
                if (query.TryGetValue("code", out var code))
                {
                    try
                    {
                        var props = await RequestAccessTokenAsync(code).ConfigureAwait(false);
                        OnRetrievedAccountProperties(props);
                    }
                    catch (Exception ex)
                    {
                        OnError(ex);
                    }
                }
                else
                {
                    OnError($"Expected {AccessTokenName} in response, but did not receive one.");
                }
            }
        }

        public Task<IDictionary<string, string>> RequestAccessTokenAsync(string code)
        {
            var queryValues = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUrl.AbsoluteUri },
                { "client_id", ClientId },
            };
            if (!string.IsNullOrEmpty(ClientSecret))
                queryValues["client_secret"] = ClientSecret;

            return RequestAccessTokenAsync(queryValues);
        }

        public async Task<IDictionary<string, string>> RequestAccessTokenAsync(IDictionary<string, string> queryValues)
        {
            using (var content = new FormUrlEncodedContent(queryValues))
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(AccessTokenUrl, content).ConfigureAwait(false);
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // Parse the response
                var data = text.Contains("{") ? WebEx.JsonDecode(text) : WebEx.FormDecode(text);

                if (data.ContainsKey("error"))
                {
                    throw new AuthException($"Error authenticating: {data["error"]}");
                }
                else if (!data.ContainsKey(AccessTokenName))
                {
                    throw new AuthException($"Expected {AccessTokenName} in access token response, but did not receive one.");
                }

                return data;
            }
        }

        public virtual async void OnRetrievedAccountProperties(IDictionary<string, string> accountProperties)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            if (getUsernameAsync != null)
            {
                // Now we just need a username for the account
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
                OnSucceeded(string.Empty, accountProperties);
            }
        }

        public async Task<int> RefreshAccessTokenAsync(string refreshToken)
        {
            var queryValues = new Dictionary<string, string>
            {
                {"refresh_token", refreshToken},
                {"client_id", ClientId},
                {"grant_type", "refresh_token"}
            };

            if (!string.IsNullOrEmpty(ClientSecret))
                queryValues["client_secret"] = ClientSecret;

            var accountProperties = await RequestAccessTokenAsync(queryValues).ConfigureAwait(false);

            OnRetrievedAccountProperties(accountProperties);

            return int.Parse(accountProperties["expires_in"]);
        }
    }
}
