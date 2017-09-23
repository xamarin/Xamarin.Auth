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
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Utilities;
using System.Net;
using System.Text;
using System.Runtime.CompilerServices;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// Implements OAuth 2.0 implicit granting. http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.2
    /// </summary>
#if XAMARIN_AUTH_INTERNAL
    internal partial class OAuth2Authenticator : WebRedirectAuthenticator
#else
    public partial class OAuth2Authenticator : WebRedirectAuthenticator
#endif
    {
        string clientId;
        string clientSecret;
        string scope;
        Uri authorizeUrl;
        Uri redirectUrl;
        Uri accessTokenUrl;
        GetUsernameAsyncFunc getUsernameAsync;


        #region     State
        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the state.
        /// OAuth2 random state string
        /// </summary>
        /// <value>The state.</value>
        public string State
        {
            get
            {
                return request_state;
            }
        }
        string request_state;

        public ulong StateStringLength
        {
            get;
            set;
        } = 16;

        /// <summary>
        /// Gets or sets the OAuth2 random state generator func.
        /// </summary>
        /// <value>
        /// The OA uth2 random state generator func.
        /// </value>
        public Func<ulong, string> OAuth2RandomStateGeneratorFunc
        {
            get;
            set;
        }

        public string GenerateOAuth2StateRandom(ulong number_of_characters = 16)
        {
            //
            // Generate a unique state string to check for forgeries
            //
            var chars = new char[number_of_characters];
            var rand = new Random();
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)rand.Next((int)'a', (int)'z' + 1);
            }
            string state_string = new string(chars);

            return state_string;
        }
        //---------------------------------------------------------------------------------------
        #endregion  State

        bool reportedForgery = false;

        #region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		OAuth2Authenticator changes to work with joind.in OAuth #91
        ///		https://github.com/xamarin/Xamarin.Auth/pull/91
        ///		
        string accessTokenName = "access_token";

        public string AccessTokenName
        {
            get
            {
                return accessTokenName;
            }
            set
            {
                accessTokenName = value;
            }
        }

        /// <summary>
        /// Gets the authorization scope.
        /// </summary>
        /// <value>The authorization scope.</value>
        public string Scope
        {
            get
            {
                return this.scope;
            }
        }

        /// <summary>
        /// Gets the authorize URL.
        /// </summary>
        /// <value>The authorize URL.</value>
        public Uri AuthorizeUrl
        {
            get
            {
                return this.authorizeUrl;
            }
        }

        /// <summary>
        /// Gets the access token URL.
        /// </summary>
        /// <value>The URL used to request access tokens after an authorization code was received.</value>
        public Uri AccessTokenUrl
        {
            get
            {
                return this.accessTokenUrl;
            }
            set
            {
                this.accessTokenUrl = value;
            }
        }
        ///---------------------------------------------------------------------------------------
        #endregion

        #region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		Adding method to request a refresh token #79
        ///		https://github.com/xamarin/Xamarin.Auth/pull/79
        ///		
        ///		those 2 properties were missing from the pull request, but were in the file, so
        ///		added manually.
        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientId
        {
            get { return this.clientId; }
        }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>The client secret.</value>
        public string ClientSecret
        {
            get
            {
                return this.clientSecret;
            }
        }

        protected bool IsImplicitFlow
        {
            get
            {
                return accessTokenUrl == null;
            }
        }

        protected bool IsAuthorizationCodeFlow
        {
            get
            {
                return
                    accessTokenUrl != null                      // AccessToken url is defined
                    &&
                    !string.IsNullOrWhiteSpace(clientSecret)   // Client Secret is defined
                    ;
            }
        }

        protected bool IsProofKeyCodeForExchange
        {
            get
            {
                return
                    accessTokenUrl != null                    // AccessToken url is defined
                    &&
                    string.IsNullOrWhiteSpace(clientSecret)   // Client Secret is not defined
                    ;
            }
        }
        ///---------------------------------------------------------------------------------------
        #endregion


        /// <summary>
        /// Initializes a new <see cref="Xamarin.Auth.OAuth2Authenticator"/>
        /// that authenticates using implicit granting (token).
        /// </summary>
        /// <param name='clientId'>
        /// Client identifier.
        /// </param>
        /// <param name='scope'>
        /// Authorization scope.
        /// </param>
        /// <param name='authorizeUrl'>
        /// Authorize URL.
        /// </param>
        /// <param name='redirectUrl'>
        /// Redirect URL.
        /// </param>
        /// <param name='getUsernameAsync'>
        /// Method used to fetch the username of an account
        /// after it has been successfully authenticated.
        /// </param>
        public OAuth2Authenticator
                        (
                            string clientId,
                            string scope,
                            Uri authorizeUrl,
                            Uri redirectUrl,
                            GetUsernameAsyncFunc getUsernameAsync = null,
                            bool isUsingNativeUI = false
                        )
            : this(redirectUrl)
        {
            this.is_using_native_ui = isUsingNativeUI;

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId must be provided", "clientId");
            }
            this.clientId = clientId;

            this.scope = scope ?? "";

            if (authorizeUrl == null)
            {
                throw new ArgumentNullException("authorizeUrl");
            }
            this.authorizeUrl = authorizeUrl;

            if (redirectUrl == null)
            {
                throw new ArgumentNullException("redirectUrl");
            }
            this.redirectUrl = redirectUrl;

            this.getUsernameAsync = getUsernameAsync;

            this.accessTokenUrl = null;

            #region
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		OAuth2Authenticator changes to work with joind.in OAuth #91
            ///		https://github.com/xamarin/Xamarin.Auth/pull/91
            ///		
            this.RequestParameters = new Dictionary<string, string>();
            ///---------------------------------------------------------------------------------------
            #endregion

