using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ComicBook
{
    [Activity(Label = "ComicBook", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //-----------------------------------------------------------------------------------------------
            // Xamarin.Auth initialization

            // Presenters Initialization
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);

            // Xamarin.Auth CustomTabs Initialization/Customisation
            global::Xamarin.Auth.CustomTabsConfiguration.ActionLabel = null;
            global::Xamarin.Auth.CustomTabsConfiguration.MenuItemTitle = null;
            global::Xamarin.Auth.CustomTabsConfiguration.AreAnimationsUsed = true;
            global::Xamarin.Auth.CustomTabsConfiguration.IsShowTitleUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsUrlBarHidingUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsCloseButtonIconUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsActionButtonUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsActionBarToolbarIconUsed = false;
            global::Xamarin.Auth.CustomTabsConfiguration.IsDefaultShareMenuItemUsed = false;

            global::Android.Graphics.Color color_xamarin_blue;
            color_xamarin_blue = new global::Android.Graphics.Color(0x34, 0x98, 0xdb);
            global::Xamarin.Auth.CustomTabsConfiguration.ToolbarColor = color_xamarin_blue;


            // ActivityFlags for tweaking closing of CustomTabs
            // please report findings!
            global::Xamarin.Auth.CustomTabsConfiguration.
               ActivityFlags = 
                    global::Android.Content.ActivityFlags.NoHistory
                    |
                    global::Android.Content.ActivityFlags.SingleTop
                    |
                    global::Android.Content.ActivityFlags.NewTask
                    ;

            global::Xamarin.Auth.CustomTabsConfiguration.IsWarmUpUsed = true;
            global::Xamarin.Auth.CustomTabsConfiguration.IsPrefetchUsed = true;



            //-----------------------------------------------------------------------------------------------

            LoadApplication(new App());
        }
    }
}