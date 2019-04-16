using System;

namespace Plugin.Threading
{
    public partial class UIThreadRunInvoker
    {
        public void BeginInvokeOnUIThread(Action action)
        {
            #if NETFX_CORE && WINDOWS_APP         // Windows WinRT 8.1
            // using Windows.ApplicationModel.Core;
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