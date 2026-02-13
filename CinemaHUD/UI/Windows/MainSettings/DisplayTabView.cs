using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using CinemaHUD.UI.Windows.Info;
using CinemaHUD.UI.Windows.SettingsSmall;
using CinemaModule;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class DisplayTabView : View
	{
		private class SavedLocationExport
		{
			public string Name { get; set; }

			public WorldPosition3D Position { get; set; }

			public float ScreenWidth { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<DisplayTabView>();

		private readonly CinemaSettings _cinemaSettings;

		private readonly CinemaUserSettings _settings;

		private readonly CinemaController _controller;

		private readonly Gw2MapService _mapService;

		private readonly PresetService _presetService;

		private Checkbox _enabledCheckbox;

		private Dropdown _displayModeDropdown;

		private FlowPanel _locationSection;

		private FlowPanel _windowHelpSection;

		private FlowPanel _locationsContainer;

		private Dictionary<string, ListCard> _locationCards = new Dictionary<string, ListCard>();

		private LocationEditorWindow _editorWindow;

		private LocationInfoWindow _presetInfoWindow;

		private EventHandler _presetsLoadedHandler;

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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_OuterControlPadding(new Vector2(55f, 0f));
			val.set_ControlPadding(new Vector2(20f, 10f));
			((Control)val).set_Parent(buildPanel);
			FlowPanel panel = val;
			BuildDisplayModeSection((Container)(object)panel);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(0f, 6f));
			((Control)val2).set_Parent((Container)(object)panel);
			_windowHelpSection = val2;
			BuildWindowHelpSection();
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			val3.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val3).set_Parent((Container)(object)panel);
			_locationSection = val3;
			BuildLocationSection();
			UpdateVisibility();
			_settings.SavedLocationsChanged += delegate
			{
				RebuildSavedLocationCards();
			};
			_presetsLoadedHandler = delegate
			{
				RebuildLocationCards();
			};
			_presetService.PresetsLoaded += _presetsLoadedHandler;
		}

		private void BuildDisplayModeSection(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Display Settings");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(190f, 0f));
			((Control)val2).set_Parent(parent);
			FlowPanel modePanel = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Width(100);
			((Control)val3).set_Height(40);
			((Control)val3).set_Parent((Container)(object)modePanel);
			Panel checkboxWrapper = val3;
			Checkbox val4 = new Checkbox();
			val4.set_Text("Enabled");
			val4.set_Checked(_cinemaSettings.IsEnabled);
			((Control)val4).set_Top(4);
			((Control)val4).set_Parent((Container)(object)checkboxWrapper);
			_enabledCheckbox = val4;
			_enabledCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_cinemaSettings.EnabledSetting.set_Value(_enabledCheckbox.get_Checked());
				UpdateVisibility();
			});
			_cinemaSettings.EnabledSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (_enabledCheckbox.get_Checked() != e.get_NewValue())
				{
					_enabledCheckbox.set_Checked(e.get_NewValue());
				}
			});
			Dropdown val5 = new Dropdown();
			((Control)val5).set_Width(160);
			((Control)val5).set_Parent((Container)(object)modePanel);
			_displayModeDropdown = val5;
			foreach (object mode in Enum.GetValues(typeof(CinemaDisplayMode)))
			{
				_displayModeDropdown.get_Items().Add(GetDisplayModeName((CinemaDisplayMode)mode));
			}
			_displayModeDropdown.set_SelectedItem(GetDisplayModeName(_settings.DisplayMode));
			_displayModeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				CinemaDisplayMode displayMode = ParseDisplayMode(_displayModeDropdown.get_SelectedItem());
				_settings.DisplayMode = displayMode;
				UpdateVisibility();
			});
		}

		private void BuildWindowHelpSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("Window Controls");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent((Container)(object)_windowHelpSection);
			Label val2 = new Label();
			val2.set_Text("• Drag anywhere on the video to move the window\n• Drag the corners or edges to resize\n• Hover over the video to access playback controls");
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_LightGray());
			((Control)val2).set_Parent((Container)(object)_windowHelpSection);
		}

		private void BuildLocationSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(30);
			((Control)val).set_Parent((Container)(object)_locationSection);
			Panel header = val;
			Label val2 = new Label();
			val2.set_Text("Locations");
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Top(4);
			((Control)val2).set_Parent((Container)(object)header);
			StandardButton val3 = new StandardButton();
			val3.set_Text("Import");
			((Control)val3).set_Width(70);
			((Control)val3).set_Left(270);
			((Control)val3).set_Parent((Container)(object)header);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ImportLocationFromClipboard();
			});
			StandardButton val4 = new StandardButton();
			val4.set_Text("+ Add New");
			((Control)val4).set_Width(100);
			((Control)val4).set_Left(350);
			((Control)val4).set_Parent((Container)(object)header);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenEditorForNewLocation();
			});
			FlowPanel val5 = new FlowPanel();
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Control)val5).set_Height(420);
			val5.set_ControlPadding(new Vector2(0f, 4f));
			((Panel)val5).set_CanScroll(true);
			((Control)val5).set_Parent((Container)(object)_locationSection);
			_locationsContainer = val5;
			RebuildLocationCards();
		}

		private void RebuildLocationCards()
		{
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel locationsContainer = _locationsContainer;
			if (locationsContainer != null)
			{
				((Container)locationsContainer).ClearChildren();
			}
			_locationCards.Clear();
			IReadOnlyList<WorldLocationPresetData> presets = _presetService.WorldLocationPresets;
			Logger.Debug($"RebuildLocationCards: Found {presets.Count} preset locations, IsLoaded={_presetService.IsLoaded}");
			foreach (WorldLocationPresetData preset in presets)
			{
				CreatePresetListItem(preset);
			}
			foreach (SavedLocation location in _settings.SavedLocations.Locations)
			{
				CreateSavedLocationListItem(location);
			}
			if (_locationCards.Count == 0)
			{
				Label val = new Label();
				val.set_Text("No locations available. Click '+ Add New' to create one.");
				val.set_AutoSizeHeight(true);
				val.set_AutoSizeWidth(true);
				val.set_TextColor(Color.get_Gray());
				((Control)val).set_Parent((Container)(object)_locationsContainer);
			}
		}

		private void CreatePresetListItem(WorldLocationPresetData preset)
		{
			bool isSelected = _settings.SelectedPresetLocationId == preset.Id && string.IsNullOrEmpty(_settings.SelectedSavedLocationId);
			int mapId = preset.Position?.MapId ?? 0;
			List<ListCardButton> buttons = new List<ListCardButton>
			{
				new ListCardButton
				{
					Text = "Info",
					Width = 50,
					OnClick = delegate
					{
						ShowPresetInfo(preset);
					}
				}
			};
			ListCard card = new ListCard((Container)(object)_locationsContainer, $"{preset.Name} - Map {mapId}", string.Empty, isSelected, 350, buttons);
			_locationCards["preset_" + preset.Id] = card;
			((Control)card).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_controller.SelectPresetLocation(preset.Id, preset.Position, preset.ScreenWidth);
				UpdateCardSelection();
			});
			UpdateCardMapName(card, mapId);
			if (preset.AvatarTexture != null)
			{
				card.SetAvatar(preset.AvatarTexture);
			}
		}

		private void RebuildSavedLocationCards()
		{
			RebuildLocationCards();
		}

		private void CreateSavedLocationListItem(SavedLocation location)
		{
			bool isSelected = _settings.SelectedSavedLocationId == location.Id;
			int mapId = location.Position?.MapId ?? 0;
			List<ListCardButton> buttons = new List<ListCardButton>
			{
				new ListCardButton
				{
					Text = "X",
					Width = 30,
					OnClick = delegate
					{
						DeleteLocation(location);
					}
				},
				new ListCardButton
				{
					Text = "Edit",
					Width = 50,
					OnClick = delegate
					{
						OpenEditorForLocation(location);
					}
				}
			};
			ListCard card = new ListCard((Container)(object)_locationsContainer, string.Format("{0} - Map {1}", location.Name ?? "Unnamed", mapId), string.Empty, isSelected, 320, buttons);
			_locationCards["saved_" + location.Id] = card;
			((Control)card).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_controller.SelectSavedLocation(location.Id);
				UpdateCardSelection();
			});
			UpdateCardMapName(card, mapId);
			AsyncTexture2D avatarTexture = global::CinemaModule.CinemaModule.Instance.TextureService.GetDefaultAvatar();
			if (avatarTexture != null)
			{
				card.SetAvatar(avatarTexture);
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
				if (!Clipboard.ContainsText())
				{
					Logger.Debug("Clipboard does not contain text for import");
					return;
				}
				SavedLocationExport importData = JsonConvert.DeserializeObject<SavedLocationExport>(Clipboard.GetText());
				if (importData?.Position == null)
				{
					Logger.Debug("Invalid location data in clipboard");
					return;
				}
				string name = (string.IsNullOrWhiteSpace(importData.Name) ? "Imported Location" : importData.Name);
				_settings.AddSavedLocation(name, importData.Position, importData.ScreenWidth);
				Logger.Debug("Imported location '" + name + "' from clipboard");
			}
			catch (Exception ex)
			{
				Logger.Debug("Failed to import location from clipboard: " + ex.Message);
			}
		}

		private void UpdateCardSelection()
		{
			foreach (KeyValuePair<string, ListCard> kvp in _locationCards)
			{
				if (kvp.Key.StartsWith("preset_"))
				{
					string presetId = kvp.Key.Substring(7);
					kvp.Value.IsSelected = presetId == _settings.SelectedPresetLocationId && string.IsNullOrEmpty(_settings.SelectedSavedLocationId);
				}
				else if (kvp.Key.StartsWith("saved_"))
				{
					string savedId = kvp.Key.Substring(6);
					kvp.Value.IsSelected = savedId == _settings.SelectedSavedLocationId;
				}
			}
		}

		private void UpdateVisibility()
		{
			bool isEnabled = _cinemaSettings.IsEnabled;
			if (_displayModeDropdown != null)
			{
				((Control)_displayModeDropdown).set_Enabled(isEnabled);
			}
			if (_windowHelpSection != null)
			{
				bool showWindowHelp = isEnabled && _settings.DisplayMode == CinemaDisplayMode.OnScreen;
				((Control)_windowHelpSection).set_Visible(showWindowHelp);
				((Control)_windowHelpSection).set_Height(showWindowHelp ? (-1) : 0);
			}
			if (_locationSection != null)
			{
				bool showLocations = isEnabled && _settings.DisplayMode == CinemaDisplayMode.InGame;
				((Control)_locationSection).set_Visible(showLocations);
				((Control)_locationSection).set_Height(showLocations ? (-1) : 0);
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

		private void ShowPresetInfo(WorldLocationPresetData preset)
		{
			GetOrCreatePresetInfoWindow().ShowPreset(preset);
		}

		private LocationInfoWindow GetOrCreatePresetInfoWindow()
		{
			if (_presetInfoWindow == null)
			{
				AsyncTexture2D bgTexture = global::CinemaModule.CinemaModule.Instance.TextureService.GetSmallWindowBackground();
				_presetInfoWindow = new LocationInfoWindow(bgTexture);
			}
			return _presetInfoWindow;
		}

		protected override void Unload()
		{
			_presetService.PresetsLoaded -= _presetsLoadedHandler;
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
