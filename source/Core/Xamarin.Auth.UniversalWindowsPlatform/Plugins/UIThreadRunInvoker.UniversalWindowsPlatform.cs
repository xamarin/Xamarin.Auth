using System;

namespace Plugin.Threading
{
    public partial class UIThreadRunInvoker
    {
        public void BeginInvokeOnUIThread(Action action)
        {
            #if NETFX_CORE && WINDOWS_UWP         // Universal Windows Platform (UWP)
            // using Windows.ApplicationModel.Core;
            var dispatcher =
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                    //Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher
                    //Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher
                    //Window.Current.Dispatcher
                    ;
            Windows.Foundation.IAsyncAction t = dispatcher.RunAsync
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