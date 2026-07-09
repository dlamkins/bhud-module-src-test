using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class ProfileCache
	{
		private sealed class ProfileCacheEntry
		{
			public string Path { get; set; }

			public SavedProfileSummary Summary { get; set; }
		}

		private sealed class SavedProfileIndexFile
		{
			public int SchemaVersion { get; set; } = 1;


			public string CacheKey { get; set; } = string.Empty;


			public ProfileIndexFields Profile { get; set; }

			public PresenceIndexFields Presence { get; set; }

			public DateTime CachedAt { get; set; }

			public bool IsBookmarked { get; set; }

			public DateTime? BookmarkedAt { get; set; }
		}

		private sealed class ProfileIndexFields
		{
			public string ProfileId { get; set; } = string.Empty;


			public string ProfileName { get; set; } = string.Empty;


			public bool IsMature { get; set; }

			public string AccountName { get; set; } = string.Empty;


			public string CharacterName { get; set; } = string.Empty;


			public string DisplayName { get; set; } = string.Empty;


			public string Race { get; set; } = string.Empty;


			public string Profession { get; set; } = string.Empty;


			public string CustomProfession { get; set; } = string.Empty;


			public ProfileRegion Region { get; set; }

			public string Currently { get; set; } = string.Empty;


			public string OutOfCharacterInfo { get; set; } = string.Empty;

		}

		private sealed class PresenceIndexFields
		{
			public string AccountName { get; set; } = string.Empty;


			public string OfficialCharacterName { get; set; } = string.Empty;


			public string DisplayCharacterName { get; set; } = string.Empty;


			public string Race { get; set; } = string.Empty;


			public string Profession { get; set; } = string.Empty;


			public string CustomProfession { get; set; } = string.Empty;


			public string ActiveProfileId { get; set; } = string.Empty;


			public string ActiveProfileName { get; set; } = string.Empty;


			public bool IsMature { get; set; }

			public RPStatus Status { get; set; }

			public string Currently { get; set; } = string.Empty;


			public string OutOfCharacterInfo { get; set; } = string.Empty;


			public string LocationName { get; set; } = string.Empty;


			public ProfileRegion Region { get; set; }

			public DateTime LastSeen { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<ProfileCache>();

		private const int MaxRecentProfiles = 250;

		private static readonly TimeSpan RecentProfileMaxAge = TimeSpan.FromDays(180.0);

		private readonly string _cacheDirectory;

		private readonly ProfileValidator _validator;

		private readonly object _cacheLock = new object();

		private Dictionary<string, ProfileCacheEntry> _indexByKey;

		public ProfileCache(DirectoriesManager directoriesManager, ProfileValidator validator)
		{
			_validator = validator;
			string sparkDirectory = directoriesManager.GetFullDirectoryPath("spark");
			_cacheDirectory = Path.Combine(sparkDirectory, "profile-cache");
			FileStore.EnsureDirectory(_cacheDirectory, Logger, "SPARK profile cache");
		}

		public SavedProfile Save(CharacterProfile profile, PlayerPresence presence, bool bookmark = false)
		{
			if (profile == null)
			{
				throw new InvalidOperationException("Cannot cache a missing profile.");
			}
			PresenceMapper.FillProfileFromPresence(profile, presence);
			ProfileValidationResult validation = _validator.Validate(profile);
			if (!validation.IsValid)
			{
				throw new InvalidOperationException(string.Join(Environment.NewLine, validation.Errors));
			}
			string cacheKey = GetCacheKey(profile, presence);
			SavedProfileSummary existing = FindSummary(cacheKey);
			SavedProfile record = new SavedProfile
			{
				CacheKey = cacheKey,
				Profile = profile,
				Presence = (presence ?? new PlayerPresence()),
				CachedAt = DateTime.UtcNow,
				IsBookmarked = (bookmark || (existing?.IsBookmarked ?? false)),
				BookmarkedAt = (bookmark ? new DateTime?(DateTime.UtcNow) : existing?.BookmarkedAt)
			};
			Write(record);
			return record;
		}

		public SavedProfile Bookmark(CharacterProfile profile, PlayerPresence presence)
		{
			return Save(profile, presence, bookmark: true);
		}

		public SavedProfile Load(string cacheKey)
		{
			cacheKey = NormalizeCacheKey(cacheKey);
			if (string.IsNullOrWhiteSpace(cacheKey))
			{
				return null;
			}
			ProfileCacheEntry entry;
			lock (_cacheLock)
			{
				GetIndexLocked().TryGetValue(cacheKey, out entry);
			}
			if (entry == null)
			{
				return null;
			}
			SavedProfile savedProfile = LoadFromPath(entry.Path);
			if (savedProfile == null)
			{
				Remove(cacheKey);
			}
			return savedProfile;
		}

		private SavedProfile LoadFromPath(string path)
		{
			SavedProfile record = FileStore.ReadFile<SavedProfile>(path, Logger, "SPARK cached profile");
			if (record != null && record.SchemaVersion == 1)
			{
				return NormalizeLoadedRecord(record, path);
			}
			if (record != null)
			{
				Logger.Warn("Skipping unsupported SPARK cached profile.");
			}
			return null;
		}

		public bool IsBookmarked(string cacheKey)
		{
			if (string.IsNullOrWhiteSpace(cacheKey))
			{
				return false;
			}
			lock (_cacheLock)
			{
				ProfileCacheEntry entry;
				return GetIndexLocked().TryGetValue(NormalizeCacheKey(cacheKey), out entry) && entry != null && (entry.Summary?.IsBookmarked).GetValueOrDefault();
			}
		}

		public IReadOnlyList<SavedProfileSummary> ListRecent()
		{
			lock (_cacheLock)
			{
				return (from entry in GetIndexLocked().Values
					select CloneSummary(entry.Summary) into summary
					where summary != null
					orderby summary.CachedAt descending
					select summary).ToList();
			}
		}

		public IReadOnlyList<SavedProfileSummary> ListBookmarked()
		{
			lock (_cacheLock)
			{
				return (from entry in GetIndexLocked().Values
					select CloneSummary(entry.Summary) into summary
					where summary?.IsBookmarked ?? false
					orderby summary.BookmarkedAt ?? summary.CachedAt
					select summary).ToList();
			}
		}

		public void RemoveBookmark(string cacheKey)
		{
			cacheKey = NormalizeCacheKey(cacheKey);
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				SavedProfile record = Load(cacheKey);
				if (record == null)
				{
					Remove(cacheKey);
					return;
				}
				record.IsBookmarked = false;
				record.BookmarkedAt = null;
				Write(record);
			}
		}

		public bool Remove(string cacheKey)
		{
			cacheKey = NormalizeCacheKey(cacheKey);
			if (string.IsNullOrWhiteSpace(cacheKey))
			{
				return false;
			}
			ProfileCacheEntry entry = null;
			bool removedFromIndex = false;
			lock (_cacheLock)
			{
				if (_indexByKey == null)
				{
					_indexByKey = ReadIndex();
				}
				if (_indexByKey.TryGetValue(cacheKey, out entry))
				{
					removedFromIndex = _indexByKey.Remove(cacheKey);
				}
			}
			string path = entry?.Path ?? FileStore.GetSafePath(_cacheDirectory, cacheKey);
			bool removedFile = TryDeleteCacheFile(path);
			return removedFromIndex || removedFile;
		}

		public static string GetCacheKey(CharacterProfile profile, PlayerPresence presence)
		{
			string cacheKey = GetProfileCacheKey(profile, presence);
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				return cacheKey;
			}
			return Guid.NewGuid().ToString();
		}

		public static string GetProfileCacheKey(CharacterProfile profile, PlayerPresence presence)
		{
			if (!string.IsNullOrWhiteSpace(profile?.ProfileId))
			{
				return profile.ProfileId.Trim();
			}
			if (!string.IsNullOrWhiteSpace(presence?.ActiveProfileId))
			{
				return presence.ActiveProfileId.Trim();
			}
			string obj = presence?.AccountName ?? profile?.AccountName ?? string.Empty;
			string fallbackKey = string.Concat(str2: (presence?.OfficialCharacterName ?? profile?.CharacterName ?? string.Empty).Trim(), str0: obj.Trim(), str1: "|");
			if (!string.IsNullOrWhiteSpace(fallbackKey.Trim('|')))
			{
				return fallbackKey;
			}
			return string.Empty;
		}

		private string GetRecordPath(SavedProfile record)
		{
			return FileStore.GetSafePath(_cacheDirectory, NormalizeCacheKey(record?.CacheKey));
		}

		private Dictionary<string, ProfileCacheEntry> GetIndexLocked()
		{
			if (_indexByKey == null)
			{
				_indexByKey = ReadIndex();
			}
			return _indexByKey;
		}

		private SavedProfileSummary FindSummary(string cacheKey)
		{
			if (string.IsNullOrWhiteSpace(cacheKey))
			{
				return null;
			}
			lock (_cacheLock)
			{
				ProfileCacheEntry entry;
				return GetIndexLocked().TryGetValue(NormalizeCacheKey(cacheKey), out entry) ? CloneSummary(entry.Summary) : null;
			}
		}

		private Dictionary<string, ProfileCacheEntry> ReadIndex()
		{
			List<ProfileCacheEntry> indexEntries = (from entry in FileStore.GetFiles(_cacheDirectory, Logger, "SPARK cached profile").Select(ReadIndexEntry)
				where entry?.Summary != null && !string.IsNullOrWhiteSpace(entry.Summary.CacheKey)
				select entry).ToList();
			int removedCount = 0;
			DateTime expirationCutoff = DateTime.UtcNow.Subtract(RecentProfileMaxAge);
			List<ProfileCacheEntry> expiredEntries = indexEntries.Where((ProfileCacheEntry entry) => !entry.Summary.IsBookmarked && entry.Summary.CachedAt < expirationCutoff).ToList();
			removedCount += RemoveCacheEntries(indexEntries, expiredEntries);
			List<ProfileCacheEntry> excessEntries = (from entry in indexEntries
				where !entry.Summary.IsBookmarked
				orderby entry.Summary.CachedAt descending
				select entry).ThenBy((ProfileCacheEntry entry) => entry.Summary.CacheKey, StringComparer.OrdinalIgnoreCase).Skip(250).ToList();
			removedCount += RemoveCacheEntries(indexEntries, excessEntries);
			if (removedCount > 0)
			{
				Logger.Info("Pruned {count} old or excess SPARK cached profile(s).", new object[1] { removedCount });
			}
			Dictionary<string, ProfileCacheEntry> index = new Dictionary<string, ProfileCacheEntry>(StringComparer.OrdinalIgnoreCase);
			foreach (ProfileCacheEntry entry2 in indexEntries.OrderByDescending((ProfileCacheEntry entry) => entry.Summary.CachedAt).ThenBy((ProfileCacheEntry entry) => entry.Summary.CacheKey, StringComparer.OrdinalIgnoreCase))
			{
				string cacheKey = NormalizeCacheKey(entry2.Summary.CacheKey);
				if (!string.IsNullOrWhiteSpace(cacheKey) && !index.ContainsKey(cacheKey))
				{
					index[cacheKey] = entry2;
				}
			}
			return index;
		}

		private int RemoveCacheEntries(List<ProfileCacheEntry> entries, IEnumerable<ProfileCacheEntry> candidates)
		{
			int removedCount = 0;
			foreach (ProfileCacheEntry entry in candidates ?? Enumerable.Empty<ProfileCacheEntry>())
			{
				if (TryDeleteCacheEntry(entry))
				{
					entries.Remove(entry);
					removedCount++;
				}
			}
			return removedCount;
		}

		private int TrimRecent()
		{
			if (_indexByKey == null)
			{
				return 0;
			}
			List<ProfileCacheEntry> list = (from entry in _indexByKey.Values
				where entry?.Summary != null && !entry.Summary.IsBookmarked
				orderby entry.Summary.CachedAt descending
				select entry).Skip(250).ToList();
			int removedCount = 0;
			foreach (ProfileCacheEntry entry2 in list)
			{
				if (TryDeleteCacheEntry(entry2))
				{
					_indexByKey.Remove(NormalizeCacheKey(entry2.Summary.CacheKey));
					removedCount++;
				}
			}
			return removedCount;
		}

		private bool TryDeleteCacheEntry(ProfileCacheEntry entry)
		{
			if (entry?.Summary == null || entry.Summary.IsBookmarked)
			{
				return false;
			}
			return TryDeleteCacheFile(entry.Path);
		}

		private bool TryDeleteCacheFile(string path)
		{
			if (!IsManagedCachePath(path))
			{
				return false;
			}
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				return true;
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to delete a SPARK cached profile ({errorType}).", new object[1] { ex.GetType().Name });
				return false;
			}
		}

		private bool IsManagedCachePath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			try
			{
				string fullCacheDirectory = Path.GetFullPath(_cacheDirectory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
				string fullPath = Path.GetFullPath(path);
				return fullPath.StartsWith(fullCacheDirectory, StringComparison.OrdinalIgnoreCase) && string.Equals(Path.GetExtension(fullPath), ".json", StringComparison.OrdinalIgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		private ProfileCacheEntry ReadIndexEntry(string path)
		{
			SavedProfileIndexFile snapshot = FileStore.ReadFile<SavedProfileIndexFile>(path, Logger, "SPARK cached profile index");
			if (snapshot != null && snapshot.SchemaVersion == 1)
			{
				return new ProfileCacheEntry
				{
					Path = path,
					Summary = NormalizeSummary(ToSummary(snapshot), path)
				};
			}
			if (snapshot != null)
			{
				Logger.Warn("Skipping unsupported SPARK cached profile index.");
			}
			return null;
		}

		private static SavedProfile NormalizeLoadedRecord(SavedProfile record, string path)
		{
			if (record == null)
			{
				return null;
			}
			if (record.CachedAt == default(DateTime))
			{
				record.CachedAt = GetFallbackCachedAt(record.BookmarkedAt, record.Presence?.LastSeen ?? default(DateTime), path);
			}
			return record;
		}

		private static SavedProfileSummary NormalizeSummary(SavedProfileSummary summary, string path)
		{
			if (summary == null)
			{
				return null;
			}
			summary.CacheKey = NormalizeCacheKey(summary.CacheKey);
			if (summary.CachedAt == default(DateTime))
			{
				summary.CachedAt = GetFallbackCachedAt(summary.BookmarkedAt, summary.LastSeen, path);
			}
			return summary;
		}

		private void Write(SavedProfile record)
		{
			record.CacheKey = NormalizeCacheKey(record.CacheKey);
			string path = GetRecordPath(record);
			if (!FileStore.TryWrite(path, record, Logger, "SPARK cached profile"))
			{
				throw new InvalidOperationException("Could not save the cached profile. Check file permissions and try again.");
			}
			lock (_cacheLock)
			{
				if (_indexByKey != null && !string.IsNullOrWhiteSpace(record.CacheKey))
				{
					_indexByKey[NormalizeCacheKey(record.CacheKey)] = new ProfileCacheEntry
					{
						Path = path,
						Summary = ToSummary(record)
					};
					int removedCount = TrimRecent();
					if (removedCount > 0)
					{
						Logger.Info("Pruned {count} excess SPARK cached profile(s).", new object[1] { removedCount });
					}
				}
			}
		}

		private static DateTime GetFallbackCachedAt(DateTime? bookmarkedAt, DateTime lastSeen, string path)
		{
			DateTime fileTime = GetFileTime(path);
			if (fileTime != default(DateTime))
			{
				return fileTime;
			}
			if (bookmarkedAt.HasValue && bookmarkedAt.Value != default(DateTime))
			{
				return bookmarkedAt.Value.ToUniversalTime();
			}
			if (lastSeen != default(DateTime))
			{
				return lastSeen.ToUniversalTime();
			}
			return DateTime.UtcNow;
		}

		private static DateTime GetFileTime(string path)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
				{
					return default(DateTime);
				}
				DateTime lastWrite = File.GetLastWriteTimeUtc(path);
				return (lastWrite.Year <= 1900) ? default(DateTime) : lastWrite;
			}
			catch
			{
				return default(DateTime);
			}
		}

		private static string NormalizeCacheKey(string cacheKey)
		{
			return cacheKey?.Trim() ?? string.Empty;
		}

		private static SavedProfileSummary ToSummary(SavedProfile record)
		{
			if (record == null)
			{
				return null;
			}
			SavedProfileSummary savedProfileSummary = new SavedProfileSummary();
			savedProfileSummary.CacheKey = NormalizeCacheKey(record.CacheKey);
			savedProfileSummary.ProfileId = Clean(TextUtil.FirstNonEmpty(record.Profile?.ProfileId, record.Presence?.ActiveProfileId));
			savedProfileSummary.ProfileName = Clean(TextUtil.FirstNonEmpty(record.Profile?.ProfileName, record.Presence?.ActiveProfileName));
			savedProfileSummary.IsMature = (record.Profile?.IsMature ?? false) || (record.Presence?.IsMature ?? false);
			savedProfileSummary.AccountName = Clean(TextUtil.FirstNonEmpty(record.Presence?.AccountName, record.Profile?.AccountName));
			savedProfileSummary.OfficialCharacterName = Clean(TextUtil.FirstNonEmpty(record.Presence?.OfficialCharacterName, record.Profile?.CharacterName));
			savedProfileSummary.DisplayCharacterName = Clean(TextUtil.FirstNonEmpty(record.Presence?.DisplayCharacterName, record.Profile?.DisplayName));
			savedProfileSummary.Race = Clean(TextUtil.FirstNonEmpty(record.Presence?.Race, record.Profile?.Race));
			savedProfileSummary.Profession = Clean(TextUtil.FirstNonEmpty(record.Presence?.Profession, record.Profile?.Profession));
			savedProfileSummary.CustomProfession = Clean(TextUtil.FirstNonEmpty(record.Presence?.CustomProfession, record.Profile?.CustomProfession));
			savedProfileSummary.ActiveProfileId = Clean(TextUtil.FirstNonEmpty(record.Presence?.ActiveProfileId, record.Profile?.ProfileId));
			savedProfileSummary.ActiveProfileName = Clean(TextUtil.FirstNonEmpty(record.Presence?.ActiveProfileName, record.Profile?.ProfileName));
			savedProfileSummary.Status = record.Presence?.Status ?? RPStatus.Online;
			savedProfileSummary.Currently = Clean(TextUtil.FirstNonEmpty(record.Presence?.Currently, record.Profile?.Currently));
			savedProfileSummary.OutOfCharacterInfo = Clean(TextUtil.FirstNonEmpty(record.Presence?.OutOfCharacterInfo, record.Profile?.OutOfCharacterInfo));
			savedProfileSummary.LocationName = Clean(record.Presence?.LocationName);
			savedProfileSummary.Region = record.Presence?.Region ?? record.Profile?.Region ?? ProfileRegion.NA;
			savedProfileSummary.LastSeen = record.Presence?.LastSeen ?? default(DateTime);
			savedProfileSummary.CachedAt = record.CachedAt;
			savedProfileSummary.IsBookmarked = record.IsBookmarked;
			savedProfileSummary.BookmarkedAt = record.BookmarkedAt;
			return savedProfileSummary;
		}

		private static SavedProfileSummary ToSummary(SavedProfileIndexFile snapshot)
		{
			if (snapshot == null)
			{
				return null;
			}
			SavedProfileSummary savedProfileSummary = new SavedProfileSummary();
			savedProfileSummary.CacheKey = NormalizeCacheKey(snapshot.CacheKey);
			savedProfileSummary.ProfileId = Clean(TextUtil.FirstNonEmpty(snapshot.Profile?.ProfileId, snapshot.Presence?.ActiveProfileId));
			savedProfileSummary.ProfileName = Clean(TextUtil.FirstNonEmpty(snapshot.Profile?.ProfileName, snapshot.Presence?.ActiveProfileName));
			savedProfileSummary.IsMature = (snapshot.Profile?.IsMature ?? false) || (snapshot.Presence?.IsMature ?? false);
			savedProfileSummary.AccountName = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.AccountName, snapshot.Profile?.AccountName));
			savedProfileSummary.OfficialCharacterName = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.OfficialCharacterName, snapshot.Profile?.CharacterName));
			savedProfileSummary.DisplayCharacterName = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.DisplayCharacterName, snapshot.Profile?.DisplayName));
			savedProfileSummary.Race = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.Race, snapshot.Profile?.Race));
			savedProfileSummary.Profession = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.Profession, snapshot.Profile?.Profession));
			savedProfileSummary.CustomProfession = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.CustomProfession, snapshot.Profile?.CustomProfession));
			savedProfileSummary.ActiveProfileId = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.ActiveProfileId, snapshot.Profile?.ProfileId));
			savedProfileSummary.ActiveProfileName = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.ActiveProfileName, snapshot.Profile?.ProfileName));
			savedProfileSummary.Status = snapshot.Presence?.Status ?? RPStatus.Online;
			savedProfileSummary.Currently = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.Currently, snapshot.Profile?.Currently));
			savedProfileSummary.OutOfCharacterInfo = Clean(TextUtil.FirstNonEmpty(snapshot.Presence?.OutOfCharacterInfo, snapshot.Profile?.OutOfCharacterInfo));
			savedProfileSummary.LocationName = Clean(snapshot.Presence?.LocationName);
			savedProfileSummary.Region = snapshot.Presence?.Region ?? snapshot.Profile?.Region ?? ProfileRegion.NA;
			savedProfileSummary.LastSeen = snapshot.Presence?.LastSeen ?? default(DateTime);
			savedProfileSummary.CachedAt = snapshot.CachedAt;
			savedProfileSummary.IsBookmarked = snapshot.IsBookmarked;
			savedProfileSummary.BookmarkedAt = snapshot.BookmarkedAt;
			return savedProfileSummary;
		}

		private static SavedProfileSummary CloneSummary(SavedProfileSummary summary)
		{
			if (summary == null)
			{
				return null;
			}
			return new SavedProfileSummary
			{
				CacheKey = summary.CacheKey,
				ProfileId = summary.ProfileId,
				ProfileName = summary.ProfileName,
				IsMature = summary.IsMature,
				AccountName = summary.AccountName,
				OfficialCharacterName = summary.OfficialCharacterName,
				DisplayCharacterName = summary.DisplayCharacterName,
				Race = summary.Race,
				Profession = summary.Profession,
				CustomProfession = summary.CustomProfession,
				ActiveProfileId = summary.ActiveProfileId,
				ActiveProfileName = summary.ActiveProfileName,
				Status = summary.Status,
				Currently = summary.Currently,
				OutOfCharacterInfo = summary.OutOfCharacterInfo,
				LocationName = summary.LocationName,
				Region = summary.Region,
				LastSeen = summary.LastSeen,
				CachedAt = summary.CachedAt,
				IsBookmarked = summary.IsBookmarked,
				BookmarkedAt = summary.BookmarkedAt
			};
		}

		private static string Clean(string value)
		{
			return value?.Trim() ?? string.Empty;
		}
	}
}
