using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
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
			: base(parent)
		{
			base.PanelHeight = 30;
			((Control)this).set_Parent(parent);
			UpdateCheckbox();
		}

		public void UpdateCheckbox()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			Checkbox checkbox = Checkbox;
			if (checkbox != null)
			{
				((Control)checkbox).Dispose();
			}
			Checkbox val = new Checkbox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(10, 10));
			val.set_Text("Show base materials");
			((Control)val).set_Width(200);
			((Control)val).set_Height(20);
			Checkbox = val;
		}
	}
}