#if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"OAuth2Authenticator ");
            sb.AppendLine($"        IsUsingNativeUI = {IsUsingNativeUI}");
            sb.AppendLine($"        redirectUrl = {redirectUrl}");
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            return;
        }

        /// <summary>
        /// Initializes a new instance <see cref="Xamarin.Auth.OAuth2Authenticator"/>
        /// that authenticates using authorization codes (code).
        /// </summary>
        /// <param name='clientId'>
        /// Client identifier.
        /// </param>
        /// <param name='clientSecret'>
        /// Client secret.
        /// </param>
        /// <param name='scope'>
        /// Authorization scope.
        /// </param>
        /// <param name='authorizeUrl'>
        /// Authorize URL.
        /// </param>
        /// <param name='redirectUrl'>
        /// Redirect URL.
        /// </param>
        /// <param name='accessTokenUrl'>
        /// URL used to request access tokens after an authorization code was received.
        /// </param>
        /// <param name='getUsernameAsync'>
        /// Method used to fetch the username of an account
        /// after it has been successfully authenticated.
        /// </param>
        public OAuth2Authenticator
                        (
                            string clientId,
                            string clientSecret,
                            string scope,
                            Uri authorizeUrl,
                            Uri redirectUrl,
                            Uri accessTokenUrl,
                            GetUsernameAsyncFunc getUsernameAsync = null,
                            bool isUsingNativeUI = false
                        )
            : this(redirectUrl, clientSecret, accessTokenUrl)
        {
            this.is_using_native_ui = isUsingNativeUI;

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId must be provided", "clientId");
            }
            this.clientId = clientId;

            if (string.IsNullOrEmpty(clientSecret))
            {
                //  Google for Installed Apps (Mobile)
                //  is Authorization Grant Flow (Explicit)
                //  2 Step Flow
                //  2nd step does not send Client Secret (null || Empty)
                //throw new ArgumentException("clientSecret must be provided", "clientSecret");

#if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($" ");
                sb.AppendLine($"        clientSecret   = null || Empty");
                sb.AppendLine($"        accessTokenUrl = {accessTokenUrl}");
                sb.AppendLine($"        Google for Installed Apps");
                sb.AppendLine($"        redirectUrl    = {redirectUrl}");
                System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif
            }
            this.clientSecret = clientSecret;

            this.scope = scope ?? "";

            if (authorizeUrl == null)
            {
                throw new ArgumentNullException("authorizeUrl");
            }
            this.authorizeUrl = authorizeUrl;

            if (redirectUrl == null)
            {
                throw new ArgumentNullException("redirectUrl");
            }
            this.redirectUrl = redirectUrl;

            if (accessTokenUrl == null)
            {
                throw new ArgumentNullException("accessTokenUrl");
            }
            this.accessTokenUrl = accessTokenUrl;

            this.getUsernameAsync = getUsernameAsync;

            return;
        }

        private OAuth2Authenticator
                        (
                            Uri redirectUrl,
                            string clientSecret = null,
                            Uri accessTokenUrl = null,
                            bool isUsingNativeUI = false
                        )
            : base(redirectUrl, redirectUrl)
        {
            this.is_using_native_ui = isUsingNativeUI;
            this.OAuth2RandomStateGeneratorFunc = GenerateOAuth2StateRandom;

            if (redirectUrl == null)
            {
                throw new ArgumentNullException("redirectUrl");
            }
            this.redirectUrl = redirectUrl;

            #region
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		MakeSecretOptional #59
            ///		https://github.com/xamarin/Xamarin.Auth/pull/59
            ///		this code was already implementing this PR, but PR was not closed		
            /*
            if (string.IsNullOrEmpty (clientSecret)) 
            {
                throw new ArgumentException ("clientSecret must be provided", "clientSecret");
            }
            */
            this.clientSecret = clientSecret;
            //---------------------------------------------------------------------------------------
            #endregion

            this.accessTokenUrl = accessTokenUrl;

            this.request_state = OAuth2RandomStateGeneratorFunc(16);

            return;
        }


        /// <summary>
        /// Method that returns the initial URL to be displayed in the web browser.
        /// </summary>
        /// <returns>
        /// A task that will return the initial URL.
        /// </returns>
        public override Task<Uri> GetInitialUrlAsync(Dictionary<string, string> custom_query_parameters = null)
        {
            /*
			 	mc++
				OriginalString property of the Uri object should be used instead of AbsoluteUri
				otherwise trailing slash is added.
			*/
            string oauth_redirect_uri_original = this.redirectUrl.OriginalString;

            #if DEBUG
            string oauth_redirect_uri_absolute = this.redirectUrl.AbsoluteUri;

            System.Diagnostics.Debug.WriteLine("GetInitialUrlAsync callbackUrl.AbsoluteUri    = " + oauth_redirect_uri_absolute);
            System.Diagnostics.Debug.WriteLine("GetInitialUrlAsync callbackUrl.OriginalString = " + oauth_redirect_uri_original);
            #endif

            #region
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		OnCreatingInitialUrl virtual method #57
            ///		https://github.com/xamarin/Xamarin.Auth/pull/57
            /*
            var url = new Uri (string.Format (
                "{0}?client_id={1}&redirect_uri={2}&response_type={3}&scope={4}&state={5}",
                authorizeUrl.AbsoluteUri,
                Uri.EscapeDataString (clientId),
                Uri.EscapeDataString (redirectUrl.AbsoluteUri),
                IsImplicit ? "token" : "code",
                Uri.EscapeDataString (scope),
                Uri.EscapeDataString (requestState)));
            */

            RequestParameters = CreateRequestQueryParameters(custom_query_parameters);

            OnCreatingInitialUrl(RequestParameters);

            // already escaped manually merged PRs 62 and 57
            //string queryString = string.Join ("&", query.Select (i => i.Key + "=" + Uri.EscapeDataString (i.Value)));
            string queryString = string.Join("&", RequestParameters.Select(i => i.Key + "=" + i.Value));

            Uri url = string.IsNullOrEmpty(queryString) ? this.authorizeUrl : new Uri(this.authorizeUrl.AbsoluteUri + "?" + queryString);
            //---------------------------------------------------------------------------------------
            #endregion

            // mc++ optimized by Mark Smith
            // var tcs = new TaskCompletionSource<Uri> ();
            // tcs.SetResult(url);

            //return tcs.Task;

            return Task.FromResult(url);
        }

        public Dictionary<string, string> CreateRequestQueryParameters
                                                (
                                                    Dictionary<string, string> custom_query_parameters = null
                                                )
        {
            Dictionary<string, string> oauth_request_query_parameters = null;

            oauth_request_query_parameters = new Dictionary<string, string>();

            //--------------------------------------------------------------------------------------- 
            string cid = this.clientId;
            if (IsUriEncodedDataString(this.clientId) == false)
            {
                cid = Uri.EscapeDataString(this.clientId);
            }
            oauth_request_query_parameters.Add("client_id", cid);
            //--------------------------------------------------------------------------------------- 

            //--------------------------------------------------------------------------------------- 
            string oauth_redirect_uri_original = this.redirectUrl.OriginalString;
            string oauth_redirect_uri_absolute = this.redirectUrl.AbsoluteUri;
            if (this.redirectUrl != null)
            {
                oauth_request_query_parameters.Add
                                                (
                                                    "redirect_uri",
                                                    Uri.EscapeDataString(oauth_redirect_uri_original)
                                                );
            }
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///     Add new property to disable the escaping of scope parameter. #62
            ///     https://github.com/xamarin/Xamarin.Auth/pull/62
            //Uri.EscapeDataString (scope),
            //{"scope", this.scope},
            //{"scope", DoNotEscapeScope? this.scope : Uri.EscapeDataString (this.scope)},

            string scope = this.scope;
            if (DoNotEscapeScope == true)
            {
                // NOOP
            }
            else
            {
                if (IsUriEncodedDataString(scope) == false)
                {
                    scope = Uri.EscapeDataString(this.scope);
                }
            }
            oauth_request_query_parameters.Add("scope", scope);
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            string response_type = OAuthFlowResponseTypeVerification();
            oauth_request_query_parameters.Add("response_type", response_type);
            //--------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            string state = this.OAuth2RandomStateGeneratorFunc(this.StateStringLength);
            if (IsUriEncodedDataString(state) == false)
            {
                state = Uri.EscapeDataString(this.request_state);
            }
            oauth_request_query_parameters.Add("state", state);
            //---------------------------------------------------------------------------------------

            #if DEBUG
            System.Diagnostics.Debug.WriteLine("OAuth Query Parameters DEFAULT:");
            foreach (KeyValuePair<string, string> kvp in oauth_request_query_parameters)
            {
                System.Diagnostics.Debug.WriteLine($"      [{kvp.Key}] = {kvp.Value}");
            }
            #endif

            //---------------------------------------------------------------------------------------
            if (custom_query_parameters != null)
            {
                foreach (KeyValuePair<string, string> kvp in custom_query_parameters)
                {
                    string k = kvp.Key;
                    string v = kvp.Value;

                    if (IsUriEncodedDataString(v) == false)
                    {
                        v = Uri.EscapeDataString(v);
                    }
                    oauth_request_query_parameters[k] = v;
                }
            }

            #if DEBUG
            System.Diagnostics.Debug.WriteLine("OAuth Query Parameters CUSTOMIZED:");
            foreach (KeyValuePair<string, string> kvp in oauth_request_query_parameters)
            {
                System.Diagnostics.Debug.WriteLine($"      [{kvp.Key}] = {kvp.Value}");
            }
            #endif
            //---------------------------------------------------------------------------------------
            return oauth_request_query_parameters;
        }

        /// <summary>
        /// OAuth flow response type verification
        /// 1. 
        /// 
        /// 
        ///     https://alexbilbie.com/guide-to-oauth-2-grants/
        /// 
        /// </summary>
        /// <returns>The uth flow response type verification.</returns>
        /// <see cref=""/>
        /// <see cref="https://alexbilbie.com/guide-to-oauth-2-grants/"/>
        /// <see cref=""/>
        /// <see cref=""/>
        protected string OAuthFlowResponseTypeVerification()
        {
            string response_type = null;

            if (this.IsImplicitFlow)
            {
                response_type = Uri.EscapeDataString("token");
            }
            else if (this.IsAuthorizationCodeFlow)
            {
                response_type = Uri.EscapeDataString("code");
            }
            else if (this.IsProofKeyCodeForExchange)
            {
            }
            else
            {
            }

            #if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"        response_type = {response_type}");
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            return response_type;
        }

        #region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		OnCreatingInitialUrl virtual method #57
        ///		https://github.com/xamarin/Xamarin.Auth/pull/57
        /// <summary>
        /// Invoked when the initial URL is being constructed.
        /// </summary>
        /// <param name='query'>
        /// The parsed query of the URL.
        /// </param>
        protected virtual void OnCreatingInitialUrl(IDictionary<string, string> query)
        {
        }
        //---------------------------------------------------------------------------------------
        #endregion

        /// <summary>
        /// Raised when a new page has been loaded.
        /// </summary>
        /// <param name='url'>
        /// URL of the page.
        /// </param>
        /// <param name='query'>
        /// The parsed query of the URL.
        /// </param>
        /// <param name='fragment'>
        /// The parsed fragment of the URL.
        /// </param>
        protected override void OnPageEncountered(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            var all = new Dictionary<string, string>(query);
            //
            // Check for forgeries
            //
            if (all.ContainsKey("state"))
            {
                if (all["state"] != request_state && !reportedForgery)
                {
                    reportedForgery = true;
                    OnError("Invalid state from server. Possible forgery!");
                    return;
                }
            }

            //
            // Continue processing
            //
            // mc++
            // TODO: schemas
            base.OnPageEncountered(url, query, fragment);
        }

        /// <summary>
        /// Raised when a new page has been loaded.
        /// </summary>
        /// <param name='url'>
        /// URL of the page.
        /// </param>
        /// <param name='query'>
        /// The parsed query string of the URL.
        /// </param>
        /// <param name='fragment'>
        /// The parsed fragment of the URL.
        /// </param>
        protected override void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            //
            // Look for the access_token
            //
            if (fragment.ContainsKey("access_token"))
            {
                //
                // We found an access_token
                //
                OnRetrievedAccountProperties(fragment);
            }
            else if (!IsImplicitFlow)
            {
                //
                // Look for the code
                //
                if (query.ContainsKey("code"))
                {
                    var code = query["code"];
                    RequestAccessTokenAsync(code)
                        .ContinueWith
                            (
                                task =>
                                {
                                    if (task.IsFaulted)
                                    {
                                        OnError(task.Exception);
                                    }
                                    else
                                    {
                                        OnRetrievedAccountProperties(task.Result);
                                    }
                                },
                                TaskScheduler.FromCurrentSynchronizationContext()
                            );
                }
                else
                {
                    OnError("Expected code in response, but did not receive one.");
                    return;
                }
            }
            else
            {
                #region
                //---------------------------------------------------------------------------------------
                /// Pull Request - manually added/fixed
                ///		OAuth2Authenticator changes to work with joind.in OAuth #91
                ///		https://github.com/xamarin/Xamarin.Auth/pull/91
                ///		
                //OnError ("Expected access_token in response, but did not receive one.");
                OnError("Expected " + AccessTokenName + " in response, but did not receive one.");
                //---------------------------------------------------------------------------------------
                #endregion
                return;
            }
        }

        /// <summary>
        /// Asynchronously requests an access token with an authorization <paramref name="code"/>.
        /// </summary>
        /// <returns>
        /// A dictionary of data returned from the authorization request.
        /// </returns>
        /// <param name='code'>The authorization code.</param>
        /// <remarks>Implements: http://tools.ietf.org/html/rfc6749#section-4.1</remarks>
        public Task<IDictionary<string, string>> RequestAccessTokenAsync(string code)
        {
            // mc++ changed protected to public for extension methods RefreshToken (Adrian Stevens) 

            var queryValues = new Dictionary<string, string> 
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUrl.AbsoluteUri },
                { "client_id", clientId },
            };
            if (!string.IsNullOrEmpty(clientSecret))
            {
                queryValues["client_secret"] = clientSecret;
            }

            return RequestAccessTokenAsync(queryValues);
        }

        /// <summary>
        /// Asynchronously makes a request to the access token URL with the given parameters.
        /// </summary>
        /// <param name="queryValues">The parameters to make the request with.</param>
        /// <returns>The data provided in the response to the access token request.</returns>
        public async Task<IDictionary<string, string>> RequestAccessTokenAsync(IDictionary<string, string> queryValues)
        {
            // mc++ changed protected to public for extension methods RefreshToken (Adrian Stevens) 
            var content = new FormUrlEncodedContent(queryValues);


            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(accessTokenUrl, content).ConfigureAwait(false);
            string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Parse the response
            var data = text.Contains("{") ? WebEx.JsonDecode(text) : WebEx.FormDecode(text);

            if (data.ContainsKey("error"))
            {
                throw new AuthException("Error authenticating: " + data["error"]);
            }
            #region
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		OAuth2Authenticator changes to work with joind.in OAuth #91
            ///		https://github.com/xamarin/Xamarin.Auth/pull/91
            ///		
            //else if (data.ContainsKey("access_token"))
            else if (data.ContainsKey(AccessTokenName))
            //---------------------------------------------------------------------------------------
            #endregion
            {
            }
            else
            {
                #region
                //---------------------------------------------------------------------------------------
                /// Pull Request - manually added/fixed
                ///		OAuth2Authenticator changes to work with joind.in OAuth #91
                ///		https://github.com/xamarin/Xamarin.Auth/pull/91
                ///		
                //throw new AuthException ("Expected access_token in access token response, but did not receive one.");
                throw new AuthException("Expected " + AccessTokenName + " in access token response, but did not receive one.");
                //---------------------------------------------------------------------------------------
                #endregion
            }

            return data;
        }

        /// <summary>
        /// Event handler that is fired when an access token has been retreived.
        /// </summary>
        /// <param name='accountProperties'>
        /// The retrieved account properties
        /// </param>
        public virtual void OnRetrievedAccountProperties(IDictionary<string, string> accountProperties)
        {
            // mc++ changed protected to public for extension methods RefreshToken (Adrian Stevens) 
            //
            // Now we just need a username for the account
            //
            if (getUsernameAsync != null)
            {
                getUsernameAsync(accountProperties).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        OnError(task.Exception);
                    }
                    else
                    {
                        OnSucceeded(task.Result, accountProperties);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                OnSucceeded("", accountProperties);
            }
        }

        #region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		Add new property to disable the escaping of scope parameter. #62
        ///		https://github.com/xamarin/Xamarin.Auth/pull/62
        /// <summary>
        /// Disables the escaping of the scope parameter.
        /// </summary>
        /// <value><c>true</c> if the scope should not be escaped; otherwise, <c>false</c>.</value>
        /// <remarks>By default, the <see cref="Xamarin.Auth.OAuth2Authenticator"/> escapes the 
        /// scope parameter. When used with some OAuth2 providers (such as Instagram), this results 
        /// in an HTTP 400 BAD REQUEST being returned on authentication. Setting this property to 
        /// <c>true</c> will prevent escaping of the scope parameter.</remarks>
        public bool DoNotEscapeScope
        {
            get;
            set;
        }
        //---------------------------------------------------------------------------------------
        #endregion
    }
}