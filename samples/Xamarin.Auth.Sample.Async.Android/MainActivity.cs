using Android.App;
using Android.Widget;
using Android.OS;

namespace Xamarin.Auth_Async_Sample
{
    [Activity(Label = "Xamarin.Auth_Async_Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private async void LoginToFacebook(bool allowCancel)
        {
            var facebookService = new FacebookService();
            var loginResult = await facebookService.LoginAsync(this, allowCancel);

            if (!loginResult.IsAuthenticated)
            {
                ShowMessage("Not Authenticated");
                return;
            }

            // in this step the username is always null, but i followed the original sample
            ShowMessage(string.Format("Authenticated {0}", loginResult.Account.Username));

            var userInfo = await facebookService.GetUserInfoAsync(loginResult.Account);
            ShowMessage(!string.IsNullOrEmpty(userInfo) ? string.Format("Logged as {0}", userInfo) : "Wasn´t possible to get the name.");
        }

        public void ShowMessage(string message)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetPositiveButton("Ok", (o, e) => { });
            builder.Create().Show();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var facebook = FindViewById<Button>(Resource.Id.FacebookButton);
            facebook.Click += delegate
            {
                LoginToFacebook(true);
            };

            var facebookNoCancel = FindViewById<Button>(Resource.Id.FacebookButtonNoCancel);
            facebookNoCancel.Click += delegate
            {
                LoginToFacebook(false);
            };
        }
    }
}

