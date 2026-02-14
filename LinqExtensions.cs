using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class LinqExtensions
{
	[IteratorStateMachine(typeof(_003CBatch_003Ed__0<>))]
	public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
	{
		return new _003CBatch_003Ed__0<T>(-2)
		{
			_003C_003E3__source = source,
			_003C_003E3__size = size
		};
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
