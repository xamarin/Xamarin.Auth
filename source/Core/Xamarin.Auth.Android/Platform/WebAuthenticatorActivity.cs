using Android.Accounts;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Http;
using Android.OS;
using Android.Webkit;
using System;
using System.Collections.Generic;

namespace Xamarin.Auth
{
    [Activity(Label = "@string/authenticator_activity_label", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public partial class WebAuthenticatorActivity : AccountAuthenticatorActivity
    {
        private static readonly ActivityStateRepository<State> stateRepo = new ActivityStateRepository<State>();

        private WebView webView;
        private State state;

        public static Intent CreateIntent(Context context, WebAuthenticator authenticator)
        {
            var state = new State
            {
                Authenticator = authenticator,
            };

            var ui = new Intent(context, typeof(WebAuthenticatorActivity));
            ui.PutExtra("ClearCookies", authenticator.ClearCookiesBeforeLogin);
            ui.PutExtra("StateKey", stateRepo.Add(state));
            return ui;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Load the state either from a configuration change or from the intent.
            state = LastNonConfigurationInstance as State;
            if (state == null && Intent.HasExtra("StateKey"))
            {
                var stateKey = Intent.GetStringExtra("StateKey");
                state = stateRepo.Remove(stateKey);
            }
            if (state == null)
            {
                Finish();
                return;
            }

            // Watch for completion
            state.Authenticator.Completed += (s, e) =>
            {
                SetResult(e.IsAuthenticated ? Result.Ok : Result.Canceled);

                if (e.IsAuthenticated && state.Authenticator.GetAccountResult != null)
                {
                    var accountResult = state.Authenticator.GetAccountResult(e.Account);

                    var result = new Bundle();
                    result.PutString(AccountManager.KeyAccountType, accountResult.AccountType);
                    result.PutString(AccountManager.KeyAccountName, accountResult.Name);
                    result.PutString(AccountManager.KeyAuthtoken, accountResult.Token);
                    result.PutString(AccountManager.KeyAccountAuthenticatorResponse, e.Account.Serialize());

                    SetAccountAuthenticatorResult(result);
                }

                Finish();
            };

            state.Authenticator.Error += (s, e) =>
            {
                if (!state.Authenticator.ShowErrors)
                    return;

                if (e.Exception != null)
                    this.ShowError("Authentication Error e.Exception = ", e.Exception);
                else
                    this.ShowError("Authentication Error e.Message = ", e.Message);

                BeginLoadingInitialUrl();
            };

            Title = state.Authenticator.Title;

            BuildUI();

            // Restore the UI state or start over
            if (savedInstanceState != null)
            {
                webView.RestoreState(savedInstanceState);
            }
            else
            {
                if (Intent.GetBooleanExtra("ClearCookies", true))
                    WebAuthenticator.ClearCookies();

                BeginLoadingInitialUrl();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (state.Authenticator.AllowCancel && state.Authenticator.IsAuthenticated())
                state.Authenticator.OnCancelled();
        }

        public override void OnBackPressed()
        {
            if (state.Authenticator.AllowCancel)
                state.Authenticator.OnCancelled();
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            return state;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            webView.SaveState(outState);
        }

        private void BuildUI()
        {
            webView = new WebView(this) { Id = 42 };
            webView.Settings.UserAgentString = WebViewConfiguration.UserAgent;
            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new Client(this));
            SetContentView(webView);
        }

        private async void BeginLoadingInitialUrl()
        {
            try
            {
                var uri = await state.Authenticator.GetInitialUrlAsync();
                webView.LoadUrl(uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                if (!state.Authenticator.ShowErrors)
                    return;

                this.ShowError("Authentication Error t.Exception = ", ex);
            }
        }

        private void BeginProgress(string message)
        {
            webView.Enabled = false;
        }

        private void EndProgress()
        {
            webView.Enabled = true;
        }

        private class State : Java.Lang.Object
        {
            public WebAuthenticator Authenticator;
        }

        private class Client : WebViewClient
        {
            private WebAuthenticatorActivity activity;
            private HashSet<SslCertificate> sslContinue;
            private Dictionary<SslCertificate, List<SslErrorHandler>> inProgress;

            public Client(WebAuthenticatorActivity activity)
            {
                this.activity = activity;
            }

            [Obsolete]
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.Settings.UserAgentString = WebViewConfiguration.UserAgent;

                var scheme = global::Android.Net.Uri.Parse(url).Scheme;
                var host = global::Android.Net.Uri.Parse(url).Host;

                activity.state.Authenticator.Host = host;
                activity.state.Authenticator.Scheme = scheme;

                return scheme != "http" && scheme != "https";
            }

            public override void OnPageStarted(WebView view, string url, global::Android.Graphics.Bitmap favicon)
            {
                view.Settings.UserAgentString = WebViewConfiguration.UserAgent;

                var uri = new Uri(url);
                activity.state.Authenticator.OnPageLoading(uri);
                activity.BeginProgress(uri.Authority);
            }

            public override void OnPageFinished(WebView view, string url)
            {
                view.Settings.UserAgentString = WebViewConfiguration.UserAgent;

                var uri = new Uri(url);
                activity.state.Authenticator.OnPageLoaded(uri);
                activity.EndProgress();
            }

            private class SslCertificateEqualityComparer : IEqualityComparer<SslCertificate>
            {
                public bool Equals(SslCertificate x, SslCertificate y)
                {
                    return Equals(x.IssuedTo, y.IssuedTo) && Equals(x.IssuedBy, y.IssuedBy) && x.ValidNotBeforeDate.Equals(y.ValidNotBeforeDate) && x.ValidNotAfterDate.Equals(y.ValidNotAfterDate);
                }

                bool Equals(SslCertificate.DName x, SslCertificate.DName y)
                {
                    if (ReferenceEquals(x, y))
                        return true;
                    if (ReferenceEquals(x, null) || ReferenceEquals(null, y))
                        return false;

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

                private int GetHashCode(SslCertificate.DName dname)
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

                if (inProgress.TryGetValue(error.Certificate, out List<SslErrorHandler> handlers))
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

                var builder = new AlertDialog.Builder(activity);
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

            private void UpdateInProgressHandlers(SslCertificate certificate, Action<SslErrorHandler> update)
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
