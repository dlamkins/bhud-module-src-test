using System;
using Blish_HUD;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	[Serializable]
	public class EncounterInterface
	{
		[JsonProperty("id")]
		public string Id = "undefined";

		[JsonIgnore]
		public string Name = "undefined";

		[JsonIgnore]
		public string Abbriviation = "undefined";

		[JsonProperty("assetId")]
		public int AssetId;

		[JsonProperty("localizedNames")]
		protected LocalizedStrings? LocalizedNames { get; set; }

		[JsonProperty("localizedAbbreviations")]
		protected LocalizedStrings? LocalizedAbbreviations { get; set; }

		protected string GetLocalizedName(string defaultName)
		{
			string locale = GetUserLocale();
			return LocalizedNames?.GetValue(locale) ?? defaultName;
		}

		protected string GetLocalizedAbbreviation(string defaultAbbreviation)
		{
			string locale = GetUserLocale();
			return LocalizedAbbreviations?.GetValue(locale) ?? defaultAbbreviation;
		}

		protected string GetUserLocale()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected I4, but got Unknown
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
					4 => "en", 
					5 => "en", 
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
