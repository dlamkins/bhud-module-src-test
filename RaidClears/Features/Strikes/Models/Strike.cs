using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Models
{
	public class Strike : Wing
	{
		public Strike(string name, int index, string shortName, IEnumerable<BoxModel> boxes)
			: base(name, index, shortName, boxes)
		{
		}

		public virtual void Dispose()
		{
		}
	}
}
