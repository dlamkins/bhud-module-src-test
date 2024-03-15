using System;
using System.Buffers;

namespace Ideka.CustomCombatText.Bridge
{
	public class RentedArray<T> : IDisposable
	{
		public int Length { get; }

		public T[] Array { get; }

		public RentedArray(int length)
		{
			Length = length;
			Array = ArrayPool<T>.get_Shared().Rent(length);
			base._002Ector();
		}

		public void Dispose()
		{
			ArrayPool<T>.get_Shared().Return(Array, false);
		}
	}
}
