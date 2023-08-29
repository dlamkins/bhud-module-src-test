using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers.Presets.Service
{
	public class MapWatchService : IDisposable
	{
		private MapData _map;

		private SettingService _setting;

		private int _currentmap;

		private List<MarkerSet> _markers = new List<MarkerSet>();

		private ScreenMap _screenMap;

		private List<BasicMarker> _triggerMarker = new List<BasicMarker>();

		public MapWatchService(MapData map, SettingService settings)
		{
			ScreenMap screenMap = new ScreenMap(map);
			((Control)screenMap).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_screenMap = screenMap;
			_map = map;
			_setting = settings;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_setting._settingInteractKeyBinding.get_Value().set_Enabled(true);
			_setting._settingInteractKeyBinding.get_Value().set_BlockSequenceFromGw2(false);
			_setting._settingInteractKeyBinding.get_Value().add_Activated((EventHandler<EventArgs>)_interactKeybind_Activated);
			CurrentMap_MapChanged(this, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
		}

		private void _interactKeybind_Activated(object sender, EventArgs e)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (_markers.Count <= 0 || !GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return;
			}
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			foreach (MarkerSet marker in _markers)
			{
				Vector3 val = playerPosition - marker.trigger!.ToVector3();
				if (((Vector3)(ref val)).Length() < 15f)
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

		private Task PlaceMarkers(MarkerSet markers, MapData mapData)
		{
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
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
			int delay = _setting._settingMarkerPlaceDelay.get_Value();
			MouseState state = Mouse.GetState();
			Point originalMousePos = ((MouseState)(ref state)).get_Position();
			InputHelper.DoHotKey(keys[0]);
			Thread.Sleep(delay / 2);
			for (int i = 0; i < markers.marks!.Count; i++)
			{
				MarkerCoord marker = markers.marks![i];
				if (marker.icon <= 9 && marker.icon >= 0)
				{
					Vector2 d = mapData.WorldToScreenMap(marker.ToVector3()) * scale;
					Mouse.SetPosition((int)d.X, (int)d.Y);
					Thread.Sleep(delay / 2);
					InputHelper.DoHotKey(keys[marker.icon]);
					Thread.Sleep(delay);
				}
			}
			Mouse.SetPosition(originalMousePos.X, originalMousePos.Y);
			return Task.CompletedTask;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			_currentmap = e.get_Value();
			_markers = Manlaan.CommanderMarkers.Service.MarkersListing.GetMarkersForMap(e.get_Value());
			_screenMap.ClearEntities();
			foreach (MarkerSet marker in _markers)
			{
				_screenMap.AddEntity(new BasicMarker(_map, marker.trigger!.ToVector3(), marker.name, marker.description));
			}
		}

		public void Dispose()
		{
			((Control)_screenMap).Dispose();
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_setting._settingInteractKeyBinding.get_Value().set_Enabled(false);
			_setting._settingInteractKeyBinding.get_Value().remove_Activated((EventHandler<EventArgs>)_interactKeybind_Activated);
		}
	}
}
