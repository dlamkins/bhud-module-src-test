using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class GeoGuessListingView : AccountRestrictedView
	{
		[CompilerGenerated]
		private GeoGuessWindowStateService _003CWindowState_003EP;

		private List<GeoGuessListItem> _filteredItems;

		private Guild _guildModel;

		private StandardButton _createNew;

		private StandardButton _searchButton;

		private StandardButton _prevButton;

		private StandardButton _nextButton;

		protected FlowPanel _secondControlPanel;

		protected FlowPanel _paginationPanel;

		public GeoGuessListingView(GeoGuessWindowStateService WindowState)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			_003CWindowState_003EP = WindowState;
			_filteredItems = new List<GeoGuessListItem>();
			_guildModel = new Guild();
			_createNew = new StandardButton();
			_searchButton = new StandardButton();
			_prevButton = new StandardButton();
			_nextButton = new StandardButton();
			FlowPanel val = new FlowPanel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(5f, 0f));
			_secondControlPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)0);
			val2.set_ControlPadding(new Vector2(5f, 0f));
			_paginationPanel = val2;
			base._002Ector(showBackButton: true);
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await base.Load(progress);
			progress.Report("Loading GeoGuess models for guild " + _003CWindowState_003EP.SelectedGuild.Name);
			Guild data = await Service.GeoServerWrapper.GetGuildInfoAsync(_003CWindowState_003EP.SelectedGuild.Id.ToString());
			if (data != null)
			{
				_guildModel = data;
			}
			else
			{
				_guildModel = new Guild();
			}
			return true;
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			StandardButton val = new StandardButton();
			val.set_Text("New Puzzle");
			((Control)val).set_Parent((Container)(object)_controlsHeader);
			((Control)val).set_Width(120);
			((Control)val).set_Height(40);
			((Control)val).set_Location(new Point(((Control)_backButton).get_Right() + 10, 5));
			_createNew = val;
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				((Control)_createNew).set_Enabled(false);
				((Control)_createNew).set_BasicTooltipText("Cannot create puzzles in competitive mode maps (PvP, WvW, etc.)");
			}
			else
			{
				((Control)_createNew).set_BasicTooltipText("Create a new GeoGuess puzzle for your guild");
			}
			((Control)_createNew).add_Click((EventHandler<MouseEventArgs>)CreateNewGeoGuessClickHandler);
			ReloadButton reloadButton = new ReloadButton();
			((Control)reloadButton).set_Parent((Container)(object)_controlsHeader);
			((Control)reloadButton).set_Location(new Point(((Control)_createNew).get_Right() + 5, 0));
			((Control)reloadButton).set_BasicTooltipText("Refresh the puzzle list");
			ReloadButton reload = reloadButton;
			((Control)reload).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.GeoGuessWindow.OpenWindowState();
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_controlsHeader);
			((Control)val2).set_Height(40);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)reload).get_Right() + 10, 5));
			val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)2));
			val2.set_TextColor(Color.get_PaleGoldenrod());
			val2.set_Text(_003CWindowState_003EP.SelectedGuild.Name + " [" + _003CWindowState_003EP.SelectedGuild.Tag + "]");
			_flowPanel.AddChildPanel((Panel)(object)_secondControlPanel);
			_flowPanel.AddChildPanel((Panel)(object)_paginationPanel);
			if (_guildModel.Puzzles.Count > 0)
			{
				CreateSearchField((Panel)(object)_secondControlPanel, _003CWindowState_003EP.SearchTerm);
				CreateListFilter((Panel)(object)_secondControlPanel, Service.Settings.PuzzleTypeFilter);
			}
			((Control)_flowPanel).set_Width(((Control)buildPanel).get_Width());
			_flowPanel.set_FlowDirection((ControlFlowDirection)0);
			((Panel)_flowPanel).set_CanScroll(true);
			FilterAndRenderList(_flowPanel, _003CWindowState_003EP.SearchTerm);
			if (_guildModel.Puzzles.Count > 0)
			{
				CreatePaginationControls((Panel)(object)_paginationPanel, _003CWindowState_003EP, _guildModel.Puzzles.Count);
			}
		}

		private void CreateListFilter(Panel panel, SettingEntry<PuzzleTypeFilterEnum> settingEntry)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			int typeDropdownWidth = 40;
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent((Container)(object)panel);
			((Control)val).set_Width(typeDropdownWidth);
			val.Show(SettingView.FromType((SettingEntry)(object)settingEntry, typeDropdownWidth));
			if (Service.Settings.ShowCameraModeFilter.get_Value())
			{
				int cameraDropdownWidth = 40;
				ViewContainer val2 = new ViewContainer();
				((Control)val2).set_Parent((Container)(object)panel);
				((Control)val2).set_Width(cameraDropdownWidth);
				val2.Show(SettingView.FromType((SettingEntry)(object)Service.Settings.CameraModeFilter, cameraDropdownWidth));
			}
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)panel);
			((Control)val3).set_Width(100);
			val3.set_Text(_003CWindowState_003EP.SortNewestFirst ? "Newest First" : "Oldest First");
			((Control)val3).set_BasicTooltipText("Toggle between newest and oldest first");
			StandardButton sortButton = val3;
			((Control)sortButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.SortNewestFirst = !_003CWindowState_003EP.SortNewestFirst;
				sortButton.set_Text(_003CWindowState_003EP.SortNewestFirst ? "Newest First" : "Oldest First");
				_003CWindowState_003EP.ResetPagination();
				_003CWindowState_003EP.SwapToGuildList();
			});
		}

		private void CreateSearchField(Panel panel, string searchTerm)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)panel);
			((Control)val).set_Width(150);
			((TextInputBase)val).set_Text(searchTerm);
			((Control)val).set_BasicTooltipText("Search by Title or Author");
			TextBox searchField = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)panel);
			((Control)val2).set_Width(100);
			val2.set_Text("Search");
			_searchButton = val2;
			searchField.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				_003CWindowState_003EP.SearchTerm = ((TextInputBase)searchField).get_Text();
				_003CWindowState_003EP.ResetPagination();
				_003CWindowState_003EP.SwapToGuildList();
			});
			((Control)_searchButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.SearchTerm = ((TextInputBase)searchField).get_Text();
				_003CWindowState_003EP.ResetPagination();
				_003CWindowState_003EP.SwapToGuildList();
			});
		}

		private void CreatePaginationControls(Panel panel, GeoGuessWindowStateService state, int puzzleCount)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Expected O, but got Unknown
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)panel);
			((Control)val).set_Width(40);
			val.set_Text("««");
			((Control)val).set_BasicTooltipText("First page");
			((Control)val).set_Enabled(_003CWindowState_003EP.CurrentPage > 0);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.GoToPage(0);
			});
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)panel);
			((Control)val2).set_Width(40);
			val2.set_Text("«");
			((Control)val2).set_BasicTooltipText("Previous 10 pages");
			((Control)val2).set_Enabled(_003CWindowState_003EP.TotalPages > 10 && _003CWindowState_003EP.CurrentPage >= 10);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.GoToPage(Math.Max(0, _003CWindowState_003EP.CurrentPage - 10));
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)panel);
			((Control)val3).set_Width(40);
			val3.set_Text("‹");
			((Control)val3).set_BasicTooltipText("Previous page");
			((Control)val3).set_Enabled(_003CWindowState_003EP.CurrentPage > 0);
			_prevButton = val3;
			((Control)_prevButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.PreviousPage();
			});
			CreateItemCountText((Panel)(object)_paginationPanel, state, puzzleCount);
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)panel);
			((Control)val4).set_Width(40);
			val4.set_Text("›");
			((Control)val4).set_BasicTooltipText("Next page");
			((Control)val4).set_Enabled(_003CWindowState_003EP.CurrentPage < _003CWindowState_003EP.TotalPages - 1);
			_nextButton = val4;
			((Control)_nextButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.NextPage();
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)panel);
			((Control)val5).set_Width(40);
			val5.set_Text("»");
			((Control)val5).set_BasicTooltipText("Next 10 pages");
			((Control)val5).set_Enabled(_003CWindowState_003EP.TotalPages > 10 && _003CWindowState_003EP.CurrentPage < _003CWindowState_003EP.TotalPages - 10);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.GoToPage(Math.Min(_003CWindowState_003EP.TotalPages - 1, _003CWindowState_003EP.CurrentPage + 10));
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)panel);
			((Control)val6).set_Width(40);
			val6.set_Text("»»");
			((Control)val6).set_BasicTooltipText("Last page");
			((Control)val6).set_Enabled(_003CWindowState_003EP.CurrentPage < _003CWindowState_003EP.TotalPages - 1);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CWindowState_003EP.GoToPage(_003CWindowState_003EP.TotalPages - 1);
			});
		}

		private int FilterAndRenderList(FlowPanel panel, string searchTerm)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			if (_guildModel.Puzzles.Count < 1)
			{
				Label val = new Label();
				val.set_Text("No GeoGuesser games are in progress for '" + _003CWindowState_003EP.SelectedGuild.Name + " [" + _003CWindowState_003EP.SelectedGuild.Tag + "]'.\n\nCreate a new game and invite your guildmates!");
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				val.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val).set_Width(((Control)panel).get_Width());
				((Control)val).set_Height(200);
				val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)0));
				val.set_TextColor(Color.get_PaleGoldenrod());
				panel.AddFlowControl((Control)(object)val);
				_003CWindowState_003EP.SetTotalPages(0);
				return 0;
			}
			_filteredItems.ForEach(delegate(GeoGuessListItem item)
			{
				item.Dispose();
			});
			_filteredItems.Clear();
			Account? account = Service.UserManager.Account;
			string user = ((account != null) ? account!.get_Name() : null) ?? "Anonymous";
			double meanUpvotes = 0.0;
			double stdDevUpvotes = 0.0;
			if (Service.Settings.PuzzleTypeFilter.get_Value() == PuzzleTypeFilterEnum.LIKED)
			{
				meanUpvotes = _guildModel.Puzzles.Average((Puzzle p) => p.Upvotes);
				stdDevUpvotes = Math.Sqrt(_guildModel.Puzzles.Sum((Puzzle p) => Math.Pow((double)p.Upvotes - meanUpvotes, 2.0)) / (double)_guildModel.Puzzles.Count);
			}
			foreach (Puzzle model in _guildModel.Puzzles)
			{
				bool owner = model.IsAuthor(user);
				bool hasGuessed = model.UserHasGuessed(user);
				bool showModel = true;
				switch (Service.Settings.PuzzleTypeFilter.get_Value())
				{
				case PuzzleTypeFilterEnum.LIKED:
					showModel = !owner && (double)model.Upvotes >= meanUpvotes + stdDevUpvotes;
					break;
				case PuzzleTypeFilterEnum.NON_COMPLETED:
					showModel = !owner && !hasGuessed;
					break;
				case PuzzleTypeFilterEnum.ONLY_COMPLETED:
					showModel = !owner && hasGuessed;
					break;
				case PuzzleTypeFilterEnum.ONLY_MINE:
					showModel = model.IsAuthor(user);
					break;
				}
				if (showModel && Service.Settings.ShowCameraModeFilter.get_Value() && Service.Settings.CameraModeFilter.get_Value() != 0)
				{
					switch (Service.Settings.CameraModeFilter.get_Value())
					{
					case CameraModeFilterEnum.FIRST_PERSON:
						showModel = model.Location.IsFirstPersonCamera();
						break;
					case CameraModeFilterEnum.THIRD_PERSON:
						showModel = !model.Location.IsFirstPersonCamera();
						break;
					}
				}
				if (showModel && (model.Title.IndexOf(searchTerm, StringComparison.InvariantCultureIgnoreCase) >= 0 || model.AccountName.IndexOf(searchTerm, StringComparison.InvariantCultureIgnoreCase) >= 0))
				{
					_filteredItems.Add(new GeoGuessListItem(model));
				}
			}
			_filteredItems.Sort(delegate(GeoGuessListItem a, GeoGuessListItem b)
			{
				int num = a.Model.CreatedAt.CompareTo(b.Model.CreatedAt);
				return (!_003CWindowState_003EP.SortNewestFirst) ? num : (-num);
			});
			_003CWindowState_003EP.SetTotalPages(_filteredItems.Count);
			if (_filteredItems.Count == 0)
			{
				return 0;
			}
			int startIndex = _003CWindowState_003EP.CurrentPage * _003CWindowState_003EP.ItemsPerPage;
			int endIndex = Math.Min(startIndex + _003CWindowState_003EP.ItemsPerPage, _filteredItems.Count);
			if (startIndex >= _filteredItems.Count)
			{
				_003CWindowState_003EP.GoToPage((_filteredItems.Count - 1) / _003CWindowState_003EP.ItemsPerPage);
				return _filteredItems.Count;
			}
			for (int i = startIndex; i < endIndex; i++)
			{
				panel.AddControl<GeoGuessListItem>(_filteredItems[i]);
			}
			return _filteredItems.Count;
		}

		private void CreateItemCountText(Panel panel, GeoGuessWindowStateService state, int all)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			if (all > 0)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)panel);
				val.set_Text($"Showing {state.CurrentPage * state.ItemsPerPage + 1}-{Math.Min((state.CurrentPage + 1) * state.ItemsPerPage, _filteredItems.Count)} of {_filteredItems.Count} (Page {state.CurrentPage + 1}/{state.TotalPages})");
				((Control)val).set_Width(250);
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				val.set_VerticalAlignment((VerticalAlignment)1);
			}
		}

		private void CreateNewGeoGuessClickHandler(object sender, MouseEventArgs e)
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				ScreenNotification.ShowNotification("Cannot create puzzles in competitive mode maps (PvP, WvW, etc.)", (NotificationType)6, (Texture2D)null, 3);
			}
			else
			{
				_003CWindowState_003EP.SwapToCreate();
			}
		}

		private void GoBackClickHandler(object sender, MouseEventArgs e)
		{
			_003CWindowState_003EP.SwapToGuildSelect();
		}

		protected override void Unload()
		{
			((Control)_createNew).remove_Click((EventHandler<MouseEventArgs>)CreateNewGeoGuessClickHandler);
			((Control)_backButton).remove_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			_filteredItems.ForEach(delegate(GeoGuessListItem item)
			{
				item.Dispose();
			});
			base.Unload();
		}
	}
}
