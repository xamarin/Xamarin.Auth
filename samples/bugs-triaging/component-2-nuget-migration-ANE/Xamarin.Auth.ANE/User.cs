using System;
using System.Collections.Generic;
using LeanKit.Core.Utility;

namespace LeanKit.Core.Domain
{
    [Serializable]
	public class User
    {
        public long Id { get; set; }
        public string FullName { get; set; }
		public RoleType Role { get; set; }
        public int WIP { get; set; }
        public bool Enabled { get; set; }
        public bool IsAccountOwner { get; set; }
        public bool IsDeleted { get; set; }
        public string GravatarFeed { get; set; }
        public string GravatarLink { get; set; }
        public string EmailAddress { get; set; }
        public string DateFormat { get; set; }
		public Dictionary<string, string> Settings { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool IsSharedRole { get; set; }
        public bool Administrator { get; set; }
        public bool BoardCreator { get; set; }
        public string TimeZone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CreationDate { get; set; }
        public string CreationDateSortString { get; set; }
        public string LastAccess { get; set; }

        public string RoleName
        {
            get { return Role.GetDescription(); }
        }
    }
}