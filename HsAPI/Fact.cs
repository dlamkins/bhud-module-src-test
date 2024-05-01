using System;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonConverter(typeof(JsonSubtypes), new object[] { "Type" })]
	[JsonSubtypes.KnownSubType(typeof(AdjustByAttributeAndLevelFact), FactType.AdjustByAttributeAndLevel)]
	[JsonSubtypes.KnownSubType(typeof(AttributeAdjustFact), FactType.AttributeAdjust)]
	[JsonSubtypes.KnownSubType(typeof(BuffFact), FactType.Buff)]
	[JsonSubtypes.KnownSubType(typeof(BuffBriefFact), FactType.BuffBrief)]
	[JsonSubtypes.KnownSubType(typeof(DistanceFact), FactType.Distance)]
	[JsonSubtypes.KnownSubType(typeof(NumberFact), FactType.Number)]
	[JsonSubtypes.KnownSubType(typeof(PercentFact), FactType.Percent)]
	[JsonSubtypes.KnownSubType(typeof(PercentHpSelfDamageFact), FactType.PercentHpSelfDamage)]
	[JsonSubtypes.KnownSubType(typeof(PercentHealthFact), FactType.PercentHealth)]
	[JsonSubtypes.KnownSubType(typeof(PercentLifeForceCostFact), FactType.PercentLifeForceCost)]
	[JsonSubtypes.KnownSubType(typeof(PercentLifeForceGainFact), FactType.PercentLifeForceGain)]
	[JsonSubtypes.KnownSubType(typeof(DamageFact), FactType.Damage)]
	[JsonSubtypes.KnownSubType(typeof(TimeFact), FactType.Time)]
	[JsonSubtypes.KnownSubType(typeof(ComboFieldFact), FactType.ComboField)]
	[JsonSubtypes.KnownSubType(typeof(ComboFinisherFact), FactType.ComboFinisher)]
	[JsonSubtypes.KnownSubType(typeof(AttributeConversionFact), FactType.AttributeConversion)]
	[JsonSubtypes.KnownSubType(typeof(NoDataFact), FactType.NoData)]
	[JsonSubtypes.KnownSubType(typeof(PrefixedBuffFact), FactType.PrefixedBuff)]
	[JsonSubtypes.KnownSubType(typeof(PrefixedBuffBriefFact), FactType.PrefixedBuffBrief)]
	[JsonSubtypes.KnownSubType(typeof(RangeFact), FactType.Range)]
	[JsonSubtypes.KnownSubType(typeof(StunBreakFact), FactType.StunBreak)]
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public abstract class Fact
	{
		public abstract FactType Type { get; }

		public int Icon { get; init; }

		public string? Text { get; init; }

		public int Order { get; init; }

		public int[] RequiresTrait { get; init; } = Array.Empty<int>();


		public int? DefianceBreak { get; init; }

		public int? InsertBefore { get; init; }

		public int? SkipNext { get; init; }
	}
}
