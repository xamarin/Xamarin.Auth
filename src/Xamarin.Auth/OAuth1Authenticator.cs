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
using System.Net;
using System.Collections.Generic;
using Xamarin.Utilities;

namespace Xamarin.Auth
{
	/// <summary>
	/// OAuth 1.0 authenticator.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class OAuth1Authenticator : WebAuthenticator
#else
	public class OAuth1Authenticator : WebAuthenticator
#endif
	{
		string consumerKey;
		string consumerSecret;

		Uri requestTokenUrl;
		Uri authorizeUrl;
		Uri accessTokenUrl;
		Uri callbackUrl;

		GetUsernameAsyncFunc getUsernameAsync;

		string token;
		string tokenSecret;

		string verifier;

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.OAuth1Authenticator"/> class.
		/// </summary>
		/// <param name='consumerKey'>
		/// Consumer key.
		/// </param>
		/// <param name='consumerSecret'>
		/// Consumer secret.
		/// </param>
		/// <param name='requestTokenUrl'>
		/// Request token URL.
		/// </param>
		/// <param name='authorizeUrl'>
		/// Authorize URL.
		/// </param>
		/// <param name='accessTokenUrl'>
		/// Access token URL.
		/// </param>
		/// <param name='callbackUrl'>
		/// Callback URL.
		/// </param>
		/// <param name='getUsernameAsync'>
		/// Method used to fetch the username of an account
		/// after it has been successfully authenticated.
		/// </param>
		public OAuth1Authenticator (string consumerKey, string consumerSecret, Uri requestTokenUrl, Uri authorizeUrl, Uri accessTokenUrl, Uri callbackUrl, GetUsernameAsyncFunc getUsernameAsync = null)
		{
			if (string.IsNullOrEmpty (consumerKey)) {
				throw new ArgumentException ("consumerKey must be provided", "consumerKey");
			}
			this.consumerKey = consumerKey;

			if (string.IsNullOrEmpty (consumerSecret)) {
				throw new ArgumentException ("consumerSecret must be provided", "consumerSecret");
			}
			this.consumerSecret = consumerSecret;

			if (requestTokenUrl == null) {
				throw new ArgumentNullException ("requestTokenUrl");
			}
			this.requestTokenUrl = requestTokenUrl;

			if (authorizeUrl == null) {
				throw new ArgumentNullException ("authorizeUrl");
			}
			this.authorizeUrl = authorizeUrl;

			if (accessTokenUrl == null) {
				throw new ArgumentNullException ("accessTokenUrl");
			}
			this.accessTokenUrl = accessTokenUrl;

			if (callbackUrl == null) {
				throw new ArgumentNullException ("callbackUrl");
			}
			this.callbackUrl = callbackUrl;

			this.getUsernameAsync = getUsernameAsync;
		}

		/// <summary>
		/// Gets the consumer key.
		/// </summary>
		public string ConsumerKey
		{
			get { return this.consumerKey; }
		}

		/// <summary>
		/// Gets the consumer secret.
		/// </summary>
		public string ConsumerSecret
		{
			get { return this.consumerSecret; }
		}

		/// <summary>
		/// Gets the request token url. http://oauth.net/core/1.0/#request_urls
		/// </summary>
		public Uri RequestTokenUrl
		{
			get { return this.requestTokenUrl; }
		}

		/// <summary>
		/// Gets the authorize url. http://oauth.net/core/1.0/#request_urls
		/// </summary>
		public Uri AuthorizeUrl
		{
			get { return this.authorizeUrl; }
		}

		/// <summary>
		/// Gets the access token url. http://oauth.net/core/1.0/#request_urls
		/// </summary>
		public Uri AccessTokenUrl
		{
			get { return this.accessTokenUrl; }
		}

		/// <summary>
		/// Gets the callback url. http://oauth.net/core/1.0/#auth_step2
		/// </summary>
		public Uri CallbackUrl
		{
			get { return this.callbackUrl; }
		}

		/// <summary>
		/// Method that returns the initial URL to be displayed in the web browser.
		/// </summary>
		/// <returns>
		/// A task that will return the initial URL.
		/// </returns>
		public override Task<Uri> GetInitialUrlAsync () {
			var req = OAuth1.CreateRequest (
				"GET",
				requestTokenUrl, 
				new Dictionary<string, string>() {
					{ "oauth_callback", callbackUrl.AbsoluteUri },
				},
				consumerKey,
				consumerSecret,
				"");

			return req.GetResponseAsync ().ContinueWith (respTask => {

				var content = respTask.Result.GetResponseText ();

				var r = WebEx.FormDecode (content);

				token = r["oauth_token"];
				tokenSecret = r["oauth_token_secret"];

				string paramType = authorizeUrl.AbsoluteUri.IndexOf("?") >= 0 ? "&" : "?";

				var url = authorizeUrl.AbsoluteUri + paramType + "oauth_token=" + Uri.EscapeDataString (token);
				return new Uri (url);
			});
		}

		/// <summary>
		/// Event handler that watches for the callback URL to be loaded.
		/// </summary>
		/// <param name='url'>
		/// The URL of the loaded page.
		/// </param>
		public override void OnPageLoaded (Uri url)
		{
			if (url.Host == callbackUrl.Host && url.AbsolutePath == callbackUrl.AbsolutePath) {
				
				var query = url.Query;
				var r = WebEx.FormDecode (query);

				r.TryGetValue ("oauth_verifier", out verifier);

				GetAccessTokenAsync ().ContinueWith (getTokenTask => {
					if (getTokenTask.IsCanceled) {
						OnCancelled ();
					} else if (getTokenTask.IsFaulted) {
						OnError (getTokenTask.Exception);
					}
				}, TaskContinuationOptions.NotOnRanToCompletion);
			}
		}

		Task GetAccessTokenAsync ()
		{
			var requestParams = new Dictionary<string, string> {
				{ "oauth_token", token }
			};

			if (verifier != null)
				requestParams["oauth_verifier"] = verifier;

			var req = OAuth1.CreateRequest (
				"GET",
				accessTokenUrl,
				requestParams,
				consumerKey,
				consumerSecret,
				tokenSecret);
			
			return req.GetResponseAsync ().ContinueWith (respTask => {				
				var content = respTask.Result.GetResponseText ();

				var accountProperties = WebEx.FormDecode (content);
				accountProperties["oauth_consumer_key"] = consumerKey;
				accountProperties["oauth_consumer_secret"] = consumerSecret;

				if (getUsernameAsync != null) {
					getUsernameAsync (accountProperties).ContinueWith (uTask => {
						if (uTask.IsFaulted) {
							OnError (uTask.Exception);
						}
						else {
							OnSucceeded (uTask.Result, accountProperties);
						}
					});
				}
				else {
					OnSucceeded ("", accountProperties);
				}
			});
		}
	}
}

