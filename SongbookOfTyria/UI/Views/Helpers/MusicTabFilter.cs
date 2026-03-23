using System;
using System.Collections.Generic;
using System.Linq;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;

namespace SongbookOfTyria.UI.Views.Helpers
{
	public class MusicTabFilter
	{
		public string SearchText { get; set; } = string.Empty;


		public bool SoloOnly { get; set; }

		public bool DuetOnly { get; set; }

		public bool BandOnly { get; set; }

		public bool BeginnerOnly { get; set; }

		public bool PracticeModeOnly { get; set; }

		public bool PianoOnly { get; set; }

		public bool FavoritesOnly { get; set; }

		public UserSettingsService UserSettingsService { get; set; }

		public HashSet<string> SelectedGenres { get; set; } = new HashSet<string>();


		public HashSet<string> SelectedTabbers { get; set; } = new HashSet<string>();


		public bool PublicOnly { get; set; }

		public bool PrivateOnly { get; set; }

		public List<MusicTab> Apply(List<MusicTab> tabs)
		{
			if (tabs == null)
			{
				return new List<MusicTab>();
			}
			return tabs.Where(MatchesAllCriteria).ToList();
		}

		private bool MatchesAllCriteria(MusicTab tab)
		{
			if (MatchesSearch(tab) && MatchesTabTypeFilter(tab) && MatchesDifficultyFilter(tab) && MatchesFeaturesFilter(tab) && MatchesFavoritesFilter(tab) && MatchesGenreFilter(tab) && MatchesTabberFilter(tab))
			{
				return MatchesVisibilityFilter(tab);
			}
			return false;
		}

		private bool MatchesSearch(MusicTab tab)
		{
			if (string.IsNullOrWhiteSpace(SearchText))
			{
				return true;
			}
			if (!ContainsIgnoreCase(tab.Name, SearchText) && !ContainsIgnoreCase(tab.Genre, SearchText))
			{
				return ContainsIgnoreCase(tab.TabbedBy, SearchText);
			}
			return true;
		}

		private bool ContainsIgnoreCase(string source, string searchText)
		{
			if (source == null)
			{
				return false;
			}
			return source.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private bool MatchesTabTypeFilter(MusicTab tab)
		{
			if (!SoloOnly && !DuetOnly && !BandOnly)
			{
				return true;
			}
			if (tab.TabType == null || tab.TabType.Count == 0)
			{
				return false;
			}
			if ((!SoloOnly || !tab.TabType.Contains("Solo")) && (!DuetOnly || !tab.TabType.Contains("Duet")))
			{
				if (BandOnly)
				{
					return tab.TabType.Contains("Band");
				}
				return false;
			}
			return true;
		}

		private bool MatchesDifficultyFilter(MusicTab tab)
		{
			if (!BeginnerOnly)
			{
				return true;
			}
			return tab.IsBeginner;
		}

		private bool MatchesFeaturesFilter(MusicTab tab)
		{
			if (PracticeModeOnly && !tab.PracticeMode)
			{
				return false;
			}
			if (PianoOnly && !tab.Piano)
			{
				return false;
			}
			return true;
		}

		private bool MatchesFavoritesFilter(MusicTab tab)
		{
			if (!FavoritesOnly || UserSettingsService == null)
			{
				return true;
			}
			return UserSettingsService.IsFavorite(tab.Id);
		}

		private bool MatchesGenreFilter(MusicTab tab)
		{
			if (SelectedGenres == null || SelectedGenres.Count == 0)
			{
				return true;
			}
			if (!string.IsNullOrEmpty(tab.Genre))
			{
				return SelectedGenres.Contains(tab.Genre);
			}
			return false;
		}

		private bool MatchesTabberFilter(MusicTab tab)
		{
			if (SelectedTabbers == null || SelectedTabbers.Count == 0)
			{
				return true;
			}
			if (tab.TabbedByMember != null && tab.TabbedByMember.Count > 0)
			{
				return tab.TabbedByMember.Any((string m) => SelectedTabbers.Contains(m));
			}
			if (!string.IsNullOrEmpty(tab.TabbedBy))
			{
				string[] array = tab.TabbedBy.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					string trimmedTabber = array[i].Trim();
					if (SelectedTabbers.Contains(trimmedTabber))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool MatchesVisibilityFilter(MusicTab tab)
		{
			if (!PublicOnly && !PrivateOnly)
			{
				return true;
			}
			if (!PublicOnly || tab.IsPrivate)
			{
				if (PrivateOnly)
				{
					return tab.IsPrivate;
				}
				return false;
			}
			return true;
		}
	}
}
