using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Xamarin.Auth;

[assembly: PlatformAccountStore(typeof(UwpPlatformEngine))]

namespace Xamarin.Auth
{
    public class UwpPlatformEngine : IPlatformEngine
    {
        public IAccountStore Create(char[] password = null)
        {
            return new UwpAccountStore(password);
        }

        public async Task InvokeOnMainThread(Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
        }
        
        public IDisposable Disable100()
        {
            return new ServicePointManagerDispabler();
        }
    }
}