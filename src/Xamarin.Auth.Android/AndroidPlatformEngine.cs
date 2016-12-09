using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Xamarin.Auth;
using Xamarin.Auth.Android;


[assembly: PlatformAccountStore(typeof(AndroidPlatformEngine))]

namespace Xamarin.Auth.Android
{
    public class AndroidPlatformEngine : IPlatformEngine
    {
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
}