using Gw2Sharp.Models;

namespace Nekres.Mistwar
{
	internal static class MapTypeExtensions
	{
		public static bool IsWorldVsWorld(this MapType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Invalid comparison between Unknown and I4
			if (type - 9 <= 3)
			{
				return true;
			}
			return false;
		}
	}
}
