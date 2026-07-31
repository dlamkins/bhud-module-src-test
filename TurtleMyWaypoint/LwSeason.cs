namespace TurtleMyWaypoint
{
	internal readonly struct LwSeason
	{
		public readonly int SeasonNumber;

		public readonly MapEntry[] Maps;

		public LwSeason(int seasonNumber, MapEntry[] maps)
		{
			SeasonNumber = seasonNumber;
			Maps = maps;
		}
	}
}
