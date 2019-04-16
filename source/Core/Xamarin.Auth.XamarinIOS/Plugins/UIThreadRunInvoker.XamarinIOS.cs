using System;

namespace Plugin.Threading
{
    public partial class UIThreadRunInvoker
    {

        public void BeginInvokeOnUIThread(Action action)
        {
            #if __IOS__
                #if !__UNIFIED__
                MonoTouch.
            #endif
            UIKit.UIApplication.SharedApplication.BeginInvokeOnMainThread
                (
                    delegate
                    {
                        action();
                    }
                );
            #endif

            return;
        }
    }
}