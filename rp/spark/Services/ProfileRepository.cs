using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class ProfileRepository
	{
		private static readonly Logger Logger = Logger.GetLogger<ProfileRepository>();

		private readonly string _sparkDirectory;

		private readonly string _profilesDirectory;

		private readonly string _activeProfilesPath;

		private readonly ProfileValidator _validator;

		private readonly object _cacheLock = new object();

		private Dictionary<string, CharacterProfile> _profilesByIdCache;

		private Dictionary<string, string> _activeProfilesCache;

		public event Action<CharacterProfile> ProfileSaved;

		public event Action<string, string, string> ActiveProfileChanged;

		public ProfileRepository(DirectoriesManager directoriesManager, ProfileValidator validator)
		{
			_validator = validator;
			_sparkDirectory = directoriesManager.GetFullDirectoryPath("spark");
			_profilesDirectory = Path.Combine(_sparkDirectory, "profiles");
			_activeProfilesPath = Path.Combine(_sparkDirectory, "active-profiles.json");
			FileStore.EnsureDirectory(_sparkDirectory, Logger, "SPARK");
			FileStore.EnsureDirectory(_profilesDirectory, Logger, "SPARK profile");
		}

		public IReadOnlyList<CharacterProfile> LoadAll()
		{
			lock (_cacheLock)
			{
				return (from profile in GetProfileCacheLocked().Values.Select(CloneProfile)
					where profile != null
					select profile).ToList();
			}
		}

		public IReadOnlyList<CharacterProfile> ListForCharacter(string accountName, string officialCharacterName)
		{
			if (string.IsNullOrWhiteSpace(officialCharacterName))
			{
				return new List<CharacterProfile>();
			}
			string normalizedCharacterName = officialCharacterName.Trim();
			string normalizedAccountName = accountName?.Trim() ?? string.Empty;
			return (from profile in LoadAll()
				where IsSameCharacter(profile, normalizedAccountName, normalizedCharacterName)
				orderby profile.ProfileName, profile.UpdatedAt descending
				select profile).ToList();
		}

		public IReadOnlyList<ProfileImportGroup> ListImports(string accountName, string currentCharacterName)
		{
			if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(currentCharacterName))
			{
				return new List<ProfileImportGroup>();
			}
			string cleanAccount = accountName.Trim();
			string cleanCharacter = currentCharacterName.Trim();
			return (from @group in (from profile in LoadAll()
					where CanImport(profile, cleanAccount, cleanCharacter)
					select profile).GroupBy((CharacterProfile profile) => profile.CharacterName.Trim(), StringComparer.OrdinalIgnoreCase)
				select new ProfileImportGroup
				{
					CharacterName = @group.Key,
					Profiles = @group.OrderBy((CharacterProfile profile) => ProfileName(profile), StringComparer.OrdinalIgnoreCase).ThenByDescending((CharacterProfile profile) => profile.UpdatedAt).Select(CloneProfile)
						.ToList()
				}).OrderBy((ProfileImportGroup group) => group.CharacterName, StringComparer.OrdinalIgnoreCase).ToList();
		}

		public CharacterProfile Load(string profileId)
		{
			if (string.IsNullOrWhiteSpace(profileId))
			{
				return null;
			}
			lock (_cacheLock)
			{
				CharacterProfile profile;
				return GetProfileCacheLocked().TryGetValue(profileId.Trim(), out profile) ? CloneProfile(profile) : null;
			}
		}

		public CharacterProfile LoadForCharacter(string officialCharacterName)
		{
			if (string.IsNullOrWhiteSpace(officialCharacterName))
			{
				return null;
			}
			return (from profile in LoadAll()
				where string.Equals(profile.CharacterName?.Trim(), officialCharacterName.Trim(), StringComparison.OrdinalIgnoreCase)
				orderby profile.UpdatedAt descending
				select profile).FirstOrDefault();
		}

		public CharacterProfile LoadActiveForCharacter(string accountName, string officialCharacterName)
		{
			string activeProfileId = GetActiveProfileId(accountName, officialCharacterName);
			if (string.IsNullOrWhiteSpace(activeProfileId))
			{
				return null;
			}
			CharacterProfile profile = Load(activeProfileId);
			if (!IsSameCharacter(profile, accountName, officialCharacterName))
			{
				return null;
			}
			return profile;
		}

		public string GetActiveProfileId(string accountName, string officialCharacterName)
		{
			if (string.IsNullOrWhiteSpace(officialCharacterName))
			{
				return string.Empty;
			}
			Dictionary<string, string> activeProfiles = LoadActiveProfiles();
			string exactKey = GetActiveProfileKey(accountName, officialCharacterName);
			string characterOnlyKey = CharacterKey(officialCharacterName);
			if (!string.IsNullOrWhiteSpace(accountName) && activeProfiles.TryGetValue(exactKey, out var exactProfileId))
			{
				return exactProfileId;
			}
			if (!activeProfiles.TryGetValue(characterOnlyKey, out var fallbackProfileId))
			{
				return string.Empty;
			}
			return fallbackProfileId;
		}

		public void SetActiveProfile(string accountName, string officialCharacterName, string profileId)
		{
			if (string.IsNullOrWhiteSpace(officialCharacterName))
			{
				throw new InvalidOperationException("Cannot set an active profile without a character name.");
			}
			if (!IsSameCharacter(Load(profileId), accountName, officialCharacterName))
			{
				throw new InvalidOperationException("Cannot set an active profile for a different character.");
			}
			UpdateActive(delegate(Dictionary<string, string> activeProfiles)
			{
				activeProfiles[CharacterKey(officialCharacterName)] = profileId;
				if (!string.IsNullOrWhiteSpace(accountName))
				{
					activeProfiles[GetActiveProfileKey(accountName, officialCharacterName)] = profileId;
				}
				return true;
			});
			this.ActiveProfileChanged?.Invoke(accountName, officialCharacterName, profileId);
		}

		public void ClearActiveProfile(string accountName, string officialCharacterName)
		{
			if (!string.IsNullOrWhiteSpace(officialCharacterName) && UpdateActive(delegate(Dictionary<string, string> activeProfiles)
			{
				bool flag = activeProfiles.Remove(GetActiveProfileKey(accountName, officialCharacterName));
				return activeProfiles.Remove(CharacterKey(officialCharacterName)) || flag;
			}))
			{
				this.ActiveProfileChanged?.Invoke(accountName, officialCharacterName, string.Empty);
			}
		}

		public CharacterProfile Duplicate(CharacterProfile sourceProfile, string profileName)
		{
			if (sourceProfile == null)
			{
				throw new InvalidOperationException("No profile selected to duplicate.");
			}
			CharacterProfile duplicate = CloneProfile(sourceProfile);
			duplicate.ProfileId = Guid.NewGuid().ToString();
			duplicate.ProfileName = (string.IsNullOrWhiteSpace(profileName) ? (ProfileName(sourceProfile) + " Copy") : profileName.Trim());
			duplicate.CreatedAt = DateTime.UtcNow;
			duplicate.UpdatedAt = DateTime.UtcNow;
			Save(duplicate);
			return duplicate;
		}

		public CharacterProfile Import(CharacterProfile sourceProfile, PlayerState targetState)
		{
			if (sourceProfile == null)
			{
				throw new InvalidOperationException("No profile selected to import.");
			}
			if (targetState == null || !targetState.CanEditProfile || string.IsNullOrWhiteSpace(targetState.AccountName) || !targetState.IsCharacterApiVerified)
			{
				throw new InvalidOperationException("Cannot import profiles until SPARK syncs your current character.");
			}
			if (!CanImport(sourceProfile, targetState.AccountName, targetState.OfficialCharacterName))
			{
				throw new InvalidOperationException("This profile cannot be imported for the current account.");
			}
			CharacterProfile imported = CloneProfile(sourceProfile);
			imported.ProfileId = Guid.NewGuid().ToString();
			imported.ProfileName = GetImportName(ProfileName(sourceProfile), targetState);
			ApplyImportTarget(imported, targetState);
			imported.CreatedAt = DateTime.UtcNow;
			imported.UpdatedAt = DateTime.UtcNow;
			Save(imported);
			return imported;
		}

		public void Delete(string profileId)
		{
			if (string.IsNullOrWhiteSpace(profileId))
			{
				return;
			}
			string normalizedProfileId = profileId.Trim();
			CharacterProfile profile = Load(normalizedProfileId);
			try
			{
				string path = GetProfilePath(normalizedProfileId);
				if (!string.IsNullOrWhiteSpace(path))
				{
					File.Delete(path);
				}
			}
			catch (UnauthorizedAccessException ex2)
			{
				BlishWarnings.FileSaveBlocked(ex2, profileId, "delete a SPARK profile");
				Logger.Warn("Failed to delete a SPARK profile ({errorType}).", new object[1] { ex2.GetType().Name });
				throw new InvalidOperationException("Could not delete the profile file. Check file permissions and try again.");
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to delete a SPARK profile ({errorType}).", new object[1] { ex.GetType().Name });
				throw new InvalidOperationException("Could not delete the profile file. Check file permissions and try again.");
			}
			lock (_cacheLock)
			{
				_profilesByIdCache?.Remove(normalizedProfileId);
			}
			if (profile != null && string.Equals(GetActiveProfileId(profile.AccountName, profile.CharacterName), profile.ProfileId, StringComparison.OrdinalIgnoreCase))
			{
				ClearActiveProfile(profile.AccountName, profile.CharacterName);
			}
		}

		public void Save(CharacterProfile profile)
		{
			if (profile == null)
			{
				throw new InvalidOperationException("Cannot save a missing profile.");
			}
			profile.ProfileId = (string.IsNullOrWhiteSpace(profile.ProfileId) ? Guid.NewGuid().ToString() : profile.ProfileId.Trim());
			profile.ProfileName = ProfileName(profile);
			if (profile.CreatedAt == default(DateTime))
			{
				profile.CreatedAt = DateTime.UtcNow;
			}
			profile.UpdatedAt = DateTime.UtcNow;
			ProfileValidationResult validation = _validator.Validate(profile);
			if (!validation.IsValid)
			{
				throw new InvalidOperationException(string.Join(Environment.NewLine, validation.Errors));
			}
			if (!FileStore.TryWrite(GetProfilePath(profile.ProfileId), profile, Logger, "SPARK profile"))
			{
				throw new InvalidOperationException("Could not save the profile file. Check file permissions and try again.");
			}
			lock (_cacheLock)
			{
				if (_profilesByIdCache != null)
				{
					_profilesByIdCache[profile.ProfileId.Trim()] = CloneProfile(profile);
				}
			}
			this.ProfileSaved?.Invoke(profile);
		}

		private CharacterProfile LoadFromFile(string path)
		{
			CharacterProfile profile = FileStore.ReadFile<CharacterProfile>(path, Logger, "SPARK profile");
			if (profile == null)
			{
				return null;
			}
			NormalizeProfile(profile);
			ProfileValidationResult validation = _validator.Validate(profile);
			if (validation.IsValid)
			{
				return profile;
			}
			Logger.Warn("Skipping invalid SPARK profile file: {errors}", new object[1] { string.Join("; ", validation.Errors) });
			return null;
		}

		private string GetProfilePath(string profileId)
		{
			return FileStore.GetSafePath(_profilesDirectory, profileId?.Trim());
		}

		private Dictionary<string, CharacterProfile> GetProfileCacheLocked()
		{
			if (_profilesByIdCache == null)
			{
				_profilesByIdCache = ReadProfiles();
			}
			return _profilesByIdCache;
		}

		private Dictionary<string, CharacterProfile> ReadProfiles()
		{
			Dictionary<string, CharacterProfile> cache = new Dictionary<string, CharacterProfile>(StringComparer.OrdinalIgnoreCase);
			foreach (CharacterProfile profile2 in from profile in FileStore.GetFiles(_profilesDirectory, Logger, "SPARK profile").Select(LoadFromFile)
				where profile != null
				select profile)
			{
				if (!string.IsNullOrWhiteSpace(profile2.ProfileId))
				{
					cache[profile2.ProfileId.Trim()] = profile2;
				}
			}
			return cache;
		}

		private Dictionary<string, string> LoadActiveProfiles()
		{
			lock (_cacheLock)
			{
				if (_activeProfilesCache == null)
				{
					_activeProfilesCache = ReadActiveProfiles();
				}
				return new Dictionary<string, string>(_activeProfilesCache, StringComparer.OrdinalIgnoreCase);
			}
		}

		private Dictionary<string, string> ReadActiveProfiles()
		{
			Dictionary<string, string> activeProfiles = FileStore.ReadFile<Dictionary<string, string>>(_activeProfilesPath, Logger, "SPARK active profile");
			if (activeProfiles == null)
			{
				return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			}
			return new Dictionary<string, string>(activeProfiles, StringComparer.OrdinalIgnoreCase);
		}

		private bool UpdateActive(Func<Dictionary<string, string>, bool> update)
		{
			lock (_cacheLock)
			{
				if (_activeProfilesCache == null)
				{
					_activeProfilesCache = ReadActiveProfiles();
				}
				Dictionary<string, string> activeProfiles = new Dictionary<string, string>(_activeProfilesCache, StringComparer.OrdinalIgnoreCase);
				if (update == null || !update(activeProfiles))
				{
					return false;
				}
				if (!FileStore.TryWrite(_activeProfilesPath, activeProfiles, Logger, "SPARK active profile"))
				{
					throw new InvalidOperationException("Could not save the active profile selection. Check file permissions and try again.");
				}
				_activeProfilesCache = new Dictionary<string, string>(activeProfiles, StringComparer.OrdinalIgnoreCase);
				return true;
			}
		}

		private static void NormalizeProfile(CharacterProfile profile)
		{
			if (profile != null)
			{
				profile.ProfileId = (string.IsNullOrWhiteSpace(profile.ProfileId) ? Guid.NewGuid().ToString() : profile.ProfileId.Trim());
				profile.ProfileName = ProfileName(profile);
				if (profile.CreatedAt == default(DateTime))
				{
					profile.CreatedAt = ((profile.UpdatedAt == default(DateTime)) ? DateTime.UtcNow : profile.UpdatedAt);
				}
				if (profile.UpdatedAt == default(DateTime))
				{
					profile.UpdatedAt = profile.CreatedAt;
				}
			}
		}

		private static bool IsSameCharacter(CharacterProfile profile, string accountName, string officialCharacterName)
		{
			if (profile == null || string.IsNullOrWhiteSpace(officialCharacterName))
			{
				return false;
			}
			if (!string.Equals(profile.CharacterName?.Trim(), officialCharacterName.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(profile.AccountName))
			{
				return true;
			}
			return string.Equals(profile.AccountName.Trim(), accountName.Trim(), StringComparison.OrdinalIgnoreCase);
		}

		private static bool CanImport(CharacterProfile profile, string accountName, string currentCharacterName)
		{
			if (profile == null || string.IsNullOrWhiteSpace(profile.ProfileId) || string.IsNullOrWhiteSpace(profile.CharacterName) || string.IsNullOrWhiteSpace(profile.AccountName) || string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(currentCharacterName))
			{
				return false;
			}
			if (string.Equals(profile.CharacterName.Trim(), currentCharacterName.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (!string.Equals(profile.AccountName.Trim(), accountName.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			return profile.IsCharacterVerified;
		}

		private static void ApplyImportTarget(CharacterProfile profile, PlayerState targetState)
		{
			profile.CharacterName = targetState.OfficialCharacterName.Trim();
			profile.AccountName = targetState.AccountName.Trim();
			profile.Race = targetState.Race?.Trim() ?? string.Empty;
			profile.Profession = targetState.Profession?.Trim() ?? string.Empty;
			profile.Specialization = targetState.Specialization?.Trim() ?? string.Empty;
			profile.IsCharacterVerified = targetState.IsCharacterApiVerified;
		}

		private static string ProfileName(CharacterProfile profile)
		{
			if (!string.IsNullOrWhiteSpace(profile?.ProfileName))
			{
				return profile.ProfileName.Trim();
			}
			return "Default";
		}

		private static CharacterProfile CloneProfile(CharacterProfile profile)
		{
			if (profile != null)
			{
				return JsonConvert.DeserializeObject<CharacterProfile>(JsonConvert.SerializeObject((object)profile));
			}
			return null;
		}

		private string GetImportName(string sourceName, PlayerState targetState)
		{
			string candidate;
			string baseName = (candidate = LimitProfileName(sourceName));
			int suffix = 2;
			List<string> existingNames = ListForCharacter(targetState.AccountName, targetState.OfficialCharacterName).Select(ProfileName).ToList();
			while (existingNames.Any((string name) => string.Equals(name, candidate, StringComparison.OrdinalIgnoreCase)))
			{
				string suffixText = $" ({suffix})";
				int maxBaseLength = Math.Max(1, 30 - suffixText.Length);
				candidate = LimitProfileName(baseName, maxBaseLength) + suffixText;
				suffix++;
			}
			return candidate;
		}

		private static string LimitProfileName(string value, int maxLength = 30)
		{
			string name = (string.IsNullOrWhiteSpace(value) ? "Profile" : value.Trim());
			if (name.Length <= maxLength)
			{
				return name;
			}
			return name.Substring(0, maxLength).TrimEnd();
		}

		private static string GetActiveProfileKey(string accountName, string officialCharacterName)
		{
			if (string.IsNullOrWhiteSpace(accountName))
			{
				return CharacterKey(officialCharacterName);
			}
			return NormalizeKey(accountName) + "|" + NormalizeKey(officialCharacterName);
		}

		private static string CharacterKey(string officialCharacterName)
		{
			return "*|" + NormalizeKey(officialCharacterName);
		}

		private static string NormalizeKey(string value)
		{
			return (value ?? string.Empty).Trim().ToLowerInvariant();
		}
	}
}
