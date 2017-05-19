// Copyright 2015 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Android.App;
using Android.Content;
using Android.Net;
using Android.Support.CustomTabs.Chromium.SharedUtilities;

namespace Android.Support.CustomTabs.Chromium.SharedUtilities
{

	/// <summary>
	/// A Fallback that opens a Webview when Custom Tabs is not available
	/// </summary>
	public class WebViewFallback : ICustomTabFallback
	{
		public void OpenUri(Activity activity, Uri uri)
		{
			Intent intent = new Intent(activity, typeof(Xamarin.Auth.WebViewActivity));
			intent.PutExtra(Xamarin.Auth.WebViewActivity.EXTRA_URL, uri.ToString());
			activity.StartActivity(intent);

            return;
		}
	}

}