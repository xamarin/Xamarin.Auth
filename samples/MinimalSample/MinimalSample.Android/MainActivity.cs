using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace MinimalSample.Droid
{
    [Activity(Label = "MinimalSample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            OAuthLoginPresenter.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
    }
}
