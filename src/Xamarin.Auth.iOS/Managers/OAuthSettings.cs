namespace Xamarin.Auth
{
	public class OAuthSettings
	{

		public OAuthSettings(
			string clientId, string clientSecret, string scope,
			string authorizeUrl, string redirectUrl, string userInfoUrl,
			string accessTokenUrl)
		{
			ClientId = clientId;
			ClientSecret = clientSecret;
			Scope = scope;
			AuthorizeUrl = authorizeUrl;
			RedirectUrl = redirectUrl;
			UserInfoUrl = userInfoUrl;
			AccessTokenUrl = accessTokenUrl;
		}

		public OAuthSettings(
			string clientId, string scope, string authorizeUrl, string redirectUrl, string userInfoUrl)
		{
			ClientId = clientId;
			Scope = scope;
			AuthorizeUrl = authorizeUrl;
			RedirectUrl = redirectUrl;
			UserInfoUrl = userInfoUrl;
		}

		public string ClientId {get; private set;}
		public string ClientSecret {get; private set;}
		public string Scope {get; private set;}
		public string AuthorizeUrl {get; private set;}
		public string RedirectUrl {get; private set;}
		public string UserInfoUrl { get; private set; }
		public string AccessTokenUrl { get; private set; }
	}
}