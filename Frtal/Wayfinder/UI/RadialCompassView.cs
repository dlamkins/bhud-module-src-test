using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Frtal.Wayfinder.Models;
using Frtal.Wayfinder.Services;
using Frtal.Wayfinder.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Frtal.Wayfinder.UI
{
	public class RadialCompassView : Control
	{
		private static readonly (string Label, Vector2 ContinentDir)[] Cardinals = new(string, Vector2)[4]
		{
			("N", new Vector2(0f, -1f)),
			("E", new Vector2(1f, 0f)),
			("S", new Vector2(0f, 1f)),
			("W", new Vector2(-1f, 0f))
		};

		private readonly BitmapFont _font = GameService.Content.get_DefaultFont14();

		private readonly MapIconService _icons;

		private Vector2 _playerCont;

		private Vector3 _playerWorld;

		private MapCalibration _calibration;

		private IReadOnlyList<CompassTarget> _targets = new List<CompassTarget>();

		private float _axisSignX = 1f;

		private float _axisSignY = 1f;

		public float RadiusMeters { get; set; } = 10f;


		public float VerticalOffset { get; set; }

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


		public float IconOpacity { get; set; } = 1f;


		public int IconSize { get; set; } = 28;


		public RadialCompassView(MapIconService icons)
			: this()
		{
			_icons = icons;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_ZIndex(10);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void UpdateContext(Vector2 playerCont, Vector3 playerWorld, MapCalibration calibration, IReadOnlyList<CompassTarget> targets, float axisSignX, float axisSignY)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_playerCont = playerCont;
			_playerWorld = playerWorld;
			_calibration = calibration;
			_targets = targets ?? new List<CompassTarget>();
			_axisSignX = axisSignX;
			_axisSignY = axisSignY;
		}

		public void ApplyLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			Screen screen = GameService.Graphics.get_SpriteScreen();
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_Size(new Point(((Control)screen).get_Width(), ((Control)screen).get_Height()));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			int w = bounds.Width;
			int h = bounds.Height;
			Texture2D pixel = Textures.get_Pixel();
			DrawRing(spriteBatch, pixel, w, h);
			if (ShowCardinals)
			{
				(string, Vector2)[] cardinals = Cardinals;
				for (int i = 0; i < cardinals.Length; i++)
				{
					var (label, contDir2) = cardinals[i];
					if (TryRingPoint(contDir2, w, h, out var p2, out var _))
					{
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, label, _font, new Rectangle((int)p2.X - 12, (int)p2.Y - 8, 24, 16), Color.get_White() * 0.9f, false, (HorizontalAlignment)1, (VerticalAlignment)1);
					}
				}
			}
			Rectangle rect = default(Rectangle);
			foreach (CompassTarget t in _targets)
			{
				Vector2 contDir = t.ContinentPosition - _playerCont;
				if (((Vector2)(ref contDir)).LengthSquared() < 0.0001f)
				{
					continue;
				}
				double distCont = ((Vector2)(ref contDir)).Length();
				double distMeters = (_calibration.IsValid ? _calibration.ToMeters(distCont) : double.NaN);
				if ((double.IsNaN(distMeters) || !(distMeters > MaxDistanceMeters)) && TryRingPoint(contDir, w, h, out var p, out var depth))
				{
					int size = IconSizeFor(depth, distMeters);
					((Rectangle)(ref rect))._002Ector((int)p.X - size / 2, (int)p.Y - size / 2, size, size);
					Texture2D tex = _icons?.TextureFor(t.Kind, Monochrome);
					if (tex != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, rect, Color.get_White() * IconOpacity);
					}
					else
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, rect, CompassTarget.ColorFor(t.Kind) * IconOpacity);
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
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, caption, _font, new Rectangle((int)p.X - 55, (int)p.Y + size / 2, 110, 13), Color.get_White() * 0.9f, false, (HorizontalAlignment)1, (VerticalAlignment)1);
					}
				}
			}
		}

		private bool TryRingPoint(Vector2 continentDir, int w, int h, out Vector2 screen, out float depth)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			screen = Vector2.get_Zero();
			depth = 0f;
			Vector2 dir = WorldProjection.ContinentDirToWorld(continentDir, _axisSignX, _axisSignY);
			if (((Vector2)(ref dir)).LengthSquared() < 0.0001f)
			{
				return false;
			}
			((Vector2)(ref dir)).Normalize();
			return WorldProjection.TryProject(new Vector3(_playerWorld.X + dir.X * RadiusMeters, _playerWorld.Y + dir.Y * RadiusMeters, _playerWorld.Z + VerticalOffset), w, h, out screen, out depth);
		}

		private void DrawRing(SpriteBatch spriteBatch, Texture2D pixel, int w, int h)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			if (BackgroundOpacity <= 0.001f)
			{
				return;
			}
			Color color = Color.get_Black() * BackgroundOpacity;
			Vector2 dir = default(Vector2);
			for (int i = 0; i < 96; i++)
			{
				double a = (double)i * (Math.PI / 48.0);
				((Vector2)(ref dir))._002Ector((float)Math.Sin(a), (float)(0.0 - Math.Cos(a)));
				if (TryRingPoint(dir, w, h, out var p, out var depth))
				{
					int s = Math.Max(2, (int)(220f / Math.Max(1f, depth)));
					s = Math.Min(s, 8);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)p.X - s / 2, (int)p.Y - s / 2, s, s), color);
				}
			}
		}

		private int IconSizeFor(float depth, double distMeters)
		{
			float perspective = MathHelper.Clamp(260f / Math.Max(1f, depth), 0.45f, 2.2f);
			float size = (float)IconSize * perspective;
			if (ScaleWithDistance && !double.IsNaN(distMeters) && ScaleDistance > 0f)
			{
				float t = MathHelper.Clamp((float)(distMeters / (double)ScaleDistance), 0f, 1f);
				size *= MathHelper.Lerp(ScaleNear, ScaleFar, t);
			}
			return Math.Max(8, (int)size);
		}
	}
}
