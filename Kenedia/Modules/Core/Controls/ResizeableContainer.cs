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
				Rectangle localBounds = base.LocalBounds;
				int num = ((Rectangle)(ref localBounds)).get_Right() - 15;
				localBounds = base.LocalBounds;
				return new Rectangle(num, ((Rectangle)(ref localBounds)).get_Bottom() - 15, 15, 15);
			}
		}

		public bool CaptureInput { get; set; } = true;


		public bool CanChange { get; set; } = true;


		public bool ShowCenter { get; set; }

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
			_dragging = _dragging && base.MouseOver;
			_resizing = _resizing && base.MouseOver;
			_mouseOverResizeHandle = _mouseOverResizeHandle && base.MouseOver;
			if (_dragging)
			{
				base.Location = Control.Input.Mouse.Position.Add(new Point(-_draggingStart.X, -_draggingStart.Y));
			}
			if (_resizing)
			{
				Point nOffset = Control.Input.Mouse.Position - _dragStart;
				Point newSize = _resizeStart + nOffset;
				base.Size = new Point(MathHelper.Clamp(newSize.X, 50, MaxSize.X), MathHelper.Clamp(newSize.Y, 25, MaxSize.Y));
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_resizeHandleBounds = new Rectangle(base.Width - _resizeTexture.Width, base.Height - _resizeTexture.Height, _resizeTexture.Width, _resizeTexture.Height);
		}

		protected override CaptureType CapturesInput()
		{
			if (!CaptureInput)
			{
				return CaptureType.None;
			}
			return base.CapturesInput();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (_resizeTexture != null && CanChange && (!ShowResizeOnlyOnMouseOver || base.MouseOver))
			{
				spriteBatch.DrawOnCtrl(this, (_resizing || _mouseOverResizeHandle) ? _resizeTextureHovered : _resizeTexture, new Rectangle(((Rectangle)(ref bounds)).get_Right() - _resizeTexture.Width - 1, ((Rectangle)(ref bounds)).get_Bottom() - _resizeTexture.Height - 1, _resizeTexture.Width, _resizeTexture.Height), _resizeTexture.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (ShowCenter)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.Width / 2 - 2, base.Height / 2 - 2, 4, 4), ContentService.Textures.Pixel.get_Bounds(), Color.get_Red(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			if (CanChange)
			{
				_dragging = false;
				_resizing = false;
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			base.OnLeftMouseButtonPressed(e);
			if (CanChange)
			{
				Rectangle resizeCorner = ResizeCorner;
				_resizing = ((Rectangle)(ref resizeCorner)).Contains(e.MousePosition);
				_resizeStart = base.Size;
				_dragStart = Control.Input.Mouse.Position;
				_dragging = !_resizing;
				_draggingStart = (_dragging ? base.RelativeMousePosition : Point.get_Zero());
			}
		}

		protected virtual Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, base.ContentRegion.X, 1024), MathHelper.Clamp(newSize.Y, base.ContentRegion.Y, 1024));
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (CanChange)
			{
				ResetMouseRegionStates();
				if (((Rectangle)(ref _resizeHandleBounds)).Contains(base.RelativeMousePosition) && base.RelativeMousePosition.X > ((Rectangle)(ref _resizeHandleBounds)).get_Right() - 16 && base.RelativeMousePosition.Y > ((Rectangle)(ref _resizeHandleBounds)).get_Bottom() - 16)
				{
					_mouseOverResizeHandle = true;
				}
			}
			base.OnMouseMoved(e);
		}

		private void ResetMouseRegionStates()
		{
			_mouseOverResizeHandle = false;
		}
	}
}
