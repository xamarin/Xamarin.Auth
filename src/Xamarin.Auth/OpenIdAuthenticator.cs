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
	/// Implements OpenId implicit granting.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class OpenIdAuthenticator : OAuth2Authenticator
#else
	public class OpenIdAuthenticator : OAuth2Authenticator
#endif
	{
		string requestNonce;

		/// <summary>
		/// Initializes a new <see cref="Xamarin.Auth.OpenIdAuthenticator"/>
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
		public OpenIdAuthenticator (string clientId, string scope, Uri authorizeUrl, Uri redirectUrl, GetUsernameAsyncFunc getUsernameAsync = null)
			: base(clientId, scope, authorizeUrl, redirectUrl, getUsernameAsync)
		{
			//
			// Generate a unique nonce string to check for forgeries
			//
			this.requestNonce = RandomState();
		}

		/// <summary>
		/// Initializes a new instance <see cref="Xamarin.Auth.OpenIdAuthenticator"/>
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
		public OpenIdAuthenticator (string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, GetUsernameAsyncFunc getUsernameAsync = null)
			: base (clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, getUsernameAsync)
		{
			//
			// Generate a unique nonce string to check for forgeries
			//
			this.requestNonce = RandomState();
		}

		/// <summary>
		/// Method that returns the initial URL to be displayed in the web browser.
		/// </summary>
		/// <returns>
		/// A task that will return the initial URL.
		/// </returns>
		public override Task<Uri> GetInitialUrlAsync ()
		{
			var responseType = IsImplicit ? "id_token token" : "code";
			var url = new Uri (string.Format (
				"{0}?client_id={1}&redirect_uri={2}&response_type={3}&scope={4}&state={5}&nonce={6}",
				authorizeUrl.AbsoluteUri,
				Uri.EscapeDataString (clientId),
				Uri.EscapeDataString (RedirectUrl.AbsoluteUri),
				responseType,
				Uri.EscapeDataString (scope),
				Uri.EscapeDataString(requestState),
				Uri.EscapeDataString(requestNonce)));

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
			if (all.ContainsKey ("nonce")) {
				if (all ["nonce"] != requestNonce && !reportedForgery) {
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
		protected override void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
		{
			//
			// Look for the id_token
			//
			if (fragment.ContainsKey("id_token"))
			{
				//
				// We found an id_token
				//
				OnRetrievedAccountProperties(fragment);
			}
			else if (!IsImplicit)
			{
				//
				// Look for the code
				//
				if (query.ContainsKey("code"))
				{
					var code = query["code"];
					RequestAccessTokenAsync(code).ContinueWith(task =>
					{
						if (task.IsFaulted)
						{
							OnError(task.Exception);
						}
						else
						{
							OnRetrievedAccountProperties(task.Result);
						}
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
				else
				{
					OnError("Expected code in response, but did not receive one.");
					return;
				}
			}
			else
			{
				OnError("Expected access_token in response, but did not receive one.");
				return;
			}
		}
	}
}

