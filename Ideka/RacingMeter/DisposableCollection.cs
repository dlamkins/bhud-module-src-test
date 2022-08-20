using System;
using System.Collections.Generic;

namespace Ideka.RacingMeter
{
	public class DisposableCollection : IDisposable
	{
		private readonly HashSet<IDisposable> _disposables = new HashSet<IDisposable>();

		public T Add<T>(T disposable) where T : notnull, IDisposable
		{
			_disposables.Add(disposable);
			return disposable;
		}

		public void Dispose()
		{
			foreach (IDisposable disposable in _disposables)
			{
				disposable.Dispose();
			}
			_disposables.Clear();
		}
	}
}
