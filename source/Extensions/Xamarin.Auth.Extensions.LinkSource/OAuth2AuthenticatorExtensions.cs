using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Xamarin.Auth
{
	public static class OAuth2AuthenticatorExtensions
	{
        # region
        //---------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		Adding method to request a refresh token #79
        ///		https://github.com/xamarin/Xamarin.Auth/pull/79
        ///		
        /// <summary>
        /// Method that requests a new access token based on an initial refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token, typically from the <see cref="AccountStore"/>'s refresh_token property</param>
        /// <returns>Time in seconds the refresh token expires in</returns>
		public static Task<int> RequestRefreshTokenAsync(this OAuth2Authenticator authenticator,string refreshToken)
        {
            var queryValues = new Dictionary<string, string>
            {
                {"refresh_token", refreshToken},
				{"client_id", authenticator.ClientId},
                {"grant_type", "refresh_token"}
            };

			if (!string.IsNullOrEmpty(authenticator.ClientSecret))
            {
				queryValues["client_secret"] = authenticator.ClientSecret;
            }

			return authenticator.RequestAccessTokenAsync(queryValues)
					.ContinueWith(result =>
		            {
		            var accountProperties = result.Result;

					authenticator.OnRetrievedAccountProperties(accountProperties);

		            return int.Parse(accountProperties["expires_in"]);
		            });
        }
        //---------------------------------------------------------------------------------------
        # endregion

	}
}

