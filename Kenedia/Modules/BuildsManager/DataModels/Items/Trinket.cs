using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class Trinket : EquipmentItem
	{
		public override void Apply(Item item)
		{
			base.Apply(item);
			if (item.Type == ItemType.Trinket)
			{
				ItemTrinket trinket = (ItemTrinket)item;
				base.AttributeAdjustment = trinket.Details.AttributeAdjustment;
				base.StatChoices = trinket.Details.StatChoices;
				base.InfusionSlots = new int[trinket.Details.InfusionSlots.Count];
				base.TemplateSlot = trinket.Details.Type.Value switch
				{
					ItemTrinketType.Amulet => TemplateSlotType.Amulet, 
					ItemTrinketType.Ring => TemplateSlotType.Ring_1, 
					ItemTrinketType.Accessory => TemplateSlotType.Accessory_1, 
					_ => TemplateSlotType.None, 
				};
			}
			else if (item.Type == ItemType.Back)
			{
				ItemBack back = (ItemBack)item;
				base.AttributeAdjustment = back.Details.AttributeAdjustment;
				base.StatChoices = back.Details.StatChoices;
				base.InfusionSlots = new int[back.Details.InfusionSlots.Count];
				base.TemplateSlot = TemplateSlotType.Back;
			}
		}
	}
}
