using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	public class Encounter : BoxModel
	{
		public Encounter(Encounters.RaidBosses boss)
			: base(boss.GetApiLabel(), boss.GetLabel(), boss.GetLabelShort())
		{
		}

		public Encounter(Encounters.StrikeMission boss)
			: base(boss.GetApiLabel(), boss.GetLabel(), boss.GetLabelShort())
		{
		}
	}
}
