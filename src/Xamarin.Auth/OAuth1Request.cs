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
using System.Threading;
using System.Net;
using System.Collections.Generic;
using Xamarin.Auth;

namespace Xamarin.Auth
{
	/// <summary>
	/// Request that is authenticated using an account retrieved from an <see cref="OAuth1Authenticator"/>.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class OAuth1Request : Request
#else
	public class OAuth1Request : Request
#endif
	{
		bool includeMultipartsInSignature;

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.OAuth1Request"/> class.
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
		/// <param name='includeMultipartsInSignature'>
		/// If set to <see langword="true"/> include multiparts when calculating the OAuth 1.0 signature.
		/// </param>
		public OAuth1Request (string method, Uri url, IDictionary<string, string> parameters, Account account, bool includeMultipartsInSignature = false)
			: base (method, url, parameters, account)
		{
			this.includeMultipartsInSignature = includeMultipartsInSignature;
		}

		/// <summary>
		/// Gets the response.
		/// </summary>
		/// <returns>
		/// The response.
		/// </returns>
		public override Task<Response> GetResponseAsync (CancellationToken cancellationToken)
		{
			//
			// Make sure we have an account
			//
			if (Account == null) {
				throw new InvalidOperationException ("You must specify an Account for this request to proceed");
			}

			//
			// Sign the request before getting the response
			//
			var req = GetPreparedWebRequest ();

			//
			// Make sure that the parameters array contains
			// mulitpart keys if we're dealing with a buggy
			// OAuth implementation (I'm looking at you Flickr).
			//
			// These normally shouldn't be included: http://tools.ietf.org/html/rfc5849#section-3.4.1.3.1
			//
			var ps = new Dictionary<string, string> (Parameters);
			if (includeMultipartsInSignature) {
				foreach (var p in Multiparts) {
					if (!string.IsNullOrEmpty (p.TextData)) {
						ps [p.Name] = p.TextData;
					}
				}
			}

			//
			// Authorize it
			//
			var authorization = OAuth1.GetAuthorizationHeader (
				Method,
				Url,
				ps,
				Account.Properties ["oauth_consumer_key"],
				Account.Properties ["oauth_consumer_secret"],
				Account.Properties ["oauth_token"],
				Account.Properties ["oauth_token_secret"]);
			
			req.Headers [HttpRequestHeader.Authorization] = authorization;
			
			return base.GetResponseAsync (cancellationToken);
		}
	}
}

