namespace Xamarin.Auth.Presenters.XamarinAndroid
{
	public class PlatformOAuthLoginPresenter 
	{
		public void Login (Authenticator authenticator)
		{
			AuthenticationConfiguration.Context.StartActivity (authenticator.GetUI(AuthenticationConfiguration.Context));
		}
	}
}