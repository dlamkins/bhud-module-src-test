using System.Collections.Generic;
using Kenedia.Modules.BuildsManager.DataModels.Stats;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class StatMap
	{
		public EquipmentStat Stat { get; set; }

		public string Name { get; set; }

		public List<int> Ids { get; set; }
	}
}
