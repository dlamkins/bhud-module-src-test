using System;
using System.Collections.Generic;
using System.Linq;
using SongbookOfTyria.Models;

namespace SongbookOfTyria.UI.Views.Helpers
{
	public class MusicTabSorter
	{
		public SortMode Mode { get; set; }

		public bool Ascending { get; set; } = true;


		public List<MusicTab> Apply(List<MusicTab> tabs)
		{
			if (tabs == null || tabs.Count == 0 || Mode == SortMode.None)
			{
				return tabs ?? new List<MusicTab>();
			}
			return Mode switch
			{
				SortMode.Name => SortBy(tabs, (MusicTab t) => t.Name), 
				SortMode.ReleaseDate => SortBy(tabs, (MusicTab t) => t.ReleaseDate), 
				_ => tabs, 
			};
		}

		private List<MusicTab> SortBy(List<MusicTab> tabs, Func<MusicTab, string> keySelector)
		{
			if (!Ascending)
			{
				return tabs.OrderByDescending((MusicTab t) => keySelector(t) ?? string.Empty).ToList();
			}
			return tabs.OrderBy((MusicTab t) => keySelector(t) ?? string.Empty).ToList();
		}
	}
}
