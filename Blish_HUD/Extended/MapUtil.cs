namespace Blish_HUD.Extended
{
	public class MapUtil
	{
		public static string GetSHA1(int continentId, int topLeftX, int topLeftY, int bottomRightX, int bottomRightY)
		{
			return $"{continentId}{topLeftX}{topLeftY}{bottomRightX}{bottomRightY}".ToSHA1Hash().Substring(0, 8);
		}
	}
}
