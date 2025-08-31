using Gw2Sharp.WebApi.V2.Models;

namespace Blish_HUD.Extended
{
	public static class MapExtensions
	{
		public static string GetHash(this Map map)
		{
			return $"{map.ContinentId}{map.ContinentRect.TopLeft.X}{map.ContinentRect.TopLeft.Y}{map.ContinentRect.BottomRight.X}{map.ContinentRect.BottomRight.Y}".ToSHA1Hash().Substring(0, 8);
		}
	}
}
