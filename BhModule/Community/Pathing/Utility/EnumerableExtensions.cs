using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BhModule.Community.Pathing.Utility
{
	public static class EnumerableExtensions
	{
		public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> funcBody, int maxDoP = 4)
		{
			return Task.WhenAll(Partitioner.Create(source).GetPartitions(maxDoP).AsParallel()
				.Select(AwaitPartition));
			async Task AwaitPartition(IEnumerator<T> partition)
			{
				using (partition)
				{
					while (partition.MoveNext())
					{
						await Task.Yield();
						await funcBody(partition.Current);
					}
				}
			}
		}
	}
}
