using System;
using System.Collections.Generic;

using Android.Content;
using Android.Content.PM;
using Android.Util;
using Android.Text;
using System.Text;

namespace Android.Support.CustomTabs.Chromium.SharedUtilities
{
    public class PackageManagerHelper
    {
		/*
        internal const string STABLE_PACKAGE = "com.android.chrome";
        internal const string BETA_PACKAGE = "com.chrome.beta";
        internal const string DEV_PACKAGE = "com.chrome.dev";
        internal const string LOCAL_PACKAGE = "com.google.android.apps.chrome";
        */
		public static Dictionary<string, string> PackagesSupportingCustomTabs
		{
			get;
			set;
		} =
			new Dictionary<string, string>()
			{
				{"STABLE_PACKAGE", "com.android.chrome"},
				{"BETA_PACKAGE", "com.chrome.beta"},
				{"DEV_PACKAGE", "com.chrome.dev"},
				{"LOCAL_PACKAGE", "com.google.android.apps.chrome"},
			};

		public static string CustomTabsExtraKeepAlive
        {
            get;
        } = "android.support.customtabs.extra.KEEP_ALIVE";

		public static string CustomTabsActionCustomTabsService
		{
			get;
		} = "android.support.customtabs.action.CustomTabsService";

        public PackageManagerHelper()
        {
        }

		private static string sPackageNameToUse;

		public static Func<Context, string, string> GetPackageNameToUse
		{
			get;
			set;
		} = GetPackageNameToUseImplementation;

		public static Func<Context, string> GetPackageNameToUseDefaultUri
		{
			get;
			set;
		} = GetPackageNameToUseImplementation;

        protected static string GetPackageNameToUseImplementation(Context context)
        {
            return GetPackageNameToUseImplementation(context, CustomTabsHelper.CustomTabsHelperUri);
        }

		/// <summary>
		/// Goes through all apps that handle VIEW intents and have a warmup service. Picks
		/// the one chosen by the user if there is one, otherwise makes a best effort to return a
		/// valid package name.
		/// 
		/// This is <strong>not</strong> threadsafe.
		/// </summary>
		/// <param name="context"> <seealso cref="Context"/> to use for accessing <seealso cref="PackageManager"/>. </param>
		/// <returns> The package name recommended to use for connecting to custom tabs related components. </returns>
		protected static string GetPackageNameToUseImplementation
                                    (
                                        Context context, 
                                        string url
                                    )
		{
			if (sPackageNameToUse != null)
			{
				return sPackageNameToUse;
			}

			PackageManager pm = context.PackageManager;
			// Get default VIEW intent handler.
			Intent activityIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
			ResolveInfo defaultViewHandlerInfo = pm.ResolveActivity(activityIntent, 0);
			string defaultViewHandlerPackageName = null;
			if (defaultViewHandlerInfo != null)
			{
				defaultViewHandlerPackageName = defaultViewHandlerInfo.ActivityInfo.PackageName;
			}

			// Get all apps that can handle VIEW intents.
			IList<ResolveInfo> resolvedActivityList = pm.QueryIntentActivities(activityIntent, PackageInfoFlags.MatchAll);
			IList<string> packagesSupportingCustomTabs = new List<string>();
			foreach (ResolveInfo info in resolvedActivityList)
			{
				Intent serviceIntent = new Intent();
				// Android.Support.CustomTabs.action.CustomTabsService
				serviceIntent.SetAction(PackageManagerHelper.CustomTabsActionCustomTabsService);
				serviceIntent.SetPackage(info.ActivityInfo.PackageName);
				if (pm.ResolveService(serviceIntent, PackageInfoFlags.MatchAll) != null)
				{
					packagesSupportingCustomTabs.Add(info.ActivityInfo.PackageName);
				}
			}

			// Now packagesSupportingCustomTabs contains all apps that can handle both VIEW intents
			// and service calls.
			if (packagesSupportingCustomTabs.Count == 0)
			{
				sPackageNameToUse = null;
			}
			else if (packagesSupportingCustomTabs.Count == 1)
			{
				sPackageNameToUse = packagesSupportingCustomTabs[0];
			}
			else if
				(
					!TextUtils.IsEmpty(defaultViewHandlerPackageName)
					&&
					!HasSpecializedHandlerIntents(context, activityIntent)
					&&
					packagesSupportingCustomTabs.Contains(defaultViewHandlerPackageName)
				)
			{
				sPackageNameToUse = defaultViewHandlerPackageName;
			}
			else if (packagesSupportingCustomTabs.Contains(PackagesSupportingCustomTabs["STABLE_PACKAGE"]))
			{
				sPackageNameToUse = PackagesSupportingCustomTabs["STABLE_PACKAGE"];
			}
			else if (packagesSupportingCustomTabs.Contains(PackagesSupportingCustomTabs["BETA_PACKAGE"]))
			{
				sPackageNameToUse = PackagesSupportingCustomTabs["BETA_PACKAGE"];
			}
			else if (packagesSupportingCustomTabs.Contains(PackagesSupportingCustomTabs["DEV_PACKAGE"]))
			{
				sPackageNameToUse = PackagesSupportingCustomTabs["DEV_PACKAGE"];
			}
			else if (packagesSupportingCustomTabs.Contains(PackagesSupportingCustomTabs["CANARY_PACKAGE"]))
			{
				sPackageNameToUse = PackagesSupportingCustomTabs["CANARY_PACKAGE"];
			}
			else if (packagesSupportingCustomTabs.Contains(PackagesSupportingCustomTabs["LOCAL_PACKAGE"]))
			{
				sPackageNameToUse = PackagesSupportingCustomTabs["LOCAL_PACKAGE"];
			}

			return sPackageNameToUse;
		}

		/// <summary>
		/// Used to check whether there is a specialized handler for a given intent. </summary>
		/// <param name="intent"> The intent to check with. </param>
		/// <returns> Whether there is a specialized handler for the given intent. </returns>
		private static bool HasSpecializedHandlerIntents(Context context, Intent intent)
		{
			try
			{
				PackageManager pm = context.PackageManager;
				IList<ResolveInfo> handlers = pm.QueryIntentActivities(intent, Android.Content.PM.PackageInfoFlags.ResolvedFilter);
				if (handlers == null || handlers.Count == 0)
				{
					return false;
				}
				foreach (ResolveInfo resolveInfo in handlers)
				{
					IntentFilter filter = resolveInfo.Filter;
					if (filter == null)
					{
						continue;
					}
					if (filter.CountDataAuthorities() == 0 || filter.CountDataPaths() == 0)
					{
						continue;
					}
					if (resolveInfo.ActivityInfo == null)
					{
						continue;
					}
					return true;
				}
			}
			catch (Exception e)
			{
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Runtime exception while getting specialized handlers");
				sb.Append("Exception = ");
				sb.Append(e.Message);
                Log.Error(CustomTabsHelper.CustomTabsHelperAndroidLogTag, sb.ToString());
			}
			return false;
		}


		/// <returns> All possible chrome package names that provide custom tabs feature. </returns>
		public static string[] Packages
		{
			get
			{
				return new string[]
				{
					"",
					PackagesSupportingCustomTabs["STABLE_PACKAGE"],
					PackagesSupportingCustomTabs["BETA_PACKAGE"],
					PackagesSupportingCustomTabs["DEV_PACKAGE"],
					PackagesSupportingCustomTabs["CANARY_PACKAGE"],
					PackagesSupportingCustomTabs["LOCAL_PACKAGE"],
				};
			}
		}
	}
}
