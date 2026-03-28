using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class FileModelCreator
	{
		public static FileModel CreateFileModel(Model model)
		{
			List<Stat> list = (from s in model.Stats.GetStats()
				where s.Signed_Count.Value != 0L || s.StatVisibility != 0 || s.Profit.HasCustomProfit
				select s).ToList();
			FileModel fileModel = new FileModel();
			foreach (Stat stat in list)
			{
				FileStat fileStat = new FileStat
				{
					ApiId = stat.ApiId,
					StatType = stat.StatType,
					Signed_Count = stat.Signed_Count.Value,
					StatVisibility = stat.StatVisibility,
					Unsigned_CustomProfitInCopper = stat.Profit.Unsigned_Custom_ProfitInCopper
				};
				fileModel.FileStats.Add(fileStat);
			}
			return fileModel;
		}
	}
}
