using Android.Content;
using Android.OS;

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
            Context.StartActivity(authenticator.GetUI(Context));
        }
    }
}
