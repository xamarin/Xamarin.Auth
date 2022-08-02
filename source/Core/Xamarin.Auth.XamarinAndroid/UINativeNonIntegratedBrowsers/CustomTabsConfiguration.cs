using System;

using Android.OS;
using Android.App;
using Android.Content;
using Android.Support.CustomTabs;
using System.Collections.Generic;
using System.Linq;

#if !AZURE_MOBILE_SERVICES
using Android.Support.CustomTabs.Chromium.SharedUtilities;
#else
using Android.Support.CustomTabs.Chromium.SharedUtilities._MobileServices;
#endif

namespace Xamarin.Auth
{
    /// <summary>
    /// Custom tabs configuration needed to pass Custom Tabs data for customization to the 
    /// Activity that launches CustomTabs
    /// 
    /// Too many complex classes to implement java interfaces
    ///     Serializable
    /// or
    ///     Parcellable (Android serialization aimed to increase performance)
    /// </summary>
    #if XAMARIN_CUSTOM_TABS_INTERNAL
    internal static partial class CustomTabsConfiguration
    #else
    public static partial class CustomTabsConfiguration
    #endif
    {
		static CustomTabsConfiguration()
        {
            color_xamarin_blue = new global::Android.Graphics.Color(0x34, 0x98, 0xdb);

            CustomTabsClosingMessage =
                        "If CustomTabs Login Screen does not close automatically"
                        + System.Environment.NewLine +
                        "close CustomTabs by navigating back to the app."
                        ;

            return;
        }

        static global::Android.Graphics.Color? color_xamarin_blue = null;

        public static void Initialize(global::Android.App.Activity a)
        {
            activity = a;

            List<string> packages = PackageManagerHelper.GetPackageNameToUse
                                                                (
                                                                    global::Android.App.Application.Context,
                                                                   "http://xamarin.com"
                                                                );
            PackagesSupportingCustomTabs = PackageManagerHelper.PackagesSupportingCustomTabs;
            PackageForCustomTabs = PackagesSupportingCustomTabs.FirstOrDefault().Value;

            CustomTabsActivityManager = new CustomTabsActivityManager(a);
            CustomTabsIntentBuilder = new CustomTabsIntent.Builder(CustomTabsActivityManager.Session);
            CustomTabActivityHelper = new CustomTabActivityHelper();

            return;
        }

        static global::Android.App.Activity activity = null;
        static CustomTabsActivityManager custom_tabs_activity_manager = null;
        static CustomTabsIntent.Builder custom_tabs_intent_builder = null;
        static CustomTabsIntent custom_tabs_intent = null;
        static CustomTabActivityHelper custom_tabs_activity_helper = null;

        public static global::Android.Net.Uri UriAndroidOS
        {
            get;
            set;
        }

		public static Dictionary<string, string> PackagesSupportingCustomTabs
		{
			get;
			set;
		}

		public static string PackageForCustomTabs
		{
			get;
			set;
		}

		public static CustomTabsIntent CustomTabsIntent
        {
            get
            {
                custom_tabs_intent = custom_tabs_intent_builder.Build();

                return custom_tabs_intent;
            }
            set
            {
                custom_tabs_intent = value;
            }
        }

        public static CustomTabsIntent.Builder CustomTabsIntentBuilder
        {
            get
            {
                return custom_tabs_intent_builder;
            }
            set
            {
                custom_tabs_intent_builder = value;
            }
        }

        public static CustomTabsActivityManager CustomTabsActivityManager
        {
            get
            {
                return custom_tabs_activity_manager;
            }
            set
            {
                custom_tabs_activity_manager = value;
            }
        }

        public static CustomTabActivityHelper CustomTabActivityHelper
        {
            get
            {
                return custom_tabs_activity_helper;
            }
            set
            {
                custom_tabs_activity_helper = value;
            }
        }


		public static string CustomTabsHelperUri
		{
			get;
			set;
		} = "http://xamarin.com";


		public static string CustomTabsHelperAndroidLogTag
		{
			get;
			set;
		} = "CustomTabsHelper";




		public static string CustomTabsClosingMessage
        {
        	get;
        	set;
        }

        public static string ActionLabel
        {
            get;
            set;
        } = "Action Label";

        public static string MenuItemTitle
        {
        	get;
        	set;
        } = "Menu Item Title";

        public static global::Android.Graphics.Color ToolbarColor
        {
            get;
        	set;
        }

        public static bool IsShowTitleUsed
        {
        	get;
        	set;
        }  = true;

        public static bool IsUrlBarHidingUsed
        {
        	get;
        	set;
        }  = true;

        public static bool IsDefaultShareMenuItemUsed
        {
        	get;
        	set;
        } = true;

        public static global::Android.Graphics.Bitmap ActionButtonIconBitmap
        {
        	get;
        	set;
        }

        public static bool IsActionButtonUsed
        {
        	get;
        	set;
        } = true;

        public static bool IsActionBarToolbarIconUsed
        {
        	get;
        	set;
        } = true;

        public static global::Android.Graphics.Bitmap ActionBarToolbarIconBitmap
        {
        	get;
        	set;
        }

        public static bool IsCloseButtonIconUsed
        {
        	get;
        	set;
        } = true;

        public static bool AreAnimationsUsed
        {
            get;
            set;
        } = true;

        public static WebViewFallback WebViewFallback
        {
        	get;
        	set;
        } = new WebViewFallback();

        public static bool IsWarmUpUsed
        {
        	get;
        	set;
        }

        public static bool IsPrefetchUsed
        {
        	get;
        	set;
        }

        private static global::Android.Content.ActivityFlags activity_flags;

        /// <summary>
        /// ActivityFlags for launching WebAuthenticatorNativeBrowserActivity
        /// </summary>
        /// <value>The activity flags.</value>
        public static global::Android.Content.ActivityFlags ActivityFlags
        {
            get
            {
                return activity_flags;
            }
            set
            {
                activity_flags = value;

                return;
            }
        }

        public static void UICustomization()
        {
            ActivityFlags = 
                    global::Android.Content.ActivityFlags.NoHistory
                    |
                    global::Android.Content.ActivityFlags.SingleTop
                    |
                    global::Android.Content.ActivityFlags.NewTask
                    |
                    global::Android.Content.ActivityFlags.ClearTop
                    ;

            /*
            CustomTabsIntentBuilder
            	.SetToolbarColor(color_xamarin_blue)
            	.SetShowTitle(true)
            	.EnableUrlBarHiding()
            	.AddDefaultShareMenuItem()
            	;
            */

            if (ToolbarColor != null)
            {
                CustomTabsIntentBuilder
                    .SetToolbarColor(ToolbarColor)
                        ;
            }
            if (IsShowTitleUsed == true)
            {
                CustomTabsIntentBuilder
                    .SetShowTitle(true)
                    ;
            }
            if (IsUrlBarHidingUsed == true)
            {
                CustomTabsIntentBuilder
                    .EnableUrlBarHiding()
                    ;
            }
            if (IsDefaultShareMenuItemUsed == true)
            {
                CustomTabsIntentBuilder
                    .AddDefaultShareMenuItem()
                    ;
            }


            if (IsActionButtonUsed == true)
            {
                PendingIntent pi = null;
                //............................................................
                // Action Button Bitmap
                // Generally do not decode bitmaps in the UI thread. 
                // Decoding it in the UI thread to keep the example short.
                ActionButtonIconBitmap = global::Android.Graphics.BitmapFactory.DecodeResource
                                                (
                                                    activity.Resources,
                                                    global::Android.Resource.Drawable.IcMenuShare
                                                );
                pi = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_ACTION_BUTTON);

                CustomTabsIntentBuilder
                    .SetActionButton(ActionButtonIconBitmap, ActionLabel, pi)
                    ;

                // TODO: ufff leaks? 
                // ActionButtonIconBitmap = null;
                // pi = null;
            }


            //............................................................
            // menu
            if (!string.IsNullOrEmpty(MenuItemTitle))
            {
                PendingIntent pi_menu_item = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_MENU_ITEM);
                CustomTabsIntentBuilder.AddMenuItem(MenuItemTitle, pi_menu_item);

                // TODO: ufff leaks? 
                // ActionButtonIconBitmap = null;
                // pi_menu_item = null;
            }
            //............................................................


