using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.DataModels;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class PvpAmulet : BaseItem
	{
		[DataMember]
		public ItemAttributes Attributes { get; set; } = new ItemAttributes();


		public string AttributesString
		{
			get
			{
				List<(int, string)> result = new List<(int, string)>();
				if (Attributes.Power.HasValue)
				{
					result.Add((Attributes.Power.Value, $"+ {Attributes.Power} {AttributeType.Power.GetDisplayName()}"));
				}
				if (Attributes.Precision.HasValue)
				{
					result.Add((Attributes.Precision.Value, $"+ {Attributes.Precision} {AttributeType.Precision.GetDisplayName()}"));
				}
				if (Attributes.Toughness.HasValue)
				{
					result.Add((Attributes.Toughness.Value, $"+ {Attributes.Toughness} {AttributeType.Toughness.GetDisplayName()}"));
				}
				if (Attributes.Vitality.HasValue)
				{
					result.Add((Attributes.Vitality.Value, $"+ {Attributes.Vitality} {AttributeType.Vitality.GetDisplayName()}"));
				}
				if (Attributes.CritDamage.HasValue)
				{
					result.Add((Attributes.CritDamage.Value, $"+ {Attributes.CritDamage} {AttributeType.CritDamage.GetDisplayName()}"));
				}
				if (Attributes.Healing.HasValue)
				{
					result.Add((Attributes.Healing.Value, $"+ {Attributes.Healing} {AttributeType.Healing.GetDisplayName()}"));
				}
				if (Attributes.ConditionDamage.HasValue)
				{
					result.Add((Attributes.ConditionDamage.Value, $"+ {Attributes.ConditionDamage} {AttributeType.ConditionDamage.GetDisplayName()}"));
				}
				if (Attributes.BoonDuration.HasValue)
				{
					result.Add((Attributes.BoonDuration.Value, $"+ {Attributes.BoonDuration} {AttributeType.BoonDuration.GetDisplayName()}"));
				}
				if (Attributes.ConditionDuration.HasValue)
				{
					result.Add((Attributes.ConditionDuration.Value, $"+ {Attributes.ConditionDuration} {AttributeType.ConditionDuration.GetDisplayName()}"));
				}
				return string.Join(Environment.NewLine, from e in result
					orderby e.amount descending
					select e.text);
			}
		}

		public PvpAmulet()
		{
			base.TemplateSlot = TemplateSlotType.PvpAmulet;
			base.Type = Kenedia.Modules.Core.DataModels.ItemType.PvpAmulet;
		}

		public override void Apply(Gw2Sharp.WebApi.V2.Models.PvpAmulet amulet)
		{
			base.Apply(amulet);
			Attributes = amulet.Attributes;
		}
	}
}
