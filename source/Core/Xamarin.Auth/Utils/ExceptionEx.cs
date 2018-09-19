using System;

namespace Xamarin.Auth
{
    internal static class ExceptionEx
    {
        public static string GetInitialMessage(this Exception error)
        {
            var e = error;
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e.Message;
        }
    }
}
