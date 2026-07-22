using System;
using System.Runtime.CompilerServices;
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

		public CaptureType? CaptureInput { get; set; }

		public Control Anchor
		{
			[CompilerGenerated]
			get
			{
				return _003CAnchor_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CAnchor_003Ek__BackingField, value, delegate(Control v)
				{
					_003CAnchor_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Control>(OnAnchorChanged));
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
			base.Location = GetPosition();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			if (Anchor != null)
			{
				base.Location = GetPosition();
			}
		}

		private Point GetPosition()
		{
			Rectangle anchorBounds = Anchor.AbsoluteBounds;
			switch (AnchorPosition)
			{
			case AnchorPos.Left:
				return new Point(anchorBounds.Left - base.Width + RelativePosition.Left, anchorBounds.Top + RelativePosition.Top);
			case AnchorPos.Top:
				return new Point(anchorBounds.Left + RelativePosition.Left, anchorBounds.Top - base.Height + RelativePosition.Top);
			case AnchorPos.Right:
				return new Point(anchorBounds.Right + RelativePosition.Right, anchorBounds.Top + RelativePosition.Top);
			case AnchorPos.Bottom:
				return new Point(anchorBounds.Left + RelativePosition.Left, anchorBounds.Bottom + RelativePosition.Bottom);
			case AnchorPos.AutoHorizontal:
				if (anchorBounds.Left + anchorBounds.Width / 2 <= GameService.Graphics.SpriteScreen.Right / 2)
				{
					return new Point(anchorBounds.Right + RelativePosition.Right, anchorBounds.Top + RelativePosition.Top);
				}
				return new Point(anchorBounds.Left - base.Width + RelativePosition.Left, anchorBounds.Top + RelativePosition.Top);
			case AnchorPos.AutoVertical:
				if (anchorBounds.Top + anchorBounds.Height / 2 <= GameService.Graphics.SpriteScreen.Bottom / 2)
				{
					return new Point(anchorBounds.Left + RelativePosition.Left, anchorBounds.Bottom + RelativePosition.Bottom);
				}
				return new Point(anchorBounds.Left + RelativePosition.Left, anchorBounds.Top - base.Height + RelativePosition.Top);
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
