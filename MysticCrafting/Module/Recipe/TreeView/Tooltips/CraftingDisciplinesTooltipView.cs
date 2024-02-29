using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Recipe.TreeView.Controls;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class CraftingDisciplinesTooltipView : View, ITooltipView, IView
	{
		public IList<MysticCurrencyQuantity> Quantities { get; set; }

		private IList<Control> Controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public CraftingDisciplinesTooltipView(IList<CraftingDisciplineRequirement> disciplines)
		{
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
			AsyncTexture2D icon = IconHelper.GetIconColor(requirement.DisciplineName);
			int paddingLeft = 0;
			if (icon != null)
			{
				Controls.Add(new Image(icon)
				{
					Size = IconSize,
					Location = new Point(0, paddingTop)
				});
				paddingLeft += 30;
			}
			paddingTop += 5;
			Label levelLabel = new Label
			{
				Text = $"{requirement.RequiredLevel}",
				Font = GameService.Content.DefaultFont16,
				Location = new Point(paddingLeft, paddingTop),
				StrokeText = true,
				AutoSizeWidth = true
			};
			Controls.Add(levelLabel);
			Label nameLabel = new Label
			{
				Text = requirement.DisciplineName,
				Font = GameService.Content.DefaultFont16,
				Location = new Point(levelLabel.Right + 5, paddingTop),
				StrokeText = true,
				AutoSizeWidth = true
			};
			Controls.Add(nameLabel);
			paddingTop += 35;
		}

		protected override void Build(Container buildPanel)
		{
			foreach (Control control in Controls)
			{
				control.Parent = buildPanel;
			}
		}
	}
}
