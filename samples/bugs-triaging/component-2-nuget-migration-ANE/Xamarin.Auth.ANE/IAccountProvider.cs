using System.Collections.Generic;
using LeanKit.Core.Domain;
using LeanKit.Core.Enums;

namespace LeanKit.Core.ApplicationServices
{
	public interface IAccountProvider
	{
		void Save(long userId, string userName, string secret, AuthorizationType authorizationType, OrganizationLink organization);
		void Delete(long userId);
		IEnumerable<LeanKitAccount> GetAll();
	}
}

