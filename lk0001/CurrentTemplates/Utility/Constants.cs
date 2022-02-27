using System.Collections.Generic;

namespace lk0001.CurrentTemplates.Utility
{
	internal class Constants
	{
		public enum Profession
		{
			None,
			Guardian,
			Warrior,
			Engineer,
			Ranger,
			Thief,
			Elementalist,
			Mesmer,
			Necromancer,
			Revenant
		}

		public enum Specialization
		{
			None,
			MesmerDueling,
			NecromancerDeathMagic,
			RevenantInvocation,
			WarriorStrength,
			RangerDruid,
			EngineerExplosives,
			ThiefDaredevil,
			RangerMarksmanship,
			RevenantRetribution,
			MesmerDomination,
			WarriorTactics,
			RevenantSalvation,
			GuardianValor,
			RevenantCorruption,
			RevenantDevastation,
			GuardianRadiance,
			ElementalistWater,
			WarriorBerserker,
			NecromancerBloodMagic,
			ThiefShadowArts,
			EngineerTools,
			WarriorDefense,
			MesmerInspiration,
			MesmerIllusions,
			RangerNatureMagic,
			ElementalistEarth,
			GuardianDragonhunter,
			ThiefDeadlyArts,
			EngineerAlchemy,
			RangerSkirmishing,
			ElementalistFire,
			RangerBeastmastery,
			RangerWildernessSurvival,
			NecromancerReaper,
			ThiefCriticalStrikes,
			WarriorArms,
			ElementalistArcane,
			EngineerFirearms,
			NecromancerCurses,
			MesmerChronomancer,
			ElementalistAir,
			GuardianZeal,
			EngineerScrapper,
			ThiefTrickery,
			MesmerChaos,
			GuardianVirtues,
			EngineerInventions,
			ElementalistTempest,
			GuardianHonor,
			NecromancerSoulReaping,
			WarriorDiscipline,
			RevenantHerald,
			NecromancerSpite,
			ThiefAcrobatics,
			RangerSoulbeast,
			ElementalistWeaver,
			EngineerHolosmith,
			ThiefDeadeye,
			MesmerMirage,
			NecromancerScourge,
			WarriorSpellbreaker,
			GuardianFirebrand,
			RevenantRenegade,
			NecromancerHarbinger,
			GuardianWillbender,
			MesmerVirtuoso,
			ElementalistCatalyst,
			WarriorBladesworn,
			RevenantVindicator,
			EngineerMechanist,
			ThiefSpecter,
			RangerUntamed
		}

