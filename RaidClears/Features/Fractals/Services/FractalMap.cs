using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Fractals.Services
{
	public class FractalMap
	{
		[JsonProperty("label")]
		private string _label = "undefined";

		[JsonProperty("short")]
		public string ShortLabel = "undefined";

		[JsonProperty("api")]
		public string ApiLabel = "undefined";

		[JsonProperty("scales")]
		public List<int> Scales = new List<int>();

		[JsonProperty("id")]
		public int MapId;

		[JsonProperty("localizedNames")]
		private LocalizedStrings? LocalizedNames { get; set; }

		public string Label
		{
			get
			{
				return GetLocalizedName(_label);
			}
			set
			{
				_label = value;
			}
		}

		private string GetLocalizedName(string defaultLabel)
		{
			string locale = GetUserLocale();
			return LocalizedNames?.GetValue(locale) ?? defaultLabel;
		}

		private string GetUserLocale()
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

		public EncounterInterface ToEncounterInterface()
		{
			return new EncounterInterface
			{
				Id = ApiLabel,
				Name = Label,
				Abbriviation = ShortLabel
			};
		}
	}
}
