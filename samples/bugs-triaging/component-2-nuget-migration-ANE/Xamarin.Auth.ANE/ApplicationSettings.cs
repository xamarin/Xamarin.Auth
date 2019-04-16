using LeanKit.Core;

namespace LeanKit.iOS.Settings
{
	public class ApplicationSettings : IApplicationSettings
    {
		private string hostName = "login";
		private string urlTemplate = "https://{0}.leankit.com";

		public string HostName { get { return hostName; } set { hostName = value;} }
		public string UrlTemplate { get { return urlTemplate; } set { urlTemplate = value; } }
		public string AppName { get { return "MyLeanKit"; } }
    }
}