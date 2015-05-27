using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Xamarin.Auth
{
	public abstract class AlaskaLoginManager
	{
	
		public enum SSO_ENVIROMENT {
			BUILDBASE = 0,
			TEST = 1,
			PROD = 2
		}

		public string EmployeeId { get; set; }
		public string Email { get; set; }
		public string FullName { get; set; }
		public OAuthSettings OAuthSettings { get; private set; }
		const string BASE_URL_TEST = "https://fedauthstg.alaskasworld.com";
		const string BASE_URL_PROD = "https://fedauth.alaskasworld.com";

		const string SCOPE = "openid profile email";
		public SSO_ENVIROMENT SSOEnvironment { get; private set; }
		protected List<String> userInfoKeys = new List<string>();

		string AUTHORIZE_URL, REDIRECT_URL, USERINFO_URL, ACCESSTOKEN_URL; 

		public AlaskaLoginManager() {}

		/// <summary>
		/// Init the specified clientId, clientSecret, redirectURL, userInfoKeys and Environment.
		/// </summary>
		/// <param name="clientId">Client identifier.</param>
		/// <param name="clientSecret">Client secret.</param>
		/// <param name="redirectURL">Redirect URL.</param>
		/// <param name="userInfoKeys">User info keys.</param>
		/// <param name="env">Environment TEST or PROD</param>
		public void Init(string clientId, string clientSecret, string redirectURL, List<string> userInfoKeys, SSO_ENVIROMENT env) {
			SSOEnvironment = env;
			this.userInfoKeys = userInfoKeys;
			SetEndpoints (env, redirectURL);
			OAuthSettings = new OAuthSettings (clientId, clientSecret, SCOPE, AUTHORIZE_URL, REDIRECT_URL, USERINFO_URL, ACCESSTOKEN_URL);
		}

		public AlaskaLoginManager(string clientId, string clientSecret, string redirectURL, SSO_ENVIROMENT env)
		{
			SetEndpoints (env, redirectURL);
			OAuthSettings = new OAuthSettings (clientId, clientSecret, SCOPE, AUTHORIZE_URL, REDIRECT_URL, USERINFO_URL, ACCESSTOKEN_URL);
		}

		public AlaskaLoginManager(string clientId, string redirectURL, SSO_ENVIROMENT env)
		{
			SetEndpoints (env, redirectURL);
			OAuthSettings = new OAuthSettings (clientId, SCOPE, AUTHORIZE_URL, REDIRECT_URL, USERINFO_URL);
		}

		public void SetEndpoints(SSO_ENVIROMENT env, string redirectURL) {
			switch (env) {
			case SSO_ENVIROMENT.BUILDBASE:
				#if DEBUG
				AUTHORIZE_URL = BASE_URL_PROD + "/as/authorization.oauth2";
				USERINFO_URL = BASE_URL_PROD + "/idp/userinfo.openid";
				ACCESSTOKEN_URL = BASE_URL_PROD + "/as/token.oauth2";
				#else
				AUTHORIZE_URL = BASE_URL_TEST + "/as/authorization.oauth2";
				USERINFO_URL = BASE_URL_TEST + "/idp/userinfo.openid";
				ACCESSTOKEN_URL = BASE_URL_TEST + "/as/token.oauth2";
				#endif
				break;
			case SSO_ENVIROMENT.TEST:
				AUTHORIZE_URL = BASE_URL_TEST + "/as/authorization.oauth2";
				USERINFO_URL = BASE_URL_TEST + "/idp/userinfo.openid";
				ACCESSTOKEN_URL = BASE_URL_TEST + "/as/token.oauth2";
				break;
			case SSO_ENVIROMENT.PROD:
				AUTHORIZE_URL = BASE_URL_PROD + "/as/authorization.oauth2";
				USERINFO_URL = BASE_URL_PROD + "/idp/userinfo.openid";
				ACCESSTOKEN_URL = BASE_URL_PROD + "/as/token.oauth2";
				break;
			}
			REDIRECT_URL = redirectURL;
		}

		public bool IsAuthenticated {
			get { return !string.IsNullOrWhiteSpace(_Token); }
		}

		string _Token;
		public string Token {
			get { return _Token; }
		}

		public void SaveToken(string token)
		{
			_Token = token;
		}

//		public void LoginOnBehalf(string employeeId) {
//			this.EmployeeId = employeeId;
//			OnSuccessfulLogin (employeeId);
//		}

		abstract public bool HasCacheAuthToken();


		abstract public void LoginToAlaska (bool allowCancel);

		public virtual void Logout()
		{
			_Token = null;
		}

		/// <summary>
		/// The callback method after successful login
		/// </summary>
		public Action<Dictionary<string,string>> OnSuccessfulLogin;

	}
}