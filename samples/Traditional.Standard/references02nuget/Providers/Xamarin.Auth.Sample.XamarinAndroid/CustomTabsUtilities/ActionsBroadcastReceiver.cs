
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xamarin.Auth.Sample.XamarinAndroid
{
    [BroadcastReceiver]
    public class ActionsBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();

            string url = intent.DataString;
			if (url != null)
			{
				string toastText = GetToastText
                                    (
                                        context, 
                                        intent.GetIntExtra
                                        (
                                            KEY_ACTION_SOURCE, 
                                            -1
                                        ), 
                                        url
                                    );
				Toast.MakeText(context, toastText, ToastLength.Short).Show();
			}

            return;
		}

		public const string KEY_ACTION_SOURCE = "org.chromium.customtabsdemos.ACTION_SOURCE";
		public const int ACTION_ACTION_BUTTON = 1;
		public const int ACTION_MENU_ITEM = 2;
		public const int ACTION_TOOLBAR = 3;

		private string GetToastText(Context context, int actionId, string url)
		{
			switch (actionId)
			{
				case ACTION_ACTION_BUTTON:
					return context.GetString(Resource.String.action_button_toast_text, url);
				case ACTION_MENU_ITEM:
					return context.GetString(Resource.String.menu_item_toast_text, url);
				case ACTION_TOOLBAR:
					return context.GetString(Resource.String.toolbar_toast_text, url);
				default:
					return context.GetString(Resource.String.unknown_toast_text, url);
			}

            return "Something is wrong?!?!";
		}
	}
}
