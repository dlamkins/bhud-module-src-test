using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Internal
{
	internal static class AwaitableThreadPool
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public readonly struct Awaitable : ICriticalNotifyCompletion, INotifyCompletion
		{
			public bool IsCompleted => false;

			public void GetResult()
			{
			}

			public Awaitable GetAwaiter()
			{
				return this;
			}

			public void OnCompleted(Action continuation)
			{
				Task.Run(continuation);
			}

			public void UnsafeOnCompleted(Action continuation)
			{
				OnCompleted(continuation);
			}
		}

		public static Awaitable Yield()
		{
			return default(Awaitable);
		}
	}
}
