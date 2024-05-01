using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class ContextOverrideContextGroup
	{
		public ContextFlag[] Context { get; init; } = Array.Empty<ContextFlag>();


		public int? Recharge { get; init; }

		public int? Activation { get; init; }

		public int? ResourceCost { get; init; }

		public float? EnduranceCost { get; init; }

		public int? SupplyCost { get; init; }

		public int? UpkeepCost { get; init; }

		public List<FactBlock> Blocks { get; init; } = new List<FactBlock>();

	}
}
