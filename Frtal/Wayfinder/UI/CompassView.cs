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
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			int barH = _barHeight;
			float halfWidth = (float)bounds.Width / 2f;
			float centerX = (float)bounds.Width / 2f;
			Texture2D pixel = Textures.get_Pixel();
			if (BackgroundOpacity > 0.001f)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, 0, bounds.Width, barH), Color.get_Black() * BackgroundOpacity);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)centerX - 1, 0, 2, barH), Color.get_White() * 0.8f);
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
				for (int i = 0; i < cardinals.Length; i++)
				{
					(string, double) tuple = cardinals[i];
					string label = tuple.Item1;
					double rel2 = Gw2CoordinateUtil.NormalizeAngle(tuple.Item2 - facingBearing);
					if (!(Math.Abs(rel2) > HalfFovRadians))
					{
						float x2 = centerX + Gw2CoordinateUtil.BearingToOffsetX(rel2, HalfFovRadians, halfWidth);
						if (!(x2 < 0f) && !(x2 > (float)bounds.Width))
						{
							SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)x2, barH - 6, 1, 6), Color.get_White() * 0.6f);
							SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, label, _font, new Rectangle((int)x2 - 12, barH - 20, 24, 14), Color.get_White() * 0.9f, false, (HorizontalAlignment)1, (VerticalAlignment)1);
						}
					}
				}
			}
			int baseIcon = Math.Max(14, barH - 14);
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
				if (!(x < 0f) && !(x > (float)bounds.Width))
				{
					int iconSize = ScaleIcon(baseIcon, distMeters);
					((Rectangle)(ref iconRect))._002Ector((int)x - iconSize / 2, 1 + (baseIcon - iconSize) / 2, iconSize, iconSize);
					Texture2D tex = _icons?.TextureFor(t.Kind, Monochrome);
					if (tex != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, iconRect, Color.get_White());
					}
					else
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)x - 2, 0, 4, barH - 10), CompassTarget.ColorFor(t.Kind));
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
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, caption, _font, new Rectangle((int)x - 55, barH - 14, 110, 13), Color.get_White() * 0.9f, false, (HorizontalAlignment)1, (VerticalAlignment)1);
					}
				}
			}
			if (_showDebug && !string.IsNullOrEmpty(DebugText))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, barH, bounds.Width, 14), Color.get_Black() * 0.7f);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, DebugText, _font, new Rectangle(4, barH, bounds.Width - 8, 14), new Color(255, 240, 120), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
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
