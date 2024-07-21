using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class CraftingDisciplinesTooltipView : View, ITooltipView, IView
	{
		public IList<CurrencyQuantity> Quantities { get; set; }

		private IList<Control> Controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public CraftingDisciplinesTooltipView(IList<CraftingDisciplineRequirement> disciplines)
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			BuildControls(disciplines);
		}

		public void BuildControls(IList<CraftingDisciplineRequirement> disciplines)
		{
			foreach (Control control in Controls)
			{
				control.Dispose();
			}
			int paddingTop = 0;
			foreach (CraftingDisciplineRequirement discipline in disciplines)
			{
				BuildControls(discipline, ref paddingTop);
			}
		}

		private void BuildControls(CraftingDisciplineRequirement requirement, ref int paddingTop)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			AsyncTexture2D icon = IconHelper.GetIconColor(requirement.DisciplineName);
			int paddingLeft = 0;
			if (icon != null)
			{
				IList<Control> controls = Controls;
				Image val = new Image(icon);
				((Control)val).set_Size(IconSize);
				((Control)val).set_Location(new Point(0, paddingTop));
				controls.Add((Control)val);
				paddingLeft += 30;
			}
			paddingTop += 5;
			Label val2 = new Label();
			val2.set_Text($"{requirement.RequiredLevel}");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Location(new Point(paddingLeft, paddingTop));
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			Label levelLabel = val2;
			Controls.Add((Control)(object)levelLabel);
			Label val3 = new Label();
			val3.set_Text(requirement.DisciplineName);
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(((Control)levelLabel).get_Right() + 5, paddingTop));
			val3.set_StrokeText(true);
			val3.set_AutoSizeWidth(true);
			Label nameLabel = val3;
			Controls.Add((Control)(object)nameLabel);
			paddingTop += 35;
		}

		protected override void Build(Container buildPanel)
		{
			foreach (Control control in Controls)
			{
				control.set_Parent(buildPanel);
			}
		}
	}
}
