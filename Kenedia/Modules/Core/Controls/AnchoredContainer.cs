using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Controls
{
	public class AnchoredContainer : FramedContainer
	{
		public enum AnchorPos
		{
			None,
			Left,
			Top,
			Right,
			Bottom,
			AutoHorizontal,
			AutoVertical
		}

		private Control _anchor;

		public Control Anchor
		{
			get
			{
				return _anchor;
			}
			set
			{
				if (_anchor != value)
				{
					if (_anchor != null)
					{
						_anchor.remove_Moved((EventHandler<MovedEventArgs>)Anchor_Moved);
						_anchor.remove_Resized((EventHandler<ResizedEventArgs>)Anchor_Moved);
					}
					_anchor = value;
					_anchor.add_Resized((EventHandler<ResizedEventArgs>)Anchor_Moved);
					_anchor.add_Moved((EventHandler<MovedEventArgs>)Anchor_Moved);
				}
			}
		}

		public AnchorPos AnchorPosition { get; set; }

		public RectangleDimensions RelativePosition { get; set; } = new RectangleDimensions(0);


		private void Anchor_Moved(object sender, EventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(GetPosition());
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			if (Anchor != null)
			{
				((Control)this).set_Location(GetPosition());
			}
		}

		private Point GetPosition()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			Rectangle anchorBounds = Anchor.get_AbsoluteBounds();
			switch (AnchorPosition)
			{
			case AnchorPos.Left:
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() - ((Control)this).get_Width() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
			case AnchorPos.Top:
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() - ((Control)this).get_Height() + RelativePosition.Top);
			case AnchorPos.Right:
				return new Point(((Rectangle)(ref anchorBounds)).get_Right() + RelativePosition.Right, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
			case AnchorPos.Bottom:
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Bottom() + RelativePosition.Bottom);
			case AnchorPos.AutoHorizontal:
				if (((Rectangle)(ref anchorBounds)).get_Left() + anchorBounds.Width / 2 <= ((Control)GameService.Graphics.get_SpriteScreen()).get_Right() / 2)
				{
					return new Point(((Rectangle)(ref anchorBounds)).get_Right() + RelativePosition.Right, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
				}
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() - ((Control)this).get_Width() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
			case AnchorPos.AutoVertical:
				if (((Rectangle)(ref anchorBounds)).get_Top() + anchorBounds.Height / 2 <= ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() / 2)
				{
					return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Bottom() + RelativePosition.Bottom);
				}
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() - ((Control)this).get_Height() + RelativePosition.Top);
			case AnchorPos.None:
				return ((Control)this).get_Location();
			default:
				return ((Control)this).get_Location();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
		}
	}
}
