using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	[Serializable]
	public class DailyBountyData
	{
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; } = "";


		[JsonProperty("name")]
		private string _name { get; set; } = "Daily Bounty";


		public string Name => GetLocalizedName(_name);

		[JsonProperty("abbreviation")]
		private string _abbreviation { get; set; } = "B";


		public string Abbreviation => GetLocalizedAbbreviation(_abbreviation);

		[JsonProperty("localizedNames")]
		public LocalizedStrings? LocalizedNames { get; set; }

		[JsonProperty("localizedAbbreviations")]
		public LocalizedStrings? LocalizedAbbreviations { get; set; }

		[JsonProperty("rotationType")]
		public string RotationType { get; set; } = "dayOfYearIndexed";


		[JsonProperty("modulo")]
		public int Modulo { get; set; } = 365;


		[JsonProperty("offset")]
		public int Offset { get; set; }

		[JsonProperty("rotation")]
		public List<List<BountyEncounterReference>> Rotation { get; set; } = new List<List<BountyEncounterReference>>();


		[JsonProperty("staticEncounters")]
		public List<BountyEncounterReference> StaticEncounters { get; set; } = new List<BountyEncounterReference>();


		[JsonProperty("bossSlots")]
		public List<BossSlotRotation> BossSlots { get; set; } = new List<BossSlotRotation>();


		public string GetLocalizedName(string defaultName)
		{
			string locale = GetUserLocale();
			return LocalizedNames?.GetValue(locale) ?? defaultName;
		}

		public string GetLocalizedAbbreviation(string defaultAbbreviation)
		{
			string locale = GetUserLocale();
			return LocalizedAbbreviations?.GetValue(locale) ?? defaultAbbreviation;
		}

		private string GetUserLocale()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected I4, but got Unknown
			try
			{
				SettingEntry<Locale> userLocaleSetting = GameService.Overlay.get_UserLocale();
				if (userLocaleSetting == null)
				{
					return "en";
				}
				Locale locale = userLocaleSetting.get_Value();
				return (int)locale switch
				{
					0 => "en", 
					3 => "fr", 
					2 => "de", 
					1 => "es", 
					_ => "en", 
				};
			}
			catch
			{
				return "en";
			}
		}
	}
}
