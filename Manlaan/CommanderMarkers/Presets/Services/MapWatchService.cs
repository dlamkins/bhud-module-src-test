using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Presets.Services
{
	public class MapWatchService : IDisposable
	{
		private MapData _map;

		private SettingService _setting;

		private int _currentmap;

		private List<MarkerSet> _markers = new List<MarkerSet>();

		private ScreenMap _screenMap;

		private List<BasicMarker> _triggerMarker = new List<BasicMarker>();

		private BillboardControl _billboards;

		private MarkerPreview? _previewMarkerSet;

		private BillBoardPreview? _billboardPreview;

		private DateTime _lastTrigger = DateTime.Now;

		public const float TRIGGER_DISTANCE_OPEN_MAP = 15f;

		public const float TRIGGER_DISTANCE_CLOSED_MAP = 2f;

		public MapWatchService(MapData map, SettingService settings)
		{
			ScreenMap screenMap = new ScreenMap(map);
			((Control)screenMap).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_screenMap = screenMap;
			BillboardControl billboardControl = new BillboardControl(map);
			((Control)billboardControl).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_billboards = billboardControl;
			_map = map;
			_setting = settings;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			Service.MarkersListing.MarkersChanged += new EventHandler(MarkersListing_MarkersChanged);
			_setting._settingInteractKeyBinding.get_Value().set_Enabled(true);
			_setting._settingInteractKeyBinding.get_Value().set_BlockSequenceFromGw2(false);
			_setting._settingInteractKeyBinding.get_Value().add_Activated((EventHandler<EventArgs>)_interactKeybind_Activated);
			CurrentMap_MapChanged(this, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			_setting.AutoMarker_FeatureEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			_setting.AutoMarker_ShowTrigger.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			_setting.AutoMarker_Billboard_FeatureEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			Service.LtMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			_setting.RtApiIntegrationEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
		}

		private void AutoMarkerBooleanSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			CurrentMap_MapChanged(this, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
		}

		private void MarkersListing_MarkersChanged(object sender, EventArgs e)
		{
			int mapId = ((_currentmap != 0) ? _currentmap : GameService.Gw2Mumble.get_CurrentMap().get_Id());
			RefreshMapMarkers(mapId);
		}

		private void RefreshMapMarkers(int mapId)
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			RemovePreviewMarkerSet();
			_screenMap.ResetPreviewState();
			_screenMap.ClearEntities();
			_billboards.ClearEntities();
			_currentmap = mapId;
			_markers = Service.MarkersListing.GetMarkersForMap(mapId);
			if (!_setting.AutoMarker_ShowTrigger.get_Value() && !_setting.AutoMarker_Billboard_FeatureEnabled.get_Value())
			{
				return;
			}
			foreach (MarkerSet marker in _markers)
			{
				if (_setting.AutoMarker_ShowTrigger.get_Value())
				{
					_screenMap.AddEntity(new BasicMarker(_map, marker.trigger!.ToVector3(), marker.name, marker.description));
				}
				if (_setting.AutoMarker_Billboard_FeatureEnabled.get_Value())
				{
					_billboards.AddEntity(new BillBoardPreview(_map, marker));
				}
			}
		}

		private void _interactKeybind_Activated(object sender, EventArgs e)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			DateTime now = DateTime.Now;
			TimeSpan cooldown = TimeSpan.FromSeconds(3.0);
			if (now - _lastTrigger < cooldown || _markers.Count <= 0 || (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() && !Service.Settings.AutoMarker_Billboard_Placement.get_Value()) || !ShouldAttemptPlacement())
			{
				return;
			}
			_lastTrigger = now;
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			MarkerSet closestMarker = null;
			float closestDistance = float.MaxValue;
			float placementThreshold = (GameService.Gw2Mumble.get_UI().get_IsMapOpen() ? 15f : 2f);
			foreach (MarkerSet marker in _markers)
			{
				Vector3 val = playerPosition;
				Vector3? val2 = marker.trigger?.ToVector3();
				Vector3? val3 = (val2.HasValue ? new Vector3?(val - val2.GetValueOrDefault()) : null);
				float num;
				if (!val3.HasValue)
				{
					num = 1000f;
				}
				else
				{
					Vector3 valueOrDefault = val3.GetValueOrDefault();
					num = ((Vector3)(ref valueOrDefault)).Length();
				}
				float d = num;
				if (d < placementThreshold && d < closestDistance)
				{
					closestMarker = marker;
					closestDistance = d;
				}
			}
			if (closestMarker != null)
			{
				PlaceMarkers(closestMarker, _map);
			}
		}

		public void Update(GameTime gameTime)
		{
			((Control)_screenMap).Update(gameTime);
			((Control)_billboards).Update(gameTime);
		}

		public Task PlaceMarkers(MarkerSet marders)
		{
			if (!marders.enabled)
			{
				return Task.CompletedTask;
			}
			return PlaceMarkers(marders, _map);
		}

		private bool ShouldAttemptPlacement()
		{
			bool shouldDoIt = Service.Settings.AutoMarker_FeatureEnabled.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable();
			if (Service.Settings._settingOnlyWhenCommander.get_Value() || Service.LtMode.get_Value())
			{
				shouldDoIt &= CommanderPermissionHelper.PassesCommanderGate();
			}
			return shouldDoIt;
		}

		private Task PlaceMarkers(MarkerSet markers, MapData mapData)
		{
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			if (markers.marks == null)
			{
				return Task.CompletedTask;
			}
			float scale = GameService.Graphics.get_UIScaleMultiplier();
			List<KeyBinding> keys = new List<KeyBinding>
			{
				_setting._settingClearGndBinding.get_Value(),
				_setting._settingArrowGndBinding.get_Value(),
				_setting._settingCircleGndBinding.get_Value(),
				_setting._settingHeartGndBinding.get_Value(),
				_setting._settingSquareGndBinding.get_Value(),
				_setting._settingStarGndBinding.get_Value(),
				_setting._settingSpiralGndBinding.get_Value(),
				_setting._settingTriangleGndBinding.get_Value(),
				_setting._settingXGndBinding.get_Value(),
				_setting._settingClearGndBinding.get_Value()
			};
			int delay = _setting.AutoMarker_PlacementDelay.get_Value();
			bool useScreenCoords = MarkerPlacementHelper.UseScreenCoordinatesForPlacement();
			Point originalMousePos = MarkerPlacementHelper.GetPlacementCursorPosition(useScreenCoords);
			Rectangle screenBounds = ScreenMap.Data.ScreenBounds;
			InputHelper.DoHotKey(keys[0]);
			Thread.Sleep(delay / 2);
			List<string> errors = new List<string>();
			for (int i = 0; i < markers.marks.Count; i++)
			{
				MarkerCoord marker = markers.marks[i];
				if (marker.icon <= 9 && marker.icon >= 0)
				{
					Vector2 blishCoord = mapData.WorldToScreenMap(marker.ToVector3());
					Point placementPos = MarkerPlacementHelper.BlishToPlacementPosition(blishCoord, scale);
					if (((Rectangle)(ref screenBounds)).Contains(blishCoord))
					{
						MarkerPlacementHelper.SetPlacementMousePosition(placementPos, useScreenCoords);
						Thread.Sleep(delay / 2);
						InputHelper.DoHotKey(keys[marker.icon]);
						Thread.Sleep(delay);
					}
					else
					{
						errors.Add(((SquadMarker)marker.icon).EnumValue() + " " + marker.name);
					}
				}
			}
			if (errors.Count > 0)
			{
				ScreenNotification.ShowNotification($"Unable to place {errors.Count} marker(s)\nTry moving your map to the marker trigger", (NotificationType)1, (Texture2D)null, 6);
			}
			MarkerPlacementHelper.SetPlacementMousePosition(originalMousePos, useScreenCoords);
			return Task.CompletedTask;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			RefreshMapMarkers(e.get_Value());
		}

		public void PreviewMarkerSet(MarkerSet preview)
		{
			RemovePreviewMarkerSet();
			if (preview.enabled && Service.Settings.AutoMarker_ShowPreview.get_Value())
			{
				_previewMarkerSet = new MarkerPreview(_map, preview);
				_screenMap.AddEntity(_previewMarkerSet);
				if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen())
				{
					_billboardPreview = new BillBoardPreview(_map, preview);
					_billboards.AddEntity(_billboardPreview);
				}
			}
		}

		public void PreviewClosestMarkerSet()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			MarkerSet closestMarker = null;
			float closestDistance = float.MaxValue;
			foreach (MarkerSet marker in _markers)
			{
				Vector3 val = playerPosition;
				Vector3? val2 = marker.trigger?.ToVector3();
				Vector3? val3 = (val2.HasValue ? new Vector3?(val - val2.GetValueOrDefault()) : null);
				float num;
				if (!val3.HasValue)
				{
					num = 1000f;
				}
				else
				{
					Vector3 valueOrDefault = val3.GetValueOrDefault();
					num = ((Vector3)(ref valueOrDefault)).Length();
				}
				float d = num;
				if (d < 15f && d < closestDistance)
				{
					closestMarker = marker;
					closestDistance = d;
				}
			}
			if (closestMarker != null)
			{
				PreviewMarkerSet(closestMarker);
			}
		}

		public void RemovePreviewMarkerSet()
		{
			if (_previewMarkerSet != null)
			{
				_screenMap.RemoveEntity(_previewMarkerSet);
				if (_billboardPreview != null)
				{
					_billboards.RemoveEntity(_billboardPreview);
				}
				_previewMarkerSet = null;
				_billboardPreview = null;
			}
		}

		public void Dispose()
		{
			((Control)_screenMap).Dispose();
			((Control)_billboards).Dispose();
			_setting.AutoMarker_Billboard_FeatureEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			_setting.AutoMarker_ShowTrigger.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			_setting.AutoMarker_FeatureEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			Service.LtMode.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			_setting.RtApiIntegrationEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			Service.MarkersListing.MarkersChanged -= new EventHandler(MarkersListing_MarkersChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_setting._settingInteractKeyBinding.get_Value().set_Enabled(false);
			_setting._settingInteractKeyBinding.get_Value().remove_Activated((EventHandler<EventArgs>)_interactKeybind_Activated);
		}
	}
}
