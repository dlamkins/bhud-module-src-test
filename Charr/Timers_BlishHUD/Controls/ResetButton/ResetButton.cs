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

		public Point MaxSize = new Point(499, 499);

		private Point _resizeStart;

		private Point _dragStart;

		private Point _draggingStart;

		public Color TintColor = Color.get_Black() * 0.5f;

		public bool TintOnHover;

		private AsyncTexture2D _resizeTexture = AsyncTexture2D.op_Implicit(TimersModule.ModuleInstance.ContentsManager.GetTexture("textures\\156009.png"));

		private AsyncTexture2D _resizeTextureHovered = AsyncTexture2D.op_Implicit(TimersModule.ModuleInstance.ContentsManager.GetTexture("textures\\156010.png"));

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
				Rectangle localBounds = ((Control)this).get_LocalBounds();
				int num = ((Rectangle)(ref localBounds)).get_Right() - 15;
				localBounds = ((Control)this).get_LocalBounds();
				return new Rectangle(num, ((Rectangle)(ref localBounds)).get_Bottom() - 15, 15, 15);
			}
		}

		public event EventHandler ButtonClicked;

		public event EventHandler BoundsChanged;

		public ResetButton()
			: this()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Location(new Point(2, 2));
			val.set_Text("Reset Timer");
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_BasicTooltipText(string.Format("Reset Active Timers" + Environment.NewLine + "Hold {0} to move and resize.", ((object)(Keys)(ref ModifierKey)).ToString()));
			((Control)val).set_Visible(false);
			Button = val;
			Image val2 = new Image();
			((Control)val2).set_Location(new Point(2, 2));
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_BasicTooltipText(string.Format("Reset Active Timers" + Environment.NewLine + "Hold {0} to move and resize.", ((object)(Keys)(ref ModifierKey)).ToString()));
			val2.set_Texture(AsyncTexture2D.op_Implicit(TimersModule.ModuleInstance.ContentsManager.GetTexture("textures\\resetbutton_big.png")));
			ImageButton = val2;
			((Control)ImageButton).add_Click((EventHandler<MouseEventArgs>)Button_Click);
			((Control)Button).add_Click((EventHandler<MouseEventArgs>)Button_Click);
		}

		private void Button_Click(object sender, MouseEventArgs e)
		{
			this.ButtonClicked?.Invoke(sender, (EventArgs)(object)e);
		}

		public void ToggleVisibility()
		{
			((Control)this).set_Visible(!((Control)this).get_Visible());
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
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
			((Control)this).OnLeftMouseButtonPressed(e);
			if (((Control)this).get_MouseOver() && Control.get_Input().get_Keyboard().get_KeysDown()
				.ToList()
				.Contains(ModifierKey))
			{
				Rectangle resizeCorner = _resizeCorner;
				_resizing = ((Rectangle)(ref resizeCorner)).Contains(e.get_MousePosition());
				_resizeStart = ((Control)this).get_Size();
				_dragStart = Control.get_Input().get_Mouse().get_Position();
				_dragging = !_resizing;
				_draggingStart = (_dragging ? ((Control)this).get_RelativeMousePosition() : Point.get_Zero());
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
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			_dragging = _dragging && ((Control)this).get_MouseOver();
			_resizing = _resizing && ((Control)this).get_MouseOver();
			MouseOverResizeHandle = MouseOverResizeHandle && ((Control)this).get_MouseOver();
			if (_dragging)
			{
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position() + new Point(-_draggingStart.X, -_draggingStart.Y));
				this.BoundsChanged?.Invoke(null, null);
			}
			if (_resizing)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				Point newSize = _resizeStart + nOffset;
				((Control)this).set_Size(new Point(MathHelper.Clamp(newSize.X, 50, MaxSize.X), MathHelper.Clamp(newSize.Y, 25, MaxSize.Y)));
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
			if (((Rectangle)(ref resizeHandleBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				int x = ((Control)this).get_RelativeMousePosition().X;
				resizeHandleBounds = ResizeHandleBounds;
				if (x > ((Rectangle)(ref resizeHandleBounds)).get_Right() - 16)
				{
					int y = ((Control)this).get_RelativeMousePosition().Y;
					resizeHandleBounds = ResizeHandleBounds;
					if (y > ((Rectangle)(ref resizeHandleBounds)).get_Bottom() - 16)
					{
						MouseOverResizeHandle = true;
					}
				}
			}
			((Control)this).OnMouseMoved(e);
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
			((Control)this).RecalculateLayout();
			ResizeHandleBounds = new Rectangle(((Control)this).get_Width() - _resizeTexture.get_Texture().get_Width(), ((Control)this).get_Height() - _resizeTexture.get_Texture().get_Height(), _resizeTexture.get_Texture().get_Width(), _resizeTexture.get_Texture().get_Height());
			Point sqSize = default(Point);
			((Point)(ref sqSize))._002Ector(Math.Min(((Control)this).get_Width(), ((Control)this).get_Height()) - 5, Math.Min(((Control)this).get_Width(), ((Control)this).get_Height()) - 5);
			if (Button != null)
			{
				((Control)Button).set_Size(((Control)this).get_Size() - new Point(5, 5));
			}
			if (ImageButton != null)
			{
				((Control)ImageButton).set_Size(sqSize);
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
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
			if (((Control)this).get_MouseOver() && Control.get_Input().get_Keyboard().get_KeysDown()
				.ToList()
				.Contains(ModifierKey))
			{
				if (!TintOnHover || ((Control)this).get_MouseOver())
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)Rectangle.get_Empty(), TintColor, 0f, default(Vector2), (SpriteEffects)0);
				}
				if (_resizeTexture != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((_resizing || MouseOverResizeHandle) ? _resizeTextureHovered : _resizeTexture), new Rectangle(((Rectangle)(ref bounds)).get_Right() - _resizeTexture.get_Texture().get_Width() - 1, ((Rectangle)(ref bounds)).get_Bottom() - _resizeTexture.get_Texture().get_Height() - 1, _resizeTexture.get_Texture().get_Width(), _resizeTexture.get_Texture().get_Height()), (Rectangle?)_resizeTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				Color color = (((Control)this).get_MouseOver() ? Colors.ColonialWhite : Color.get_Transparent());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			}
		}
	}
}
