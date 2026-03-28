namespace FarmingTracker
{
	public class SearchService
	{
		public static bool IncludesSearchTerm(Stat stat, string searchTerm)
		{
			searchTerm = searchTerm.ToLower().Trim();
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				return stat.Details.Name.ToLower().Contains(searchTerm);
			}
			return true;
		}
	}
}
