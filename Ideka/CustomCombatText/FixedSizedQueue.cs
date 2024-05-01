using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ideka.CustomCombatText
{
	public class FixedSizedQueue<T> : IEnumerable<T>, IEnumerable
	{
		private readonly ConcurrentQueue<T> _queue;

		private readonly object _lock;

		public int Size { get; }

		public FixedSizedQueue(int size)
		{
			Size = size;
			_queue = new ConcurrentQueue<T>();
			_lock = new object();
			base._002Ector();
		}

		public void Enqueue(T obj)
		{
			lock (_lock)
			{
				_queue.Enqueue(obj);
				while (_queue.Count > Size)
				{
					_queue.TryDequeue(out var _);
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)_queue).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_queue).GetEnumerator();
		}
	}
}
