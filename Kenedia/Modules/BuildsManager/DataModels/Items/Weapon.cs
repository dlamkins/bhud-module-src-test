using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class Weapon : EquipmentItem
	{
		[DataMember]
		public ItemWeaponType WeaponType { get; protected set; }

		[DataMember]
		public int Defense { get; protected set; }

		public override void Apply(Item item)
		{
			base.Apply(item);
			if (item.Type == ItemType.Weapon)
			{
				ItemWeapon weapon = (ItemWeapon)item;
				base.AttributeAdjustment = weapon.Details.AttributeAdjustment;
				WeaponType = weapon.Details.Type;
				base.StatChoices = weapon.Details.StatChoices;
				base.InfusionSlots = new int[weapon.Details.InfusionSlots.Count];
				Defense = weapon.Details.Defense;
				base.TemplateSlot = weapon.Details.Type.Value switch
				{
					ItemWeaponType.Speargun => TemplateSlotType.Aquatic, 
					ItemWeaponType.Harpoon => TemplateSlotType.Aquatic, 
					ItemWeaponType.Trident => TemplateSlotType.Aquatic, 
					ItemWeaponType.Shield => TemplateSlotType.OffHand, 
					ItemWeaponType.Focus => TemplateSlotType.OffHand, 
					ItemWeaponType.Torch => TemplateSlotType.OffHand, 
					ItemWeaponType.Warhorn => TemplateSlotType.OffHand, 
					_ => TemplateSlotType.MainHand, 
				};
			}
		}
	}
}
