using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Xamarin.Auth
{
    public interface IPlatformEngine
    {
        IAccountStore Create(char[] password = null);
        Task InvokeOnMainThread(Action action);
        IDisposable Disable100();
    }
#if PLATFORM_ANDROID
   
    internal class PlatformEngine : IPlatformEngine
    {
        private static readonly IPlatformEngine INSTANCE = new PlatformEngine();

        public IAccountStore Create(char[] password = null)
        {
            return new AndroidAccountStore(password);
        }
        
        public Task InvokeOnMainThread(Action action)
        {
            // http://stackoverflow.com/questions/12850143/android-basics-running-code-in-the-ui-thread
            new Handler(Looper.MainLooper).Post(action);
            return Task.FromResult(true);
        }

        public IDisposable Disable100()
        {
            return new ServicePointManagerDispabler();
        }
    }
#elif WINDOWS_UWP
    internal class PlatformEngine : IPlatformEngine
    {
        private static readonly IPlatformEngine INSTANCE = new PlatformEngine();

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
#else
    internal class PlatformEngine : IPlatformEngine
    {
        private static readonly IPlatformEngine INSTANCE = new PlatformEngine();

        public IAccountStore Create(char[] password = null)
        {
            throw new NotImplementedException();
        }

        public Task InvokeOnMainThread(Action action)
        {
            throw new NotImplementedException();
        }

        public IDisposable Disable100()
        {
            throw new NotImplementedException();
        }
    }
#endif
}
