namespace TurtleMyWaypoint
{
	internal readonly struct MapEntry
	{
		public readonly string NameEn;

		public readonly string NameFr;

		public readonly string[] Codes;

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

		public MapEntry(string nameEn, string nameFr, string[] codes)
		{
			NameEn = nameEn;
			NameFr = nameFr;
			Codes = codes;
		}
	}
}
