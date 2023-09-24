using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Presets.Model
{
	public class BasicMarker : IMapEntity
	{
		private MapData _mapData;

		private Vector3 _trigger;

		private string? _name;

		private string? _description;

		public BasicMarker(MapData mapData, Vector3 trigger, string? name, string? description)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_mapData = mapData;
			_trigger = trigger;
			_name = name;
			_description = description;
		}

		public float DistanceFrom(Vector3 playerPosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = playerPosition - _trigger;
			return ((Vector3)(ref val)).Length();
		}

		public void DrawToMap(SpriteBatch spriteBatch, IMapBounds map, Control control, Vector3 playerPosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (!(Math.Abs(playerPosition.Z - _trigger.Z) > 30f))
			{
				Vector2 mapCoordinates = _mapData.WorldToScreenMap(_trigger);
				Rectangle blishIcon = default(Rectangle);
				((Rectangle)(ref blishIcon))._002Ector((int)mapCoordinates.X - 16, (int)mapCoordinates.Y - 16, 32, 32);
				spriteBatch.Draw(Service.Textures!._blishHeart, blishIcon, Color.get_White());
			}
		}

		public string GetMarkerText()
		{
			return _name + "\n" + _description;
		}
	}
}
