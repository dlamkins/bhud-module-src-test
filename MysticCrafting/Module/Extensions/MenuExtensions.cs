using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Menu;

namespace MysticCrafting.Module.Extensions
{
	public static class MenuExtensions
	{
		public static IEnumerable<FilterMenuItem> Items(this Menu menu)
		{
			return ((IEnumerable)((Container)menu).get_Children()).OfType<FilterMenuItem>().ToList();
		}

		public static EnumFilterMenuItem<ItemRarity> AddRarity(this Menu menu, ItemRarity rarity)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			EnumFilterMenuItem<ItemRarity> enumFilterMenuItem = new EnumFilterMenuItem<ItemRarity>(rarity, LocalizationHelper.TranslateRarity(rarity));
			((Control)enumFilterMenuItem).set_Parent((Container)(object)menu);
			((MenuItem)enumFilterMenuItem).set_CanCheck(true);
			enumFilterMenuItem.TextColor = ColorHelper.FromRarity(rarity.ToString());
			((MenuItem)enumFilterMenuItem).set_Checked(true);
			return enumFilterMenuItem;
		}

		public static FilterMenuItem AddDiscipline(this Menu menu, Discipline discipline)
		{
			EnumFilterMenuItem<Discipline> enumFilterMenuItem = new EnumFilterMenuItem<Discipline>(discipline, LocalizationHelper.TranslateDiscipline(discipline));
			((Control)enumFilterMenuItem).set_Parent((Container)(object)menu);
			((MenuItem)enumFilterMenuItem).set_CanCheck(true);
			((MenuItem)enumFilterMenuItem).set_Icon(IconHelper.GetIcon(discipline));
			((MenuItem)enumFilterMenuItem).set_Checked(true);
			return enumFilterMenuItem;
		}

		public static FilterMenuItem AddWeightClass(this Menu menu, WeightClass weightClass)
		{
			EnumFilterMenuItem<WeightClass> enumFilterMenuItem = new EnumFilterMenuItem<WeightClass>(weightClass, LocalizationHelper.TranslateWeightClass(weightClass));
			((Control)enumFilterMenuItem).set_Parent((Container)(object)menu);
			((MenuItem)enumFilterMenuItem).set_CanCheck(true);
			((MenuItem)enumFilterMenuItem).set_Checked(true);
			return enumFilterMenuItem;
		}

		public static FilterMenuItem AddCustomMenuItem(this Menu menu, string text, AsyncTexture2D icon)
		{
			FilterMenuItem filterMenuItem = new FilterMenuItem(text);
			((Control)filterMenuItem).set_Parent((Container)(object)menu);
			((MenuItem)filterMenuItem).set_CanCheck(true);
			((MenuItem)filterMenuItem).set_Icon(icon);
			((MenuItem)filterMenuItem).set_Checked(true);
			return filterMenuItem;
		}
	}
}
