using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class ResizeableContainer : FramedContainer
	{
		private const int s_resizeHandleSize = 16;

		private readonly AsyncTexture2D _resizeTexture = AsyncTexture2D.FromAssetId(156009);

		private readonly AsyncTexture2D _resizeTextureHovered = AsyncTexture2D.FromAssetId(156010);

		private bool _dragging;

		private bool _resizing;

		private bool _mouseOverResizeHandle;

		private Point _resizeStart;

		private Point _dragStart;

		private Point _draggingStart;

		private Rectangle _resizeHandleBounds = Rectangle.get_Empty();

		public bool ShowResizeOnlyOnMouseOver { get; set; }

		public Point MaxSize { get; set; } = new Point(499, 499);


		private Rectangle ResizeCorner
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				Rectangle localBounds = ((Control)this).get_LocalBounds();
				int num = ((Rectangle)(ref localBounds)).get_Right() - 15;
				localBounds = ((Control)this).get_LocalBounds();
				return new Rectangle(num, ((Rectangle)(ref localBounds)).get_Bottom() - 15, 15, 15);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			_dragging = _dragging && ((Control)this).get_MouseOver();
			_resizing = _resizing && ((Control)this).get_MouseOver();
			_mouseOverResizeHandle = _mouseOverResizeHandle && ((Control)this).get_MouseOver();
			if (_dragging)
			{
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(-_draggingStart.X, -_draggingStart.Y)));
			}
			if (_resizing)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				Point newSize = _resizeStart + nOffset;
				((Control)this).set_Size(new Point(MathHelper.Clamp(newSize.X, 50, MaxSize.X), MathHelper.Clamp(newSize.Y, 25, MaxSize.Y)));
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_resizeHandleBounds = new Rectangle(((Control)this).get_Width() - _resizeTexture.get_Width(), ((Control)this).get_Height() - _resizeTexture.get_Height(), _resizeTexture.get_Width(), _resizeTexture.get_Height());
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (_resizeTexture != null && (!ShowResizeOnlyOnMouseOver || ((Control)this).get_MouseOver()))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((_resizing || _mouseOverResizeHandle) ? _resizeTextureHovered : _resizeTexture), new Rectangle(((Rectangle)(ref bounds)).get_Right() - _resizeTexture.get_Width() - 1, ((Rectangle)(ref bounds)).get_Bottom() - _resizeTexture.get_Height() - 1, _resizeTexture.get_Width(), _resizeTexture.get_Height()), (Rectangle?)_resizeTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			_dragging = false;
			_resizing = false;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Rectangle resizeCorner = ResizeCorner;
			_resizing = ((Rectangle)(ref resizeCorner)).Contains(e.get_MousePosition());
			_resizeStart = ((Control)this).get_Size();
			_dragStart = Control.get_Input().get_Mouse().get_Position();
			_dragging = !_resizing;
			_draggingStart = (_dragging ? ((Control)this).get_RelativeMousePosition() : Point.get_Zero());
		}

		protected virtual Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, ((Container)this).get_ContentRegion().X, 1024), MathHelper.Clamp(newSize.Y, ((Container)this).get_ContentRegion().Y, 1024));
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			ResetMouseRegionStates();
			if (((Rectangle)(ref _resizeHandleBounds)).Contains(((Control)this).get_RelativeMousePosition()) && ((Control)this).get_RelativeMousePosition().X > ((Rectangle)(ref _resizeHandleBounds)).get_Right() - 16 && ((Control)this).get_RelativeMousePosition().Y > ((Rectangle)(ref _resizeHandleBounds)).get_Bottom() - 16)
			{
				_mouseOverResizeHandle = true;
			}
			base.OnMouseMoved(e);
		}

		private void ResetMouseRegionStates()
		{
			_mouseOverResizeHandle = false;
		}
	}
}
