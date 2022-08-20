using System.Collections.Generic;
using System.Linq;

namespace Ideka.RacingMeter
{
	public class DropOutStack<T> : LinkedList<T>
	{
		public int Capacity { get; }

		public DropOutStack(int capacity)
		{
			Capacity = capacity;
		}

		public void Push(T item)
		{
			while (base.Count >= Capacity)
			{
				RemoveFirst();
			}
			AddLast(item);
		}

		public bool TryGetLast(out T last)
		{
			bool any = this.Any();
			last = (any ? base.Last.Value : default(T));
			return any;
		}
	}
}
