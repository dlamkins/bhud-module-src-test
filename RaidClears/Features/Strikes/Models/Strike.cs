using System.Collections.Generic;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Models
{
	public class Strike : GroupModel
	{
		public ExpansionStrikes Expansion;

		public Strike(string name, string id, int index, string shortName, IEnumerable<BoxModel> boxes)
			: base(name, shortName, index, shortName, boxes)
		{
		}

		public Strike(ExpansionStrikes expansion)
			: base(expansion.Name, expansion.Id, 0, Service.StrikeSettings.GetEncounterLabel(expansion.Id), expansion.GetEncounters())
		{
			Expansion = expansion;
		}

		public virtual void Dispose()
		{
		}
	}
}
