using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class RecipeCheckboxNode : TreeNodeBase
	{
		public Checkbox Checkbox { get; set; }

		public override string PathName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public RecipeCheckboxNode(Container parent)
		{
			base.PanelHeight = 30;
			base.Parent = parent;
			UpdateCheckbox();
		}

		public void UpdateCheckbox()
		{
			Checkbox?.Dispose();
			Checkbox = new Checkbox
			{
				Parent = this,
				Location = new Point(10, 10),
				Text = "Show base materials",
				Width = 200,
				Height = 20
			};
		}
	}
}
