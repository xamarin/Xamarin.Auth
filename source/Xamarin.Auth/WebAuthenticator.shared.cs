using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public abstract partial class WebAuthenticator : Authenticator
    {
        public string Scheme { get; set; }

        public string Host { get; set; }

        public Dictionary<string, string> RequestParameters { get; set; }

        public bool ClearCookiesBeforeLogin { get; set; } = true;

        public bool IsUsingNativeUI { get; protected set; } = false;

        public event EventHandler BrowsingCompleted;

        public abstract Task<Uri> GetInitialUrlAsync(Dictionary<string, string> custom_query_parameters = null);

        public virtual void OnPageLoading(Uri url)
        {
        }

        public abstract void OnPageLoaded(Uri url);

        public static Task ClearCookiesAsync()
        {
            ClearCookies();
            return Task.CompletedTask;
        }

        protected virtual void OnBrowsingCompleted()
        {
            BrowsingCompleted?.Invoke(this, EventArgs.Empty);
        }

        public bool IsUriEncodedDataString(string s)
        {
            return
                s != null &&
                s.Equals(Uri.EscapeDataString(s)) &&
                Uri.IsWellFormedUriString(s, UriKind.RelativeOrAbsolute);
        }

        public string EnsureUriEncodedDataString(string s)
        {
            if (IsUriEncodedDataString(s))
                return s;
            return Uri.EscapeDataString(s);
        }
    }
}
