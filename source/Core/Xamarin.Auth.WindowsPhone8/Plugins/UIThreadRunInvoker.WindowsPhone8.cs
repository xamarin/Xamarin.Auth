using System;

namespace Plugin.Threading
{
    public partial class UIThreadRunInvoker
    {
        public void BeginInvokeOnUIThread(Action action)
        {
            #if WINDOWS_PHONE && SILVERLIGHT      // Windows Phone Silverlight (8 and 8.1)
            var dispatcher =
                    Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher
                    //Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher
                    //Window.Current.Dispatcher
                    ;
            dispatcher.RunAsync
                             (
                                Windows.UI.Core.CoreDispatcherPriority.Normal,
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