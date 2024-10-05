using System.Collections.Generic;
using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class EquipmentItem : BaseItem
	{
		[DataMember]
		public double AttributeAdjustment { get; set; }

		[DataMember]
		public ItemEquipmentSlotType Slot { get; set; }

		[DataMember]
		public IReadOnlyList<int> StatChoices { get; set; }

		[DataMember]
		public int[] InfusionSlots { get; set; }

		public EquipmentItem()
		{
			base.Rarity = ItemRarity.Legendary;
		}
	}
}
