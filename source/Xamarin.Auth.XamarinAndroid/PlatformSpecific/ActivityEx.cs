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
			var b = new AlertDialog.Builder (activity);
			b.SetMessage (message);
			b.SetTitle (title);
			b.SetNeutralButton ("OK", (s, e) => {
				((AlertDialog)s).Cancel ();
			});
			var alert = b.Create ();
			alert.Show ();
		}
	}
}

