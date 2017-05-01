namespace Xamarin.Auth
{
	public class PlatformOAuthLoginPresenter 
	{
		public void Login (Authenticator authenticator)
		{
			Auth.Context.StartActivity (authenticator.GetUI(Auth.Context));
		}
	}
}