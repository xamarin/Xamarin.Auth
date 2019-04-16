using LeanKit.Core.ApplicationServices;
using System.Collections.Generic;
using LeanKit.Core.Domain;
using Xamarin.Auth;
using LeanKit.Core;
using LeanKit.Core.Enums;
using System;
using LeanKit.Core.Utility;
using ServiceStack.Text;

namespace LeanKit.UI.Application
{
	public class AccountProvider : IAccountProvider
	{
		private readonly AccountStore _accountStore;
		private readonly IApplicationSettings _applicationSettings;

		public AccountProvider(AccountStore accountStore, IApplicationSettings applicationSettings)
		{
			_accountStore = accountStore;
			_applicationSettings = applicationSettings;
		}

		public void Save(long userId, string userName, string secret, AuthorizationType authorizationType, OrganizationLink organization)
		{
			var props = new Dictionary<string, string>();
			props.Add("UserName", userName);
			props.Add("Secret", secret);
			props.Add("AuthorizationType", authorizationType.ToString());
			props.Add("Organization", organization.ToJsonOrEmptyString());
			props.Add("AvatarUrl", userId.ToString().AsAvatarUrl(organization.HostName));

			_accountStore.Save(new Account(userId.ToString(), props), _applicationSettings.AppName);
		}

		public void Delete(long userId)
		{
			_accountStore.Delete(new Account(userId.ToString()), _applicationSettings.AppName);
		}

		public IEnumerable<LeanKitAccount> GetAll()
		{
			var accounts = _accountStore.FindAccountsForService(_applicationSettings.AppName);

			var lkAccounts = new List<LeanKitAccount>();

			foreach (var account in accounts)
			{
				long userId;
				if (long.TryParse(account.Username, out userId))
				{
					var organization = account.Properties["Organization"].FromJson<OrganizationLink>();
					var authorizationType = (AuthorizationType)Enum.Parse(typeof(AuthorizationType), account.Properties["AuthorizationType"]);
					lkAccounts.Add(new LeanKitAccount(userId, 
						account.Properties["UserName"], 
						account.Properties["Secret"], 
						authorizationType,
						organization, 
						account.Properties["AvatarUrl"]));
				}
				else
				{
					//delete the legacy accounts when username was the account "key"
					_accountStore.Delete(account, _applicationSettings.AppName);
				}
			}

			return lkAccounts;
		}
	}
}

