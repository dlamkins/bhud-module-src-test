using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class Transformation
	{
		public int[] Armor { get; init; } = Array.Empty<int>();


		public int?[] Weapons { get; init; } = Array.Empty<int?>();


		public TransformationAttributes? Attributes { get; init; }

		public int? Buff { get; init; }

		public int? Palette { get; init; }
	}
}
