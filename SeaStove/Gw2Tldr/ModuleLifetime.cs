using System;

namespace SeaStove.Gw2Tldr
{
	internal sealed class ModuleLifetime
	{
		private readonly object _gate = new object();

		private bool _unloading;

		public bool TryUse(Action action)
		{
			lock (_gate)
			{
				if (_unloading)
				{
					return false;
				}
				action();
				return true;
			}
		}

		public void Unload(Action cleanup)
		{
			lock (_gate)
			{
				if (!_unloading)
				{
					_unloading = true;
					cleanup();
				}
			}
		}
	}
}
