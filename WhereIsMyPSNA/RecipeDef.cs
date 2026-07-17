using Blish_HUD;
using Gw2Sharp.WebApi;

namespace WhereIsMyPSNA
{
	internal class RecipeDef
	{
		public int[] RecipeSheetIds { get; set; }

		public int[] CraftingRecipeIds { get; set; }

		public string NameEn { get; set; }

		public string NameDe { get; set; }

		public string NameEs { get; set; }

		public string NameFr { get; set; }

		public string SheetNameEn { get; set; }

		public string SheetNameDe { get; set; }

		public string SheetNameEs { get; set; }

		public string SheetNameFr { get; set; }

		public string DescriptionEn { get; set; }

		public string DescriptionDe { get; set; }

		public string DescriptionEs { get; set; }

		public string DescriptionFr { get; set; }

		public int IconId { get; set; }

		public int SheetIconId { get; set; }

		public string Rarity { get; set; }

		public int DurationSecs { get; set; }

		public string Binding { get; set; }

		public int VendorValue { get; set; }

		public string Name => Localize(NameEn, NameDe, NameEs, NameFr);

		public string SheetName => Localize(SheetNameEn, SheetNameDe, SheetNameEs, SheetNameFr);

		public string Description => Localize(DescriptionEn, DescriptionDe, DescriptionEs, DescriptionFr);

		private static string Localize(string en, string de, string es, string fr)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected I4, but got Unknown
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			switch (value - 1)
			{
			case 1:
				if (!string.IsNullOrEmpty(de))
				{
					return de;
				}
				return en;
			case 0:
				if (!string.IsNullOrEmpty(es))
				{
					return es;
				}
				return en;
			case 2:
				if (!string.IsNullOrEmpty(fr))
				{
					return fr;
				}
				return en;
			default:
				return en;
			}
		}
	}
}
