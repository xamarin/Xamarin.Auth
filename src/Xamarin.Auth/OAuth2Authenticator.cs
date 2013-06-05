//
//  Copyright 2012, Xamarin Inc.
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
		Uri redirectUrl;
		Uri accessTokenUrl;
		GetUsernameAsyncFunc getUsernameAsync;

		string requestState;
		bool reportedForgery = false;

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

			if (redirectUrl == null) {
				throw new ArgumentNullException ("redirectUrl");
			}
			this.redirectUrl = redirectUrl;

			this.getUsernameAsync = getUsernameAsync;

			this.accessTokenUrl = null;
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

			if (redirectUrl == null) {
				throw new ArgumentNullException ("redirectUrl");
			}
			this.redirectUrl = redirectUrl;

			if (accessTokenUrl == null) {
				throw new ArgumentNullException ("accessTokenUrl");
			}
			this.accessTokenUrl = accessTokenUrl;

			this.getUsernameAsync = getUsernameAsync;
		}

		OAuth2Authenticator (Uri redirectUrl, string clientSecret = null, Uri accessTokenUrl = null)
			: base (redirectUrl, redirectUrl)
		{
			if (redirectUrl == null) {
				throw new ArgumentNullException ("redirectUrl");
			}
			this.redirectUrl = redirectUrl;

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
			var url = new Uri (string.Format (
				"{0}?client_id={1}&redirect_uri={2}&response_type={3}&scope={4}&state={5}",
				authorizeUrl.AbsoluteUri,
				Uri.EscapeDataString (clientId),
				Uri.EscapeDataString (redirectUrl.AbsoluteUri),
				IsImplicit ? "token" : "code",
				Uri.EscapeDataString (scope),
				Uri.EscapeDataString (requestState)));

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
		/// Implements: http://tools.ietf.org/html/rfc6749#section-4.1
		/// </summary>
		/// <returns>
		/// The access token async.
		/// </returns>
		/// <param name='code'>
		/// Code.
		/// </param>
		Task<IDictionary<string,string>> RequestAccessTokenAsync (string code)
		{
			var queryValues = new Dictionary<string, string> {
				{ "grant_type", "authorization_code" },
				{ "code", code },
				{ "redirect_uri", redirectUrl.AbsoluteUri },
				{ "client_id", clientId },
			};
			if (!string.IsNullOrEmpty (clientSecret)) {
				queryValues ["client_secret"] = clientSecret;
			}

			return RequestAccessTokenAsync (queryValues);
		}

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

