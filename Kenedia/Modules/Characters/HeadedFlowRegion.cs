using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters
{
	public class HeadedFlowRegion : HeaderUnderlined
	{
		public FlowPanel contentFlowPanel;

		public HeadedFlowRegion()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Location(new Point(0, ((Control)Separator_Image).get_Location().Y + ((Control)Separator_Image).get_Height() + base.VerticalPadding));
			val.set_OuterControlPadding(new Vector2(base.HorizontalPadding, base.VerticalPadding));
			contentFlowPanel = val;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)Separator_Image).set_Size(new Point(((Control)this).get_Width(), 4));
				((Control)Separator_Image).set_Location(new Point(0, ((Control)textLabel).get_Location().Y + ((Control)textLabel).get_Height() + base.VerticalPadding));
				((Control)contentFlowPanel).set_Location(new Point(0, ((Control)Separator_Image).get_Location().Y + ((Control)Separator_Image).get_Height() + base.VerticalPadding));
			});
		}
	}
}
