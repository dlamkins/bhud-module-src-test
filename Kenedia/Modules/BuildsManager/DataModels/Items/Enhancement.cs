using System.Collections.Generic;
using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class Enhancement : BaseItem
	{
		[DataMember]
		public List<BonusStat> Bonuses { get; set; } = new List<BonusStat>();


		[DataMember]
		public ConsumableDetails Details { get; set; } = new ConsumableDetails();


		public Enhancement()
		{
			base.TemplateSlot = TemplateSlotType.Enhancement;
		}

		public override void Apply(Item item)
		{
			base.Apply(item);
			if (item.Type == ItemType.Consumable)
			{
				ItemConsumable consumable = (ItemConsumable)item;
				Details.Description = consumable.Details.Description;
				Details.DurationMs = consumable.Details.DurationMs;
				Details.Name = consumable.Details.Name;
			}
		}
	}
}
