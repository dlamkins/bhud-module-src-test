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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Invalid comparison between Unknown and I4
			if (type - 9 <= 3 || (int)type == 15)
			{
				return true;
			}
			return false;
		}
	}
}
