using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class GearTemplateSlotExtension
	{
		public static bool IsArmor(this TemplateSlotType slot)
		{
			if ((uint)slot <= 6u)
			{
				return true;
			}
			return false;
		}

		public static bool IsWeapon(this TemplateSlotType slot)
		{
			if ((uint)(slot - 7) <= 5u)
			{
				return true;
			}
			return false;
		}

		public static bool IsAquatic(this TemplateSlotType slot)
		{
			if (slot == TemplateSlotType.AquaBreather || slot == TemplateSlotType.Aquatic || slot == TemplateSlotType.AltAquatic)
			{
				return true;
			}
			return false;
		}

		public static bool IsOffhand(this TemplateSlotType slot)
		{
			if (slot == TemplateSlotType.OffHand || slot == TemplateSlotType.AltOffHand)
			{
				return true;
			}
			return false;
		}

		public static TemplateSlotType? GetOffhand(this TemplateSlotType slot)
		{
			return slot switch
			{
				TemplateSlotType.MainHand => TemplateSlotType.OffHand, 
				TemplateSlotType.AltMainHand => TemplateSlotType.AltOffHand, 
				_ => null, 
			};
		}

		public static TemplateSlotType? GetMainHand(this TemplateSlotType slot)
		{
			return slot switch
			{
				TemplateSlotType.OffHand => TemplateSlotType.MainHand, 
				TemplateSlotType.AltOffHand => TemplateSlotType.AltMainHand, 
				_ => null, 
			};
		}

		public static bool IsJewellery(this TemplateSlotType slot)
		{
			if ((uint)(slot - 13) <= 5u)
			{
				return true;
			}
			return false;
		}
	}
}
