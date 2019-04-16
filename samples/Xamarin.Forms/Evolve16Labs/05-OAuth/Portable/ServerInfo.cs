using System;

namespace ComicBook
{
	public static class ServerInfo
	{
		public static Uri AuthorizationEndpoint = new Uri("http://xamuath.azurewebsites.net/oauth/authorize");
		public static Uri TokenEndpoint         = new Uri("http://xamuath.azurewebsites.net/oauth/token");
		public static Uri ApiEndpoint           = new Uri("http://xamuath.azurewebsites.net/api/whoami");
		public static Uri RedirectionEndpoint 		= new Uri("http://www.xamarin.com");
		public static string ClientId 				= "A8375B66";
		public static string ClientSecret 			= "A32D8C3CBE9A";
	}
}