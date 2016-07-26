using System;
using SQLite;

namespace Diary
{
	[Table("DiaryEntry")]
	public class DiaryEntry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; } = -1;

		public DateTime Timestamp  { get; set; }
		public byte[] InitializationVector { get; set; }
		public byte[] CipherText  { get; set; }
		public string AccountName { get; set; }

		public DiaryEntry()
		{
			this.Timestamp = DateTime.Now;
		}

		public DiaryEntry(byte[] initializationVector, byte[] cipherText, string account) 
			: this ()
		{
			InitializationVector = initializationVector;
			CipherText = cipherText;
			AccountName = account;
		}
	}
}