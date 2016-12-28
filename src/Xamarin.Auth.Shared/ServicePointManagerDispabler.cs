using System;
using System.Net;

namespace Xamarin.Auth
{
    public class ServicePointManagerDispabler : IDisposable
    {
        private bool _prevValue;

        public ServicePointManagerDispabler()
        {
#if !WINDOWS_UWP
            _prevValue = ServicePointManager.Expect100Continue;
            ServicePointManager.Expect100Continue = false;
#endif
        }

        public void Dispose()
        {
#if !WINDOWS_UWP
            ServicePointManager.Expect100Continue = _prevValue;
#endif
        }
    }
}
