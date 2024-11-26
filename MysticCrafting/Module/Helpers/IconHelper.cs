using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Content;

namespace MysticCrafting.Module.Helpers
{
	public static class IconHelper
	{
		public static readonly AsyncTexture2D ArmorSmithing = AsyncTexture2D.FromAssetId(156615);

		public static readonly AsyncTexture2D WeaponSmithing = AsyncTexture2D.FromAssetId(156622);

		public static readonly AsyncTexture2D Jewelery = AsyncTexture2D.FromAssetId(156619);

		public static readonly AsyncTexture2D Artificer = AsyncTexture2D.FromAssetId(156616);

		public static readonly AsyncTexture2D Huntsman = AsyncTexture2D.FromAssetId(156618);

		public static readonly AsyncTexture2D Cooking = AsyncTexture2D.FromAssetId(156617);

		public static readonly AsyncTexture2D Leatherworking = AsyncTexture2D.FromAssetId(156620);

		public static readonly AsyncTexture2D Tailoring = AsyncTexture2D.FromAssetId(156621);

		public static readonly AsyncTexture2D Scribing = AsyncTexture2D.FromAssetId(1228680);

		public static readonly AsyncTexture2D Heavy = AsyncTexture2D.FromAssetId(156715);

		public static readonly AsyncTexture2D Medium = AsyncTexture2D.FromAssetId(156620);

		public static readonly AsyncTexture2D Light = AsyncTexture2D.FromAssetId(1293677);

		private static readonly AsyncTexture2D ArmorSmithingColor = AsyncTexture2D.FromAssetId(102461);

		private static readonly AsyncTexture2D WeaponSmithingColor = AsyncTexture2D.FromAssetId(102460);

		private static readonly AsyncTexture2D JeweleryColor = AsyncTexture2D.FromAssetId(102458);

		private static readonly AsyncTexture2D ArtificerColor = AsyncTexture2D.FromAssetId(102463);

		private static readonly AsyncTexture2D HuntsmanColor = AsyncTexture2D.FromAssetId(102462);

		private static readonly AsyncTexture2D CookingColor = AsyncTexture2D.FromAssetId(102465);

		private static readonly AsyncTexture2D LeatherworkingColor = AsyncTexture2D.FromAssetId(102464);

		private static readonly AsyncTexture2D TailoringColor = AsyncTexture2D.FromAssetId(102459);

		private static readonly AsyncTexture2D ScribingColor = AsyncTexture2D.FromAssetId(1228680);

		public static AsyncTexture2D GetIcon(string profession)
		{
			string text = profession.ToLower();
			if (text != null)
			{
				switch (text.Length)
				{
				case 6:
					switch (text[0])
					{
					case 't':
						if (!(text == "tailor"))
						{
							break;
						}
						return Tailoring;
					case 's':
						if (!(text == "scribe"))
						{
							break;
						}
						return Scribing;
					}
					break;
				case 10:
					if (!(text == "armorsmith"))
					{
						break;
					}
					return ArmorSmithing;
				case 11:
					if (!(text == "weaponsmith"))
					{
						break;
					}
					return WeaponSmithing;
				case 7:
					if (!(text == "jeweler"))
					{
						break;
					}
					return Jewelery;
				case 9:
					if (!(text == "artificer"))
					{
						break;
					}
					return Artificer;
				case 8:
					if (!(text == "huntsman"))
					{
						break;
					}
					return Huntsman;
				case 4:
					if (!(text == "chef"))
					{
						break;
					}
					return Cooking;
				case 13:
					if (!(text == "leatherworker"))
					{
						break;
					}
					return Leatherworking;
				}
			}
			return ArmorSmithing;
		}

		public static AsyncTexture2D GetIcon(Discipline profession)
		{
			return (AsyncTexture2D)(profession switch
			{
				Discipline.Armorsmith => ArmorSmithing, 
				Discipline.Weaponsmith => WeaponSmithing, 
				Discipline.Jeweler => Jewelery, 
				Discipline.Artificer => Artificer, 
				Discipline.Huntsman => Huntsman, 
				Discipline.Chef => Cooking, 
				Discipline.Leatherworker => Leatherworking, 
				Discipline.Tailor => Tailoring, 
				Discipline.Scribe => Scribing, 
				_ => ArmorSmithing, 
			});
		}

		public static AsyncTexture2D GetIcon(WeightClass @class)
		{
			return (AsyncTexture2D)(@class switch
			{
				WeightClass.Heavy => Heavy, 
				WeightClass.Medium => Medium, 
				WeightClass.Light => Light, 
				_ => null, 
			});
		}

		public static AsyncTexture2D GetIconColor(string profession)
		{
			string text = profession.ToLower();
			if (text != null)
			{
				switch (text.Length)
				{
				case 6:
					switch (text[0])
					{
					case 't':
						if (!(text == "tailor"))
						{
							break;
						}
						return TailoringColor;
					case 's':
						if (!(text == "scribe"))
						{
							break;
						}
						return ScribingColor;
					}
					break;
				case 10:
					if (!(text == "armorsmith"))
					{
						break;
					}
					return ArmorSmithingColor;
				case 11:
					if (!(text == "weaponsmith"))
					{
						break;
					}
					return WeaponSmithingColor;
				case 7:
					if (!(text == "jeweler"))
					{
						break;
					}
					return JeweleryColor;
				case 9:
					if (!(text == "artificer"))
					{
						break;
					}
					return ArtificerColor;
				case 8:
					if (!(text == "huntsman"))
					{
						break;
					}
					return HuntsmanColor;
				case 4:
					if (!(text == "chef"))
					{
						break;
					}
					return CookingColor;
				case 13:
					if (!(text == "leatherworker"))
					{
						break;
					}
					return LeatherworkingColor;
				}
			}
			return ArmorSmithingColor;
		}
	}
}
