using System;

namespace Estreya.BlishHUD.Shared.Extensions
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

		public static void MoveEntry<T>(this T[] array, int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				T tmp = array[oldIndex];
				if (newIndex < oldIndex)
				{
					Array.Copy(array, newIndex, array, newIndex + 1, oldIndex - newIndex);
				}
				else
				{
					Array.Copy(array, oldIndex + 1, array, oldIndex, newIndex - oldIndex);
				}
				array[newIndex] = tmp;
			}
		}
	}
}
