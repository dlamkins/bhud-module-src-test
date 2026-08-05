using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Frtal.Wayfinder.Models;
using Frtal.Wayfinder.Services;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Frtal.Wayfinder.UI
{
	public class DiscoveryMapView : Control
	{
		private const int PlotPadding = 10;

		private const int BaseIconSize = 26;

		private const float MinZoom = 0.5f;

		private const float MaxZoom = 8f;

		private const int DragSlop = 5;

		private readonly DiscoveryTracker _discovery;

		private readonly MapObjectivesService _objectives;

		private readonly MapIconService _icons;

		private readonly MapTileService _tiles;

		private readonly List<(CompassTarget Target, Rectangle Rect)> _hitAreas = new List<(CompassTarget, Rectangle)>();

		private float _zoom = 1f;

		private Vector2 _pan = Vector2.get_Zero();

		private bool _isDown;

		private bool _panning;

		private Point _pressMouse;

		private Vector2 _pressPan;

		public bool ShowTiles { get; set; } = true;


		public Texture2D PositionMarker { get; set; }

		public Action Changed { get; set; }

		public DiscoveryMapView(DiscoveryTracker discovery, MapObjectivesService objectives, MapIconService icons, MapTileService tiles)
			: this()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			_discovery = discovery;
			_objectives = objectives;
			_icons = icons;
			_tiles = tiles;
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)12;
		}

		public void ResetView()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_zoom = 1f;
			_pan = Vector2.get_Zero();
		}

		public void ZoomBy(float steps)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Point center = default(Point);
			((Point)(ref center))._002Ector(((Control)this).get_Width() / 2, ((Control)this).get_Height() / 2);
			ApplyZoom(steps, center);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			_isDown = true;
			_panning = false;
			_pressMouse = GameService.Input.get_Mouse().get_Position();
			_pressPan = _pan;
		}

		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseWheelScrolled(e);
			MouseState state = GameService.Input.get_Mouse().get_State();
			int delta = ((MouseState)(ref state)).get_ScrollWheelValue();
			if (delta != 0)
			{
				ApplyZoom((float)delta / 120f, ((Control)this).get_RelativeMousePosition());
			}
		}

		internal void EndDrag()
		{
			_isDown = false;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			_isDown = false;
			if (_panning)
			{
				_panning = false;
				return;
			}
			Point mouse = ((Control)this).get_RelativeMousePosition();
			for (int i = _hitAreas.Count - 1; i >= 0; i--)
			{
				(CompassTarget, Rectangle) tuple = _hitAreas[i];
				if (((Rectangle)(ref tuple.Item2)).Contains(mouse))
				{
					CompassTarget t = _hitAreas[i].Target;
					_discovery.SetSeen(t.Id, !_discovery.IsSeen(t.Id));
					Changed?.Invoke();
					break;
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			_tiles?.ProcessDownloads();
			if (_isDown)
			{
				Point now = GameService.Input.get_Mouse().get_Position();
				Vector2 delta = default(Vector2);
				((Vector2)(ref delta))._002Ector((float)(now.X - _pressMouse.X), (float)(now.Y - _pressMouse.Y));
				if (_panning || ((Vector2)(ref delta)).Length() > 5f)
				{
					_panning = true;
					_pan = _pressPan + delta;
				}
			}
		}

		private void DrawTiles(SpriteBatch spriteBatch, Rectangle bounds, Vector2 mapCenter, Vector2 viewCent, float scale)
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			if (_tiles == null || _objectives.ContinentId < 0)
			{
				return;
			}
			int continentId = _objectives.ContinentId;
			int floor = _objectives.Floor;
			int maxZoom = MapTileService.MaxZoomFor(continentId);
			int zoom = (int)Math.Round((double)maxZoom + Math.Log(Math.Max(scale, 1E-06), 2.0));
			zoom = MathHelper.Clamp(zoom, 1, maxZoom);
			double unitsPerTile = MapTileService.ContinentUnitsPerTile(continentId, zoom);
			if (unitsPerTile <= 0.0)
			{
				return;
			}
			float contLeft = mapCenter.X + (0f - _pan.X - viewCent.X) / scale;
			float contTop = mapCenter.Y + (0f - _pan.Y - viewCent.Y) / scale;
			float contRight = mapCenter.X + ((float)bounds.Width - _pan.X - viewCent.X) / scale;
			float num = mapCenter.Y + ((float)bounds.Height - _pan.Y - viewCent.Y) / scale;
			int tx2 = (int)Math.Floor((double)contLeft / unitsPerTile);
			int ty2 = (int)Math.Floor((double)contTop / unitsPerTile);
			int tx3 = (int)Math.Floor((double)contRight / unitsPerTile);
			int ty3 = (int)Math.Floor((double)num / unitsPerTile);
			if ((tx3 - tx2 + 1) * (ty3 - ty2 + 1) > 240)
			{
				return;
			}
			Vector2 topLeft = default(Vector2);
			Rectangle dest = default(Rectangle);
			for (int tx = tx2; tx <= tx3; tx++)
			{
				for (int ty = ty2; ty <= ty3; ty++)
				{
					if (tx >= 0 && ty >= 0)
					{
						Texture2D tex = _tiles.GetTile(continentId, floor, zoom, tx, ty);
						if (tex != null)
						{
							((Vector2)(ref topLeft))._002Ector((float)((double)tx * unitsPerTile), (float)((double)ty * unitsPerTile));
							Vector2 a = viewCent + _pan + (topLeft - mapCenter) * scale;
							Vector2 b = viewCent + _pan + (topLeft + new Vector2((float)unitsPerTile) - mapCenter) * scale;
							((Rectangle)(ref dest))._002Ector((int)Math.Floor(a.X), (int)Math.Floor(a.Y), (int)Math.Ceiling(b.X - a.X) + 1, (int)Math.Ceiling(b.Y - a.Y) + 1);
							SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, dest, Color.get_White());
						}
					}
				}
			}
		}

		private void ApplyZoom(float steps, Point anchor)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			if (!(Math.Abs(steps) < 0.01f))
			{
				float oldZoom = _zoom;
				float newZoom = MathHelper.Clamp(oldZoom * (float)Math.Pow(1.2, steps), 0.5f, 8f);
				if (!(Math.Abs(newZoom - oldZoom) < 0.0001f))
				{
					Vector2 center = default(Vector2);
					((Vector2)(ref center))._002Ector((float)((Control)this).get_Width() / 2f, (float)((Control)this).get_Height() / 2f);
					Vector2 a = default(Vector2);
					((Vector2)(ref a))._002Ector((float)anchor.X, (float)anchor.Y);
					_pan = a - center - (a - center - _pan) * (newZoom / oldZoom);
					_zoom = newZoom;
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			IReadOnlyList<CompassTarget> targets = _objectives.Targets;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, 0, bounds.Width, bounds.Height), new Color(16, 17, 20));
			_hitAreas.Clear();
			if (targets.Count == 0)
			{
				return;
			}
			Vector2 min = default(Vector2);
			((Vector2)(ref min))._002Ector(float.MaxValue, float.MaxValue);
			Vector2 max = default(Vector2);
			((Vector2)(ref max))._002Ector(float.MinValue, float.MinValue);
			foreach (CompassTarget t2 in targets)
			{
				min = Vector2.Min(min, t2.ContinentPosition);
				max = Vector2.Max(max, t2.ContinentPosition);
			}
			float spanX = Math.Max(1f, max.X - min.X);
			float spanY = Math.Max(1f, max.Y - min.Y);
			int usableW = bounds.Width - 20;
			int usableH = bounds.Height - 20;
			if (usableW <= 0 || usableH <= 0)
			{
				return;
			}
			float scale = Math.Min((float)usableW / spanX, (float)usableH / spanY) * _zoom;
			Vector2 mapCenter = (min + max) * 0.5f;
			Vector2 viewCent = default(Vector2);
			((Vector2)(ref viewCent))._002Ector((float)bounds.Width / 2f, (float)bounds.Height / 2f);
			if (ShowTiles)
			{
				DrawTiles(spriteBatch, bounds, mapCenter, viewCent, scale);
			}
			int iconSize = (int)MathHelper.Clamp(26f * _zoom, 14f, 72f);
			Rectangle rect = default(Rectangle);
			Color ring = default(Color);
			foreach (CompassTarget t in targets)
			{
				Vector2 p = viewCent + _pan + (t.ContinentPosition - mapCenter) * scale;
				((Rectangle)(ref rect))._002Ector((int)p.X - iconSize / 2, (int)p.Y - iconSize / 2, iconSize, iconSize);
				if (((Rectangle)(ref rect)).get_Right() >= 0 && ((Rectangle)(ref rect)).get_Bottom() >= 0 && rect.X <= bounds.Width && rect.Y <= bounds.Height)
				{
					_hitAreas.Add((t, rect));
					bool seen = _discovery.IsSeen(t.Id);
					Texture2D tex = _icons?.TextureFor(t.Kind, monochrome: false);
					if (!seen)
					{
						Color halo = new Color(0, 0, 0) * 0.55f;
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(rect.X - 3, rect.Y - 3, rect.Width + 6, rect.Height + 6), halo);
					}
					Color tint = (seen ? (Color.get_White() * 0.35f) : Color.get_White());
					if (tex != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, rect, tint);
					}
					else
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, rect, CompassTarget.ColorFor(t.Kind) * (seen ? 0.35f : 1f));
					}
					if (!seen)
					{
						((Color)(ref ring))._002Ector(255, 215, 120);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(rect.X - 3, rect.Y - 3, rect.Width + 6, 2), ring);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(rect.X - 3, ((Rectangle)(ref rect)).get_Bottom() + 1, rect.Width + 6, 2), ring);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(rect.X - 3, rect.Y - 3, 2, rect.Height + 6), ring);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(((Rectangle)(ref rect)).get_Right() + 1, rect.Y - 3, 2, rect.Height + 6), ring);
					}
				}
			}
			DrawPlayerMarker(spriteBatch, bounds, pixel, mapCenter, viewCent, scale);
		}

		private void DrawPlayerMarker(SpriteBatch spriteBatch, Rectangle bounds, Texture2D pixel, Vector2 mapCenter, Vector2 viewCent, float scale)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			Coordinates2 mp = GameService.Gw2Mumble.get_UI().get_MapPosition();
			Vector2 me = default(Vector2);
			((Vector2)(ref me))._002Ector((float)((Coordinates2)(ref mp)).get_X(), (float)((Coordinates2)(ref mp)).get_Y());
			Vector2 p = viewCent + _pan + (me - mapCenter) * scale;
			if (!(p.X < 0f) && !(p.Y < 0f) && !(p.X > (float)bounds.Width) && !(p.Y > (float)bounds.Height))
			{
				if (PositionMarker == null)
				{
					Color c = default(Color);
					((Color)(ref c))._002Ector(255, 230, 120);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)p.X - 8, (int)p.Y - 1, 17, 3), c);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)p.X - 1, (int)p.Y - 8, 3, 17), c);
					return;
				}
				Vector3 fwd = GameService.Gw2Mumble.get_PlayerCharacter().get_Forward();
				Vector2 facing = default(Vector2);
				((Vector2)(ref facing))._002Ector(fwd.X, 0f - fwd.Y);
				float angle = ((((Vector2)(ref facing)).LengthSquared() > 0.0001f) ? ((float)Math.Atan2(facing.X, 0f - facing.Y)) : 0f);
				Rectangle dest = default(Rectangle);
				((Rectangle)(ref dest))._002Ector((int)p.X, (int)p.Y, 30, 30);
				Vector2 origin = default(Vector2);
				((Vector2)(ref origin))._002Ector((float)PositionMarker.get_Width() / 2f, (float)PositionMarker.get_Height() / 2f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PositionMarker, dest, (Rectangle?)null, Color.get_White(), angle, origin, (SpriteEffects)0);
			}
		}
	}
}
