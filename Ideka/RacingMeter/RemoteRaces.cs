using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Ideka.RacingMeterLib;
using Newtonsoft.Json;

namespace Ideka.RacingMeter
{
	public class RemoteRaces
	{
		private static readonly Logger Logger = Logger.GetLogger<RemoteRaces>();

		public IDictionary<string, FullRace> Races { get; set; } = new Dictionary<string, FullRace>();


		public static RemoteRaces FromCache(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					return JsonConvert.DeserializeObject<RemoteRaces>(File.ReadAllText(path));
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to load Races cache.");
			}
			return new RemoteRaces();
		}

		public void ToCache(string path)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllText(path, JsonConvert.SerializeObject((object)this));
		}
	}
}
