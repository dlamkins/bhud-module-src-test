namespace TurtleMyWaypoint
{
	internal readonly struct MapEntry
	{
		public readonly string NameEn;

		public readonly string NameFr;

		public readonly string[] Codes;

		public readonly string[] WaypointNamesEn;

		public readonly string[] WaypointNamesFr;

		public string Name
		{
			get
			{
				if (!Strings.IsFrench)
				{
					return NameEn;
				}
				return NameFr;
			}
		}

		public MapEntry(string nameEn, string nameFr, string[] codes, string[] waypointNamesEn, string[] waypointNamesFr)
		{
			NameEn = nameEn;
			NameFr = nameFr;
			Codes = codes;
			WaypointNamesEn = waypointNamesEn;
			WaypointNamesFr = waypointNamesFr;
		}

		public string WaypointName(int index)
		{
			if (!Strings.IsFrench)
			{
				return WaypointNamesEn[index];
			}
			return WaypointNamesFr[index];
		}
	}
}
