using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class FileModelService
	{
		public static Model CreateModel(FileModel fileModel)
		{
			Model model = new Model
			{
				FavoriteItemApiIds = new SafeList<int>(fileModel.FavoriteItemApiIds)
			};
			foreach (FileStat fileCurrency in fileModel.FileCurrencies)
			{
				model.CurrencyById[fileCurrency.ApiId] = new Stat
				{
					ApiId = fileCurrency.ApiId,
					Count = fileCurrency.Count
				};
			}
			foreach (FileStat fileItem in fileModel.FileItems)
			{
				model.ItemById[fileItem.ApiId] = new Stat
				{
					ApiId = fileItem.ApiId,
					Count = fileItem.Count
				};
			}
			model.IgnoredItemApiIds = new SafeList<int>(fileModel.IgnoredItemApiIds);
			foreach (int ignoredItemApiId in fileModel.IgnoredItemApiIds)
			{
				if (!model.ItemById.ContainsKey(ignoredItemApiId))
				{
					model.ItemById[ignoredItemApiId] = new Stat
					{
						ApiId = ignoredItemApiId,
						Count = 0L
					};
				}
			}
			model.UpdateStatsSnapshot();
			return model;
		}

		public static FileModel CreateFileModel(Model model)
		{
			FileModel fileModel = new FileModel
			{
				IgnoredItemApiIds = model.IgnoredItemApiIds.ToListSafe(),
				FavoriteItemApiIds = model.FavoriteItemApiIds.ToListSafe()
			};
			StatsSnapshot statsSnapshot = model.StatsSnapshot;
			List<Stat> items = statsSnapshot.ItemById.Values.Where((Stat s) => s.Count != 0).ToList();
			List<Stat> currencies = statsSnapshot.CurrencyById.Values.Where((Stat s) => s.Count != 0).ToList();
			foreach (Stat item in items)
			{
				FileStat fileStat2 = new FileStat
				{
					ApiId = item.ApiId,
					Count = item.Count
				};
				fileModel.FileItems.Add(fileStat2);
			}
			foreach (Stat currency in currencies)
			{
				FileStat fileStat = new FileStat
				{
					ApiId = currency.ApiId,
					Count = currency.Count
				};
				fileModel.FileCurrencies.Add(fileStat);
			}
			return fileModel;
		}
	}
}
