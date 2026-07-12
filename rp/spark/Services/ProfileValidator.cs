using System;
using System.Collections.Generic;
using System.Linq;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class ProfileValidator
	{
		public ProfileValidationResult Validate(CharacterProfile profile)
		{
			ProfileValidationResult result = new ProfileValidationResult();
			if (profile == null)
			{
				result.AddError("Profile missing!");
				return result;
			}
			if (profile.SchemaVersion != 1)
			{
				result.AddError("Profile schema is invalid.");
			}
			if (string.IsNullOrWhiteSpace(profile.ProfileName))
			{
				result.AddError("Profile name is required.");
			}
			string profileName = profile.ProfileName;
			if (profileName != null && profileName.Length > 30)
			{
				result.AddError("Profile name is too long.");
			}
			string characterName = profile.CharacterName;
			if (characterName != null && characterName.Length > 20)
			{
				result.AddError("Character name is too long.");
			}
			string accountName = profile.AccountName;
			if (accountName != null && accountName.Length > 30)
			{
				result.AddError("Account name is too long.");
			}
			string displayName = profile.DisplayName;
			if (displayName != null && displayName.Length > 30)
			{
				result.AddError("Display name is too long.");
			}
			string pronouns = profile.Pronouns;
			if (pronouns != null && pronouns.Length > 20)
			{
				result.AddError("Pronouns are too long.");
			}
			string race = profile.Race;
			if (race != null && race.Length > 16)
			{
				result.AddError("Race is too long.");
			}
			string customRace = profile.CustomRace;
			if (customRace != null && customRace.Length > 16)
			{
				result.AddError("Custom race is too long.");
			}
			string profession = profile.Profession;
			if (profession != null && profession.Length > 40)
			{
				result.AddError("Profession is too long.");
			}
			string specialization = profile.Specialization;
			if (specialization != null && specialization.Length > 40)
			{
				result.AddError("Specialization is too long.");
			}
			string customProfession = profile.CustomProfession;
			if (customProfession != null && customProfession.Length > 40)
			{
				result.AddError("Custom profession is too long.");
			}
			if (!Enum.IsDefined(typeof(ProfileExperience), profile.Experience))
			{
				result.AddError("Experience selection is invalid.");
			}
			if (((uint)profile.Preferences & 0xFFFFFFC0u) != 0)
			{
				result.AddError("Preferences selection is invalid.");
			}
			if (((uint)profile.Themes & 0xFFFFFFF0u) != 0)
			{
				result.AddError("Themes selection is invalid.");
			}
			if (((uint)profile.Styles & 0xFFFFFFE0u) != 0)
			{
				result.AddError("Styles selection is invalid.");
			}
			string knownFor = profile.KnownFor;
			if (knownFor != null && knownFor.Length > 500)
			{
				result.AddError("Known For is too long.");
			}
			string description = profile.Description;
			if (description != null && description.Length > 8000)
			{
				result.AddError("Description is too long.");
			}
			string currently = profile.Currently;
			if (currently != null && currently.Length > 500)
			{
				result.AddError("Currently info is too long.");
			}
			string outOfCharacterInfo = profile.OutOfCharacterInfo;
			if (outOfCharacterInfo != null && outOfCharacterInfo.Length > 1000)
			{
				result.AddError("Other information is too long.");
			}
			if (profile.AtAGlance != null && profile.AtAGlance.Count > 5)
			{
				result.AddError($"At a glance can contain {5} total entries. Please fix your JSON to remove extras from the file.");
			}
			IEnumerable<AtAGlanceEntry> atAGlance = profile.AtAGlance;
			foreach (AtAGlanceEntry entry in atAGlance ?? Enumerable.Empty<AtAGlanceEntry>())
			{
				if (entry == null)
				{
					result.AddError("At a glance entries are null. Please delete the profile in your local files to fix.");
					continue;
				}
				if (entry.AssetId <= 0)
				{
					result.AddError("At a glance icon asset IDs must be positive numbers!");
				}
				string title = entry.Title;
				if (title != null && title.Length > 60)
				{
					result.AddError("At a glance title is too long.");
				}
				string description2 = entry.Description;
				if (description2 != null && description2.Length > 280)
				{
					result.AddError("At a glance description is too long.");
				}
				string tooltip = entry.Tooltip;
				if (tooltip != null && tooltip.Length > 160)
				{
					result.AddError("At a glance tooltip is too long.");
				}
			}
			return result;
		}
	}
}
