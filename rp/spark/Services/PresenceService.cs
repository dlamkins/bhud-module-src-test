using System;
using System.Threading;
using System.Threading.Tasks;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class PresenceService
	{
		private const string HiddenLocationName = "Hidden";

		private const string UnknownLocationName = "Unknown";

		private readonly ProfileRepository _profileRepository;

		private readonly PlayerStateService _playerState;

		private readonly SparkSettings _settings;

		public PresenceService(ProfileRepository profileRepository, PlayerStateService playerState, SparkSettings settings)
		{
			_profileRepository = profileRepository;
			_playerState = playerState;
			_settings = settings;
		}

		public PlayerPresence GetCurrentPresence()
		{
			PlayerState state = _playerState.GetCached();
			CharacterProfile activeProfile = _profileRepository.LoadActiveForCharacter(state.AccountName, state.OfficialCharacterName);
			return BuildPresence(state, activeProfile);
		}

		public async Task<PlayerPresence> GetCurrentPresenceAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			PlayerState state = await _playerState.GetCurrentAsync(cancellationToken);
			CharacterProfile activeProfile = _profileRepository.LoadActiveForCharacter(state.AccountName, state.OfficialCharacterName);
			return BuildPresence(state, activeProfile);
		}

		public PlayerPresence BuildPresence(PlayerState state, CharacterProfile activeProfile)
		{
			state = state ?? _playerState.GetCached();
			string accountName = TextUtil.FirstNonEmpty(state.AccountName, activeProfile?.AccountName);
			string officialCharacterName = TextUtil.FirstNonEmpty(state.OfficialCharacterName, activeProfile?.CharacterName);
			string activeProfileId = _profileRepository.GetActiveProfileId(accountName, officialCharacterName);
			bool hasActiveProfile = activeProfile != null && !string.IsNullOrWhiteSpace(activeProfileId) && string.Equals(activeProfile.ProfileId, activeProfileId, StringComparison.OrdinalIgnoreCase);
			RPStatus status = NormalizeStatus((_settings?.CurrentStatus?.get_Value()).GetValueOrDefault());
			bool broadcastEnabled = (_settings?.BroadcastProfile?.get_Value()).GetValueOrDefault();
			bool locationHidden = (_settings?.HideLocation?.get_Value()).GetValueOrDefault();
			bool locationResolved = locationHidden || state.IsLocationResolved;
			bool isInGame = state.CanEditProfile;
			PlayerPresence playerPresence = new PlayerPresence();
			playerPresence.AccountName = accountName;
			playerPresence.OfficialCharacterName = officialCharacterName;
			playerPresence.DisplayCharacterName = activeProfile?.DisplayName?.Trim() ?? string.Empty;
			playerPresence.Race = TextUtil.FirstNonEmpty(state.Race, activeProfile?.Race);
			playerPresence.CustomRace = activeProfile?.CustomRace?.Trim() ?? string.Empty;
			playerPresence.Profession = TextUtil.FirstNonEmpty(state.Profession, activeProfile?.Profession);
			playerPresence.CustomProfession = activeProfile?.CustomProfession?.Trim() ?? string.Empty;
			playerPresence.ActiveProfileId = (hasActiveProfile ? activeProfile.ProfileId : string.Empty);
			playerPresence.ActiveProfileName = ((!hasActiveProfile) ? string.Empty : (activeProfile.ProfileName?.Trim() ?? string.Empty));
			playerPresence.IsMature = hasActiveProfile && activeProfile.IsMature;
			playerPresence.ProfileUpdatedAtTime = (hasActiveProfile ? activeProfile.UpdatedAt : default(DateTime));
			playerPresence.Status = status;
			playerPresence.Currently = ((!hasActiveProfile) ? string.Empty : (activeProfile.Currently?.Trim() ?? string.Empty));
			playerPresence.OutOfCharacterInfo = ((!hasActiveProfile) ? string.Empty : (activeProfile.OutOfCharacterInfo?.Trim() ?? string.Empty));
			playerPresence.LocationName = GetLocationName(state, locationHidden, locationResolved);
			playerPresence.IsLocationHidden = locationHidden;
			playerPresence.Region = (_settings?.RegionFilter?.get_Value()).GetValueOrDefault();
			playerPresence.IsVerified = state.IsCharacterApiVerified;
			playerPresence.IsInGame = isInGame;
			playerPresence.HasActiveProfile = hasActiveProfile;
			playerPresence.ShareEnabled = broadcastEnabled && isInGame;
			playerPresence.LastSeen = DateTime.UtcNow;
			playerPresence.CanShare = CanShare(playerPresence);
			playerPresence.ShareBlockReason = GetShareBlockReason(playerPresence);
			return playerPresence;
		}

		private static bool CanShare(PlayerPresence snapshot)
		{
			if (snapshot.ShareEnabled && snapshot.Status != RPStatus.Invisible && snapshot.HasActiveProfile && !string.IsNullOrWhiteSpace(snapshot.AccountName))
			{
				return !string.IsNullOrWhiteSpace(snapshot.OfficialCharacterName);
			}
			return false;
		}

		private static string GetShareBlockReason(PlayerPresence snapshot)
		{
			if (!snapshot.ShareEnabled)
			{
				if (!snapshot.IsInGame)
				{
					return "No character detected.";
				}
				return "Profile sharing is disabled.";
			}
			if (snapshot.Status == RPStatus.Invisible)
			{
				return "Status is invisible.";
			}
			if (!snapshot.HasActiveProfile)
			{
				return "No active profile selected.";
			}
			if (string.IsNullOrWhiteSpace(snapshot.AccountName))
			{
				return "Account name unavailable.";
			}
			if (string.IsNullOrWhiteSpace(snapshot.OfficialCharacterName))
			{
				return "Character name unavailable.";
			}
			return string.Empty;
		}

		private static string GetLocationName(PlayerState state, bool locationHidden, bool locationResolved)
		{
			if (locationHidden)
			{
				return "Hidden";
			}
			if (!locationResolved)
			{
				return "Unknown";
			}
			if (!string.IsNullOrWhiteSpace(state.LocationName))
			{
				return state.LocationName.Trim();
			}
			return "Unknown";
		}

		private static RPStatus NormalizeStatus(RPStatus status)
		{
			if (status != RPStatus.Offline)
			{
				return status;
			}
			return RPStatus.Online;
		}
	}
}
