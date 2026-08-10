using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Internal
{
	internal static class ValueTaskExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task GetAsTask(this in ValueTask<FlushResult> valueTask)
		{
			if (valueTask.IsCompletedSuccessfully)
			{
				valueTask.GetAwaiter().GetResult();
				return Task.CompletedTask;
			}
			return valueTask.AsTask();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask GetAsValueTask(this in ValueTask<FlushResult> valueTask)
		{
			if (valueTask.IsCompletedSuccessfully)
			{
				valueTask.GetAwaiter().GetResult();
				return default(ValueTask);
			}
			return new ValueTask(valueTask.AsTask());
		}
	}
}
