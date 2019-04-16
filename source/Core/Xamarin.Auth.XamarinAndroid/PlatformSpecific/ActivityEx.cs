using System;
using Android.App;
using Android.Views;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Utilities.Android
#else
namespace Xamarin.Utilities._MobileServices.Android
#endif
{
    public static class ActivityEx
    {
        public static void ShowError(this Activity activity, string title, Exception exception)
        {
            var ex = exception;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            ShowError(activity, title, ex.Message);

            return;
        }

        public static void ShowError(this Activity activity, string title, string message)
        {
            /*
			 	try/catch added to solve
				Android.Views.WindowManagerBadTokenException: 
					Unable to add window -- token android.os.BinderProxy@52907c38 is not valid; 
					is your activity running?

				https://bugzilla.xamarin.com/show_bug.cgi?id=37870
				https://forums.xamarin.com/discussion/11491/xamarin-auth-alway-throws-exception-on-android

                should be also prevented by checking if the activity is finishing (in that case avoid adding the alert dialog)
			*/
            try
            {
                if (activity.IsFinishing)
                    return;

                var b = new AlertDialog.Builder(activity);
                b.SetMessage(message);
                b.SetTitle(title);
                b.SetNeutralButton("OK", (s, e) =>
                {
                    ((AlertDialog)s).Cancel();
                });
                var alert = b.Create();
                alert.Show();
            }
            catch (WindowManagerBadTokenException ex)
            {
                string msg = ex.Message;
                global::Android.Util.Log.Error("Xamarin.Auth", "Error: {0}:{1} - {2}", title, message, msg);
            }

            return;
        }
    }
}

