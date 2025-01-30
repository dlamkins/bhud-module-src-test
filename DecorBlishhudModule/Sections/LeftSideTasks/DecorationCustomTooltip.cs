using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using DecorBlishhudModule.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.Sections.LeftSideTasks
{
	public class DecorationCustomTooltip
	{
		private static readonly Dictionary<Decoration, Tooltip> decorationTooltips = new Dictionary<Decoration, Tooltip>();

		public static async Task<Tooltip> CreateTooltipWithIconsAsync(Decoration decoration, Texture2D mainIcon)
		{
			Texture2D icon1 = null;
			Texture2D icon2 = null;
			Texture2D icon3 = null;
			Texture2D icon4 = null;
			if (decoration.CraftingIngredientName1 != null)
			{
				icon1 = await LeftSideSection.GetOrCreateTextureAsync(decoration.CraftingIngredientName1, decoration.CraftingIngredientIcon1);
				if (decoration.CraftingIngredientName2 != null)
				{
					icon2 = await LeftSideSection.GetOrCreateTextureAsync(decoration.CraftingIngredientName2, decoration.CraftingIngredientIcon2);
					if (decoration.CraftingIngredientName3 != null)
					{
						icon3 = await LeftSideSection.GetOrCreateTextureAsync(decoration.CraftingIngredientName3, decoration.CraftingIngredientIcon3);
						if (decoration.CraftingIngredientName4 != null)
						{
							icon4 = await LeftSideSection.GetOrCreateTextureAsync(decoration.CraftingIngredientName4, decoration.CraftingIngredientIcon4);
						}
					}
				}
			}
			return await CustomTooltip(decoration, mainIcon, icon1, icon2, icon3, icon4);
		}

		public static async Task<Tooltip> CustomTooltip(Decoration decoration, Texture2D mainIcon, Texture2D icon1, Texture2D icon2, Texture2D icon3, Texture2D icon4)
		{
			if (decorationTooltips.TryGetValue(decoration, out var cachedTooltip))
			{
				return cachedTooltip;
			}
			Tooltip val = new Tooltip();
			((Control)val).set_Width(300);
			Tooltip customTooltip = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)customTooltip);
			val2.set_Texture(AsyncTexture2D.op_Implicit(mainIcon));
			((Control)val2).set_Size(new Point(40, 40));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)customTooltip);
			val3.set_Text(decoration.Name);
			val3.set_TextColor(Color.get_White());
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(50, 5));
			val3.set_AutoSizeWidth(true);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)customTooltip);
			val4.set_Text(decoration.Book);
			val4.set_TextColor(new Color(255, 164, 5));
			val4.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val4).set_Location(new Point(50, 22));
			val4.set_AutoSizeWidth(true);
			if (!string.IsNullOrEmpty(decoration.CraftingIngredientQty1))
			{
				AddIngredientSection(customTooltip, new(string, Texture2D, string)[4]
				{
					(decoration.CraftingIngredientQty1, icon1, decoration.CraftingIngredientName1),
					(decoration.CraftingIngredientQty2, icon2, decoration.CraftingIngredientName2),
					(decoration.CraftingIngredientQty3, icon3, decoration.CraftingIngredientName3),
					(decoration.CraftingIngredientQty4, icon4, decoration.CraftingIngredientName4)
				});
			}
			decorationTooltips[decoration] = customTooltip;
			return customTooltip;
		}

		private static void AddIngredientSection(Tooltip parent, IEnumerable<(string qty, Texture2D icon, string name)> ingredients)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			int yOffset = 50;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text("Recipe:");
			val.set_TextColor(Color.get_White());
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Location(new Point(5, yOffset));
			val.set_AutoSizeWidth(true);
			foreach (var item in ingredients.Where(((string qty, Texture2D icon, string name) i) => !string.IsNullOrEmpty(i.qty)))
			{
				string qty = item.qty;
				Texture2D icon = item.icon;
				string name = item.name;
				yOffset += 30;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)parent);
				val2.set_Text(qty);
				val2.set_TextColor(Color.get_White());
				val2.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val2).set_Location(new Point(5, yOffset));
				val2.set_AutoSizeWidth(true);
				Image val3 = new Image();
				((Control)val3).set_Parent((Container)(object)parent);
				val3.set_Texture(AsyncTexture2D.op_Implicit(icon));
				((Control)val3).set_Size(new Point(25, 25));
				((Control)val3).set_Location(new Point(35, yOffset));
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)parent);
				val4.set_Text(name);
				val4.set_TextColor(Color.get_White());
				val4.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val4).set_Location(new Point(70, yOffset));
				val4.set_AutoSizeWidth(true);
			}
		}
	}
}
