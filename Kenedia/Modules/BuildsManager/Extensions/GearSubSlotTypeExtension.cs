using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class GearSubSlotTypeExtension
	{
		public static GearSubSlotTypeType GetGroupType(this TemplateSlotType templateSlotType)
		{
			switch (templateSlotType)
			{
			case TemplateSlotType.Head:
			case TemplateSlotType.Shoulder:
			case TemplateSlotType.Chest:
			case TemplateSlotType.Hand:
			case TemplateSlotType.Leg:
			case TemplateSlotType.Foot:
			case TemplateSlotType.AquaBreather:
			case TemplateSlotType.Back:
				return GearSubSlotTypeType.Equipment;
			case TemplateSlotType.Amulet:
			case TemplateSlotType.Accessory_1:
			case TemplateSlotType.Accessory_2:
			case TemplateSlotType.Ring_1:
			case TemplateSlotType.Ring_2:
				return GearSubSlotTypeType.Equipment;
			case TemplateSlotType.MainHand:
			case TemplateSlotType.OffHand:
			case TemplateSlotType.Aquatic:
			case TemplateSlotType.AltMainHand:
			case TemplateSlotType.AltOffHand:
			case TemplateSlotType.AltAquatic:
				return GearSubSlotTypeType.Equipment;
			case TemplateSlotType.Nourishment:
				return GearSubSlotTypeType.Nourishment;
			case TemplateSlotType.Enhancement:
				return GearSubSlotTypeType.Enhancement;
			case TemplateSlotType.PowerCore:
				return GearSubSlotTypeType.PowerCore;
			case TemplateSlotType.PveRelic:
			case TemplateSlotType.PvpRelic:
				return GearSubSlotTypeType.Relic;
			case TemplateSlotType.None:
				return GearSubSlotTypeType.None;
			case TemplateSlotType.PvpAmulet:
				return GearSubSlotTypeType.PvpAmulet;
			default:
				return GearSubSlotTypeType.None;
			}
		}
	}
}
