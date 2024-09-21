using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Helpers
{
	public class LocalizationHelper
	{
		public static string TranslateDiscipline(string profession)
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
						return Professions.Tailor;
					case 's':
						if (!(text == "scribe"))
						{
							break;
						}
						return Professions.Scribe;
					}
					break;
				case 10:
					if (!(text == "armorsmith"))
					{
						break;
					}
					return Professions.Armorsmith;
				case 11:
					if (!(text == "weaponsmith"))
					{
						break;
					}
					return Professions.Weaponsmith;
				case 7:
					if (!(text == "jeweler"))
					{
						break;
					}
					return Professions.Jeweler;
				case 9:
					if (!(text == "artificer"))
					{
						break;
					}
					return Professions.Artificer;
				case 8:
					if (!(text == "huntsman"))
					{
						break;
					}
					return Professions.Huntsman;
				case 4:
					if (!(text == "chef"))
					{
						break;
					}
					return Professions.Chef;
				case 13:
					if (!(text == "leatherworker"))
					{
						break;
					}
					return Professions.Leatherworker;
				}
			}
			return string.Empty;
		}

		public static string TranslateDiscipline(Discipline profession)
		{
			return profession switch
			{
				Discipline.Armorsmith => Professions.Armorsmith, 
				Discipline.Weaponsmith => Professions.Weaponsmith, 
				Discipline.Jeweler => Professions.Jeweler, 
				Discipline.Artificer => Professions.Artificer, 
				Discipline.Huntsman => Professions.Huntsman, 
				Discipline.Chef => Professions.Chef, 
				Discipline.Leatherworker => Professions.Leatherworker, 
				Discipline.Tailor => Professions.Tailor, 
				Discipline.Scribe => Professions.Scribe, 
				_ => string.Empty, 
			};
		}

		public static string TranslateRecipeSource(string source)
		{
			return source.ToLower() switch
			{
				"automatic" => MysticCrafting.Module.Strings.Recipe.RecipeSourceAutomatic, 
				"recipesheet" => MysticCrafting.Module.Strings.Recipe.RecipeSourceRecipeSheet, 
				"other" => MysticCrafting.Module.Strings.Recipe.RecipeSourceOther, 
				_ => string.Empty, 
			};
		}

		public static string TranslateRarity(ItemRarity rarity)
		{
			return rarity switch
			{
				ItemRarity.Ascended => Rarities.Ascended, 
				ItemRarity.Basic => Rarities.Basic, 
				ItemRarity.Exotic => Rarities.Exotic, 
				ItemRarity.Fine => Rarities.Fine, 
				ItemRarity.Legendary => Rarities.Legendary, 
				ItemRarity.Masterwork => Rarities.Masterwork, 
				ItemRarity.Rare => Rarities.Rare, 
				_ => string.Empty, 
			};
		}

		public static string TranslateMenuItem(string menuItemName)
		{
			if (!string.IsNullOrWhiteSpace(MysticCrafting.Module.Strings.Menu.ResourceManager.GetString(menuItemName)))
			{
				return MysticCrafting.Module.Strings.Menu.ResourceManager.GetString(menuItemName);
			}
			return menuItemName;
		}
	}
}
