using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Features.Strikes.Models
{
	public class Strike : Wing
	{
		public ExpansionStrikes Expansion;

		public Strike(string name, int index, string shortName, IEnumerable<BoxModel> boxes)
			: base(name, index, shortName, boxes)
		{
		}

		public Strike(ExpansionStrikes expansion)
			: base(expansion.Name, 0, expansion.Abbriviation, expansion.GetEncounters())
		{
			Expansion = expansion;
		}

		public virtual void Dispose()
		{
		}
	}
}
