using System.Collections.Generic;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers
{
	public class MarkerSequence
	{
		private int _mapId;

		private List<Vector3> _markers = new List<Vector3>();

		public MarkerSequence(int mapId, List<Vector3> markers)
		{
			_mapId = mapId;
			_markers = markers;
		}

		public void PlaceMarkers(MapData mapData)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			float scale = GameService.Graphics.get_UIScaleMultiplier();
			GameService.Gw2Mumble.get_UI().get_UISize();
			Keys key = (Keys)49;
			if (GameService.Gw2Mumble.get_CurrentMap().get_Id() == _mapId)
			{
				InputHelper.DoHotKey(new KeyBinding((ModifierKeys)2, (Keys)57));
				Thread.Sleep(20);
				for (int i = 0; i < _markers.Count; i++)
				{
					Vector2 d = mapData.WorldToScreenMap(_markers[i]) * scale;
					Mouse.SetPosition((int)d.X, (int)d.Y);
					Thread.Sleep(10);
					InputHelper.DoHotKey(new KeyBinding((ModifierKeys)2, (Keys)(key + i)));
					Thread.Sleep(60);
				}
			}
		}
	}
}
