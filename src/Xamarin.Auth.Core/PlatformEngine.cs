using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if PLATFORM_ANDROID
using Android.OS;
#elif WINDOWS_UWP
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#elif PLATFORM_IOS
using UIKit;
#endif

namespace Xamarin.Auth
{
    internal class Platform : IPlatformEngine
    {
        internal static IPlatformEngine Engine = new Platform();

#if PLATFORM_ANDROID
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
#elif PLATFORM_IOS
        public IAccountStore Create(char[] password = null)
        {
            return new KeyChainAccountStore(password);
        }

        public Task InvokeOnMainThread(Action action)
        {
            UIApplication.SharedApplication.BeginInvokeOnMainThread(delegate {
                action();
            });
            return Task.FromResult(true);
        }

        public IDisposable Disable100()
        {
            return new ServicePointManagerDispabler();
        }

#elif WINDOWS_UWP
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
#else
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
#endif
    }
}
