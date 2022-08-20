using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Ideka.NetCommon;
using Ideka.RacingMeterLib;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace Ideka.RacingMeter
{
	public static class DataInterface
	{
		private class Inner
		{
		}

		private static readonly Logger Logger = Logger.GetLogger<Inner>();

		public static FullRace GetLocalRace(string raceId)
		{
			string path = Path.Combine(RacingModule.RacePath, raceId + ".race");
			if (!File.Exists(path))
			{
				return null;
			}
			return FullRace.FromLocal(path, JsonConvert.DeserializeObject<Race>(File.ReadAllText(path)));
		}

		public static Dictionary<string, FullRace> GetLocalRaces()
		{
			Dictionary<string, FullRace> dict = new Dictionary<string, FullRace>();
			if (!Directory.Exists(RacingModule.RacePath))
			{
				return dict;
			}
			foreach (string fname in from p in Directory.GetFiles(RacingModule.RacePath)
				where p.EndsWith(".race", StringComparison.InvariantCultureIgnoreCase)
				select p)
			{
				try
				{
					dict[fname] = FullRace.FromLocal(fname, JsonConvert.DeserializeObject<Race>(File.ReadAllText(fname)));
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to load race file " + fname + ", skipping.");
				}
			}
			return dict;
		}

		public static void SaveRace(FullRace race)
		{
			race.Race.Modified = DateTime.UtcNow;
			Directory.CreateDirectory(RacingModule.RacePath);
			File.WriteAllText(Path.Combine(RacingModule.RacePath, race.Meta.Id + ".race"), JsonConvert.SerializeObject((object)race.Race));
		}

		public static void DeleteRace(FullRace race)
		{
			string racePath = Path.Combine(RacingModule.RacePath, race.Meta.Id + ".race");
			if (File.Exists(racePath))
			{
				FileSystem.DeleteFile(racePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			}
		}

		public static Dictionary<string, FullGhost> GetLocalGhosts(FullRace race)
		{
			Dictionary<string, FullGhost> dict = new Dictionary<string, FullGhost>();
			if (race == null || race.Meta?.Id == null)
			{
				return dict;
			}
			string path = RacingModule.GhostRacePath(race.Meta.Id);
			if (!Directory.Exists(path))
			{
				return dict;
			}
			foreach (string fname in from p in Directory.GetFiles(path)
				where p.EndsWith(".ghost", StringComparison.InvariantCultureIgnoreCase)
				select p)
			{
				try
				{
					Ghost ghost = JsonConvert.DeserializeObject<Ghost>(File.ReadAllText(fname));
					if (ghost.Validate(race.Race))
					{
						dict[fname] = FullGhost.FromLocal(fname, ghost);
					}
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to load ghost file " + fname + ", skipping.");
				}
			}
			return dict;
		}

		public static bool SaveGhost(ref Dictionary<string, FullGhost> existing, Ghost ghost, int max = -1)
		{
			if (max == 0)
			{
				return false;
			}
			existing = existing.Where((KeyValuePair<string, FullGhost> kv) => File.Exists(kv.Key)).ToDictionary((KeyValuePair<string, FullGhost> kv) => kv.Key, (KeyValuePair<string, FullGhost> kv) => kv.Value);
			string path = RacingModule.GhostRacePath(ghost.RaceId);
			string target = Path.Combine(path, $"{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss-fff}.ghost");
			existing[target] = FullGhost.FromLocal(target, ghost);
			while (max > 0 && existing.Count > max)
			{
				KeyValuePair<string, FullGhost> worst = existing.MaxBy((KeyValuePair<string, FullGhost> p) => p.Value.Meta.Time);
				File.Delete(worst.Key);
				existing.Remove(worst.Key);
			}
			if (existing.ContainsKey(target))
			{
				Directory.CreateDirectory(path);
				File.WriteAllText(target, JsonConvert.SerializeObject((object)ghost));
				return true;
			}
			return false;
		}
	}
}
