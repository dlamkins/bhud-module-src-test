using System;
using System.Collections.Generic;

namespace Kenedia.Modules.Core.Extensions
{
	public static class DisposableExtensions
	{
		public static void DisposeAll(this IEnumerable<IDisposable> disposables)
		{
			try
			{
				foreach (IDisposable disposable in disposables)
				{
					disposable?.Dispose();
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
