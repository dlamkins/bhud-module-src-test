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
using Charr.Timers_BlishHUD.Controls.BigWigs;
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

		private List<IAlertPanel> _testAlertPanels;

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

		public SettingEntry<AlertType> _alertSizeSetting;

		public SettingEntry<ControlFlowDirection> _alertDisplayOrientationSetting;

		private SettingEntry<Point> _alertContainerLocationSetting;

		public SettingEntry<float> _alertMoveDelaySetting;

		public SettingEntry<float> _alertFadeDelaySetting;

		public SettingEntry<bool> _alertFillDirection;

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
			_alertSizeSetting = _alertSettingCollection.DefineSetting<AlertType>("AlertSize", AlertType.BigWigStyle, (Func<string>)null, (Func<string>)null);
			_alertDisplayOrientationSetting = _alertSettingCollection.DefineSetting<ControlFlowDirection>("AlertDisplayOrientation", (ControlFlowDirection)1, (Func<string>)null, (Func<string>)null);
			_alertContainerLocationSetting = _alertSettingCollection.DefineSetting<Point>("AlertContainerLocation", Point.get_Zero(), (Func<string>)null, (Func<string>)null);
			_alertMoveDelaySetting = _alertSettingCollection.DefineSetting<float>("AlertMoveSpeed", 1f, (Func<string>)null, (Func<string>)null);
			_alertFadeDelaySetting = _alertSettingCollection.DefineSetting<float>("AlertFadeSpeed", 1f, (Func<string>)null, (Func<string>)null);
			_alertFillDirection = _alertSettingCollection.DefineSetting<bool>("FillDirection", true, (Func<string>)null, (Func<string>)null);
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
			_testAlertPanels?.ForEach(delegate(IAlertPanel panel)
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
			case AlertType.Small:
				AlertPanel.DEFAULT_ALERTPANEL_WIDTH = 320;
				AlertPanel.DEFAULT_ALERTPANEL_HEIGHT = 64;
				break;
			case AlertType.Medium:
				AlertPanel.DEFAULT_ALERTPANEL_WIDTH = 320;
				AlertPanel.DEFAULT_ALERTPANEL_HEIGHT = 96;
				break;
			case AlertType.Large:
				AlertPanel.DEFAULT_ALERTPANEL_WIDTH = 320;
				AlertPanel.DEFAULT_ALERTPANEL_HEIGHT = 128;
				break;
			case AlertType.BigWigStyle:
				AlertPanel.DEFAULT_ALERTPANEL_WIDTH = 336;
				AlertPanel.DEFAULT_ALERTPANEL_HEIGHT = 35;
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
			_testAlertPanels = new List<IAlertPanel>();
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
			_alertSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<AlertType>>)SettingsUpdateAlertSize);
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
				SortedZipArchiveReader zipDataReader = new SortedZipArchiveReader(item);
				PathableResourceManager zipResourceManager = new PathableResourceManager((IDataReader)(object)zipDataReader);
				_pathableResourceManagers.Add(zipResourceManager);
				zipDataReader.LoadOnFileType(delegate(Stream fileStream, IDataReader dataReader)
				{
					readJson(fileStream, zipResourceManager);
				}, ".bhtimer");
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
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Expected O, but got Unknown
			//IL_061c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0621: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Expected O, but got Unknown
			//IL_0691: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06be: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f2: Expected O, but got Unknown
			//IL_0720: Unknown result type (might be due to invalid IL or missing references)
			//IL_0725: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Unknown result type (might be due to invalid IL or missing references)
			//IL_073c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_074d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0767: Unknown result type (might be due to invalid IL or missing references)
			//IL_0772: Unknown result type (might be due to invalid IL or missing references)
			//IL_0781: Expected O, but got Unknown
			//IL_07af: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0801: Unknown result type (might be due to invalid IL or missing references)
			//IL_0810: Expected O, but got Unknown
			//IL_083e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0843: Unknown result type (might be due to invalid IL or missing references)
			//IL_084f: Unknown result type (might be due to invalid IL or missing references)
			//IL_085a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0865: Unknown result type (might be due to invalid IL or missing references)
			//IL_086b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0885: Unknown result type (might be due to invalid IL or missing references)
			//IL_0890: Unknown result type (might be due to invalid IL or missing references)
			//IL_089f: Expected O, but got Unknown
			//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08de: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0914: Unknown result type (might be due to invalid IL or missing references)
			//IL_091f: Unknown result type (might be due to invalid IL or missing references)
			//IL_092e: Expected O, but got Unknown
			//IL_095b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0960: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0977: Unknown result type (might be due to invalid IL or missing references)
			//IL_097e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0984: Unknown result type (might be due to invalid IL or missing references)
			//IL_099e: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b5: Expected O, but got Unknown
			//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_09cc: Expected O, but got Unknown
			//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab2: Expected O, but got Unknown
			//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af2: Expected O, but got Unknown
			//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b51: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b74: Expected I4, but got Unknown
			//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bfa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9d: Expected O, but got Unknown
			//IL_0cb0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ceb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf7: Expected O, but got Unknown
			//IL_0d3e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d64: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d75: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dba: Expected O, but got Unknown
			//IL_0dba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dcb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df1: Expected O, but got Unknown
			//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e60: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e77: Expected O, but got Unknown
			//IL_0e78: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e89: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e94: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ead: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eee: Expected O, but got Unknown
			//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f00: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f16: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f21: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f94: Expected O, but got Unknown
			//IL_0fc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fde: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
			//IL_100c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1017: Unknown result type (might be due to invalid IL or missing references)
			//IL_1023: Expected O, but got Unknown
			//IL_1024: Unknown result type (might be due to invalid IL or missing references)
			//IL_1029: Unknown result type (might be due to invalid IL or missing references)
			//IL_1035: Unknown result type (might be due to invalid IL or missing references)
			//IL_1040: Unknown result type (might be due to invalid IL or missing references)
			//IL_104f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1059: Unknown result type (might be due to invalid IL or missing references)
			//IL_1068: Unknown result type (might be due to invalid IL or missing references)
			//IL_1075: Unknown result type (might be due to invalid IL or missing references)
			//IL_109a: Expected O, but got Unknown
			//IL_109b: Unknown result type (might be due to invalid IL or missing references)
			//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_10c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_10cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_10de: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_10f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1108: Unknown result type (might be due to invalid IL or missing references)
			//IL_1112: Unknown result type (might be due to invalid IL or missing references)
			//IL_112b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1140: Expected O, but got Unknown
			//IL_116e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1173: Unknown result type (might be due to invalid IL or missing references)
			//IL_117f: Unknown result type (might be due to invalid IL or missing references)
			//IL_118c: Expected O, but got Unknown
			//IL_11c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1244: Unknown result type (might be due to invalid IL or missing references)
			//IL_129b: Unknown result type (might be due to invalid IL or missing references)
			//IL_12a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12db: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_12f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1300: Unknown result type (might be due to invalid IL or missing references)
			//IL_130d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1485: Unknown result type (might be due to invalid IL or missing references)
			//IL_148a: Unknown result type (might be due to invalid IL or missing references)
			//IL_14a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_14bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_14cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_14e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_14fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_1508: Unknown result type (might be due to invalid IL or missing references)
			//IL_150f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1521: Unknown result type (might be due to invalid IL or missing references)
			//IL_1533: Expected O, but got Unknown
			//IL_157b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1580: Unknown result type (might be due to invalid IL or missing references)
			//IL_1582: Unknown result type (might be due to invalid IL or missing references)
			//IL_1587: Unknown result type (might be due to invalid IL or missing references)
			//IL_158b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1595: Unknown result type (might be due to invalid IL or missing references)
			//IL_159d: Unknown result type (might be due to invalid IL or missing references)
			//IL_15a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_15b0: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Size(((Rectangle)(ref panelBounds)).get_Size());
			Panel mainPanel = val;
			AlertContainer alertContainer = new AlertContainer();
			((Control)alertContainer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			alertContainer.ControlPadding = new Vector2(10f, 5f);
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
			StandardWindow val13 = new StandardWindow(Resources.AlertSettingsBackground, new Rectangle(24, 17, 505, 390), new Rectangle(38, 38, 472, 350));
			((Control)val13).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val13).set_Title("Alert Settings");
			((WindowBase2)val13).set_Emblem(Resources.TextureTimerEmblem);
			((WindowBase2)val13).set_SavesPosition(true);
			((WindowBase2)val13).set_Id("TimersAlertSettingsWindow");
			_alertSettingsWindow = val13;
			((Control)_alertSettingsWindow).Hide();
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)_alertSettingsWindow).get_Visible())
				{
					((Control)_alertSettingsWindow).Hide();
				}
				else
				{
					((Control)_alertSettingsWindow).Show();
				}
			});
			Checkbox val14 = new Checkbox();
			((Control)val14).set_Parent((Container)(object)_alertSettingsWindow);
			val14.set_Text("Lock Alerts Container");
			((Control)val14).set_BasicTooltipText("When enabled, the alerts container will be locked and cannot be moved.");
			((Control)val14).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0));
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
			Checkbox val19 = new Checkbox();
			((Control)val19).set_Parent((Container)(object)_alertSettingsWindow);
			val19.set_Text("Invert Alert Fill");
			((Control)val19).set_BasicTooltipText("When enabled, alerts fill up as time passes.\nWhen disabled, alerts drain as time passes.");
			((Control)val19).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideMarkersCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox fillDirection = val19;
			fillDirection.set_Checked(_alertFillDirection.get_Value());
			fillDirection.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_alertFillDirection.set_Value(fillDirection.get_Checked());
			});
			Label val20 = new Label();
			((Control)val20).set_Parent((Container)(object)_alertSettingsWindow);
			val20.set_Text("Alert Size");
			val20.set_AutoSizeWidth(true);
			((Control)val20).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)fillDirection).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertSizeLabel = val20;
			Dropdown val21 = new Dropdown();
			((Control)val21).set_Parent((Container)(object)_alertSettingsWindow);
			Dropdown alertSizeDropdown = val21;
			alertSizeDropdown.get_Items().Add("Small");
			alertSizeDropdown.get_Items().Add("Medium");
			alertSizeDropdown.get_Items().Add("Large");
			alertSizeDropdown.get_Items().Add("BigWig Style");
			alertSizeDropdown.set_SelectedItem(_alertSizeSetting.get_Value().ToString());
			alertSizeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_alertSizeSetting.set_Value((AlertType)Enum.Parse(typeof(AlertType), alertSizeDropdown.get_SelectedItem().Replace(" ", ""), ignoreCase: true));
			});
			Label val22 = new Label();
			((Control)val22).set_Parent((Container)(object)_alertSettingsWindow);
			val22.set_Text("Alert Display Orientation");
			val22.set_AutoSizeWidth(true);
			((Control)val22).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertSizeLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertDisplayOrientationLabel = val22;
			Dropdown val23 = new Dropdown();
			((Control)val23).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val23).set_Location(new Point(((Control)alertDisplayOrientationLabel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationLabel).get_Top()));
			Dropdown alertDisplayOrientationDropdown = val23;
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
			Label val24 = new Label();
			((Control)val24).set_Parent((Container)(object)_alertSettingsWindow);
			val24.set_Text("Alert Preview");
			val24.set_AutoSizeWidth(true);
			((Control)val24).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton val25 = new StandardButton();
			((Control)val25).set_Parent((Container)(object)_alertSettingsWindow);
			val25.set_Text("Add Test Alert");
			((Control)val25).set_Location(new Point(((Control)alertDisplayOrientationDropdown).get_Left(), ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton addTestAlertButton = val25;
			((Control)addTestAlertButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				IAlertPanel alertPanel3;
				if (_alertSizeSetting.get_Value() != AlertType.BigWigStyle)
				{
					AlertPanel alertPanel = new AlertPanel();
					((FlowPanel)alertPanel).set_ControlPadding(new Vector2(10f, 10f));
					((FlowPanel)alertPanel).set_PadLeftBeforeControl(true);
					((FlowPanel)alertPanel).set_PadTopBeforeControl(true);
					IAlertPanel alertPanel2 = alertPanel;
					alertPanel3 = alertPanel2;
				}
				else
				{
					IAlertPanel alertPanel2 = new BigWigAlert();
					alertPanel3 = alertPanel2;
				}
				IAlertPanel alertPanel4 = alertPanel3;
				alertPanel4.Text = "Test Alert " + (_testAlertPanels.Count + 1);
				alertPanel4.TextColor = Color.get_White();
				alertPanel4.Icon = AsyncTexture2D.op_Implicit(Texture2DExtension.Duplicate(AsyncTexture2D.op_Implicit(Resources.GetIcon("raid"))));
				alertPanel4.MaxFill = 100f;
				alertPanel4.CurrentFill = (float)RandomUtil.GetRandom(0, 100) + (float)RandomUtil.GetRandom(0, 100) * 0.01f;
				alertPanel4.FillColor = Color.get_Red();
				alertPanel4.ShouldShow = !_hideAlertsSetting.get_Value();
				((Control)alertPanel4).set_Parent((Container)(object)_alertContainer);
				_testAlertPanels.Add(alertPanel4);
				_alertContainer.UpdateDisplay();
			});
			StandardButton val26 = new StandardButton();
			((Control)val26).set_Parent((Container)(object)_alertSettingsWindow);
			val26.set_Text("Clear Test Alerts");
			((Control)val26).set_Location(new Point(((Control)addTestAlertButton).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)addTestAlertButton).get_Top()));
			StandardButton clearTestAlertsButton = val26;
			((Control)clearTestAlertsButton).set_Width((int)((double)((Control)clearTestAlertsButton).get_Width() * 1.15));
			((Control)clearTestAlertsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_testAlertPanels.ForEach(delegate(IAlertPanel panel)
				{
					panel.Dispose();
				});
				_testAlertPanels.Clear();
				_alertContainer.UpdateDisplay();
			});
			((Control)alertSizeDropdown).set_Width(((Control)addTestAlertButton).get_Width() + ((Control)clearTestAlertsButton).get_Width() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			((Control)alertDisplayOrientationDropdown).set_Width(((Control)alertSizeDropdown).get_Width());
			Label val27 = new Label();
			((Control)val27).set_Parent((Container)(object)_alertSettingsWindow);
			val27.set_Text("Alert Container Position");
			val27.set_AutoSizeWidth(true);
			((Control)val27).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)clearTestAlertsButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertContainerPositionLabel = val27;
			StandardButton val28 = new StandardButton();
			((Control)val28).set_Parent((Container)(object)_alertSettingsWindow);
			val28.set_Text("Reset Position");
			((Control)val28).set_Location(new Point(((Control)addTestAlertButton).get_Left(), ((Control)alertContainerPositionLabel).get_Top()));
			StandardButton resetAlertContainerPositionButton = val28;
			((Control)resetAlertContainerPositionButton).set_Width(((Control)alertSizeDropdown).get_Width());
			((Control)resetAlertContainerPositionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				_alertContainerLocationSetting.set_Value(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - ((Control)_alertContainer).get_Width() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 - ((Control)_alertContainer).get_Height() / 2));
			});
			Label val29 = new Label();
			((Control)val29).set_Parent((Container)(object)_alertSettingsWindow);
			val29.set_Text("Alert Move Delay");
			((Control)val29).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val29.set_AutoSizeWidth(true);
			((Control)val29).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)resetAlertContainerPositionButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertMoveDelayLabel = val29;
			TextBox val30 = new TextBox();
			((Control)val30).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val30).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			((Control)val30).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val30).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val30).set_Height(((Control)alertMoveDelayLabel).get_Height());
			((TextInputBase)val30).set_Text($"{_alertMoveDelaySetting.get_Value():0.00}");
			TextBox alertMoveDelayTextBox = val30;
			TrackBar val31 = new TrackBar();
			((Control)val31).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val31).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val31.set_MinValue(0f);
			val31.set_MaxValue(3f);
			val31.set_Value(_alertMoveDelaySetting.get_Value());
			val31.set_SmallStep(true);
			((Control)val31).set_Location(new Point(((Control)alertMoveDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val31).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertMoveDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertMoveDelaySlider = val31;
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
			Label val32 = new Label();
			((Control)val32).set_Parent((Container)(object)_alertSettingsWindow);
			val32.set_Text("Alert Fade In/Out Delay");
			((Control)val32).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val32.set_AutoSizeWidth(true);
			((Control)val32).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertFadeDelayLabel = val32;
			TextBox val33 = new TextBox();
			((Control)val33).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val33).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			((Control)val33).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val33).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val33).set_Height(((Control)alertFadeDelayLabel).get_Height());
			((TextInputBase)val33).set_Text($"{_alertFadeDelaySetting.get_Value():0.00}");
			TextBox alertFadeDelayTextBox = val33;
			TrackBar val34 = new TrackBar();
			((Control)val34).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val34).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val34.set_MinValue(0f);
			val34.set_MaxValue(3f);
			val34.set_Value(_alertFadeDelaySetting.get_Value());
			val34.set_SmallStep(true);
			((Control)val34).set_Location(new Point(((Control)alertFadeDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val34).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertFadeDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertFadeDelaySlider = val34;
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
			StandardButton val35 = new StandardButton();
			((Control)val35).set_Parent((Container)(object)_alertSettingsWindow);
			val35.set_Text("Close");
			StandardButton closeAlertSettingsButton = val35;
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
					GlowButton val36 = new GlowButton();
					val36.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureDescription));
					((Control)val36).set_BasicTooltipText("By: " + enc2.Author);
					((Control)val36).set_Parent((Container)(object)entry);
				}
				GlowButton val37 = new GlowButton();
				((Control)val37).set_Parent((Container)(object)entry);
				val37.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureX));
				val37.set_ToggleGlow(true);
				((Control)val37).set_BasicTooltipText(enc2.Description);
				((Control)val37).set_Enabled(false);
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
					GlowButton val38 = new GlowButton();
					val38.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureDescription));
					((Control)val38).set_BasicTooltipText("By: " + enc3.Author);
					((Control)val38).set_Parent((Container)(object)entry2);
				}
				GlowButton val39 = new GlowButton();
				val39.set_Icon(AsyncTexture2D.op_Implicit(Resources.TextureEye));
				val39.set_ActiveIcon(AsyncTexture2D.op_Implicit(Resources.TextureEyeActive));
				((Control)val39).set_BasicTooltipText("Click to toggle timer");
				val39.set_ToggleGlow(true);
				val39.set_Checked(enc3.Enabled);
				((Control)val39).set_Parent((Container)(object)entry2);
				GlowButton toggleButton = val39;
				((Control)toggleButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					enc3.Enabled = toggleButton.get_Checked();
					setting.set_Value(toggleButton.get_Checked());
					((DetailsButton)entry2).set_ToggleState(toggleButton.get_Checked());
					ResetActivatedEncounters();
				});
				_allTimerDetails.Add(entry2);
			}
			Menu val40 = new Menu();
			Rectangle contentRegion = ((Container)menuSection).get_ContentRegion();
			((Control)val40).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val40.set_MenuItemHeight(40);
			((Control)val40).set_Parent((Container)(object)menuSection);
			val40.set_CanSelect(true);
			Menu timerCategories = val40;
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
			if (_encounters.Any((Encounter e) => e.Invalid))
			{
				((Control)timerCategories.AddMenuItem("Invalid Timers", (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					timerPanel.FilterChildren<TimerDetailsButton>((Func<TimerDetailsButton, bool>)((TimerDetailsButton db) => db.Encounter.Invalid));
					_displayedTimerDetails = _allTimerDetails.Where((TimerDetailsButton db) => db.Encounter.Invalid).ToList();
				});
			}
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
			_alertSizeSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<AlertType>>)SettingsUpdateAlertSize);
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
			_testAlertPanels.ForEach(delegate(IAlertPanel panel)
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
