using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
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

		private StandardWindow _alertSettingsWindow;

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
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Expected O, but got Unknown
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Expected O, but got Unknown
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Expected O, but got Unknown
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Expected O, but got Unknown
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Expected O, but got Unknown
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Expected O, but got Unknown
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			//IL_0631: Unknown result type (might be due to invalid IL or missing references)
			//IL_0637: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0650: Unknown result type (might be due to invalid IL or missing references)
			//IL_065f: Expected O, but got Unknown
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0692: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06df: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Expected O, but got Unknown
			//IL_071c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0721: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Unknown result type (might be due to invalid IL or missing references)
			//IL_0743: Unknown result type (might be due to invalid IL or missing references)
			//IL_0749: Unknown result type (might be due to invalid IL or missing references)
			//IL_0763: Unknown result type (might be due to invalid IL or missing references)
			//IL_076e: Unknown result type (might be due to invalid IL or missing references)
			//IL_077d: Expected O, but got Unknown
			//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_080c: Expected O, but got Unknown
			//IL_083a: Unknown result type (might be due to invalid IL or missing references)
			//IL_083f: Unknown result type (might be due to invalid IL or missing references)
			//IL_084b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0856: Unknown result type (might be due to invalid IL or missing references)
			//IL_0861: Unknown result type (might be due to invalid IL or missing references)
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_0881: Unknown result type (might be due to invalid IL or missing references)
			//IL_088c: Unknown result type (might be due to invalid IL or missing references)
			//IL_089b: Expected O, but got Unknown
			//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_090b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0916: Unknown result type (might be due to invalid IL or missing references)
			//IL_0922: Expected O, but got Unknown
			//IL_0923: Unknown result type (might be due to invalid IL or missing references)
			//IL_0928: Unknown result type (might be due to invalid IL or missing references)
			//IL_0939: Expected O, but got Unknown
			//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0a: Expected O, but got Unknown
			//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4a: Expected O, but got Unknown
			//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acc: Expected I4, but got Unknown
			//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bde: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf5: Expected O, but got Unknown
			//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c4f: Expected O, but got Unknown
			//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ce5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cfb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d12: Expected O, but got Unknown
			//IL_0d12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d49: Expected O, but got Unknown
			//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d8a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d95: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0db8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dc3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dcf: Expected O, but got Unknown
			//IL_0dd0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e05: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e46: Expected O, but got Unknown
			//IL_0e47: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e8a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ebe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eec: Expected O, but got Unknown
			//IL_0f1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f36: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f64: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7b: Expected O, but got Unknown
			//IL_0f7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fcd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff2: Expected O, but got Unknown
			//IL_0ff3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1004: Unknown result type (might be due to invalid IL or missing references)
			//IL_100f: Unknown result type (might be due to invalid IL or missing references)
			//IL_101a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1025: Unknown result type (might be due to invalid IL or missing references)
			//IL_1036: Unknown result type (might be due to invalid IL or missing references)
			//IL_103d: Unknown result type (might be due to invalid IL or missing references)
			//IL_104e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1060: Unknown result type (might be due to invalid IL or missing references)
			//IL_106a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1083: Unknown result type (might be due to invalid IL or missing references)
			//IL_1098: Expected O, but got Unknown
			//IL_10c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_10cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e4: Expected O, but got Unknown
			//IL_1119: Unknown result type (might be due to invalid IL or missing references)
			//IL_112c: Unknown result type (might be due to invalid IL or missing references)
			//IL_119c: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_120e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1225: Unknown result type (might be due to invalid IL or missing references)
			//IL_122e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1233: Unknown result type (might be due to invalid IL or missing references)
			//IL_123b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1251: Unknown result type (might be due to invalid IL or missing references)
			//IL_1258: Unknown result type (might be due to invalid IL or missing references)
			//IL_1265: Unknown result type (might be due to invalid IL or missing references)
			//IL_13dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_13e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_13f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1414: Unknown result type (might be due to invalid IL or missing references)
			//IL_1424: Unknown result type (might be due to invalid IL or missing references)
			//IL_1429: Unknown result type (might be due to invalid IL or missing references)
			//IL_143f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1455: Unknown result type (might be due to invalid IL or missing references)
			//IL_1460: Unknown result type (might be due to invalid IL or missing references)
			//IL_1467: Unknown result type (might be due to invalid IL or missing references)
			//IL_1479: Unknown result type (might be due to invalid IL or missing references)
			//IL_148b: Expected O, but got Unknown
			//IL_14d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_14da: Unknown result type (might be due to invalid IL or missing references)
			//IL_14df: Unknown result type (might be due to invalid IL or missing references)
			//IL_14e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_14f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_14fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1508: Expected O, but got Unknown
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
			if (!Directory.EnumerateFiles(DirectoriesManager.GetFullDirectoryPath("timers")).Any())
			{
				Panel val8 = new Panel();
				((Control)val8).set_Parent((Container)(object)mainPanel);
				((Control)val8).set_Location(new Point(((Control)menuSection).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
				val8.set_ShowBorder(true);
				((Control)val8).set_Size(((Control)timerPanel).get_Size());
				Panel noTimersPanel = val8;
				Label val9 = new Label();
				val9.set_Text("You don't have any timers!\nDownload some and place them in your timers folder.");
				val9.set_HorizontalAlignment((HorizontalAlignment)1);
				val9.set_VerticalAlignment((VerticalAlignment)2);
				((Control)val9).set_Parent((Container)(object)noTimersPanel);
				((Control)val9).set_Size(new Point(((Control)noTimersPanel).get_Width(), ((Control)noTimersPanel).get_Height() / 2 - 64));
				((Control)val9).set_ClipsBounds(false);
				Label noTimersNotice = val9;
				StandardButton val10 = new StandardButton();
				val10.set_Text("Download Hero's Timers");
				((Control)val10).set_Parent((Container)(object)noTimersPanel);
				((Control)val10).set_Width(196);
				((Control)val10).set_Location(new Point(((Control)noTimersPanel).get_Width() / 2 - 200, ((Control)noTimersNotice).get_Bottom() + 24));
				StandardButton val11 = new StandardButton();
				val11.set_Text("Open Timers Folder");
				((Control)val11).set_Parent((Container)(object)noTimersPanel);
				((Control)val11).set_Width(196);
				((Control)val11).set_Location(new Point(((Control)noTimersPanel).get_Width() / 2 + 4, ((Control)noTimersNotice).get_Bottom() + 24));
				StandardButton openTimersFolder = val11;
				Label val12 = new Label();
				val12.set_Text("Once done, restart this module or Blish HUD to enable them.");
				val12.set_HorizontalAlignment((HorizontalAlignment)1);
				val12.set_VerticalAlignment((VerticalAlignment)0);
				((Control)val12).set_Parent((Container)(object)noTimersPanel);
				val12.set_AutoSizeHeight(true);
				((Control)val12).set_Width(((Control)noTimersNotice).get_Width());
				((Control)val12).set_Top(((Control)openTimersFolder).get_Bottom() + 4);
				((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start("https://github.com/QuitarHero/Hero-Timers/releases/latest/download/Hero-Timers.zip");
				});
				((Control)openTimersFolder).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start("explorer.exe", "/open, \"" + DirectoriesManager.GetFullDirectoryPath("timers") + "\\\"");
				});
			}
			((Control)searchBox).set_Width(((Control)menuSection).get_Width());
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => ((DetailsButton)db).get_Text().ToLower().Contains(((TextInputBase)searchBox).get_Text().ToLower())));
			});
			((Control)val5).set_Location(new Point(((Control)menuSection).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			((Control)enableAllButton).set_Location(new Point(((Control)timerPanel).get_Right() - ((Control)enableAllButton).get_Width() - ((Control)disableAllButton).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X * 2, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			((Control)disableAllButton).set_Location(new Point(((Control)enableAllButton).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardWindow val13 = new StandardWindow(Resources.AlertSettingsBackground, new Rectangle(24, 17, 505, 390), new Rectangle(38, 45, 472, 350));
			((Control)val13).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val13).set_Title("Alert Settings");
			((WindowBase2)val13).set_Emblem(Resources.TextureTimerEmblem);
			_alertSettingsWindow = val13;
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
			Checkbox val14 = new Checkbox();
			((Control)val14).set_Parent((Container)(object)_alertSettingsWindow);
			val14.set_Text("Lock Alerts Container");
			((Control)val14).set_BasicTooltipText("When enabled, the alerts container will be locked and cannot be moved.");
			((Control)val14).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox lockAlertsWindowCB = val14;
			lockAlertsWindowCB.set_Checked(_lockAlertContainerSetting.get_Value());
			lockAlertsWindowCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_lockAlertContainerSetting.set_Value(lockAlertsWindowCB.get_Checked());
			});
			Checkbox val15 = new Checkbox();
			((Control)val15).set_Parent((Container)(object)_alertSettingsWindow);
			val15.set_Text("Center Alerts Container");
			((Control)val15).set_BasicTooltipText("When enabled, the location of the alerts container will always be set to the center of the screen.");
			((Control)val15).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)lockAlertsWindowCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox centerAlertsWindowCB = val15;
			centerAlertsWindowCB.set_Checked(_centerAlertContainerSetting.get_Value());
			centerAlertsWindowCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_centerAlertContainerSetting.set_Value(centerAlertsWindowCB.get_Checked());
			});
			Checkbox val16 = new Checkbox();
			((Control)val16).set_Parent((Container)(object)_alertSettingsWindow);
			val16.set_Text("Hide Alerts");
			((Control)val16).set_BasicTooltipText("When enabled, alerts on the screen will be hidden.");
			((Control)val16).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)centerAlertsWindowCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideAlertsCB = val16;
			hideAlertsCB.set_Checked(_hideAlertsSetting.get_Value());
			hideAlertsCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideAlertsSetting.set_Value(hideAlertsCB.get_Checked());
			});
			Checkbox val17 = new Checkbox();
			((Control)val17).set_Parent((Container)(object)_alertSettingsWindow);
			val17.set_Text("Hide Directions");
			((Control)val17).set_BasicTooltipText("When enabled, directions on the screen will be hidden.");
			((Control)val17).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideAlertsCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideDirectionsCB = val17;
			hideDirectionsCB.set_Checked(_hideDirectionsSetting.get_Value());
			hideDirectionsCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideDirectionsSetting.set_Value(hideDirectionsCB.get_Checked());
			});
			Checkbox val18 = new Checkbox();
			((Control)val18).set_Parent((Container)(object)_alertSettingsWindow);
			val18.set_Text("Hide Markers");
			((Control)val18).set_BasicTooltipText("When enabled, markers on the screen will be hidden.");
			((Control)val18).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideDirectionsCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideMarkersCB = val18;
			hideMarkersCB.set_Checked(_hideMarkersSetting.get_Value());
			hideMarkersCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideMarkersSetting.set_Value(hideMarkersCB.get_Checked());
			});
			Label val19 = new Label();
			((Control)val19).set_Parent((Container)(object)_alertSettingsWindow);
			val19.set_Text("Alert Size");
			val19.set_AutoSizeWidth(true);
			((Control)val19).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideMarkersCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertSizeLabel = val19;
			Dropdown val20 = new Dropdown();
			((Control)val20).set_Parent((Container)(object)_alertSettingsWindow);
			Dropdown alertSizeDropdown = val20;
			alertSizeDropdown.get_Items().Add("Small");
			alertSizeDropdown.get_Items().Add("Medium");
			alertSizeDropdown.get_Items().Add("Large");
			alertSizeDropdown.set_SelectedItem(_alertSizeSetting.get_Value().ToString());
			alertSizeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_alertSizeSetting.set_Value((AlertSize)Enum.Parse(typeof(AlertSize), alertSizeDropdown.get_SelectedItem(), ignoreCase: true));
			});
			Label val21 = new Label();
			((Control)val21).set_Parent((Container)(object)_alertSettingsWindow);
			val21.set_Text("Alert Display Orientation");
			val21.set_AutoSizeWidth(true);
			((Control)val21).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertSizeLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertDisplayOrientationLabel = val21;
			Dropdown val22 = new Dropdown();
			((Control)val22).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val22).set_Location(new Point(((Control)alertDisplayOrientationLabel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationLabel).get_Top()));
			Dropdown alertDisplayOrientationDropdown = val22;
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
			Label val23 = new Label();
			((Control)val23).set_Parent((Container)(object)_alertSettingsWindow);
			val23.set_Text("Alert Preview");
			val23.set_AutoSizeWidth(true);
			((Control)val23).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton val24 = new StandardButton();
			((Control)val24).set_Parent((Container)(object)_alertSettingsWindow);
			val24.set_Text("Add Test Alert");
			((Control)val24).set_Location(new Point(((Control)alertDisplayOrientationDropdown).get_Left(), ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton addTestAlertButton = val24;
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
			StandardButton val25 = new StandardButton();
			((Control)val25).set_Parent((Container)(object)_alertSettingsWindow);
			val25.set_Text("Clear Test Alerts");
			((Control)val25).set_Location(new Point(((Control)addTestAlertButton).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)addTestAlertButton).get_Top()));
			StandardButton clearTestAlertsButton = val25;
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
			Label val26 = new Label();
			((Control)val26).set_Parent((Container)(object)_alertSettingsWindow);
			val26.set_Text("Alert Container Position");
			val26.set_AutoSizeWidth(true);
			((Control)val26).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)clearTestAlertsButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertContainerPositionLabel = val26;
			StandardButton val27 = new StandardButton();
			((Control)val27).set_Parent((Container)(object)_alertSettingsWindow);
			val27.set_Text("Reset Position");
			((Control)val27).set_Location(new Point(((Control)addTestAlertButton).get_Left(), ((Control)alertContainerPositionLabel).get_Top()));
			StandardButton resetAlertContainerPositionButton = val27;
			((Control)resetAlertContainerPositionButton).set_Width(((Control)alertSizeDropdown).get_Width());
			((Control)resetAlertContainerPositionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				_alertContainerLocationSetting.set_Value(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - ((Control)_alertContainer).get_Width() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 - ((Control)_alertContainer).get_Height() / 2));
			});
			Label val28 = new Label();
			((Control)val28).set_Parent((Container)(object)_alertSettingsWindow);
			val28.set_Text("Alert Move Delay");
			((Control)val28).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val28.set_AutoSizeWidth(true);
			((Control)val28).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)resetAlertContainerPositionButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertMoveDelayLabel = val28;
			TextBox val29 = new TextBox();
			((Control)val29).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val29).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			((Control)val29).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val29).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val29).set_Height(((Control)alertMoveDelayLabel).get_Height());
			((TextInputBase)val29).set_Text($"{_alertMoveDelaySetting.get_Value():0.00}");
			TextBox alertMoveDelayTextBox = val29;
			TrackBar val30 = new TrackBar();
			((Control)val30).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val30).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val30.set_MinValue(0f);
			val30.set_MaxValue(3f);
			val30.set_Value(_alertMoveDelaySetting.get_Value());
			val30.set_SmallStep(true);
			((Control)val30).set_Location(new Point(((Control)alertMoveDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val30).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertMoveDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertMoveDelaySlider = val30;
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
			Label val31 = new Label();
			((Control)val31).set_Parent((Container)(object)_alertSettingsWindow);
			val31.set_Text("Alert Fade In/Out Delay");
			((Control)val31).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val31.set_AutoSizeWidth(true);
			((Control)val31).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertFadeDelayLabel = val31;
			TextBox val32 = new TextBox();
			((Control)val32).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val32).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			((Control)val32).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val32).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val32).set_Height(((Control)alertFadeDelayLabel).get_Height());
			((TextInputBase)val32).set_Text($"{_alertFadeDelaySetting.get_Value():0.00}");
			TextBox alertFadeDelayTextBox = val32;
			TrackBar val33 = new TrackBar();
			((Control)val33).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val33).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val33.set_MinValue(0f);
			val33.set_MaxValue(3f);
			val33.set_Value(_alertFadeDelaySetting.get_Value());
			val33.set_SmallStep(true);
			((Control)val33).set_Location(new Point(((Control)alertFadeDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val33).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertFadeDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertFadeDelaySlider = val33;
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
			StandardButton val34 = new StandardButton();
			((Control)val34).set_Parent((Container)(object)_alertSettingsWindow);
			val34.set_Text("Close");
			StandardButton closeAlertSettingsButton = val34;
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
					GlowButton val35 = new GlowButton();
					val35.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureDescription));
					((Control)val35).set_BasicTooltipText("By: " + enc2.Author);
					((Control)val35).set_Parent((Container)(object)entry);
				}
				GlowButton val36 = new GlowButton();
				((Control)val36).set_Parent((Container)(object)entry);
				val36.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureX));
				val36.set_ToggleGlow(true);
				((Control)val36).set_BasicTooltipText(enc2.Description);
				((Control)val36).set_Enabled(false);
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
					GlowButton val37 = new GlowButton();
					val37.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureDescription));
					((Control)val37).set_BasicTooltipText("By: " + enc3.Author);
					((Control)val37).set_Parent((Container)(object)entry2);
				}
				GlowButton val38 = new GlowButton();
				val38.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureEye));
				val38.set_ActiveIcon(AsyncTexture2D.op_Implicit(Resources.TextureEyeActive));
				((Control)val38).set_BasicTooltipText("Click to toggle timer");
				val38.set_ToggleGlow(true);
				val38.set_Checked(enc3.Enabled);
				((Control)val38).set_Parent((Container)(object)entry2);
				GlowButton toggleButton = val38;
				((Control)toggleButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					enc3.Enabled = toggleButton.get_Checked();
					setting.set_Value(toggleButton.get_Checked());
					((DetailsButton)entry2).set_ToggleState(toggleButton.get_Checked());
					ResetActivatedEncounters();
				});
				_allTimerDetails.Add(entry2);
			}
			Menu val39 = new Menu();
			Rectangle contentRegion = ((Container)menuSection).get_ContentRegion();
			((Control)val39).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val39.set_MenuItemHeight(40);
			((Control)val39).set_Parent((Container)(object)menuSection);
			val39.set_CanSelect(true);
			Menu timerCategories = val39;
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