            int TOOLBAR_ITEM_ID = 1;

            if (IsActionBarToolbarIconUsed == true)
            {
                //............................................................
                // Action Label Toolbar
                // Generally do not decode bitmaps in the UI thread. 
                // Decoding it in the UI thread to keep the example short.
                ActionBarToolbarIconBitmap = global::Android.Graphics.BitmapFactory.DecodeResource
                                                    (
                                                        activity.Resources,
                                                        global::Android.Resource.Drawable.IcMenuShare
                                                    );
                PendingIntent pi = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_ACTION_BUTTON);
                pi = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_TOOLBAR);

                CustomTabsIntentBuilder.AddToolbarItem(TOOLBAR_ITEM_ID, ActionBarToolbarIconBitmap, ActionLabel, pi);
                //............................................................

                // TODO: ufff leaks? 
                // ActionBarToolbarIconBitmap = null;
                // pi = null;

            }

            if (IsCloseButtonIconUsed == true)
            {
                //............................................................
                // Custom Back Button Bitmap
                // Generally do not decode bitmaps in the UI thread. 
                // Decoding it in the UI thread to keep the example short.
                CustomTabsIntentBuilder
                    .SetCloseButtonIcon
                        (
                            global::Android.Graphics.BitmapFactory.DecodeResource
                                                        (
                                                            activity.Resources,
                                                            Resource.Drawable.ic_arrow_back
                                                        )
                        );
                //............................................................
            }

            if (AreAnimationsUsed == true)
            {
                //............................................................
                // Animations
                CustomTabsIntentBuilder
                    .SetStartAnimations
                            (
                                activity,
                                Resource.Animation.slide_in_right,
                                Resource.Animation.slide_out_left
                            )
                    .SetExitAnimations
                            (
                                activity,
                                global::Android.Resource.Animation.SlideInLeft,
                                global::Android.Resource.Animation.SlideOutRight
                            );
                //............................................................
            }

            //............................................................
            // TODO: bottom bar
            //............................................................
            //------------------------------------------------------------

            if (UriAndroidOS != null)
            {
                //------------------------------------------------------------
                //      Optimisations
                //      [OPTIONAL] [RECOMENDED]
                //          *   WarmUp, 
                //          *   Prefetching
                //
                bool launchable_uri = CustomTabActivityHelper.MayLaunchUrl
                                            (
                                                UriAndroidOS,
                                                null,
                                                null
                                            );
            }

            //------------------------------------------------------------
            //  CustomTabsIntent property getter will call 
            //      CustomTabsIntent.Builder.Build() internally
            CustomTabsIntent.Intent.AddFlags(ActivityFlags);

            return;
        }

        public static PendingIntent CreatePendingIntent(int actionSourceId)
        {
            Intent actionIntent = new Intent
                                        (
                                            activity.ApplicationContext, 
                                            typeof(CustomTabsActionsBroadcastReceiver)
                                        );
            actionIntent.PutExtra
                        (
                            CustomTabsActionsBroadcastReceiver.KEY_ACTION_SOURCE, 
                            actionSourceId
                        );

            PendingIntent broadcast = PendingIntent.GetBroadcast
                                                    (
                                                       activity.ApplicationContext,
                                                       actionSourceId,
                                                       actionIntent,
                                                       PendingIntentFlags.Immutable
                                                    );
            return broadcast;
        }

    }
}
