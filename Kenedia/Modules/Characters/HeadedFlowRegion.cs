using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters
{
	public class HeadedFlowRegion : HeaderUnderlined
	{
		public FlowPanel contentFlowPanel;

		public HeadedFlowRegion()
		{
			contentFlowPanel = new FlowPanel
			{
				Parent = this,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				Location = new Point(0, Separator_Image.Location.Y + Separator_Image.Height + base.VerticalPadding),
				OuterControlPadding = new Vector2(base.HorizontalPadding, base.VerticalPadding)
			};
			base.Resized += delegate
			{
				Separator_Image.Size = new Point(base.Width, 4);
				Separator_Image.Location = new Point(0, textLabel.Location.Y + textLabel.Height + base.VerticalPadding);
				contentFlowPanel.Location = new Point(0, Separator_Image.Location.Y + Separator_Image.Height + base.VerticalPadding);
			};
		}
	}
}
