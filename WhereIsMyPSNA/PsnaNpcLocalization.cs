using System.Collections.Generic;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace WhereIsMyPSNA
{
	internal static class PsnaNpcLocalization
	{
		private class Entry
		{
			public string De;

			public string Es;

			public string Fr;
		}

		private static readonly Dictionary<string, Entry> NpcNames = new Dictionary<string, Entry>
		{
			["Mehem the Traveled"] = new Entry
			{
				De = "Mehem der Weitgereiste",
				Es = "Mehem el Viajero",
				Fr = "Mehem le Voyageur"
			},
			["The Fox"] = new Entry
			{
				De = "Der Fuchs",
				Es = "El Zorro",
				Fr = "Le renard"
			},
			["Specialist Yana"] = new Entry
			{
				De = "Spezialistin Yana",
				Es = "Especialista Yana",
				Fr = "Yana"
			},
			["Lady Derwena"] = new Entry
			{
				De = "Fürstin Derwena",
				Es = "Lady Derwena",
				Fr = "Dame Derwena"
			},
			["Despina Katelyn"] = new Entry
			{
				De = "Despina Katelyn",
				Es = "Despina Katelyn",
				Fr = "Katelyn"
			},
			["Verma Giftrender"] = new Entry
			{
				De = "Verma die Geschenkbotin",
				Es = "Verna Dejarregalos",
				Fr = "Verma Broyedon"
			}
		};

		public static string LocalizeNpc(string en)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected I4, but got Unknown
			if (en == null || !NpcNames.TryGetValue(en, out var entry))
			{
				return en;
			}
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			switch (value - 1)
			{
			case 1:
				if (!string.IsNullOrEmpty(entry.De))
				{
					return entry.De;
				}
				return en;
			case 0:
				if (!string.IsNullOrEmpty(entry.Es))
				{
					return entry.Es;
				}
				return en;
			case 2:
				if (!string.IsNullOrEmpty(entry.Fr))
				{
					return entry.Fr;
				}
				return en;
			default:
				return en;
			}
		}
	}
}
