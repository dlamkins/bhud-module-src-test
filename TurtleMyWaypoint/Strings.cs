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

		public static string SearchPlaceholder
		{
			get
			{
				if (!IsFrench)
				{
					return "Search for a waypoint...";
				}
				return "Rechercher un point de passage...";
			}
		}

		public static string SearchHint
		{
			get
			{
				if (!IsFrench)
				{
					return "Type a waypoint name to find it";
				}
				return "Tapez le nom d'un point de passage pour le retrouver";
			}
		}

		public static string NoResults
		{
			get
			{
				if (!IsFrench)
				{
					return "No results";
				}
				return "Aucun résultat";
			}
		}

		public static string SearchResults
		{
			get
			{
				if (!IsFrench)
				{
					return "Results";
				}
				return "Résultats";
			}
		}

		public static string Tab_Search
		{
			get
			{
				if (!IsFrench)
				{
					return "Search";
				}
				return "Recherche";
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
