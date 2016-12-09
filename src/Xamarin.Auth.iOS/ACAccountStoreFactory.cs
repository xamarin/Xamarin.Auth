using System;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Auth;
using Xamarin.Auth.iOS;


[assembly: PlatformAccountStore(typeof(KeyChainPlatformEngine))]

namespace Xamarin.Auth.iOS
{
    public class KeyChainPlatformEngine : IPlatformEngine
    {
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
    }
}