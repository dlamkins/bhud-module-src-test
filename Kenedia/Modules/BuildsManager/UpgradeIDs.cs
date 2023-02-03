using System;
using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager
{
	public class UpgradeIDs : IDisposable
	{
		public List<int> Sigils { get; private set; }

		public List<int> Runes { get; private set; }

		public void Dispose()
		{
			Sigils.Clear();
			Runes.Clear();
		}

		public UpgradeIDs(List<int> runes, List<int> sigils)
		{
			Sigils = new List<int>(sigils);
			Runes = new List<int>(runes);
		}
	}
}
