using System;
using System.Linq;
using rp.spark.Models;

namespace rp.spark.UI.Views
{
	internal static class ProfileText
	{
		public static string Clean(string value)
		{
			return value?.Trim() ?? string.Empty;
		}

		public static string DisplayName(CharacterProfile profile, string fallback = "Unnamed Character")
		{
			if (!string.IsNullOrWhiteSpace(profile?.DisplayName))
			{
				return profile.DisplayName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(profile?.CharacterName))
			{
				return profile.CharacterName.Trim();
			}
			return fallback;
		}

		public static string AccountName(CharacterProfile profile, PlayerPresence presence, string fallback = "Unknown")
		{
			if (!string.IsNullOrWhiteSpace(presence?.AccountName))
			{
				return presence.AccountName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(profile?.AccountName))
			{
				return profile.AccountName.Trim();
			}
			return fallback;
		}

		public static string PresenceLocation(PlayerPresence presence)
		{
			if (!string.IsNullOrWhiteSpace(presence?.LocationName))
			{
				return presence.LocationName.Trim();
			}
			return "Unknown";
		}

		public static string PresenceRace(PlayerPresence presence, string fallback = "Unknown")
		{
			if (!string.IsNullOrWhiteSpace(presence?.Race))
			{
				return presence.Race.Trim();
			}
			return fallback;
		}

		public static string PresenceCharacterDetails(PlayerPresence presence)
		{
			return CharacterDetails(PresenceRace(presence, string.Empty), presence?.VisibleProfession());
		}

		public static string ProfileProfession(CharacterProfile profile)
		{
			if (!string.IsNullOrWhiteSpace(profile?.CustomProfession))
			{
				return profile.CustomProfession.Trim();
			}
			return Clean(profile?.Profession);
		}

		public static string ProfileCharacterDetails(CharacterProfile profile)
		{
			return CharacterDetails(profile?.Race, ProfileProfession(profile));
		}

		public static string SavedCharacterName(SavedProfile record)
		{
			if (!string.IsNullOrWhiteSpace(record?.Presence?.DisplayCharacterName))
			{
				return record.Presence.DisplayCharacterName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(record?.Profile?.DisplayName))
			{
				return record.Profile.DisplayName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(record?.Presence?.OfficialCharacterName))
			{
				return record.Presence.OfficialCharacterName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(record?.Profile?.CharacterName))
			{
				return record.Profile.CharacterName.Trim();
			}
			return "Unknown character";
		}

		public static string SavedCharacterName(SavedProfileSummary summary)
		{
			if (!string.IsNullOrWhiteSpace(summary?.DisplayCharacterName))
			{
				return summary.DisplayCharacterName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(summary?.OfficialCharacterName))
			{
				return summary.OfficialCharacterName.Trim();
			}
			return "Unknown character";
		}

		public static string SavedAccountName(SavedProfile record, string fallback = "Unknown")
		{
			if (!string.IsNullOrWhiteSpace(record?.Presence?.AccountName))
			{
				return record.Presence.AccountName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(record?.Profile?.AccountName))
			{
				return record.Profile.AccountName.Trim();
			}
			return fallback;
		}

		public static string SavedAccountName(SavedProfileSummary summary, string fallback = "Unknown")
		{
			if (!string.IsNullOrWhiteSpace(summary?.AccountName))
			{
				return summary.AccountName.Trim();
			}
			return fallback;
		}

		public static string SavedRace(SavedProfile record, string fallback = "Unknown")
		{
			if (!string.IsNullOrWhiteSpace(record?.Presence?.Race))
			{
				return record.Presence.Race.Trim();
			}
			if (!string.IsNullOrWhiteSpace(record?.Profile?.Race))
			{
				return record.Profile.Race.Trim();
			}
			return fallback;
		}

		public static string SavedRace(SavedProfileSummary summary, string fallback = "Unknown")
		{
			if (!string.IsNullOrWhiteSpace(summary?.Race))
			{
				return summary.Race.Trim();
			}
			return fallback;
		}

		public static string SavedProfession(SavedProfile record)
		{
			string profession = record?.Presence?.VisibleProfession();
			if (string.IsNullOrWhiteSpace(profession))
			{
				profession = ((!string.IsNullOrWhiteSpace(record?.Profile?.CustomProfession)) ? record.Profile.CustomProfession : record?.Profile?.Profession);
			}
			return Clean(profession);
		}

		public static string SavedProfession(SavedProfileSummary summary)
		{
			return Clean((!string.IsNullOrWhiteSpace(summary?.CustomProfession)) ? summary.CustomProfession : summary?.Profession);
		}

		public static string SavedCharacterDetails(SavedProfile record)
		{
			return CharacterDetails(SavedRace(record, string.Empty), SavedProfession(record));
		}

		public static string SavedCharacterDetails(SavedProfileSummary summary)
		{
			return CharacterDetails(SavedRace(summary, string.Empty), SavedProfession(summary));
		}

		public static DateTime SavedLastSeen(SavedProfile record)
		{
			DateTime lastSeen = (record?.Presence?.LastSeen).GetValueOrDefault();
			if (!(lastSeen == default(DateTime)))
			{
				return lastSeen.ToUniversalTime();
			}
			return record?.CachedAt ?? default(DateTime);
		}

		public static DateTime SavedLastSeen(SavedProfileSummary summary)
		{
			DateTime lastSeen = summary?.LastSeen ?? default(DateTime);
			if (!(lastSeen == default(DateTime)))
			{
				return lastSeen.ToUniversalTime();
			}
			return summary?.CachedAt ?? default(DateTime);
		}

		public static string CharacterDetails(string race, string profession)
		{
			race = Clean(race);
			profession = Clean(profession);
			if (string.IsNullOrWhiteSpace(race) && string.IsNullOrWhiteSpace(profession))
			{
				return string.Empty;
			}
			if (string.IsNullOrWhiteSpace(race))
			{
				return profession;
			}
			if (string.IsNullOrWhiteSpace(profession))
			{
				return race;
			}
			return race + " | " + profession;
		}

		public static string JoinSearchText(params string[] parts)
		{
			return string.Join(" ", from part in parts ?? new string[0]
				select Clean(part) into part
				where !string.IsNullOrWhiteSpace(part)
				select part);
		}

		public static string FormatShortTime(DateTime dateTime, string fallback = "")
		{
			if (dateTime == default(DateTime))
			{
				return fallback;
			}
			return dateTime.ToLocalTime().ToString("MMM d h:mm tt");
		}
	}
}
