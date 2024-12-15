using System.Collections.Generic;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	public class Wing : GroupModel
	{
		public Wing(string name, string id, int index, string shortName, IEnumerable<BoxModel> boxes)
			: base(name, id, index, Service.RaidSettings.GetEncounterLabel(id), boxes)
		{
		}
	}
}
