using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Xna.Framework.Input;

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

		private MarkerPreview? _previewMarkerSet;

		private DateTime _lastTrigger = DateTime.Now;

		public MapWatchService(MapData map, SettingService settings)
		{
			ScreenMap screenMap = new ScreenMap(map);
			((Control)screenMap).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_screenMap = screenMap;
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
			Service.LtMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
		}

		private void AutoMarkerBooleanSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			CurrentMap_MapChanged(this, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
		}

		private void MarkersListing_MarkersChanged(object sender, EventArgs e)
		{
			CurrentMap_MapChanged(this, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
		}

		private void _interactKeybind_Activated(object sender, EventArgs e)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			DateTime now = DateTime.Now;
			TimeSpan cooldown = TimeSpan.FromSeconds(3.0);
			if (now - _lastTrigger < cooldown || _markers.Count <= 0 || !GameService.Gw2Mumble.get_UI().get_IsMapOpen() || !ShouldAttemptPlacement())
			{
				return;
			}
			_lastTrigger = now;
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
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
				if (num < 15f)
				{
					PlaceMarkers(marker, _map);
					break;
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			((Control)_screenMap).Update(gameTime);
		}

		public Task PlaceMarkers(MarkerSet marders)
		{
			return PlaceMarkers(marders, _map);
		}

		private bool ShouldAttemptPlacement()
		{
			bool shouldDoIt = Service.Settings.AutoMarker_FeatureEnabled.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable();
			if (Service.Settings._settingOnlyWhenCommander.get_Value() || Service.LtMode.get_Value())
			{
				shouldDoIt &= GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander() || Service.LtMode.get_Value();
			}
			return shouldDoIt;
		}

		private Task PlaceMarkers(MarkerSet markers, MapData mapData)
		{
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
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
			MouseState state = Mouse.GetState();
			Point originalMousePos = ((MouseState)(ref state)).get_Position();
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
					Vector2 d = blishCoord * scale;
					if (((Rectangle)(ref screenBounds)).Contains(blishCoord))
					{
						Mouse.SetPosition((int)d.X, (int)d.Y);
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
			Mouse.SetPosition(originalMousePos.X, originalMousePos.Y);
			return Task.CompletedTask;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			_screenMap.ClearEntities();
			if (!_setting.AutoMarker_ShowTrigger.get_Value())
			{
				return;
			}
			_currentmap = e.get_Value();
			_markers = (from m in Service.MarkersListing.GetMarkersForMap(e.get_Value())
				where m.enabled
				select m).ToList();
			foreach (MarkerSet marker in _markers)
			{
				_screenMap.AddEntity(new BasicMarker(_map, marker.trigger!.ToVector3(), marker.name, marker.description));
			}
		}

		public void PreviewMarkerSet(MarkerSet preview)
		{
			RemovePreviewMarkerSet();
			if (Service.Settings.AutoMarker_ShowPreview.get_Value())
			{
				_previewMarkerSet = new MarkerPreview(_map, preview);
				_screenMap.AddEntity(_previewMarkerSet);
			}
		}

		public void PreviewClosestMarkerSet()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
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
				if (num < 15f)
				{
					PreviewMarkerSet(marker);
					break;
				}
			}
		}

		public void RemovePreviewMarkerSet()
		{
			if (_previewMarkerSet != null)
			{
				_screenMap.RemoveEntity(_previewMarkerSet);
				_previewMarkerSet = null;
			}
		}

		public void Dispose()
		{
			((Control)_screenMap).Dispose();
			_setting.AutoMarker_ShowTrigger.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			_setting.AutoMarker_FeatureEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			Service.LtMode.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoMarkerBooleanSettingChanged);
			Service.MarkersListing.MarkersChanged -= new EventHandler(MarkersListing_MarkersChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_setting._settingInteractKeyBinding.get_Value().set_Enabled(false);
			_setting._settingInteractKeyBinding.get_Value().remove_Activated((EventHandler<EventArgs>)_interactKeybind_Activated);
		}
	}
}
