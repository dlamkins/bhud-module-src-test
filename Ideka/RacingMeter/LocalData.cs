using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace Ideka.RacingMeter
{
	internal class LocalData : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<LocalData>();

		private readonly Dictionary<string, FullRace> _races = new Dictionary<string, FullRace>();

		public IReadOnlyDictionary<string, FullRace> Races => _races;

		public event Action<IReadOnlyDictionary<string, FullRace>>? RacesChanged;

		public event Action<FullRace>? RaceCreated;

		public event Action<FullRace>? GhostsChanged;

		public static FullRace? GetRaceFromDisk(string raceId)
		{
			string path = Path.Combine(RacingModule.RacePath, raceId + ".race");
			if (File.Exists(path))
			{
				Race race = JsonConvert.DeserializeObject<Race>(File.ReadAllText(path));
				if (race != null)
				{
					return FullRace.FromLocal(race, path);
				}
			}
			return null;
		}

		public void ReloadRaces()
		{
			_races.Clear();
			this.RacesChanged?.Invoke(Races);
			if (!Directory.Exists(RacingModule.RacePath))
			{
				return;
			}
			foreach (string fname in from p in Directory.GetFiles(RacingModule.RacePath)
				where p.EndsWith(".race", StringComparison.InvariantCultureIgnoreCase)
				select p)
			{
				try
				{
					Race race = JsonConvert.DeserializeObject<Race>(File.ReadAllText(fname));
					if (race == null)
					{
						Logger.Warn("Failed to load race file " + fname + ", skipping.");
						continue;
					}
					FullRace fullRace = FullRace.FromLocal(race, fname);
					_races[fullRace.Meta.Id] = fullRace;
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Error while loading race file " + fname + ", skipping.");
				}
			}
			this.RacesChanged?.Invoke(Races);
		}

		public void SaveRace(FullRace fullRace)
		{
			DateTime dateTime2 = (fullRace.Race.Modified = (fullRace.Meta.Modified = DateTime.UtcNow));
			Directory.CreateDirectory(RacingModule.RacePath);
			File.WriteAllText(Path.Combine(RacingModule.RacePath, fullRace.Meta.Id + ".race"), JsonConvert.SerializeObject(fullRace.Race));
			bool num = _races.ContainsKey(fullRace.Meta.Id);
			_races[fullRace.Meta.Id] = fullRace;
			this.RacesChanged?.Invoke(Races);
			if (!num)
			{
				this.RaceCreated?.Invoke(fullRace);
			}
		}

		public void DeleteRace(FullRace fullRace)
		{
			string racePath = Path.Combine(RacingModule.RacePath, fullRace.Meta.Id + ".race");
			if (File.Exists(racePath))
			{
				FileSystem.DeleteFile(racePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			}
			if (_races.Remove(fullRace.Meta.Id))
			{
				this.RacesChanged?.Invoke(Races);
			}
		}

		public static Dictionary<string, FullGhost> GetGhosts(FullRace fullRace)
		{
			Dictionary<string, FullGhost> dict = new Dictionary<string, FullGhost>();
			string path = RacingModule.GhostRacePath(fullRace.Meta.Id);
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
					if (ghost == null || !ghost.Validate(fullRace.Race))
					{
						Logger.Warn("Failed to load ghost file " + fname + ", skipping.");
					}
					else
					{
						dict[fname] = FullGhost.FromLocal(ghost, fname);
					}
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Error while loading ghost file " + fname + ", skipping.");
				}
			}
			return dict;
		}

		public bool SaveGhost(FullRace fullRace, ref Dictionary<string, FullGhost> existing, Ghost ghost, int max = -1)
		{
			bool changed = false;
			if (max == 0)
			{
				return false;
			}
			existing = existing.Where<KeyValuePair<string, FullGhost>>((KeyValuePair<string, FullGhost> kv) => File.Exists(kv.Key)).ToDictionary((KeyValuePair<string, FullGhost> kv) => kv.Key, (KeyValuePair<string, FullGhost> kv) => kv.Value);
			string path = RacingModule.GhostRacePath(ghost.RaceId);
			string target = Path.Combine(path, $"{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss-fff}.ghost");
			existing[target] = FullGhost.FromLocal(ghost, target);
			while (max > 0 && existing.Count > max)
			{
				KeyValuePair<string, FullGhost> worst = existing.MaxBy<KeyValuePair<string, FullGhost>, TimeSpan>((KeyValuePair<string, FullGhost> p) => p.Value.Meta.Time);
				File.Delete(worst.Key);
				existing.Remove(worst.Key);
				changed = true;
			}
			if (existing.ContainsKey(target))
			{
				Directory.CreateDirectory(path);
				File.WriteAllText(target, JsonConvert.SerializeObject(ghost));
				this.GhostsChanged?.Invoke(fullRace);
				return true;
			}
			if (changed)
			{
				this.GhostsChanged?.Invoke(fullRace);
			}
			return false;
		}

		public void Dispose()
		{
			_races.Clear();
		}
	}
}
