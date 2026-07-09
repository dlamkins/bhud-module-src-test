using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class ProfileEditorSession
	{
		private readonly ProfileRepository _profiles;

		private readonly PlayerStateService _playerState;

		private List<CharacterProfile> _availableProfiles = new List<CharacterProfile>();

		private List<ProfileImportGroup> _importGroups = new List<ProfileImportGroup>();

		public CharacterProfile Profile { get; private set; }

		public PlayerState State { get; private set; }

		public bool HasImportState
		{
			get
			{
				if (State.CanEditProfile && !string.IsNullOrWhiteSpace(State.AccountName))
				{
					return State.IsCharacterApiVerified;
				}
				return false;
			}
		}

		public AtAGlanceEntry[] Glance { get; }

		public IReadOnlyList<CharacterProfile> Profiles => _availableProfiles;

		public IReadOnlyList<ProfileImportGroup> ImportGroups => _importGroups;

		public string ActiveProfileId => _profiles.GetActiveProfileId(State.AccountName, State.OfficialCharacterName);

		public bool IsSelectedProfileActive
		{
			get
			{
				if (Profile != null && !string.IsNullOrWhiteSpace(ActiveProfileId))
				{
					return string.Equals(Profile.ProfileId, ActiveProfileId, StringComparison.OrdinalIgnoreCase);
				}
				return false;
			}
		}

		public string StatusText { get; private set; } = string.Empty;


		public event Action<string> StatusChanged;

		public event Action ProfileChanged;

		public event Action ImportsChanged;

		public ProfileEditorSession(ProfileRepository profiles, PlayerStateService playerState, PlayerState initialState)
		{
			_profiles = profiles;
			_playerState = playerState;
			State = initialState ?? new PlayerState();
			Glance = new AtAGlanceEntry[5];
			RefreshProfiles(null, preferActive: true);
		}

		public void SelectProfile(string profileId)
		{
			if (!string.IsNullOrWhiteSpace(profileId))
			{
				CharacterProfile selectedProfile = _availableProfiles.FirstOrDefault((CharacterProfile profile) => string.Equals(profile.ProfileId, profileId, StringComparison.OrdinalIgnoreCase));
				if (selectedProfile != null)
				{
					Profile = selectedProfile;
					LoadGlanceDraft();
					SetStatus("Editing " + GetProfileName(Profile) + ".");
					this.ProfileChanged?.Invoke();
				}
			}
		}

		public void CreateProfile()
		{
			EnsureEditable();
			CharacterProfile profile = CreateBlankProfile(GetUniqueProfileName("New Profile"));
			_profiles.Save(profile);
			RefreshProfiles(profile.ProfileId);
			SetStatus("New profile created. Rename it and save when ready.");
		}

		public void DuplicateProfile()
		{
			EnsureEditable();
			ApplyGlance();
			string duplicateName = GetUniqueProfileName(GetProfileName(Profile) + " Copy");
			CharacterProfile duplicate = _profiles.Duplicate(Profile, duplicateName);
			RefreshProfiles(duplicate.ProfileId);
			SetStatus("Profile duplicated.");
		}

		public async Task<bool> ImportAsync(string profileId)
		{
			if (string.IsNullOrWhiteSpace(profileId))
			{
				SetStatus("Choose a profile to import.");
				return false;
			}
			SetStatus("Importing profile...");
			PlayerState state = await _playerState.GetCurrentAsync();
			if (!state.CanEditProfile)
			{
				SetStatus("Character info unavailable.");
				return false;
			}
			CharacterProfile source = _profiles.Load(profileId);
			if (source == null)
			{
				RefreshProfiles(Profile?.ProfileId);
				SetStatus("Profile not found.");
				return false;
			}
			try
			{
				CharacterProfile imported = _profiles.Import(source, state);
				RefreshProfiles(imported.ProfileId);
				SetStatus("Imported " + GetProfileName(imported) + ".");
				return true;
			}
			catch
			{
				RefreshImportGroups();
				SetStatus("Import failed.");
				return false;
			}
		}

		public void DeleteProfile()
		{
			EnsureEditable();
			if (Profile != null)
			{
				string deletedProfileName = GetProfileName(Profile);
				_profiles.Delete(Profile.ProfileId);
				RefreshProfiles(null, preferActive: true);
				SetStatus("Deleted " + deletedProfileName + ".");
			}
		}

		public async Task SetActiveAsync()
		{
			if (await SaveAsync(clearStatusAfterSave: false))
			{
				_profiles.SetActiveProfile(Profile.AccountName, Profile.CharacterName, Profile.ProfileId);
				RefreshProfiles(Profile.ProfileId);
				SetStatus(GetProfileName(Profile) + " is now active.");
			}
		}

		public async Task<bool> SaveAsync()
		{
			return await SaveAsync(clearStatusAfterSave: true);
		}

		public async Task<bool> SaveAsync(bool clearStatusAfterSave)
		{
			SetStatus("Checking current character...");
			PlayerState state = await _playerState.GetCurrentAsync();
			if (!state.CanEditProfile)
			{
				SetStatus("Cannot save until Mumble Link detects your current character.");
				return false;
			}
			if (!string.IsNullOrWhiteSpace(Profile.CharacterName) && !string.Equals(Profile.CharacterName.Trim(), state.OfficialCharacterName.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				SetStatus("Character changed. Reopen the editor before saving.");
				return false;
			}
			ApplyStateToProfile(state);
			ApplyGlance();
			try
			{
				_profiles.Save(Profile);
				RefreshProfiles(Profile.ProfileId);
				SetStatus(string.IsNullOrWhiteSpace(Profile.AccountName) ? "Profile saved. API account unavailable." : "Profile saved!");
				if (clearStatusAfterSave)
				{
					await Task.Delay(1000);
					SetStatus(string.Empty);
				}
				return true;
			}
			catch (Exception ex)
			{
				SetStatus(ex.Message);
				return false;
			}
		}

		public string GetHeaderText()
		{
			string characterName = (string.IsNullOrWhiteSpace(State.OfficialCharacterName) ? "Unknown character" : State.OfficialCharacterName.Trim());
			string accountName = (string.IsNullOrWhiteSpace(State.AccountName) ? "API account unavailable" : State.AccountName.Trim());
			string locationName = (string.IsNullOrWhiteSpace(State.LocationName) ? "Unknown location" : State.LocationName.Trim());
			string characterDetails = GetCharacterDetailsText(State);
			string verification = (State.IsCharacterApiVerified ? "API character verified" : (State.HasCharactersPermission ? "API character not verified yet" : "Characters API unavailable"));
			return "Editing profile for " + characterName + " | " + characterDetails + " | " + accountName + " | " + verification + " | " + locationName;
		}

		public void SetStatus(string statusText)
		{
			StatusText = statusText ?? string.Empty;
			this.StatusChanged?.Invoke(StatusText);
		}

		private void RefreshProfiles(string selectedProfileId = null, bool preferActive = false)
		{
			_availableProfiles = (State.CanEditProfile ? _profiles.ListForCharacter(State.AccountName, State.OfficialCharacterName).ToList() : new List<CharacterProfile>());
			RefreshImportGroups();
			CharacterProfile selectedProfile = null;
			if (!string.IsNullOrWhiteSpace(selectedProfileId))
			{
				selectedProfile = _availableProfiles.FirstOrDefault((CharacterProfile profile) => string.Equals(profile.ProfileId, selectedProfileId, StringComparison.OrdinalIgnoreCase));
			}
			if (selectedProfile == null && preferActive)
			{
				string activeProfileId = ActiveProfileId;
				if (!string.IsNullOrWhiteSpace(activeProfileId))
				{
					selectedProfile = _availableProfiles.FirstOrDefault((CharacterProfile profile) => string.Equals(profile.ProfileId, activeProfileId, StringComparison.OrdinalIgnoreCase));
				}
			}
			if (selectedProfile == null)
			{
				selectedProfile = _availableProfiles.OrderByDescending((CharacterProfile profile) => profile.UpdatedAt).FirstOrDefault();
			}
			Profile = selectedProfile ?? CreateBlankProfile("Default");
			LoadGlanceDraft();
			this.ProfileChanged?.Invoke();
		}

		private void RefreshImportGroups()
		{
			_importGroups = (State.CanEditProfile ? _profiles.ListImports(State.AccountName, State.OfficialCharacterName).ToList() : new List<ProfileImportGroup>());
		}

		public async Task<bool> RefreshImportsAsync()
		{
			PlayerState state = await _playerState.GetCurrentAsync();
			if (!state.CanEditProfile)
			{
				return false;
			}
			State = state;
			RefreshImportGroups();
			this.ImportsChanged?.Invoke();
			return HasImportState;
		}

		private CharacterProfile CreateBlankProfile(string profileName)
		{
			CharacterProfile profile = new CharacterProfile
			{
				ProfileName = LimitProfileName(string.IsNullOrWhiteSpace(profileName) ? "Default" : profileName.Trim())
			};
			if (State.CanEditProfile)
			{
				ApplyStateToProfile(State, profile);
			}
			return profile;
		}

		private void ApplyStateToProfile(PlayerState state)
		{
			ApplyStateToProfile(state, Profile);
		}

		private static void ApplyStateToProfile(PlayerState state, CharacterProfile profile)
		{
			if (state != null && profile != null && state.CanEditProfile)
			{
				profile.CharacterName = state.OfficialCharacterName.Trim();
				if (!string.IsNullOrWhiteSpace(state.AccountName))
				{
					profile.AccountName = state.AccountName.Trim();
				}
				profile.Race = state.Race?.Trim() ?? string.Empty;
				profile.Profession = state.Profession?.Trim() ?? string.Empty;
				profile.Specialization = state.Specialization?.Trim() ?? string.Empty;
				profile.IsCharacterVerified = state.IsCharacterApiVerified;
			}
		}

		private void LoadGlanceDraft()
		{
			if (Profile.AtAGlance == null)
			{
				Profile.AtAGlance = new List<AtAGlanceEntry>();
			}
			for (int i = 0; i < Glance.Length; i++)
			{
				AtAGlanceEntry source = ((Profile.AtAGlance.Count > i) ? Profile.AtAGlance[i] : null);
				Glance[i] = new AtAGlanceEntry
				{
					AssetId = (source?.AssetId ?? 0),
					Title = (source?.Title ?? string.Empty),
					Description = (string.IsNullOrWhiteSpace(source?.Description) ? (source?.Tooltip ?? string.Empty) : source.Description),
					Tooltip = string.Empty
				};
			}
		}

		private void ApplyGlance()
		{
			if (Profile.AtAGlance == null)
			{
				Profile.AtAGlance = new List<AtAGlanceEntry>();
			}
			Profile.AtAGlance.Clear();
			AtAGlanceEntry[] glance = Glance;
			foreach (AtAGlanceEntry entry in glance)
			{
				if (entry.AssetId > 0)
				{
					Profile.AtAGlance.Add(new AtAGlanceEntry
					{
						AssetId = entry.AssetId,
						Title = (entry.Title?.Trim() ?? string.Empty),
						Description = (entry.Description?.Trim() ?? string.Empty),
						Tooltip = string.Empty
					});
				}
			}
		}

		private string GetUniqueProfileName(string baseName)
		{
			string candidate;
			string cleanBaseName = (candidate = LimitProfileName(string.IsNullOrWhiteSpace(baseName) ? "Profile" : baseName.Trim()));
			int suffix = 2;
			while (_availableProfiles.Any((CharacterProfile profile) => string.Equals(GetProfileName(profile), candidate, StringComparison.OrdinalIgnoreCase)))
			{
				string suffixText = $" {suffix}";
				int maxBaseLength = Math.Max(1, 30 - suffixText.Length);
				candidate = LimitProfileName(cleanBaseName, maxBaseLength) + suffixText;
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

		private void EnsureEditable()
		{
			if (!State.CanEditProfile)
			{
				throw new InvalidOperationException("Cannot manage profiles until Mumble Link detects your current character.");
			}
		}

		private static string GetProfileName(CharacterProfile profile)
		{
			if (!string.IsNullOrWhiteSpace(profile?.ProfileName))
			{
				return profile.ProfileName.Trim();
			}
			return "Default";
		}

		private static string GetCharacterDetailsText(PlayerState state)
		{
			string race = state.Race?.Trim() ?? string.Empty;
			string profession = state.Profession?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(race) && string.IsNullOrWhiteSpace(profession))
			{
				return "Unknown race/profession";
			}
			if (string.IsNullOrWhiteSpace(race))
			{
				return profession;
			}
			if (string.IsNullOrWhiteSpace(profession))
			{
				return race;
			}
			return race + " " + profession;
		}
	}
}
