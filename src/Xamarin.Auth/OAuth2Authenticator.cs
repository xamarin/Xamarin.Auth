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

namespace Xamarin.Auth
{
	/// <summary>
	/// Implements OAuth 2.0 implicit granting. http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.2
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class OAuth2Authenticator : WebRedirectAuthenticator
#else
	public class OAuth2Authenticator : WebRedirectAuthenticator
#endif
	{
		string clientId;
		string clientSecret;
		string scope;
		Uri authorizeUrl;
		Uri accessTokenUrl;
		bool sendRedirectUrl;
		GetUsernameAsyncFunc getUsernameAsync;

		string requestState;
		bool reportedForgery = false;

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
			get { return this.clientSecret; }
		}

		/// <summary>
		/// Gets the authorization scope.
		/// </summary>
		/// <value>The authorization scope.</value>
		public string Scope
		{
			get { return this.scope; }
		}

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
		/// Gets or sets a value indicating whether this <see cref="Xamarin.Auth.OAuth2Authenticator"/> includes the redirect URI when constructing the authorize URI.
		/// </summary>
		/// <remarks>>OAuth2 RFC states RedirectUri is optional https://tools.ietf.org/html/rfc6749#section-4.1.1</remarks>
		/// <value><c>true</c> to send redirect URI; otherwise, <c>false</c>.</value>
		public bool SendRedirectUrl {
			get { return this.sendRedirectUrl; }
			set { this.sendRedirectUrl = value; }
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
		/// <param name='getUsernameAsync'>
		/// Method used to fetch the username of an account
		/// after it has been successfully authenticated.
		/// </param>
		public OAuth2Authenticator (string clientId, string scope, Uri authorizeUrl, Uri redirectUrl, GetUsernameAsyncFunc getUsernameAsync = null)
			: this (redirectUrl)
		{
			if (string.IsNullOrEmpty (clientId)) {
				throw new ArgumentException ("clientId must be provided", "clientId");
			}
			this.clientId = clientId;

			this.scope = scope ?? "";

			if (authorizeUrl == null) {
				throw new ArgumentNullException ("authorizeUrl");
			}
			this.authorizeUrl = authorizeUrl;

			this.getUsernameAsync = getUsernameAsync;

			this.accessTokenUrl = null;

			this.sendRedirectUrl = true;
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
		public OAuth2Authenticator (string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, GetUsernameAsyncFunc getUsernameAsync = null)
			: this (redirectUrl, clientSecret, accessTokenUrl)
		{
			if (string.IsNullOrEmpty (clientId)) {
				throw new ArgumentException ("clientId must be provided", "clientId");
			}
			this.clientId = clientId;

			if (string.IsNullOrEmpty (clientSecret)) {
				throw new ArgumentException ("clientSecret must be provided", "clientSecret");
			}
			this.clientSecret = clientSecret;

			this.scope = scope ?? "";

			if (authorizeUrl == null) {
				throw new ArgumentNullException ("authorizeUrl");
			}
			this.authorizeUrl = authorizeUrl;

			if (accessTokenUrl == null) {
				throw new ArgumentNullException ("accessTokenUrl");
			}
			this.accessTokenUrl = accessTokenUrl;

			this.getUsernameAsync = getUsernameAsync;

			this.sendRedirectUrl = true;
		}

		OAuth2Authenticator (Uri redirectUrl, string clientSecret = null, Uri accessTokenUrl = null)
			: base (redirectUrl, redirectUrl)
		{
			this.clientSecret = clientSecret;

			this.accessTokenUrl = accessTokenUrl;

			//
			// Generate a unique state string to check for forgeries
			//
			var chars = new char[16];
			var rand = new Random ();
			for (var i = 0; i < chars.Length; i++) {
				chars [i] = (char)rand.Next ((int)'a', (int)'z' + 1);
			}
			this.requestState = new string (chars);

			this.sendRedirectUrl = true;
		}

		bool IsImplicit { get { return accessTokenUrl == null; } }

		/// <summary>
		/// Method that returns the initial URL to be displayed in the web browser.
		/// </summary>
		/// <returns>
		/// A task that will return the initial URL.
		/// </returns>
		public override Task<Uri> GetInitialUrlAsync ()
		{
			StringBuilder builder = new StringBuilder ();
			builder.AppendFormat ("{0}?client_id={1}&response_type={2}&scope={3}&state={4}",
				authorizeUrl.AbsoluteUri,
				Uri.EscapeDataString (clientId),
				IsImplicit ? "token" : "code",
				Uri.EscapeDataString (scope),
				Uri.EscapeDataString (requestState));

			if (this.sendRedirectUrl) {
				builder.AppendFormat ("&redirect_uri={0}", Uri.EscapeDataString (RedirectUrl.AbsoluteUri));
			}

			var url = new Uri (builder.ToString());

			var tcs = new TaskCompletionSource<Uri> ();
			tcs.SetResult (url);
			return tcs.Task;
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
			// Check for forgeries
			//
			if (all.ContainsKey ("state")) {
				if (all ["state"] != requestState && !reportedForgery) {
					reportedForgery = true;
					OnError ("Invalid state from server. Possible forgery!");
					return;
				}
			}

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
			//
			// Look for the access_token
			//
			if (fragment.ContainsKey ("access_token")) {
				//
				// We found an access_token
				//
				OnRetrievedAccountProperties (fragment);
			} else if (!IsImplicit) {
				//
				// Look for the code
				//
				if (query.ContainsKey ("code")) {
					var code = query ["code"];
					RequestAccessTokenAsync (code).ContinueWith (task => {
						if (task.IsFaulted) {
							OnError (task.Exception);
						} else {
							OnRetrievedAccountProperties (task.Result);
						}
					}, TaskScheduler.FromCurrentSynchronizationContext ());
				} else {
					OnError ("Expected code in response, but did not receive one.");
					return;
				}
			} else {
				OnError ("Expected access_token in response, but did not receive one.");
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
		Task<IDictionary<string,string>> RequestAccessTokenAsync (string code)
		{
			var queryValues = new Dictionary<string, string> {
				{ "grant_type", "authorization_code" },
				{ "code", code },
				{ "redirect_uri", RedirectUrl.AbsoluteUri },
				{ "client_id", clientId },
			};
			if (!string.IsNullOrEmpty (clientSecret)) {
				queryValues ["client_secret"] = clientSecret;
			}

			return RequestAccessTokenAsync (queryValues);
		}

		/// <summary>
		/// Asynchronously makes a request to the access token URL with the given parameters.
		/// </summary>
		/// <param name="queryValues">The parameters to make the request with.</param>
		/// <returns>The data provided in the response to the access token request.</returns>
		protected Task<IDictionary<string,string>> RequestAccessTokenAsync (IDictionary<string, string> queryValues)
		{
			var query = queryValues.FormEncode ();

			var req = WebRequest.Create (accessTokenUrl);
			req.Method = "POST";
			var body = Encoding.UTF8.GetBytes (query);
			req.ContentLength = body.Length;
			req.ContentType = "application/x-www-form-urlencoded";
			using (var s = req.GetRequestStream ()) {
				s.Write (body, 0, body.Length);
			}
			return req.GetResponseAsync ().ContinueWith (task => {
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

