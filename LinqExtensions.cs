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
		while (true)
		{
			IEnumerable<T> batch = GetBatch(enumerator, size);
			if (batch != null)
			{
				yield return batch;
				continue;
			}
			break;
		}
	}

	private static IEnumerable<T>? GetBatch<T>(IEnumerator<T> enumerator, int size)
	{
		if (!enumerator.MoveNext())
		{
			return null;
		}
		List<T> batch = new List<T> { enumerator.Current };
		for (int i = 0; i < size; i++)
		{
			if (!enumerator.MoveNext())
			{
				break;
			}
			batch.Add(enumerator.Current);
		}
		return batch;
	}
}
