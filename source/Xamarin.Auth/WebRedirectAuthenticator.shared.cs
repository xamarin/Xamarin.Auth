using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public class WebRedirectAuthenticator : WebAuthenticator
    {
        public WebRedirectAuthenticator(Uri initialUrl, Uri redirectUrl)
        {
            InitialUrl = initialUrl ?? throw new ArgumentNullException(nameof(initialUrl));
            RedirectUrl = redirectUrl ?? throw new ArgumentNullException(nameof(redirectUrl));
        }

        public Uri InitialUrl { get; }

        public Uri RedirectUrl { get; }

        public IDictionary<string, string> Query { get; set; }

        public IDictionary<string, string> Fragment { get; set; }

        public bool IsLoadableRedirectUri { get; set; } = false;

        public bool ShouldEncounterOnPageLoading { get; set; } = true;

        public bool ShouldEncounterOnPageLoaded { get; set; } = true;

        public override Task<Uri> GetInitialUrlAsync(Dictionary<string, string> custom_query_parameters = null)
        {
            return Task.FromResult(InitialUrl);
        }

        public override void OnPageLoaded(Uri url)
        {
            Query = WebEx.FormDecode(url.Query);
            Fragment = WebEx.FormDecode(url.Fragment);

            if (ShouldEncounterOnPageLoaded)
                OnPageEncountered(url, Query, Fragment);
        }

        public override void OnPageLoading(Uri url)
        {
            var query = WebEx.FormDecode(url.Query);
            var fragment = WebEx.FormDecode(url.Fragment);

            // TODO: schemas
            if (ShouldEncounterOnPageLoading)
                OnPageEncountered(url, query, fragment);
        }

        protected virtual void OnPageEncountered(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            var all = new Dictionary<string, string>(query);
            foreach (var kv in fragment)
            {
                all[kv.Key] = kv.Value;
            }

            // check for errors
            if (all.TryGetValue("error", out var description))
            {
                if (all.TryGetValue("error_description", out var desc))
                    description = desc;

                OnError(string.Format("OAuth Error = {0}", description));
            }

            // watch for the redirect
            // TODO: schemas
            if (UrlMatchesRedirect(url))
                OnRedirectPageLoaded(url, query, fragment);
        }

        protected virtual void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            // TODO:  mc++ schemas
            if (fragment.Any())
                OnSucceeded("N/A", fragment);
        }

        private bool UrlMatchesRedirect(Uri url)
        {
            // TODO: schemas
            return url.Host == RedirectUrl.Host && url.LocalPath == RedirectUrl.LocalPath;
        }
    }
}
