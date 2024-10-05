using System;
using System.Collections.Generic;
using System.Linq;

namespace Kenedia.Modules.Core.Extensions
{
	public static class DisposableExtensions
	{
		public static void DisposeAll(this IEnumerable<IDisposable> disposables)
		{
			foreach (IDisposable item in disposables.ToList())
			{
				item?.Dispose();
			}
		}
	}
}
