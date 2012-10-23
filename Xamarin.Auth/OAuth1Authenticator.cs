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
using System.Net;
using System.Collections.Generic;
using Xamarin.Utilities;

namespace Xamarin.Auth
{
	public class OAuth1Authenticator : WebAuthenticator
	{
		public delegate Task<string> GetUsernameAsyncFunc (IDictionary<string, string> accountProperties);

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

		public OAuth1Authenticator (string consumerKey, string consumerSecret, Uri requestTokenUrl, Uri authorizeUrl, Uri accessTokenUrl, Uri callbackUrl, GetUsernameAsyncFunc getUsernameAsync)
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

			if (getUsernameAsync == null) {
				throw new ArgumentNullException ("getUsernameAsync");
			}
			this.getUsernameAsync = getUsernameAsync;
		}

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

				var url = authorizeUrl.AbsoluteUri + "?oauth_token=" + Uri.EscapeDataString (token);

				return new Uri (url);
			});
		}

		public override void OnPageLoaded (Uri url)
		{
			if (url.Host == callbackUrl.Host && url.AbsolutePath == callbackUrl.AbsolutePath) {
				
				var query = url.Query;
				var r = WebEx.FormDecode (query);
				
				verifier = r["oauth_verifier"];
				
				GetAccessTokenAsync ();
			}
		}

		Task GetAccessTokenAsync ()
		{
			var req = OAuth1.CreateRequest (
				"GET",
				accessTokenUrl,
				new Dictionary<string, string> {
					{ "oauth_verifier", verifier },
					{ "oauth_token", token },
				},
				consumerKey,
				consumerSecret,
				tokenSecret);
			
			return req.GetResponseAsync ().ContinueWith (respTask => {				
				var content = respTask.Result.GetResponseText ();

				var accountProperties = WebEx.FormDecode (content);
				accountProperties["oauth_consumer_key"] = consumerKey;
				accountProperties["oauth_consumer_secret"] = consumerSecret;

				getUsernameAsync (accountProperties).ContinueWith (uTask => {
					if (uTask.IsFaulted) {
						OnError (uTask.Exception);
					}
					else {
						OnSucceeded (uTask.Result, accountProperties);
					}
				});
			});
		}
	}
}

