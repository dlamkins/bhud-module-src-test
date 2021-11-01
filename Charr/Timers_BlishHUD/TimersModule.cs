using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Charr.Timers_BlishHUD.Controls;
using Charr.Timers_BlishHUD.Models;
using Charr.Timers_BlishHUD.Pathing.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD
{
	[Export(typeof(Module))]
	public class TimersModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static TimersModule ModuleInstance;

		public Resources Resources;

		public AlertContainer _alertContainer;

		private AlertWindow _alertSettingsWindow;

		private List<AlertPanel> _testAlertPanels;

		private WindowTab _timersTab;

		private Panel _tabPanel;

		private List<TimerDetailsButton> _allTimerDetails;

		private List<TimerDetailsButton> _displayedTimerDetails;

		private Label _debugText;

		private List<PathableResourceManager> _pathableResourceManagers;

		private JsonSerializerSettings _jsonSettings;

		private bool _encountersLoaded;

		private bool _errorCaught;

		private HashSet<string> _encounterIds;

		public Dictionary<string, Alert> _activeAlertIds;

		public Dictionary<string, Direction> _activeDirectionIds;

		public Dictionary<string, Marker> _activeMarkerIds;

		private List<Encounter> _encounters;

		private List<Encounter> _activeEncounters;

		private List<Encounter> _invalidEncounters;

		private EventHandler<ValueEventArgs<int>> _onNewMapLoaded;

		private SettingEntry<bool> _showDebugSetting;

		public SettingEntry<bool> _debugModeSetting;

		private Dictionary<string, SettingEntry<bool>> _encounterEnableSettings;

		private SettingEntry<bool> _sortCategorySetting;

		private SettingCollection _timerSettingCollection;

		private SettingCollection _alertSettingCollection;

		private SettingEntry<bool> _lockAlertContainerSetting;

		private SettingEntry<bool> _centerAlertContainerSetting;

		public SettingEntry<bool> _hideAlertsSetting;

		public SettingEntry<bool> _hideDirectionsSetting;

		public SettingEntry<bool> _hideMarkersSetting;

		private SettingEntry<AlertSize> _alertSizeSetting;

		public SettingEntry<ControlFlowDirection> _alertDisplayOrientationSetting;

		private SettingEntry<Point> _alertContainerLocationSetting;

		public SettingEntry<float> _alertMoveDelaySetting;

		public SettingEntry<float> _alertFadeDelaySetting;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public TimersModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			_showDebugSetting = settings.DefineSetting<bool>("ShowDebugText", false, "Show Debug Text", "For creating timers. Placed in top-left corner. Displays location status.", (SettingTypeRendererDelegate)null);
			_debugModeSetting = settings.DefineSetting<bool>("DebugMode", false, "Enable Debug Mode", "All Timer triggers will ignore requireCombat setting, allowing you to test them freely.", (SettingTypeRendererDelegate)null);
			_sortCategorySetting = settings.DefineSetting<bool>("SortByCategory", false, "Sort Categories", "When enabled, categories from loaded timer files are sorted in alphanumerical order.\nOtherwise, categories are in the order they are loaded.\nThe module needs to be restarted to take effect.", (SettingTypeRendererDelegate)null);
			_timerSettingCollection = settings.AddSubCollection("EnabledTimers", false);
			_alertSettingCollection = settings.AddSubCollection("AlertSetting", false);
			_lockAlertContainerSetting = _alertSettingCollection.DefineSetting<bool>("LockAlertContainer", false, (Func<string>)null, (Func<string>)null);
			_hideAlertsSetting = _alertSettingCollection.DefineSetting<bool>("HideAlerts", false, (Func<string>)null, (Func<string>)null);
			_hideDirectionsSetting = _alertSettingCollection.DefineSetting<bool>("HideDirections", false, (Func<string>)null, (Func<string>)null);
			_hideMarkersSetting = _alertSettingCollection.DefineSetting<bool>("HideMarkers", false, (Func<string>)null, (Func<string>)null);
			_centerAlertContainerSetting = _alertSettingCollection.DefineSetting<bool>("CenterAlertContainer", true, (Func<string>)null, (Func<string>)null);
			_alertSizeSetting = _alertSettingCollection.DefineSetting<AlertSize>("AlertSize", AlertSize.Medium, (Func<string>)null, (Func<string>)null);
			_alertDisplayOrientationSetting = _alertSettingCollection.DefineSetting<ControlFlowDirection>("AlertDisplayOrientation", (ControlFlowDirection)2, (Func<string>)null, (Func<string>)null);
			_alertContainerLocationSetting = _alertSettingCollection.DefineSetting<Point>("AlertContainerLocation", Point.get_Zero(), (Func<string>)null, (Func<string>)null);
			_alertMoveDelaySetting = _alertSettingCollection.DefineSetting<float>("AlertMoveSpeed", 1f, (Func<string>)null, (Func<string>)null);
			_alertFadeDelaySetting = _alertSettingCollection.DefineSetting<float>("AlertFadeSpeed", 1f, (Func<string>)null, (Func<string>)null);
		}

		private void SettingsUpdateShowDebug(object sender = null, EventArgs e = null)
		{
			if (_showDebugSetting.get_Value())
			{
				Label debugText = _debugText;
				if (debugText != null)
				{
					((Control)debugText).Show();
				}
			}
			else
			{
				Label debugText2 = _debugText;
				if (debugText2 != null)
				{
					((Control)debugText2).Hide();
				}
			}
		}

		private void SettingsUpdateLockAlertContainer(object sender = null, EventArgs e = null)
		{
			if (_alertContainer != null)
			{
				_alertContainer.Lock = _lockAlertContainerSetting.get_Value();
			}
		}

		private void SettingsUpdateCenterAlertContainer(object sender = null, EventArgs e = null)
		{
		}

		private void SettingsUpdateHideAlerts(object sender = null, EventArgs e = null)
		{
			_testAlertPanels?.ForEach(delegate(AlertPanel panel)
			{
				panel.ShouldShow = !_hideAlertsSetting.get_Value();
			});
			_encounters?.ForEach(delegate(Encounter enc)
			{
				enc.ShowAlerts = !_hideAlertsSetting.get_Value();
			});
			if (_alertContainer == null)
			{
				return;
			}
			if (_hideAlertsSetting.get_Value())
			{
				AlertContainer alertContainer = _alertContainer;
				if (alertContainer != null)
				{
					((Control)alertContainer).Hide();
				}
			}
			else
			{
				_alertContainer.UpdateDisplay();
				AlertContainer alertContainer2 = _alertContainer;
				if (alertContainer2 != null)
				{
					((Control)alertContainer2).Show();
				}
			}
			_alertContainer.AutoShow = !_hideAlertsSetting.get_Value();
		}

		private void SettingsUpdateHideDirections(object sender = null, EventArgs e = null)
		{
			_encounters?.ForEach(delegate(Encounter enc)
			{
				enc.ShowDirections = !_hideDirectionsSetting.get_Value();
			});
		}

		private void SettingsUpdateHideMarkers(object sender = null, EventArgs e = null)
		{
			_encounters?.ForEach(delegate(Encounter enc)
			{
				enc.ShowMarkers = !_hideMarkersSetting.get_Value();
			});
		}

		private void SettingsUpdateAlertSize(object sender = null, EventArgs e = null)
		{
			switch (_alertSizeSetting.get_Value())
			{
			case AlertSize.Small:
				AlertPanel.DEFAULT_ALERTPANEL_WIDTH = 320;
				AlertPanel.DEFAULT_ALERTPANEL_HEIGHT = 64;
				break;
			case AlertSize.Medium:
				AlertPanel.DEFAULT_ALERTPANEL_WIDTH = 320;
				AlertPanel.DEFAULT_ALERTPANEL_HEIGHT = 96;
				break;
			case AlertSize.Large:
				AlertPanel.DEFAULT_ALERTPANEL_WIDTH = 320;
				AlertPanel.DEFAULT_ALERTPANEL_HEIGHT = 128;
				break;
			}
		}

		private void SettingsUpdateAlertDisplayOrientation(object sender = null, EventArgs e = null)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_alertContainer.FlowDirection = _alertDisplayOrientationSetting.get_Value();
			_alertContainer.UpdateDisplay();
		}

		private void SettingsUpdateAlertContainerLocation(object sender = null, EventArgs e = null)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected I4, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			ControlFlowDirection value = _alertDisplayOrientationSetting.get_Value();
			switch (value - 2)
			{
			case 0:
			case 1:
				((Control)_alertContainer).set_Location(_alertContainerLocationSetting.get_Value());
				break;
			case 3:
				((Control)_alertContainer).set_Location(new Point(_alertContainerLocationSetting.get_Value().X - ((Control)_alertContainer).get_Width(), _alertContainerLocationSetting.get_Value().Y));
				break;
			case 5:
				((Control)_alertContainer).set_Location(new Point(_alertContainerLocationSetting.get_Value().X, _alertContainerLocationSetting.get_Value().Y - ((Control)_alertContainer).get_Height()));
				break;
			case 2:
			case 4:
				break;
			}
		}

		private void SettingsUpdateAlertMoveDelay(object sender = null, EventArgs e = null)
		{
		}

		private void SettingsUpdateAlertFadeDelay(object sender = null, EventArgs e = null)
		{
		}

		protected override void Initialize()
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Expected O, but got Unknown
			Resources = new Resources();
			_pathableResourceManagers = new List<PathableResourceManager>();
			_encounterEnableSettings = new Dictionary<string, SettingEntry<bool>>();
			_encounterIds = new HashSet<string>();
			_activeAlertIds = new Dictionary<string, Alert>();
			_activeDirectionIds = new Dictionary<string, Direction>();
			_activeMarkerIds = new Dictionary<string, Marker>();
			_encounters = new List<Encounter>();
			_activeEncounters = new List<Encounter>();
			_invalidEncounters = new List<Encounter>();
			_allTimerDetails = new List<TimerDetailsButton>();
			_testAlertPanels = new List<AlertPanel>();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Location(new Point(10, 38));
			val.set_TextColor(Color.get_White());
			val.set_Font(Resources.Font);
			val.set_Text("DEBUG");
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_StrokeText(true);
			val.set_ShowShadow(true);
			val.set_AutoSizeWidth(true);
			_debugText = val;
			SettingsUpdateShowDebug();
			_showDebugSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateShowDebug);
			_lockAlertContainerSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateLockAlertContainer);
			_centerAlertContainerSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateCenterAlertContainer);
			_hideAlertsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideAlerts);
			_hideDirectionsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideDirections);
			_hideMarkersSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideMarkers);
			_alertSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<AlertSize>>)SettingsUpdateAlertSize);
			_alertDisplayOrientationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<ControlFlowDirection>>)SettingsUpdateAlertDisplayOrientation);
			_alertContainerLocationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)SettingsUpdateAlertContainerLocation);
			_alertMoveDelaySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingsUpdateAlertMoveDelay);
			_alertFadeDelaySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingsUpdateAlertFadeDelay);
		}

		private void ResetActivatedEncounters()
		{
			_activeEncounters.Clear();
			foreach (Encounter enc in _encounters)
			{
				if (enc.Map == GameService.Gw2Mumble.get_CurrentMap().get_Id() && enc.Enabled)
				{
					if (!enc.Activated)
					{
						enc.Activated = true;
					}
					_activeEncounters.Add(enc);
				}
				else
				{
					enc.Activated = false;
				}
			}
		}

		private void readJson(Stream fileStream, PathableResourceManager pathableResourceManager)
		{
			string jsonContent;
			using (StreamReader jsonReader = new StreamReader(fileStream))
			{
				jsonContent = jsonReader.ReadToEnd();
			}
			Encounter enc = null;
			try
			{
				enc = JsonConvert.DeserializeObject<Encounter>(jsonContent, _jsonSettings);
				enc.Initialize(pathableResourceManager);
				if (!_encounterIds.Contains(enc.Id))
				{
					_encounters.Add(enc);
					_encounterIds.Add(enc.Id);
				}
			}
			catch (TimerReadException ex2)
			{
				enc.Description = ex2.Message;
				_invalidEncounters.Add(enc);
				_errorCaught = true;
				Logger.Error(enc.Name + " Timer parsing failure: " + ex2.Message);
			}
			catch (Exception ex)
			{
				enc?.Dispose();
				Encounter encounter = new Encounter();
				encounter.Name = ((FileStream)fileStream).Name.Split('\\').Last();
				encounter.Description = "File Path: " + ((FileStream)fileStream).Name + "\n\nInvalid JSON format: " + ex.Message;
				enc = encounter;
				_invalidEncounters.Add(enc);
				_errorCaught = true;
				Logger.Error("File Path: " + ((FileStream)fileStream).Name + "\n\nInvalid JSON format: " + ex.Message);
			}
		}

		protected override async Task LoadAsync()
		{
			string timerDirectory = DirectoriesManager.GetFullDirectoryPath("timers");
			DirectoryReader directoryReader = new DirectoryReader(timerDirectory);
			PathableResourceManager _directResourceManager = new PathableResourceManager((IDataReader)(object)directoryReader);
			_pathableResourceManagers.Add(_directResourceManager);
			TimersModule timersModule = this;
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_NullValueHandling((NullValueHandling)1);
			timersModule._jsonSettings = val;
			directoryReader.LoadOnFileType((Action<Stream, IDataReader>)delegate(Stream fileStream, IDataReader dataReader)
			{
				readJson(fileStream, _directResourceManager);
			}, ".bhtimer", (IProgress<string>)null);
			List<string> list = new List<string>();
			list.AddRange(Directory.GetFiles(timerDirectory, "*.zip", SearchOption.AllDirectories));
			foreach (string item in list)
			{
				ZipArchiveReader zipDataReader = new ZipArchiveReader(item, "");
				PathableResourceManager zipResourceManager = new PathableResourceManager((IDataReader)(object)zipDataReader);
				_pathableResourceManagers.Add(zipResourceManager);
				zipDataReader.LoadOnFileType((Action<Stream, IDataReader>)delegate(Stream fileStream, IDataReader dataReader)
				{
					readJson(fileStream, zipResourceManager);
				}, ".bhtimer", (IProgress<string>)null);
			}
			_encountersLoaded = true;
			_tabPanel = BuildSettingsPanel(((Container)GameService.Overlay.get_BlishHudWindow()).get_ContentRegion());
			_onNewMapLoaded = delegate
			{
				ResetActivatedEncounters();
			};
			ResetActivatedEncounters();
			SettingsUpdateHideAlerts();
			SettingsUpdateHideDirections();
			SettingsUpdateHideMarkers();
		}

		private Panel BuildSettingsPanel(Rectangle panelBounds)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected O, but got Unknown
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Expected O, but got Unknown
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Expected O, but got Unknown
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Expected O, but got Unknown
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Expected O, but got Unknown
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Expected O, but got Unknown
			//IL_0616: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0627: Unknown result type (might be due to invalid IL or missing references)
			//IL_0632: Unknown result type (might be due to invalid IL or missing references)
			//IL_063d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0644: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0679: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Expected O, but got Unknown
			//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0719: Unknown result type (might be due to invalid IL or missing references)
			//IL_0728: Expected O, but got Unknown
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_075a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0766: Unknown result type (might be due to invalid IL or missing references)
			//IL_0771: Unknown result type (might be due to invalid IL or missing references)
			//IL_0778: Unknown result type (might be due to invalid IL or missing references)
			//IL_077f: Unknown result type (might be due to invalid IL or missing references)
			//IL_078e: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Expected O, but got Unknown
			//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d7: Expected O, but got Unknown
			//IL_0852: Unknown result type (might be due to invalid IL or missing references)
			//IL_0857: Unknown result type (might be due to invalid IL or missing references)
			//IL_0863: Unknown result type (might be due to invalid IL or missing references)
			//IL_086e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0875: Unknown result type (might be due to invalid IL or missing references)
			//IL_087c: Unknown result type (might be due to invalid IL or missing references)
			//IL_088b: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b9: Expected O, but got Unknown
			//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f9: Expected O, but got Unknown
			//IL_0953: Unknown result type (might be due to invalid IL or missing references)
			//IL_0958: Unknown result type (might be due to invalid IL or missing references)
			//IL_095a: Unknown result type (might be due to invalid IL or missing references)
			//IL_095d: Unknown result type (might be due to invalid IL or missing references)
			//IL_097b: Expected I4, but got Unknown
			//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab5: Expected O, but got Unknown
			//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0f: Expected O, but got Unknown
			//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b81: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be3: Expected O, but got Unknown
			//IL_0be3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1a: Expected O, but got Unknown
			//IL_0c3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb1: Expected O, but got Unknown
			//IL_0cb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cdd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d28: Expected O, but got Unknown
			//IL_0d29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d84: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0db9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dce: Expected O, but got Unknown
			//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e31: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e6e: Expected O, but got Unknown
			//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ec0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ee5: Expected O, but got Unknown
			//IL_0ee6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f8b: Expected O, but got Unknown
			//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd7: Expected O, but got Unknown
			//IL_100c: Unknown result type (might be due to invalid IL or missing references)
			//IL_101f: Unknown result type (might be due to invalid IL or missing references)
			//IL_108f: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_10eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1101: Unknown result type (might be due to invalid IL or missing references)
			//IL_1118: Unknown result type (might be due to invalid IL or missing references)
			//IL_1121: Unknown result type (might be due to invalid IL or missing references)
			//IL_1126: Unknown result type (might be due to invalid IL or missing references)
			//IL_112e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1144: Unknown result type (might be due to invalid IL or missing references)
			//IL_114b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1158: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_12eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1307: Unknown result type (might be due to invalid IL or missing references)
			//IL_1317: Unknown result type (might be due to invalid IL or missing references)
			//IL_131c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1332: Unknown result type (might be due to invalid IL or missing references)
			//IL_1348: Unknown result type (might be due to invalid IL or missing references)
			//IL_1353: Unknown result type (might be due to invalid IL or missing references)
			//IL_135a: Unknown result type (might be due to invalid IL or missing references)
			//IL_136c: Unknown result type (might be due to invalid IL or missing references)
			//IL_137e: Expected O, but got Unknown
			//IL_13c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_13cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_13d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_13d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_13e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_13e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_13fb: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Size(((Rectangle)(ref panelBounds)).get_Size());
			Panel mainPanel = val;
			AlertContainer alertContainer = new AlertContainer();
			((Control)alertContainer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			alertContainer.ControlPadding = new Vector2(10f, 10f);
			alertContainer.PadLeftBeforeControl = true;
			alertContainer.PadTopBeforeControl = true;
			((Control)alertContainer).set_BackgroundColor(new Color(Color.get_Black(), 0.3f));
			alertContainer.FlowDirection = _alertDisplayOrientationSetting.get_Value();
			alertContainer.Lock = _lockAlertContainerSetting.get_Value();
			((Control)alertContainer).set_Location(_alertContainerLocationSetting.get_Value());
			((Control)alertContainer).set_Visible(!_hideAlertsSetting.get_Value());
			_alertContainer = alertContainer;
			SettingsUpdateAlertSize();
			SettingsUpdateAlertContainerLocation();
			AlertContainer alertContainer2 = _alertContainer;
			alertContainer2.ContainerDragged = (EventHandler<EventArgs>)Delegate.Combine(alertContainer2.ContainerDragged, (EventHandler<EventArgs>)delegate
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Expected I4, but got Unknown
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				ControlFlowDirection value2 = _alertDisplayOrientationSetting.get_Value();
				switch (value2 - 2)
				{
				case 0:
				case 1:
					_alertContainerLocationSetting.set_Value(((Control)_alertContainer).get_Location());
					break;
				case 3:
					_alertContainerLocationSetting.set_Value(new Point(((Control)_alertContainer).get_Right(), ((Control)_alertContainer).get_Location().Y));
					break;
				case 5:
					_alertContainerLocationSetting.set_Value(new Point(((Control)_alertContainer).get_Location().X, ((Control)_alertContainer).get_Bottom()));
					break;
				case 2:
				case 4:
					break;
				}
			});
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)mainPanel);
			((Control)val2).set_Location(new Point(((DesignStandard)(ref Dropdown.Standard)).get_ControlOffset().X, ((DesignStandard)(ref Dropdown.Standard)).get_ControlOffset().Y));
			((TextInputBase)val2).set_PlaceholderText("Search");
			TextBox searchBox = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)mainPanel);
			((Control)val3).set_Location(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_PanelOffset().X, ((Control)searchBox).get_Bottom() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			((Control)val3).set_Size(((DesignStandard)(ref Panel.MenuStandard)).get_Size() - new Point(0, ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			val3.set_Title("Timer Categories");
			val3.set_CanScroll(true);
			val3.set_ShowBorder(true);
			Panel menuSection = val3;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)mainPanel);
			((Control)val4).set_Location(new Point(((Control)menuSection).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			val4.set_FlowDirection((ControlFlowDirection)0);
			val4.set_ControlPadding(new Vector2(8f, 8f));
			((Panel)val4).set_CanScroll(true);
			((Panel)val4).set_ShowBorder(true);
			FlowPanel timerPanel = val4;
			((Control)searchBox).set_Width(((Control)menuSection).get_Width());
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => ((DetailsButton)db).get_Text().ToLower().Contains(((TextInputBase)searchBox).get_Text().ToLower())));
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)mainPanel);
			val5.set_Text("Alert Settings");
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)mainPanel);
			val6.set_Text("Enable All");
			StandardButton enableAllButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)mainPanel);
			val7.set_Text("Disable All");
			StandardButton disableAllButton = val7;
			((Control)timerPanel).set_Size(new Point(((Control)mainPanel).get_Right() - ((Control)menuSection).get_Right() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)mainPanel).get_Height() - ((Control)enableAllButton).get_Height() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y * 2));
			((Control)val5).set_Location(new Point(((Control)menuSection).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			((Control)enableAllButton).set_Location(new Point(((Control)timerPanel).get_Right() - ((Control)enableAllButton).get_Width() - ((Control)disableAllButton).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X * 2, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			((Control)disableAllButton).set_Location(new Point(((Control)enableAllButton).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			AlertWindow alertWindow = new AlertWindow();
			((Control)alertWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			alertWindow.Title = "Alert Settings";
			((Control)alertWindow).set_Width(460);
			((Control)alertWindow).set_Height(500);
			alertWindow.WindowBackground = Resources.AlertSettingsBackground;
			alertWindow.Emblem = Resources.TextureTimerEmblem;
			_alertSettingsWindow = alertWindow;
			((Control)_alertSettingsWindow).Hide();
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)_alertSettingsWindow).get_Visible())
				{
					((Control)_alertSettingsWindow).Hide();
				}
				else
				{
					((Control)_alertSettingsWindow).Show();
					((Control)_alertSettingsWindow).set_Location(new Point(GameService.Input.get_Mouse().get_Position().X + 10, GameService.Input.get_Mouse().get_Position().Y - ((Control)_alertSettingsWindow).get_Height() / 4));
				}
			});
			Checkbox val8 = new Checkbox();
			((Control)val8).set_Parent((Container)(object)_alertSettingsWindow);
			val8.set_Text("Lock Alerts Container");
			((Control)val8).set_BasicTooltipText("When enabled, the alerts container will be locked and cannot be moved.");
			((Control)val8).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, _alertSettingsWindow.ValidChildRegion().Y - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox lockAlertsWindowCB = val8;
			lockAlertsWindowCB.set_Checked(_lockAlertContainerSetting.get_Value());
			lockAlertsWindowCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_lockAlertContainerSetting.set_Value(lockAlertsWindowCB.get_Checked());
			});
			Checkbox val9 = new Checkbox();
			((Control)val9).set_Parent((Container)(object)_alertSettingsWindow);
			val9.set_Text("Center Alerts Container");
			((Control)val9).set_BasicTooltipText("When enabled, the location of the alerts container will always be set to the center of the screen.");
			((Control)val9).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)lockAlertsWindowCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox centerAlertsWindowCB = val9;
			centerAlertsWindowCB.set_Checked(_centerAlertContainerSetting.get_Value());
			centerAlertsWindowCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_centerAlertContainerSetting.set_Value(centerAlertsWindowCB.get_Checked());
			});
			Checkbox val10 = new Checkbox();
			((Control)val10).set_Parent((Container)(object)_alertSettingsWindow);
			val10.set_Text("Hide Alerts");
			((Control)val10).set_BasicTooltipText("When enabled, alerts on the screen will be hidden.");
			((Control)val10).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)centerAlertsWindowCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideAlertsCB = val10;
			hideAlertsCB.set_Checked(_hideAlertsSetting.get_Value());
			hideAlertsCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideAlertsSetting.set_Value(hideAlertsCB.get_Checked());
			});
			Checkbox val11 = new Checkbox();
			((Control)val11).set_Parent((Container)(object)_alertSettingsWindow);
			val11.set_Text("Hide Directions");
			((Control)val11).set_BasicTooltipText("When enabled, directions on the screen will be hidden.");
			((Control)val11).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideAlertsCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideDirectionsCB = val11;
			hideDirectionsCB.set_Checked(_hideDirectionsSetting.get_Value());
			hideDirectionsCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideDirectionsSetting.set_Value(hideDirectionsCB.get_Checked());
			});
			Checkbox val12 = new Checkbox();
			((Control)val12).set_Parent((Container)(object)_alertSettingsWindow);
			val12.set_Text("Hide Markers");
			((Control)val12).set_BasicTooltipText("When enabled, markers on the screen will be hidden.");
			((Control)val12).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideDirectionsCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideMarkersCB = val12;
			hideMarkersCB.set_Checked(_hideMarkersSetting.get_Value());
			hideMarkersCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideMarkersSetting.set_Value(hideMarkersCB.get_Checked());
			});
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)_alertSettingsWindow);
			val13.set_Text("Alert Size");
			val13.set_AutoSizeWidth(true);
			((Control)val13).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideMarkersCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertSizeLabel = val13;
			Dropdown val14 = new Dropdown();
			((Control)val14).set_Parent((Container)(object)_alertSettingsWindow);
			Dropdown alertSizeDropdown = val14;
			alertSizeDropdown.get_Items().Add("Small");
			alertSizeDropdown.get_Items().Add("Medium");
			alertSizeDropdown.get_Items().Add("Large");
			alertSizeDropdown.set_SelectedItem(_alertSizeSetting.get_Value().ToString());
			alertSizeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_alertSizeSetting.set_Value((AlertSize)Enum.Parse(typeof(AlertSize), alertSizeDropdown.get_SelectedItem(), ignoreCase: true));
			});
			Label val15 = new Label();
			((Control)val15).set_Parent((Container)(object)_alertSettingsWindow);
			val15.set_Text("Alert Display Orientation");
			val15.set_AutoSizeWidth(true);
			((Control)val15).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertSizeLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertDisplayOrientationLabel = val15;
			Dropdown val16 = new Dropdown();
			((Control)val16).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val16).set_Location(new Point(((Control)alertDisplayOrientationLabel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationLabel).get_Top()));
			Dropdown alertDisplayOrientationDropdown = val16;
			alertDisplayOrientationDropdown.get_Items().Add("Left to Right");
			alertDisplayOrientationDropdown.get_Items().Add("Right to Left");
			alertDisplayOrientationDropdown.get_Items().Add("Top to Bottom");
			alertDisplayOrientationDropdown.get_Items().Add("Bottom to Top");
			ControlFlowDirection value = _alertDisplayOrientationSetting.get_Value();
			switch (value - 2)
			{
			case 0:
				alertDisplayOrientationDropdown.set_SelectedItem("Left to Right");
				break;
			case 3:
				alertDisplayOrientationDropdown.set_SelectedItem("Right to Left");
				break;
			case 1:
				alertDisplayOrientationDropdown.set_SelectedItem("Top to Bottom");
				break;
			case 5:
				alertDisplayOrientationDropdown.set_SelectedItem("Bottom to Top");
				break;
			}
			alertDisplayOrientationDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				switch (alertDisplayOrientationDropdown.get_SelectedItem())
				{
				case "Left to Right":
					_alertDisplayOrientationSetting.set_Value((ControlFlowDirection)2);
					break;
				case "Right to Left":
					_alertDisplayOrientationSetting.set_Value((ControlFlowDirection)5);
					break;
				case "Top to Bottom":
					_alertDisplayOrientationSetting.set_Value((ControlFlowDirection)3);
					break;
				case "Bottom to Top":
					_alertDisplayOrientationSetting.set_Value((ControlFlowDirection)7);
					break;
				}
			});
			((Control)alertSizeDropdown).set_Location(new Point(((Control)alertDisplayOrientationDropdown).get_Left(), ((Control)alertSizeLabel).get_Top()));
			Label val17 = new Label();
			((Control)val17).set_Parent((Container)(object)_alertSettingsWindow);
			val17.set_Text("Alert Preview");
			val17.set_AutoSizeWidth(true);
			((Control)val17).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton val18 = new StandardButton();
			((Control)val18).set_Parent((Container)(object)_alertSettingsWindow);
			val18.set_Text("Add Test Alert");
			((Control)val18).set_Location(new Point(((Control)alertDisplayOrientationDropdown).get_Left(), ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton addTestAlertButton = val18;
			((Control)addTestAlertButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				List<AlertPanel> testAlertPanels = _testAlertPanels;
				AlertPanel alertPanel = new AlertPanel();
				((Control)alertPanel).set_Parent((Container)(object)_alertContainer);
				((FlowPanel)alertPanel).set_ControlPadding(new Vector2(10f, 10f));
				((FlowPanel)alertPanel).set_PadLeftBeforeControl(true);
				((FlowPanel)alertPanel).set_PadTopBeforeControl(true);
				alertPanel.Text = "Test Alert " + (_testAlertPanels.Count + 1);
				alertPanel.TextColor = Color.get_White();
				alertPanel.Icon = AsyncTexture2D.op_Implicit(Texture2DExtension.Duplicate(AsyncTexture2D.op_Implicit(Resources.GetIcon("raid"))));
				alertPanel.MaxFill = 0f;
				alertPanel.CurrentFill = 0f;
				alertPanel.FillColor = Color.get_Red();
				alertPanel.ShouldShow = !_hideAlertsSetting.get_Value();
				testAlertPanels.Add(alertPanel);
				_alertContainer.UpdateDisplay();
			});
			StandardButton val19 = new StandardButton();
			((Control)val19).set_Parent((Container)(object)_alertSettingsWindow);
			val19.set_Text("Clear Test Alerts");
			((Control)val19).set_Location(new Point(((Control)addTestAlertButton).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)addTestAlertButton).get_Top()));
			StandardButton clearTestAlertsButton = val19;
			((Control)clearTestAlertsButton).set_Width((int)((double)((Control)clearTestAlertsButton).get_Width() * 1.15));
			((Control)clearTestAlertsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_testAlertPanels.ForEach(delegate(AlertPanel panel)
				{
					panel.Dispose();
				});
				_testAlertPanels.Clear();
				_alertContainer.UpdateDisplay();
			});
			((Control)alertSizeDropdown).set_Width(((Control)addTestAlertButton).get_Width() + ((Control)clearTestAlertsButton).get_Width() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			((Control)alertDisplayOrientationDropdown).set_Width(((Control)alertSizeDropdown).get_Width());
			Label val20 = new Label();
			((Control)val20).set_Parent((Container)(object)_alertSettingsWindow);
			val20.set_Text("Alert Container Position");
			val20.set_AutoSizeWidth(true);
			((Control)val20).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)clearTestAlertsButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertContainerPositionLabel = val20;
			StandardButton val21 = new StandardButton();
			((Control)val21).set_Parent((Container)(object)_alertSettingsWindow);
			val21.set_Text("Reset Position");
			((Control)val21).set_Location(new Point(((Control)addTestAlertButton).get_Left(), ((Control)alertContainerPositionLabel).get_Top()));
			StandardButton resetAlertContainerPositionButton = val21;
			((Control)resetAlertContainerPositionButton).set_Width(((Control)alertSizeDropdown).get_Width());
			((Control)resetAlertContainerPositionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				_alertContainerLocationSetting.set_Value(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - ((Control)_alertContainer).get_Width() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 - ((Control)_alertContainer).get_Height() / 2));
			});
			Label val22 = new Label();
			((Control)val22).set_Parent((Container)(object)_alertSettingsWindow);
			val22.set_Text("Alert Move Delay");
			((Control)val22).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val22.set_AutoSizeWidth(true);
			((Control)val22).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)resetAlertContainerPositionButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertMoveDelayLabel = val22;
			TextBox val23 = new TextBox();
			((Control)val23).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val23).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			((Control)val23).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val23).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val23).set_Height(((Control)alertMoveDelayLabel).get_Height());
			((TextInputBase)val23).set_Text($"{_alertMoveDelaySetting.get_Value():0.00}");
			TextBox alertMoveDelayTextBox = val23;
			TrackBar val24 = new TrackBar();
			((Control)val24).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val24).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val24.set_MinValue(0f);
			val24.set_MaxValue(3f);
			val24.set_Value(_alertMoveDelaySetting.get_Value());
			val24.set_SmallStep(true);
			((Control)val24).set_Location(new Point(((Control)alertMoveDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val24).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertMoveDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertMoveDelaySlider = val24;
			((TextInputBase)alertMoveDelayTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				int cursorIndex2 = ((TextInputBase)alertMoveDelayTextBox).get_CursorIndex();
				if (float.TryParse(((TextInputBase)alertMoveDelayTextBox).get_Text(), out var result2) && result2 >= alertMoveDelaySlider.get_MinValue() && result2 <= alertMoveDelaySlider.get_MaxValue())
				{
					result2 = (float)Math.Round(result2, 2);
					_alertMoveDelaySetting.set_Value(result2);
					alertMoveDelaySlider.set_Value(result2);
					((TextInputBase)alertMoveDelayTextBox).set_Text($"{result2:0.00}");
				}
				else
				{
					((TextInputBase)alertMoveDelayTextBox).set_Text($"{_alertMoveDelaySetting.get_Value():0.00}");
				}
				((TextInputBase)alertMoveDelayTextBox).set_CursorIndex(cursorIndex2);
			});
			alertMoveDelaySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num2 = (float)Math.Round(alertMoveDelaySlider.get_Value(), 2);
				_alertMoveDelaySetting.set_Value(num2);
				((TextInputBase)alertMoveDelayTextBox).set_Text($"{num2:0.00}");
			});
			Label val25 = new Label();
			((Control)val25).set_Parent((Container)(object)_alertSettingsWindow);
			val25.set_Text("Alert Fade In/Out Delay");
			((Control)val25).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val25.set_AutoSizeWidth(true);
			((Control)val25).set_Location(new Point(_alertSettingsWindow.ValidChildRegion().X + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertFadeDelayLabel = val25;
			TextBox val26 = new TextBox();
			((Control)val26).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val26).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			((Control)val26).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val26).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val26).set_Height(((Control)alertFadeDelayLabel).get_Height());
			((TextInputBase)val26).set_Text($"{_alertFadeDelaySetting.get_Value():0.00}");
			TextBox alertFadeDelayTextBox = val26;
			TrackBar val27 = new TrackBar();
			((Control)val27).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val27).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val27.set_MinValue(0f);
			val27.set_MaxValue(3f);
			val27.set_Value(_alertFadeDelaySetting.get_Value());
			val27.set_SmallStep(true);
			((Control)val27).set_Location(new Point(((Control)alertFadeDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val27).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertFadeDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertFadeDelaySlider = val27;
			((TextInputBase)alertFadeDelayTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				int cursorIndex = ((TextInputBase)alertFadeDelayTextBox).get_CursorIndex();
				if (float.TryParse(((TextInputBase)alertFadeDelayTextBox).get_Text(), out var result) && result >= alertFadeDelaySlider.get_MinValue() && result <= alertFadeDelaySlider.get_MaxValue())
				{
					result = (float)Math.Round(result, 2);
					_alertFadeDelaySetting.set_Value(result);
					alertFadeDelaySlider.set_Value(result);
					((TextInputBase)alertFadeDelayTextBox).set_Text($"{result:0.00}");
				}
				else
				{
					((TextInputBase)alertFadeDelayTextBox).set_Text($"{_alertFadeDelaySetting.get_Value():0.00}");
				}
				((TextInputBase)alertFadeDelayTextBox).set_CursorIndex(cursorIndex);
			});
			alertFadeDelaySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num = (float)Math.Round(alertFadeDelaySlider.get_Value(), 2);
				_alertFadeDelaySetting.set_Value(num);
				((TextInputBase)alertFadeDelayTextBox).set_Text($"{num:0.00}");
			});
			StandardButton val28 = new StandardButton();
			((Control)val28).set_Parent((Container)(object)_alertSettingsWindow);
			val28.set_Text("Close");
			StandardButton closeAlertSettingsButton = val28;
			((Control)closeAlertSettingsButton).set_Location(new Point((((Control)_alertSettingsWindow).get_Left() + ((Control)_alertSettingsWindow).get_Right()) / 2 - ((Control)closeAlertSettingsButton).get_Width() / 2, ((Control)_alertSettingsWindow).get_Bottom() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y - ((Control)closeAlertSettingsButton).get_Height()));
			((Control)closeAlertSettingsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_alertSettingsWindow).Hide();
			});
			foreach (Encounter enc2 in _invalidEncounters)
			{
				TimerDetailsButton timerDetailsButton = new TimerDetailsButton();
				((Control)timerDetailsButton).set_Parent((Container)(object)timerPanel);
				timerDetailsButton.Encounter = enc2;
				((DetailsButton)timerDetailsButton).set_Text(enc2.Name + "\nLoading error\nHover for details");
				((DetailsButton)timerDetailsButton).set_IconSize((DetailsIconSize)0);
				((Control)timerDetailsButton).set_BackgroundColor(Color.get_DarkRed());
				((DetailsButton)timerDetailsButton).set_ShowVignette(false);
				((DetailsButton)timerDetailsButton).set_HighlightType((DetailsHighlightType)2);
				((DetailsButton)timerDetailsButton).set_ShowToggleButton(true);
				((DetailsButton)timerDetailsButton).set_Icon(enc2.Icon ?? AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel()));
				((Control)timerDetailsButton).set_BasicTooltipText(enc2.Description);
				TimerDetailsButton entry = timerDetailsButton;
				if (!string.IsNullOrEmpty(enc2.Author))
				{
					GlowButton val29 = new GlowButton();
					val29.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureDescription));
					((Control)val29).set_BasicTooltipText("By: " + enc2.Author);
					((Control)val29).set_Parent((Container)(object)entry);
				}
				GlowButton val30 = new GlowButton();
				((Control)val30).set_Parent((Container)(object)entry);
				val30.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureX));
				val30.set_ToggleGlow(true);
				((Control)val30).set_BasicTooltipText(enc2.Description);
				((Control)val30).set_Enabled(false);
				_allTimerDetails.Add(entry);
			}
			foreach (Encounter enc3 in _encounters)
			{
				SettingEntry<bool> setting = _timerSettingCollection.DefineSetting<bool>("TimerEnable:" + enc3.Id, enc3.Enabled, (Func<string>)null, (Func<string>)null);
				enc3.Enabled = setting.get_Value();
				_encounterEnableSettings.Add("TimerEnable:" + enc3.Id, setting);
				TimerDetailsButton timerDetailsButton2 = new TimerDetailsButton();
				((Control)timerDetailsButton2).set_Parent((Container)(object)timerPanel);
				timerDetailsButton2.Encounter = enc3;
				((DetailsButton)timerDetailsButton2).set_Text(enc3.Name);
				((DetailsButton)timerDetailsButton2).set_IconSize((DetailsIconSize)0);
				((DetailsButton)timerDetailsButton2).set_ShowVignette(false);
				((DetailsButton)timerDetailsButton2).set_HighlightType((DetailsHighlightType)2);
				((DetailsButton)timerDetailsButton2).set_ShowToggleButton(true);
				((DetailsButton)timerDetailsButton2).set_ToggleState(enc3.Enabled);
				((DetailsButton)timerDetailsButton2).set_Icon(enc3.Icon);
				((Control)timerDetailsButton2).set_BasicTooltipText(enc3.Description);
				TimerDetailsButton entry2 = timerDetailsButton2;
				if (!string.IsNullOrEmpty(enc3.Author))
				{
					GlowButton val31 = new GlowButton();
					val31.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureDescription));
					((Control)val31).set_BasicTooltipText("By: " + enc3.Author);
					((Control)val31).set_Parent((Container)(object)entry2);
				}
				GlowButton val32 = new GlowButton();
				val32.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureEye));
				val32.set_ActiveIcon(AsyncTexture2D.op_Implicit(Resources.TextureEyeActive));
				((Control)val32).set_BasicTooltipText("Click to toggle timer");
				val32.set_ToggleGlow(true);
				val32.set_Checked(enc3.Enabled);
				((Control)val32).set_Parent((Container)(object)entry2);
				GlowButton toggleButton = val32;
				((Control)toggleButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					enc3.Enabled = toggleButton.get_Checked();
					setting.set_Value(toggleButton.get_Checked());
					((DetailsButton)entry2).set_ToggleState(toggleButton.get_Checked());
					ResetActivatedEncounters();
				});
				_allTimerDetails.Add(entry2);
			}
			Menu val33 = new Menu();
			Rectangle contentRegion = ((Container)menuSection).get_ContentRegion();
			((Control)val33).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val33.set_MenuItemHeight(40);
			((Control)val33).set_Parent((Container)(object)menuSection);
			val33.set_CanSelect(true);
			Menu timerCategories = val33;
			MenuItem obj = timerCategories.AddMenuItem("All Timers", (Texture2D)null);
			obj.Select();
			_displayedTimerDetails = _allTimerDetails.Where((TimerDetailsButton db) => true).ToList();
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => true));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetailsButton db) => true).ToList();
			});
			MenuItem enabledTimers = timerCategories.AddMenuItem("Enabled Timers", (Texture2D)null);
			((Control)enabledTimers).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => db.Encounter.Enabled));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetailsButton db) => db.Encounter.Enabled).ToList();
			});
			((Control)timerCategories.AddMenuItem("Current Map", (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => db.Encounter.Map == GameService.Gw2Mumble.get_CurrentMap().get_Id()));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetailsButton db) => db.Encounter.Map == GameService.Gw2Mumble.get_CurrentMap().get_Id()).ToList();
			});
			((Control)timerCategories.AddMenuItem("Invalid Timers", (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => db.Encounter.Invalid));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetailsButton db) => db.Encounter.Invalid).ToList();
			});
			List<IGrouping<string, Encounter>> categories = (from enc in _encounters
				group enc by enc.Category).ToList();
			if (_sortCategorySetting.get_Value())
			{
				categories.Sort((IGrouping<string, Encounter> cat1, IGrouping<string, Encounter> cat2) => cat1.Key.CompareTo(cat2.Key));
			}
			foreach (IGrouping<string, Encounter> category in categories)
			{
				((Control)timerCategories.AddMenuItem(category.Key, (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => string.Equals(db.Encounter.Category, category.Key)));
					_displayedTimerDetails = _allTimerDetails.Where((TimerDetailsButton db) => string.Equals(db.Encounter.Category, category.Key)).ToList();
				});
			}
			((Control)enableAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (timerCategories.get_SelectedMenuItem() != enabledTimers)
				{
					_displayedTimerDetails.ForEach(delegate(TimerDetailsButton db)
					{
						//IL_003c: Unknown result type (might be due to invalid IL or missing references)
						if (!db.Encounter.Invalid)
						{
							((GlowButton)((IEnumerable<Control>)((Container)db).get_Children()).Where((Control c) => c is GlowButton && ((GlowButton)c).get_ToggleGlow()).First()).set_Checked(true);
							db.Encounter.Enabled = true;
							_encounterEnableSettings["TimerEnable:" + db.Encounter.Id].set_Value(true);
						}
					});
				}
			});
			((Control)disableAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_displayedTimerDetails.ForEach(delegate(TimerDetailsButton db)
				{
					//IL_003c: Unknown result type (might be due to invalid IL or missing references)
					if (!db.Encounter.Invalid)
					{
						((GlowButton)((IEnumerable<Control>)((Container)db).get_Children()).Where((Control c) => c is GlowButton && ((GlowButton)c).get_ToggleGlow()).First()).set_Checked(false);
						db.Encounter.Enabled = false;
						_encounterEnableSettings["TimerEnable:" + db.Encounter.Id].set_Value(false);
					}
				});
				if (timerCategories.get_SelectedMenuItem() == enabledTimers)
				{
					timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => db.Encounter.Enabled));
				}
			});
			return mainPanel;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged(_onNewMapLoaded);
			_timersTab = GameService.Overlay.get_BlishHudWindow().AddTab("Timers", AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("textures\\155035small.png")), _tabPanel);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			if (_encountersLoaded)
			{
				_activeEncounters.ForEach(delegate(Encounter enc)
				{
					enc.Update(gameTime);
				});
			}
			if (((Control)_debugText).get_Visible())
			{
				_debugText.set_Text("Debug: " + GameService.Gw2Mumble.get_PlayerCharacter().get_Position().X.ToString("0.0") + " " + GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y.ToString("0.0") + " " + GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z.ToString("0.0") + " ");
			}
			if (_centerAlertContainerSetting.get_Value())
			{
				((Control)_alertContainer).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - ((Control)_alertContainer).get_Width() / 2, ((Control)_alertContainer).get_Location().Y));
			}
		}

		protected override void Unload()
		{
			((Control)_debugText).Dispose();
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged(_onNewMapLoaded);
			_showDebugSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateShowDebug);
			_lockAlertContainerSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateLockAlertContainer);
			_centerAlertContainerSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateCenterAlertContainer);
			_hideAlertsSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideAlerts);
			_hideDirectionsSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideDirections);
			_hideMarkersSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideMarkers);
			_alertSizeSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<AlertSize>>)SettingsUpdateAlertSize);
			_alertDisplayOrientationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ControlFlowDirection>>)SettingsUpdateAlertDisplayOrientation);
			_alertContainerLocationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)SettingsUpdateAlertContainerLocation);
			_alertMoveDelaySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingsUpdateAlertMoveDelay);
			_alertFadeDelaySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingsUpdateAlertFadeDelay);
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_timersTab);
			((Control)_tabPanel).Dispose();
			_allTimerDetails.ForEach(delegate(TimerDetailsButton de)
			{
				((Control)de).Dispose();
			});
			_allTimerDetails.Clear();
			((Control)_alertContainer).Dispose();
			((Control)_alertSettingsWindow).Dispose();
			_testAlertPanels.ForEach(delegate(AlertPanel panel)
			{
				panel.Dispose();
			});
			_encounterEnableSettings.Clear();
			_activeAlertIds.Clear();
			_activeDirectionIds.Clear();
			_activeMarkerIds.Clear();
			_encounterIds.Clear();
			_encounters.ForEach(delegate(Encounter enc)
			{
				enc.Dispose();
			});
			_encounters.Clear();
			_activeEncounters.Clear();
			_invalidEncounters.ForEach(delegate(Encounter enc)
			{
				enc.Dispose();
			});
			_invalidEncounters.Clear();
			_pathableResourceManagers.ForEach(delegate(PathableResourceManager m)
			{
				m.Dispose();
			});
			_pathableResourceManagers.Clear();
			ModuleInstance = null;
		}
	}
}
