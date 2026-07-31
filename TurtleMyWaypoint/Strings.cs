using Blish_HUD;

namespace TurtleMyWaypoint
{
	internal static class Strings
	{
		public static bool IsFrench => (int)GameService.Overlay.get_UserLocale().get_Value() == 3;

		public static string Copy
		{
			get
			{
				if (!IsFrench)
				{
					return "Copy";
				}
				return "Copier";
			}
		}

		public static string Part
		{
			get
			{
				if (!IsFrench)
				{
					return "Part";
				}
				return "Partie";
			}
		}

		public static string NoDataYet
		{
			get
			{
				if (!IsFrench)
				{
					return "Content coming soon";
				}
				return "Contenu à venir";
			}
		}

		public static string Season
		{
			get
			{
				if (!IsFrench)
				{
					return "Season";
				}
				return "Saison";
			}
		}

		public static string Tab_CoreTyria
		{
			get
			{
				if (!IsFrench)
				{
					return "Core Tyria";
				}
				return "Tyrie centrale";
			}
		}

		public static string Tab_HeartOfThorns => "Heart of Thorns";

		public static string Tab_PathOfFire => "Path of Fire";

		public static string Tab_EndOfDragons => "End of Dragons";

		public static string Tab_SecretsOfTheObscure => "Secrets of the Obscure";

		public static string Tab_JanthirWilds => "Janthir Wilds";

		public static string Tab_VisionsOfEternity => "Visions of Eternity";

		public static string Tab_LivingWorld
		{
			get
			{
				if (!IsFrench)
				{
					return "Living World";
				}
				return "Monde vivant";
			}
		}
	}
}
