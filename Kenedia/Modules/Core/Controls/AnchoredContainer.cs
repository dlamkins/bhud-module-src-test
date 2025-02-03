using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
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

		public CaptureType? CaptureInput { get; set; }

		public Control Anchor
		{
			get
			{
				return _anchor;
			}
			set
			{
				Common.SetProperty(ref _anchor, value, new ValueChangedEventHandler<Control>(OnAnchorChanged));
			}
		}

		public AnchorPos AnchorPosition { get; set; }

		public RectangleDimensions RelativePosition { get; set; } = new RectangleDimensions(0);


		private void OnAnchorChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Control> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.Moved -= Anchor_Moved;
				e.OldValue!.Resized -= Anchor_Moved;
			}
			if (e.NewValue != null)
			{
				e.NewValue!.Moved += Anchor_Moved;
				e.NewValue!.Resized += Anchor_Moved;
			}
		}

		private void Anchor_Moved(object sender, EventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.Location = GetPosition();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			if (Anchor != null)
			{
				base.Location = GetPosition();
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
			Rectangle anchorBounds = Anchor.AbsoluteBounds;
			switch (AnchorPosition)
			{
			case AnchorPos.Left:
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() - base.Width + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
			case AnchorPos.Top:
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() - base.Height + RelativePosition.Top);
			case AnchorPos.Right:
				return new Point(((Rectangle)(ref anchorBounds)).get_Right() + RelativePosition.Right, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
			case AnchorPos.Bottom:
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Bottom() + RelativePosition.Bottom);
			case AnchorPos.AutoHorizontal:
				if (((Rectangle)(ref anchorBounds)).get_Left() + anchorBounds.Width / 2 <= GameService.Graphics.SpriteScreen.Right / 2)
				{
					return new Point(((Rectangle)(ref anchorBounds)).get_Right() + RelativePosition.Right, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
				}
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() - base.Width + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() + RelativePosition.Top);
			case AnchorPos.AutoVertical:
				if (((Rectangle)(ref anchorBounds)).get_Top() + anchorBounds.Height / 2 <= GameService.Graphics.SpriteScreen.Bottom / 2)
				{
					return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Bottom() + RelativePosition.Bottom);
				}
				return new Point(((Rectangle)(ref anchorBounds)).get_Left() + RelativePosition.Left, ((Rectangle)(ref anchorBounds)).get_Top() - base.Height + RelativePosition.Top);
			case AnchorPos.None:
				return base.Location;
			default:
				return base.Location;
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
		}

		protected override CaptureType CapturesInput()
		{
			return CaptureInput ?? base.CapturesInput();
		}
	}
}
