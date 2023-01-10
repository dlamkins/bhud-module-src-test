using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Charr.Timers_BlishHUD.Controls.ResetButton
{
	internal class ResetButton : Container
	{
		private bool _dragging;

		private bool _resizing;

		private Keys ModifierKey = (Keys)162;

		private const int RESIZEHANDLE_SIZE = 16;

		private bool MouseOverResizeHandle;

		public int MinSize = 50;

		public int MaxSize = 499;

		private Point _resizeStart;

		private Point _dragStart;

		private Point _draggingStart;

		public Color TintColor = Color.get_Black() * 0.5f;

		public bool TintOnHover;

		private AsyncTexture2D _resizeTexture = TimersModule.ModuleInstance.ContentsManager.GetTexture("textures\\156009.png");

		private AsyncTexture2D _resizeTextureHovered = TimersModule.ModuleInstance.ContentsManager.GetTexture("textures\\156010.png");

		private StandardButton Button;

		private Image ImageButton;

		protected Rectangle ResizeHandleBounds { get; private set; } = Rectangle.get_Empty();


		private Rectangle _resizeCorner
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

		public event EventHandler ButtonClicked;

		public event EventHandler BoundsChanged;

		public ResetButton()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			Button = new StandardButton
			{
				Location = new Point(2, 2),
				Text = "Reset Timer",
				Parent = this,
				BasicTooltipText = string.Format("Reset Active Timers" + Environment.NewLine + "Hold {0} to move and resize.", ((object)(Keys)(ref ModifierKey)).ToString()),
				Visible = false
			};
			ImageButton = new Image
			{
				Location = new Point(2, 2),
				Parent = this,
				BasicTooltipText = string.Format("Reset Active Timers" + Environment.NewLine + "Hold {0} to move and resize.", ((object)(Keys)(ref ModifierKey)).ToString()),
				Texture = TimersModule.ModuleInstance.ContentsManager.GetTexture("textures\\resetbutton_big.png")
			};
			ImageButton.Click += Button_Click;
			Button.Click += Button_Click;
		}

		private void Button_Click(object sender, MouseEventArgs e)
		{
			this.ButtonClicked?.Invoke(sender, e);
		}

		public void ToggleVisibility()
		{
			base.Visible = !base.Visible;
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			_dragging = false;
			_resizing = false;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			base.OnLeftMouseButtonPressed(e);
			if (base.MouseOver && Control.Input.Keyboard.KeysDown.ToList().Contains(ModifierKey))
			{
				Rectangle resizeCorner = _resizeCorner;
				_resizing = ((Rectangle)(ref resizeCorner)).Contains(e.MousePosition);
				_resizeStart = base.Size;
				_dragStart = Control.Input.Mouse.Position;
				_dragging = !_resizing;
				_draggingStart = (_dragging ? base.RelativeMousePosition : Point.get_Zero());
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			_dragging = _dragging && base.MouseOver;
			_resizing = _resizing && base.MouseOver;
			MouseOverResizeHandle = MouseOverResizeHandle && base.MouseOver;
			if (_dragging)
			{
				base.Location = Control.Input.Mouse.Position + new Point(-_draggingStart.X, -_draggingStart.Y);
				this.BoundsChanged?.Invoke(null, null);
			}
			if (_resizing)
			{
				Point nOffset = Control.Input.Mouse.Position - _dragStart;
				Point newSize = _resizeStart + nOffset;
				int longestLength = Math.Max(newSize.X, newSize.Y);
				base.Size = new Point(MathHelper.Clamp(longestLength, MinSize, MaxSize), MathHelper.Clamp(longestLength, MinSize, MaxSize));
				this.BoundsChanged?.Invoke(null, null);
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			ResetMouseRegionStates();
			Rectangle resizeHandleBounds = ResizeHandleBounds;
			if (((Rectangle)(ref resizeHandleBounds)).Contains(base.RelativeMousePosition))
			{
				int x = base.RelativeMousePosition.X;
				resizeHandleBounds = ResizeHandleBounds;
				if (x > ((Rectangle)(ref resizeHandleBounds)).get_Right() - 16)
				{
					int y = base.RelativeMousePosition.Y;
					resizeHandleBounds = ResizeHandleBounds;
					if (y > ((Rectangle)(ref resizeHandleBounds)).get_Bottom() - 16)
					{
						MouseOverResizeHandle = true;
					}
				}
			}
			base.OnMouseMoved(e);
		}

		private void ResetMouseRegionStates()
		{
			MouseOverResizeHandle = false;
		}

		public override void RecalculateLayout()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			ResizeHandleBounds = new Rectangle(base.Width - _resizeTexture.Texture.get_Width(), base.Height - _resizeTexture.Texture.get_Height(), _resizeTexture.Texture.get_Width(), _resizeTexture.Texture.get_Height());
			Point sqSize = default(Point);
			((Point)(ref sqSize))._002Ector(Math.Min(base.Width, base.Height) - 5, Math.Min(base.Width, base.Height) - 5);
			if (Button != null)
			{
				Button.Size = base.Size - new Point(5, 5);
			}
			if (ImageButton != null)
			{
				ImageButton.Size = sqSize;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			if (base.MouseOver && Control.Input.Keyboard.KeysDown.ToList().Contains(ModifierKey))
			{
				if (!TintOnHover || base.MouseOver)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Rectangle.get_Empty(), TintColor, 0f, default(Vector2), (SpriteEffects)0);
				}
				if (_resizeTexture != null)
				{
					spriteBatch.DrawOnCtrl(this, (_resizing || MouseOverResizeHandle) ? _resizeTextureHovered : _resizeTexture, new Rectangle(((Rectangle)(ref bounds)).get_Right() - _resizeTexture.Texture.get_Width() - 1, ((Rectangle)(ref bounds)).get_Bottom() - _resizeTexture.Texture.get_Height() - 1, _resizeTexture.Texture.get_Width(), _resizeTexture.Texture.get_Height()), _resizeTexture.Texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				Color color = (base.MouseOver ? ContentService.Colors.ColonialWhite : Color.get_Transparent());
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), Rectangle.get_Empty(), color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), Rectangle.get_Empty(), color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), Rectangle.get_Empty(), color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), Rectangle.get_Empty(), color * 0.6f);
			}
		}
	}
}
