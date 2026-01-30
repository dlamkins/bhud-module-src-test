using System;
using Blish_HUD;
using LiteDB;

namespace Maestro.Services.Community
{
	public class UploadRateLimiter
	{
		private class UploadRecord
		{
			public string Id { get; set; }

			public DateTime Date { get; set; }

			public int Count { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<UploadRateLimiter>();

		private const int MAX_UPLOADS_PER_DAY = 3;

		private const string RATE_LIMIT_COLLECTION = "upload_rate_limit";

		private readonly ILiteCollection<UploadRecord> _collection;

		private int _todayCount;

		private string _todayKey;

		public UploadRateLimiter(LiteDatabase database)
		{
			_collection = database.GetCollection<UploadRecord>("upload_rate_limit");
			LoadTodayCount();
		}

		public int GetRemainingUploads()
		{
			EnsureTodayKey();
			return Math.Max(0, 3 - _todayCount);
		}

		public bool CanUpload()
		{
			return GetRemainingUploads() > 0;
		}

		public void RecordUpload()
		{
			EnsureTodayKey();
			_todayCount++;
			UploadRecord record = _collection.FindById(_todayKey);
			if (record == null)
			{
				record = new UploadRecord
				{
					Id = _todayKey,
					Date = DateTime.UtcNow.Date,
					Count = _todayCount
				};
				_collection.Insert(record);
			}
			else
			{
				record.Count = _todayCount;
				_collection.Update(record);
			}
			Logger.Info($"Recorded upload. Uploads today: {_todayCount}/{3}");
		}

		public void CleanupOldRecords()
		{
			DateTime cutoff = DateTime.UtcNow.Date.AddDays(-7.0);
			int deleted = _collection.DeleteMany((UploadRecord r) => r.Date < cutoff);
			if (deleted > 0)
			{
				Logger.Debug($"Cleaned up {deleted} old upload rate limit records");
			}
		}

		private void LoadTodayCount()
		{
			_todayKey = GetTodayKey();
			_todayCount = _collection.FindById(_todayKey)?.Count ?? 0;
		}

		private void EnsureTodayKey()
		{
			string currentKey = GetTodayKey();
			if (_todayKey != currentKey)
			{
				_todayKey = currentKey;
				_todayCount = 0;
			}
		}

		private string GetTodayKey()
		{
			return DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
		}
	}
}
