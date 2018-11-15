//
//  Copyright 2012-2016, Xamarin Inc.
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
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Utilities;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
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
        public WebRedirectAuthenticator(Uri initialUrl, Uri redirectUrl)
        {
            this.initialUrl = initialUrl;
            this.redirectUrl = redirectUrl;

            return;
        }

        /// <summary>
        /// Method that returns the initial URL to be displayed in the web browser.
        /// </summary>
        /// <returns>
        /// A task that will return the initial URL.
        /// </returns>
        public override Task<Uri> GetInitialUrlAsync(Dictionary<string, string> custom_query_parameters = null)
        {
            // mc++ optimized by Mark Smith
            //var tcs = new TaskCompletionSource<Uri> ();
            //tcs.SetResult (initialUrl);
            //return tcs.Task;

            return Task.FromResult(initialUrl);
        }

        /// <summary>
        /// Event handler called when a page has completed loading.
        /// </summary>
        /// <param name='url'>
        /// The URL of the page.
        /// </param>
        public override void OnPageLoaded(Uri url)
        {
			IDictionary<string, string> query = WebEx.FormDecode(url.Query);
			IDictionary<string, string> fragment = WebEx.FormDecode(url.Fragment);
			this.Query = query;
			this.Fragment = fragment;


            if (ShouldEncounterOnPageLoaded)
            {
                OnPageEncountered(url, query, fragment);
            }

            return;
        }

        public IDictionary<string, string> Query
        {
            get;
            set;
        }

		public IDictionary<string, string> Fragment
		{
			get;
			set;
		}

		/// <summary>
		/// Event handler called when a new page is being loaded in the web browser.
		/// </summary>
		/// <param name='url'>
		/// The URL of the page.
		/// </param>
		public override void OnPageLoading(Uri url)
        {
            #if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("WebRedirectAuthenticator OnPageLoading Called");
            sb.AppendLine("     AbsoluteUri  = ").AppendLine(url.AbsoluteUri);
            sb.AppendLine("     AbsolutePath = ").AppendLine(url.AbsolutePath);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            var query = WebEx.FormDecode(url.Query);
            var fragment = WebEx.FormDecode(url.Fragment);

            // mc++
            // TODO: schemas
            if (ShouldEncounterOnPageLoading)
            {
                OnPageEncountered(url, query, fragment);
            }
            return;
        }

        /// <summary>
        /// Raised when a new page has been encountered.
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
        /// <remarks>
        /// <para>
        /// This is invoked on any event that has a URL: <see cref="OnPageLoaded" /> and <see cref="OnPageLoading" />.
        /// Not all platforms may support triggering <see cref="OnPageLoading" />, so this is provided as a blanket
        /// method to check redirect URLs at the earliest possible time to avoid showing redirect pages if unnecessary.
        /// </para>
        /// </remarks>
        protected virtual void OnPageEncountered(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            Dictionary<string, string> all = new Dictionary<string, string>(query);
            foreach (var kv in fragment)
            {
                all[kv.Key] = kv.Value;
            }

            //
            // Check for errors
            //
            if (all.ContainsKey("error"))
            {
                string description = all["error"];
                if (all.ContainsKey("error_description"))
                {
                    description = all["error_description"];
                }
                OnError(string.Format("OAuth Error = {0}",description));

                return;
            }

            //
            // Watch for the redirect
            //
            if (UrlMatchesRedirect(url))
            {
                // TODO:  mc++ schemas
                OnRedirectPageLoaded(url, query, fragment);
            }

            return;
        }

        protected bool UrlMatchesRedirect(Uri url)
        {
            // mc++
            // TODO: schemas
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
        protected virtual void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            string msg = null;

            #if DEBUG
            string q = string.Join("  ;  ", query.Select(x => x.Key + "=" + x.Value));
            msg = String.Format("WebRedirectAuthenticator.OnRedirectPageLoaded {0}", q);
            string f = string.Join("  ;  ", query.Select(x => x.Key + "=" + x.Value));
            msg = String.Format("WebRedirectAuthenticator.OnRedirectPageLoaded {0}", f);
            System.Diagnostics.Debug.WriteLine(msg);
            #endif

            // TODO:  mc++ schemas
            if (fragment.Any())
            {
                OnSucceeded("N/A", fragment);
            }

            return;
        }

        /// <summary>
        /// Sets a value indicating whether this <see cref="T:Xamarin.Auth.WebRedirectAuthenticator"/> 
        /// is loadabl redirect URI.
        /// 
        /// localhost, 127.0.0.1 and ::1 hosts will not be loadable
        /// </summary>
        /// <value><c>true</c> if is loadable redirect URI; otherwise, <c>false</c>.</value>
        public bool IsLoadableRedirectUri
        {
            get;
            set;
        } = false;


		/// <summary>
		/// Sets a value indicating whether OnPageEncountered gets fired during OnPageLoading.
		/// </summary>       
		/// <value><c>true</c> if OnPageEncountered should be fired in OnPageLoading; otherwise, <c>false</c>.</value>
		public bool ShouldEncounterOnPageLoading
        {
            get;
            set;
        } = true;


		/// <summary>
		/// Sets a value indicating whether OnPageEncountered gets fired during OnPageLoaded.
		/// </summary>       
		/// <value><c>true</c> if OnPageEncountered should be fired in OnPageLoaded; otherwise, <c>false</c>.</value>
		public bool ShouldEncounterOnPageLoaded
        {
            get;
            set;
        } = true;

        public override string ToString()
        {
            /*
            string msg = string.Format
                               (
                                   "[WebRedirectAuthenticator: Query={0}, Fragment={1}, IsLoadableRedirectUri={2},"
                                   + 
                                   "ShouldEncounterOnPageLoading={3}, ShouldEncounterOnPageLoaded={4}]", 
                                   Query, 
                                   Fragment, 
                                   IsLoadableRedirectUri, 
                                   ShouldEncounterOnPageLoading, 
                                   ShouldEncounterOnPageLoaded
                                );
            */
            System.Text.StringBuilder sb = new System.Text.StringBuilder(base.ToString());

            sb.AppendLine().AppendLine(this.GetType().ToString());
            classlevel_depth++;
            string prefix = new string('\t', classlevel_depth);
            sb.Append(prefix).AppendLine($"Query                        = {Query}");
            sb.Append(prefix).AppendLine($"Fragment                     = {Fragment}");
            sb.Append(prefix).AppendLine($"IsLoadableRedirectUri        = {IsLoadableRedirectUri}");
            sb.Append(prefix).AppendLine($"ShouldEncounterOnPageLoading = {ShouldEncounterOnPageLoading}");
            sb.Append(prefix).AppendLine($"ShouldEncounterOnPageLoaded  = {ShouldEncounterOnPageLoaded}");

            return sb.ToString();
        }
    }
}
