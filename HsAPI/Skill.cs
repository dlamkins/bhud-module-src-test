using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class Skill
	{
		public int Id { get; init; }

		public string Name { get; init; } = "";


		public string? NameBrief { get; init; }

		public string? Description { get; init; }

		public string? DescriptionBrief { get; init; }

		public int? Icon { get; init; }

		public string[] Categories { get; init; } = Array.Empty<string>();


		public int[] Palettes { get; init; } = Array.Empty<int>();


		public int[] RelatedSkills { get; init; } = Array.Empty<int>();


		public ConditionalSkill[] AmbushSkills { get; init; } = Array.Empty<ConditionalSkill>();


		[Obsolete("BundleSkills will probably be removed.")]
		public int[] BundleSkills { get; init; } = Array.Empty<int>();


		public int? BundleItem { get; init; }

		public Modifier[] Modifiers { get; init; } = Array.Empty<Modifier>();


		public BuffType? BuffType { get; init; }

		public Flag[] Flags { get; init; } = Array.Empty<Flag>();


		public int? ToolbeltSkill { get; init; }

		public Transformation? Transformation { get; init; }

		public ContextOverrideContextGroup[] OverrideGroups { get; init; } = Array.Empty<ContextOverrideContextGroup>();


		public int? Recharge { get; init; }

		public int? Activation { get; init; }

		public int? ResourceCost { get; init; }

		public float? EnduranceCost { get; init; }

		public int? SupplyCost { get; init; }

		public int? UpkeepCost { get; init; }

		public List<FactBlock> Blocks { get; set; } = new List<FactBlock>();

	}
}
