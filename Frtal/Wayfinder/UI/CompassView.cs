using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Frtal.Wayfinder.Models;
using Frtal.Wayfinder.Services;
using Frtal.Wayfinder.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Frtal.Wayfinder.UI
{
	public class CompassView : Control
	{
		private static readonly (string Label, double Bearing)[] Cardinals = new(string, double)[8]
		{
			("N", 0.0),
			("NE", Math.PI / 4.0),
			("E", Math.PI / 2.0),
			("SE", Math.PI * 3.0 / 4.0),
			("S", Math.PI),
			("SW", 3.9269908169872414),
			("W", 4.71238898038469),
			("NW", 5.497787143782138)
		};

		private readonly BitmapFont _font = GameService.Content.get_DefaultFont14();

		private readonly MapIconService _icons;

		private Vector2 _playerCont;

		private Vector2 _facing = new Vector2(0f, -1f);

		private MapCalibration _calibration;

		private IReadOnlyList<CompassTarget> _targets = new List<CompassTarget>();

		private int _barHeight = 38;

		private bool _showDebug;

		private bool _dragging;

		private Point _dragOffset;

		public double HalfFovRadians { get; set; } = Math.PI;


		public double MaxDistanceMeters { get; set; } = 750.0;


		public bool ShowDistance { get; set; } = true;


		public bool ShowNames { get; set; }

		public bool ScaleWithDistance { get; set; } = true;


		public float ScaleNear { get; set; } = 2f;


		public float ScaleFar { get; set; } = 0.45f;


		public float ScaleDistance { get; set; } = 250f;


		public bool ShowCardinals { get; set; } = true;


		public bool Monochrome { get; set; }

		public bool MarkUndiscovered { get; set; }

		public float EdgeFade { get; set; } = 0.14f;


		public Func<string, bool> IsDiscovered { get; set; }

		public float BackgroundOpacity { get; set; } = 0.45f;


		public bool DragEnabled { get; set; }

		public string DebugText { get; set; }

		public Action<Point> PositionChanged { get; set; }

		public bool IsDragging => _dragging;

		public CompassView(MapIconService icons)
			: this()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			_icons = icons;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_ZIndex(10);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
		}

		protected override CaptureType CapturesInput()
		{
			if (DragEnabled)
			{
				return (CaptureType)4;
			}
			return (CaptureType)0;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			if (DragEnabled)
			{
				_dragging = true;
				_dragOffset = ((Control)this).get_RelativeMousePosition();
			}
		}

		private void OnGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				_dragging = false;
				PositionChanged?.Invoke(((Control)this).get_Location());
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				if (!DragEnabled)
				{
					_dragging = false;
					return;
				}
				Point position = GameService.Input.get_Mouse().get_Position();
				int x = position.X - _dragOffset.X;
				int y = position.Y - _dragOffset.Y;
				Screen screen = GameService.Graphics.get_SpriteScreen();
				x = MathHelper.Clamp(x, 0, Math.Max(0, ((Control)screen).get_Width() - ((Control)this).get_Width()));
				y = MathHelper.Clamp(y, 0, Math.Max(0, ((Control)screen).get_Height() - ((Control)this).get_Height()));
				((Control)this).set_Location(new Point(x, y));
			}
		}

		public void UpdateContext(Vector2 playerCont, Vector2 facing, MapCalibration calibration, IReadOnlyList<CompassTarget> targets)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			_playerCont = playerCont;
			if (((Vector2)(ref facing)).LengthSquared() > 0.0001f)
			{
				_facing = facing;
			}
			_calibration = calibration;
			_targets = targets ?? new List<CompassTarget>();
		}

		public void ApplyLayout(float widthFraction, float scale, bool showDebug)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			int w = (int)((float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width() * MathHelper.Clamp(widthFraction, 0.2f, 1f));
			_barHeight = (int)(38f * MathHelper.Clamp(scale, 0.5f, 2f));
			_showDebug = showDebug;
			((Control)this).set_Size(new Point(w, _barHeight + (showDebug ? 14 : 0)));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_063a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_065e: Unknown result type (might be due to invalid IL or missing references)
			//IL_066f: Unknown result type (might be due to invalid IL or missing references)
			int barH = _barHeight;
			float halfWidth = (float)bounds.Width / 2f;
			float centerX = (float)bounds.Width / 2f;
			Texture2D pixel = Textures.get_Pixel();
			if (BackgroundOpacity > 0.001f)
			{
				int sliceW = Math.Max(1, bounds.Width / 64 + 1);
				for (int j = 0; j < 64; j++)
				{
					int x3 = j * bounds.Width / 64;
					float a = BackgroundOpacity * EdgeAlpha(x3, bounds.Width);
					if (!(a <= 0.002f))
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(x3, 0, sliceW, barH), Color.get_Black() * a);
					}
				}
				for (int i = 0; i < 64; i++)
				{
					int x4 = i * bounds.Width / 64;
					float a2 = 0.35f * BackgroundOpacity * EdgeAlpha(x4, bounds.Width);
					if (!(a2 <= 0.002f))
					{
						Color edge2 = new Color(220, 200, 150) * a2;
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(x4, 0, sliceW, 1), edge2);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(x4, barH - 1, sliceW, 1), edge2);
					}
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)centerX - 1, 0, 2, barH), new Color(255, 235, 180) * 0.9f);
			if (DragEnabled)
			{
				Color edge = new Color(255, 220, 120) * (_dragging ? 0.95f : 0.55f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, 0, bounds.Width, 1), edge);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, barH - 1, bounds.Width, 1), edge);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, 0, 1, barH), edge);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(bounds.Width - 1, 0, 1, barH), edge);
			}
			double facingBearing = Gw2CoordinateUtil.BearingFromNorth(_facing);
			if (ShowCardinals)
			{
				(string, double)[] cardinals = Cardinals;
				for (int k = 0; k < cardinals.Length; k++)
				{
					(string, double) tuple = cardinals[k];
					string label = tuple.Item1;
					double rel2 = Gw2CoordinateUtil.NormalizeAngle(tuple.Item2 - facingBearing);
					if (Math.Abs(rel2) > HalfFovRadians)
					{
						continue;
					}
					float x2 = centerX + Gw2CoordinateUtil.BearingToOffsetX(rel2, HalfFovRadians, halfWidth);
					if (!(x2 < 0f) && !(x2 > (float)bounds.Width))
					{
						float ca = EdgeAlpha((int)x2, bounds.Width);
						if (!(ca <= 0.02f))
						{
							SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)x2, barH - 6, 1, 6), new Color(230, 215, 175) * (0.7f * ca));
							SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, label, _font, new Rectangle((int)x2 - 12, barH - 20, 24, 14), new Color(255, 240, 205) * ca, false, (HorizontalAlignment)1, (VerticalAlignment)1);
						}
					}
				}
			}
			int baseIcon = Math.Max(14, barH - 14);
			float lastCaption = float.NegativeInfinity;
			Rectangle iconRect = default(Rectangle);
			foreach (CompassTarget t in _targets)
			{
				double rel = Gw2CoordinateUtil.RelativeBearing(_playerCont, t.ContinentPosition, _facing);
				if (Math.Abs(rel) > HalfFovRadians)
				{
					continue;
				}
				double distCont = Vector2.Distance(_playerCont, t.ContinentPosition);
				double distMeters = (_calibration.IsValid ? _calibration.ToMeters(distCont) : double.NaN);
				if (!double.IsNaN(distMeters) && distMeters > MaxDistanceMeters)
				{
					continue;
				}
				float x = centerX + Gw2CoordinateUtil.BearingToOffsetX(rel, HalfFovRadians, halfWidth);
				if (x < 0f || x > (float)bounds.Width)
				{
					continue;
				}
				float alpha = EdgeAlpha((int)x, bounds.Width);
				if (alpha <= 0.02f)
				{
					continue;
				}
				bool undiscovered = MarkUndiscovered && IsDiscovered != null && !IsDiscovered(t.Id);
				bool grey = Monochrome || undiscovered;
				int iconSize = ScaleIcon(baseIcon, distMeters);
				((Rectangle)(ref iconRect))._002Ector((int)x - iconSize / 2, 1 + (baseIcon - iconSize) / 2, iconSize, iconSize);
				Texture2D tex = _icons?.TextureFor(t.Kind, grey);
				Color tint = Color.get_White() * (undiscovered ? (alpha * 0.85f) : alpha);
				if (tex != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, iconRect, tint);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)x - 2, 0, 4, barH - 10), CompassTarget.ColorFor(t.Kind) * alpha);
				}
				string distStr = ((ShowDistance && t.ShowDistance && !double.IsNaN(distMeters)) ? $"{(int)distMeters}m" : null);
				string caption;
				if (ShowNames && !string.IsNullOrEmpty(t.Label))
				{
					string nm = ((t.Label.Length > 8) ? t.Label.Substring(0, 8) : t.Label);
					caption = ((distStr != null) ? (nm + " " + distStr) : nm);
				}
				else
				{
					caption = distStr;
				}
				if (!string.IsNullOrEmpty(caption))
				{
					float needed = (float)caption.Length * 6.5f;
					if (x - needed / 2f > lastCaption)
					{
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, caption, _font, new Rectangle((int)x - 55, barH - 14, 110, 13), Color.get_White() * (0.9f * alpha), false, (HorizontalAlignment)1, (VerticalAlignment)1);
						lastCaption = x + needed / 2f;
					}
				}
			}
			if (_showDebug && !string.IsNullOrEmpty(DebugText))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, barH, bounds.Width, 14), Color.get_Black() * 0.7f);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, DebugText, _font, new Rectangle(4, barH, bounds.Width - 8, 14), new Color(255, 240, 120), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		private float EdgeAlpha(int x, int width)
		{
			if (EdgeFade <= 0.001f || width <= 0)
			{
				return 1f;
			}
			float fade = Math.Max(1f, (float)width * EdgeFade);
			float d = Math.Min(x, width - x);
			if (d >= fade)
			{
				return 1f;
			}
			return MathHelper.Clamp(d / fade, 0f, 1f);
		}

		private int ScaleIcon(int baseSize, double distMeters)
		{
			if (!ScaleWithDistance || double.IsNaN(distMeters) || ScaleDistance <= 0f)
			{
				return baseSize;
			}
			float t = MathHelper.Clamp((float)(distMeters / (double)ScaleDistance), 0f, 1f);
			float f = MathHelper.Lerp(ScaleNear, ScaleFar, t);
			return Math.Max(6, (int)((float)baseSize * f));
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			((Control)this).DisposeControl();
		}
	}
}
