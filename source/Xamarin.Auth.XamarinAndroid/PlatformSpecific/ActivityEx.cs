using System;
using Android.App;

namespace Xamarin.Utilities.Android
{
	internal static class ActivityEx
	{
		public static void ShowError (this Activity activity, string title, Exception exception)
		{
			var ex = exception;
			while (ex.InnerException != null) {
				ex = ex.InnerException;
			}
			ShowError (activity, title, ex.Message);
		}

		public static void ShowError (this Activity activity, string title, string message)
		{
			/*
			 	try/catch added to solve
				Android.Views.WindowManagerBadTokenException: 
					Unable to add window -- token android.os.BinderProxy@52907c38 is not valid; 
					is your activity running?

				https://bugzilla.xamarin.com/show_bug.cgi?id=37870
				https://forums.xamarin.com/discussion/11491/xamarin-auth-alway-throws-exception-on-android
			*/
			try
			{
				var b = new AlertDialog.Builder (activity);
				b.SetMessage (message);
				b.SetTitle (title);
				b.SetNeutralButton ("OK", (s, e) => {
					((AlertDialog)s).Cancel ();
				});
				var alert = b.Create ();
				alert.Show ();
			}
			catch (Exception ex) 
			{
				global::Android.Util.Log.Error("Xamarin.Auth", "Error: {0}:{1}", title, message);
			}

			return;
		}
	}
}

