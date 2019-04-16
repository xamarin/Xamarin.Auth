using System;
using System.Collections.Generic;
using System.Text;

using Android.Content;
using Android.Content.PM;
using Android.Util;
using Android.Text;

#if ! AZURE_MOBILE_SERVICES
namespace Android.Support.CustomTabs.Chromium.SharedUtilities
#else
namespace Android.Support.CustomTabs.Chromium.SharedUtilities._MobileServices
#endif
{
    #if XAMARIN_CUSTOM_TABS_INTERNAL
    internal class PackageManagerHelper
    #else
    public class PackageManagerHelper
	#endif
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

		private static List<string> sPackageNamesToUse;
		private static string sPackageNameToUse;

		public static Func<Context, string, List<string>> GetPackageNameToUse
        {
            get;
            set;
        } = GetPackageNamesToUseImplementation;

        public static Func<Context, List<string>> GetPackageNamesToUseDefaultUri
        {
            get;
            set;
        } = GetPackageNamesToUseImplementation;

        protected static List<string> GetPackageNamesToUseImplementation(Context context)
        {
            return GetPackageNamesToUseImplementation
                        (
                            context,
                            Xamarin.Auth.CustomTabsConfiguration.CustomTabsHelperUri
                        );
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
        protected static List<string> GetPackageNamesToUseImplementation
                                    (
                                        Context context,
                                        string url
                                    )
        {
            sPackageNamesToUse = new List<string>();

            if ( ! string.IsNullOrEmpty(sPackageNameToUse) )
            {
                // User has set the Package to handle Opening Urls
                sPackageNamesToUse.Add(sPackageNameToUse);

                // do not try to detect available packages
                return sPackageNamesToUse;
            }

            PackageManager pm = context.PackageManager;
            // Get default VIEW intent handler.
            Intent activityIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            ResolveInfo resolve_info_default_view_handler = pm.ResolveActivity(activityIntent, 0);
            string defaultViewHandlerPackageName = null;
            if (resolve_info_default_view_handler != null)
            {
                defaultViewHandlerPackageName = resolve_info_default_view_handler.ActivityInfo.PackageName;
            }

            System.Diagnostics.Debug.WriteLine($"defaultViewHandlerPackageName = {defaultViewHandlerPackageName}");

            #if DEBUG
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine($"      package for url ");
            sb1.AppendLine($"         url = {url.ToString()}");
			sb1.AppendLine($"         resolve_info_default_view_handler.ResolvePackageName       = {resolve_info_default_view_handler.ResolvePackageName}");
			sb1.AppendLine($"         resolve_info_default_view_handler.ActivityInfo.PackageName = {resolve_info_default_view_handler.ActivityInfo.PackageName}");
			sb1.AppendLine($"         resolve_info_default_view_handler.ActivityInfo.Name        = {resolve_info_default_view_handler.ActivityInfo.Name}");
            sb1.AppendLine($"         resolve_info_default_view_handler.ActivityInfo.ParentActivityName = {resolve_info_default_view_handler.ActivityInfo.ParentActivityName}");
            System.Diagnostics.Debug.WriteLine(sb1.ToString());
            #endif

            // Get all apps that can handle VIEW intents.
            IList<ResolveInfo> resolvedActivityList = pm.QueryIntentActivities(activityIntent, PackageInfoFlags.MatchAll);
            IList<string> packagesSupportingCustomTabs = new List<string>();
            foreach (ResolveInfo info in resolvedActivityList)
            {
                #if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"      activity resolved ");
                sb.AppendLine($"         info ");
                sb.AppendLine($"           Name = {info.ActivityInfo.Name}");
                sb.AppendLine($"           PackageName = {info.ActivityInfo.PackageName}");
                sb.AppendLine($"           ProcessName = {info.ActivityInfo.ProcessName}");
                System.Diagnostics.Debug.WriteLine(sb.ToString());
                #endif


                Intent serviceIntent = new Intent();
                // Android.Support.CustomTabs.action.CustomTabsService
                serviceIntent.SetAction(PackageManagerHelper.CustomTabsActionCustomTabsService);
                serviceIntent.SetPackage(info.ActivityInfo.PackageName);
                if (pm.ResolveService(serviceIntent, PackageInfoFlags.MatchAll) != null)
                {
                    #if DEBUG
                    StringBuilder sb2 = new StringBuilder();
                    sb2.AppendLine($"      activity resolved ");
                    sb2.AppendLine($"         info ");
                    sb2.AppendLine($"           Name = {info.ActivityInfo.Name}");
                    sb2.AppendLine($"           PackageName = {info.ActivityInfo.PackageName}");
                    sb2.AppendLine($"           ProcessName = {info.ActivityInfo.ProcessName}");
                    System.Diagnostics.Debug.WriteLine(sb2.ToString());
                    #endif

                    packagesSupportingCustomTabs.Add(info.ActivityInfo.PackageName);
                }
            }

            // Now packagesSupportingCustomTabs contains all apps that can handle both VIEW intents
            // and service calls.
            if (packagesSupportingCustomTabs.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine($" Packages Supporting CustomTabs Count = 0");
                sPackageNameToUse = null;
            }
            else if (packagesSupportingCustomTabs.Count == 1)
            {
                System.Diagnostics.Debug.WriteLine($" Packages Supporting CustomTabs Count = 1");
                System.Diagnostics.Debug.WriteLine($" Packages Supporting CustomTabs = {packagesSupportingCustomTabs[0]}");
                sPackageNameToUse = packagesSupportingCustomTabs[0];
                sPackageNamesToUse.Add(sPackageNameToUse);
            }
            else if
                (
                    // !TextUtils.IsEmpty(defaultViewHandlerPackageName)    // Android API
                    string.IsNullOrEmpty(defaultViewHandlerPackageName)     // .NET API
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

            for (int i = 0; i < sPackageNamesToUse.Count; i++)
            {
                PackagesSupportingCustomTabs.Add($"Detected {i}", sPackageNamesToUse[i]);
            }

            return sPackageNamesToUse;
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
                Log.Error
                        (
                            Xamarin.Auth.CustomTabsConfiguration.CustomTabsHelperAndroidLogTag, 
                            sb.ToString()
                        );
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
