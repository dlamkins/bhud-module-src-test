using System;
using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager
{
	public class _UpgradeIDs : IDisposable
	{
		private bool disposed;

		public List<int> _Sigils { get; private set; }

		public List<int> _Runes { get; private set; }

		public void Dispose()
		{
			_Sigils.Clear();
			_Runes.Clear();
		}

		public _UpgradeIDs(List<int> runes, List<int> sigils)
		{
			_Sigils = new List<int>(sigils);
			_Runes = new List<int>(runes);
		}
	}
}
