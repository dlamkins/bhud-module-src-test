using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace RaidClears.Raids.Services
{
	public static class GetDailyFractalService
	{
		public static async Task<(List<Achievement> t4s, List<Achievement> recs)> GetDailyFractals(Gw2ApiManager gw2ApiManager, Logger logger)
		{
			List<Achievement> recs = new List<Achievement>();
			List<Achievement> t4s = new List<Achievement>();
			try
			{
				int[] fractal_achievement_list = (from x in (await ((IBlobClient<AchievementsDaily>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Achievements()
						.get_Daily()).GetAsync(default(CancellationToken))).get_Fractals()
					select x.get_Id()).ToArray();
				foreach (Achievement fractal in await ((IBulkExpandableClient<Achievement, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Achievements()).ManyAsync((IEnumerable<int>)fractal_achievement_list, default(CancellationToken)))
				{
					if (Regex.Match(fractal.get_Name(), "Daily Recommended").Success)
					{
						recs.Add(fractal);
					}
					else if (Regex.Match(fractal.get_Name(), "Daily Tier 4").Success)
					{
						t4s.Add(fractal);
					}
				}
				recs.Sort((Achievement x, Achievement y) => x.get_Name().CompareTo(y.get_Name()));
				t4s.Sort((Achievement x, Achievement y) => x.get_Name().CompareTo(y.get_Name()));
			}
			catch (Exception e)
			{
				logger.Warn(e, "Could not get fractals from API");
			}
			return (t4s, recs);
		}

		public static (string shortName, string longName) TierFourDisplayName(string achievementName)
		{
			Match name = Regex.Match(achievementName, "Daily Tier 4 (.+)");
			if (name.Captures.Count > 0)
			{
				string shortName = "";
				return (name.Captures[0].Value switch
				{
					"Aetherblade" => "aeth", 
					"Aquatic Ruins" => "aqua", 
					"Captain Mai Trin Boss" => "mai", 
					"Chaos" => "chaos", 
					"Cliffside" => "cliff", 
					"Deepstone" => "deep", 
					"Molten Boss" => "m boss", 
					"Molten Furnace" => "m furn", 
					"Nightmare" => "night", 
					"Shattered Observatory" => "s-obs", 
					"Siren's Reef" => "siren", 
					"Snowblind" => "snow", 
					"Sunqua Peak" => "sun", 
					"Solid Ocean" => "solid", 
					"Swampland" => "swamp", 
					"Thaumanova Reactor" => "thau", 
					"Twilight Oasis" => "twili", 
					"Uncategorized" => "uncat", 
					"Underground Facility" => "under", 
					"Urban Battleground" => "urban", 
					"Volcanic" => "volc", 
					_ => "???", 
				}, name.Captures[0].Value);
			}
			return ("???", "Unknown error");
		}
	}
}
