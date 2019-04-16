using ServiceStack.Text;

namespace LeanKit.Core.Utility
{
	public static class ObjectSerializer
	{
		public static string ToJsonOrEmptyString(this object obj)
		{
			return null == obj ? string.Empty : obj.ToJson();
		}
	}
}