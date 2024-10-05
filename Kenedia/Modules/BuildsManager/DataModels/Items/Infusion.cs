using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class Infusion : BaseItem
	{
		[DataMember]
		public LocalizedString Bonuses { get; set; } = new LocalizedString();


		public string Bonus
		{
			get
			{
				return Bonuses.Text.InterpretItemDescription();
			}
			set
			{
				Bonuses.Text = value;
			}
		}

		public List<BonusStat> Attributes { get; set; } = new List<BonusStat>();


		public override void Apply(Item item)
		{
			base.Apply(item);
			if (!(item.Type == ItemType.UpgradeComponent))
			{
				return;
			}
			ItemUpgradeComponent upgrade = (ItemUpgradeComponent)item;
			if (upgrade.Details.InfixUpgrade == null || upgrade.Details.InfixUpgrade!.Attributes == null)
			{
				return;
			}
			Bonus = upgrade.Details.InfixUpgrade!.Buff.Description;
			base.DisplayText = upgrade.Details.InfixUpgrade!.Buff.Description.Trim();
			foreach (ItemUpgradeAttribute b in upgrade.Details.InfixUpgrade!.Attributes)
			{
				Attributes.Add(new BonusStat
				{
					Amount = b.Modifier,
					Type = (Enum.TryParse<BonusType>(b.Attribute.ToString(), out var type) ? type : BonusType.Unkown)
				});
			}
		}
	}
}
