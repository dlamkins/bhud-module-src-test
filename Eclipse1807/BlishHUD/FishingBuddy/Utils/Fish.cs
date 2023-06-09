using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Blish_HUD;
using Blish_HUD.Content;
using Eclipse1807.BlishHUD.FishingBuddy.Properties;
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
		public enum FishingHole
		{
			None,
			Any,
			[EnumMember(Value = "Boreal Fish")]
			BorealFish,
			[EnumMember(Value = "Cavern Fish")]
			CavernFish,
			[EnumMember(Value = "Channel Fish")]
			ChannelFish,
			[EnumMember(Value = "Coastal Fish")]
			CoastalFish,
			[EnumMember(Value = "Deep Fishing Hole")]
			DeepFishingHole,
			[EnumMember(Value = "Desert Fish")]
			DesertFish,
			[EnumMember(Value = "Freshwater Fish")]
			FreshwaterFish,
			[EnumMember(Value = "Grotto Fish")]
			GrottoFish,
			[EnumMember(Value = "Lake Fish")]
			LakeFish,
			[EnumMember(Value = "Lutgardis Trout")]
			LutgardisTrout,
			[EnumMember(Value = "Mysterious Waters Fish")]
			MysteriousWatersFish,
			[EnumMember(Value = "Noxious Water Fish")]
			NoxiousWaterFish,
			[EnumMember(Value = "Offshore Fish")]
			OffshoreFish,
			[EnumMember(Value = "Polluted Lake Fish")]
			PollutedLakeFish,
			[EnumMember(Value = "Quarry Fish")]
			QuarryFish,
			[EnumMember(Value = "Rare Fish")]
			RareFish,
			[EnumMember(Value = "River Fish")]
			RiverFish,
			[EnumMember(Value = "Saltwater Fish")]
			SaltwaterFish,
			[EnumMember(Value = "Special Fishing Hole")]
			SpecialFishingHole,
			[EnumMember(Value = "Shore Fish")]
			ShoreFish,
			[EnumMember(Value = "Volcanic Fish")]
			VolcanicFish,
			[EnumMember(Value = "Wreckage Site")]
			WreckageSite
		}

		internal static readonly Logger Logger = Logger.GetLogger(typeof(Fish));

		public string Name { get; set; }

		public int ItemId { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ItemRarity Rarity { get; set; }

		[JsonProperty("FishingHole")]
		public FishingHole Hole { get; set; }

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

		public static string BuildFishTooltip(Fish fish)
		{
			string name = Strings.FishName + ": " + fish.Name;
			string bait = Strings.FishFavoredBait + ": " + fish.Bait.GetEnumMemberValue();
			string time = Strings.FishTimeOfDay + ": " + fish.Time.GetEnumMemberValue();
			string hole = Strings.FishFishingHole + ": " + fish.Hole.GetEnumMemberValue() + (fish.OpenWater ? (", " + Strings.OpenWater) : string.Empty);
			string achieve = Strings.Achievement + ": " + fish.Achievement;
			string rarity = Strings.Rarity + ": " + Strings.ResourceManager.GetString(fish.Rarity.ToString(), Strings.Culture);
			string hiddenReason = string.Empty;
			if (FishingBuddyModule._useAPIToken)
			{
				if (!fish.Visible && fish.Caught)
				{
					hiddenReason = Strings.Hidden + ": " + Strings.TimeOfDay + ", " + Strings.HiddenCaught;
				}
				else if (!fish.Visible)
				{
					hiddenReason = Strings.Hidden + ": " + Strings.TimeOfDay;
				}
				else if (fish.Caught)
				{
					hiddenReason = Strings.Hidden + ": " + Strings.HiddenCaught;
				}
			}
			string notes = ((!string.IsNullOrWhiteSpace(fish.Notes)) ? (Strings.Notes + ": " + Strings.ResourceManager.GetString(fish.Notes, Strings.Culture)) : string.Empty);
			return FishingBuddyModule._fishPanelTooltipDisplay.get_Value().Replace("@1", name).Replace("@2", bait)
				.Replace("@3", time)
				.Replace("@4", hole)
				.Replace("@5", achieve)
				.Replace("@6", rarity)
				.Replace("@7", hiddenReason)
				.Replace("@8", notes)
				.Replace("#1", fish.Name)
				.Replace("#2", fish.Bait.GetEnumMemberValue())
				.Replace("#3", fish.Time.GetEnumMemberValue())
				.Replace("#4", fish.Hole.GetEnumMemberValue() + (fish.OpenWater ? (", " + Strings.OpenWater) : string.Empty))
				.Replace("#5", fish.Achievement)
				.Replace("#6", Strings.ResourceManager.GetString(fish.Rarity.ToString(), Strings.Culture))
				.Replace("#8", Strings.ResourceManager.GetString(fish.Notes, Strings.Culture))
				.Replace("\\n", "\n")
				.Replace("\n\n", "\n")
				.Trim();
		}
	}
}
