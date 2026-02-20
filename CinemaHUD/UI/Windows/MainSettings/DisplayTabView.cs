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

		private Panel _locationSection;

		private Panel _windowHelpSection;

		private FlowPanel _locationsContainer;

		private Dictionary<string, ListCard> _locationCards = new Dictionary<string, ListCard>();

		private LocationEditorWindow _editorWindow;

		private LocationInfoWindow _presetInfoWindow;

		private EventHandler _presetsLoadedHandler;

		private EventHandler _savedLocationsChangedHandler;

		private EventHandler<ValueChangedEventArgs<bool>> _enabledSettingChangedHandler;

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
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Display Settings");
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width() - 70, 120));
			((Control)val).set_Location(new Point(23, 10));
			((Control)val).set_Parent(buildPanel);
			Panel displaySettingsPanel = val;
			BuildDisplayModeSection((Container)(object)displaySettingsPanel);
			Panel val2 = new Panel();
			val2.set_ShowBorder(true);
			val2.set_Title("Window Controls");
			((Control)val2).set_Size(new Point(((Control)buildPanel).get_Width() - 70, 130));
			((Control)val2).set_Location(new Point(23, 140));
			((Control)val2).set_Parent(buildPanel);
			_windowHelpSection = val2;
			BuildWindowHelpSection();
			Panel val3 = new Panel();
			val3.set_ShowBorder(true);
			((Control)val3).set_Size(new Point(((Control)buildPanel).get_Width() - 70, ((Control)buildPanel).get_Height() - 280));
			((Control)val3).set_Location(new Point(23, 140));
			((Control)val3).set_Parent(buildPanel);
			_locationSection = val3;
			BuildLocationSection();
			UpdateVisibility();
			_savedLocationsChangedHandler = delegate
			{
				RebuildLocationCards();
			};
			_settings.SavedLocationsChanged += _savedLocationsChangedHandler;
			_presetsLoadedHandler = delegate
			{
				RebuildLocationCards();
			};
			_presetService.PresetsLoaded += _presetsLoadedHandler;
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
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
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
			((Control)val2).set_Location(new Point(150, 13));
			((Control)val2).set_Parent(parent);
			_displayModeDropdown = val2;
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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("• Drag anywhere on the video to move the window\n• Drag the corners or edges to resize\n• Hover over the video to access playback controls");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Location(new Point(10, 10));
			val.set_TextColor(Color.get_LightGray());
			((Control)val).set_Parent((Container)(object)_windowHelpSection);
		}

		private void BuildLocationSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Size(new Point(((Container)_locationSection).get_ContentRegion().Width, 36));
			((Control)val).set_Location(new Point(0, 0));
			val.set_BackgroundTexture(global::CinemaModule.CinemaModule.Instance.TextureService.GetCardBackground());
			((Control)val).set_Parent((Container)(object)_locationSection);
			Panel headerPanel = val;
			Label val2 = new Label();
			val2.set_Text("Locations");
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(10, 8));
			((Control)val2).set_Parent((Container)(object)headerPanel);
			StandardButton val3 = new StandardButton();
			val3.set_Text("+ Add New");
			((Control)val3).set_Width(100);
			((Control)val3).set_Parent((Container)(object)headerPanel);
			StandardButton addButton = val3;
			((Control)addButton).set_Location(new Point(((Control)headerPanel).get_Width() - ((Control)addButton).get_Width() - 10, 5));
			((Control)addButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenEditorForNewLocation();
			});
			GlowButton val4 = new GlowButton();
			val4.set_Icon(global::CinemaModule.CinemaModule.Instance.TextureService.GetImportIcon());
			((Control)val4).set_Size(new Point(30, 26));
			((Control)val4).set_BasicTooltipText("Import from Clipboard");
			((Control)val4).set_Parent((Container)(object)headerPanel);
			GlowButton importButton = val4;
			((Control)importButton).set_Location(new Point(((Control)addButton).get_Location().X - ((Control)importButton).get_Width() - 5, 7));
			((Control)importButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ImportLocationFromClipboard();
			});
			FlowPanel val5 = new FlowPanel();
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Control)val5).set_Size(new Point(((Container)_locationSection).get_ContentRegion().Width, ((Container)_locationSection).get_ContentRegion().Height - 50));
			((Control)val5).set_Location(new Point(0, 46));
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
					Text = "",
					Width = 30,
					Icon = global::CinemaModule.CinemaModule.Instance.TextureService.GetInfoIcon(),
					Tooltip = "View Details",
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

		private void CreateSavedLocationListItem(SavedLocation location)
		{
			bool isSelected = _settings.SelectedSavedLocationId == location.Id;
			int mapId = location.Position?.MapId ?? 0;
			List<ListCardButton> buttons = new List<ListCardButton>
			{
				new ListCardButton
				{
					Text = "",
					Width = 30,
					Icon = global::CinemaModule.CinemaModule.Instance.TextureService.GetDeleteIcon(),
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
					Icon = global::CinemaModule.CinemaModule.Instance.TextureService.GetExportIcon(),
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
			ListCard card = new ListCard((Container)(object)_locationsContainer, string.Format("{0} - Map {1}", location.Name ?? "Unnamed", mapId), string.Empty, isSelected, 260, buttons);
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

		private void ExportLocationToClipboard(SavedLocation location)
		{
			ExportToClipboard(location.Name, location.Position, location.ScreenWidth);
		}

		private void ExportToClipboard(string name, WorldPosition3D position, float screenWidth)
		{
			try
			{
				Clipboard.SetText(JsonConvert.SerializeObject((object)new SavedLocationExport
				{
					Name = name,
					Position = position,
					ScreenWidth = screenWidth
				}, (Formatting)1));
				Logger.Debug("Exported location '" + name + "' to clipboard");
			}
			catch (Exception ex)
			{
				Logger.Debug("Failed to export location to clipboard: " + ex.Message);
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
			if (_windowHelpSection != null && _locationSection != null)
			{
				((Control)_windowHelpSection).set_Visible(isEnabled && _settings.DisplayMode == CinemaDisplayMode.OnScreen);
				((Control)_locationSection).set_Visible(isEnabled && _settings.DisplayMode == CinemaDisplayMode.InGame);
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
			_settings.SavedLocationsChanged -= _savedLocationsChangedHandler;
			_presetService.PresetsLoaded -= _presetsLoadedHandler;
			_cinemaSettings.EnabledSetting.remove_SettingChanged(_enabledSettingChangedHandler);
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
