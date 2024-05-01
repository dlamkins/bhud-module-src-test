using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class Palette
	{
		public int Id { get; init; }

		public SkillPaletteType Type { get; init; }

		public WeaponType? WeaponType { get; init; }

		public SlotGroup[] Groups { get; init; } = Array.Empty<SlotGroup>();

	}
}
