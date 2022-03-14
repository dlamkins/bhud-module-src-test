using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Manlaan.Clock.Control
{
	internal class DrawClock : Container
	{
		public bool ShowLocal = false;

		public bool ShowTyria = false;

		public bool ShowServer = false;

		public bool ShowDayNight = false;

		public bool Show24H = false;

		public bool HideLabel = false;

		public bool Drag = false;

		public FontSize Font_Size = (FontSize)11;

		public DateTime LocalTime = DateTime.Now;

		public DateTime TyriaTime;

		public DateTime ServerTime;

		public string DayNightTime;

		public HorizontalAlignment LabelAlign = (HorizontalAlignment)2;

		public HorizontalAlignment TimeAlign = (HorizontalAlignment)2;

		private static BitmapFont _font;

		private Point _dragStart = Point.get_Zero();

		private bool _dragging;

		public DrawClock()
			: this()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(0, 0));
			((Control)this).set_Size(new Point(0, 0));
			((Control)this).set_Visible(true);
			((Control)this).set_Padding(Thickness.Zero);
		}

		protected override CaptureType CapturesInput()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (Drag)
			{
				return (CaptureType)4;
			}
			return (CaptureType)1;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (Drag)
			{
				_dragging = true;
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (Drag)
			{
				_dragging = false;
				Module._settingClockLoc.set_Value(((Control)this).get_Location());
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		public void EnsureLocationIsInBounds()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			Point windowSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			if (((Control)this).get_Location().X < 1)
			{
				((Control)this).set_Location(new Point(1, ((Control)this).get_Location().Y));
			}
			else if (((Control)this).get_Location().X + ((Control)this).get_Size().X > windowSize.X)
			{
				((Control)this).set_Location(new Point(windowSize.X - ((Control)this).get_Size().X, ((Control)this).get_Location().Y));
			}
			if (((Control)this).get_Location().Y < 1)
			{
				((Control)this).set_Location(new Point(((Control)this).get_Location().X, 1));
			}
			else if (((Control)this).get_Location().Y + ((Control)this).get_Size().Y > windowSize.Y)
			{
				((Control)this).set_Location(new Point(((Control)this).get_Location().X, windowSize.Y - ((Control)this).get_Size().Y));
			}
		}

		private bool IsPointInBounds(Point point)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			Point windowSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			return point.X > 0 && point.Y > 0 && point.X < windowSize.X && point.Y < windowSize.Y;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				if (IsPointInBounds(Control.get_Input().get_Mouse().get_Position()))
				{
					Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
					((Control)this).set_Location(((Control)this).get_Location() + nOffset);
					EnsureLocationIsInBounds();
				}
				else
				{
					_dragging = false;
					Module._settingClockLoc.set_Value(((Control)this).get_Location());
				}
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			List<string> labels = new List<string>();
			List<string> times = new List<string>();
			_font = GameService.Content.GetFont((FontFace)0, Font_Size, (FontStyle)0);
			string format = "h:mm tt";
			if (Show24H)
			{
				format = "HH:mm";
			}
			if (ShowLocal)
			{
				if (!HideLabel)
				{
					labels.Add(" Local: ");
				}
				times.Add(" " + LocalTime.ToString(format));
			}
			if (ShowTyria)
			{
				if (!HideLabel)
				{
					labels.Add(" Tyria: ");
				}
				times.Add(" " + TyriaTime.ToString(format));
			}
			if (ShowServer)
			{
				if (!HideLabel)
				{
					labels.Add(" Server: ");
				}
				times.Add(" " + ServerTime.ToString(format));
			}
			Point LabelSize = default(Point);
			((Point)(ref LabelSize))._002Ector((int)_font.MeasureString(string.Join("\n", labels)).Width, (int)_font.MeasureString(string.Join("\n", labels)).Height);
			Point TimeSize = default(Point);
			((Point)(ref TimeSize))._002Ector((int)_font.MeasureString(string.Join("\n", times)).Width, (int)_font.MeasureString(string.Join("\n", times)).Height);
			int maxHeight = Math.Max(LabelSize.Y, TimeSize.Y);
			((Control)this).set_Size(new Point(LabelSize.X + TimeSize.X, maxHeight));
			if (!HideLabel)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, string.Join("\n", labels), _font, new Rectangle(0, 0, LabelSize.X, ((Control)this).get_Size().Y), Color.get_White(), false, true, 1, LabelAlign, (VerticalAlignment)0);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, string.Join("\n", times), _font, new Rectangle(LabelSize.X, 0, TimeSize.X, ((Control)this).get_Size().Y), Color.get_White(), false, true, 1, TimeAlign, (VerticalAlignment)0);
			if (ShowDayNight)
			{
				Point DayNightSize = default(Point);
				((Point)(ref DayNightSize))._002Ector((int)_font.MeasureString(DayNightTime).Width, (int)_font.MeasureString(DayNightTime).Height);
				((Control)this).set_Size(new Point(LabelSize.X + TimeSize.X, maxHeight + DayNightSize.Y));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, DayNightTime, _font, new Rectangle(0, maxHeight, TimeSize.X + LabelSize.X, DayNightSize.Y), Color.get_White(), false, true, 1, LabelAlign, (VerticalAlignment)0);
			}
		}
	}
}
