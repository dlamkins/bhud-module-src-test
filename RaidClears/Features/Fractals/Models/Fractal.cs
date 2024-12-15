using System.Collections.Generic;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Fractals.Models
{
	public class Fractal : GroupModel
	{
		public static string ChallengeMoteLabel = "Challenge Mote";

		public static string ChallengeMoteId = "CM";

		public static string TomorrowLabel = "Tomorrow T#";

		public static string TomorrowId = "Tom";

		public static string TierNLabel = "Tier #";

		public static string TierNId = "T#";

		public static string RecLabel = "Daily Recommended";

		public static string RecId = "Rec";

		public Fractal(string name, int index, string shortName, IEnumerable<BoxModel> boxes)
			: base(name, shortName, index, Service.FractalPersistance.GetEncounterLabel(shortName), boxes)
		{
		}

		public virtual void Dispose()
		{
		}
	}
}
