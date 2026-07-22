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

		private Rectangle _resizeHandleBounds = Rectangle.Empty;

		public bool ShowResizeOnlyOnMouseOver { get; set; }

		public Point MaxSize { get; set; } = new Point(499, 499);


		private Rectangle ResizeCorner => new Rectangle(base.LocalBounds.Right - 15, base.LocalBounds.Bottom - 15, 15, 15);

		public bool CaptureInput { get; set; } = true;


		public bool CanChange { get; set; } = true;


		public bool ShowCenter { get; set; }

		public override void UpdateContainer(GameTime gameTime)
		{
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
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (_resizeTexture != null && CanChange && (!ShowResizeOnlyOnMouseOver || base.MouseOver))
			{
				spriteBatch.DrawOnCtrl(this, (_resizing || _mouseOverResizeHandle) ? _resizeTextureHovered : _resizeTexture, new Rectangle(bounds.Right - _resizeTexture.Width - 1, bounds.Bottom - _resizeTexture.Height - 1, _resizeTexture.Width, _resizeTexture.Height), _resizeTexture.Bounds, Color.White, 0f, default(Vector2));
			}
			if (ShowCenter)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.Width / 2 - 2, base.Height / 2 - 2, 4, 4), ContentService.Textures.Pixel.Bounds, Color.Red, 0f, default(Vector2));
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
			base.OnLeftMouseButtonPressed(e);
			if (CanChange)
			{
				_resizing = ResizeCorner.Contains(e.MousePosition);
				_resizeStart = base.Size;
				_dragStart = Control.Input.Mouse.Position;
				_dragging = !_resizing;
				_draggingStart = (_dragging ? base.RelativeMousePosition : Point.Zero);
			}
		}

		protected virtual Point HandleWindowResize(Point newSize)
		{
			return new Point(MathHelper.Clamp(newSize.X, base.ContentRegion.X, 1024), MathHelper.Clamp(newSize.Y, base.ContentRegion.Y, 1024));
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			if (CanChange)
			{
				ResetMouseRegionStates();
				if (_resizeHandleBounds.Contains(base.RelativeMousePosition) && base.RelativeMousePosition.X > _resizeHandleBounds.Right - 16 && base.RelativeMousePosition.Y > _resizeHandleBounds.Bottom - 16)
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
