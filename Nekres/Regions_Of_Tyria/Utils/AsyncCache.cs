using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Nekres.Regions_Of_Tyria.Utils
{
	public class AsyncCache<TKey, TValue>
	{
		private readonly Func<TKey, Task<TValue>> _valueFactory;

		private readonly ConcurrentDictionary<TKey, TaskCompletionSource<TValue>> _completionSourceCache = new ConcurrentDictionary<TKey, TaskCompletionSource<TValue>>();

		public AsyncCache(Func<TKey, Task<TValue>> valueFactory)
		{
			_valueFactory = valueFactory;
		}

		public async Task<TValue> GetItem(TKey key)
		{
			TaskCompletionSource<TValue> newSource = new TaskCompletionSource<TValue>();
			TaskCompletionSource<TValue> currentSource = _completionSourceCache.GetOrAdd(key, newSource);
			if (currentSource != newSource)
			{
				return await currentSource.Task;
			}
			try
			{
				newSource.SetResult(await _valueFactory(key));
			}
			catch (Exception e)
			{
				newSource.SetException(e);
			}
			return await newSource.Task;
		}

		public async Task<TValue> RemoveItem(TKey key)
		{
			TaskCompletionSource<TValue> item;
			return (!_completionSourceCache.TryRemove(key, out item)) ? default(TValue) : (await item.Task);
		}
	}
}
