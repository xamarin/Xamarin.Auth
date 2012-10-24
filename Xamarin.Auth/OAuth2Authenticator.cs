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

namespace Xamarin.Auth
{
	/// <summary>
	/// OAuth 2.0 response type.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal enum OAuth2ResponseType
#else
	public enum OAuth2ResponseType
#endif
	{
		/// <summary>
		/// Request an authorization code according to:
		/// http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.1.1
		/// </summary>
		Code,

		/// <summary>
		/// Request an access token (implicit grant) according to:
		/// http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.2.1
		/// </summary>
		Token,
	}

	/// <summary>
	/// Implements OAuth 2.0 implicit granting. http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.2
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class OAuth2Authenticator : WebAuthenticator
#else
	public class OAuth2Authenticator : WebAuthenticator
#endif
	{
		/// <summary>
		/// Type of method used to fetch the username of an account
		/// after it has been successfully authenticated.
		/// </summary>
		public delegate Task<string> GetUsernameAsyncFunc (string accessToken);

		/// <summary>
		/// Gets or sets the type of the response: token or code.
		/// </summary>
		/// <value>
		/// The type of the response.
		/// </value>
		public OAuth2ResponseType ResponseType { get; set; }

		string clientId;
		string scope;
		Uri authorizeUrl;
		Uri redirectUrl;
		GetUsernameAsyncFunc getUsernameAsync;

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.OAuth2Authenticator"/> class.
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
		{
			ResponseType = OAuth2ResponseType.Token;

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
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.OAuth2Authenticator"/> class
		/// for subclasses.
		/// </summary>
		/// <param name='redirectUrl'>
		/// Redirect URL.
		/// </param>
		protected OAuth2Authenticator (Uri redirectUrl)
		{
			if (redirectUrl == null) {
				throw new ArgumentNullException ("redirectUrl");
			}
			this.redirectUrl = redirectUrl;
		}

		/// <summary>
		/// Method that returns the initial URL to be displayed in the web browser.
		/// </summary>
		/// <returns>
		/// A task that will return the initial URL.
		/// </returns>
		public override Task<Uri> GetInitialUrlAsync ()
		{
			var url = new Uri (string.Format (
				"{0}?client_id={1}&redirect_uri={2}&response_type={3}&scope={4}",
				authorizeUrl.AbsoluteUri,
				Uri.EscapeDataString (clientId),
				Uri.EscapeDataString (redirectUrl.AbsoluteUri),
				ResponseType == OAuth2ResponseType.Token ? "token" : "code",
				Uri.EscapeDataString (scope)));

			return Task.Factory.StartNew (() => {
				return url;
			});
		}

		/// <summary>
		/// Event handler that watches for the redirect URL to be loaded.
		/// </summary>
		/// <param name='url'>
		/// The URL of the loaded page.
		/// </param>
		public override void OnPageLoaded (Uri url)
		{
			if (url.Host == redirectUrl.Host && url.LocalPath == redirectUrl.LocalPath) {
				//
				// Look for the access_token
				//
				var part = url
					.Fragment
						.Split ('#', '?', '&')
						.FirstOrDefault (p => p.StartsWith ("access_token="));
				if (part != null) {
					var accessToken = part.Substring ("access_token=".Length);
					
					//
					// Now we just need a username for the account
					//
					if (getUsernameAsync != null) {
						getUsernameAsync (accessToken).ContinueWith (task => {
							if (task.IsFaulted) {
								OnError (task.Exception);
							}
							else {
								OnSucceeded (task.Result, new Dictionary<string,string> {
									{ "access_token", accessToken },
								});
							}
						}, TaskScheduler.FromCurrentSynchronizationContext ());
					}
					else {
						OnSucceeded ("", new Dictionary<string,string> {
							{ "access_token", accessToken },
						});
					}
				}
				else if (ResponseType == OAuth2ResponseType.Code) {
					throw new NotSupportedException ("ResponseType=code is not supported.");
				}
				else {
					throw new AuthException ("Expected the RedirectUrl to contain an access_token. It did not.");
				}
			}
		}
	}
}

