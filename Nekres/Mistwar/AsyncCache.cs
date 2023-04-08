using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Nekres.Mistwar
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

		public async Task Clear()
		{
			foreach (TKey key in _completionSourceCache.Keys)
			{
				TValue item = await RemoveItem(key);
				if (item == null)
				{
					continue;
				}
				IDisposable disposable = item as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
					continue;
				}
				IEnumerable enumerable = item as IEnumerable;
				if (enumerable == null)
				{
					continue;
				}
				foreach (object item2 in enumerable)
				{
					(item2 as IDisposable)?.Dispose();
				}
			}
		}

		public bool ContainsKey(TKey key)
		{
			return _completionSourceCache.ContainsKey(key);
		}
	}
}
