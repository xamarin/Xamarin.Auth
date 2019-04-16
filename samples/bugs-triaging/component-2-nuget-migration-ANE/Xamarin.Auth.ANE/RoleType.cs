using System.ComponentModel;

namespace LeanKit.Core.Domain
{
	public enum RoleType
	{
		[Description("No access")]
		None, //0
		[Description("Reader")]
		BoardReader, //1
		[Description("User")]
		BoardUser, //2
		[Description("Manager")]
		BoardManager, //3
		[Description("Administrator")]
		BoardAdministrator, //4
		[Description("Board Creator")]
		BoardCreator, //5
	}
}