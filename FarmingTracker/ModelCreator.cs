using System.Collections.Generic;

namespace FarmingTracker
{
	public class ModelCreator
	{
		public static Model CreateModel(FileModel fileModel)
		{
			Model model = new Model
			{
				IgnoredItemApiIds = new SafeList<int>(fileModel.IgnoredItemApiIds),
				FavoriteItemApiIds = new SafeList<int>(fileModel.FavoriteItemApiIds),
				CustomStatProfits = new SafeList<CustomStatProfit>(fileModel.CustomStatProfits)
			};
			AddStatsToModel(model.Stats.CurrencyById, fileModel.FileCurrencies, StatType.Currency);
			AddStatsToModel(model.Stats.ItemById, fileModel.FileItems, StatType.Item);
			foreach (CustomStatProfit customStatProfit in fileModel.CustomStatProfits)
			{
				AddStatToModelIfMissing((customStatProfit.StatType == StatType.Item) ? model.Stats.ItemById : model.Stats.CurrencyById, customStatProfit.ApiId, customStatProfit.StatType);
			}
			foreach (int ignoredItemApiId in fileModel.IgnoredItemApiIds)
			{
				AddStatToModelIfMissing(model.Stats.ItemById, ignoredItemApiId, StatType.Item);
			}
			model.Stats.UpdateStatsSnapshot();
			return model;
		}

		private static void AddStatsToModel(Dictionary<int, Stat> statById, List<FileStat> fileStats, StatType statType)
		{
			foreach (FileStat fileStat in fileStats)
			{
				statById[fileStat.ApiId] = new Stat
				{
					ApiId = fileStat.ApiId,
					StatType = statType,
					Signed_Count = fileStat.Count
				};
			}
		}

		private static void AddStatToModelIfMissing(Dictionary<int, Stat> statById, int statId, StatType statType)
		{
			if (!statById.ContainsKey(statId))
			{
				statById[statId] = new Stat
				{
					ApiId = statId,
					StatType = statType,
					Signed_Count = 0L
				};
			}
		}
	}
}
