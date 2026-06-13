using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using CinemaModule.Controllers;
using CinemaModule.Models;
using CinemaModule.Models.Location;
using CinemaModule.Services;
using CinemaModule.Settings;
using CinemaModule.UI.Windows.Dialogs;
using CinemaModule.UI.Windows.Info;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace CinemaModule.UI.Windows.MainSettings
{
	public class DisplayTabView : View
	{
		private class SavedLocationExport
		{
			public string Name { get; set; }

			public WorldPosition3D Position { get; set; }

			public float ScreenWidth { get; set; }
		}

		private const int MenuPanelWidth = 240;

		private const int CardVerticalSpacing = 4;

		private const string KeyPrefixPreset = "preset:";

		private const string KeyPrefixSaved = "saved:";

		private const string CategoryMyLocations = "My Locations";

		private static readonly Logger Logger = Logger.GetLogger<DisplayTabView>();

		private readonly CinemaSettings _cinemaSettings;

		private readonly CinemaUserSettings _settings;

		private readonly CinemaController _controller;

		private readonly Gw2MapService _mapService;

		private readonly PresetService _presetService;

		private Checkbox _enabledCheckbox;

		private Dropdown _displayModeDropdown;

		private Panel _windowHelpSection;

		private Panel _locationSection;

		private Menu _categoryMenu;

		private Panel _menuPanel;

		private Panel _contentContainer;

		private Panel _headerSection;

		private FlowPanel _cardsPanel;

		private readonly Dictionary<string, ListCard> _locationCards = new Dictionary<string, ListCard>();

		private readonly Dictionary<string, WorldLocationCategory> _categoryLookup = new Dictionary<string, WorldLocationCategory>();

		private string _selectedLocationKey;

		private string _selectedCategoryId;

		private LocationEditorWindow _editorWindow;

		private LocationInfoWindow _presetInfoWindow;

		private EventHandler _presetsLoadedHandler;

		private EventHandler _savedLocationsChangedHandler;

		private EventHandler<ValueChangedEventArgs<bool>> _enabledSettingChangedHandler;

		private EventHandler<ResizedEventArgs> _parentResizedHandler;

		private Container _buildPanel;

		public DisplayTabView(CinemaSettings cinemaSettings, CinemaUserSettings settings, CinemaController controller, Gw2MapService mapService, PresetService presetService)
			: this()
		{
			_cinemaSettings = cinemaSettings;
			_settings = settings;
			_controller = controller;
			_mapService = mapService;
			_presetService = presetService;
		}

		protected override void Build(Container buildPanel)
		{
			_buildPanel = buildPanel;
			InitializeSelectedLocationKey();
			BuildDisplaySettingsPanel(buildPanel);
			BuildWindowHelpSection(buildPanel);
			BuildLocationSection(buildPanel);
			SubscribeToEvents();
			UpdateVisibility();
		}

		private void BuildDisplaySettingsPanel(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Display Settings");
			((Control)val).set_Size(new Point(((Control)parent).get_Width() - 70, 120));
			((Control)val).set_Location(new Point(23, 10));
			((Control)val).set_Parent(parent);
			Panel displaySettingsPanel = val;
			BuildDisplayModeSection((Container)(object)displaySettingsPanel);
		}

		private void BuildDisplayModeSection(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			Checkbox val = new Checkbox();
			val.set_Text("Enabled");
			val.set_Checked(_cinemaSettings.IsEnabled);
			((Control)val).set_Location(new Point(10, 15));
			((Control)val).set_Parent(parent);
			_enabledCheckbox = val;
			_enabledCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_cinemaSettings.EnabledSetting.set_Value(_enabledCheckbox.get_Checked());
				UpdateVisibility();
			});
			_enabledSettingChangedHandler = delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (_enabledCheckbox.get_Checked() != e.get_NewValue())
				{
					_enabledCheckbox.set_Checked(e.get_NewValue());
				}
			};
			_cinemaSettings.EnabledSetting.add_SettingChanged(_enabledSettingChangedHandler);
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Width(160);
			((Control)val2).set_Location(new Point(120, 13));
			((Control)val2).set_Parent(parent);
			_displayModeDropdown = val2;
			_displayModeDropdown.get_Items().Add(GetDisplayModeName(CinemaDisplayMode.InGame));
			_displayModeDropdown.get_Items().Add(GetDisplayModeName(CinemaDisplayMode.OnScreen));
			_displayModeDropdown.set_SelectedItem(GetDisplayModeName(_settings.DisplayMode));
			_displayModeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				CinemaDisplayMode displayMode = ParseDisplayMode(_displayModeDropdown.get_SelectedItem());
				_settings.DisplayMode = displayMode;
				UpdateVisibility();
			});
			Label val3 = new Label();
			val3.set_Text("In-Game World: 3D Video displayed at in-game, press '+ Add New' to start\nOn-Screen Window: A draggable, resizable overlay window");
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(300, 10));
			val3.set_TextColor(Color.get_LightGray());
			((Control)val3).set_Parent(parent);
		}

		private void BuildWindowHelpSection(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Window Controls");
			((Control)val).set_Size(new Point(((Control)parent).get_Width() - 70, 130));
			((Control)val).set_Location(new Point(23, 140));
			((Control)val).set_Parent(parent);
			_windowHelpSection = val;
			Label val2 = new Label();
			val2.set_Text("• Drag anywhere on the video to move the window\n• Drag the corners or edges to resize\n• Hover over the video to access playback controls");
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 10));
			val2.set_TextColor(Color.get_LightGray());
			((Control)val2).set_Parent((Container)(object)_windowHelpSection);
		}

		private void BuildLocationSection(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Size(new Point(((Control)parent).get_Width() - 70, ((Control)parent).get_Height() - 280));
			((Control)val).set_Location(new Point(23, 140));
			((Control)val).set_Parent(parent);
			_locationSection = val;
			BuildCategoryMenu();
			BuildContentPanel();
			PopulateCategoryMenu();
			SelectInitialCategory();
		}

		private void BuildCategoryMenu()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			((Control)val).set_Size(new Point(240, ((Control)_locationSection).get_Height()));
			((Control)val).set_Location(new Point(0, 0));
			val.set_Title("Categories");
			((Control)val).set_Parent((Container)(object)_locationSection);
			val.set_CanScroll(true);
			_menuPanel = val;
			Menu val2 = new Menu();
			Rectangle contentRegion = ((Container)_menuPanel).get_ContentRegion();
			((Control)val2).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val2.set_MenuItemHeight(50);
			((Control)val2).set_Parent((Container)(object)_menuPanel);
			val2.set_CanSelect(true);
			_categoryMenu = val2;
			_categoryMenu.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)OnCategorySelected);
		}

		private void BuildContentPanel()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Size(new Point(((Control)_locationSection).get_Width() - 240 - 6, ((Control)_locationSection).get_Height()));
			((Control)val).set_Location(new Point(246, 0));
			val.set_ShowBorder(true);
			((Control)val).set_Parent((Container)(object)_locationSection);
			_contentContainer = val;
			Panel val2 = new Panel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(0);
			((Control)val2).set_Parent((Container)(object)_contentContainer);
			_headerSection = val2;
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Size(new Point(((Container)_contentContainer).get_ContentRegion().Width, ((Container)_contentContainer).get_ContentRegion().Height));
			((Control)val3).set_Location(new Point(0, 0));
			val3.set_ControlPadding(new Vector2(0f, 4f));
			((Panel)val3).set_CanScroll(true);
			((Control)val3).set_Parent((Container)(object)_contentContainer);
			_cardsPanel = val3;
		}

		private void PopulateCategoryMenu()
		{
			((Container)_categoryMenu).ClearChildren();
			_categoryLookup.Clear();
			_categoryMenu.AddMenuItem("My Locations", (Texture2D)null).set_Icon(CinemaModule.Instance.TextureService.GetEmblem());
			foreach (WorldLocationCategory category in _presetService.WorldLocationCategories)
			{
				_categoryMenu.AddMenuItem(category.Name, (Texture2D)null).set_Icon(category.IconTexture);
				_categoryLookup[category.Name] = category;
			}
		}

		private void SelectInitialCategory()
		{
			string initialCategory = DetermineInitialCategory();
			MenuItem menuItem = ((IEnumerable)((Container)_categoryMenu).get_Children()).OfType<MenuItem>().FirstOrDefault((MenuItem m) => m.get_Text() == initialCategory);
			if (menuItem != null)
			{
				_categoryMenu.Select(menuItem);
			}
		}

		private string DetermineInitialCategory()
		{
			string lastCategory = _settings.LastSelectedLocationCategory;
			if (string.IsNullOrEmpty(lastCategory) || (!_categoryLookup.ContainsKey(lastCategory) && !(lastCategory == "My Locations")))
			{
				return _presetService.WorldLocationCategories.FirstOrDefault()?.Name ?? "My Locations";
			}
			return lastCategory;
		}

		private void OnCategorySelected(object sender, ControlActivatedEventArgs e)
		{
			Control activatedControl = e.get_ActivatedControl();
			MenuItem menuItem = (MenuItem)(object)((activatedControl is MenuItem) ? activatedControl : null);
			if (menuItem != null)
			{
				_selectedCategoryId = menuItem.get_Text();
				_settings.LastSelectedLocationCategory = _selectedCategoryId;
				RefreshContent();
			}
		}

		private void ReselectCurrentCategory()
		{
			if (!string.IsNullOrEmpty(_selectedCategoryId))
			{
				MenuItem menuItem = ((IEnumerable)((Container)_categoryMenu).get_Children()).OfType<MenuItem>().FirstOrDefault((MenuItem m) => m.get_Text() == _selectedCategoryId);
				if (menuItem != null)
				{
					_categoryMenu.Select(menuItem);
				}
			}
		}

		private void RefreshContent()
		{
			_locationCards.Clear();
			((Container)_headerSection).ClearChildren();
			((Control)_headerSection).set_Height(0);
			((Container)_cardsPanel).ClearChildren();
			UpdateCardsPanelLayout();
			WorldLocationCategory category;
			if (_selectedCategoryId == "My Locations")
			{
				LoadMyLocationsContent();
			}
			else if (_categoryLookup.TryGetValue(_selectedCategoryId, out category))
			{
				LoadCategoryContent(category);
			}
		}

		private void LoadCategoryContent(WorldLocationCategory category)
		{
			BuildCategoryHeader(category);
			if (category.Locations.Count == 0)
			{
				ShowEmptyMessage("No locations available in this category");
				return;
			}
			foreach (WorldLocationPresetData location in category.Locations)
			{
				CreatePresetLocationCard(location);
			}
		}

		private void LoadMyLocationsContent()
		{
			BuildMyLocationsToolbar();
			List<SavedLocation> savedLocations = _settings.SavedLocations.Locations;
			if (savedLocations.Count == 0)
			{
				ShowEmptyMessage("No custom locations. Click '+ Add New' to create one.");
				return;
			}
			foreach (SavedLocation location in savedLocations)
			{
				CreateSavedLocationCard(location);
			}
		}

		private void BuildCategoryHeader(WorldLocationCategory category)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			if (!string.IsNullOrEmpty(category.Description))
			{
				Panel val = new Panel();
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Control)val).set_Parent((Container)(object)_headerSection);
				Panel headerPanel = val;
				Label val2 = new Label();
				val2.set_Text(category.Description);
				((Control)val2).set_Width(Math.Max(((Control)_contentContainer).get_Width() - 40, 100));
				val2.set_AutoSizeHeight(true);
				val2.set_WrapText(true);
				val2.set_TextColor(Color.get_LightGray());
				val2.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val2).set_Left(10);
				((Control)val2).set_Top(10);
				((Control)val2).set_Parent((Container)(object)headerPanel);
				Label descLabel = val2;
				((Control)headerPanel).set_Height(Math.Max(((Control)descLabel).get_Height() + 20, 46));
				((Control)_headerSection).set_Height(((Control)headerPanel).get_Height());
				UpdateCardsPanelLayout();
			}
		}

		private void BuildMyLocationsToolbar()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			((Control)_headerSection).set_Height(40);
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(40);
			((Control)val).set_Parent((Container)(object)_headerSection);
			Panel toolbar = val;
			Label val2 = new Label();
			val2.set_Text("Create your own screen locations");
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_Gray());
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(12);
			((Control)val2).set_Parent((Container)(object)toolbar);
			GlowButton val3 = new GlowButton();
			val3.set_Icon(CinemaModule.Instance.TextureService.GetImportIcon());
			((Control)val3).set_Size(new Point(30, 26));
			((Control)val3).set_BasicTooltipText("Import from Clipboard");
			((Control)val3).set_Left(((Control)_contentContainer).get_Width() - 145);
			((Control)val3).set_Top(7);
			((Control)val3).set_Parent((Container)(object)toolbar);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ImportLocationFromClipboard();
			});
			StandardButton val4 = new StandardButton();
			val4.set_Text("+ Add New");
			((Control)val4).set_Width(100);
			((Control)val4).set_Left(((Control)_contentContainer).get_Width() - 110);
			((Control)val4).set_Top(5);
			((Control)val4).set_Parent((Container)(object)toolbar);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenEditorForNewLocation();
			});
			UpdateCardsPanelLayout();
		}

		private void UpdateCardsPanelLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			((Control)_cardsPanel).set_Location(new Point(0, ((Control)_headerSection).get_Height()));
			((Control)_cardsPanel).set_Height(((Container)_contentContainer).get_ContentRegion().Height - ((Control)_headerSection).get_Height());
		}

		private void ShowEmptyMessage(string message)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(40);
			((Control)val).set_Parent((Container)(object)_cardsPanel);
			Panel container = val;
			Label val2 = new Label();
			val2.set_Text(message);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_Gray());
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(12);
			((Control)val2).set_Parent((Container)(object)container);
		}

		private void CreatePresetLocationCard(WorldLocationPresetData preset)
		{
			string key = "preset:" + preset.Id;
			bool isSelected = key == _selectedLocationKey;
			int mapId = preset.Position?.MapId ?? 0;
			List<ListCardButton> buttons = new List<ListCardButton>
			{
				new ListCardButton
				{
					Text = "",
					Width = 30,
					Icon = CinemaModule.Instance.TextureService.GetInfoIcon(),
					Tooltip = "View Details",
					OnClick = delegate
					{
						ShowPresetInfo(preset);
					}
				}
			};
			if (!string.IsNullOrEmpty(preset.Waypoint))
			{
				buttons.Insert(0, new ListCardButton
				{
					Text = "",
					Width = 30,
					Icon = CinemaModule.Instance.TextureService.GetWaypointIcon(),
					Tooltip = "Copy Waypoint",
					OnClick = delegate
					{
						CopyWaypointToClipboard(preset.Waypoint);
					}
				});
			}
			ListCard card = new ListCard((Container)(object)_cardsPanel, $"{preset.Name} - Map {mapId}", preset.Description ?? string.Empty, isSelected, 220, buttons);
			_locationCards[key] = card;
			((Control)card).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectPresetLocation(preset, key);
			});
			UpdateCardMapName(card, mapId);
			if (preset.AvatarTexture != null)
			{
				card.SetAvatar(preset.AvatarTexture);
			}
		}

		private void CreateSavedLocationCard(SavedLocation location)
		{
			string key = "saved:" + location.Id;
			bool isSelected = key == _selectedLocationKey;
			int mapId = location.Position?.MapId ?? 0;
			List<ListCardButton> buttons = new List<ListCardButton>
			{
				new ListCardButton
				{
					Text = "",
					Width = 30,
					Icon = CinemaModule.Instance.TextureService.GetDeleteIcon(),
					Tooltip = "Delete",
					OnClick = delegate
					{
						DeleteLocation(location);
					}
				},
				new ListCardButton
				{
					Text = "",
					Width = 30,
					Icon = CinemaModule.Instance.TextureService.GetExportIcon(),
					Tooltip = "Export to Clipboard",
					OnClick = delegate
					{
						ExportLocationToClipboard(location);
					}
				},
				new ListCardButton
				{
					Text = "Edit",
					Width = 50,
					Tooltip = "Edit Location",
					OnClick = delegate
					{
						OpenEditorForLocation(location);
					}
				}
			};
			ListCard card = new ListCard((Container)(object)_cardsPanel, string.Format("{0} - Map {1}", location.Name ?? "Unnamed", mapId), string.Empty, isSelected, 220, buttons);
			_locationCards[key] = card;
			((Control)card).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectSavedLocation(location, key);
			});
			UpdateCardMapName(card, mapId);
			AsyncTexture2D avatarTexture = CinemaModule.Instance.TextureService.GetDefaultAvatar();
			if (avatarTexture != null)
			{
				card.SetAvatar(avatarTexture);
			}
		}

		private void InitializeSelectedLocationKey()
		{
			if (!string.IsNullOrEmpty(_settings.SelectedSavedLocationId))
			{
				_selectedLocationKey = "saved:" + _settings.SelectedSavedLocationId;
			}
			else if (!string.IsNullOrEmpty(_settings.SelectedPresetLocationId))
			{
				_selectedLocationKey = "preset:" + _settings.SelectedPresetLocationId;
			}
		}

		private void SelectPresetLocation(WorldLocationPresetData preset, string key)
		{
			_selectedLocationKey = key;
			_controller.SelectPresetLocation(preset.Id, preset.Position, preset.ScreenWidth);
			UpdateCardSelection();
		}

		private void SelectSavedLocation(SavedLocation location, string key)
		{
			_selectedLocationKey = key;
			_controller.SelectSavedLocation(location.Id);
			UpdateCardSelection();
		}

		private void UpdateCardSelection()
		{
			foreach (KeyValuePair<string, ListCard> kvp in _locationCards)
			{
				kvp.Value.IsSelected = kvp.Key == _selectedLocationKey;
			}
		}

		private void CopyWaypointToClipboard(string waypoint)
		{
			if (!string.IsNullOrEmpty(waypoint))
			{
				try
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(waypoint);
					ScreenNotification.ShowNotification("Waypoint copied!", (NotificationType)0, (Texture2D)null, 4);
				}
				catch
				{
				}
			}
		}

		private LocationEditorWindow GetOrCreateEditorWindow()
		{
			if (_editorWindow == null)
			{
				_editorWindow = new LocationEditorWindow(_settings, _controller);
			}
			return _editorWindow;
		}

		private void OpenEditorForNewLocation()
		{
			GetOrCreateEditorWindow().CreateNew();
		}

		private void OpenEditorForLocation(SavedLocation location)
		{
			GetOrCreateEditorWindow().Edit(location);
		}

		private void DeleteLocation(SavedLocation location)
		{
			_settings.DeleteSavedLocation(location.Id);
		}

		private void ImportLocationFromClipboard()
		{
			try
			{
				if (Clipboard.ContainsText())
				{
					SavedLocationExport importData = JsonConvert.DeserializeObject<SavedLocationExport>(Clipboard.GetText());
					if (importData?.Position != null)
					{
						string name = (string.IsNullOrWhiteSpace(importData.Name) ? "Imported Location" : importData.Name);
						_settings.AddSavedLocation(name, importData.Position, importData.ScreenWidth);
					}
				}
			}
			catch
			{
			}
		}

		private void ExportLocationToClipboard(SavedLocation location)
		{
			ExportToClipboard(location.Name, location.Position, location.ScreenWidth);
		}

		private void ExportToClipboard(string name, WorldPosition3D position, float screenWidth)
		{
			try
			{
				Clipboard.SetText(JsonConvert.SerializeObject(new SavedLocationExport
				{
					Name = name,
					Position = position,
					ScreenWidth = screenWidth
				}, Formatting.Indented));
			}
			catch
			{
			}
		}

		private void ShowPresetInfo(WorldLocationPresetData preset)
		{
			GetOrCreatePresetInfoWindow().ShowPreset(preset);
		}

		private LocationInfoWindow GetOrCreatePresetInfoWindow()
		{
			if (_presetInfoWindow == null)
			{
				AsyncTexture2D bgTexture = CinemaModule.Instance.TextureService.GetSmallWindowBackground();
				_presetInfoWindow = new LocationInfoWindow(bgTexture);
			}
			return _presetInfoWindow;
		}

		private void UpdateVisibility()
		{
			bool isEnabled = _cinemaSettings.IsEnabled;
			if (_displayModeDropdown != null)
			{
				((Control)_displayModeDropdown).set_Enabled(isEnabled);
			}
			if (_windowHelpSection != null && _locationSection != null)
			{
				((Control)_windowHelpSection).set_Visible(isEnabled && _settings.DisplayMode == CinemaDisplayMode.OnScreen);
				((Control)_locationSection).set_Visible(isEnabled && _settings.DisplayMode == CinemaDisplayMode.InGame);
			}
		}

		private async void UpdateCardMapName(ListCard card, int mapId)
		{
			if (mapId > 0)
			{
				string mapName = await _mapService.GetMapNameAsync(mapId);
				string currentTitle = card.Title;
				int dashIndex = currentTitle.LastIndexOf(" - ");
				if (dashIndex > 0)
				{
					string namePrefix = currentTitle.Substring(0, dashIndex);
					card.Title = namePrefix + " - " + mapName;
				}
			}
		}

		private CinemaDisplayMode ParseDisplayMode(string name)
		{
			if (name == "On-Screen Window")
			{
				return CinemaDisplayMode.OnScreen;
			}
			if (name == "In-Game World")
			{
				return CinemaDisplayMode.InGame;
			}
			return CinemaDisplayMode.OnScreen;
		}

		private string GetDisplayModeName(CinemaDisplayMode mode)
		{
			return mode switch
			{
				CinemaDisplayMode.OnScreen => "On-Screen Window", 
				CinemaDisplayMode.InGame => "In-Game World", 
				_ => mode.ToString(), 
			};
		}

		private void SubscribeToEvents()
		{
			_savedLocationsChangedHandler = delegate
			{
				if (_selectedCategoryId == "My Locations")
				{
					RefreshContent();
				}
			};
			_settings.SavedLocationsChanged += _savedLocationsChangedHandler;
			_presetsLoadedHandler = delegate
			{
				PopulateCategoryMenu();
				ReselectCurrentCategory();
				RefreshContent();
			};
			_presetService.PresetsLoaded += _presetsLoadedHandler;
			_parentResizedHandler = delegate
			{
				UpdateSectionSizes();
			};
			((Control)_buildPanel).add_Resized(_parentResizedHandler);
		}

		private void UpdateSectionSizes()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			if (_buildPanel != null && _locationSection != null)
			{
				((Control)_locationSection).set_Size(new Point(((Control)_buildPanel).get_Width() - 70, ((Control)_buildPanel).get_Height() - 280));
				((Control)_menuPanel).set_Height(((Control)_locationSection).get_Height());
				((Control)_categoryMenu).set_Height(((Container)_menuPanel).get_ContentRegion().Height);
				((Control)_contentContainer).set_Size(new Point(((Control)_locationSection).get_Width() - 240 - 6, ((Control)_locationSection).get_Height()));
				((Control)_cardsPanel).set_Width(((Container)_contentContainer).get_ContentRegion().Width);
				UpdateCardsPanelLayout();
			}
		}

		protected override void Unload()
		{
			_settings.SavedLocationsChanged -= _savedLocationsChangedHandler;
			_presetService.PresetsLoaded -= _presetsLoadedHandler;
			_cinemaSettings.EnabledSetting.remove_SettingChanged(_enabledSettingChangedHandler);
			_categoryMenu.remove_ItemSelected((EventHandler<ControlActivatedEventArgs>)OnCategorySelected);
			if (_buildPanel != null)
			{
				((Control)_buildPanel).remove_Resized(_parentResizedHandler);
			}
			LocationEditorWindow editorWindow = _editorWindow;
			if (editorWindow != null)
			{
				((Control)editorWindow).Dispose();
			}
			LocationInfoWindow presetInfoWindow = _presetInfoWindow;
			if (presetInfoWindow != null)
			{
				((Control)presetInfoWindow).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
