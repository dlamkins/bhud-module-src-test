using System.Collections.Generic;
using System.Linq;

namespace Ideka.RacingMeter
{
	public class DropOutStack<T> : LinkedList<T>
	{
		private readonly object _lock = new object();

		public int Capacity { get; }

		public DropOutStack(int capacity)
		{
			Capacity = capacity;
		}

		public void Push(T item)
		{
			lock (_lock)
			{
				while (base.Count >= Capacity)
				{
					RemoveFirst();
				}
				AddLast(item);
			}
		}

		public bool TryGetLast(out T? last)
		{
			lock (_lock)
			{
				bool any = this.Any();
				last = (any ? base.Last.Value : default(T));
				return any;
			}
		}
	}
}
