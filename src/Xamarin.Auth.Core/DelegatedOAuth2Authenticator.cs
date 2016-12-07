//
//  Copyright 2012-2014, Xamarin Inc.
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
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Utilities;
using System.Net;
using System.Text;
using System.Threading;

namespace Xamarin.Auth
{
	/// <summary>
	/// Implements OAuth 2.0 implicit granting. http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.2
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class OAuth2Authenticator : WebRedirectAuthenticator
#else
	public class DelegatedOAuth2Authenticator : WebRedirectAuthenticator
#endif
	{
		Uri authorizeUrl;
	    private readonly string clientId;
	    private readonly string clientSecret;
	    Uri accessTokenUrl;
		GetUsernameAsyncFunc getUsernameAsync;
        
		bool reportedForgery = false;
	    private CancellationTokenSource _errorTokenSource = null;
	    private bool _authenticated;
        
        /// <summary>
		/// Gets the authorize URL.
		/// </summary>
		/// <value>The authorize URL.</value>
		public Uri AuthorizeUrl
		{
			get { return this.authorizeUrl; }
		}

		/// <summary>
		/// Gets the access token URL.
		/// </summary>
		/// <value>The URL used to request access tokens after an authorization code was received.</value>
		public Uri AccessTokenUrl
		{
			get { return this.accessTokenUrl; }
		}

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientId
        {
            get { return clientId; }
        }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>The client secret.</value>
        public string ClientSecret
        {
            get { return clientSecret; }
        }

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
	    /// <param name="clientSecret"></param>
	    /// <param name='getUsernameAsync'>
	    /// Method used to fetch the username of an account
	    /// after it has been successfully authenticated.
	    /// </param>
	    public DelegatedOAuth2Authenticator (Uri authorizeUrl, Uri redirectUrl, string clientId, string clientSecret, GetUsernameAsyncFunc getUsernameAsync = null)
			: this (redirectUrl)
		{
			if (authorizeUrl == null) {
				throw new ArgumentNullException ("authorizeUrl");
			}
	        if (clientId == null) throw new ArgumentNullException(nameof(clientId));
	        if (clientSecret == null) throw new ArgumentNullException(nameof(clientSecret));

            this.authorizeUrl = authorizeUrl;
            this.clientId = clientId;
	        this.clientSecret = clientSecret;

	        this.getUsernameAsync = getUsernameAsync;

			this.accessTokenUrl = null;
		}

#if PLATFORM_ANDROID
        protected override Intent GetPlatformUI(Context context)
	    {
            // store change state
	        _authenticated = false;
	        EnableDomStorage = true;
            EnableJavaScript = true;

	        return base.GetPlatformUI(context);
	    }
#endif

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
		public DelegatedOAuth2Authenticator(Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, GetUsernameAsyncFunc getUsernameAsync = null)
			: this (redirectUrl, accessTokenUrl)
		{
			if (authorizeUrl == null) {
				throw new ArgumentNullException ("authorizeUrl");
			}
			this.authorizeUrl = authorizeUrl;

			if (accessTokenUrl == null) {
				throw new ArgumentNullException ("accessTokenUrl");
			}
			this.accessTokenUrl = accessTokenUrl;

			this.getUsernameAsync = getUsernameAsync;
		}

        DelegatedOAuth2Authenticator(Uri redirectUrl, Uri accessTokenUrl = null)
			: base (redirectUrl, redirectUrl)
		{
			this.accessTokenUrl = accessTokenUrl;
		}
        
		/// <summary>
		/// Method that returns the initial URL to be displayed in the web browser.
		/// </summary>
		/// <returns>
		/// A task that will return the initial URL.
		/// </returns>
		public override Task<Uri> GetInitialUrlAsync ()
		{
		    var url = authorizeUrl;

            // add fragment parameters if present
		    if (!string.IsNullOrWhiteSpace(clientId))
		    {
		        var urlStr = url + (authorizeUrl.OriginalString.Contains("#") ? "&" : "#") + $"client_id={clientId}";

                if (!string.IsNullOrWhiteSpace(clientSecret))
                {
                    urlStr += $"&client_secret={clientSecret}";
                }
		        url = new Uri(urlStr);
		    }
			return Task.FromResult (url);
		}

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
		protected override void OnPageEncountered (Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
		{
			var all = new Dictionary<string, string> (query);
			foreach (var kv in fragment)
				all [kv.Key] = kv.Value;

			//
			// Continue processing
			//
			base.OnPageEncountered (url, query, fragment);
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
		protected override void OnRedirectPageLoaded (Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
		{
		    _errorTokenSource?.Cancel();
		    if (_authenticated)
		        return;

		    //
            // Look for the access_token
            //
            if (fragment.ContainsKey ("access_token")) {
				//
				// We found an access_token
				//
                _authenticated = true;
                OnSucceeding();
				OnRetrievedAccountProperties (fragment);
            }
            else { 
                // cancel after 1000 milliseconds just in case we get another redirect that returns a token
			    _errorTokenSource = new CancellationTokenSource();
			    Task.Delay(1000, _errorTokenSource.Token).ContinueWith((t) =>
			    {
			        if (!t.IsCanceled)
			        {
			            OnError("Expected access_token in response, but did not receive one.");
			        }
			    });
			}
		}
        
		/// <summary>
		/// Asynchronously makes a request to the access token URL with the given parameters.
		/// </summary>
		/// <param name="queryValues">The parameters to make the request with.</param>
		/// <returns>The data provided in the response to the access token request.</returns>
		protected async Task<IDictionary<string,string>> RequestAccessTokenAsync (IDictionary<string, string> queryValues)
		{
			var query = queryValues.FormEncode ();

			var req = WebRequest.Create (accessTokenUrl);
			req.Method = "POST";
			var body = Encoding.UTF8.GetBytes (query);
			//req.ContentLength = body.Length;
			req.ContentType = "application/x-www-form-urlencoded";
			using (var s = await req.GetRequestStreamAsync ()) {
				s.Write (body, 0, body.Length);
			}
			return await req.GetResponseAsync().ContinueWith (task => {
				var text = task.Result.GetResponseText ();

				// Parse the response
				var data = text.Contains ("{") ? WebEx.JsonDecode (text) : WebEx.FormDecode (text);

				if (data.ContainsKey ("error")) {
					throw new AuthException ("Error authenticating: " + data ["error"]);
				} else if (data.ContainsKey ("access_token")) {
					return data;
				} else {
					throw new AuthException ("Expected access_token in access token response, but did not receive one.");
				}
			});
		}

		/// <summary>
		/// Event handler that is fired when an access token has been retreived.
		/// </summary>
		/// <param name='accountProperties'>
		/// The retrieved account properties
		/// </param>
		protected virtual void OnRetrievedAccountProperties (IDictionary<string, string> accountProperties)
		{
			//
			// Now we just need a username for the account
			//
			if (getUsernameAsync != null) {
				getUsernameAsync (accountProperties).ContinueWith (task => {
					if (task.IsFaulted) {
						OnError (task.Exception);
					} else {
						OnSucceeded (task.Result, accountProperties);
					}
				}, TaskScheduler.FromCurrentSynchronizationContext ());
			} else {
				OnSucceeded ("", accountProperties);
			}
		}
	}
}

