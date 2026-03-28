using System.Threading;

namespace FarmingTracker
{
	public class ThreadSafeLong
	{
		private long _value;

		public long Value
		{
			get
			{
				return Interlocked.Read(ref _value);
			}
			set
			{
				Interlocked.Exchange(ref _value, value);
			}
		}

		public void Add(long value)
		{
			Interlocked.Add(ref _value, value);
		}

		public void Add(ThreadSafeLong threadSafeLong)
		{
			Interlocked.Add(ref _value, threadSafeLong.Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
