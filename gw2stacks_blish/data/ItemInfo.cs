using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;

namespace gw2stacks_blish.data
{
	internal class ItemInfo
	{
		public int Id;

		public string Name;

		public int IconId;

		public int Rarity;

		public string Description;

		public int Type;

		public bool isFoodOrUtility;

		public List<int> Flags;

		public int Level;

		public int VendorValue;

		public string chatLink;

		public ItemWeightType armorWeight;

		public ItemArmorSlotType armorType;

		public ItemWeaponType weaponType;

		public ItemTrinketType trinketType;

		public int defaultSkin;

		public IReadOnlyList<int> skinId;

		public int miniId;

		public int recipeId;

		public ItemInfo()
		{
			Id = 0;
			Name = null;
			IconId = 63369;
			Rarity = 0;
			Description = null;
			Type = 0;
			isFoodOrUtility = false;
			Flags = new List<int>();
			Level = 0;
			VendorValue = 0;
			chatLink = "";
			armorWeight = ItemWeightType.Unknown;
			armorType = ItemArmorSlotType.Unknown;
			weaponType = ItemWeaponType.Unknown;
			trinketType = ItemTrinketType.Unknown;
			defaultSkin = -1;
			skinId = new List<int> { -1 };
			miniId = -1;
			recipeId = -1;
		}
	}
}
