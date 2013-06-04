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

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.WebRedirectAuthenticator"/> class.
		/// </summary>
		/// <param name='initialUrl'>
		/// The initial URL loaded into the web browser.
		/// </param>
		/// <param name='redirectUrl'>
		/// The URL watched for.
		/// </param>
		public WebRedirectAuthenticator (Uri initialUrl, Uri redirectUrl)
		{
			this.initialUrl = initialUrl;
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
			var tcs = new TaskCompletionSource<Uri> ();
			tcs.SetResult (initialUrl);
			return tcs.Task;
		}

		/// <summary>
		/// Event handler called when a page has completed loading.
		/// </summary>
		/// <param name='url'>
		/// The URL of the page.
		/// </param>
		public override void OnPageLoaded (Uri url)
		{
			var query = WebEx.FormDecode (url.Query);
			var fragment = WebEx.FormDecode (url.Fragment);

			OnPageEncountered (url, query, fragment);
		}

		/// <summary>
		/// Event handler called when a new page is being loaded in the web browser.
		/// </summary>
		/// <param name='url'>
		/// The URL of the page.
		/// </param>
		public override void OnPageLoading (Uri url)
		{
			var query = WebEx.FormDecode (url.Query);
			var fragment = WebEx.FormDecode (url.Fragment);

			OnPageEncountered (url, query, fragment);
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
		protected virtual void OnPageEncountered (Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
		{
			var all = new Dictionary<string, string> (query);
			foreach (var kv in fragment)
				all [kv.Key] = kv.Value;

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
			if (UrlMatchesRedirect (url)) {
				OnRedirectPageLoaded (url, query, fragment);
			}
		}

		private bool UrlMatchesRedirect (Uri url)
		{
			return url.Host == redirectUrl.Host && url.LocalPath == redirectUrl.LocalPath;
		}

		/// <summary>
		/// Raised when the redirect page has been loaded.
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
		protected virtual void OnRedirectPageLoaded (Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
		{
			OnSucceeded ("", fragment);
		}
	}
}
