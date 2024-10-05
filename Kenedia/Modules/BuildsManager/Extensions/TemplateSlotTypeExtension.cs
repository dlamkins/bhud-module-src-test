using System;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class TemplateSlotTypeExtension
	{
		public static TemplateSlotType[] GetGroup(this TemplateSlotType slot)
		{
			switch (slot)
			{
			case TemplateSlotType.Head:
			case TemplateSlotType.Shoulder:
			case TemplateSlotType.Chest:
			case TemplateSlotType.Hand:
			case TemplateSlotType.Leg:
			case TemplateSlotType.Foot:
			case TemplateSlotType.AquaBreather:
				return new TemplateSlotType[7]
				{
					TemplateSlotType.Head,
					TemplateSlotType.Shoulder,
					TemplateSlotType.Chest,
					TemplateSlotType.Hand,
					TemplateSlotType.Leg,
					TemplateSlotType.Foot,
					TemplateSlotType.AquaBreather
				};
			case TemplateSlotType.MainHand:
			case TemplateSlotType.OffHand:
			case TemplateSlotType.Aquatic:
			case TemplateSlotType.AltMainHand:
			case TemplateSlotType.AltOffHand:
			case TemplateSlotType.AltAquatic:
				return new TemplateSlotType[6]
				{
					TemplateSlotType.MainHand,
					TemplateSlotType.AltMainHand,
					TemplateSlotType.OffHand,
					TemplateSlotType.AltOffHand,
					TemplateSlotType.Aquatic,
					TemplateSlotType.AltAquatic
				};
			case TemplateSlotType.Back:
			case TemplateSlotType.Amulet:
			case TemplateSlotType.Accessory_1:
			case TemplateSlotType.Accessory_2:
			case TemplateSlotType.Ring_1:
			case TemplateSlotType.Ring_2:
				return new TemplateSlotType[6]
				{
					TemplateSlotType.Back,
					TemplateSlotType.Amulet,
					TemplateSlotType.Accessory_1,
					TemplateSlotType.Accessory_2,
					TemplateSlotType.Ring_1,
					TemplateSlotType.Ring_2
				};
			default:
				return Array.Empty<TemplateSlotType>();
			}
		}

		public static TemplateSlotType[] GetWeaponGroup(this TemplateSlotType slot)
		{
			switch (slot)
			{
			case TemplateSlotType.Aquatic:
			case TemplateSlotType.AltAquatic:
				return new TemplateSlotType[2]
				{
					TemplateSlotType.Aquatic,
					TemplateSlotType.AltAquatic
				};
			case TemplateSlotType.MainHand:
			case TemplateSlotType.OffHand:
			case TemplateSlotType.AltMainHand:
			case TemplateSlotType.AltOffHand:
				return new TemplateSlotType[4]
				{
					TemplateSlotType.MainHand,
					TemplateSlotType.AltMainHand,
					TemplateSlotType.OffHand,
					TemplateSlotType.AltOffHand
				};
			default:
			{
				_003CPrivateImplementationDetails_003E.ThrowSwitchExpressionException(slot);
				TemplateSlotType[] result = default(TemplateSlotType[]);
				return result;
			}
			}
		}

		public static TemplateSlotType[] GetSigilGroup(this TemplateSubSlotType subSlot, bool reset = false)
		{
			if (reset)
			{
				return new TemplateSlotType[6]
				{
					TemplateSlotType.MainHand,
					TemplateSlotType.AltMainHand,
					TemplateSlotType.Aquatic,
					TemplateSlotType.AltAquatic,
					TemplateSlotType.OffHand,
					TemplateSlotType.AltOffHand
				};
			}
			return subSlot switch
			{
				TemplateSubSlotType.Sigil1 => new TemplateSlotType[4]
				{
					TemplateSlotType.MainHand,
					TemplateSlotType.AltMainHand,
					TemplateSlotType.Aquatic,
					TemplateSlotType.AltAquatic
				}, 
				TemplateSubSlotType.Sigil2 => new TemplateSlotType[4]
				{
					TemplateSlotType.OffHand,
					TemplateSlotType.AltOffHand,
					TemplateSlotType.Aquatic,
					TemplateSlotType.AltAquatic
				}, 
				_ => Array.Empty<TemplateSlotType>(), 
			};
		}
	}
}
