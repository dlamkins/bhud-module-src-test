using System.Linq;

namespace FarmingTracker
{
	public class FavoriteStatService
	{
		public static void RemoveFromFavoriteStats(Stat stat, Model model, Services services)
		{
			Stat stat2 = stat;
			int apiId = ReplaceApiIdIfCustomCoin(stat2);
			Stat matchingStat = model.Stats.GetStats().FirstOrDefault((Stat s) => s.StatType == stat2.StatType && s.ApiId == apiId);
			if (matchingStat == null || matchingStat.StatVisibility != StatVisibility.Favorite)
			{
				Module.Logger.Error("Cannot remove stat from favorites. It does not exist or is no favorite.");
				return;
			}
			matchingStat.StatVisibility = StatVisibility.Regular;
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		public static void AddToFavoriteStats(Stat stat, Model model, Services services)
		{
			Stat stat2 = stat;
			int apiId = ReplaceApiIdIfCustomCoin(stat2);
			Stat matchingStat = model.Stats.GetStats().FirstOrDefault((Stat s) => s.StatType == stat2.StatType && s.ApiId == apiId);
			if (matchingStat == null || matchingStat.StatVisibility == StatVisibility.Favorite)
			{
				Module.Logger.Error("Cannot add stat to favorites. Stat does not exist or is already a favorite.");
				return;
			}
			matchingStat.StatVisibility = StatVisibility.Favorite;
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static int ReplaceApiIdIfCustomCoin(Stat stat)
		{
			if (!stat.Details.IsCustomCoinStat)
			{
				return stat.ApiId;
			}
			return 1;
		}
	}
}
