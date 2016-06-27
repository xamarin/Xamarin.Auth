using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using System.Linq;

namespace Diary
{
	public class DiaryEntryStore
	{
		string path;
		SQLiteAsyncConnection connection;

		public DiaryEntryStore(string folder, string filename)
		{
			path = System.IO.Path.Combine(folder, filename);

			connection = new SQLiteAsyncConnection(path);

			connection.CreateTableAsync<DiaryEntry>().Wait();
		}

		public Task SaveEntryAsync(DiaryEntry entry)
		{
			if (entry.Id == -1)
				return connection.InsertAsync(entry);
			else 
				return connection.InsertOrReplaceAsync(entry);
		}

		public Task DeleteEntryAsync (DiaryEntry entry)
		{
			return connection.DeleteAsync (entry);
		}

		public Task<List<DiaryEntry>> GetEntriesAsync(string accountName)
		{
			return connection.Table < DiaryEntry> ().Where (d => d.AccountName == accountName).ToListAsync ();
		}

		public Task<DiaryEntry> GetEntry(int id)
		{
			return connection.Table<DiaryEntry> ().Where (d => d.Id == id).FirstOrDefaultAsync ();
		}
	}
}