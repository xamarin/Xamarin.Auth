using Android.App;
using Android.Content;

namespace Xamarin.Auth
{
    partial class AccountStore
    {
        public static AccountStore Create(Context context) => PlatformCreate(context, null);

        public static AccountStore Create(Context context, string password) => PlatformCreate(context, password);

        public static AccountStore Create(string password) => PlatformCreate(null, password);

        private static AccountStore PlatformCreate(Context context, string password)
        {
            context = context ?? Application.Context;

            if (!string.IsNullOrEmpty(password))
                return new AndroidAccountStore(context, password);

            if (!string.IsNullOrEmpty(StorePassword))
                return new AndroidAccountStore(context, StorePassword);

            return new AndroidAccountStore(context);
        }
    }
}
