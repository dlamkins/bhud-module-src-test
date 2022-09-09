using System;
using System.Linq;

namespace Nekres.Mumble_Info
{
	internal static class BitmaskUtil
	{
		public static uint GetBitmask(params bool[] bits)
		{
			return (uint)bits.Select((bool b, int i) => b ? (1 << i) : 0).Aggregate((int a, int b) => a | b);
		}

		public static bool[] GetBooleans(uint mask)
		{
			return (from b in mask.ToString().ToCharArray().Select(Convert.ToInt32)
				select (mask & (1 << b)) != 0).ToArray();
		}
	}
}
