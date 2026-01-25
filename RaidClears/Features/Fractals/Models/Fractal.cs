using System.Collections.Generic;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;

namespace RaidClears.Features.Fractals.Models
{
	public class Fractal : GroupModel
	{
		public static string ChallengeMoteLabel = Strings.Fractal_ChallengeMoteLabel;

		public static string ChallengeMoteId = "CM";

		public static string TomorrowLabel = Strings.Fractal_TomorrowLabel;

		public static string TomorrowId = "Tom";

		public static string TierNLabel = Strings.Fractal_TierNLabel;

		public static string TierNId = "T#";

		public static string RecLabel = Strings.Fractal_RecLabel;

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
