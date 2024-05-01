using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class AttributeAdjustFact : Fact
	{
		public override FactType Type { get; } = FactType.AttributeAdjust;


		public int[] Range { get; init; } = Array.Empty<int>();


		public BaseAttribute Target { get; init; }
	}
}
