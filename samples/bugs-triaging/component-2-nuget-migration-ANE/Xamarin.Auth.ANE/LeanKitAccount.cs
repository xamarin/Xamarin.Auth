using LeanKit.Core.Enums;

namespace LeanKit.Core.Domain
{
	public interface ILeanKitAccount
	{
		long UserId { get; set; }
	}

	public class LeanKitAccount : ILeanKitAccount
	{
		public OrganizationLink Organization { get; set; }

		public long UserId { get; set; }

		public string UserName { get; set; }

		public string Secret { get; set; }

		public string AvatarUrl { get; set; }

		public AuthorizationType AuthorizationType { get; set; }

		public LeanKitAccount(long userId, string userName, string secret, AuthorizationType authorizationType, OrganizationLink organizationLink, string avatarUrl)
		{
			UserId = userId;
			UserName = userName;
			Secret = secret;
			AuthorizationType = authorizationType;
			Organization = organizationLink;
			AvatarUrl = avatarUrl;
		}
	}
}
