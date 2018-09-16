using System;
using System.Threading;
using System.Threading.Tasks;

#if __ANDROID__
using Android.OS;
#elif __IOS__
using Foundation;
#elif WINDOWS_UWP
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#endif

namespace Xamarin.Auth
{
    internal static class MainThread
    {
#if __ANDROID__
        static Handler handler;
#endif

        public static void BeginInvokeOnMainThread(Action action)
        {
            if (IsMainThread)
                action();
            else
                PlatformBeginInvokeOnMainThread(action);
        }

        internal static bool IsMainThread
        {
            get
            {
#if __ANDROID__
                if ((int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.M)
                    return Looper.MainLooper.IsCurrentThread;
                return Looper.MyLooper() == Looper.MainLooper;
#elif __IOS__
                return NSThread.Current.IsMainThread;
#elif WINDOWS_UWP
                return CoreApplication.MainView.CoreWindow?.Dispatcher?.HasThreadAccess ?? false;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        private static void PlatformBeginInvokeOnMainThread(Action action)
        {
#if __ANDROID__
            if (handler?.Looper != Looper.MainLooper)
                handler = new Handler(Looper.MainLooper);
            handler.Post(action);
#elif __IOS__
            NSRunLoop.Main.BeginInvokeOnMainThread(action.Invoke);
#elif WINDOWS_UWP
            var dispatcher = CoreApplication.MainView.CoreWindow?.Dispatcher;
            if (dispatcher == null)
                throw new InvalidOperationException("Unable to find main thread.");
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).AsTask().WatchForError();
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if WINDOWS_UWP
        private static void WatchForError(this Task self)
        {
            var context = SynchronizationContext.Current;
            if (context == null)
                return;

            self.ContinueWith(
                t => ThrowException(t.Exception),
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default);

            void ThrowException(AggregateException ex)
            {
                var exception = ex.InnerExceptions.Count > 1 ? ex : ex.InnerException;
                context.Post(e => throw (Exception)e, exception);
            }
        }
#endif
    }
}
