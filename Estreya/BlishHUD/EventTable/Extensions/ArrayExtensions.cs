using System;

namespace Estreya.BlishHUD.EventTable.Extensions
{
	public static class ArrayExtensions
	{
		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if (array.LongLength != 0L)
			{
				ArrayTraverse walker = new ArrayTraverse(array);
				do
				{
					action(array, walker.Position);
				}
				while (walker.Step());
			}
		}
	}
}
