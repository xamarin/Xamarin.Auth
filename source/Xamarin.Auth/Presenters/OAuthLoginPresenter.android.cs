using Android.Content;
using Android.OS;
using System;

namespace Xamarin.Auth.Presenters
{
    partial class OAuthLoginPresenter
    {
        internal static Context Context { get; set; }

        public static void Init(Context context, Bundle bundle)
        {
            // TODO: have a better init that hooks up the activity lifecycle events
            Context = context;
        }

        private void PlatformLoginImplementation(Authenticator authenticator)
        {
            EnsureInitialized();

            Context.StartActivity(authenticator.GetUI(Context));
        }

        private static void EnsureInitialized()
        {
            if (Context == null)
            {
                throw new InvalidOperationException(
                    "Xamarin.Auth was not properly initialized. " +
                    "Make an overload of OAuthLoginPresenter.Init() was called.");
            }
        }
    }
}
