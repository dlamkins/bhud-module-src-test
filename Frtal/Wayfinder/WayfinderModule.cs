using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Frtal.Wayfinder.Models;
using Frtal.Wayfinder.Services;
using Frtal.Wayfinder.UI;
using Frtal.Wayfinder.Util;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.Wayfinder
{
	[Export(typeof(Module))]
	public class WayfinderModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<WayfinderModule>();

		private const float AxisSignX = 1f;

		private const float AxisSignY = -1f;

		private ModuleSettings _settings;

		private MapObjectivesService _objectives;

		private DiscoveryTracker _discovery;

		private MapIconService _icons;

		private MapTileService _tiles;

		private DiscoveryWindow _discoveryWindow;

		private CompassView _view;

		private RadialCompassView _radialView;

		private MapOverlay _mapOverlay;

		private CornerIcon _cornerIcon;

		private Texture2D _iconTexture;

		private Texture2D _iconHoverTexture;

		private bool _enabled = true;

		private Vector2 _smoothedFacing = new Vector2(0f, -1f);

		private const double RebuildInterval = 0.2;

		private const double ProximityInterval = 0.25;

		private List<CompassTarget> _targetCache = new List<CompassTarget>();

		private double _sinceRebuild = double.MaxValue;

		private double _sinceProximity = double.MaxValue;

		private int _cachedMapId = -1;

		private double _peekRemaining;

		private double _peekTotal;

		[ImportingConstructor]
		public WayfinderModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)


		protected override void DefineSettings(SettingCollection settings)
		{
			_settings = new ModuleSettings(settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new WayfinderSettingsView(_settings);
		}

		protected override async Task LoadAsync()
		{
			string dir = base.ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("wayfinder");
			_objectives = new MapObjectivesService(base.ModuleParameters.get_Gw2ApiManager());
			_discovery = new DiscoveryTracker(dir);
			_icons = new MapIconService(base.ModuleParameters.get_Gw2ApiManager());
			await _icons.LoadAsync();
			_view = new CompassView(_icons);
			_radialView = new RadialCompassView(_icons);
			_mapOverlay = new MapOverlay();
			_tiles = new MapTileService();
			_discoveryWindow = new DiscoveryWindow(_discovery, _objectives, _icons, _tiles);
			_settings.OpenDiscovery = delegate
			{
				_discoveryWindow.Toggle();
			};
			_view.PositionChanged = delegate(Point p)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				_settings.PositionX.set_Value(p.X);
				_settings.PositionY.set_Value(p.Y);
			};
			RegisterHotkeys();
			try
			{
				GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					_iconTexture = ProceduralIcon.CreateCompassIcon(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
					_iconHoverTexture = ProceduralIcon.CreateCompassIcon(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), 64, hover: true);
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
				WayfinderModule wayfinderModule = this;
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(_iconTexture));
				val.set_HoverIcon(AsyncTexture2D.op_Implicit(_iconHoverTexture));
				((Control)val).set_BasicTooltipText("Wayfinder");
				wayfinderModule._cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_enabled = !_enabled;
				});
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to create the corner icon.");
			}
			await Task.CompletedTask;
		}

		private void RegisterHotkeys()
		{
			_settings.KeyToggleCompass.get_Value().set_Enabled(true);
			_settings.KeyToggleCompass.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				_enabled = !_enabled;
			});
			_settings.KeyToggleMode.get_Value().set_Enabled(true);
			_settings.KeyToggleMode.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				_settings.RadialMode.set_Value(!_settings.RadialMode.get_Value());
			});
			_settings.KeyPeek.get_Value().set_Enabled(true);
			_settings.KeyPeek.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				_peekTotal = Math.Max(1, _settings.PeekSeconds.get_Value());
				_peekRemaining = _peekTotal;
			});
			_settings.KeyDiscovery.get_Value().set_Enabled(true);
			_settings.KeyDiscovery.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				_discoveryWindow?.Toggle();
			});
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_063e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService mumble = GameService.Gw2Mumble;
			if (_view == null)
			{
				return;
			}
			if (_peekRemaining > 0.0)
			{
				_peekRemaining = Math.Max(0.0, _peekRemaining - gameTime.get_ElapsedGameTime().TotalSeconds);
			}
			float peekAlpha = PeekAlpha();
			bool peeking = peekAlpha > 0.001f;
			if ((!_enabled && !peeking) || !mumble.get_IsAvailable())
			{
				((Control)_view).set_Visible(false);
				((Control)_radialView).set_Visible(false);
				return;
			}
			int mapId = mumble.get_CurrentMap().get_Id();
			_objectives.Update(mapId);
			_discovery.SetMap(mapId);
			MapCalibration cal = _objectives.Calibration;
			Coordinates2 mp = mumble.get_UI().get_MapPosition();
			Vector2 playerCont = default(Vector2);
			((Vector2)(ref playerCont))._002Ector((float)((Coordinates2)(ref mp)).get_X(), (float)((Coordinates2)(ref mp)).get_Y());
			Vector3 world = mumble.get_PlayerCharacter().get_Position();
			Vector3 fwd = mumble.get_PlayerCamera().get_Forward();
			Vector2 facing = default(Vector2);
			((Vector2)(ref facing))._002Ector(1f * fwd.X, -1f * fwd.Y);
			facing = SmoothFacing(facing);
			double dt = gameTime.get_ElapsedGameTime().TotalSeconds;
			_sinceProximity += dt;
			_sinceRebuild += dt;
			if (_sinceProximity >= 0.25)
			{
				_sinceProximity = 0.0;
				float thresholdCont = (cal.IsValid ? ((float)((double)_settings.DiscoveryThresholdMeters.get_Value() / cal.MetersPerContinent)) : ((float)_settings.DiscoveryThresholdMeters.get_Value()));
				_discovery.MarkNearby(playerCont, _objectives.Targets, thresholdCont);
			}
			if (_sinceRebuild >= 0.2 || mapId != _cachedMapId)
			{
				_sinceRebuild = 0.0;
				_cachedMapId = mapId;
				_targetCache = BuildTargets(playerCont);
			}
			List<CompassTarget> targets = _targetCache;
			bool hudVisible = !mumble.get_UI().get_IsMapOpen() && !mumble.get_UI().get_IsTextInputFocused();
			bool radial = _settings.RadialMode.get_Value();
			float bgOpacity = (float)_settings.BackgroundOpacity.get_Value() / 100f;
			float opacity = (float)_settings.OpacityPercent.get_Value() / 100f;
			if (!_enabled || peeking)
			{
				opacity *= peekAlpha;
			}
			bool mono = _settings.MonochromeIcons.get_Value();
			bool cardinals = _settings.ShowCardinals.get_Value();
			float scaleNear = (float)_settings.IconScaleNear.get_Value() / 100f;
			float scaleFar = (float)_settings.IconScaleFar.get_Value() / 100f;
			float scaleDist = _settings.IconScaleDistance.get_Value();
			if (mono)
			{
				_icons.EnsureMonochrome();
			}
			_view.HalfFovRadians = ComputeHalfFov();
			_view.MaxDistanceMeters = _settings.MaxDistanceMeters.get_Value();
			_view.ShowDistance = _settings.ShowDistance.get_Value();
			_view.ShowNames = _settings.ShowNames.get_Value();
			_view.ScaleWithDistance = _settings.ScaleWithDistance.get_Value();
			_view.ScaleNear = scaleNear;
			_view.ScaleFar = scaleFar;
			_view.ScaleDistance = scaleDist;
			_view.ShowCardinals = cardinals;
			_view.Monochrome = mono;
			_view.MarkUndiscovered = _settings.GreyUndiscovered.get_Value();
			_view.IsDiscovered = (string id) => _discovery.IsSeen(id);
			_view.EdgeFade = (float)_settings.EdgeFadePercent.get_Value() / 100f;
			_view.BackgroundOpacity = bgOpacity;
			_view.DragEnabled = _settings.DragMode.get_Value() && !radial;
			((Control)_view).set_Opacity(opacity);
			_view.ApplyLayout((float)_settings.WidthPercent.get_Value() / 100f, (float)_settings.ScalePercent.get_Value() / 100f, _settings.Debug.get_Value());
			ApplyCompassPosition();
			_view.UpdateContext(playerCont, facing, cal, targets);
			_view.DebugText = (_settings.Debug.get_Value() ? BuildDebugText(mapId, playerCont, cal, targets, facing) : null);
			((Control)_view).set_Visible(hudVisible && !radial);
			_radialView.RadiusMeters = _settings.RadialRadius.get_Value();
			_radialView.VerticalOffset = _settings.RadialVerticalOffset.get_Value();
			_radialView.IconSize = _settings.RadialIconSize.get_Value();
			_radialView.MaxDistanceMeters = _settings.MaxDistanceMeters.get_Value();
			_radialView.ShowDistance = _settings.ShowDistance.get_Value();
			_radialView.ShowNames = _settings.ShowNames.get_Value();
			_radialView.ScaleWithDistance = _settings.ScaleWithDistance.get_Value();
			_radialView.ScaleNear = scaleNear;
			_radialView.ScaleFar = scaleFar;
			_radialView.ScaleDistance = scaleDist;
			_radialView.ShowCardinals = cardinals;
			_radialView.Monochrome = mono;
			_radialView.IconOpacity = (float)_settings.RadialIconOpacity.get_Value() / 100f;
			_radialView.BackgroundOpacity = bgOpacity;
			((Control)_radialView).set_Opacity(opacity);
			_radialView.ApplyLayout();
			_radialView.UpdateContext(playerCont, world, cal, targets, 1f, -1f);
			((Control)_radialView).set_Visible(hudVisible && radial);
			UpdateMapOverlay();
			_discoveryWindow?.Refresh();
		}

		private float PeekAlpha()
		{
			if (_peekRemaining <= 0.0 || _peekTotal <= 0.0)
			{
				if (!_enabled)
				{
					return 0f;
				}
				return 1f;
			}
			double elapsed = _peekTotal - _peekRemaining;
			if (elapsed < 0.6)
			{
				return (float)(elapsed / 0.6);
			}
			if (_peekRemaining < 0.6)
			{
				return (float)(_peekRemaining / 0.6);
			}
			return 1f;
		}

		private Vector2 SmoothFacing(Vector2 target)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			if (((Vector2)(ref target)).LengthSquared() < 0.0001f)
			{
				return _smoothedFacing;
			}
			float amount = MathHelper.Clamp((float)_settings.Smoothing.get_Value() / 100f, 0f, 0.95f);
			if (amount <= 0.001f)
			{
				_smoothedFacing = target;
				return target;
			}
			float t = MathHelper.Clamp(1f - amount, 0.05f, 1f);
			_smoothedFacing = Vector2.Lerp(_smoothedFacing, target, t);
			if (((Vector2)(ref _smoothedFacing)).LengthSquared() < 0.0001f)
			{
				_smoothedFacing = target;
			}
			return _smoothedFacing;
		}

		private void ApplyCompassPosition()
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			if (!_view.IsDragging)
			{
				Screen screen = GameService.Graphics.get_SpriteScreen();
				int x = _settings.PositionX.get_Value();
				int y = _settings.PositionY.get_Value();
				if (x < 0 || y < 0)
				{
					x = (((Control)screen).get_Width() - ((Control)_view).get_Width()) / 2;
					y = _settings.TopOffset.get_Value();
				}
				x = MathHelper.Clamp(x, 0, Math.Max(0, ((Control)screen).get_Width() - ((Control)_view).get_Width()));
				y = MathHelper.Clamp(y, 0, Math.Max(0, ((Control)screen).get_Height() - ((Control)_view).get_Height()));
				((Control)_view).set_Location(new Point(x, y));
			}
		}

		private void UpdateMapOverlay()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			if (_mapOverlay == null)
			{
				return;
			}
			Gw2MumbleService mumble = GameService.Gw2Mumble;
			if (!_settings.MapOverlayDebug.get_Value() || !mumble.get_UI().get_IsMapOpen())
			{
				((Control)_mapOverlay).set_Visible(false);
				return;
			}
			Coordinates2 mc = mumble.get_UI().get_MapCenter();
			Vector2 mapCenter = default(Vector2);
			((Vector2)(ref mapCenter))._002Ector((float)((Coordinates2)(ref mc)).get_X(), (float)((Coordinates2)(ref mc)).get_Y());
			double mapScale = mumble.get_UI().get_MapScale();
			Point res = GameService.Graphics.get_Resolution();
			int ssW = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
			int ssH = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			float kx = ((res.X > 0) ? ((float)ssW / (float)res.X) : 1f);
			float ky = ((res.Y > 0) ? ((float)ssH / (float)res.Y) : 1f);
			List<MapOverlay.Dot> dots = new List<MapOverlay.Dot>();
			foreach (CompassTarget t in _objectives.Targets)
			{
				Vector2 px = MapProjection.ContinentToScreen(t.ContinentPosition, mapCenter, mapScale, res.X, res.Y);
				dots.Add(new MapOverlay.Dot(new Vector2(px.X * kx, px.Y * ky), CompassTarget.ColorFor(t.Kind)));
			}
			_mapOverlay.SetDots(dots);
			_mapOverlay.DebugText = $"MapCenter=({((Coordinates2)(ref mc)).get_X():0},{((Coordinates2)(ref mc)).get_Y():0}) MapScale={mapScale:0.####} res={res.X}x{res.Y} ss={ssW}x{ssH}";
			((Control)_mapOverlay).set_Visible(true);
		}

		private List<CompassTarget> BuildTargets(Vector2 playerCont)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			List<CompassTarget> list = new List<CompassTarget>();
			bool onlyUndiscovered = _settings.OnlyUndiscovered.get_Value();
			int maxPerCategory = _settings.MaxPerCategory.get_Value();
			Dictionary<TargetKind, List<CompassTarget>> byKind = new Dictionary<TargetKind, List<CompassTarget>>();
			foreach (CompassTarget t in _objectives.Targets)
			{
				if (KindEnabled(t.Kind) && (!onlyUndiscovered || !_discovery.IsSeen(t.Id)))
				{
					if (!byKind.TryGetValue(t.Kind, out var kindList))
					{
						kindList = new List<CompassTarget>();
						byKind[t.Kind] = kindList;
					}
					kindList.Add(t);
				}
			}
			foreach (KeyValuePair<TargetKind, List<CompassTarget>> kv in byKind)
			{
				kv.Value.Sort((CompassTarget a, CompassTarget b) => Vector2.DistanceSquared(playerCont, a.ContinentPosition).CompareTo(Vector2.DistanceSquared(playerCont, b.ContinentPosition)));
				int take = Math.Min(maxPerCategory, kv.Value.Count);
				for (int i = 0; i < take; i++)
				{
					list.Add(kv.Value[i]);
				}
			}
			return list;
		}

		private string BuildDebugText(int mapId, Vector2 playerCont, MapCalibration cal, List<CompassTarget> targets, Vector2 facing)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			CompassTarget nearest = null;
			float best = float.MaxValue;
			foreach (CompassTarget t in targets)
			{
				float d = Vector2.DistanceSquared(playerCont, t.ContinentPosition);
				if (d < best)
				{
					best = d;
					nearest = t;
				}
			}
			string near = "-";
			if (nearest != null)
			{
				double rel = Gw2CoordinateUtil.RelativeBearing(playerCont, nearest.ContinentPosition, facing);
				double dm = (cal.IsValid ? cal.ToMeters(Vector2.Distance(playerCont, nearest.ContinentPosition)) : double.NaN);
				near = $"{nearest.Kind} '{nearest.Label}' b={MathHelper.ToDegrees((float)rel):0} d={dm:0}m";
			}
			Vector3 pc = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			return $"m{mapId} cont=({playerCont.X:0},{playerCont.Y:0}) w=({pc.X:0},{pc.Y:0},{pc.Z:0}) | {near}";
		}

		private double ComputeHalfFov()
		{
			if (_settings.MatchGameFov.get_Value())
			{
				float fv = GameService.Gw2Mumble.get_PlayerCamera().get_FieldOfView();
				float aspect = GameService.Graphics.get_AspectRatio();
				if (fv > 0.02f && aspect > 0.1f)
				{
					return Math.Atan(Math.Tan((double)fv / 2.0) * (double)aspect);
				}
			}
			return (double)MathHelper.ToRadians((float)_settings.FovDegrees.get_Value()) / 2.0;
		}

		private bool KindEnabled(TargetKind kind)
		{
			return kind switch
			{
				TargetKind.Waypoint => _settings.ShowWaypoints.get_Value(), 
				TargetKind.PointOfInterest => _settings.ShowPois.get_Value(), 
				TargetKind.Vista => _settings.ShowVistas.get_Value(), 
				TargetKind.Heart => _settings.ShowHearts.get_Value(), 
				TargetKind.SkillPoint => _settings.ShowSkillPoints.get_Value(), 
				_ => true, 
			};
		}

		protected override void Unload()
		{
			CompassView view = _view;
			if (view != null)
			{
				((Control)view).Dispose();
			}
			RadialCompassView radialView = _radialView;
			if (radialView != null)
			{
				((Control)radialView).Dispose();
			}
			MapOverlay mapOverlay = _mapOverlay;
			if (mapOverlay != null)
			{
				((Control)mapOverlay).Dispose();
			}
			DiscoveryWindow discoveryWindow = _discoveryWindow;
			if (discoveryWindow != null)
			{
				((Control)discoveryWindow).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Texture2D iconTexture = _iconTexture;
			if (iconTexture != null)
			{
				((GraphicsResource)iconTexture).Dispose();
			}
			Texture2D iconHoverTexture = _iconHoverTexture;
			if (iconHoverTexture != null)
			{
				((GraphicsResource)iconHoverTexture).Dispose();
			}
			_icons?.Dispose();
			_tiles?.Dispose();
		}
	}
}
