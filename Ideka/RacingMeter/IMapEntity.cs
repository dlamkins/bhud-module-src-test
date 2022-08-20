using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public interface IMapEntity
	{
		void DrawToMap(SpriteBatch spriteBatch, MapBounds map);
	}
}
