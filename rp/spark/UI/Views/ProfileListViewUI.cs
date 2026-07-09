using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	internal static class ProfileListViewUI
	{
		public const int BodyWidth = 760;

		public const int HeaderY = 92;

		public const int ListY = 128;

		public const int ListHeight = 400;

		public const int StatusY = 565;

		public const int PageY = 532;

		public const int BookmarkMarkerY = 11;

		public const int BookmarkIconAssetId = 102439;

		public const string SearchAllFields = "All fields";

		public const string SearchName = "Name";

		public const string SearchAccount = "Account";

		public const string SearchRace = "Race";

		public const string SearchProfession = "Profession";

		public const string SortName = "Name";

		public const string SortRecentlySeen = "Recently Seen";

		public const string SortRace = "Race";

		public const string SortAccount = "Account";

		private static readonly Color SecondaryTextColor = new Color(220, 220, 220);

		private static readonly Color HeaderTextColor = new Color(255, 233, 180);

		private const int SearchDebounceMilliseconds = 450;

		public static void AddTitle(Container parent, string text, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(width, 34));
			((Control)val).set_Parent(parent);
		}

		public static StandardButton AddRefreshButton(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			val.set_Text("Refresh");
			((Control)val).set_Location(new Point(650, 0));
			((Control)val).set_Size(new Point(100, 30));
			((Control)val).set_Parent(parent);
			return val;
		}

		public static ProfileListSearchControls AddSearchControls(Container parent, string placeholderText, IEnumerable<string> searchOptions, IEnumerable<string> sortOptions, string selectedSortOption, Action changed)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			//IL_0174: Expected O, but got Unknown
			TextBox val = new TextBox();
			((TextInputBase)val).set_PlaceholderText(placeholderText);
			((Control)val).set_Location(new Point(0, 40));
			((Control)val).set_Size(new Point(250, 35));
			((Control)val).set_Parent(parent);
			int searchChangeVersion = 0;
			((TextInputBase)val).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				int scheduledVersion = ++searchChangeVersion;
				QueueDebouncedAsync(delegate
				{
					if (scheduledVersion == searchChangeVersion)
					{
						changed?.Invoke();
					}
				});
			});
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Location(new Point(260, 40));
			((Control)val2).set_Size(new Point(140, 35));
			((Control)val2).set_Parent(parent);
			Dropdown searchFieldDropdown = val2;
			AddOptions(searchFieldDropdown, searchOptions);
			searchFieldDropdown.set_SelectedItem("All fields");
			searchFieldDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				searchChangeVersion++;
				changed?.Invoke();
			});
			Label val3 = new Label();
			val3.set_Text("Sort by:");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(SecondaryTextColor);
			((Control)val3).set_Location(new Point(410, 48));
			((Control)val3).set_Size(new Point(60, 24));
			((Control)val3).set_Parent(parent);
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Location(new Point(470, 40));
			((Control)val4).set_Size(new Point(170, 35));
			((Control)val4).set_Parent(parent);
			Dropdown sortDropdown = val4;
			List<string> sortOptionList = (sortOptions ?? Enumerable.Empty<string>()).ToList();
			AddOptions(sortDropdown, sortOptionList);
			sortDropdown.set_SelectedItem(selectedSortOption ?? sortOptionList.FirstOrDefault());
			sortDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				searchChangeVersion++;
				changed?.Invoke();
			});
			return new ProfileListSearchControls(val, searchFieldDropdown, sortDropdown);
		}

		public static Label AddStatusLabel(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(string.Empty);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(SecondaryTextColor);
			((Control)val).set_Location(new Point(0, 565));
			((Control)val).set_Size(new Point(760, 24));
			((Control)val).set_Parent(parent);
			return val;
		}

		public static void AddHeader(Container parent, string text, int x, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(HeaderTextColor);
			val.set_StrokeText(true);
			((Control)val).set_Location(new Point(x, 92));
			((Control)val).set_Size(new Point(width, 26));
			((Control)val).set_Parent(parent);
		}

		public static AssetIcon AddBookmarkMarker(Container row)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			AssetIcon assetIcon = new AssetIcon();
			((Control)assetIcon).set_Location(new Point(7, 11));
			((Control)assetIcon).set_Size(new Point(18, 18));
			((Control)assetIcon).set_Parent(row);
			assetIcon.SetAssetId(102439);
			return assetIcon;
		}

		public static string GetSearchSuffix(TextBox searchBox)
		{
			if (!string.IsNullOrWhiteSpace((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null))
			{
				return " match";
			}
			return string.Empty;
		}

		private static void AddOptions(Dropdown dropdown, IEnumerable<string> options)
		{
			foreach (string option in options ?? Enumerable.Empty<string>())
			{
				dropdown.get_Items().Add(option);
			}
		}

		private static async Task QueueDebouncedAsync(Action action)
		{
			await Task.Delay(450);
			SparkUiThread.Queue(action);
		}
	}
}
