using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models;
using SongbookOfTyria.Models.Api;
using SongbookOfTyria.Services;
using SongbookOfTyria.UI.Views.Helpers;

namespace SongbookOfTyria.UI.Views
{
	public class SongLibraryView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<SongLibraryView>();

		private const int MenuPanelWidth = 240;

		private const int CardVerticalSpacing = 4;

		private const int VerticalPadding = 10;

		private const int LeftPadding = 50;

		private readonly TabsService _tabsService;

		private readonly UserSettingsService _userSettingsService;

		private readonly GuildAuthService _guildAuthService;

		private readonly TextureService _textureService;

		private readonly MusicTabFilter _filter;

		private readonly MusicTabSorter _sorter;

		private TabCardListManager _cardManager;

		private FlowPanel _filterPanel;

		private Panel _contentContainer;

		private Panel _headerSection;

		private FlowPanel _cardsPanel;

		private TextBox _searchBox;

		private LoadingSpinner _loadingSpinner;

		private Label _errorLabel;

		private Container _parentContainer;

		private Checkbox _soloCheckbox;

		private Checkbox _duetCheckbox;

		private Checkbox _bandCheckbox;

		private Checkbox _beginnerCheckbox;

		private Checkbox _pianoCheckbox;

		private Checkbox _practiceModeCheckbox;

		private Checkbox _favoritesCheckbox;

		private StandardButton _resetButton;

		private FlowPanel _visibilityPanel;

		private Checkbox _publicCheckbox;

		private Checkbox _privateCheckbox;

		private FlowPanel _genrePanel;

		private readonly Dictionary<string, Checkbox> _genreCheckboxes = new Dictionary<string, Checkbox>();

		private FlowPanel _tabTypePanel;

		private FlowPanel _featuresPanel;

		private FlowPanel _tabberPanel;

		private readonly Dictionary<string, Checkbox> _tabberCheckboxes = new Dictionary<string, Checkbox>();

		private StandardButton _sortByNameButton;

		private StandardButton _sortByDateButton;

		private List<MusicTab> _allTabs;

		private List<MusicTab> _displayedTabs;

		private bool _isLoading;

		private bool _hasLoaded;

		private bool _isOpeningTab;

		private string _errorMessage;

		private readonly List<Checkbox> _filterCheckboxes = new List<Checkbox>();

		public event EventHandler<MusicTab> TabClicked;

		public SongLibraryView(TabsService tabsService, TextureService textureService, UserSettingsService userSettingsService, GuildAuthService guildAuthService)
			: this()
		{
			_tabsService = tabsService;
			_textureService = textureService;
			_userSettingsService = userSettingsService;
			_guildAuthService = guildAuthService;
			_filter = new MusicTabFilter
			{
				UserSettingsService = userSettingsService
			};
			_sorter = new MusicTabSorter();
			FilterState savedState = _userSettingsService.GetFilterState();
			RestoreFilterState(savedState);
			if (_guildAuthService != null)
			{
				_guildAuthService.AuthStatusChanged += OnAuthStatusChanged;
			}
		}

		private void InitializeCardManager()
		{
			if (_cardManager != null)
			{
				_cardManager.CardClicked -= OnCardClicked;
				_cardManager.FavoriteToggled -= OnFavoriteToggled;
				_cardManager.RenderingStarted -= OnRenderingStarted;
				_cardManager.RenderingCompleted -= OnRenderingCompleted;
				_cardManager.Dispose();
			}
			_cardManager = new TabCardListManager(_textureService, _userSettingsService);
			_cardManager.CardClicked += OnCardClicked;
			_cardManager.FavoriteToggled += OnFavoriteToggled;
			_cardManager.RenderingStarted += OnRenderingStarted;
			_cardManager.RenderingCompleted += OnRenderingCompleted;
		}

		protected override void Build(Container buildPanel)
		{
			InitializeCardManager();
			_parentContainer = buildPanel;
			((Control)_parentContainer).add_Resized((EventHandler<ResizedEventArgs>)OnParentResized);
			BuildFilterPanel(buildPanel);
			BuildContentPanel(buildPanel);
			BuildLoadingControls(buildPanel);
			RestoreOrLoadTabs();
		}

		private void OnParentResized(object sender, ResizedEventArgs e)
		{
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (_parentContainer != null)
			{
				int availableHeight = _parentContainer.get_ContentRegion().Height - 20;
				int contentWidth = _parentContainer.get_ContentRegion().Width - 240 - 50 - 30;
				if (_filterPanel != null)
				{
					((Control)_filterPanel).set_Height(availableHeight);
				}
				if (_contentContainer != null)
				{
					((Control)_contentContainer).set_Size(new Point(contentWidth, availableHeight));
				}
				if (_cardsPanel != null)
				{
					FlowPanel cardsPanel = _cardsPanel;
					Panel contentContainer = _contentContainer;
					((Control)cardsPanel).set_Width((contentContainer != null) ? ((Container)contentContainer).get_ContentRegion().Width : contentWidth);
					UpdateCardsPanelLayout();
				}
				UpdateSpinnerPosition();
			}
		}

