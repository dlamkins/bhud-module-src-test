using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager
{
	public class _UpgradeIDs
	{
		public List<int> _Sigils { get; private set; }

		public List<int> _Runes { get; private set; }

		public _UpgradeIDs(List<int> runes, List<int> sigils)
		{
			_Sigils = new List<int>(sigils);
			_Runes = new List<int>(runes);
		}
	}
}
