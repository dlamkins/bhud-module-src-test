using System;

namespace FarmingTracker
{
	public class GenericEventArgs<T> : EventArgs
	{
		public T Data { get; }

		public GenericEventArgs(T data)
		{
			Data = data;
		}
	}
}
