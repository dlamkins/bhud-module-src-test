using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class ProcessExtensions
	{
		public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (process.HasExited)
			{
				return Task.CompletedTask;
			}
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			process.EnableRaisingEvents = true;
			process.Exited += delegate
			{
				tcs.TrySetResult(null);
			};
			if (cancellationToken != default(CancellationToken))
			{
				cancellationToken.Register(delegate
				{
					tcs.SetCanceled();
				});
			}
			if (!process.HasExited)
			{
				return tcs.Task;
			}
			return Task.CompletedTask;
		}
	}
}
