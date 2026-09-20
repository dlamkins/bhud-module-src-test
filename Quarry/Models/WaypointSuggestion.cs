namespace Quarry.Models
{
	public class WaypointSuggestion
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public float MetresFromObjective { get; set; }

		public float MetresOnFoot { get; set; }

		public bool WorthTaking { get; set; }

		public string Describe()
		{
			if (Name == null)
			{
				return "Copy waypoint";
			}
			if (WorthTaking)
			{
				return $"Copy {Name}\n{MetresFromObjective:F0} m from your next objective (you're {MetresOnFoot:F0} m away now)";
			}
			return $"You're closer on foot ({MetresOnFoot:F0} m) than any waypoint\nCopy {Name} anyway — {MetresFromObjective:F0} m from it";
		}
	}
}