		private void BuildFilterPanel(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_Size(new Point(240, parent.get_ContentRegion().Height - 20));
			((Control)val).set_Location(new Point(50, 10));
			((Control)val).set_Parent(parent);
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_OuterControlPadding(new Vector2(-2f, 7f));
			_filterPanel = val;
			Panel val2 = new Panel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(30);
			((Control)val2).set_Parent((Container)(object)_filterPanel);
			Panel headerPanel = val2;
			Label val3 = new Label();
			val3.set_Text("Filters");
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(20, 2));
			((Control)val3).set_Parent((Container)(object)headerPanel);
			StandardButton val4 = new StandardButton();
			val4.set_Text("Reset");
			((Control)val4).set_Width(60);
			((Control)val4).set_Height(26);
			((Control)val4).set_Location(new Point(155, 0));
			((Control)val4).set_Parent((Container)(object)headerPanel);
			((Control)val4).set_Enabled(false);
			_resetButton = val4;
			((Control)_resetButton).add_Click((EventHandler<MouseEventArgs>)OnResetFiltersClicked);
			BuildTabTypeSection();
			BuildFeaturesSection();
			BuildVisibilitySection();
			BuildGenreSection();
			BuildTabberSection();
			RestoreCheckboxStates();
		}

		private void RestoreFilterState(FilterState savedState)
		{
			_filter.SearchText = savedState.SearchText ?? string.Empty;
			_filter.BeginnerOnly = savedState.BeginnerOnly;
			_filter.SoloOnly = savedState.SoloOnly;
			_filter.DuetOnly = savedState.DuetOnly;
			_filter.BandOnly = savedState.BandOnly;
			_filter.PianoOnly = savedState.PianoOnly;
			_filter.PracticeModeOnly = savedState.PracticeModeOnly;
			_filter.FavoritesOnly = savedState.FavoritesOnly;
			bool isOpusMember = _guildAuthService != null && _guildAuthService.IsOpusMember;
			_filter.PublicOnly = isOpusMember && savedState.PublicOnly;
			_filter.PrivateOnly = isOpusMember && savedState.PrivateOnly;
			_filter.SelectedGenres.Clear();
			if (savedState.SelectedGenres != null)
			{
				foreach (string genre in savedState.SelectedGenres)
				{
					_filter.SelectedGenres.Add(genre);
				}
			}
			_filter.SelectedTabbers.Clear();
			if (savedState.SelectedTabbers != null)
			{
				foreach (string tabber in savedState.SelectedTabbers)
				{
					_filter.SelectedTabbers.Add(tabber);
				}
			}
			_sorter.Mode = savedState.SortMode;
			_sorter.Ascending = savedState.SortAscending;
		}

		private void RestoreCheckboxStates()
		{
			FilterState savedState = _userSettingsService.GetFilterState();
			SetCheckboxState(_beginnerCheckbox, savedState.BeginnerOnly);
			SetCheckboxState(_soloCheckbox, savedState.SoloOnly);
			SetCheckboxState(_duetCheckbox, savedState.DuetOnly);
			SetCheckboxState(_bandCheckbox, savedState.BandOnly);
			SetCheckboxState(_pianoCheckbox, savedState.PianoOnly);
			SetCheckboxState(_practiceModeCheckbox, savedState.PracticeModeOnly);
			SetCheckboxState(_favoritesCheckbox, savedState.FavoritesOnly);
			SetCheckboxState(_publicCheckbox, savedState.PublicOnly);
			SetCheckboxState(_privateCheckbox, savedState.PrivateOnly);
			RestorePanelCollapsedStates(savedState.CollapsedPanels);
			UpdateSortButtonStates();
		}

		private static void SetCheckboxState(Checkbox checkbox, bool isChecked)
		{
			if (checkbox != null)
			{
				checkbox.set_Checked(isChecked);
			}
		}

		private void RestorePanelCollapsedStates(Dictionary<string, bool> collapsedPanels)
		{
			if (collapsedPanels != null && collapsedPanels.Count != 0)
			{
				if (_tabTypePanel != null && collapsedPanels.TryGetValue("Tab Type", out var tabTypeCollapsed))
				{
					((Panel)_tabTypePanel).set_Collapsed(tabTypeCollapsed);
				}
				if (_featuresPanel != null && collapsedPanels.TryGetValue("Features", out var featuresCollapsed))
				{
					((Panel)_featuresPanel).set_Collapsed(featuresCollapsed);
				}
				if (_visibilityPanel != null && collapsedPanels.TryGetValue("Visibility", out var visibilityCollapsed))
				{
					((Panel)_visibilityPanel).set_Collapsed(visibilityCollapsed);
				}
				if (_genrePanel != null && collapsedPanels.TryGetValue("Genre", out var genreCollapsed))
				{
					((Panel)_genrePanel).set_Collapsed(genreCollapsed);
				}
				if (_tabberPanel != null && collapsedPanels.TryGetValue("Tabbed By", out var tabberCollapsed))
				{
					((Panel)_tabberPanel).set_Collapsed(tabberCollapsed);
				}
			}
		}

		private void SavePanelCollapsedStates()
		{
			FilterState savedState = _userSettingsService.GetFilterState();
			if (_tabTypePanel != null)
			{
				savedState.CollapsedPanels["Tab Type"] = ((Panel)_tabTypePanel).get_Collapsed();
			}
			if (_featuresPanel != null)
			{
				savedState.CollapsedPanels["Features"] = ((Panel)_featuresPanel).get_Collapsed();
			}
			if (_visibilityPanel != null)
			{
				savedState.CollapsedPanels["Visibility"] = ((Panel)_visibilityPanel).get_Collapsed();
			}
			if (_genrePanel != null)
			{
				savedState.CollapsedPanels["Genre"] = ((Panel)_genrePanel).get_Collapsed();
			}
			if (_tabberPanel != null)
			{
				savedState.CollapsedPanels["Tabbed By"] = ((Panel)_tabberPanel).get_Collapsed();
			}
			_userSettingsService.SaveFilterState(savedState);
		}

		private void RestoreGenreAndTabberCheckboxes()
		{
			FilterState savedState = _userSettingsService.GetFilterState();
			foreach (KeyValuePair<string, Checkbox> kvp2 in _genreCheckboxes)
			{
				kvp2.Value.set_Checked(savedState.SelectedGenres?.Contains(kvp2.Key) ?? false);
			}
			foreach (KeyValuePair<string, Checkbox> kvp in _tabberCheckboxes)
			{
				kvp.Value.set_Checked(savedState.SelectedTabbers?.Contains(kvp.Key) ?? false);
			}
		}

		private void SaveFilterState()
		{
			FilterState filterState2 = new FilterState();
			TextBox searchBox = _searchBox;
			filterState2.SearchText = ((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null) ?? string.Empty;
			Checkbox beginnerCheckbox = _beginnerCheckbox;
			filterState2.BeginnerOnly = beginnerCheckbox != null && beginnerCheckbox.get_Checked();
			Checkbox soloCheckbox = _soloCheckbox;
			filterState2.SoloOnly = soloCheckbox != null && soloCheckbox.get_Checked();
			Checkbox duetCheckbox = _duetCheckbox;
			filterState2.DuetOnly = duetCheckbox != null && duetCheckbox.get_Checked();
			Checkbox bandCheckbox = _bandCheckbox;
			filterState2.BandOnly = bandCheckbox != null && bandCheckbox.get_Checked();
			Checkbox pianoCheckbox = _pianoCheckbox;
			filterState2.PianoOnly = pianoCheckbox != null && pianoCheckbox.get_Checked();
			Checkbox practiceModeCheckbox = _practiceModeCheckbox;
			filterState2.PracticeModeOnly = practiceModeCheckbox != null && practiceModeCheckbox.get_Checked();
			Checkbox favoritesCheckbox = _favoritesCheckbox;
			filterState2.FavoritesOnly = favoritesCheckbox != null && favoritesCheckbox.get_Checked();
			Checkbox publicCheckbox = _publicCheckbox;
			filterState2.PublicOnly = publicCheckbox != null && publicCheckbox.get_Checked();
			Checkbox privateCheckbox = _privateCheckbox;
			filterState2.PrivateOnly = privateCheckbox != null && privateCheckbox.get_Checked();
			filterState2.SelectedGenres = new List<string>(_filter.SelectedGenres);
			filterState2.SelectedTabbers = new List<string>(_filter.SelectedTabbers);
			filterState2.SortMode = _sorter.Mode;
			filterState2.SortAscending = _sorter.Ascending;
			FilterState filterState = filterState2;
			_userSettingsService.SaveFilterState(filterState);
		}

		private void OnResetFiltersClicked(object sender, MouseEventArgs e)
		{
			ResetCheckbox(_beginnerCheckbox);
			ResetCheckbox(_soloCheckbox);
			ResetCheckbox(_duetCheckbox);
			ResetCheckbox(_bandCheckbox);
			ResetCheckbox(_pianoCheckbox);
			ResetCheckbox(_practiceModeCheckbox);
			ResetCheckbox(_favoritesCheckbox);
			ResetCheckbox(_publicCheckbox);
			ResetCheckbox(_privateCheckbox);
			foreach (Checkbox value in _genreCheckboxes.Values)
			{
				value.set_Checked(false);
			}
			_filter.SelectedGenres.Clear();
			foreach (Checkbox value2 in _tabberCheckboxes.Values)
			{
				value2.set_Checked(false);
			}
			_filter.SelectedTabbers.Clear();
			if (_searchBox != null)
			{
				((TextInputBase)_searchBox).set_Text(string.Empty);
			}
			ApplyFiltersAndSort();
		}

		private static void ResetCheckbox(Checkbox checkbox)
		{
			if (checkbox != null)
			{
				checkbox.set_Checked(false);
			}
		}

		private void BuildTabTypeSection()
		{
			_tabTypePanel = CreateFilterSection("Tab Type");
			_soloCheckbox = CreateFilterCheckbox("Solo", (Container)(object)_tabTypePanel);
			_duetCheckbox = CreateFilterCheckbox("Duet", (Container)(object)_tabTypePanel);
			_bandCheckbox = CreateFilterCheckbox("Band", (Container)(object)_tabTypePanel);
			AddBottomSpacer((Container)(object)_tabTypePanel);
		}

		private void BuildFeaturesSection()
		{
			_featuresPanel = CreateFilterSection("Features");
			_beginnerCheckbox = CreateFilterCheckbox("Beginner Friendly", (Container)(object)_featuresPanel);
			_favoritesCheckbox = CreateFilterCheckbox("Favorites", (Container)(object)_featuresPanel);
			_pianoCheckbox = CreateFilterCheckbox("Piano", (Container)(object)_featuresPanel);
			_practiceModeCheckbox = CreateFilterCheckbox("Practice Mode", (Container)(object)_featuresPanel);
			AddBottomSpacer((Container)(object)_featuresPanel);
		}

		private void BuildVisibilitySection()
		{
			if (_guildAuthService != null && _guildAuthService.IsOpusMember)
			{
				_visibilityPanel = CreateFilterSection("Visibility");
				_publicCheckbox = CreateFilterCheckbox("Public", (Container)(object)_visibilityPanel);
				_privateCheckbox = CreateFilterCheckbox("Private", (Container)(object)_visibilityPanel);
				AddBottomSpacer((Container)(object)_visibilityPanel);
			}
		}

		private void OnAuthStatusChanged(object sender, GuildAuthStatusChangedEventArgs e)
		{
			if (e.IsOpusMember)
			{
				if (_visibilityPanel == null && _filterPanel != null)
				{
					RebuildDynamicFilterSections();
				}
			}
			else
			{
				RemoveVisibilitySection();
			}
		}

		private void RebuildDynamicFilterSections()
		{
			RemoveGenreSection();
			RemoveTabberSection();
			BuildVisibilitySection();
			BuildGenreSection();
			BuildTabberSection();
			if (_allTabs != null)
			{
				PopulateGenreCheckboxes();
				PopulateTabberCheckboxes();
			}
			FlowPanel filterPanel = _filterPanel;
			if (filterPanel != null)
			{
				((Control)filterPanel).Invalidate();
			}
		}

		private void RemoveGenreSection()
		{
			foreach (Checkbox value in _genreCheckboxes.Values)
			{
				value.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnGenreCheckboxChanged);
			}
			_genreCheckboxes.Clear();
			FlowPanel genrePanel = _genrePanel;
			if (genrePanel != null)
			{
				((Control)genrePanel).Dispose();
			}
			_genrePanel = null;
		}

		private void RemoveTabberSection()
		{
			foreach (Checkbox value in _tabberCheckboxes.Values)
			{
				value.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnTabberCheckboxChanged);
			}
			_tabberCheckboxes.Clear();
			FlowPanel tabberPanel = _tabberPanel;
			if (tabberPanel != null)
			{
				((Control)tabberPanel).Dispose();
			}
			_tabberPanel = null;
		}

		private void RemoveVisibilitySection()
		{
			if (_visibilityPanel != null)
			{
				if (_publicCheckbox != null)
				{
					_publicCheckbox.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnFilterCheckboxChanged);
					_filterCheckboxes.Remove(_publicCheckbox);
					((Control)_publicCheckbox).Dispose();
					_publicCheckbox = null;
				}
				if (_privateCheckbox != null)
				{
					_privateCheckbox.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnFilterCheckboxChanged);
					_filterCheckboxes.Remove(_privateCheckbox);
					((Control)_privateCheckbox).Dispose();
					_privateCheckbox = null;
				}
				((Control)_visibilityPanel).Dispose();
				_visibilityPanel = null;
				_filter.PublicOnly = false;
				_filter.PrivateOnly = false;
				if (_allTabs != null)
				{
					ApplyFiltersAndSort();
				}
			}
		}

		private void BuildGenreSection()
		{
			_genrePanel = CreateFilterSection("Genre");
		}

		private void BuildTabberSection()
		{
			_tabberPanel = CreateFilterSection("Tabbed By");
		}

		private FlowPanel CreateFilterSection(string title)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_Title(title);
			((Control)val).set_Width(223);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)_filterPanel);
			((Panel)val).set_CanCollapse(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			return val;
		}

		private Checkbox CreateFilterCheckbox(string text, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			val.set_Text(text);
			((Control)val).set_Parent(parent);
			Checkbox checkbox = val;
			checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnFilterCheckboxChanged);
			_filterCheckboxes.Add(checkbox);
			return checkbox;
		}

		private void AddBottomSpacer(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Width(1);
			((Control)val).set_Height(5);
			((Control)val).set_Parent(parent);
		}

		private void PopulateGenreCheckboxes()
		{
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Expected O, but got Unknown
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			if (_allTabs == null || _genrePanel == null || _genreCheckboxes == null)
			{
				return;
			}
			foreach (Checkbox value in _genreCheckboxes.Values)
			{
				value.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnGenreCheckboxChanged);
			}
			_genreCheckboxes.Clear();
			((Container)_genrePanel).ClearChildren();
			foreach (KeyValuePair<string, int> genreCount in from kvp in (from t in _allTabs
					where !string.IsNullOrEmpty(t.Genre)
					group t by t.Genre).ToDictionary((IGrouping<string, MusicTab> g) => g.Key, (IGrouping<string, MusicTab> g) => g.Count())
				orderby kvp.Value descending, kvp.Key
				select kvp)
			{
				if (!string.IsNullOrEmpty(genreCount.Key))
				{
					Checkbox val = new Checkbox();
					val.set_Text($"{genreCount.Key} ({genreCount.Value})");
					((Control)val).set_Parent((Container)(object)_genrePanel);
					Checkbox checkbox = val;
					checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnGenreCheckboxChanged);
					_genreCheckboxes[genreCount.Key] = checkbox;
				}
			}
			Panel val2 = new Panel();
			((Control)val2).set_Width(1);
			((Control)val2).set_Height(5);
			((Control)val2).set_Parent((Container)(object)_genrePanel);
			RestoreGenreAndTabberCheckboxes();
		}

		private void OnGenreCheckboxChanged(object sender, CheckChangedEvent e)
		{
			_filter.SelectedGenres.Clear();
			foreach (KeyValuePair<string, Checkbox> kvp in _genreCheckboxes)
			{
				if (kvp.Value.get_Checked())
				{
					_filter.SelectedGenres.Add(kvp.Key);
				}
			}
			ApplyFiltersAndSort();
		}

		private void PopulateTabberCheckboxes()
		{
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Expected O, but got Unknown
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			if (_allTabs == null || _tabberPanel == null || _tabberCheckboxes == null)
			{
				return;
			}
			foreach (Checkbox value in _tabberCheckboxes.Values)
			{
				value.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnTabberCheckboxChanged);
			}
			_tabberCheckboxes.Clear();
			((Container)_tabberPanel).ClearChildren();
			Dictionary<string, int> tabberCounts = new Dictionary<string, int>();
			HashSet<string> excludedTabbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "abyss", "nes", "none" };
			foreach (MusicTab tab in _allTabs)
			{
				if (tab.TabbedByMember != null && tab.TabbedByMember.Count > 0)
				{
					foreach (string member in tab.TabbedByMember)
					{
						if (!string.IsNullOrEmpty(member) && !excludedTabbers.Contains(member))
						{
							if (!tabberCounts.ContainsKey(member))
							{
								tabberCounts[member] = 0;
							}
							tabberCounts[member]++;
						}
					}
				}
				else
				{
					if (string.IsNullOrEmpty(tab.TabbedBy))
					{
						continue;
					}
					string[] array = tab.TabbedBy.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						string trimmedTabber = array[i].Trim();
						if (!string.IsNullOrEmpty(trimmedTabber) && !excludedTabbers.Contains(trimmedTabber))
						{
							if (!tabberCounts.ContainsKey(trimmedTabber))
							{
								tabberCounts[trimmedTabber] = 0;
							}
							tabberCounts[trimmedTabber]++;
						}
					}
				}
			}
			foreach (KeyValuePair<string, int> tabberCount in from kvp in tabberCounts
				orderby kvp.Value descending, kvp.Key
				select kvp)
			{
				Checkbox val = new Checkbox();
				val.set_Text($"{tabberCount.Key} ({tabberCount.Value})");
				((Control)val).set_Parent((Container)(object)_tabberPanel);
				Checkbox checkbox = val;
				checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnTabberCheckboxChanged);
				_tabberCheckboxes[tabberCount.Key] = checkbox;
			}
			Panel val2 = new Panel();
			((Control)val2).set_Width(1);
			((Control)val2).set_Height(5);
			((Control)val2).set_Parent((Container)(object)_tabberPanel);
		}

		private void OnTabberCheckboxChanged(object sender, CheckChangedEvent e)
		{
			_filter.SelectedTabbers.Clear();
			foreach (KeyValuePair<string, Checkbox> kvp in _tabberCheckboxes)
			{
				if (kvp.Value.get_Checked())
				{
					_filter.SelectedTabbers.Add(kvp.Key);
				}
			}
			ApplyFiltersAndSort();
		}

		private void UpdateFilterCounts()
		{
			if (_allTabs == null)
			{
				return;
			}
			List<MusicTab> currentFiltered = _displayedTabs ?? _allTabs;
			UpdateCheckboxText(_beginnerCheckbox, "Beginner Friendly", currentFiltered.Count((MusicTab t) => t.IsBeginner));
			UpdateCheckboxText(_soloCheckbox, "Solo", currentFiltered.Count((MusicTab t) => t.TabType != null && t.TabType.Contains("Solo")));
			UpdateCheckboxText(_duetCheckbox, "Duet", currentFiltered.Count((MusicTab t) => t.TabType != null && t.TabType.Contains("Duet")));
			UpdateCheckboxText(_bandCheckbox, "Band", currentFiltered.Count((MusicTab t) => t.TabType != null && t.TabType.Contains("Band")));
			UpdateCheckboxText(_pianoCheckbox, "Piano", currentFiltered.Count((MusicTab t) => t.Piano));
			UpdateCheckboxText(_practiceModeCheckbox, "Practice Mode", currentFiltered.Count((MusicTab t) => t.PracticeMode));
			UpdateCheckboxText(_favoritesCheckbox, "Favorites", (_userSettingsService != null) ? currentFiltered.Count((MusicTab t) => _userSettingsService.IsFavorite(t.Id)) : 0);
			UpdateCheckboxText(_publicCheckbox, "Public", currentFiltered.Count((MusicTab t) => !t.IsPrivate));
			UpdateCheckboxText(_privateCheckbox, "Private", currentFiltered.Count((MusicTab t) => t.IsPrivate));
			foreach (KeyValuePair<string, Checkbox> kvp2 in _genreCheckboxes)
			{
				int genreCount = currentFiltered.Count((MusicTab t) => t.Genre == kvp2.Key);
				kvp2.Value.set_Text($"{kvp2.Key} ({genreCount})");
			}
			foreach (KeyValuePair<string, Checkbox> kvp in _tabberCheckboxes)
			{
				int tabberCount = currentFiltered.Count((MusicTab t) => t.TabbedBy == kvp.Key || (t.TabbedByMember != null && t.TabbedByMember.Contains(kvp.Key)));
				kvp.Value.set_Text($"{kvp.Key} ({tabberCount})");
			}
		}

		private static void UpdateCheckboxText(Checkbox checkbox, string label, int count)
		{
			if (checkbox != null)
			{
				checkbox.set_Text($"{label} ({count})");
			}
		}

		private void OnFilterCheckboxChanged(object sender, CheckChangedEvent e)
		{
			ApplyFiltersAndSort();
		}

		private void BuildContentPanel(Container parent)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			int contentLeft = 296;
			int contentWidth = parent.get_ContentRegion().Width - 240 - 50 - 30;
			Panel val = new Panel();
			((Control)val).set_Size(new Point(contentWidth, parent.get_ContentRegion().Height - 20));
			((Control)val).set_Location(new Point(contentLeft, 10));
			val.set_ShowBorder(true);
			((Control)val).set_Parent(parent);
			_contentContainer = val;
			BuildHeaderSection();
			BuildCardsPanel();
		}

		private void BuildHeaderSection()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(50);
			((Control)val).set_Parent((Container)(object)_contentContainer);
			_headerSection = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Width(250);
			((Control)val2).set_Height(30);
			((Control)val2).set_Location(new Point(10, 10));
			((TextInputBase)val2).set_PlaceholderText("Search songs...");
			((TextInputBase)val2).set_Text(_filter.SearchText ?? string.Empty);
			((Control)val2).set_Parent((Container)(object)_headerSection);
			_searchBox = val2;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)OnSearchTextChanged);
			Label val3 = new Label();
			val3.set_Text("Sort:");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(280, 15));
			((Control)val3).set_Parent((Container)(object)_headerSection);
			StandardButton val4 = new StandardButton();
			val4.set_Text("Name A-Z");
			((Control)val4).set_Width(100);
			((Control)val4).set_Height(30);
			((Control)val4).set_Location(new Point(320, 10));
			((Control)val4).set_Parent((Container)(object)_headerSection);
			_sortByNameButton = val4;
			((Control)_sortByNameButton).add_Click((EventHandler<MouseEventArgs>)OnSortByNameClicked);
			StandardButton val5 = new StandardButton();
			val5.set_Text("Date");
			((Control)val5).set_Width(110);
			((Control)val5).set_Height(30);
			((Control)val5).set_Location(new Point(430, 10));
			((Control)val5).set_Parent((Container)(object)_headerSection);
			_sortByDateButton = val5;
			((Control)_sortByDateButton).add_Click((EventHandler<MouseEventArgs>)OnSortByDateClicked);
			UpdateSortButtonStates();
		}

		private void BuildCardsPanel()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Size(new Point(((Container)_contentContainer).get_ContentRegion().Width, ((Container)_contentContainer).get_ContentRegion().Height - ((Control)_headerSection).get_Height()));
			((Control)val).set_Location(new Point(0, ((Control)_headerSection).get_Height()));
			val.set_ControlPadding(new Vector2(0f, 4f));
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Parent((Container)(object)_contentContainer);
			_cardsPanel = val;
			_cardManager.SetCardsPanel(_cardsPanel);
		}

		private void UpdateCardsPanelLayout()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (_cardsPanel != null && _contentContainer != null && _headerSection != null)
			{
				((Control)_cardsPanel).set_Location(new Point(0, ((Control)_headerSection).get_Height()));
				((Control)_cardsPanel).set_Height(((Container)_contentContainer).get_ContentRegion().Height - ((Control)_headerSection).get_Height());
			}
		}

		private void BuildLoadingControls(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent((Container)(object)_contentContainer);
			((Control)val).set_Visible(false);
			((Control)val).set_ZIndex(100);
			_loadingSpinner = val;
			UpdateSpinnerPosition();
			Label val2 = new Label();
			val2.set_Text(string.Empty);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_TextColor(Color.get_White());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)_contentContainer);
			((Control)val2).set_Visible(false);
			((Control)val2).set_ZIndex(100);
			_errorLabel = val2;
		}

		private void UpdateSpinnerPosition()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (_loadingSpinner != null && _contentContainer != null)
			{
				int centerX = (((Container)_contentContainer).get_ContentRegion().Width - ((Control)_loadingSpinner).get_Width()) / 2;
				int centerY = (((Container)_contentContainer).get_ContentRegion().Height - ((Control)_loadingSpinner).get_Height()) / 2;
				((Control)_loadingSpinner).set_Location(new Point(centerX, centerY));
				if (_errorLabel != null)
				{
					((Control)_errorLabel).set_Location(new Point(centerX, centerY));
				}
			}
		}

		private void OnSearchTextChanged(object sender, EventArgs e)
		{
			ApplyFiltersAndSort();
		}

		private void OnSortByNameClicked(object sender, EventArgs e)
		{
			if (_sorter.Mode == SortMode.Name)
			{
				_sorter.Ascending = !_sorter.Ascending;
			}
			else
			{
				_sorter.Mode = SortMode.Name;
				_sorter.Ascending = true;
			}
			UpdateSortButtonStates();
			ApplyFiltersAndSort();
		}

		private void OnSortByDateClicked(object sender, EventArgs e)
		{
			if (_sorter.Mode == SortMode.ReleaseDate)
			{
				_sorter.Ascending = !_sorter.Ascending;
			}
			else
			{
				_sorter.Mode = SortMode.ReleaseDate;
				_sorter.Ascending = false;
			}
			UpdateSortButtonStates();
			ApplyFiltersAndSort();
		}

		private void UpdateSortButtonStates()
		{
			if (_sortByNameButton != null)
			{
				_sortByNameButton.set_Text((_sorter.Mode != SortMode.Name) ? "Name" : (_sorter.Ascending ? "Name A-Z" : "Name Z-A"));
			}
			if (_sortByDateButton != null)
			{
				_sortByDateButton.set_Text((_sorter.Mode != SortMode.ReleaseDate) ? "Date" : (_sorter.Ascending ? "Date Old-New" : "Date New-Old"));
			}
		}

		private void RestoreOrLoadTabs()
		{
			TabsResponse cachedResponse = _tabsService.GetCachedTabs();
			if (cachedResponse?.Tabs != null && cachedResponse.Tabs.Count > 0)
			{
				_allTabs = cachedResponse.Tabs;
				_displayedTabs = new List<MusicTab>(_allTabs);
				_hasLoaded = true;
				PopulateGenreCheckboxes();
				PopulateTabberCheckboxes();
				ApplyFiltersAndSort();
				if (_errorLabel != null)
				{
					((Control)_errorLabel).set_Visible(false);
				}
			}
			else if (_hasLoaded && _allTabs != null)
			{
				RestoreCachedTabs();
			}
			else if (_hasLoaded && _errorMessage != null)
			{
				ShowError(_errorMessage);
			}
			else if (_isLoading)
			{
				ShowLoading();
			}
			else if (!_hasLoaded)
			{
				RefreshTabListWithErrorHandling();
			}
		}

		private void RestoreCachedTabs()
		{
			_displayedTabs = new List<MusicTab>(_allTabs);
			ApplyFiltersAndSort();
			SetControlVisible((Control)(object)_errorLabel, visible: false);
		}

		private void ShowError(string message)
		{
			SetControlVisible((Control)(object)_loadingSpinner, visible: false);
			if (_errorLabel != null)
			{
				_errorLabel.set_Text(message);
				((Control)_errorLabel).set_Visible(true);
			}
			SetControlVisible((Control)(object)_cardsPanel, visible: false);
		}

		private void ShowLoading()
		{
			SetControlVisible((Control)(object)_loadingSpinner, visible: true);
			SetControlVisible((Control)(object)_errorLabel, visible: false);
		}

		private static void SetControlVisible(Control control, bool visible)
		{
			if (control != null)
			{
				control.set_Visible(visible);
			}
		}

		private void ApplyFiltersAndSort()
		{
			if (_allTabs != null && _cardsPanel != null)
			{
				UpdateFilterFromCheckboxes();
				MusicTabFilter filter = _filter;
				TextBox searchBox = _searchBox;
				filter.SearchText = ((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null) ?? string.Empty;
				_displayedTabs = _filter.Apply(_allTabs);
				_displayedTabs = _sorter.Apply(_displayedTabs);
				UpdateFilterCounts();
				UpdateResetButtonState();
				RefreshCardList();
				SaveFilterState();
			}
		}

		private void UpdateResetButtonState()
		{
			if (_resetButton == null)
			{
				return;
			}
			TextBox searchBox = _searchBox;
			int num;
			if (string.IsNullOrEmpty((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null))
			{
				Checkbox beginnerCheckbox = _beginnerCheckbox;
				if (beginnerCheckbox == null || !beginnerCheckbox.get_Checked())
				{
					Checkbox soloCheckbox = _soloCheckbox;
					if (soloCheckbox == null || !soloCheckbox.get_Checked())
					{
						Checkbox duetCheckbox = _duetCheckbox;
						if (duetCheckbox == null || !duetCheckbox.get_Checked())
						{
							Checkbox bandCheckbox = _bandCheckbox;
							if (bandCheckbox == null || !bandCheckbox.get_Checked())
							{
								Checkbox pianoCheckbox = _pianoCheckbox;
								if (pianoCheckbox == null || !pianoCheckbox.get_Checked())
								{
									Checkbox practiceModeCheckbox = _practiceModeCheckbox;
									if (practiceModeCheckbox == null || !practiceModeCheckbox.get_Checked())
									{
										Checkbox favoritesCheckbox = _favoritesCheckbox;
										if (favoritesCheckbox == null || !favoritesCheckbox.get_Checked())
										{
											Checkbox publicCheckbox = _publicCheckbox;
											if (publicCheckbox == null || !publicCheckbox.get_Checked())
											{
												Checkbox privateCheckbox = _privateCheckbox;
												if ((privateCheckbox == null || !privateCheckbox.get_Checked()) && _filter.SelectedGenres.Count <= 0)
												{
													num = ((_filter.SelectedTabbers.Count > 0) ? 1 : 0);
													goto IL_010e;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			num = 1;
			goto IL_010e;
			IL_010e:
			bool hasActiveFilters = (byte)num != 0;
			((Control)_resetButton).set_Enabled(hasActiveFilters);
		}

		private void UpdateFilterFromCheckboxes()
		{
			MusicTabFilter filter = _filter;
			Checkbox beginnerCheckbox = _beginnerCheckbox;
			filter.BeginnerOnly = beginnerCheckbox != null && beginnerCheckbox.get_Checked();
			MusicTabFilter filter2 = _filter;
			Checkbox soloCheckbox = _soloCheckbox;
			filter2.SoloOnly = soloCheckbox != null && soloCheckbox.get_Checked();
			MusicTabFilter filter3 = _filter;
			Checkbox duetCheckbox = _duetCheckbox;
			filter3.DuetOnly = duetCheckbox != null && duetCheckbox.get_Checked();
			MusicTabFilter filter4 = _filter;
			Checkbox bandCheckbox = _bandCheckbox;
			filter4.BandOnly = bandCheckbox != null && bandCheckbox.get_Checked();
			MusicTabFilter filter5 = _filter;
			Checkbox pianoCheckbox = _pianoCheckbox;
			filter5.PianoOnly = pianoCheckbox != null && pianoCheckbox.get_Checked();
			MusicTabFilter filter6 = _filter;
			Checkbox practiceModeCheckbox = _practiceModeCheckbox;
			filter6.PracticeModeOnly = practiceModeCheckbox != null && practiceModeCheckbox.get_Checked();
			MusicTabFilter filter7 = _filter;
			Checkbox favoritesCheckbox = _favoritesCheckbox;
			filter7.FavoritesOnly = favoritesCheckbox != null && favoritesCheckbox.get_Checked();
			MusicTabFilter filter8 = _filter;
			Checkbox publicCheckbox = _publicCheckbox;
			filter8.PublicOnly = publicCheckbox != null && publicCheckbox.get_Checked();
			MusicTabFilter filter9 = _filter;
			Checkbox privateCheckbox = _privateCheckbox;
			filter9.PrivateOnly = privateCheckbox != null && privateCheckbox.get_Checked();
		}

		private void RefreshCardList()
		{
			if (_cardsPanel != null)
			{
				if (_displayedTabs == null || _displayedTabs.Count == 0)
				{
					((Container)_cardsPanel).ClearChildren();
					ShowEmptyMessage();
					HideLoadingSpinner();
				}
				else
				{
					_cardManager?.RefreshCards(_displayedTabs);
				}
			}
		}

		private void OnRenderingStarted(object sender, EventArgs e)
		{
			ShowLoadingSpinner();
		}

		private void OnRenderingCompleted(object sender, EventArgs e)
		{
			HideLoadingSpinner();
		}

		private void ShowLoadingSpinner()
		{
			SetControlVisible((Control)(object)_loadingSpinner, visible: true);
			SetControlVisible((Control)(object)_cardsPanel, visible: false);
		}

		private void HideLoadingSpinner()
		{
			SetControlVisible((Control)(object)_loadingSpinner, visible: false);
			SetControlVisible((Control)(object)_cardsPanel, visible: true);
		}

		private void OnFavoriteToggled(object sender, MusicTab tab)
		{
			UpdateFilterCounts();
		}

		private void OnCardClicked(object sender, MusicTab tab)
		{
			if (!_isOpeningTab && this.TabClicked != null)
			{
				_isOpeningTab = true;
				this.TabClicked(this, tab);
			}
		}

		public void SetTabOpeningComplete()
		{
			_isOpeningTab = false;
		}

		private void ShowEmptyMessage()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("No songs found");
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(Color.get_LightGray());
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Location(new Point(10, 10));
			((Control)val).set_Parent((Container)(object)_cardsPanel);
		}

		private async void RefreshTabListWithErrorHandling()
		{
			try
			{
				await RefreshTabListAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to refresh tab list");
				_errorMessage = "Failed to load tabs.";
				_isLoading = false;
				_hasLoaded = true;
				HandleLoadError();
			}
		}

		public async Task RefreshTabListAsync()
		{
			if (!_isLoading)
			{
				_isLoading = true;
				_errorMessage = null;
				PrepareForLoading();
				TabsResponse tabsResponse = await _tabsService.GetTabsAsync().ConfigureAwait(continueOnCapturedContext: false);
				_isLoading = false;
				_hasLoaded = true;
				if (tabsResponse == null)
				{
					HandleLoadError();
					return;
				}
				ProcessTabsResponse(tabsResponse);
				ShowLoadedTabs();
			}
		}

		private void PrepareForLoading()
		{
			if (_loadingSpinner != null)
			{
				((Control)_loadingSpinner).set_Visible(true);
			}
			if (_errorLabel != null)
			{
				((Control)_errorLabel).set_Visible(false);
			}
			_cardManager?.ClearAllCards();
		}

		private void HandleLoadError()
		{
			_errorMessage = "Failed to load tabs.";
			if (_loadingSpinner != null)
			{
				((Control)_loadingSpinner).set_Visible(false);
			}
			if (_errorLabel != null)
			{
				_errorLabel.set_Text(_errorMessage);
				((Control)_errorLabel).set_Visible(true);
			}
		}

		private void ProcessTabsResponse(TabsResponse response)
		{
			_allTabs = response.Tabs;
			_displayedTabs = new List<MusicTab>(_allTabs);
			UpdateFilterCounts();
			PopulateGenreCheckboxes();
			PopulateTabberCheckboxes();
			if (_allTabs != null && _cardsPanel != null)
			{
				ApplyFiltersAndSort();
			}
		}

		private void ShowLoadedTabs()
		{
			if (_errorLabel != null)
			{
				((Control)_errorLabel).set_Visible(false);
			}
		}

		protected override void Unload()
		{
			SavePanelCollapsedStates();
			if (_parentContainer != null)
			{
				((Control)_parentContainer).remove_Resized((EventHandler<ResizedEventArgs>)OnParentResized);
			}
			if (_searchBox != null)
			{
				((TextInputBase)_searchBox).remove_TextChanged((EventHandler<EventArgs>)OnSearchTextChanged);
			}
			if (_resetButton != null)
			{
				((Control)_resetButton).remove_Click((EventHandler<MouseEventArgs>)OnResetFiltersClicked);
			}
			if (_sortByNameButton != null)
			{
				((Control)_sortByNameButton).remove_Click((EventHandler<MouseEventArgs>)OnSortByNameClicked);
			}
			if (_sortByDateButton != null)
			{
				((Control)_sortByDateButton).remove_Click((EventHandler<MouseEventArgs>)OnSortByDateClicked);
			}
			foreach (Checkbox filterCheckbox in _filterCheckboxes)
			{
				filterCheckbox.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnFilterCheckboxChanged);
			}
			_filterCheckboxes.Clear();
			foreach (Checkbox value in _genreCheckboxes.Values)
			{
				value.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnGenreCheckboxChanged);
			}
			_genreCheckboxes.Clear();
			foreach (Checkbox value2 in _tabberCheckboxes.Values)
			{
				value2.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnTabberCheckboxChanged);
			}
			_tabberCheckboxes.Clear();
			_cardManager?.ClearAllCards();
			((View<IPresenter>)this).Unload();
		}
	}
}
