using System.Collections.Generic;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	public class Wing : GroupModel
	{
		public Wing(string name, int index, string shortName, IEnumerable<BoxModel> boxes)
			: base(name, index, shortName, boxes)
		{
		}
	}
}
