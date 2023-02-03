using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Ideka.RacingMeter.Lib;
using Newtonsoft.Json;

namespace Ideka.RacingMeter
{
	public class RemoteRaces
	{
		private static readonly Logger Logger = Logger.GetLogger<RemoteRaces>();

		public IDictionary<string, FullRace> Races { get; set; } = new Dictionary<string, FullRace>();


		public static RemoteRaces FromCache(string path)
		{
			RemoteRaces races = null;
			if (File.Exists(path))
			{
				try
				{
					races = JsonConvert.DeserializeObject<RemoteRaces>(File.ReadAllText(path));
					if (races == null)
					{
						Logger.Warn("Failed to load Races cache.");
					}
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Error when loading Races cache.");
				}
			}
			return races ?? new RemoteRaces();
		}

		public void ToCache(string path)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllText(path, JsonConvert.SerializeObject(this));
		}
	}
}
