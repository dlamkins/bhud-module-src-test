using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class Modifier
	{
		public int Id { get; init; }

		public float BaseAmount { get; init; }

		public float FormulaParam1 { get; init; }

		public float FormulaParam2 { get; init; }

		public BuffScaling Formula { get; init; }

		public string? Target { get; init; }

		public BaseAttribute? SourceAttribute { get; init; }

		public string Description { get; init; } = "";


		public DescriptionOverride[] DescriptionOverride { get; init; } = Array.Empty<DescriptionOverride>();


		public ModifierFlag[] Flags { get; init; } = Array.Empty<ModifierFlag>();


		public int? SourceTraitReq { get; init; }

		public int? TargetTraitReq { get; init; }

		public GameMode? Mode { get; init; }
	}
}
