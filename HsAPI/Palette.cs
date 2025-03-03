using System;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class Palette : IIdentifiable<int>
	{
		public int Id { get; set; }

		public SkillPaletteType Type { get; init; }

		public WeaponType? WeaponType { get; init; }

		public SlotGroup[] Groups { get; init; } = Array.Empty<SlotGroup>();

	}
}
