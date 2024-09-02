using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class SearchService
	{
		public static (List<Stat> items, List<Stat> currencies) FilterBySearchTerm(List<Stat> items, List<Stat> currencies, string searchTerm)
		{
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				currencies = FilterBySearchTerm(currencies, searchTerm);
				items = FilterBySearchTerm(items, searchTerm);
			}
			return (items, currencies);
		}

		private static List<Stat> FilterBySearchTerm(List<Stat> stats, string searchTerm)
		{
			searchTerm = searchTerm.ToLower().Trim();
			return stats.Where((Stat s) => s.Details.Name.ToLower().Contains(searchTerm)).ToList();
		}
	}
}
