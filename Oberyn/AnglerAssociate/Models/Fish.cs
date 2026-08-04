namespace Oberyn.AnglerAssociate.Models
{
	public class Fish
	{
		public string Name { get; set; }

		public Rarity Rarity { get; set; }

		public Cycle Cycle { get; set; }

		public Region Region { get; set; }

		public Location Location { get; set; }

		public FishingHole? Hole1 { get; set; }

		public FishingHole? Hole2 { get; set; }

		public FishingHole? Hole3 { get; set; }

		public FishingHole? Hole4 { get; set; }

		public Bait Bait { get; set; }

		public TimeOfDay TimeOfDay { get; set; }

		public TimeOfDay? TimeOfDay2 { get; set; }

		public TimeOfDay? HigherChance { get; set; }

		public string FishingPower { get; set; }

		public string Collection { get; set; }

		public int? CollectionId { get; set; }

		public string AvidCollection { get; set; }

		public int? AvidCollectionId { get; set; }

		public int? BitIndex { get; set; }

		public string FoundIn { get; set; }

		public bool IsCatchableAt(TimeOfDay currentState)
		{
			if (TimeOfDay == TimeOfDay.Any)
			{
				return true;
			}
			if (TimeOfDay != currentState)
			{
				return TimeOfDay2 == currentState;
			}
			return true;
		}

		public bool HasHigherChance(TimeOfDay currentState)
		{
			if (HigherChance.HasValue)
			{
				return HigherChance.Value == currentState;
			}
			return false;
		}
	}
}
