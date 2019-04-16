
namespace LeanKit.Core.Domain
{
	public class OrganizationLink
	{
		public string Link { get; set; }
		public string Text { get; set; }
		public string HostName { get; set; }
		public string Title { get; set; }
		public bool IsPasswordVerified { get; set; }
		public long? UserId { get; set;}

		public OrganizationLink() {}
		
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof(OrganizationLink))
				return false;
			var other = (OrganizationLink)obj;
			return Link == other.Link;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return Link.GetHashCode();
			}
		}
	}
}