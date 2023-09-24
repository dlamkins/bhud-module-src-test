using Blish_HUD;
using Blish_HUD.Controls;
using Manlaan.CommanderMarkers.Library.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Manlaan.CommanderMarkers.Presets.Model
{
	public class MarkerPreview : IMapEntity
	{
		private MapData _mapData;

		private Vector3 _trigger;

		private MarkerSet _markerSet;

		private BitmapFont _bitmapFont = GameService.Content.get_DefaultFont16();

		public MarkerPreview(MapData mapData, MarkerSet markerSet)
		{
			_mapData = mapData;
			_markerSet = markerSet;
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
			SpriteBatch spriteBatch2 = spriteBatch;
			_markerSet.marks.ForEach(delegate(MarkerCoord mark)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				Texture2D icon = ((SquadMarker)mark.icon).GetIcon();
				Vector2 val = _mapData.WorldToScreenMap(mark.ToVector3());
				Rectangle val2 = default(Rectangle);
				((Rectangle)(ref val2))._002Ector((int)val.X - 16, (int)val.Y - 16, 32, 32);
				spriteBatch2.Draw(icon, val2, Color.get_White());
			});
		}

		public string GetMarkerText()
		{
			return _markerSet.name ?? "marker";
		}
	}
}
