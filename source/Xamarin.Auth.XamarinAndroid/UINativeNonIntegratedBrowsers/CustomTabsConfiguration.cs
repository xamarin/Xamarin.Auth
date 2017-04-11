using System;

using Android.OS;
using Android.App;
using Android.Support.CustomTabs;
using Android.Support.CustomTabs.Chromium.SharedUtilities;
using Android.Content;

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
    public static partial class CustomTabsConfiguration
    {
        static CustomTabsConfiguration()
        {
        }

        public static void Initialize(global::Android.App.Activity activity)
        {
            CustomTabsActivityManager = new CustomTabsActivityManager(activity);
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

        public static WebViewFallback WebViewFallback
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
        } = "Action Label";

        private const int TOOLBAR_ITEM_ID = 1;

        static global::Android.Graphics.Color color_xamarin_blue = new global::Android.Graphics.Color(0x34, 0x98, 0xdb);

        public static void UICustomization
                            (
                            )
        {
            //------------------------------------------------------------
            // WalkThrough Step 2.2
            //      UI Customisation
            //      [OPTIONAL] 
            // CustomTabsIntent.Builder
            CustomTabsIntentBuilder
                .SetToolbarColor(color_xamarin_blue)
                .SetShowTitle(true)
                .EnableUrlBarHiding()
                .AddDefaultShareMenuItem()
                ;

            global::Android.Graphics.Bitmap icon = null;
            PendingIntent pi = null;
            //............................................................
            // Action Button Bitmap
            // Generally do not decode bitmaps in the UI thread. 
            // Decoding it in the UI thread to keep the example short.
            icon = global::Android.Graphics.BitmapFactory.DecodeResource
                                            (
                                                activity.Resources,
                                                global::Android.Resource.Drawable.IcMenuShare
                                            );
            pi = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_ACTION_BUTTON);

            CustomTabsIntentBuilder
                .SetActionButton(icon, ActionLabel, pi)
                ;
            //............................................................

            //............................................................
            // menu
            PendingIntent pi_menu_item = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_MENU_ITEM);

            CustomTabsIntentBuilder
                .AddMenuItem(MenuItemTitle, pi_menu_item)
                ;
            //............................................................

            //............................................................
            // Action Label Toolbar
            // Generally do not decode bitmaps in the UI thread. 
            // Decoding it in the UI thread to keep the example short.
            icon = global::Android.Graphics.BitmapFactory.DecodeResource
                                                (
                                                    activity.Resources,
                                                    global::Android.Resource.Drawable.IcMenuShare
                                                );
            pi = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_TOOLBAR);

            CustomTabsIntentBuilder
                .AddToolbarItem(TOOLBAR_ITEM_ID, icon, ActionLabel, pi)
                ;
            //............................................................

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


            //............................................................
            // TODO: bottom bar
            //............................................................
            //------------------------------------------------------------

            //------------------------------------------------------------
            // WalkThrough Step 2.2
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
            //------------------------------------------------------------
            //  CustomTabsIntent property getter will call 
            //      CustomTabsIntent.Builder.Build() internally
            CustomTabsIntent
                .Intent.AddFlags(global::Android.Content.ActivityFlags.NoHistory)
                ;

            //------------------------------------------------------------
            // WalkThrough Step 3
            //      Launching UI
            //      [REQUIRED] 
            // ensures the intent is not kept in the history stack, which makes
            // sure navigating away from it will close it

            //------------------------------------------------------------

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
                                                       0
                                                    );
            return broadcast;
        }

    }
}