		public static readonly Dictionary<Specialization, TraitLine> TraitLines = new Dictionary<Specialization, TraitLine>
		{
			{
				Specialization.None,
				new TraitLine("", new int[3][]
				{
					new int[4],
					new int[4],
					new int[4]
				})
			},
			{
				Specialization.MesmerDueling,
				new TraitLine("Dueling", new int[3][]
				{
					new int[4] { 0, 701, 705, 700 },
					new int[4] { 0, 1889, 1960, 708 },
					new int[4] { 0, 692, 1950, 704 }
				})
			},
			{
				Specialization.NecromancerDeathMagic,
				new TraitLine("Death Magic", new int[3][]
				{
					new int[4] { 0, 820, 857, 1922 },
					new int[4] { 0, 858, 860, 855 },
					new int[4] { 0, 842, 1940, 1694 }
				})
			},
			{
				Specialization.RevenantInvocation,
				new TraitLine("Invocation", new int[3][]
				{
					new int[4] { 0, 1732, 1761, 1784 },
					new int[4] { 0, 1774, 1760, 1781 },
					new int[4] { 0, 1749, 1791, 1719 }
				})
			},
			{
				Specialization.WarriorStrength,
				new TraitLine("Strength", new int[3][]
				{
					new int[4] { 0, 1447, 1451, 1444 },
					new int[4] { 0, 2000, 1338, 1449 },
					new int[4] { 0, 1437, 1454, 1440 }
				})
			},
			{
				Specialization.RangerDruid,
				new TraitLine("Druid", new int[3][]
				{
					new int[4] { 0, 1868, 2016, 1935 },
					new int[4] { 0, 2053, 2001, 2056 },
					new int[4] { 0, 2057, 2058, 2055 }
				}, elite: true)
			},
			{
				Specialization.EngineerExplosives,
				new TraitLine("Explosives", new int[3][]
				{
					new int[4] { 0, 514, 525, 1882 },
					new int[4] { 0, 482, 1892, 1944 },
					new int[4] { 0, 1541, 505, 1947 }
				})
			},
			{
				Specialization.ThiefDaredevil,
				new TraitLine("Daredevil", new int[3][]
				{
					new int[4] { 0, 1933, 2023, 1949 },
					new int[4] { 0, 1884, 1893, 1975 },
					new int[4] { 0, 1833, 1964, 2047 }
				}, elite: true)
			},
			{
				Specialization.RangerMarksmanship,
				new TraitLine("Marksmanship", new int[3][]
				{
					new int[4] { 0, 1021, 1014, 986 },
					new int[4] { 0, 1001, 1000, 1070 },
					new int[4] { 0, 996, 1015, 1698 }
				})
			},
			{
				Specialization.RevenantRetribution,
				new TraitLine("Retribution", new int[3][]
				{
					new int[4] { 0, 1811, 1728, 1810 },
					new int[4] { 0, 1766, 1782, 1740 },
					new int[4] { 0, 1779, 1770, 1790 }
				})
			},
			{
				Specialization.MesmerDomination,
				new TraitLine("Domination", new int[3][]
				{
					new int[4] { 0, 686, 682, 687 },
					new int[4] { 0, 693, 713, 712 },
					new int[4] { 0, 681, 680, 1688 }
				})
			},
			{
				Specialization.WarriorTactics,
				new TraitLine("Tactics", new int[3][]
				{
					new int[4] { 0, 1469, 1474, 1471 },
					new int[4] { 0, 1486, 1479, 1482 },
					new int[4] { 0, 1667, 1470, 1711 }
				})
			},
			{
				Specialization.RevenantSalvation,
				new TraitLine("Salvation", new int[3][]
				{
					new int[4] { 0, 1823, 1824, 1822 },
					new int[4] { 0, 1819, 1817, 1818 },
					new int[4] { 0, 1815, 1825, 1820 }
				})
			},
			{
				Specialization.GuardianValor,
				new TraitLine("Valor", new int[3][]
				{
					new int[4] { 0, 588, 581, 633 },
					new int[4] { 0, 580, 584, 1684 },
					new int[4] { 0, 585, 586, 589 }
				})
			},
			{
				Specialization.RevenantCorruption,
				new TraitLine("Corruption", new int[3][]
				{
					new int[4] { 0, 1793, 1789, 1741 },
					new int[4] { 0, 1727, 1726, 1714 },
					new int[4] { 0, 1795, 1720, 1721 }
				})
			},
			{
				Specialization.RevenantDevastation,
				new TraitLine("Devastation", new int[3][]
				{
					new int[4] { 0, 1776, 1767, 1755 },
					new int[4] { 0, 1786, 1765, 1802 },
					new int[4] { 0, 1715, 1800, 1754 }
				})
			},
			{
				Specialization.GuardianRadiance,
				new TraitLine("Radiance", new int[3][]
				{
					new int[4] { 0, 577, 566, 574 },
					new int[4] { 0, 578, 567, 565 },
					new int[4] { 0, 1686, 579, 1683 }
				})
			},
			{
				Specialization.ElementalistWater,
				new TraitLine("Water", new int[3][]
				{
					new int[4] { 0, 348, 363, 360 },
					new int[4] { 0, 364, 358, 349 },
					new int[4] { 0, 362, 361, 2028 }
				})
			},
			{
				Specialization.WarriorBerserker,
				new TraitLine("Berserker", new int[3][]
				{
					new int[4] { 0, 2049, 2039, 1977 },
					new int[4] { 0, 2011, 2042, 2002 },
					new int[4] { 0, 1928, 2038, 2043 }
				}, elite: true)
			},
			{
				Specialization.NecromancerBloodMagic,
				new TraitLine("Blood Magic", new int[3][]
				{
					new int[4] { 0, 780, 788, 1876 },
					new int[4] { 0, 789, 799, 1844 },
					new int[4] { 0, 782, 1692, 778 }
				})
			},
			{
				Specialization.ThiefShadowArts,
				new TraitLine("Shadow Arts", new int[3][]
				{
					new int[4] { 0, 1160, 1293, 1284 },
					new int[4] { 0, 1297, 1130, 1300 },
					new int[4] { 0, 1134, 1135, 1162 }
				})
			},
			{
				Specialization.EngineerTools,
				new TraitLine("Tools", new int[3][]
				{
					new int[4] { 0, 532, 1997, 531 },
					new int[4] { 0, 512, 1946, 1832 },
					new int[4] { 0, 1856, 523, 1679 }
				})
			},
			{
				Specialization.WarriorDefense,
				new TraitLine("Defense", new int[3][]
				{
					new int[4] { 0, 1376, 1488, 1372 },
					new int[4] { 0, 1368, 1379, 1367 },
					new int[4] { 0, 1375, 1649, 1708 }
				})
			},
			{
				Specialization.MesmerInspiration,
				new TraitLine("Inspiration", new int[3][]
				{
					new int[4] { 0, 756, 738, 744 },
					new int[4] { 0, 751, 740, 1980 },
					new int[4] { 0, 2005, 1866, 752 }
				})
			},
			{
				Specialization.MesmerIllusions,
				new TraitLine("Illusions", new int[3][]
				{
					new int[4] { 0, 721, 1869, 691 },
					new int[4] { 0, 722, 729, 1690 },
					new int[4] { 0, 733, 2035, 753 }
				})
			},
			{
				Specialization.RangerNatureMagic,
				new TraitLine("Nature Magic", new int[3][]
				{
					new int[4] { 0, 1062, 978, 1060 },
					new int[4] { 0, 1054, 965, 964 },
					new int[4] { 0, 1038, 1988, 1697 }
				})
			},
			{
				Specialization.ElementalistEarth,
				new TraitLine("Earth", new int[3][]
				{
					new int[4] { 0, 282, 1507, 289 },
					new int[4] { 0, 275, 281, 277 },
					new int[4] { 0, 1508, 287, 1674 }
				})
			},
			{
				Specialization.GuardianDragonhunter,
				new TraitLine("Dragonhunter", new int[3][]
				{
					new int[4] { 0, 1898, 1983, 1911 },
					new int[4] { 0, 2037, 1835, 1943 },
					new int[4] { 0, 1908, 1963, 1955 }
				}, elite: true)
			},
			{
				Specialization.ThiefDeadlyArts,
				new TraitLine("Deadly Arts", new int[3][]
				{
					new int[4] { 0, 1245, 1276, 1164 },
					new int[4] { 0, 1169, 1292, 1704 },
					new int[4] { 0, 1291, 1167, 1269 }
				})
			},
			{
				Specialization.EngineerAlchemy,
				new TraitLine("Alchemy", new int[3][]
				{
					new int[4] { 0, 396, 509, 521 },
					new int[4] { 0, 520, 469, 470 },
					new int[4] { 0, 473, 1871, 1854 }
				})
			},
			{
				Specialization.RangerSkirmishing,
				new TraitLine("Skirmishing", new int[3][]
				{
					new int[4] { 0, 1069, 1067, 1075 },
					new int[4] { 0, 1016, 1700, 1846 },
					new int[4] { 0, 1064, 1912, 1888 }
				})
			},
			{
				Specialization.ElementalistFire,
				new TraitLine("Fire", new int[3][]
				{
					new int[4] { 0, 296, 328, 335 },
					new int[4] { 0, 325, 340, 334 },
					new int[4] { 0, 1510, 294, 1675 }
				})
			},
			{
				Specialization.RangerBeastmastery,
				new TraitLine("Beastmastery", new int[3][]
				{
					new int[4] { 0, 1861, 1072, 1606 },
					new int[4] { 0, 975, 1047, 970 },
					new int[4] { 0, 1945, 968, 1066 }
				})
			},
			{
				Specialization.RangerWildernessSurvival,
				new TraitLine("Wilderness Survival", new int[3][]
				{
					new int[4] { 0, 1098, 1086, 1099 },
					new int[4] { 0, 1101, 2032, 1100 },
					new int[4] { 0, 1094, 1699, 1701 }
				})
			},
			{
				Specialization.NecromancerReaper,
				new TraitLine("Reaper", new int[3][]
				{
					new int[4] { 0, 1974, 2020, 2026 },
					new int[4] { 0, 1969, 2008, 2031 },
					new int[4] { 0, 1932, 1919, 2021 }
				}, elite: true)
			},
			{
				Specialization.ThiefCriticalStrikes,
				new TraitLine("Critical Strikes", new int[3][]
				{
					new int[4] { 0, 1209, 1267, 1268 },
					new int[4] { 0, 1170, 1272, 1299 },
					new int[4] { 0, 1904, 1215, 1702 }
				})
			},
			{
				Specialization.WarriorArms,
				new TraitLine("Arms", new int[3][]
				{
					new int[4] { 0, 1455, 1344, 1334 },
					new int[4] { 0, 1315, 1316, 1333 },
					new int[4] { 0, 1336, 1346, 1707 }
				})
			},
			{
				Specialization.ElementalistArcane,
				new TraitLine("Arcane", new int[3][]
				{
					new int[4] { 0, 253, 266, 1487 },
					new int[4] { 0, 265, 1673, 257 },
					new int[4] { 0, 238, 263, 1511 }
				})
			},
			{
				Specialization.EngineerFirearms,
				new TraitLine("Firearms", new int[3][]
				{
					new int[4] { 0, 1878, 1930, 1914 },
					new int[4] { 0, 1984, 2006, 1923 },
					new int[4] { 0, 510, 526, 433 }
				})
			},
			{
				Specialization.NecromancerCurses,
				new TraitLine("Curses", new int[3][]
				{
					new int[4] { 0, 1883, 2013, 815 },
					new int[4] { 0, 816, 1693, 812 },
					new int[4] { 0, 813, 1696, 801 }
				})
			},
			{
				Specialization.MesmerChronomancer,
				new TraitLine("Chronomancer", new int[3][]
				{
					new int[4] { 0, 1838, 1995, 1987 },
					new int[4] { 0, 2009, 1913, 1978 },
					new int[4] { 0, 1942, 2022, 1890 }
				}, elite: true)
			},
			{
				Specialization.ElementalistAir,
				new TraitLine("Air", new int[3][]
				{
					new int[4] { 0, 227, 224, 232 },
					new int[4] { 0, 229, 214, 1502 },
					new int[4] { 0, 226, 1503, 1672 }
				})
			},
			{
				Specialization.GuardianZeal,
				new TraitLine("Zeal", new int[3][]
				{
					new int[4] { 0, 563, 634, 1925 },
					new int[4] { 0, 628, 653, 1556 },
					new int[4] { 0, 635, 637, 2017 }
				})
			},
			{
				Specialization.EngineerScrapper,
				new TraitLine("Scrapper", new int[3][]
				{
					new int[4] { 0, 1917, 1971, 1867 },
					new int[4] { 0, 1954, 1999, 1860 },
					new int[4] { 0, 1981, 2052, 1849 }
				}, elite: true)
			},
			{
				Specialization.ThiefTrickery,
				new TraitLine("Trickery", new int[3][]
				{
					new int[4] { 0, 1159, 1252, 1163 },
					new int[4] { 0, 1277, 1286, 1190 },
					new int[4] { 0, 1187, 1158, 1706 }
				})
			},
			{
				Specialization.MesmerChaos,
				new TraitLine("Chaos", new int[3][]
				{
					new int[4] { 0, 670, 675, 677 },
					new int[4] { 0, 673, 668, 669 },
					new int[4] { 0, 671, 674, 1687 }
				})
			},
			{
				Specialization.GuardianVirtues,
				new TraitLine("Virtues", new int[3][]
				{
					new int[4] { 0, 624, 625, 617 },
					new int[4] { 0, 603, 610, 587 },
					new int[4] { 0, 622, 554, 612 }
				})
			},
			{
				Specialization.EngineerInventions,
				new TraitLine("Inventions", new int[3][]
				{
					new int[4] { 0, 394, 1901, 507 },
					new int[4] { 0, 1678, 1834, 445 },
					new int[4] { 0, 472, 1680, 1916 }
				})
			},
			{
				Specialization.ElementalistTempest,
				new TraitLine("Tempest", new int[3][]
				{
					new int[4] { 0, 1952, 1962, 1886 },
					new int[4] { 0, 1891, 1902, 2015 },
					new int[4] { 0, 1839, 2033, 1986 }
				}, elite: true)
			},
			{
				Specialization.GuardianHonor,
				new TraitLine("Honor", new int[3][]
				{
					new int[4] { 0, 1899, 559, 654 },
					new int[4] { 0, 557, 549, 562 },
					new int[4] { 0, 553, 558, 1682 }
				})
			},
			{
				Specialization.NecromancerSoulReaping,
				new TraitLine("Soul Reaping", new int[3][]
				{
					new int[4] { 0, 875, 898, 888 },
					new int[4] { 0, 894, 861, 892 },
					new int[4] { 0, 889, 893, 905 }
				})
			},
			{
				Specialization.WarriorDiscipline,
				new TraitLine("Discipline", new int[3][]
				{
					new int[4] { 0, 1329, 1413, 1381 },
					new int[4] { 0, 1484, 1489, 1709 },
					new int[4] { 0, 1369, 1317, 1657 }
				})
			},
			{
				Specialization.RevenantHerald,
				new TraitLine("Herald", new int[3][]
				{
					new int[4] { 0, 1813, 1806, 1716 },
					new int[4] { 0, 1738, 1743, 1730 },
					new int[4] { 0, 1746, 1772, 1803 }
				}, elite: true)
			},
			{
				Specialization.NecromancerSpite,
				new TraitLine("Spite", new int[3][]
				{
					new int[4] { 0, 914, 916, 1863 },
					new int[4] { 0, 899, 829, 909 },
					new int[4] { 0, 919, 853, 903 }
				})
			},
			{
				Specialization.ThiefAcrobatics,
				new TraitLine("Acrobatics", new int[3][]
				{
					new int[4] { 0, 1112, 1289, 1237 },
					new int[4] { 0, 1241, 1192, 1290 },
					new int[4] { 0, 1238, 1295, 1703 }
				})
			},
			{
				Specialization.RangerSoulbeast,
				new TraitLine("Soulbeast", new int[3][]
				{
					new int[4] { 0, 2134, 2071, 2072 },
					new int[4] { 0, 2119, 2085, 2161 },
					new int[4] { 0, 2155, 2128, 2143 }
				}, elite: true)
			},
			{
				Specialization.ElementalistWeaver,
				new TraitLine("Weaver", new int[3][]
				{
					new int[4] { 0, 2177, 2165, 2115 },
					new int[4] { 0, 2180, 2061, 2170 },
					new int[4] { 0, 2131, 2090, 2138 }
				}, elite: true)
			},
			{
				Specialization.EngineerHolosmith,
				new TraitLine("Holosmith", new int[3][]
				{
					new int[4] { 0, 2114, 2157, 2106 },
					new int[4] { 0, 2103, 2152, 2091 },
					new int[4] { 0, 2066, 2137, 2064 }
				}, elite: true)
			},
			{
				Specialization.ThiefDeadeye,
				new TraitLine("Deadeye", new int[3][]
				{
					new int[4] { 0, 2145, 2173, 2136 },
					new int[4] { 0, 2118, 2078, 2160 },
					new int[4] { 0, 2111, 2093, 2146 }
				}, elite: true)
			},
			{
				Specialization.MesmerMirage,
				new TraitLine("Mirage", new int[3][]
				{
					new int[4] { 0, 2141, 2082, 2110 },
					new int[4] { 0, 2178, 2174, 2098 },
					new int[4] { 0, 2070, 2113, 2169 }
				}, elite: true)
			},
			{
				Specialization.NecromancerScourge,
				new TraitLine("Scourge", new int[3][]
				{
					new int[4] { 0, 2167, 2074, 2102 },
					new int[4] { 0, 2059, 2067, 2123 },
					new int[4] { 0, 2112, 2164, 2080 }
				}, elite: true)
			},
			{
				Specialization.WarriorSpellbreaker,
				new TraitLine("Spellbreaker", new int[3][]
				{
					new int[4] { 0, 2107, 2153, 2140 },
					new int[4] { 0, 2126, 2097, 2095 },
					new int[4] { 0, 2163, 2168, 2060 }
				}, elite: true)
			},
			{
				Specialization.GuardianFirebrand,
				new TraitLine("Firebrand", new int[3][]
				{
					new int[4] { 0, 2075, 2101, 2086 },
					new int[4] { 0, 2063, 2076, 2116 },
					new int[4] { 0, 2105, 2179, 2159 }
				}, elite: true)
			},
			{
				Specialization.RevenantRenegade,
				new TraitLine("Renegade", new int[3][]
				{
					new int[4] { 0, 2166, 2079, 2120 },
					new int[4] { 0, 2133, 2092, 2108 },
					new int[4] { 0, 2094, 2100, 2182 }
				}, elite: true)
			},
			{
				Specialization.NecromancerHarbinger,
				new TraitLine("Harbinger", new int[3][]
				{
					new int[4] { 0, 2188, 2219, 2185 },
					new int[4] { 0, 2192, 2220, 2209 },
					new int[4] { 0, 2218, 2194, 2203 }
				}, elite: true)
			},
			{
				Specialization.GuardianWillbender,
				new TraitLine("Willbender", new int[3][]
				{
					new int[4] { 0, 2191, 2190, 2187 },
					new int[4] { 0, 2197, 2210, 2199 },
					new int[4] { 0, 2195, 2201, 2198 }
				}, elite: true)
			},
			{
				Specialization.MesmerVirtuoso,
				new TraitLine("Virtuoso", new int[3][]
				{
					new int[4] { 0, 2212, 2208, 2202 },
					new int[4] { 0, 2215, 2205, 2207 },
					new int[4] { 0, 2211, 2206, 2223 }
				}, elite: true)
			},
			{
				Specialization.ElementalistCatalyst,
				new TraitLine("Catalyst", new int[3][]
				{
					new int[4] { 0, 2230, 2252, 2224 },
					new int[4] { 0, 2247, 2249, 2234 },
					new int[4] { 0, 2233, 2241, 2251 }
				}, elite: true)
			},
			{
				Specialization.WarriorBladesworn,
				new TraitLine("Bladesworn", new int[3][]
				{
					new int[4] { 0, 2237, 2260, 2225 },
					new int[4] { 0, 2253, 2244, 2240 },
					new int[4] { 0, 2261, 2239, 2245 }
				}, elite: true)
			},
			{
				Specialization.RevenantVindicator,
				new TraitLine("Vindicator", new int[3][]
				{
					new int[4] { 0, 2258, 2248, 2228 },
					new int[4] { 0, 2259, 2243, 2255 },
					new int[4] { 0, 2257, 2232, 2238 }
				}, elite: true)
			},
			{
				Specialization.EngineerMechanist,
				new TraitLine("Mechanist", new int[3][]
				{
					new int[4] { 0, 2282, 2296, 2279 },
					new int[4] { 0, 2270, 2276, 2294 },
					new int[4] { 0, 2292, 2281, 2298 }
				}, elite: true)
			},
			{
				Specialization.ThiefSpecter,
				new TraitLine("Specter", new int[3][]
				{
					new int[4] { 0, 2284, 2299, 2275 },
					new int[4] { 0, 2290, 2288, 2285 },
					new int[4] { 0, 2264, 2300, 2289 }
				}, elite: true)
			},
			{
				Specialization.RangerUntamed,
				new TraitLine("Untamed", new int[3][]
				{
					new int[4] { 0, 2297, 2277, 2301 },
					new int[4] { 0, 2263, 2287, 2278 },
					new int[4] { 0, 2271, 2283, 2274 }
				}, elite: true)
			}
		};

		public static Profession GetProfessionId(string profession)
		{
			return profession switch
			{
				"Guardian" => Profession.Guardian, 
				"Warrior" => Profession.Warrior, 
				"Engineer" => Profession.Engineer, 
				"Ranger" => Profession.Ranger, 
				"Thief" => Profession.Thief, 
				"Elementalist" => Profession.Elementalist, 
				"Mesmer" => Profession.Mesmer, 
				"Necromancer" => Profession.Necromancer, 
				"Revenant" => Profession.Revenant, 
				_ => Profession.None, 
			};
		}
	}
}
