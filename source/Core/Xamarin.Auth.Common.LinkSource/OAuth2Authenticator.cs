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
    /// Implements OAuth 2.0 
    ///     - authorization 
    /// and 
    ///     - implicit 
    /// grant types. 
    /// http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.2
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
        public OAuth2.State State
        {
            get
            {
                return request_state;
            }
            set
            {
                request_state = value;

                return;
            }
        }
        OAuth2.State request_state;
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
                    // AccessToken url is defined
                    accessTokenUrl != null                      
                    &&
                    // Client Secret MAY be defined
                    //
                    // true
                    ( string.IsNullOrWhiteSpace(clientSecret) || !string.IsNullOrWhiteSpace(clientSecret) )  
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
            if (authorizeUrl == null)
            {
                throw new ArgumentNullException("authorizeUrl");
            }
            if (accessTokenUrl == null)
            {
                throw new ArgumentNullException("accessTokenUrl");
            }
            if (redirectUrl == null)
            {
                throw new ArgumentNullException("redirectUrl");
            }


            if (string.IsNullOrEmpty(clientSecret))
            {
                /*
                    https://tools.ietf.org/html/rfc6749#section-2.3.1

                    RFC6749 2.3.1 Client Password
                    
                    Alternatively, the authorization server MAY support including the
                    client credentials in the request-body using the following
                    parameters:
   
                    client_id
                		 REQUIRED.The client identifier issued to the client during
                		 the registration process described by Section 2.2.

                    client_secret
                		 REQUIRED.  The client secret.The client MAY omit the
                		 parameter if the client secret is an empty string.


                    Including the client credentials in the request-body using the two
                    parameters is NOT RECOMMENDED and SHOULD be limited to clients unable
                    to directly utilize the HTTP Basic authentication scheme(or other
                    password-based HTTP authentication schemes).  The parameters can only
                    be transmitted in the request-body and MUST NOT be included in the
                    request URI.

                    For example, a request to refresh an access token(Section 6) using
                    the body parameters(with extra line breaks for display purposes
                    only):

                    Google for Installed Apps(Mobile, NativeUI) is Authorization Code Grant Flow
                    2 Steps Flow
                    2nd step does not send Client Secret(null || Empty)
                    OK according to the RFC
                    
    				//throw new ArgumentException("clientSecret must be provided", "clientSecret");
                 */ 

                System.Diagnostics.Debug.WriteLine(this.ToString());
            }

            this.clientId = clientId;                   // required
            this.clientSecret = clientSecret;           // optional
            this.authorizeUrl = authorizeUrl;           // required 
            this.redirectUrl = redirectUrl;             // optional 
            this.accessTokenUrl = accessTokenUrl;       // optional - required for Authorization Code Grant
            this.scope = scope ?? "";                   // optional

            this.getUsernameAsync = getUsernameAsync;   // Xamarin.legacy

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
            this.State = new OAuth2.State();

			this.redirectUrl = redirectUrl;
			this.accessTokenUrl = accessTokenUrl;

			Verify();


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


            return;
        }

        protected void Verify()
        {
            if (redirectUrl == null)
            {
                throw new ArgumentNullException("redirectUrl");
            }

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

        /// <summary>
        /// Creates the request query parameters. The method is called before request to Authorization server.
        /// 
        /// </summary>
        /// <returns>The request query parameters (standard and custom)</returns>
        /// <param name="custom_query_parameters">Dictionary of custom query parameters.</param>
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
            List<string> response_types = OAuthFlowResponseTypeVerification();

            string response_type = string.Join(" ", response_types);
            oauth_request_query_parameters.Add("response_type", response_type);
            //--------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            string state = this.State.RandomString;
            if (IsUriEncodedDataString(state) == false)
            {
                state = Uri.EscapeDataString(state);
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
        /// <returns>The OAuth flow response type verification.</returns>
        /// <see cref=""/>
        /// <see cref="https://alexbilbie.com/guide-to-oauth-2-grants/"/>
        /// <see cref=""/>
        /// <see cref=""/>
        protected List<string> OAuthFlowResponseTypeVerification()
        {
            List<string> response_types = VerifyOAuth2FlowResponseType
                                                    (
                                                        this.AccessTokenUrl, 
                                                        this.ClientSecret,      // MAY indicate
                                                        null
                                                    );

            if (this.IsImplicitFlow)
            {
                response_types = new List<string>() { };
                response_types.Add(Uri.EscapeDataString("token"));
            }
            else if (this.IsAuthorizationCodeFlow)
            {
                response_types = new List<string>() { };
                response_types.Add(Uri.EscapeDataString("code"));
            }
            else if (this.IsProofKeyCodeForExchange)
            {
                //
            }
            else
            {
                throw new InvalidOperationException("Uknown response_type");
            }

            #if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.Append($"        response_type = ").AppendLine(string.Join(" ",response_types));
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            return response_types;
        }

        /*
            The authorization endpoint is used by the authorization code grant
            type and implicit grant type flows.  The client informs the
            authorization server of the desired grant type using the following
            parameter:

            response_type
            	 REQUIRED.The value MUST be one of 
            	 "code" for requesting an authorization code as described by Section 4.1.1, 
            	 "token" for requesting an access token (implicit grant) as described by Section 4.2.1, 
            	 or a 
            	 registered extension value as described by Section 8.4.

           Extension response types MAY contain a space-delimited(%x20) list of
           values, where the order of values does not matter(e.g., response
           type "a b" is the same as "b a").  The meaning of such composite
           response types is defined by their respective specifications.

           If an authorization request is missing the "response_type" parameter,
           or if the response type is not understood, the authorization server
           MUST return an error response as described in Section 4.1.2.1.         
        */
        public List<string> VerifyOAuth2FlowResponseType
                                        (
                                            System.Uri accessTokenUrl,
                                            string clientSecret,
                                            string[] curtomResponseTypes    // extensions or mutiple
                                        )
        {
            List<string> respponse_types_generated = null;

            return respponse_types_generated;
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
                if (all["state"] != request_state.RandomStringUriEscaped && !reportedForgery)
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
            // access_token parameter lookup
            if (fragment.ContainsKey("access_token"))
            {
                // access_token found
                OnRetrievedAccountProperties(fragment);
            }
            else if (!IsImplicitFlow)
            {
                // code parameter lookpup
                if (query.ContainsKey("code"))
                {
                    string code = query["code"];
                    TaskScheduler task_scheduler = null;
                    try
                    {
                        task_scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                    }
                    catch (System.InvalidOperationException exc_io)
                    {
                        /* 
                            TODO: INVESTIGATE UWP only exception
                                System.InvalidOperationException: 
                                    The current SynchronizationContext may not be used as a TaskScheduler
                       */
                        string msg = exc_io.Message;
                        System.Diagnostics.Debug.WriteLine($"OAuthAuthenticator exception {msg}");
                    }

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
                                    task_scheduler
                                );
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


        public override string ToString()
        {
            /*
            string msg = string.Format
                                (
                                    "[OAuth2Authenticator: State={0}, " 
                                    +
                                    "StateStringLength={1}, OAuth2RandomStateGeneratorFunc={2}, AccessTokenName={3}, Scope={4}, AuthorizeUrl={5},"
                                    +
                                    "AccessTokenUrl={6}, ClientId={7}, ClientSecret={8}, DoNotEscapeScope={9}, HttpWebClientUsed={10}]",
                                    State, 
                                    StateStringLength, 
                                    OAuth2RandomStateGeneratorFunc, 
                                    AccessTokenName, 
                                    Scope, 
                                    AuthorizeUrl, 
                                    AccessTokenUrl, 
                                    ClientId, 
                                    ClientSecret, 
                                    DoNotEscapeScope, 
                                    HttpWebClientUsed
                                 );
            */
            System.Text.StringBuilder sb = new System.Text.StringBuilder(base.ToString());

            sb.AppendLine().AppendLine(this.GetType().ToString());
            classlevel_depth++;
            string prefix = new string('\t', classlevel_depth);
            sb.Append(prefix).AppendLine($"AuthorizeUrl      = {AuthorizeUrl}");
            sb.Append(prefix).AppendLine($"AccessTokenUrl    = {AccessTokenUrl}");
            sb.Append(prefix).AppendLine($"State             = {State}");
            sb.Append(prefix).AppendLine($"StateStringLength = {this.State.StateStringLength}");
            sb.Append(prefix).AppendLine($"Scope             = {Scope}");
            sb.Append(prefix).AppendLine($"DoNotEscapeScope  = {DoNotEscapeScope}");
            sb.Append(prefix).AppendLine($"ClientId          = {ClientId}");
            sb.Append(prefix).AppendLine($"ClientSecret      = {ClientSecret}");
            sb.Append(prefix).AppendLine($"ClientId          = {ClientId}");

            return sb.ToString();
        }
    }
}
