using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class FileModelCreator
	{
		public static FileModel CreateFileModel(Model model)
		{
			FileModel obj = new FileModel
			{
				IgnoredItemApiIds = model.IgnoredItemApiIds.ToListSafe(),
				FavoriteItemApiIds = model.FavoriteItemApiIds.ToListSafe(),
				CustomStatProfits = model.CustomStatProfits.ToListSafe()
			};
			StatsSnapshot statsSnapshot = model.Stats.StatsSnapshot;
			List<Stat> items = statsSnapshot.ItemById.Values.Where((Stat s) => s.Signed_Count != 0).ToList();
			List<Stat> stats = statsSnapshot.CurrencyById.Values.Where((Stat s) => s.Signed_Count != 0).ToList();
			IEnumerable<FileStat> fileItems = CreateFileStats(items);
			IEnumerable<FileStat> fileCurrencies = CreateFileStats(stats);
			obj.FileItems.AddRange(fileItems);
			obj.FileCurrencies.AddRange(fileCurrencies);
			return obj;
		}

		private static IEnumerable<FileStat> CreateFileStats(List<Stat> stats)
		{
			foreach (Stat stat in stats)
			{
				yield return new FileStat
				{
					ApiId = stat.ApiId,
					Count = stat.Signed_Count
				};
			}
		}
	}
}
