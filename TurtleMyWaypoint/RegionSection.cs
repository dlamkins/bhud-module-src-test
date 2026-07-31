namespace TurtleMyWaypoint
{
	internal readonly struct RegionSection
	{
		public readonly string RegionNameEn;

		public readonly string RegionNameFr;

		public readonly MapEntry[] Maps;

		public string RegionName
		{
			get
			{
				if (!Strings.IsFrench)
				{
					return RegionNameEn;
				}
				return RegionNameFr;
			}
		}

		public RegionSection(string regionNameEn, string regionNameFr, MapEntry[] maps)
		{
			RegionNameEn = regionNameEn;
			RegionNameFr = regionNameFr;
			Maps = maps;
		}
	}
}
