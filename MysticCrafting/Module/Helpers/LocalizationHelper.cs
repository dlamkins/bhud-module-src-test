using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Helpers
{
	public class LocalizationHelper
	{
		public static string TranslateProfession(string profession)
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

		public static string TranslateRecipeSource(string source)
		{
			return source.ToLower() switch
			{
				"automatic" => MysticCrafting.Module.Strings.Recipe.RecipeSourceAutomatic, 
				"recipe sheet" => MysticCrafting.Module.Strings.Recipe.RecipeSourceRecipeSheet, 
				"other" => MysticCrafting.Module.Strings.Recipe.RecipeSourceOther, 
				_ => string.Empty, 
			};
		}

		public static string TranslateMenuItem(string menuItem)
		{
			return MysticCrafting.Module.Strings.Menu.ResourceManager.GetString(menuItem);
		}
	}
}
