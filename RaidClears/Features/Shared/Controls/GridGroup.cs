using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Enums;
using RaidClears.Utils;

namespace RaidClears.Features.Shared.Controls
{
	public class GridGroup : FlowPanel
	{
		public GridGroup(Container parent, SettingEntry<Layout> layout)
			: this()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 2f));
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((FlowPanel)(object)this).LayoutChange(layout, 1);
		}
	}
}
