using System.Collections.Generic;
using Blish_HUD;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class FishingMaps
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(FishingMaps));

		private Dictionary<int, List<int>> _mapAchievements;

		public static readonly int FISHING_ACHIEVEMENT_CATEGORY_ID = 317;

		public static readonly List<int> AscalonianFisher = new List<int> { 6330, 6484 };

		public static readonly List<int> AscalonianMaps = new List<int> { 22, 32, 19, 1330, 21, 25 };

		public static readonly List<int> KrytanFisher = new List<int> { 6068, 6263 };

		public static readonly List<int> KrytanMaps = new List<int> { 73, 17, 24, 50, 873, 23, 15, 1185 };

		public static readonly List<int> MaguumaFisher = new List<int> { 6344, 6475 };

		public static readonly List<int> MaguumaMaps = new List<int>
		{
			53, 39, 34, 35, 54, 1068, 1101, 1107, 1108, 1121,
			1069, 1071, 1076, 1104, 1124
		};

		public static readonly List<int> ShiverpeaksFisher = new List<int> { 6179, 6153 };

		public static readonly List<int> ShiverpeaksMaps = new List<int> { 30, 1371, 1310, 29, 27, 31, 28 };

		public static readonly List<int> OrrianFisher = new List<int> { 6363, 6227 };

		public static readonly List<int> OrrianMaps = new List<int> { 1203, 51, 65, 62 };

		public static readonly List<int> DesertFisher = new List<int> { 6317, 6509 };

		public static readonly List<int> DesertMaps = new List<int>
		{
			1210, 1288, 1226, 1228, 1211, 1214, 1215, 1224, 1232, 1243,
			1250
		};

		public static readonly List<int> DesertIslesFisher = new List<int> { 6106, 6250 };

		public static readonly List<int> DesertIslesMaps = new List<int> { 1263, 1271 };

		public static readonly List<int> RingOfFireFisher = new List<int> { 6489, 6339 };

		public static readonly List<int> RingOfFireMaps = new List<int> { 1175, 1195 };

		public static readonly List<int> CanthaMaps = new List<int> { 1442, 1419, 1444, 1462, 1438, 1452, 1428, 1422 };

		public static readonly List<int> SeitungProvinceFisher = new List<int> { 6336, 6264 };

		public static readonly List<int> SeitungProvinceMaps = new List<int> { 1442, 1419, 1444, 1462 };

		public static readonly List<int> KainengFisher = new List<int> { 6342, 6192 };

		public static readonly List<int> KainengMaps = new List<int> { 1438 };

		public static readonly List<int> EchovaldWildsFisher = new List<int> { 6258, 6466 };

		public static readonly List<int> EchovaldWildsMaps = new List<int> { 1452, 1428 };

		public static readonly List<int> DragonsEndFisher = new List<int> { 6506, 6402 };

		public static readonly List<int> DragonsEndMaps = new List<int> { 1422 };

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

		public Dictionary<int, List<int>> mapAchievements => _mapAchievements;

		public FishingMaps()
		{
			_mapAchievements = new Dictionary<int, List<int>>();
			foreach (int mapId12 in AscalonianMaps)
			{
				_mapAchievements.Add(mapId12, AscalonianFisher);
			}
			foreach (int mapId11 in KrytanMaps)
			{
				_mapAchievements.Add(mapId11, KrytanFisher);
			}
			foreach (int mapId10 in MaguumaMaps)
			{
				_mapAchievements.Add(mapId10, MaguumaFisher);
			}
			foreach (int mapId9 in ShiverpeaksMaps)
			{
				_mapAchievements.Add(mapId9, ShiverpeaksFisher);
			}
			foreach (int mapId8 in OrrianMaps)
			{
				_mapAchievements.Add(mapId8, OrrianFisher);
			}
			foreach (int mapId7 in DesertMaps)
			{
				_mapAchievements.Add(mapId7, DesertFisher);
			}
			foreach (int mapId6 in DesertIslesMaps)
			{
				_mapAchievements.Add(mapId6, DesertIslesFisher);
			}
			foreach (int mapId5 in RingOfFireMaps)
			{
				_mapAchievements.Add(mapId5, RingOfFireFisher);
			}
			foreach (int mapId4 in SeitungProvinceMaps)
			{
				_mapAchievements.Add(mapId4, SeitungProvinceFisher);
			}
			foreach (int mapId3 in KainengMaps)
			{
				_mapAchievements.Add(mapId3, KainengFisher);
			}
			foreach (int mapId2 in EchovaldWildsMaps)
			{
				_mapAchievements.Add(mapId2, EchovaldWildsFisher);
			}
			foreach (int mapId in DragonsEndMaps)
			{
				_mapAchievements.Add(mapId, DragonsEndFisher);
			}
		}
	}
}
