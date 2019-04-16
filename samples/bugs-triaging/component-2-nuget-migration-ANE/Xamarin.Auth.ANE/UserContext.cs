
namespace LeanKit.Core.Domain
{
	public class UserContext
	{
		public UserContext(LeanKitAccount leanKitAccount, User user)
		{
			LeanKitAccount = leanKitAccount;
			User = user;
		}

		public LeanKitAccount LeanKitAccount { get; private set; }

		public User User { get; private set; }
	}

    public class ActionContext
    {
        public ActionContext(UserContext userContext)
        {
			UserContext = userContext;
        }

		public UserContext UserContext { get; private set; }
    }

	public static class UserContextHelper
	{
		public static ActionContext ToActionContext(this UserContext userContext)
		{
			return new ActionContext(userContext);
		}
	}
}