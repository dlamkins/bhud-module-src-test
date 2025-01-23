using System;
using System.Collections.Generic;

public static class LinqExtensions
{
	public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (size <= 0)
		{
			throw new ArgumentOutOfRangeException("size", "Size must be greater than 0.");
		}
		using IEnumerator<T> enumerator = source.GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return GetBatch(enumerator, size - 1);
		}
	}

	private static IEnumerable<T> GetBatch<T>(IEnumerator<T> enumerator, int size)
	{
		yield return enumerator.Current;
		for (int i = 0; i < size; i++)
		{
			if (!enumerator.MoveNext())
			{
				break;
			}
			yield return enumerator.Current;
		}
	}
}
