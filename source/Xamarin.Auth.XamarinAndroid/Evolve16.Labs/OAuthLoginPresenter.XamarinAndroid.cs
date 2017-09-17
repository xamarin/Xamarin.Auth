namespace Xamarin.Auth.Presenters.XamarinAndroid
{
    #if XAMARIN_AUTH_INTERNAL
    internal class PlatformOAuthLoginPresenter
    #else
    public class PlatformOAuthLoginPresenter
    #endif
    {
        public void Login(Authenticator authenticator)
        {
            AuthenticationConfiguration.Context.StartActivity(authenticator.GetUI(AuthenticationConfiguration.Context));
        }
    }
}