using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Blish_HUD;
using Blish_HUD.Content;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public class Fish
	{
		[Flags]
		[JsonConverter(typeof(StringEnumConverter))]
		public enum TimeOfDay
		{
			None = 0x0,
			Dawn = 0x1,
			Day = 0x2,
			Dusk = 0x4,
			Night = 0x8,
			[EnumMember(Value = "Dawn/Dusk")]
			DawnDusk = 0x5,
			[EnumMember(Value = "Dusk/Dawn")]
			DuskDawn = 0x5,
			Any = 0xF
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public enum FishBait
		{
			Any,
			[EnumMember(Value = "Fish Eggs")]
			FishEggs,
			[EnumMember(Value = "Glow Worms")]
			GlowWorms,
			[EnumMember(Value = "Haiju Minnows")]
			HaijuMinnows,
			[EnumMember(Value = "Lava Beetles")]
			LavaBeetles,
			Leeches,
			[EnumMember(Value = "Lightning Bugs")]
			LightningBugs,
			Mackerel,
			Minnows,
			Nightcrawlers,
			[EnumMember(Value = "Ramshorn Snails")]
			RamshornSnails,
			Sardines,
			Scorpions,
			Shrimplings,
			[EnumMember(Value = "Sparkfly Larvae")]
			SparkflyLarvae
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(Fish));

		public string Name { get; set; }

		public int ItemId { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ItemRarity Rarity { get; set; }

		public string FishingHole { get; set; }

		public FishBait Bait { get; set; }

		public TimeOfDay Time { get; set; }

		public bool OpenWater { get; set; }

		public string Location { get; set; }

		public List<int> Locations { get; set; }

		public string Achievement { get; set; }

		public int AchievementId { get; set; }

		public List<int> AchievementIds { get; set; }

		public string Notes { get; set; }

		public RenderUrl Icon { get; set; }

		public bool Visible { get; set; } = true;


		public bool Caught { get; set; }

		public AsyncTexture2D IconImg { get; set; }

		public string ChatLink { get; set; }
	}
}
