using System.Collections.Generic;
using SongbookOfTyria.UI.Views.Helpers;

namespace SongbookOfTyria.Services
{
	public class FilterState
	{
		public string SearchText { get; set; } = string.Empty;


		public bool SoloOnly { get; set; }

		public bool DuetOnly { get; set; }

		public bool BandOnly { get; set; }

		public bool BeginnerOnly { get; set; }

		public bool PracticeModeOnly { get; set; }

		public bool PianoOnly { get; set; }

		public bool FavoritesOnly { get; set; }

		public List<string> SelectedGenres { get; set; } = new List<string>();


		public List<string> SelectedTabbers { get; set; } = new List<string>();


		public bool PublicOnly { get; set; }

		public bool PrivateOnly { get; set; }

		public SortMode SortMode { get; set; } = SortMode.ReleaseDate;


		public bool SortAscending { get; set; }

		public Dictionary<string, bool> CollapsedPanels { get; set; } = new Dictionary<string, bool>();

	}
}
