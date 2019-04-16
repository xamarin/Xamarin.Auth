using System;

using UIContext = Android.Content.Context;

namespace Plugin.Threading
{
    public partial class UIThreadRunInvoker
    {
        public UIContext Context
        {
            get;
            set;
        }

        public UIThreadRunInvoker(UIContext c)
        {
            this.Context = c;

            return;
        }

        public void BeginInvokeOnUIThread(Action action)
        {
            #if __ANDROID__
            global::Android.App.Activity a = Context as global::Android.App.Activity;
            if (a != null)
            {
                a.RunOnUiThread(action);
            }
            else
            {
                action();
            }
            #endif

            return;
        }
    }
}