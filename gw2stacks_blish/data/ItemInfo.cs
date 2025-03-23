using System.Collections.Generic;

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
		}
	}
}
