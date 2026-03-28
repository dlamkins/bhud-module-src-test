namespace FarmingTracker
{
	public class ModelCreator
	{
		public static Model CreateModel(FileModel fileModel)
		{
			Model model = new Model();
			foreach (FileStat fileStat in fileModel.FileStats)
			{
				AddStatToModel(fileStat, model.Stats);
			}
			return model;
		}

		private static void AddStatToModel(FileStat fileStat, Stats stats)
		{
			Stat stat2 = new Stat();
			stat2.ApiId = fileStat.ApiId;
			stat2.StatType = fileStat.StatType;
			stat2.Signed_Count.Value = fileStat.Signed_Count;
			stat2.StatVisibility = fileStat.StatVisibility;
			stat2.Profit.Unsigned_Custom_ProfitInCopper = fileStat.Unsigned_CustomProfitInCopper;
			Stat stat = stat2;
			stats.AddStat(stat);
		}
	}
}
