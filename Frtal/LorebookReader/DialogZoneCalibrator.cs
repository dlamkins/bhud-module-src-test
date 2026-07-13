using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	public sealed class DialogZoneCalibrator : Control
	{
		private enum Drag
		{
			None,
			Move,
			TL,
			TR,
			BL,
			BR
		}

		private const int HandleSize = 16;

		private const int MinW = 80;

		private const int MinH = 32;

		private static readonly Color DimColor = new Color(0, 0, 0, 115);

		private static readonly Color FillColor = new Color(90, 170, 255, 38);

		private static readonly Color BorderColor = new Color(120, 200, 255, 255);

		private static readonly Color HandleColor = new Color(255, 232, 150, 255);

		private Rectangle _zone;

		private readonly Action<Rectangle> _onSave;

		private readonly Action _onCancel;

		private Drag _mode;

		private bool _dragging;

		private Point _dragStart;

		private Rectangle _startZone;

		private readonly StandardButton _saveBtn;

		private readonly StandardButton _cancelBtn;

		private readonly Label _hint;

		private static Point Mouse => GameService.Input.get_Mouse().get_Position();

		public DialogZoneCalibrator(Rectangle initialZone, Action<Rectangle> onSave, Action onCancel)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			_zone = initialZone;
			_onSave = onSave;
			_onCancel = onCancel;
			Screen screen = GameService.Graphics.get_SpriteScreen();
			((Control)this).set_Parent((Container)(object)screen);
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_Size(((Control)screen).get_Size());
			((Control)this).set_ZIndex(2147483637);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)screen);
			val.set_Text("Drag the frame over the TEXT to capture (corners = resize). Then Save.");
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_ZIndex(2147483638);
			_hint = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)screen);
			val2.set_Text("Save zone");
			((Control)val2).set_Width(150);
			((Control)val2).set_ZIndex(2147483638);
			_saveBtn = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)screen);
			val3.set_Text("Cancel");
			((Control)val3).set_Width(110);
			((Control)val3).set_ZIndex(2147483638);
			_cancelBtn = val3;
			((Control)_saveBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				_onSave?.Invoke(_zone);
			});
			((Control)_cancelBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_onCancel?.Invoke();
			});
			ClampZone();
			LayoutChrome();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			if (!_dragging)
			{
				Point i = Mouse;
				_mode = HitTest(i);
				if (_mode != 0)
				{
					_dragging = true;
					_dragStart = i;
					_startZone = _zone;
					GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
					GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
				}
			}
		}

		private void OnGlobalMouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				Point i = Mouse;
				ApplyDrag(i.X - _dragStart.X, i.Y - _dragStart.Y);
				ClampZone();
				LayoutChrome();
			}
		}

		private void OnGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			StopDrag();
		}

		private void StopDrag()
		{
			if (_dragging)
			{
				_dragging = false;
				_mode = Drag.None;
				GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
				GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			Point sz = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			if (((Control)this).get_Size() != sz)
			{
				((Control)this).set_Size(sz);
			}
		}

		private Drag HitTest(Point m)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (InHandle(m, ((Rectangle)(ref _zone)).get_Left(), ((Rectangle)(ref _zone)).get_Top()))
			{
				return Drag.TL;
			}
			if (InHandle(m, ((Rectangle)(ref _zone)).get_Right(), ((Rectangle)(ref _zone)).get_Top()))
			{
				return Drag.TR;
			}
			if (InHandle(m, ((Rectangle)(ref _zone)).get_Left(), ((Rectangle)(ref _zone)).get_Bottom()))
			{
				return Drag.BL;
			}
			if (InHandle(m, ((Rectangle)(ref _zone)).get_Right(), ((Rectangle)(ref _zone)).get_Bottom()))
			{
				return Drag.BR;
			}
			if (((Rectangle)(ref _zone)).Contains(m))
			{
				return Drag.Move;
			}
			return Drag.None;
		}

		private static bool InHandle(Point m, int cx, int cy)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (Math.Abs(m.X - cx) <= 16)
			{
				return Math.Abs(m.Y - cy) <= 16;
			}
			return false;
		}

		private void ApplyDrag(int dx, int dy)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			Rectangle z = _startZone;
			switch (_mode)
			{
			case Drag.Move:
				z.X += dx;
				z.Y += dy;
				break;
			case Drag.BR:
				z.Width += dx;
				z.Height += dy;
				break;
			case Drag.TL:
				z.X += dx;
				z.Y += dy;
				z.Width -= dx;
				z.Height -= dy;
				break;
			case Drag.TR:
				z.Y += dy;
				z.Width += dx;
				z.Height -= dy;
				break;
			case Drag.BL:
				z.X += dx;
				z.Width -= dx;
				z.Height += dy;
				break;
			}
			_zone = z;
		}

		private void ClampZone()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			Point sz = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			if (_zone.Width < 80)
			{
				_zone.Width = 80;
			}
			if (_zone.Height < 32)
			{
				_zone.Height = 32;
			}
			if (_zone.Width > sz.X)
			{
				_zone.Width = sz.X;
			}
			if (_zone.Height > sz.Y)
			{
				_zone.Height = sz.Y;
			}
			if (_zone.X < 0)
			{
				_zone.X = 0;
			}
			if (_zone.Y < 0)
			{
				_zone.Y = 0;
			}
			if (((Rectangle)(ref _zone)).get_Right() > sz.X)
			{
				_zone.X = Math.Max(0, sz.X - _zone.Width);
			}
			if (((Rectangle)(ref _zone)).get_Bottom() > sz.Y)
			{
				_zone.Y = Math.Max(0, sz.Y - _zone.Height);
			}
		}

		private void LayoutChrome()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			Point size = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			((Control)_hint).set_Location(new Point(Math.Max(8, _zone.X), Math.Max(8, _zone.Y - 28)));
			int by = Math.Min(size.Y - 40, ((Rectangle)(ref _zone)).get_Bottom() + 8);
			((Control)_cancelBtn).set_Location(new Point(Math.Max(8, ((Rectangle)(ref _zone)).get_Right() - ((Control)_cancelBtn).get_Width()), by));
			((Control)_saveBtn).set_Location(new Point(Math.Max(8, ((Rectangle)(ref _zone)).get_Right() - ((Control)_cancelBtn).get_Width() - ((Control)_saveBtn).get_Width() - 8), by));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			Texture2D px = Textures.get_Pixel();
			int L = ((Rectangle)(ref _zone)).get_Left();
			int T = ((Rectangle)(ref _zone)).get_Top();
			int R = ((Rectangle)(ref _zone)).get_Right();
			int B = ((Rectangle)(ref _zone)).get_Bottom();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(0, 0, bounds.Width, T), DimColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(0, B, bounds.Width, bounds.Height - B), DimColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(0, T, L, B - T), DimColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(R, T, bounds.Width - R, B - T), DimColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, _zone, FillColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(L, T, _zone.Width, 2), BorderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(L, B - 2, _zone.Width, 2), BorderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(L, T, 2, _zone.Height), BorderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, px, new Rectangle(R - 2, T, 2, _zone.Height), BorderColor);
			DrawHandle(spriteBatch, px, L, T);
			DrawHandle(spriteBatch, px, R, T);
			DrawHandle(spriteBatch, px, L, B);
			DrawHandle(spriteBatch, px, R, B);
		}

		private void DrawHandle(SpriteBatch sb, Texture2D px, int cx, int cy)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(cx - 8, cy - 8, 16, 16), HandleColor);
		}

		protected override void DisposeControl()
		{
			StopDrag();
			Label hint = _hint;
			if (hint != null)
			{
				((Control)hint).Dispose();
			}
			StandardButton saveBtn = _saveBtn;
			if (saveBtn != null)
			{
				((Control)saveBtn).Dispose();
			}
			StandardButton cancelBtn = _cancelBtn;
			if (cancelBtn != null)
			{
				((Control)cancelBtn).Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
