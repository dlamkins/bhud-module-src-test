using System;
using System.Collections.Generic;
using Taskmaster.Models;

namespace Taskmaster.Services
{
	public static class PsnaRotation
	{
		public static readonly DateTime AnchorUtc = new DateTime(2015, 10, 29, 0, 0, 0, DateTimeKind.Utc) + ResetEngine.PsnaResetAtUtc;

		public static readonly IReadOnlyList<TaskPresetSlot> Slots = new TaskPresetSlot[6]
		{
			TaskPresetSlot.PsnaMaguumaWastes,
			TaskPresetSlot.PsnaMaguumaJungle,
			TaskPresetSlot.PsnaRuinsOfOrr,
			TaskPresetSlot.PsnaKryta,
			TaskPresetSlot.PsnaShiverpeaks,
			TaskPresetSlot.PsnaAscalon
		};

		private static readonly Dictionary<TaskPresetSlot, string> Regions = new Dictionary<TaskPresetSlot, string>
		{
			{
				TaskPresetSlot.PsnaMaguumaWastes,
				"Maguuma Wastes"
			},
			{
				TaskPresetSlot.PsnaMaguumaJungle,
				"Maguuma Jungle"
			},
			{
				TaskPresetSlot.PsnaRuinsOfOrr,
				"Ruins of Orr"
			},
			{
				TaskPresetSlot.PsnaKryta,
				"Kryta"
			},
			{
				TaskPresetSlot.PsnaShiverpeaks,
				"Shiverpeaks"
			},
			{
				TaskPresetSlot.PsnaAscalon,
				"Ascalon"
			}
		};

		private static readonly Dictionary<TaskPresetSlot, string> Npcs = new Dictionary<TaskPresetSlot, string>
		{
			{
				TaskPresetSlot.PsnaMaguumaWastes,
				"Mehem the Traveled"
			},
			{
				TaskPresetSlot.PsnaMaguumaJungle,
				"The Fox"
			},
			{
				TaskPresetSlot.PsnaRuinsOfOrr,
				"Specialist Yana"
			},
			{
				TaskPresetSlot.PsnaKryta,
				"Lady Derwena"
			},
			{
				TaskPresetSlot.PsnaShiverpeaks,
				"Despina Katelyn"
			},
			{
				TaskPresetSlot.PsnaAscalon,
				"Verma Giftrender"
			}
		};

		private static readonly Dictionary<TaskPresetSlot, string[]> LocationNames = new Dictionary<TaskPresetSlot, string[]>
		{
			{
				TaskPresetSlot.PsnaMaguumaWastes,
				new string[7] { "Blue Oasis", "Repair Station", "Camp Resolve Waypoint", "Azarr's Arbor", "Restoration Refuge", "Camp Resolve Waypoint", "Town of Prosperity" }
			},
			{
				TaskPresetSlot.PsnaMaguumaJungle,
				new string[7] { "Seraph Protectors", "Breth Ayahusasca", "Gallant's Folly", "Mabon Waypoint", "Lionguard Waystation Waypoint", "Desider Atum Waypoint", "Swampwatch Post" }
			},
			{
				TaskPresetSlot.PsnaRuinsOfOrr,
				new string[7] { "Armada Harbor", "Shelter Docks", "Augur's Torch", "Fort Trinity Waypoint", "Rally Waypoint", "Waste Hollows Waypoint", "Caer Shadowfain" }
			},
			{
				TaskPresetSlot.PsnaKryta,
				new string[7] { "Altar Brook Trading Post", "Pearl Islet Waypoint", "Vigil Keep Waypoint", "Mudflat Camp", "Marshwatch Haven Waypoint", "Garenhoff", "Shieldbluff Waypoint" }
			},
			{
				TaskPresetSlot.PsnaShiverpeaks,
				new string[7] { "Rocklair", "Dolyak Pass Waypoint", "Balddistead", "Blue Ice Shining Waypoint", "Ridgerock Camp Waypoint", "Travelen's Waypoint", "Mennerheim" }
			},
			{
				TaskPresetSlot.PsnaAscalon,
				new string[7] { "Village of Scalecatch Waypoint", "Hawkgates Waypoint", "Bovarin Estate", "Snow Ridge Camp Waypoint", "Haymal Gore", "Temperus Point Waypoint", "Ferrusatos Village" }
			}
		};

		private static readonly Dictionary<TaskPresetSlot, int[]> MapIds = new Dictionary<TaskPresetSlot, int[]>
		{
			{
				TaskPresetSlot.PsnaMaguumaWastes,
				new int[7] { 1963, 1940, 1919, 1929, 1927, 1919, 1918 }
			},
			{
				TaskPresetSlot.PsnaMaguumaJungle,
				new int[7] { 79, 707, 697, 314, 844, 72, 450 }
			},
			{
				TaskPresetSlot.PsnaRuinsOfOrr,
				new int[7] { 1021, 667, 785, 750, 1234, 680, 765 }
			},
			{
				TaskPresetSlot.PsnaKryta,
				new int[7] { 131, 1749, 402, 45, 422, 25, 166 }
			},
			{
				TaskPresetSlot.PsnaShiverpeaks,
				new int[7] { 1629, 379, 578, 645, 643, 612, 824 }
			},
			{
				TaskPresetSlot.PsnaAscalon,
				new int[7] { 487, 211, 272, 545, 527, 387, 497 }
			}
		};

		public static int GetCycleIndex(DateTime nowUtc)
		{
			long ticks = (nowUtc - AnchorUtc).Ticks;
			long days = ticks / 864000000000L;
			if (ticks < 0 && ticks % 864000000000L != 0L)
			{
				days--;
			}
			return (int)((days % 7 + 7) % 7);
		}

		public static PsnaLocation GetLocation(TaskPresetSlot slot, DateTime nowUtc)
		{
			if (!LocationNames.TryGetValue(slot, out var names) || !MapIds.TryGetValue(slot, out var ids))
			{
				return null;
			}
			int index = GetCycleIndex(nowUtc);
			return new PsnaLocation(slot, Regions[slot], Npcs[slot], names[index], ids[index]);
		}

		public static string EncodeMapLink(int mapId)
		{
			if (mapId <= 0)
			{
				return null;
			}
			byte[] bytes = new byte[5]
			{
				4,
				(byte)((uint)mapId & 0xFFu),
				(byte)((uint)(mapId >> 8) & 0xFFu),
				(byte)((uint)(mapId >> 16) & 0xFFu),
				(byte)((uint)(mapId >> 24) & 0xFFu)
			};
			return "[&" + Convert.ToBase64String(bytes) + "]";
		}
	}
}
