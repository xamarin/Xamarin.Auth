//#if PLATFORM_IOS
//using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
//#elif PLATFORM_ANDROID
//using AuthenticateUIType = Android.Content.Intent;
//using UIContext = Android.Content.Context;
//#elif PLATFORM_WINPHONE
//using Microsoft.Phone.Shell;
//using AuthenticateUIType = System.Uri;
//#else
//using AuthenticateUIType = System.Object;
//#endif
//--------------------------------------------------------------------

namespace Xamarin.Auth
{
    /// <summary>
    /// An authenticator that displays a web page.
    /// </summary>
    public abstract partial class WebAuthenticator : Authenticator
    {
        /// <summary>
        /// Gets or sets whether to automatically clear cookies before logging in.
        /// </summary>
        /// <seealso cref="ClearCookies"/>
        public bool ClearCookiesBeforeLogin
        {
            get { return this.clearCookies; }
            set { this.clearCookies = value; }
        }

        /// <summary>
        /// Method that returns the initial URL to be displayed in the web browser.
        /// </summary>
        /// <returns>
        /// A task that will return the initial URL.
        /// </returns>
        /// <param name="custom_query_parameters">Custom Query parameters</param>
        public abstract Task<Uri> GetInitialUrlAsync(Dictionary<string, string> custom_query_parameters = null);

        /// <summary>
        /// Event handler called when a new page is being loaded in the web browser.
        /// 
        /// Must be virtual because of OAuth1Authenticator
        /// </summary>
        /// <param name='url'>
        /// The URL of the page.
        /// </param>
        public virtual void OnPageLoading(Uri url)
        {
            #if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("WebAuthenticator OnPageLoading Called");
            sb.AppendLine("     AbsoluteUri  = ").AppendLine(url.AbsoluteUri);
            sb.AppendLine("     AbsolutePath = ").AppendLine(url.AbsolutePath);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            return;
        }

        /// <summary>
        /// Event handler called when a new page has been loaded in the web browser.
        /// Implementations should call <see cref="Authenticator.OnSucceeded(Xamarin.Auth.Account)"/> if this page
        /// signifies a successful authentication.
        /// </summary>
        /// <param name='url'>
        /// The URL of the page.
        /// </param>
        public abstract void OnPageLoaded(Uri url);

        /// <summary>
        /// Clears all cookies.
        /// </summary>
        /// <seealso cref="ClearCookiesBeforeLogin"/>
        //public static void ClearCookies()
        //{
        //#if PLATFORM_IOS
        //var store = MonoTouch.Foundation.NSHttpCookieStorage.SharedStorage;
        //var cookies = store.Cookies;
        //foreach (var c in cookies) {
        //	store.DeleteCookie (c);
        //}
        //#elif PLATFORM_ANDROID
        //Android.Webkit.CookieSyncManager.CreateInstance (Android.App.Application.Context);
        //Android.Webkit.CookieManager.Instance.RemoveAllCookie ();
        //#endif
        //}

        /// <summary>
        /// Occurs when the visual, user-interactive, browsing has completed but there
        /// is more authentication work to do.
        /// </summary>
        public event EventHandler BrowsingCompleted;

        private bool clearCookies = true;

        /// <summary>
        /// Raises the browsing completed event.
        /// </summary>
        protected virtual void OnBrowsingCompleted()
        {
            var ev = BrowsingCompleted;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }


        public string Scheme
        {
            get;
            set;
        }

        public string Host
        {
            get;
            set;
        }

        protected Dictionary<string, string> request_parameters;

        /// <summary>
        /// Gets or sets the request parameters.
        /// Request Parameters for OAuth and OpenId parameters
        /// </summary>
        /// <value>The request parameters.</value>
        public Dictionary<string, string> RequestParameters
        {
        	get
        	{
        		return this.request_parameters;
        	}
            set
            {
                request_parameters = value;

                return;
			}

        }

        public bool IsUriEncodedDataString(string s)
        {
        	if
        		(
        			s.Equals(Uri.EscapeDataString(s))
        			&&
        			Uri.IsWellFormedUriString(s, UriKind.RelativeOrAbsolute)
        		)
        	{
        		return true;
        	}

        	return false;
        }

        public override string ToString()
        {
            /*
            string msg = string.Format
                                (
                                    "[WebAuthenticator: ClearCookiesBeforeLogin={0}, Scheme={1}, Host={2}, "
                                    + 
                                    "RequestParameters={3}, PlatformUIMethod={4}, IsUsingNativeUI={5}]", 
                                    ClearCookiesBeforeLogin, 
                                    Scheme, 
                                    Host, 
                                    RequestParameters, 
                                    PlatformUIMethod, 
                                    IsUsingNativeUI
                                );
            */
            System.Text.StringBuilder sb = new System.Text.StringBuilder(base.ToString());

            sb.AppendLine().AppendLine(this.GetType().ToString());
            classlevel_depth++;
            string prefix = new string('\t', classlevel_depth);
            sb.Append(prefix).AppendLine($"IsUsingNativeUI         = {IsUsingNativeUI}");
            sb.Append(prefix).AppendLine($"Scheme                  = {Scheme}");
            sb.Append(prefix).AppendLine($"Host                    = {Host}");
            sb.Append(prefix).AppendLine($"RequestParameters       = {RequestParameters}");
            sb.Append(prefix).AppendLine($"ClearCookiesBeforeLogin = {ClearCookiesBeforeLogin}");
            sb.Append(prefix).AppendLine($"PlatformUIMethod        = {PlatformUIMethod}");

            return sb.ToString();
        }
    }
}

