using System;
using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager
{
	internal static class DisposableExtensions
	{
		public static void DisposeAll(this IEnumerable<IDisposable> disposables)
		{
			foreach (IDisposable disposable in disposables)
			{
				disposable?.Dispose();
			}
		}
	}
}
