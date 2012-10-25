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
using System.Threading.Tasks;
using Xamarin.Utilities;

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that displays web pages until a given "redirect" page is encountered. It then
	/// returns an account with the fragment on that URL.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class WebRedirectAuthenticator : WebAuthenticator
#else
	public class WebRedirectAuthenticator : WebAuthenticator
#endif
	{
		Uri initialUrl;
		Uri redirectUrl;

		public WebRedirectAuthenticator (Uri initialUrl, Uri redirectUrl)
		{
			this.initialUrl = initialUrl;
			this.redirectUrl = redirectUrl;
		}

		public override System.Threading.Tasks.Task<Uri> GetInitialUrlAsync ()
		{
			return Task.Factory.StartNew (() => initialUrl);
		}

		public override void OnPageLoaded (Uri url)
		{
			var query = WebEx.FormDecode (url.Query);
			var fragment = WebEx.FormDecode (url.Fragment);

			OnPageLoaded (url, query, fragment);
		}

		protected virtual void OnPageLoaded (Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
		{
			var all = new Dictionary<string, string> (query);
			foreach (var kv in fragment) all[kv.Key] = kv.Value;

			//
			// Check for errors
			//
			if (all.ContainsKey ("error")) {
				var description = all ["error"];
				if (all.ContainsKey ("error_description")) {
					description = all ["error_description"];
				}
				OnError (description);
				return;
			}

			//
			// Watch for the redirect
			//
			if (url.Host == redirectUrl.Host && url.LocalPath == redirectUrl.LocalPath) {
				OnRedirectPageLoaded (url, query, fragment);
			}
		}

		protected virtual void OnRedirectPageLoaded (Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
		{
			OnSucceeded ("", new Dictionary<string, string> {
				{ "fragment", url.Fragment },
			});
		}
	}
}
