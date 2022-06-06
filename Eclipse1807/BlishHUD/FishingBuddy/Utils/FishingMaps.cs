using System.Collections.Generic;
using Blish_HUD;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class FishingMaps
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(FishingMaps));

		public static readonly int FISHING_ACHIEVEMENT_CATEGORY_ID = 317;

		public static readonly List<int> AscalonianFisher = new List<int> { 6330, 6484 };

		public static readonly List<int> AscalonianMaps = new List<int> { 22, 32, 19, 1330, 21, 25 };

		public static readonly List<int> KrytanFisher = new List<int> { 6068, 6263 };

		public static readonly List<int> KrytanMaps = new List<int> { 73, 17, 24, 50, 873, 23, 15, 1185 };

		public static readonly List<int> MaguumaFisher = new List<int> { 6344, 6475 };

		public static readonly List<int> MaguumaMaps = new List<int>
		{
			53, 39, 34, 35, 54, 139, 1045, 1068, 1101, 1107,
			1108, 1121, 1069, 1071, 1076, 1104, 1124
		};

		public static readonly List<int> ShiverpeaksFisher = new List<int> { 6179, 6153 };

		public static readonly List<int> ShiverpeaksMaps = new List<int> { 30, 1371, 1310, 29, 27, 31, 28, 1178 };

		public static readonly List<int> OrrianFisher = new List<int> { 6363, 6227 };

		public static readonly List<int> OrrianMaps = new List<int> { 1203, 51, 65, 62 };

		public static readonly List<int> DesertFisher = new List<int> { 6317, 6509 };

		public static readonly List<int> DesertMaps = new List<int>
		{
			1210, 1288, 1226, 1228, 1211, 1248, 1214, 1215, 1224, 1232,
			1243, 1250
		};

		public static readonly List<int> DesertIslesFisher = new List<int> { 6106, 6250 };

		public static readonly List<int> DesertIslesMaps = new List<int> { 1263, 1271 };

		public static readonly List<int> RingOfFireFisher = new List<int> { 6489, 6339 };

		public static readonly List<int> RingOfFireMaps = new List<int> { 1175, 1195 };

		public static readonly int CanthaRegionId = 37;

		public static readonly List<int> CanthaMaps = new List<int> { 1442, 1419, 1444, 1462, 1438, 1452, 1428, 1422 };

		public static readonly List<int> SeitungProvinceFisher = new List<int> { 6336, 6264 };

		public static readonly List<int> SeitungProvinceMaps = new List<int> { 1442, 1419, 1444, 1462 };

		public static readonly List<int> KainengFisher = new List<int> { 6342, 6192 };

		public static readonly List<int> KainengMaps = new List<int> { 1438 };

		public static readonly List<int> EchovaldWildsFisher = new List<int> { 6258, 6466 };

		public static readonly List<int> EchovaldWildsMaps = new List<int> { 1452, 1428 };

		public static readonly List<int> DragonsEndFisher = new List<int> { 6506, 6402 };

		public static readonly List<int> DragonsEndMaps = new List<int> { 1422 };

		public static readonly List<int> ThousandSeasPavilionFisher = new List<int> { 6336, 6264, 6342, 6192 };

		public static readonly List<int> ThousandSeasPavilion = new List<int> { 1465 };

		public static readonly List<int> WorldClassFisher = new List<int> { 6224, 6110 };

		public static readonly List<int> SaltwaterFisher = new List<int> { 6471, 6393 };

		public static readonly List<int> FISHER_ACHIEVEMENT_IDS = new List<int>
		{
			6330, 6484, 6068, 6263, 6344, 6475, 6179, 6153, 6363, 6227,
			6317, 6509, 6106, 6250, 6489, 6339, 6336, 6264, 6342, 6192,
			6258, 6466, 6506, 6402, 6224, 6110, 6471, 6393
		};

		public static readonly List<int> BASE_FISHER_ACHIEVEMENT_IDS = new List<int>
		{
			6330, 6068, 6344, 6179, 6363, 6317, 6106, 6489, 6336, 6342,
			6258, 6506, 6224, 6471
		};

		public static readonly List<int> AVID_FISHER_ACHIEVEMENT_IDS = new List<int>
		{
			6484, 6263, 6475, 6153, 6227, 6509, 6250, 6339, 6264, 6192,
			6466, 6402, 6110, 6393
		};

		public Dictionary<int, List<int>> MapAchievements { get; }

		public FishingMaps()
		{
			MapAchievements = new Dictionary<int, List<int>>();
			foreach (int mapId13 in AscalonianMaps)
			{
				MapAchievements.Add(mapId13, AscalonianFisher);
			}
			foreach (int mapId12 in KrytanMaps)
			{
				MapAchievements.Add(mapId12, KrytanFisher);
			}
			foreach (int mapId11 in MaguumaMaps)
			{
				MapAchievements.Add(mapId11, MaguumaFisher);
			}
			foreach (int mapId10 in ShiverpeaksMaps)
			{
				MapAchievements.Add(mapId10, ShiverpeaksFisher);
			}
			foreach (int mapId9 in OrrianMaps)
			{
				MapAchievements.Add(mapId9, OrrianFisher);
			}
			foreach (int mapId8 in DesertMaps)
			{
				MapAchievements.Add(mapId8, DesertFisher);
			}
			foreach (int mapId7 in DesertIslesMaps)
			{
				MapAchievements.Add(mapId7, DesertIslesFisher);
			}
			foreach (int mapId6 in RingOfFireMaps)
			{
				MapAchievements.Add(mapId6, RingOfFireFisher);
			}
			foreach (int mapId5 in SeitungProvinceMaps)
			{
				MapAchievements.Add(mapId5, SeitungProvinceFisher);
			}
			foreach (int mapId4 in KainengMaps)
			{
				MapAchievements.Add(mapId4, KainengFisher);
			}
			foreach (int mapId3 in EchovaldWildsMaps)
			{
				MapAchievements.Add(mapId3, EchovaldWildsFisher);
			}
			foreach (int mapId2 in DragonsEndMaps)
			{
				MapAchievements.Add(mapId2, DragonsEndFisher);
			}
			foreach (int mapId in ThousandSeasPavilion)
			{
				MapAchievements.Add(mapId, ThousandSeasPavilionFisher);
			}
		}
	}
}