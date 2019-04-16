using System;

namespace Plugin.Threading
{
    public partial class UIThreadRunInvoker
    {
        public void BeginInvokeOnUIThread(Action action)
        {
            #if NETFX_CORE && WINDOWS_PHONE       // Windows Phone WinRT 8.1
            // using Windows.ApplicationModel.Core;
            var dispatcher = 
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
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