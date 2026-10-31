using System.Collections.Generic;
using System.Linq;

namespace SeaStove.Gw2Tldr.Data
{
	internal static class MapGuideIndex
	{
		private sealed class Rule
		{
			public int Map;

			public string Category;

			public string Group;

			public string Name;

			public Rule(int map, string category, string group, string name)
			{
				Map = map;
				Category = category;
				Group = group;
				Name = name;
			}
		}

		private static readonly Rule[] Rules = new Rule[105]
		{
			new Rule(24, "metas", "Core", "Ley-Line Anomaly"),
			new Rule(25, "metas", "Core", "Ley-Line Anomaly"),
			new Rule(29, "metas", "Core", "Ley-Line Anomaly"),
			new Rule(30, "metas", "Core", "Claw of Jormag"),
			new Rule(33, "dungeons", "Ascalonian Catacombs", null),
			new Rule(36, "dungeons", "Ascalonian Catacombs", null),
			new Rule(53, "metas", "Core", "Tequatl the Sunless"),
			new Rule(63, "dungeons", "Sorrow's Embrace", null),
			new Rule(64, "dungeons", "Sorrow's Embrace", null),
			new Rule(66, "dungeons", "Citadel of Flame", null),
			new Rule(67, "dungeons", "Twilight Arbor", null),
			new Rule(68, "dungeons", "Twilight Arbor", null),
			new Rule(69, "dungeons", "Citadel of Flame", null),
			new Rule(70, "dungeons", "Honor of the Waves", null),
			new Rule(71, "dungeons", "Honor of the Waves", null),
			new Rule(73, "metas", "Core", "Triple Trouble"),
			new Rule(75, "dungeons", "Caudecus's Manor", null),
			new Rule(76, "dungeons", "Caudecus's Manor", null),
			new Rule(81, "dungeons", "Crucible of Eternity", null),
			new Rule(82, "dungeons", "Crucible of Eternity", null),
			new Rule(112, "dungeons", "The Ruined City of Arah", null),
			new Rule(873, "metas", "Core", "Karka Queen"),
			new Rule(947, "fractals", "Fractals", "Uncategorized"),
			new Rule(948, "fractals", "Fractals", "Snowblind"),
			new Rule(949, "fractals", "Fractals", "Swampland"),
			new Rule(950, "fractals", "Fractals", "Urban Battleground"),
			new Rule(951, "fractals", "Fractals", "Aquatic Ruins"),
			new Rule(952, "fractals", "Fractals", "Cliffside"),
			new Rule(953, "fractals", "Fractals", "Underground Facility"),
			new Rule(954, "fractals", "Fractals", "Volcanic"),
			new Rule(955, "fractals", "Fractals", "Molten Furnace"),
			new Rule(956, "fractals", "Fractals", "Aetherblade"),
			new Rule(957, "fractals", "Fractals", "Thaumanova Reactor"),
			new Rule(958, "fractals", "Fractals", "Solid Ocean"),
			new Rule(959, "fractals", "Fractals", "Molten Boss"),
			new Rule(960, "fractals", "Fractals", "Captain Mai Trin"),
			new Rule(1042, "metas", "Heart of Thorns", "Night and the Enemy"),
			new Rule(1043, "metas", "Heart of Thorns", "Battle in Tarir (Octovine)"),
			new Rule(1045, "metas", "Heart of Thorns", "Chak Gerent"),
			new Rule(1052, "metas", "Heart of Thorns", "Night and the Enemy"),
			new Rule(1062, "raids", "Wing 1", null),
			new Rule(1149, "raids", "Wing 2", null),
			new Rule(1156, "raids", "Wing 3", null),
			new Rule(1164, "fractals", "Fractals", "Chaos"),
			new Rule(1177, "fractals", "Fractals", "Nightmare"),
			new Rule(1188, "raids", "Wing 4", null),
			new Rule(1205, "fractals", "Fractals", "Shattered Observatory"),
			new Rule(1210, "metas", "Path of Fire / LWS4", "Choya Piñata (Casino Blitz)"),
			new Rule(1226, "metas", "Path of Fire / LWS4", "Junundu Rising"),
			new Rule(1228, "metas", "Path of Fire / LWS4", "The Path to Ascension"),
			new Rule(1248, "metas", "Path of Fire / LWS4", "Forged with Fire"),
			new Rule(1263, "metas", "Path of Fire / LWS4", "Palawadan, Jewel of Istan"),
			new Rule(1264, "raids", "Wing 5", null),
			new Rule(1267, "fractals", "Fractals", "Twilight Oasis"),
			new Rule(1290, "fractals", "Fractals", "Deepstone"),
			new Rule(1301, "metas", "Path of Fire / LWS4", "Death-Branded Shatterer"),
			new Rule(1303, "raids", "Wing 6", null),
			new Rule(1309, "fractals", "Fractals", "Siren's Reef"),
			new Rule(1310, "metas", "Path of Fire / LWS4", "The Oil Floes"),
			new Rule(1323, "raids", "Wing 7", null),
			new Rule(1330, "metas", "The Icebrood Saga", "Flame Legion Effigy"),
			new Rule(1330, "metas", "The Icebrood Saga", "Doomlore Shrine"),
			new Rule(1330, "metas", "The Icebrood Saga", "Ooze Pits"),
			new Rule(1332, "strikes", "Icebrood Saga", "Shiverpeaks Pass"),
			new Rule(1339, "strikes", "Icebrood Saga", "Boneskinner"),
			new Rule(1341, "strikes", "Icebrood Saga", "Fraenir of Jormag"),
			new Rule(1343, "metas", "The Icebrood Saga", "Drakkar"),
			new Rule(1346, "strikes", "Icebrood Saga", "Voice & Claw of the Fallen"),
			new Rule(1359, "strikes", "Icebrood Saga", "Whisper of Jormag"),
			new Rule(1374, "strikes", "Icebrood Saga", "Cold War"),
			new Rule(1384, "fractals", "Fractals", "Sunqua Peak"),
			new Rule(1409, "metas", "The Icebrood Saga", "Dragonstorm"),
			new Rule(1412, "metas", "The Icebrood Saga", "Dragonstorm"),
			new Rule(1414, "metas", "Core", "Twisted Marionette"),
			new Rule(1422, "metas", "End of Dragons", "The Battle for the Jade Sea"),
			new Rule(1432, "strikes", "End of Dragons", "Aetherblade Hideout"),
			new Rule(1437, "strikes", "End of Dragons", "Harvest Temple"),
			new Rule(1438, "metas", "End of Dragons", "Adolescent Leviathan"),
			new Rule(1438, "metas", "End of Dragons", "Kaineng Blackout"),
			new Rule(1442, "metas", "End of Dragons", "Adolescent Leviathan"),
			new Rule(1442, "metas", "End of Dragons", "Aetherblade Assault"),
			new Rule(1450, "strikes", "End of Dragons", "Xunlai Jade Junkyard"),
			new Rule(1451, "strikes", "End of Dragons", "Kaineng Overlook"),
			new Rule(1452, "metas", "End of Dragons", "Assault on Fort Aspenwood"),
			new Rule(1452, "metas", "End of Dragons", "The Gang War of Echovald"),
			new Rule(1480, "metas", "Core", "Twisted Marionette"),
			new Rule(1485, "strikes", "Base Game", "Old Lion's Court"),
			new Rule(1500, "fractals", "Fractals", "Silent Surf"),
			new Rule(1510, "metas", "Secrets of the Obscure", "Unlocking the Wizard's Tower"),
			new Rule(1515, "strikes", "Secrets of the Obscure", "Cosmic Observatory"),
			new Rule(1517, "metas", "Secrets of the Obscure", "The Defense of Amnytas"),
			new Rule(1520, "strikes", "Secrets of the Obscure", "Temple of Febe (Cerus)"),
			new Rule(1526, "metas", "Secrets of the Obscure", "Into the Spider's Lair"),
			new Rule(1538, "fractals", "Fractals", "Lonely Tower"),
			new Rule(1554, "metas", "Janthir Wilds", "Of Mists and Monsters"),
			new Rule(1564, "raids", "Wing 8", null),
			new Rule(1574, "metas", "Janthir Wilds", "A Titanic Voyage"),
			new Rule(1584, "fractals", "Fractals", "Kinfall"),
			new Rule(1593, "metas", "Visions of Eternity", "Secrets of the Weald"),
			new Rule(1595, "metas", "Visions of Eternity", "Hammerhart Rumble"),
			new Rule(1609, "strikes", "Visions of Eternity", "Guardian's Glade"),
			new Rule(1622, "metas", "Visions of Eternity", "Shackles of the Ancients"),
			new Rule(1633, "metas", "Visions of Eternity", "Depths of Cruelty"),
			new Rule(1636, "fractals", "Fractals", "Solitary Throne"),
			new Rule(1638, "strikes", "Visions of Eternity", "Nexus of Eternity")
		};

		public static List<GuideEntry> Find(GuideCatalog catalog, int mapId)
		{
			if (catalog == null || mapId <= 0)
			{
				return new List<GuideEntry>();
			}
			return catalog.Guides.Where((GuideEntry guide) => Rules.Any((Rule rule) => rule.Map == mapId && rule.Category == guide.Category && rule.Group == guide.Group && (rule.Name == null || rule.Name == guide.Name))).ToList();
		}
	}
}
