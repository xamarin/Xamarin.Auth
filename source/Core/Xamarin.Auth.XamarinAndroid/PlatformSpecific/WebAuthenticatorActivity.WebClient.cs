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
using Android.App;
using Android.Net.Http;
using Android.Webkit;
using Android.OS;
using System.Threading.Tasks;
using Xamarin.Utilities.Android;
using System.Text;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    #if XAMARIN_AUTH_INTERNAL
    internal partial class WebAuthenticatorActivity : global::Android.Accounts.AccountAuthenticatorActivity
    #else
    public partial class WebAuthenticatorActivity : global::Android.Accounts.AccountAuthenticatorActivity
	#endif
    {
        class Client : WebViewClient
        {
            WebAuthenticatorActivity activity;
            HashSet<SslCertificate> sslContinue;
            Dictionary<SslCertificate, List<SslErrorHandler>> inProgress;

            public Client(WebAuthenticatorActivity activity)
            {
                this.activity = activity;
            }

            [Obsolete]
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.Settings.UserAgentString = WebViewConfiguration.Android.UserAgent;
                
                string scheme = null;
                string host = null;

                scheme = global::Android.Net.Uri.Parse(url).Scheme;
                host = global::Android.Net.Uri.Parse(url).Host;

                activity.state.Authenticator.Host = host;
                activity.state.Authenticator.Scheme = scheme;

                #if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.Append("Xamarin.Auth.Android.WebAuthenticatorActivity").AppendLine("");
                sb.Append("             Scheme = ").AppendLine(scheme);
                sb.Append("             Host   = ").AppendLine(host);
                System.Diagnostics.Debug.WriteLine(sb.ToString());
                #endif

                bool should_override = false;

                if (url != null && scheme == "http" /*url.StartsWith("http://")*/)
                {
                    should_override = false;
                }
                else if (url != null && scheme == "https" /*url.StartsWith("https://")*/)
                {
                    should_override = false;
                }
                else
                {
                    should_override = true;
                }

                return should_override;
            }
            /// <summary>
            /// On the page started.
            /// </summary>
            /// <param name="view">View.</param>
            /// <param name="url">URL.</param>
            /// <param name="favicon">Favicon.</param>
            public override void OnPageStarted(WebView view, string url, global::Android.Graphics.Bitmap favicon)
            {
                view.Settings.UserAgentString = WebViewConfiguration.Android.UserAgent;

                var uri = new Uri(url);
                activity.state.Authenticator.OnPageLoading(uri);
                activity.BeginProgress(uri.Authority);

                return;
            }

            public override void OnPageFinished(WebView view, string url)
            {
                view.Settings.UserAgentString = WebViewConfiguration.Android.UserAgent;

                var uri = new Uri(url);
                activity.state.Authenticator.OnPageLoaded(uri);
                activity.EndProgress();

                return;
            }

            class SslCertificateEqualityComparer
                : IEqualityComparer<SslCertificate>
            {
                public bool Equals(SslCertificate x, SslCertificate y)
                {
                    return Equals(x.IssuedTo, y.IssuedTo) && Equals(x.IssuedBy, y.IssuedBy) && x.ValidNotBeforeDate.Equals(y.ValidNotBeforeDate) && x.ValidNotAfterDate.Equals(y.ValidNotAfterDate);
                }

                bool Equals(SslCertificate.DName x, SslCertificate.DName y)
                {
                    if (ReferenceEquals(x, y))
                        return true;

                    #region
                    ///-------------------------------------------------------------------------------------------------
                    /// Pull Request - manually added/fixed
                    ///     bug fix in SslCertificateEqualityComparer #76
                    ///     https://github.com/xamarin/Xamarin.Auth/pull/76
                    //if (ReferenceEquals (x, y) || ReferenceEquals (null, y))
                    if (ReferenceEquals(x, null) || ReferenceEquals(null, y))
                        return false;
                    ///-------------------------------------------------------------------------------------------------
                    #endregion
                    return x.GetDName().Equals(y.GetDName());
                }

                public int GetHashCode(SslCertificate obj)
                {
                    unchecked
                    {
                        int hashCode = GetHashCode(obj.IssuedTo);
                        hashCode = (hashCode * 397) ^ GetHashCode(obj.IssuedBy);
                        hashCode = (hashCode * 397) ^ obj.ValidNotBeforeDate.GetHashCode();
                        hashCode = (hashCode * 397) ^ obj.ValidNotAfterDate.GetHashCode();
                        return hashCode;
                    }
                }

                int GetHashCode(SslCertificate.DName dname)
                {
                    return dname.GetDName().GetHashCode();
                }
            }

            public override void OnReceivedSslError(WebView view, SslErrorHandler handler, SslError error)
            {
                if (sslContinue == null)
                {
                    var certComparer = new SslCertificateEqualityComparer();
                    sslContinue = new HashSet<SslCertificate>(certComparer);
                    inProgress = new Dictionary<SslCertificate, List<SslErrorHandler>>(certComparer);
                }

                List<SslErrorHandler> handlers;
                if (inProgress.TryGetValue(error.Certificate, out handlers))
                {
                    handlers.Add(handler);
                    return;
                }

                if (sslContinue.Contains(error.Certificate))
                {
                    handler.Proceed();
                    return;
                }

                inProgress[error.Certificate] = new List<SslErrorHandler>();

                AlertDialog.Builder builder = new AlertDialog.Builder(this.activity);
                builder.SetTitle("Security warning");
                builder.SetIcon(global::Android.Resource.Drawable.IcDialogAlert);
                builder.SetMessage("There are problems with the security certificate for this site.");

                builder.SetNegativeButton("Go back", (sender, args) =>
                {
                    UpdateInProgressHandlers(error.Certificate, h => h.Cancel());
                    handler.Cancel();
                });

                builder.SetPositiveButton("Continue", (sender, args) =>
                {
                    sslContinue.Add(error.Certificate);
                    UpdateInProgressHandlers(error.Certificate, h => h.Proceed());
                    handler.Proceed();
                });

                builder.Create().Show();
            }

            void UpdateInProgressHandlers(SslCertificate certificate, Action<SslErrorHandler> update)
            {
                List<SslErrorHandler> inProgressHandlers;
                if (!this.inProgress.TryGetValue(certificate, out inProgressHandlers))
                    return;

                foreach (SslErrorHandler sslErrorHandler in inProgressHandlers)
                    update(sslErrorHandler);

                inProgressHandlers.Clear();
            }
        }
    }
}

