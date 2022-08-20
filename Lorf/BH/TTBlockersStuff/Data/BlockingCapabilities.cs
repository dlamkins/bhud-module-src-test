using Gw2Sharp.Models;

namespace Lorf.BH.TTBlockersStuff.Data
{
	internal class BlockingCapabilities
	{
		public static readonly BlockingCapabilities Mesmer = new BlockingCapabilities((ProfessionType)7, 0, BlockingType.All, BlockingType.All, BlockingType.All);

		public static readonly BlockingCapabilities Chronomancer = new BlockingCapabilities((ProfessionType)7, 40, BlockingType.All, BlockingType.AllNotRecommended, BlockingType.Some);

		public static readonly BlockingCapabilities Mirage = new BlockingCapabilities((ProfessionType)7, 59, BlockingType.All, BlockingType.All, BlockingType.All);

		public static readonly BlockingCapabilities Virtuoso = new BlockingCapabilities((ProfessionType)7, 66, BlockingType.All, BlockingType.AllNotRecommended, BlockingType.Some);

		public static readonly BlockingCapabilities Elementalist = new BlockingCapabilities((ProfessionType)6, 0, BlockingType.All, BlockingType.All, BlockingType.Most);

		public static readonly BlockingCapabilities Tempest = new BlockingCapabilities((ProfessionType)6, 48, BlockingType.All, BlockingType.AllNotRecommended, BlockingType.Most);

		public static readonly BlockingCapabilities Weaver = new BlockingCapabilities((ProfessionType)6, 56, BlockingType.All, BlockingType.All, BlockingType.Most);

		public static readonly BlockingCapabilities Catalyst = new BlockingCapabilities((ProfessionType)6, 67, BlockingType.All, BlockingType.AllNotRecommended, BlockingType.Most);

		public static readonly BlockingCapabilities Thief = new BlockingCapabilities((ProfessionType)5, 0, BlockingType.All, BlockingType.All, BlockingType.Most);

		public static readonly BlockingCapabilities Daredevil = new BlockingCapabilities((ProfessionType)5, 7, BlockingType.All, BlockingType.AllNotRecommended, BlockingType.Most);

		public static readonly BlockingCapabilities Deadeye = new BlockingCapabilities((ProfessionType)5, 58, BlockingType.All, BlockingType.All, BlockingType.Most);

		public static readonly BlockingCapabilities Specter = new BlockingCapabilities((ProfessionType)5, 71, BlockingType.All, BlockingType.AllNotRecommended, BlockingType.Most);

		public ProfessionType Profession { get; }

		public int Specialization { get; }

		public BlockingType EggBlocking { get; }

		public BlockingType HuskBlocking { get; }

		public BlockingType AoEBlocking { get; }

		public bool SylvariRequired { get; }

		public bool SylvariRecommended { get; }

		public BlockingCapabilities(ProfessionType profession, int specialization, BlockingType eggBlocking, BlockingType huskBlocking, BlockingType aoEBlocking)
		{
		}
	}
}
