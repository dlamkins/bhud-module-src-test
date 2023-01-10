using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using Charr.Timers_BlishHUD.Controls.ResetButton;
using Charr.Timers_BlishHUD.IO;
using Charr.Timers_BlishHUD.Models;
using Charr.Timers_BlishHUD.Pathing.Content;
using Charr.Timers_BlishHUD.State;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Octokit;

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

		private ResetButton _resetButton;

		private WindowTab _timersTab;

		public Menu timerCategories;

		public FlowPanel timerPanel;

		private Panel _tabPanel;

		private List<TimerDetails> _allTimerDetails;

		private List<TimerDetails> _displayedTimerDetails;

		private Blish_HUD.Controls.Label _debugText;

		private List<PathableResourceManager> _pathableResourceManagers;

		private JsonSerializerSettings _jsonSettings;

		private bool _encountersLoaded;

		private bool _errorCaught;

		private HashSet<string> _encounterIds;

		public Dictionary<string, Alert> _activeAlertIds;

		public Dictionary<string, Direction> _activeDirectionIds;

		public Dictionary<string, Marker> _activeMarkerIds;

		private HashSet<Encounter> _encounters;

		private HashSet<Encounter> _activeEncounters;

		private HashSet<Encounter> _invalidEncounters;

		private EventHandler<ValueEventArgs<int>> _onNewMapLoaded;

		private SettingEntry<Update> _lastTimersUpdate;

		private SettingEntry<bool> _showDebugSetting;

		public SettingEntry<bool> _debugModeSetting;

		private Dictionary<string, SettingEntry<bool>> _encounterEnableSettings;

		private SettingEntry<bool> _sortCategorySetting;

		public SettingCollection _timerSettingCollection;

		public SettingEntry<KeyBinding>[] _keyBindSettings;

		public SettingEntry<bool> _showResetTimerButton;

		public SettingEntry<Point> _resetTimerButtonLocationSetting;

		public SettingEntry<Point> _resetTimerButtonSizeSetting;

		public SettingEntry<KeyBinding> _resetTimerHotKeySetting;

		private SettingCollection _alertSettingCollection;

		public SettingEntry<bool> _lockAlertContainerSetting;

		private SettingEntry<bool> _centerAlertContainerSetting;

		public SettingEntry<bool> _hideAlertsSetting;

		public SettingEntry<bool> _hideDirectionsSetting;

		public SettingEntry<bool> _hideMarkersSetting;

		public SettingEntry<bool> _hideSoundsSetting;

		public SettingEntry<AlertType> _alertSizeSetting;

		public SettingEntry<AlertFlowDirection> _alertDisplayOrientationSetting;

		public SettingEntry<Point> _alertContainerLocationSetting;

		public SettingEntry<Point> _alertContainerSizeSetting;

		public SettingEntry<float> _alertMoveDelaySetting;

		public SettingEntry<float> _alertFadeDelaySetting;

		public SettingEntry<bool> _alertFillDirection;

		public Update update = new Update();

		public TimerLoader timerLoader;

		private bool _timersNeedUpdate;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public TimersModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			if (!settings.TryGetSetting("LastTimersUpdate", out _lastTimersUpdate))
			{
				_lastTimersUpdate = settings.DefineSetting("LastTimersUpdate", new Update(), "Last Timers Update", "Date of last timers update");
			}
			_showDebugSetting = settings.DefineSetting("ShowDebugText", defaultValue: false, "Show Debug Text", "For creating timers. Placed in top-left corner. Displays location status.");
			_debugModeSetting = settings.DefineSetting("DebugMode", defaultValue: false, "Enable Debug Mode", "All Timer triggers will ignore requireCombat setting, allowing you to test them freely.");
			_sortCategorySetting = settings.DefineSetting("SortByCategory", defaultValue: false, "Sort Categories", "When enabled, categories from loaded timer files are sorted in alphanumerical order.\nOtherwise, categories are in the order they are loaded.\nThe module needs to be restarted to take effect.");
			_timerSettingCollection = settings.AddSubCollection("EnabledTimers");
			_showResetTimerButton = settings.DefineSetting("Show Reset Button", defaultValue: true);
			_showResetTimerButton.SettingChanged += _showResetTimerButton_SettingChanged;
			_resetTimerHotKeySetting = settings.DefineSetting("Reset Active Timer", new KeyBinding());
			_resetTimerHotKeySetting.Value.Enabled = true;
			_resetTimerHotKeySetting.Value.Activated += ResetHotkey_Activated;
			SettingCollection _internalSettings = settings.AddSubCollection("Internal Settings");
			_resetTimerButtonLocationSetting = _internalSettings.DefineSetting<Point>("Reset Timer Button Location", new Point(150, 150));
			_resetTimerButtonSizeSetting = _internalSettings.DefineSetting<Point>("Reset Timer Button Size", new Point(64, 64));
			_keyBindSettings = new SettingEntry<KeyBinding>[5];
			for (int i = 0; i < 5; i++)
			{
				_keyBindSettings[i] = settings.DefineSetting("Trigger Key " + i, new KeyBinding(), "Trigger Key " + i, "For timers that require keys to trigger.");
			}
			_alertSettingCollection = settings.AddSubCollection("AlertSetting");
			_lockAlertContainerSetting = _alertSettingCollection.DefineSetting("LockAlertContainer", defaultValue: false);
			_hideAlertsSetting = _alertSettingCollection.DefineSetting("HideAlerts", defaultValue: false);
			_hideDirectionsSetting = _alertSettingCollection.DefineSetting("HideDirections", defaultValue: false);
			_hideMarkersSetting = _alertSettingCollection.DefineSetting("HideMarkers", defaultValue: false);
			_hideSoundsSetting = _alertSettingCollection.DefineSetting("HideSounds", defaultValue: false);
			_centerAlertContainerSetting = _alertSettingCollection.DefineSetting("CenterAlertContainer", defaultValue: true);
			_alertSizeSetting = _alertSettingCollection.DefineSetting("AlertSize", AlertType.BigWigStyle);
			_alertDisplayOrientationSetting = _alertSettingCollection.DefineSetting("AlertDisplayOrientation", AlertFlowDirection.TopToBottom);
			_alertContainerLocationSetting = _alertSettingCollection.DefineSetting<Point>("AlertContainerLocation", new Point(GameService.Graphics.WindowWidth - GameService.Graphics.WindowWidth / 4, GameService.Graphics.WindowHeight / 2));
			_alertContainerSizeSetting = _alertSettingCollection.DefineSetting<Point>("AlertContainerSize", new Point(0, 0));
			_alertMoveDelaySetting = _alertSettingCollection.DefineSetting("AlertMoveSpeed", 0.75f);
			_alertFadeDelaySetting = _alertSettingCollection.DefineSetting("AlertFadeSpeed", 1f);
			_alertFillDirection = _alertSettingCollection.DefineSetting("FillDirection", defaultValue: true);
		}

		private void _showResetTimerButton_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (_resetButton != null)
			{
				_resetButton.Visible = e.NewValue && _activeEncounters.Count > 0;
			}
		}

		private void ResetHotkey_Activated(object sender, EventArgs e)
		{
			ResetActiveEncounters();
		}

		private void ResetActiveEncounters()
		{
			foreach (Encounter activeEncounter in _activeEncounters)
			{
				activeEncounter.Reset();
			}
		}

		private void SettingsUpdateShowDebug(object sender = null, EventArgs e = null)
		{
			if (_showDebugSetting.Value)
			{
				_debugText?.Show();
			}
			else
			{
				_debugText?.Hide();
			}
		}

		private void SettingsUpdateLockAlertContainer(object sender = null, EventArgs e = null)
		{
			if (_alertContainer != null)
			{
				_alertContainer.LocationLock = _lockAlertContainerSetting.Value;
			}
		}

		private void SettingsUpdateCenterAlertContainer(object sender = null, EventArgs e = null)
		{
		}

		private void SettingsUpdateHideAlerts(object sender = null, EventArgs e = null)
		{
			_testAlertPanels?.ForEach(delegate(IAlertPanel panel)
			{
				panel.ShouldShow = !_hideAlertsSetting.Value;
			});
			foreach (Encounter encounter in _encounters)
			{
				encounter.ShowAlerts = !_hideAlertsSetting.Value;
			}
		}

		private void SettingsUpdateHideDirections(object sender = null, EventArgs e = null)
		{
			foreach (Encounter encounter in _encounters)
			{
				encounter.ShowDirections = !_hideDirectionsSetting.Value;
			}
		}

		private void SettingsUpdateHideMarkers(object sender = null, EventArgs e = null)
		{
			foreach (Encounter encounter in _encounters)
			{
				encounter.ShowMarkers = !_hideMarkersSetting.Value;
			}
		}

		private void SettingsUpdateAlertSize(object sender = null, EventArgs e = null)
		{
			switch (_alertSizeSetting.Value)
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
			_alertContainer.FlowDirection = _alertDisplayOrientationSetting.Value;
		}

		private void SettingsUpdateAlertMoveDelay(object sender = null, EventArgs e = null)
		{
		}

		private void SettingsUpdateAlertFadeDelay(object sender = null, EventArgs e = null)
		{
		}

		protected override void Initialize()
		{
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			Resources = new Resources();
			_pathableResourceManagers = new List<PathableResourceManager>();
			_encounterEnableSettings = new Dictionary<string, SettingEntry<bool>>();
			_encounterIds = new HashSet<string>();
			_activeAlertIds = new Dictionary<string, Alert>();
			_activeDirectionIds = new Dictionary<string, Direction>();
			_activeMarkerIds = new Dictionary<string, Marker>();
			_encounters = new HashSet<Encounter>();
			_activeEncounters = new HashSet<Encounter>();
			_invalidEncounters = new HashSet<Encounter>();
			_allTimerDetails = new List<TimerDetails>();
			_testAlertPanels = new List<IAlertPanel>();
			_debugText = new Blish_HUD.Controls.Label
			{
				Parent = GameService.Graphics.SpriteScreen,
				Location = new Point(10, 38),
				TextColor = Color.get_White(),
				Font = Resources.Font,
				Text = "DEBUG",
				HorizontalAlignment = HorizontalAlignment.Left,
				StrokeText = true,
				ShowShadow = true,
				AutoSizeWidth = true
			};
			SettingsUpdateShowDebug();
			_showDebugSetting.SettingChanged += SettingsUpdateShowDebug;
			_lockAlertContainerSetting.SettingChanged += SettingsUpdateLockAlertContainer;
			_centerAlertContainerSetting.SettingChanged += SettingsUpdateCenterAlertContainer;
			_hideAlertsSetting.SettingChanged += SettingsUpdateHideAlerts;
			_hideDirectionsSetting.SettingChanged += SettingsUpdateHideDirections;
			_hideMarkersSetting.SettingChanged += SettingsUpdateHideMarkers;
			_alertSizeSetting.SettingChanged += SettingsUpdateAlertSize;
			_alertDisplayOrientationSetting.SettingChanged += SettingsUpdateAlertDisplayOrientation;
			_alertMoveDelaySetting.SettingChanged += SettingsUpdateAlertMoveDelay;
			_alertFadeDelaySetting.SettingChanged += SettingsUpdateAlertFadeDelay;
		}

		private void ResetActivatedEncounters()
		{
			_activeEncounters.Clear();
			foreach (Encounter enc in _encounters)
			{
				if (enc.Map == GameService.Gw2Mumble.CurrentMap.Id && enc.Enabled)
				{
					enc.Activate();
					_activeEncounters.Add(enc);
				}
				else
				{
					enc.Deactivate();
				}
			}
			_resetButton.Visible = _showResetTimerButton.Value && _activeEncounters.Count > 0;
		}

		private Encounter ParseEncounter(TimerStream timerStream)
		{
			string jsonContent;
			using (StreamReader jsonReader = new StreamReader(timerStream.Stream))
			{
				jsonContent = jsonReader.ReadToEnd();
			}
			Encounter enc = null;
			try
			{
				enc = JsonConvert.DeserializeObject<Encounter>(jsonContent, _jsonSettings);
				enc.Initialize(timerStream.ResourceManager);
			}
			catch (TimerReadException ex2)
			{
				enc.Description = ex2.Message;
				_errorCaught = true;
			}
			catch (Exception ex)
			{
				enc?.Dispose();
				Encounter encounter = new Encounter();
				encounter.Name = timerStream.FileName.Split('\\').Last();
				encounter.Description = "File Path: " + timerStream.FileName + "\n\nInvalid JSON format: " + ex.Message;
				enc = encounter;
				_errorCaught = true;
			}
			finally
			{
				enc.IsFromZip = timerStream.IsFromZip;
				enc.ZipFile = timerStream.ZipFile;
				enc.TimerFile = timerStream.FileName;
			}
			return enc;
		}

		private void AddEncounter(TimerStream timerStream)
		{
			Encounter enc = ParseEncounter(timerStream);
			AddEncounter(enc);
		}

		private void AddEncounter(Encounter enc)
		{
			if (enc.State != 0)
			{
				_encounters.Add(enc);
				_encounterIds.Add(enc.Id);
			}
			else
			{
				_invalidEncounters.Add(enc);
			}
		}

		private void UpdateEncounter(Encounter enc)
		{
			if (enc.State != 0)
			{
				_encounters.RemoveWhere((Encounter e) => e.Equals(enc));
			}
			else
			{
				_invalidEncounters.RemoveWhere((Encounter e) => e.Equals(enc));
			}
			AddEncounter(enc);
		}

		[Conditional("DEBUG")]
		private async void ShowLatestRelease()
		{
			GitHubClient github = new GitHubClient(new ProductHeaderValue("BlishHUD_Timers"));
			await github.Repository.Release.GetLatest("QuitarHero", "Hero-Timers");
			await github.Repository.Release.GetAll("QuitarHero", "Hero-Timers");
		}

		protected override async Task LoadAsync()
		{
			string timerDirectory = DirectoriesManager.GetFullDirectoryPath("timers");
			try
			{
				using (new WebClient())
				{
					List<Update> updates = JsonConvert.DeserializeObject<List<Update>>(new WebClient().DownloadString("https://bhm.blishhud.com/Charr.Timers_BlishHUD/timer_update.json"), _jsonSettings);
					if (updates == null || updates.Count == 0)
					{
						throw new ArgumentNullException();
					}
					update = updates[0];
					if (update.CreatedAt > _lastTimersUpdate.Value.CreatedAt)
					{
						_timersNeedUpdate = true;
						ScreenNotification.ShowNotification("New timers available. Go to settings to update!", ScreenNotification.NotificationType.Warning, null, 3);
					}
				}
			}
			catch (Exception)
			{
				_timersNeedUpdate = false;
			}
			if (!_timersNeedUpdate)
			{
				timerLoader = new TimerLoader(timerDirectory);
				timerLoader.LoadFiles(AddEncounter);
				_encountersLoaded = true;
			}
			_tabPanel = BuildSettingsPanel(GameService.Overlay.BlishHudWindow.ContentRegion);
			_onNewMapLoaded = delegate
			{
				ResetActivatedEncounters();
			};
			_resetButton = new ResetButton
			{
				Parent = GameService.Graphics.SpriteScreen,
				Size = _resetTimerButtonSizeSetting.Value,
				Location = _resetTimerButtonLocationSetting.Value,
				Visible = _showResetTimerButton.Value
			};
			_resetButton.ButtonClicked += _resetButton_ButtonClicked;
			_resetButton.BoundsChanged += _resetButton_BoundsChanged;
			ResetActivatedEncounters();
			SettingsUpdateHideAlerts();
			SettingsUpdateHideDirections();
			SettingsUpdateHideMarkers();
		}

		private void _resetButton_BoundsChanged(object sender, EventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			_resetTimerButtonLocationSetting.Value = _resetButton.Location;
			_resetTimerButtonSizeSetting.Value = _resetButton.Size;
		}

		private void _resetButton_ButtonClicked(object sender, EventArgs e)
		{
			ResetActiveEncounters();
		}

		private void ShowTimerEntries(Panel timerPanel)
		{
			foreach (Encounter enc3 in _encounters)
			{
				TimerDetails entry2 = new TimerDetails
				{
					Parent = timerPanel,
					Encounter = enc3
				};
				entry2.Initialize();
				entry2.PropertyChanged += delegate
				{
					ResetActivatedEncounters();
				};
				_allTimerDetails.Add(entry2);
				entry2.ReloadClicked += delegate(object sender, Encounter enc)
				{
					if (enc.IsFromZip)
					{
						timerLoader.ReloadFile(delegate(TimerStream timerStream)
						{
							Encounter encounter4 = ParseEncounter(timerStream);
							UpdateEncounter(encounter4);
							entry2.Encounter?.Dispose();
							entry2.Encounter = encounter4;
							ScreenNotification.ShowNotification("Encounter <" + encounter4.Name + "> reloaded!", ScreenNotification.NotificationType.Info, encounter4.Icon, 3);
						}, enc.ZipFile, enc.TimerFile);
					}
					else
					{
						timerLoader.ReloadFile(delegate(TimerStream timerStream)
						{
							Encounter encounter3 = ParseEncounter(timerStream);
							UpdateEncounter(encounter3);
							entry2.Encounter?.Dispose();
							entry2.Encounter = encounter3;
							ScreenNotification.ShowNotification("Encounter <" + encounter3.Name + "> reloaded!", ScreenNotification.NotificationType.Info, encounter3.Icon, 3);
						}, enc.TimerFile);
					}
				};
			}
			foreach (Encounter enc2 in _invalidEncounters)
			{
				TimerDetails entry = new TimerDetails
				{
					Parent = timerPanel,
					Encounter = enc2
				};
				entry.Initialize();
				entry.PropertyChanged += delegate
				{
					ResetActivatedEncounters();
				};
				_allTimerDetails.Add(entry);
				entry.ReloadClicked += delegate(object sender, Encounter enc)
				{
					if (enc.IsFromZip)
					{
						timerLoader.ReloadFile(delegate(TimerStream timerStream)
						{
							Encounter encounter2 = ParseEncounter(timerStream);
							UpdateEncounter(encounter2);
							entry.Encounter?.Dispose();
							entry.Encounter = encounter2;
							ScreenNotification.ShowNotification("Encounter <" + encounter2.Name + "> reloaded!", ScreenNotification.NotificationType.Info, encounter2.Icon, 3);
						}, enc.ZipFile, enc.TimerFile);
					}
					else
					{
						timerLoader.ReloadFile(delegate(TimerStream timerStream)
						{
							Encounter encounter = ParseEncounter(timerStream);
							UpdateEncounter(encounter);
							entry.Encounter?.Dispose();
							entry.Encounter = encounter;
							ScreenNotification.ShowNotification("Encounter <" + encounter.Name + "> reloaded!", ScreenNotification.NotificationType.Info, encounter.Icon, 3);
						}, enc.TimerFile);
					}
				};
			}
		}

		public void ShowCustomTimerCategories()
		{
			List<IGrouping<string, Encounter>> categories = (from enc in _encounters
				group enc by enc.Category).ToList();
			if (_sortCategorySetting.Value)
			{
				categories.Sort((IGrouping<string, Encounter> cat1, IGrouping<string, Encounter> cat2) => cat1.Key.CompareTo(cat2.Key));
			}
			foreach (IGrouping<string, Encounter> category in categories)
			{
				timerCategories.AddMenuItem(category.Key).Click += delegate
				{
					timerPanel.FilterChildren((TimerDetails db) => string.Equals(db.Encounter.Category, category.Key));
					_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => string.Equals(db.Encounter.Category, category.Key)).ToList();
				};
			}
		}

		private Panel BuildSettingsPanel(Rectangle panelBounds)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0618: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_0657: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			//IL_067d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_074b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0756: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07da: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_084f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0869: Unknown result type (might be due to invalid IL or missing references)
			//IL_0874: Unknown result type (might be due to invalid IL or missing references)
			//IL_08de: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0903: Unknown result type (might be due to invalid IL or missing references)
			//IL_096d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0987: Unknown result type (might be due to invalid IL or missing references)
			//IL_0992: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e51: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eaa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fbc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1063: Unknown result type (might be due to invalid IL or missing references)
			//IL_1079: Unknown result type (might be due to invalid IL or missing references)
			//IL_1084: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1163: Unknown result type (might be due to invalid IL or missing references)
			//IL_1175: Unknown result type (might be due to invalid IL or missing references)
			//IL_1198: Unknown result type (might be due to invalid IL or missing references)
			//IL_120f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1225: Unknown result type (might be due to invalid IL or missing references)
			//IL_1230: Unknown result type (might be due to invalid IL or missing references)
			//IL_1268: Unknown result type (might be due to invalid IL or missing references)
			//IL_130f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1321: Unknown result type (might be due to invalid IL or missing references)
			//IL_1344: Unknown result type (might be due to invalid IL or missing references)
			//IL_13da: Unknown result type (might be due to invalid IL or missing references)
			//IL_13e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1416: Unknown result type (might be due to invalid IL or missing references)
			//IL_141b: Unknown result type (might be due to invalid IL or missing references)
			//IL_141f: Unknown result type (might be due to invalid IL or missing references)
			Panel mainPanel = new Panel
			{
				CanScroll = false,
				Size = ((Rectangle)(ref panelBounds)).get_Size()
			};
			_alertContainer = new AlertContainer
			{
				Parent = GameService.Graphics.SpriteScreen,
				ControlPadding = new Vector2(10f, 5f),
				PadLeftBeforeControl = true,
				PadTopBeforeControl = true,
				BackgroundColor = new Color(Color.get_Black(), 0.3f),
				FlowDirection = _alertDisplayOrientationSetting.Value,
				LocationLock = _lockAlertContainerSetting.Value,
				Location = _alertContainerLocationSetting.Value
			};
			SettingsUpdateAlertSize();
			TextBox searchBox = new TextBox
			{
				Parent = mainPanel,
				Location = new Point(Dropdown.Standard.ControlOffset.X, Dropdown.Standard.ControlOffset.Y),
				PlaceholderText = "Search"
			};
			Panel menuSection = new Panel
			{
				Parent = mainPanel,
				Location = new Point(Panel.MenuStandard.PanelOffset.X, searchBox.Bottom + Panel.MenuStandard.ControlOffset.Y),
				Size = Panel.MenuStandard.Size - new Point(0, Panel.MenuStandard.ControlOffset.Y),
				Title = "Timer Categories",
				CanScroll = true,
				ShowBorder = true
			};
			timerPanel = new FlowPanel
			{
				Parent = mainPanel,
				Location = new Point(menuSection.Right + Panel.MenuStandard.ControlOffset.X, Panel.MenuStandard.ControlOffset.Y),
				FlowDirection = ControlFlowDirection.LeftToRight,
				ControlPadding = new Vector2(8f, 8f),
				CanScroll = true,
				ShowBorder = true
			};
			StandardButton obj = new StandardButton
			{
				Parent = mainPanel,
				Text = "Alert Settings"
			};
			StandardButton enableAllButton = new StandardButton
			{
				Parent = mainPanel,
				Text = "Enable All"
			};
			StandardButton disableAllButton = new StandardButton
			{
				Parent = mainPanel,
				Text = "Disable All"
			};
			timerPanel.Size = new Point(mainPanel.Right - menuSection.Right - Control.ControlStandard.ControlOffset.X, mainPanel.Height - enableAllButton.Height - Control.ControlStandard.ControlOffset.Y * 2);
			if (!Directory.EnumerateFiles(DirectoriesManager.GetFullDirectoryPath("timers")).Any() || _timersNeedUpdate)
			{
				Panel noTimersPanel = new Panel
				{
					Parent = mainPanel,
					Location = new Point(menuSection.Right + Panel.MenuStandard.ControlOffset.X, Panel.MenuStandard.ControlOffset.Y),
					ShowBorder = true,
					Size = timerPanel.Size
				};
				Blish_HUD.Controls.Label notice = new Blish_HUD.Controls.Label
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Bottom,
					Parent = noTimersPanel,
					Size = new Point(noTimersPanel.Width, noTimersPanel.Height / 2 - 64),
					ClipsBounds = false
				};
				if (_timersNeedUpdate)
				{
					notice.Text = "Your timers are outdated!\nDownload some now!";
				}
				else
				{
					notice.Text = "You don't have any timers!\nDownload some now!";
				}
				FlowPanel downloadPanel = new FlowPanel
				{
					Parent = noTimersPanel,
					FlowDirection = ControlFlowDirection.LeftToRight
				};
				downloadPanel.Resized += delegate
				{
					//IL_002f: Unknown result type (might be due to invalid IL or missing references)
					downloadPanel.Location = new Point(noTimersPanel.Width / 2 - downloadPanel.Width / 2, notice.Bottom + 24);
				};
				downloadPanel.Width = 196;
				StandardButton obj2 = new StandardButton
				{
					Text = "Download Hero's Timers",
					Parent = downloadPanel,
					Width = 196
				};
				StandardButton manualDownload = new StandardButton
				{
					Text = "Manual Download",
					Parent = downloadPanel,
					Width = 196
				};
				manualDownload.Visible = false;
				StandardButton openTimersFolder = new StandardButton
				{
					Text = "Open Timers Folder",
					Parent = noTimersPanel,
					Width = 196,
					Location = new Point(noTimersPanel.Width / 2 - 200, downloadPanel.Bottom + 4)
				};
				StandardButton skipUpdate = new StandardButton
				{
					Text = "Skip for now",
					Parent = noTimersPanel,
					Width = 196,
					Location = new Point(openTimersFolder.Right + 4, downloadPanel.Bottom + 4)
				};
				Blish_HUD.Controls.Label restartBlishHudAfter = new Blish_HUD.Controls.Label
				{
					Text = "Once done, restart this module or Blish HUD to enable them.",
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Top,
					Parent = noTimersPanel,
					AutoSizeHeight = true,
					Width = notice.Width,
					Top = skipUpdate.Bottom + 4
				};
				bool isDownloading = false;
				obj2.Click += async delegate
				{
					try
					{
						if (!isDownloading)
						{
							isDownloading = true;
							Uri downloadUrl = update.URL;
							using WebClient webClient = new WebClient();
							webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
							webClient.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
							webClient.DownloadFileAsync(downloadUrl, DirectoriesManager.GetFullDirectoryPath("timers") + "/" + update.name);
							restartBlishHudAfter.Text = "Downloading latest version of timers, please wait...";
							webClient.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs eventArgs)
							{
								if (File.Exists(DirectoriesManager.GetFullDirectoryPath("timers") + "/Hero-Timers.zip"))
								{
									File.Delete(DirectoriesManager.GetFullDirectoryPath("timers") + "/Hero-Timers.zip");
								}
								string[] files = Directory.GetFiles(DirectoriesManager.GetFullDirectoryPath("timers"), "*.bhtimer", SearchOption.AllDirectories);
								DirectoryReader directoryReader = new DirectoryReader(DirectoriesManager.GetFullDirectoryPath("timers"));
								PathableResourceManager pathableResourceManager = new PathableResourceManager(directoryReader);
								string[] array = files;
								foreach (string text in array)
								{
									if (Directory.GetLastWriteTimeUtc(text) <= DateTimeOffset.Parse("5/2/2022 5:28:21 PM +00:00"))
									{
										Encounter encounter = ParseEncounter(new TimerStream(directoryReader.GetFileStream(text), pathableResourceManager, text));
										if (encounter.Author == "QuitarHero.1645" && File.Exists(text))
										{
											File.Delete(text);
										}
										encounter.Dispose();
									}
								}
								directoryReader.Dispose();
								pathableResourceManager.Dispose();
								if (eventArgs.Error != null)
								{
									notice.Text = "Download failed: " + eventArgs.Error.Message;
									Logger.Error("Download failed: " + eventArgs.Error.Message);
									ScreenNotification.ShowNotification("Failed to download timers: " + eventArgs.Error.Message, ScreenNotification.NotificationType.Error, null, 3);
									restartBlishHudAfter.Text = "Wait and try downloading again\nOr manually download and place them in your timers folder.";
									downloadPanel.Width = 400;
									manualDownload.Visible = true;
									manualDownload.Click += delegate
									{
										Process.Start("https://github.com/QuitarHero/Hero-Timers/releases/latest/download/Hero.Timer.Pack.zip");
										_lastTimersUpdate.Value = update;
									};
									downloadPanel.RecalculateLayout();
								}
								else
								{
									notice.Text = "Your timers have been updated!";
									restartBlishHudAfter.Text = "Download complete, click Continue to enable them.";
									ScreenNotification.ShowNotification("Timers updated!", ScreenNotification.NotificationType.Info, null, 3);
									_lastTimersUpdate.Value = update;
									downloadPanel.Dispose();
									skipUpdate.Text = "Continue";
								}
								isDownloading = false;
							};
						}
					}
					catch (Exception)
					{
						ScreenNotification.ShowNotification("Failed to download timers: try again later...", ScreenNotification.NotificationType.Error, null, 3);
						isDownloading = false;
					}
				};
				openTimersFolder.Click += delegate
				{
					Process.Start("explorer.exe", "/open, \"" + DirectoriesManager.GetFullDirectoryPath("timers") + "\\\"");
				};
				skipUpdate.Click += delegate
				{
					if (!_encountersLoaded)
					{
						string fullDirectoryPath = DirectoriesManager.GetFullDirectoryPath("timers");
						timerLoader = new TimerLoader(fullDirectoryPath);
						timerLoader.LoadFiles(AddEncounter);
						_encountersLoaded = true;
						noTimersPanel.Dispose();
						ShowTimerEntries(timerPanel);
						ShowCustomTimerCategories();
					}
				};
			}
			searchBox.Width = menuSection.Width;
			searchBox.TextChanged += delegate
			{
				timerPanel.FilterChildren((TimerDetails db) => db.Text.ToLower().Contains(searchBox.Text.ToLower()));
			};
			obj.Location = new Point(menuSection.Right + Panel.MenuStandard.ControlOffset.X, timerPanel.Bottom + Control.ControlStandard.ControlOffset.Y);
			enableAllButton.Location = new Point(timerPanel.Right - enableAllButton.Width - disableAllButton.Width - Control.ControlStandard.ControlOffset.X * 2, timerPanel.Bottom + Control.ControlStandard.ControlOffset.Y);
			disableAllButton.Location = new Point(enableAllButton.Right + Control.ControlStandard.ControlOffset.X, timerPanel.Bottom + Control.ControlStandard.ControlOffset.Y);
			_alertSettingsWindow = new StandardWindow(Resources.AlertSettingsBackground, new Rectangle(24, 17, 500, 500), new Rectangle(40, 40, 480, 480))
			{
				Parent = GameService.Graphics.SpriteScreen,
				Title = "Alert Settings",
				Emblem = Resources.TextureTimerEmblem,
				SavesPosition = true,
				Id = "TimersAlertSettingsWindow"
			};
			_alertSettingsWindow.Hide();
			obj.Click += delegate
			{
				if (_alertSettingsWindow.Visible)
				{
					_alertSettingsWindow.Hide();
				}
				else
				{
					_alertSettingsWindow.Show();
				}
			};
			Checkbox lockAlertsWindowCB = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Lock Alerts Container",
				BasicTooltipText = "When enabled, the alerts container will be locked and cannot be moved.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, 0)
			};
			lockAlertsWindowCB.Checked = _lockAlertContainerSetting.Value;
			lockAlertsWindowCB.CheckedChanged += delegate
			{
				_lockAlertContainerSetting.Value = lockAlertsWindowCB.Checked;
			};
			Checkbox centerAlertsWindowCB = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Center Alerts Container",
				BasicTooltipText = "When enabled, the location of the alerts container will always be set to the center of the screen.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, lockAlertsWindowCB.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			centerAlertsWindowCB.Checked = _centerAlertContainerSetting.Value;
			centerAlertsWindowCB.CheckedChanged += delegate
			{
				_centerAlertContainerSetting.Value = centerAlertsWindowCB.Checked;
			};
			Checkbox hideAlertsCB = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Hide Alerts",
				BasicTooltipText = "When enabled, alerts on the screen will be hidden.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, centerAlertsWindowCB.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			hideAlertsCB.Checked = _hideAlertsSetting.Value;
			hideAlertsCB.CheckedChanged += delegate
			{
				_hideAlertsSetting.Value = hideAlertsCB.Checked;
			};
			Checkbox hideDirectionsCB = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Hide Directions",
				BasicTooltipText = "When enabled, directions on the screen will be hidden.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, hideAlertsCB.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			hideDirectionsCB.Checked = _hideDirectionsSetting.Value;
			hideDirectionsCB.CheckedChanged += delegate
			{
				_hideDirectionsSetting.Value = hideDirectionsCB.Checked;
			};
			Checkbox hideMarkersCB = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Hide Markers",
				BasicTooltipText = "When enabled, markers on the screen will be hidden.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, hideDirectionsCB.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			hideMarkersCB.Checked = _hideMarkersSetting.Value;
			hideMarkersCB.CheckedChanged += delegate
			{
				_hideMarkersSetting.Value = hideMarkersCB.Checked;
			};
			Checkbox hideResetTimersCB = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Hide Reset Timers Button",
				BasicTooltipText = "When enabled, the reset timers button will always be hidden.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, hideMarkersCB.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			hideResetTimersCB.Checked = !_showResetTimerButton.Value;
			hideResetTimersCB.CheckedChanged += delegate
			{
				_showResetTimerButton.Value = !hideResetTimersCB.Checked;
			};
			Checkbox hideSoundsCB = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Mute Text to Speech",
				BasicTooltipText = "When enabled, text to speech will be muted.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, hideResetTimersCB.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			hideSoundsCB.Checked = _hideSoundsSetting.Value;
			hideSoundsCB.CheckedChanged += delegate
			{
				_hideSoundsSetting.Value = hideSoundsCB.Checked;
			};
			Checkbox fillDirection = new Checkbox
			{
				Parent = _alertSettingsWindow,
				Text = "Invert Alert Fill",
				BasicTooltipText = "When enabled, alerts fill up as time passes.\nWhen disabled, alerts drain as time passes.",
				Location = new Point(Control.ControlStandard.ControlOffset.X, hideSoundsCB.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			fillDirection.Checked = _alertFillDirection.Value;
			fillDirection.CheckedChanged += delegate
			{
				_alertFillDirection.Value = fillDirection.Checked;
			};
			Blish_HUD.Controls.Label alertSizeLabel = new Blish_HUD.Controls.Label
			{
				Parent = _alertSettingsWindow,
				Text = "Alert Size",
				AutoSizeWidth = true,
				Location = new Point(Control.ControlStandard.ControlOffset.X, fillDirection.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			Dropdown alertSizeDropdown = new Dropdown
			{
				Parent = _alertSettingsWindow
			};
			alertSizeDropdown.Items.Add("Small");
			alertSizeDropdown.Items.Add("Medium");
			alertSizeDropdown.Items.Add("Large");
			alertSizeDropdown.Items.Add("BigWig Style");
			alertSizeDropdown.SelectedItem = _alertSizeSetting.Value.ToString();
			alertSizeDropdown.ValueChanged += delegate
			{
				_alertSizeSetting.Value = (AlertType)Enum.Parse(typeof(AlertType), alertSizeDropdown.SelectedItem.Replace(" ", ""), ignoreCase: true);
			};
			Blish_HUD.Controls.Label alertDisplayOrientationLabel = new Blish_HUD.Controls.Label
			{
				Parent = _alertSettingsWindow,
				Text = "Alert Display Orientation",
				AutoSizeWidth = true,
				Location = new Point(Control.ControlStandard.ControlOffset.X, alertSizeLabel.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			Dropdown alertDisplayOrientationDropdown = new Dropdown
			{
				Parent = _alertSettingsWindow,
				Location = new Point(alertDisplayOrientationLabel.Right + Control.ControlStandard.ControlOffset.X, alertDisplayOrientationLabel.Top)
			};
			alertDisplayOrientationDropdown.Items.Add("Left to Right");
			alertDisplayOrientationDropdown.Items.Add("Right to Left");
			alertDisplayOrientationDropdown.Items.Add("Top to Bottom");
			alertDisplayOrientationDropdown.Items.Add("Bottom to Top");
			switch (_alertDisplayOrientationSetting.Value)
			{
			case AlertFlowDirection.LeftToRight:
				alertDisplayOrientationDropdown.SelectedItem = "Left to Right";
				break;
			case AlertFlowDirection.RightToLeft:
				alertDisplayOrientationDropdown.SelectedItem = "Right to Left";
				break;
			case AlertFlowDirection.TopToBottom:
				alertDisplayOrientationDropdown.SelectedItem = "Top to Bottom";
				break;
			case AlertFlowDirection.BottomToTop:
				alertDisplayOrientationDropdown.SelectedItem = "Bottom to Top";
				break;
			}
			alertDisplayOrientationDropdown.ValueChanged += delegate
			{
				switch (alertDisplayOrientationDropdown.SelectedItem)
				{
				case "Left to Right":
					_alertDisplayOrientationSetting.Value = AlertFlowDirection.LeftToRight;
					break;
				case "Right to Left":
					_alertDisplayOrientationSetting.Value = AlertFlowDirection.RightToLeft;
					break;
				case "Top to Bottom":
					_alertDisplayOrientationSetting.Value = AlertFlowDirection.TopToBottom;
					break;
				case "Bottom to Top":
					_alertDisplayOrientationSetting.Value = AlertFlowDirection.BottomToTop;
					break;
				}
			};
			alertSizeDropdown.Location = new Point(alertDisplayOrientationDropdown.Left, alertSizeLabel.Top);
			new Blish_HUD.Controls.Label
			{
				Parent = _alertSettingsWindow,
				Text = "Alert Preview",
				AutoSizeWidth = true,
				Location = new Point(Control.ControlStandard.ControlOffset.X, alertDisplayOrientationDropdown.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			StandardButton addTestAlertButton = new StandardButton
			{
				Parent = _alertSettingsWindow,
				Text = "Add Test Alert",
				Location = new Point(alertDisplayOrientationDropdown.Left, alertDisplayOrientationDropdown.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			addTestAlertButton.Click += delegate
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				IAlertPanel alertPanel2;
				if (_alertSizeSetting.Value != AlertType.BigWigStyle)
				{
					IAlertPanel alertPanel = new AlertPanel
					{
						ControlPadding = new Vector2(10f, 10f),
						PadLeftBeforeControl = true,
						PadTopBeforeControl = true
					};
					alertPanel2 = alertPanel;
				}
				else
				{
					IAlertPanel alertPanel = new BigWigAlert();
					alertPanel2 = alertPanel;
				}
				IAlertPanel alertPanel3 = alertPanel2;
				alertPanel3.Text = "Test Alert " + (_testAlertPanels.Count + 1);
				alertPanel3.TextColor = Color.get_White();
				alertPanel3.Icon = Texture2DExtension.Duplicate(Resources.GetIcon("raid"));
				alertPanel3.MaxFill = 100f;
				alertPanel3.CurrentFill = (float)RandomUtil.GetRandom(0, 100) + (float)RandomUtil.GetRandom(0, 100) * 0.01f;
				alertPanel3.FillColor = Color.get_Red();
				alertPanel3.ShouldShow = !_hideAlertsSetting.Value;
				((Control)alertPanel3).Parent = _alertContainer;
				_testAlertPanels.Add(alertPanel3);
			};
			StandardButton clearTestAlertsButton = new StandardButton
			{
				Parent = _alertSettingsWindow,
				Text = "Clear Test Alerts",
				Location = new Point(addTestAlertButton.Right + Control.ControlStandard.ControlOffset.X, addTestAlertButton.Top)
			};
			clearTestAlertsButton.Width = (int)((double)clearTestAlertsButton.Width * 1.15);
			clearTestAlertsButton.Click += delegate
			{
				_alertContainer.ClearChildren();
				_testAlertPanels.ForEach(delegate(IAlertPanel panel)
				{
					panel.Dispose();
				});
				_testAlertPanels.Clear();
			};
			alertSizeDropdown.Width = addTestAlertButton.Width + clearTestAlertsButton.Width + Control.ControlStandard.ControlOffset.X;
			alertDisplayOrientationDropdown.Width = alertSizeDropdown.Width;
			Blish_HUD.Controls.Label alertContainerPositionLabel = new Blish_HUD.Controls.Label
			{
				Parent = _alertSettingsWindow,
				Text = "Alert Container Position",
				AutoSizeWidth = true,
				Location = new Point(Control.ControlStandard.ControlOffset.X, clearTestAlertsButton.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			StandardButton resetAlertContainerPositionButton = new StandardButton
			{
				Parent = _alertSettingsWindow,
				Text = "Reset Position",
				Location = new Point(addTestAlertButton.Left, alertContainerPositionLabel.Top)
			};
			resetAlertContainerPositionButton.Width = alertSizeDropdown.Width;
			resetAlertContainerPositionButton.Click += delegate
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				_alertContainerLocationSetting.Value = new Point(GameService.Graphics.SpriteScreen.Width / 2 - _alertContainer.Width / 2, GameService.Graphics.SpriteScreen.Height / 2 - _alertContainer.Height / 2);
			};
			Blish_HUD.Controls.Label alertMoveDelayLabel = new Blish_HUD.Controls.Label
			{
				Parent = _alertSettingsWindow,
				Text = "Alert Animation Duration",
				BasicTooltipText = "How many seconds alerts will take to reposition itself.",
				AutoSizeWidth = true,
				Location = new Point(Control.ControlStandard.ControlOffset.X, resetAlertContainerPositionButton.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			TextBox alertMoveDelayTextBox = new TextBox
			{
				Parent = _alertSettingsWindow,
				BasicTooltipText = "How many seconds alerts will take to reposition itself.",
				Location = new Point(resetAlertContainerPositionButton.Left, alertMoveDelayLabel.Top),
				Width = resetAlertContainerPositionButton.Width / 5,
				Height = alertMoveDelayLabel.Height,
				Text = $"{_alertMoveDelaySetting.Value:0.00}"
			};
			TrackBar alertMoveDelaySlider = new TrackBar
			{
				Parent = _alertSettingsWindow,
				BasicTooltipText = "How many seconds alerts will take to reposition itself.",
				MinValue = 0f,
				MaxValue = 3f,
				Value = _alertMoveDelaySetting.Value,
				SmallStep = true,
				Location = new Point(alertMoveDelayTextBox.Right + Control.ControlStandard.ControlOffset.X, alertMoveDelayLabel.Top),
				Width = resetAlertContainerPositionButton.Width - alertMoveDelayTextBox.Width - Control.ControlStandard.ControlOffset.X
			};
			alertMoveDelayTextBox.TextChanged += delegate
			{
				int cursorIndex2 = alertMoveDelayTextBox.CursorIndex;
				if (float.TryParse(alertMoveDelayTextBox.Text, out var result2) && result2 >= alertMoveDelaySlider.MinValue && result2 <= alertMoveDelaySlider.MaxValue)
				{
					result2 = (float)Math.Round(result2, 2);
					_alertMoveDelaySetting.Value = result2;
					alertMoveDelaySlider.Value = result2;
					alertMoveDelayTextBox.Text = $"{result2:0.00}";
				}
				else
				{
					alertMoveDelayTextBox.Text = $"{_alertMoveDelaySetting.Value:0.00}";
				}
				alertMoveDelayTextBox.CursorIndex = cursorIndex2;
			};
			alertMoveDelaySlider.ValueChanged += delegate
			{
				float num2 = (float)Math.Round(alertMoveDelaySlider.Value, 2);
				_alertMoveDelaySetting.Value = num2;
				alertMoveDelayTextBox.Text = $"{num2:0.00}";
			};
			Blish_HUD.Controls.Label alertFadeDelayLabel = new Blish_HUD.Controls.Label
			{
				Parent = _alertSettingsWindow,
				Text = "Alert Fade In/Out Delay",
				BasicTooltipText = "How many seconds alerts will take to appear/disappear.",
				AutoSizeWidth = true,
				Location = new Point(Control.ControlStandard.ControlOffset.X, alertMoveDelayLabel.Bottom + Control.ControlStandard.ControlOffset.Y)
			};
			TextBox alertFadeDelayTextBox = new TextBox
			{
				Parent = _alertSettingsWindow,
				BasicTooltipText = "How many seconds alerts will take to appear/disappear.",
				Location = new Point(resetAlertContainerPositionButton.Left, alertFadeDelayLabel.Top),
				Width = resetAlertContainerPositionButton.Width / 5,
				Height = alertFadeDelayLabel.Height,
				Text = $"{_alertFadeDelaySetting.Value:0.00}"
			};
			TrackBar alertFadeDelaySlider = new TrackBar
			{
				Parent = _alertSettingsWindow,
				BasicTooltipText = "How many seconds alerts will take to appear/disappear.",
				MinValue = 0f,
				MaxValue = 3f,
				Value = _alertFadeDelaySetting.Value,
				SmallStep = true,
				Location = new Point(alertFadeDelayTextBox.Right + Control.ControlStandard.ControlOffset.X, alertFadeDelayLabel.Top),
				Width = resetAlertContainerPositionButton.Width - alertFadeDelayTextBox.Width - Control.ControlStandard.ControlOffset.X
			};
			alertFadeDelayTextBox.TextChanged += delegate
			{
				int cursorIndex = alertFadeDelayTextBox.CursorIndex;
				if (float.TryParse(alertFadeDelayTextBox.Text, out var result) && result >= alertFadeDelaySlider.MinValue && result <= alertFadeDelaySlider.MaxValue)
				{
					result = (float)Math.Round(result, 2);
					_alertFadeDelaySetting.Value = result;
					alertFadeDelaySlider.Value = result;
					alertFadeDelayTextBox.Text = $"{result:0.00}";
				}
				else
				{
					alertFadeDelayTextBox.Text = $"{_alertFadeDelaySetting.Value:0.00}";
				}
				alertFadeDelayTextBox.CursorIndex = cursorIndex;
			};
			alertFadeDelaySlider.ValueChanged += delegate
			{
				float num = (float)Math.Round(alertFadeDelaySlider.Value, 2);
				_alertFadeDelaySetting.Value = num;
				alertFadeDelayTextBox.Text = $"{num:0.00}";
			};
			StandardButton closeAlertSettingsButton = new StandardButton
			{
				Parent = _alertSettingsWindow,
				Text = "Close"
			};
			closeAlertSettingsButton.Location = new Point((_alertSettingsWindow.Left + _alertSettingsWindow.Right) / 2 - closeAlertSettingsButton.Width / 2, alertFadeDelayTextBox.Bottom + Control.ControlStandard.ControlOffset.Y);
			closeAlertSettingsButton.Click += delegate
			{
				_alertSettingsWindow.Hide();
			};
			ShowTimerEntries(timerPanel);
			Menu menu = new Menu();
			Rectangle contentRegion = menuSection.ContentRegion;
			menu.Size = ((Rectangle)(ref contentRegion)).get_Size();
			menu.MenuItemHeight = 40;
			menu.Parent = menuSection;
			menu.CanSelect = true;
			timerCategories = menu;
			MenuItem menuItem = timerCategories.AddMenuItem("All Timers");
			menuItem.Select();
			_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => true).ToList();
			menuItem.Click += delegate
			{
				timerPanel.FilterChildren((TimerDetails db) => true);
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => true).ToList();
			};
			MenuItem enabledTimers = timerCategories.AddMenuItem("Enabled Timers");
			enabledTimers.Click += delegate
			{
				timerPanel.FilterChildren((TimerDetails db) => db.Encounter.Enabled);
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => db.Encounter.Enabled).ToList();
			};
			timerCategories.AddMenuItem("Current Map").Click += delegate
			{
				timerPanel.FilterChildren((TimerDetails db) => db.Encounter.Map == GameService.Gw2Mumble.CurrentMap.Id);
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => db.Encounter.Map == GameService.Gw2Mumble.CurrentMap.Id).ToList();
			};
			timerCategories.AddMenuItem("Invalid Timers").Click += delegate
			{
				timerPanel.FilterChildren((TimerDetails db) => db.Encounter.State == Encounter.EncounterStates.Error);
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => db.Encounter.State == Encounter.EncounterStates.Error).ToList();
			};
			ShowCustomTimerCategories();
			enableAllButton.Click += delegate
			{
				if (timerCategories.SelectedMenuItem != enabledTimers)
				{
					_displayedTimerDetails.ForEach(delegate(TimerDetails db)
					{
						if (db.Encounter.State != 0)
						{
							db.Enabled = true;
						}
					});
				}
			};
			disableAllButton.Click += delegate
			{
				_displayedTimerDetails.ForEach(delegate(TimerDetails db)
				{
					if (db.Encounter.State != 0)
					{
						db.Enabled = false;
					}
				});
				if (timerCategories.SelectedMenuItem == enabledTimers)
				{
					timerPanel.FilterChildren((TimerDetails db) => db.Enabled);
				}
			};
			return mainPanel;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			GameService.Gw2Mumble.CurrentMap.MapChanged += _onNewMapLoaded;
			_timersTab = GameService.Overlay.BlishHudWindow.AddTab("Timers", ContentsManager.GetTexture("textures\\155035small.png"), _tabPanel);
			base.OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			if (_encountersLoaded)
			{
				foreach (Encounter activeEncounter in _activeEncounters)
				{
					activeEncounter.Update(gameTime);
				}
			}
			if (_debugText.Visible)
			{
				_debugText.Text = "Debug: " + GameService.Gw2Mumble.PlayerCharacter.Position.X.ToString("0.0") + " " + GameService.Gw2Mumble.PlayerCharacter.Position.Y.ToString("0.0") + " " + GameService.Gw2Mumble.PlayerCharacter.Position.Z.ToString("0.0") + " ";
			}
			if (_centerAlertContainerSetting.Value)
			{
				_alertContainer.Location = new Point(GameService.Graphics.SpriteScreen.Width / 2 - _alertContainer.Width / 2, _alertContainer.Location.Y);
			}
		}

		protected override void Unload()
		{
			_debugText?.Dispose();
			timerLoader?.Dispose();
			GameService.Gw2Mumble.CurrentMap.MapChanged -= _onNewMapLoaded;
			_showDebugSetting.SettingChanged -= SettingsUpdateShowDebug;
			_lockAlertContainerSetting.SettingChanged -= SettingsUpdateLockAlertContainer;
			_centerAlertContainerSetting.SettingChanged -= SettingsUpdateCenterAlertContainer;
			_hideAlertsSetting.SettingChanged -= SettingsUpdateHideAlerts;
			_hideDirectionsSetting.SettingChanged -= SettingsUpdateHideDirections;
			_hideMarkersSetting.SettingChanged -= SettingsUpdateHideMarkers;
			_alertSizeSetting.SettingChanged -= SettingsUpdateAlertSize;
			_alertDisplayOrientationSetting.SettingChanged -= SettingsUpdateAlertDisplayOrientation;
			_alertMoveDelaySetting.SettingChanged -= SettingsUpdateAlertMoveDelay;
			_alertFadeDelaySetting.SettingChanged -= SettingsUpdateAlertFadeDelay;
			_resetButton?.Dispose();
			GameService.Overlay.BlishHudWindow.RemoveTab(_timersTab);
			_tabPanel?.Dispose();
			_allTimerDetails?.ForEach(delegate(TimerDetails de)
			{
				de.Dispose();
			});
			_allTimerDetails?.Clear();
			_alertContainer?.Dispose();
			_alertSettingsWindow?.Dispose();
			_testAlertPanels?.ForEach(delegate(IAlertPanel panel)
			{
				panel.Dispose();
			});
			_encounterEnableSettings?.Clear();
			_activeAlertIds?.Clear();
			_activeDirectionIds?.Clear();
			_activeMarkerIds?.Clear();
			_encounterIds?.Clear();
			foreach (Encounter encounter in _encounters)
			{
				encounter.Dispose();
			}
			_encounters?.Clear();
			_activeEncounters?.Clear();
			foreach (Encounter invalidEncounter in _invalidEncounters)
			{
				invalidEncounter.Dispose();
			}
			_invalidEncounters?.Clear();
			_pathableResourceManagers?.ForEach(delegate(PathableResourceManager m)
			{
				m.Dispose();
			});
			_pathableResourceManagers?.Clear();
			ModuleInstance = null;
		}
	}
}
