using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class RecipeSourceTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

		private RecipeSource RecipeSource { get; set; }

		public Container BuildPanel { get; set; }

		public RecipeSourceTooltipView(RecipeSource recipeSource)
			: this()
		{
			RecipeSource = recipeSource;
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			if (!Initialized)
			{
				Initialize();
			}
		}

		public virtual void InitializeMysticForge()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			AsyncTexture2D icon = ServiceContainer.TextureRepository.Textures.MysticForgeIcon;
			List<Control> controls = _controls;
			Image val = new Image(icon);
			((Control)val).set_Parent(BuildPanel);
			((Control)val).set_Size(new Point(35, 35));
			((Control)val).set_Location(new Point(0, 0));
			controls.Add((Control)val);
			List<Control> controls2 = _controls;
			Label val2 = new Label();
			((Control)val2).set_Parent(BuildPanel);
			val2.set_Text(MysticCrafting.Module.Strings.Recipe.MysticForge);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Location(new Point(35, 7));
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			controls2.Add((Control)val2);
		}

		public virtual void InitializeCrafting()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Expected O, but got Unknown
			int yPos = 0;
			int xPos = 0;
			foreach (Discipline item in RecipeSource.Recipe.Disciplines ?? new List<Discipline>())
			{
				AsyncTexture2D icon = IconHelper.GetIconColor(item);
				List<Control> controls = _controls;
				Image val = new Image(icon);
				((Control)val).set_Parent(BuildPanel);
				((Control)val).set_Size(new Point(30, 30));
				((Control)val).set_Location(new Point(xPos, yPos));
				controls.Add((Control)val);
				xPos += 25;
			}
			if (RecipeSource.Recipe.DisciplineCount == 1)
			{
				string disciplineLabel = LocalizationHelper.TranslateProfession(RecipeSource.Recipe.Disciplines.FirstOrDefault());
				List<Control> controls2 = _controls;
				Label val2 = new Label();
				((Control)val2).set_Parent(BuildPanel);
				val2.set_Text(disciplineLabel);
				val2.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val2).set_Location(new Point(xPos + 5, yPos + 5));
				val2.set_StrokeText(true);
				val2.set_AutoSizeWidth(true);
				controls2.Add((Control)val2);
			}
			if (RecipeSource.Recipe.DisciplineCount > 0)
			{
				yPos += 30;
			}
			List<Control> controls3 = _controls;
			Label val3 = new Label();
			((Control)val3).set_Parent(BuildPanel);
			val3.set_Text(MysticCrafting.Module.Strings.Recipe.RecipeSource + ": " + LocalizationHelper.TranslateRecipeSource(RecipeSource.DisplayName));
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(5, yPos));
			val3.set_StrokeText(true);
			val3.set_AutoSizeWidth(true);
			controls3.Add((Control)val3);
			yPos += 25;
			if (RecipeSource.Recipe.RequiredRating != 0)
			{
				Label val4 = new Label();
				((Control)val4).set_Parent(BuildPanel);
				val4.set_Text($"{MysticCrafting.Module.Strings.Recipe.RecipeRequiredLevel}: {RecipeSource.Recipe.RequiredRating}");
				val4.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val4).set_Location(new Point(5, yPos));
				val4.set_StrokeText(true);
				val4.set_AutoSizeWidth(true);
				Label ratingLabel = val4;
				_controls.Add((Control)(object)ratingLabel);
			}
		}

		public virtual void Initialize()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			if (RecipeSource.Recipe.IsMysticForgeRecipe)
			{
				InitializeMysticForge();
			}
			else
			{
				InitializeCrafting();
			}
			int yPosition = (_controls.Any() ? (_controls.Max((Control c) => c.get_Bottom()) + 15) : 15);
			List<Control> controls = _controls;
			Label val = new Label();
			((Control)val).set_Parent(BuildPanel);
			val.set_Text(MysticCrafting.Module.Strings.Recipe.Ingredients);
			val.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val).set_Location(new Point(5, yPosition));
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			val.set_AutoSizeWidth(true);
			controls.Add((Control)val);
			yPosition += 25;
			foreach (Ingredient ingredient in RecipeSource.Recipe.Ingredients)
			{
				if (ingredient.Item != null)
				{
					List<Control> controls2 = _controls;
					Image val2 = new Image(ServiceContainer.TextureRepository.GetTexture(ingredient.Item.Icon));
					((Control)val2).set_Parent(BuildPanel);
					((Control)val2).set_Size(new Point(20, 20));
					((Control)val2).set_Location(new Point(5, yPosition));
					controls2.Add((Control)val2);
				}
				Label val3 = new Label();
				((Control)val3).set_Parent(BuildPanel);
				val3.set_Text(ingredient.Quantity.ToString());
				val3.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val3).set_Location(new Point(30, yPosition));
				val3.set_StrokeText(true);
				val3.set_AutoSizeWidth(true);
				Label countLabel = val3;
				_controls.Add((Control)(object)countLabel);
				Label val4 = new Label();
				((Control)val4).set_Parent(BuildPanel);
				val4.set_Text((ingredient.Item != null) ? ingredient.Item.LocalizedName() : string.Empty);
				val4.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val4).set_Location(new Point(((Control)countLabel).get_Right() + 5, yPosition));
				val4.set_StrokeText(true);
				val4.set_AutoSizeWidth(true);
				Label itemNameLabel = val4;
				if (ingredient.Item != null)
				{
					itemNameLabel.set_TextColor(ColorHelper.FromRarity(ingredient.Item.Rarity.ToString()));
				}
				_controls.Add((Control)(object)itemNameLabel);
				yPosition += 25;
			}
		}

		protected override void Unload()
		{
			_controls?.SafeDispose();
			_controls?.Clear();
			BuildPanel = null;
			((View<IPresenter>)this).Unload();
		}
	}
}
