using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Frtal.Wayfinder.Models;
using Frtal.Wayfinder.Services;
using Frtal.Wayfinder.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.Wayfinder.UI
{
	public class DiscoveryWindow : Panel
	{
		private const int TitleBarHeight = 26;

		private readonly DiscoveryTracker _discovery;

		private readonly MapObjectivesService _objectives;

		private readonly MapIconService _icons;

		private readonly MapTileService _tiles;

		private readonly Label _header;

		private readonly Label _status;

		private readonly FlowPanel _list;

		private readonly DiscoveryMapView _map;

		private readonly StandardButton _listBtn;

		private readonly StandardButton _mapBtn;

		private readonly StandardButton _zoomIn;

		private readonly StandardButton _zoomOut;

		private readonly StandardButton _fitBtn;

		private readonly StandardButton _closeBtn;

		private readonly Checkbox _tilesCheck;

		private readonly HashSet<TargetKind> _collapsedKinds = new HashSet<TargetKind>();

		private const int MinWidth = 420;

		private const int MinHeight = 360;

		private const int GripSize = 30;

		private const int ContentTop = 90;

		private Texture2D _positionMarker;

		private int _builtForMap = -1;

		private bool _dragging;

		private bool _resizing;

		private Point _dragOffset;

		public DiscoveryWindow(DiscoveryTracker discovery, MapObjectivesService objectives, MapIconService icons, MapTileService tiles)
			: this()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Expected O, but got Unknown
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Expected O, but got Unknown
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Expected O, but got Unknown
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Expected O, but got Unknown
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Expected O, but got Unknown
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Expected O, but got Unknown
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Expected O, but got Unknown
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			_discovery = discovery;
			_objectives = objectives;
			_icons = icons;
			_tiles = tiles;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(new Point(760, 680));
			((Panel)this).set_Title("Wayfinder - Discovered objectives   (drag the title bar to move, bottom-right corner to resize)");
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_CanScroll(false);
			((Control)this).set_ZIndex(20);
			((Control)this).set_Visible(false);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(10, 6));
			((Control)val).set_Width(((Control)this).get_Width() - 60);
			((Control)val).set_Height(20);
			_header = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("X");
			((Control)val2).set_Size(new Point(28, 22));
			((Control)val2).set_BasicTooltipText("Close");
			_closeBtn = val2;
			((Control)_closeBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).set_Visible(false);
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Mark all discovered");
			((Control)val3).set_Location(new Point(10, 30));
			((Control)val3).set_Width(160);
			((Control)val3).set_Height(26);
			((Control)val3).set_BasicTooltipText("Marks every objective on this map as already found.");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_discovery.MarkAll(_objectives.Targets);
				Refresh(force: true);
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Clear all");
			((Control)val4).set_Location(new Point(178, 30));
			((Control)val4).set_Width(100);
			((Control)val4).set_Height(26);
			((Control)val4).set_BasicTooltipText("Marks everything on this map as not yet found.");
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_discovery.ClearAll();
				Refresh(force: true);
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("List");
			((Control)val5).set_Location(new Point(10, 60));
			((Control)val5).set_Width(80);
			((Control)val5).set_Height(24);
			_listBtn = val5;
			((Control)_listBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetMode(mapMode: false);
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Map");
			((Control)val6).set_Location(new Point(96, 60));
			((Control)val6).set_Width(80);
			((Control)val6).set_Height(24);
			((Control)val6).set_BasicTooltipText("Click objectives to toggle them. Scroll to zoom, drag to pan.");
			_mapBtn = val6;
			((Control)_mapBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetMode(mapMode: true);
			});
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("-");
			((Control)val7).set_Location(new Point(186, 60));
			((Control)val7).set_Width(30);
			((Control)val7).set_Height(24);
			((Control)val7).set_BasicTooltipText("Zoom out");
			_zoomOut = val7;
			((Control)_zoomOut).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_map.ZoomBy(-1f);
			});
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("+");
			((Control)val8).set_Location(new Point(220, 60));
			((Control)val8).set_Width(30);
			((Control)val8).set_Height(24);
			((Control)val8).set_BasicTooltipText("Zoom in");
			_zoomIn = val8;
			((Control)_zoomIn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_map.ZoomBy(1f);
			});
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Text("Fit");
			((Control)val9).set_Location(new Point(254, 60));
			((Control)val9).set_Width(50);
			((Control)val9).set_Height(24);
			((Control)val9).set_BasicTooltipText("Reset zoom and position");
			_fitBtn = val9;
			((Control)_fitBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_map.ResetView();
			});
			Checkbox val10 = new Checkbox();
			((Control)val10).set_Parent((Container)(object)this);
			val10.set_Text("Map image");
			((Control)val10).set_Location(new Point(312, 63));
			((Control)val10).set_Width(110);
			((Control)val10).set_Height(20);
			val10.set_Checked(true);
			((Control)val10).set_BasicTooltipText("Shows the real world map behind the objectives.\nArenaNet stopped updating these tiles after Living World S4,\nso the newest maps have no imagery.");
			_tilesCheck = val10;
			_tilesCheck.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_map.ShowTiles = e.get_Checked();
			});
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)this);
			((Control)val11).set_Location(new Point(430, 63));
			((Control)val11).set_Width(Math.Max(60, ((Control)this).get_Width() - 440));
			((Control)val11).set_Height(18);
			val11.set_TextColor(new Color(255, 220, 140));
			_status = val11;
			FlowPanel val12 = new FlowPanel();
			((Control)val12).set_Parent((Container)(object)this);
			((Control)val12).set_Location(new Point(6, 90));
			val12.set_FlowDirection((ControlFlowDirection)3);
			val12.set_ControlPadding(new Vector2(0f, 2f));
			val12.set_OuterControlPadding(new Vector2(4f, 4f));
			((Panel)val12).set_CanScroll(true);
			((Panel)val12).set_ShowBorder(true);
			_list = val12;
			DiscoveryMapView discoveryMapView = new DiscoveryMapView(_discovery, _objectives, _icons, _tiles);
			((Control)discoveryMapView).set_Parent((Container)(object)this);
			((Control)discoveryMapView).set_Location(new Point(6, 90));
			((Control)discoveryMapView).set_Visible(false);
			_map = discoveryMapView;
			_map.Changed = UpdateHeaderCount;
			try
			{
				GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					_positionMarker = ProceduralIcon.CreatePositionMarker(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
				_map.PositionMarker = _positionMarker;
			}
			catch (Exception ex)
			{
				Logger.GetLogger<DiscoveryWindow>().Warn(ex, "Could not build the position marker.");
			}
			Relayout();
		}

		private void Relayout()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			int contentW = ((Control)this).get_Width() - 16;
			int contentH = ((Control)this).get_Height() - 90 - 10;
			((Control)_header).set_Width(((Control)this).get_Width() - 60);
			((Control)_status).set_Width(Math.Max(60, ((Control)this).get_Width() - 440));
			((Control)_closeBtn).set_Location(new Point(((Control)this).get_Width() - 36, 4));
			((Control)_list).set_Size(new Point(contentW, contentH));
			((Control)_map).set_Size(new Point(contentW, contentH));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Point rel = ((Control)this).get_RelativeMousePosition();
			if (rel.X >= ((Control)this).get_Width() - 30 && rel.Y >= ((Control)this).get_Height() - 30)
			{
				_resizing = true;
			}
			else if (rel.Y <= 26)
			{
				_dragging = true;
				_dragOffset = rel;
			}
		}

		private void OnGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			_dragging = false;
			_resizing = false;
			_map?.EndDrag();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			Screen screen = GameService.Graphics.get_SpriteScreen();
			Point i = GameService.Input.get_Mouse().get_Position();
			if (_resizing)
			{
				int w = MathHelper.Clamp(i.X - ((Control)this).get_Location().X, 420, Math.Max(420, ((Control)screen).get_Width() - ((Control)this).get_Location().X));
				int h = MathHelper.Clamp(i.Y - ((Control)this).get_Location().Y, 360, Math.Max(360, ((Control)screen).get_Height() - ((Control)this).get_Location().Y));
				if (w != ((Control)this).get_Width() || h != ((Control)this).get_Height())
				{
					((Control)this).set_Size(new Point(w, h));
					Relayout();
					if (((Control)_list).get_Visible())
					{
						Refresh(force: true);
					}
				}
			}
			else if (_dragging)
			{
				int x = MathHelper.Clamp(i.X - _dragOffset.X, 0, Math.Max(0, ((Control)screen).get_Width() - ((Control)this).get_Width()));
				int y = MathHelper.Clamp(i.Y - _dragOffset.Y, 0, Math.Max(0, ((Control)screen).get_Height() - ((Control)this).get_Height()));
				((Control)this).set_Location(new Point(x, y));
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, bounds.Width, bounds.Height), new Color(24, 25, 29));
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
			Texture2D pixel = Textures.get_Pixel();
			Color bright = default(Color);
			((Color)(ref bright))._002Ector(255, 220, 140);
			Color shadow = new Color(0, 0, 0) * 0.6f;
			for (int row = 0; row < 3; row++)
			{
				for (int col = 0; col < 3 - row; col++)
				{
					int x = bounds.Width - 7 - 4 - col * 7;
					int y = bounds.Height - 7 - 4 - row * 7;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(x + 1, y + 1, 4, 4), shadow);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(x, y, 4, 4), bright);
				}
			}
		}

		private void SetMode(bool mapMode)
		{
			((Control)_list).set_Visible(!mapMode);
			((Control)_map).set_Visible(mapMode);
		}

		public void Toggle()
		{
			((Control)this).set_Visible(!((Control)this).get_Visible());
			if (((Control)this).get_Visible())
			{
				CenterOnScreen();
				Refresh(force: true);
			}
		}

		private void CenterOnScreen()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Screen screen = GameService.Graphics.get_SpriteScreen();
			((Control)this).set_Location(new Point((((Control)screen).get_Width() - ((Control)this).get_Width()) / 2, Math.Max(0, (((Control)screen).get_Height() - ((Control)this).get_Height()) / 2)));
		}

		public void Refresh(bool force = false)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible())
			{
				return;
			}
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			if (!force && mapId == _builtForMap)
			{
				return;
			}
			_builtForMap = mapId;
			((Container)_list).ClearChildren();
			UpdateHeaderCount();
			IReadOnlyList<CompassTarget> targets = _objectives.Targets;
			if (targets.Count == 0)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_list);
				((Control)val).set_Width(((Control)_list).get_Width() - 30);
				((Control)val).set_Height(40);
				val.set_Text("No objectives loaded for this map yet.");
				return;
			}
			Dictionary<TargetKind, List<CompassTarget>> byKind = new Dictionary<TargetKind, List<CompassTarget>>();
			foreach (CompassTarget t in targets)
			{
				if (!byKind.TryGetValue(t.Kind, out var list))
				{
					list = new List<CompassTarget>();
					byKind[t.Kind] = list;
				}
				list.Add(t);
			}
			foreach (KeyValuePair<TargetKind, List<CompassTarget>> kv in byKind)
			{
				BuildSection(kv.Key, kv.Value);
			}
		}

		private void BuildSection(TargetKind kind, List<CompassTarget> items)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Expected O, but got Unknown
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			int rowW = ((Control)_list).get_Width() - 34;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_list);
			((Control)val).set_Width(rowW);
			((Control)val).set_Height(26);
			((Control)val).set_BasicTooltipText("Click to expand or collapse.");
			Panel header = val;
			AsyncTexture2D iconTex = _icons?.For(kind);
			if (iconTex != null)
			{
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)header);
				val2.set_Texture(iconTex);
				((Control)val2).set_Location(new Point(2, 3));
				((Control)val2).set_Size(new Point(20, 20));
			}
			int seenHere = 0;
			foreach (CompassTarget t2 in items)
			{
				if (_discovery.IsSeen(t2.Id))
				{
					seenHere++;
				}
			}
			bool collapsed = _collapsedKinds.Contains(kind);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			((Control)val3).set_Location(new Point(26, 4));
			((Control)val3).set_Width(rowW - 30);
			((Control)val3).set_Height(20);
			val3.set_Text(string.Format("{0}  {1}  ({2}/{3})", collapsed ? "+" : "-", kind, seenHere, items.Count));
			val3.set_TextColor(new Color(255, 220, 140));
			Label label = val3;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)_list);
			((Control)val4).set_Width(rowW);
			val4.set_FlowDirection((ControlFlowDirection)3);
			val4.set_ControlPadding(new Vector2(0f, 1f));
			val4.set_OuterControlPadding(new Vector2(14f, 0f));
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Visible(!collapsed);
			FlowPanel content = val4;
			foreach (CompassTarget t in items)
			{
				CompassTarget target = t;
				Checkbox val5 = new Checkbox();
				((Control)val5).set_Parent((Container)(object)content);
				((Control)val5).set_Width(rowW - 20);
				((Control)val5).set_Height(22);
				val5.set_Text(string.IsNullOrEmpty(target.Label) ? target.Kind.ToString() : target.Label);
				val5.set_Checked(_discovery.IsSeen(target.Id));
				val5.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
				{
					_discovery.SetSeen(target.Id, e.get_Checked());
					UpdateHeaderCount();
				});
			}
			((Control)header).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleSection();
			});
			((Control)label).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleSection();
			});
			void ToggleSection()
			{
				bool nowCollapsed = !_collapsedKinds.Contains(kind);
				if (nowCollapsed)
				{
					_collapsedKinds.Add(kind);
				}
				else
				{
					_collapsedKinds.Remove(kind);
				}
				((Control)content).set_Visible(!nowCollapsed);
				label.set_Text(string.Format("{0}  {1}  ({2}/{3})", nowCollapsed ? "+" : "-", kind, seenHere, items.Count));
				((Control)_list).Invalidate();
			}
		}

		private void UpdateHeaderCount()
		{
			IReadOnlyList<CompassTarget> targets = _objectives.Targets;
			int seen = 0;
			foreach (CompassTarget t in targets)
			{
				if (_discovery.IsSeen(t.Id))
				{
					seen++;
				}
			}
			string mapName = (string.IsNullOrEmpty(_objectives.MapName) ? $"map {_builtForMap}" : _objectives.MapName);
			_header.set_Text($"{mapName} - {seen} of {targets.Count} marked as discovered");
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			Texture2D positionMarker = _positionMarker;
			if (positionMarker != null)
			{
				((GraphicsResource)positionMarker).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
