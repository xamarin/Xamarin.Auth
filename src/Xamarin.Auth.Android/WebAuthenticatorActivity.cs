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
using Android.App;
using Android.Webkit;
using Android.OS;
using System.Threading.Tasks;
using Xamarin.Utilities.Android;

namespace Xamarin.Auth
{
	[Activity (Label = "Web Authenticator")]
#if XAMARIN_AUTH_INTERNAL
	internal class WebAuthenticatorActivity : Activity
#else
	public class WebAuthenticatorActivity : Activity
#endif
	{
		WebView webView;

		internal class State : Java.Lang.Object
		{
			public WebAuthenticator Authenticator;
		}
		internal static readonly ActivityStateRepository<State> StateRepo = new ActivityStateRepository<State> ();

		State state;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			//
			// Load the state either from a configuration change or from the intent.
			//
			state = LastNonConfigurationInstance as State;
			if (state == null && Intent.HasExtra ("StateKey")) {
				var stateKey = Intent.GetStringExtra ("StateKey");
				state = StateRepo.Remove (stateKey);
			}
			if (state == null) {
				Finish ();
				return;
			}

			Title = state.Authenticator.Title;

			//
			// Watch for completion
			//
			state.Authenticator.Completed += (s, e) => {
				SetResult (e.IsAuthenticated ? Result.Ok : Result.Canceled);
				Finish ();
			};
			state.Authenticator.Error += (s, e) => {
				if (e.Exception != null) {
					this.ShowError ("Authentication Error", e.Exception);
				}
				else {
					this.ShowError ("Authentication Error", e.Message);
				}
				BeginLoadingInitialUrl ();
			};

			//
			// Build the UI
			//
			webView = new WebView (this) {
				Id = 42,
			};
			webView.Settings.JavaScriptEnabled = true;
			webView.SetWebViewClient (new Client (this));
			SetContentView (webView);

			//
			// Restore the UI state or start over
			//
			if (savedInstanceState != null) {
				webView.RestoreState (savedInstanceState);
			}
			else {
				Android.Webkit.CookieManager.Instance.RemoveAllCookie ();
				BeginLoadingInitialUrl ();
			}
		}

		void BeginLoadingInitialUrl ()
		{
			state.Authenticator.GetInitialUrlAsync ().ContinueWith (t => {
				if (t.IsFaulted) {
					this.ShowError ("Authentication Error", t.Exception);
				}
				else {
					webView.LoadUrl (t.Result.AbsoluteUri);
				}
			}, TaskScheduler.FromCurrentSynchronizationContext ());
		}

		public override void OnBackPressed ()
		{
			state.Authenticator.OnCancelled ();
		}

		public override Java.Lang.Object OnRetainNonConfigurationInstance ()
		{
			return state;
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			webView.SaveState (outState);
		}

		ProgressDialog shownProgress = null;

		void BeginLoading (Uri url)
		{
			// Build.VERSION.SdkInt >= BuildVersionCodes.Eclair

			if (shownProgress != null) {
				EndLoading ();
			}
			shownProgress = ProgressDialog.Show (this, "Loading...", url.Authority, true);
		}

		void EndLoading ()
		{
			if (shownProgress != null) {
				shownProgress.Dismiss ();
				shownProgress = null;
			}
		}

		class Client : WebViewClient
		{
			WebAuthenticatorActivity activity;

			public Client (WebAuthenticatorActivity activity)
			{
				this.activity = activity;
			}

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				return false;
			}

			public override void OnPageStarted (WebView view, string url, Android.Graphics.Bitmap favicon)
			{
				activity.state.Authenticator.OnPageLoading (new Uri (url));
				activity.BeginLoading (new Uri (url));
			}

			public override void OnPageFinished (WebView view, string url)
			{
				activity.state.Authenticator.OnPageLoaded (new Uri (url));
				activity.EndLoading ();
			}
		}
	}
}

