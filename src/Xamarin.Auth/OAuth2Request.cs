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
using System.Collections.Generic;
using Xamarin.Auth;

namespace Xamarin.Auth
{
	/// <summary>
	/// Request that is authenticated using an account retrieved from an <see cref="OAuth2Authenticator"/>.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class OAuth2Request : Request
#else
	public class OAuth2Request : Request
#endif
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.OAuth2Request"/> class.
		/// </summary>
		/// <param name='method'>
		/// The HTTP method.
		/// </param>
		/// <param name='url'>
		/// The URL.
		/// </param>
		/// <param name='parameters'>
		/// Parameters that will pre-populate the <see cref="Xamarin.Auth.Request.Parameters"/> property or <see langword="null"/>.
		/// </param>
		/// <param name='account'>
		/// The account used to authenticate this request.
		/// </param>
		public OAuth2Request (string method, Uri url, IDictionary<string, string> parameters, Account account)
			: base (method, url, parameters, account)
		{
			AccessTokenParameterName = "access_token";
		}
		
		/// <summary>
		/// Gets the response.
		/// </summary>
		/// <returns>
		/// The response.
		/// </returns>
		protected override Uri GetPreparedUrl ()
		{
			return GetAuthenticatedUrl (Account, base.GetPreparedUrl (), AccessTokenParameterName);
		}

		/// <summary>
		/// The name of the access token parameter in URLs
		/// </summary>
		public string AccessTokenParameterName { get; set; }

		/// <summary>
		/// Transforms an unauthenticated URL to an authenticated one.
		/// </summary>
		/// <returns>
		/// The authenticated URL.
		/// </returns>
		/// <param name='account'>
		/// The <see cref="Account"/> that's been authenticated.
		/// </param>
		/// <param name='unauthenticatedUrl'>
		/// The unauthenticated URL.
		/// </param>
		public static Uri GetAuthenticatedUrl (Account account, Uri unauthenticatedUrl, string accessTokenParameterName = "access_token")
		{
			if (account == null) {
				throw new ArgumentNullException ("account");
			}
			if (!account.Properties.ContainsKey ("access_token")) {
				throw new ArgumentException ("OAuth2 account is missing required access_token property.", "account");
			}
			if (unauthenticatedUrl == null) {
				throw new ArgumentNullException ("unauthenticatedUrl");
			}
			
			var url = unauthenticatedUrl.AbsoluteUri;
			
			if (url.Contains ("?")) {
				url += "&" + accessTokenParameterName + "=" + account.Properties ["access_token"];
			} else {
				url += "?" + accessTokenParameterName + "=" + account.Properties ["access_token"];
			}
			
			return new Uri (url);
		}

		/// <summary>
		/// Gets an authenticated HTTP Authorization header.
		/// </summary>
		/// <returns>
		/// The authorization header.
		/// </returns>
		/// <param name='account'>
		/// The <see cref="Account"/> that's been authenticated.
		/// </param>
		public static string GetAuthorizationHeader (Account account)
		{
			if (account == null) {
				throw new ArgumentNullException ("account");
			}
			if (!account.Properties.ContainsKey ("access_token")) {
				throw new ArgumentException ("OAuth2 account is missing required access_token property.", "account");
			}
			
			return "Bearer " + account.Properties ["access_token"];
		}
	}
}

