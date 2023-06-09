using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
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
}
