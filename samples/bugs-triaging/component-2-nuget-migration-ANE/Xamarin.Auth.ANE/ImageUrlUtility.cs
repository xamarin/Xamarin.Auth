using System;
using System.Text.RegularExpressions;
using LeanKit.Core.Domain;

namespace LeanKit.Core.Utility
{
	public static class ImageUrlUtility
	{
		public static string AsAvatarUrl(this string url, ActionContext actionContext)
		{
			return actionContext == null || actionContext.UserContext == null ? 
				string.Empty : 
				AsAvatarUrl(url, actionContext.UserContext.LeanKitAccount.Organization.HostName);
		}

		public static string AsAvatarUrl(this string url, string hostName)
		{
			var appSettings = TinyIoC.TinyIoCContainer.Current.Resolve<IApplicationSettings>();

			var avatarSize = string.Format("?s={0}", 100);

			if(Regex.IsMatch(url, @"(\?s=\d+)$"))
			{
				url = Regex.Replace(url, @"(\?s=\d+)$", avatarSize);
			}
			else
			{
				url = url + avatarSize;
			}

			var baseUrl = string.Format(appSettings.UrlTemplate, hostName);

			return String.Format("{0}/avatar/Show/{1}", baseUrl, url);
		}
	}
}