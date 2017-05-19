using System;
using System.Collections.Generic;

using Android.Content;
using Android.Net;
using Android.Text;
using Android.Content.PM;
using Android.Util;

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

namespace Android.Support.CustomTabs.Chromium.SharedUtilities
{

	/// <summary>
	/// Helper class for Custom Tabs.
	/// </summary>
	public partial class CustomTabsHelper
	{
        public static string CustomTabsHelperAndroidLogTag
		{
			get;
            set;
		} = "CustomTabsHelper";

        public static string CustomTabsHelperUri
		{
			get;
            set;
		} = "http://xamarin.com";


		private CustomTabsHelper()
		{
		}

		public static void AddKeepAliveExtra(Context context, Intent intent)
		{
            //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical 
            // to the Java Class.getCanonicalName method:
			Intent keepAliveIntent = (new Intent()).SetClassName(context.PackageName, typeof(KeepAliveService).FullName);
			intent.PutExtra
                    (
                      PackageManagerHelper.CustomTabsExtraKeepAlive, 
                      keepAliveIntent
                     );

            return;
		}

	}

}