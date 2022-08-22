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
using Microsoft.Xna.Framework.Graphics;
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

		private Label _debugText;

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

		public SettingEntry<float> _alertMoveDelaySetting;

		public SettingEntry<float> _alertFadeDelaySetting;

		public SettingEntry<bool> _alertFillDirection;

		public Update update = new Update();

		public TimerLoader timerLoader;

		private bool _timersNeedUpdate;

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
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			if (!settings.TryGetSetting<Update>("LastTimersUpdate", ref _lastTimersUpdate))
			{
				_lastTimersUpdate = settings.DefineSetting<Update>("LastTimersUpdate", new Update(), "Last Timers Update", "Date of last timers update", (SettingTypeRendererDelegate)null);
			}
			_showDebugSetting = settings.DefineSetting<bool>("ShowDebugText", false, "Show Debug Text", "For creating timers. Placed in top-left corner. Displays location status.", (SettingTypeRendererDelegate)null);
			_debugModeSetting = settings.DefineSetting<bool>("DebugMode", false, "Enable Debug Mode", "All Timer triggers will ignore requireCombat setting, allowing you to test them freely.", (SettingTypeRendererDelegate)null);
			_sortCategorySetting = settings.DefineSetting<bool>("SortByCategory", false, "Sort Categories", "When enabled, categories from loaded timer files are sorted in alphanumerical order.\nOtherwise, categories are in the order they are loaded.\nThe module needs to be restarted to take effect.", (SettingTypeRendererDelegate)null);
			_timerSettingCollection = settings.AddSubCollection("EnabledTimers", false);
			_showResetTimerButton = settings.DefineSetting<bool>("Show Reset Button", true, (Func<string>)null, (Func<string>)null);
			_showResetTimerButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)_showResetTimerButton_SettingChanged);
			_resetTimerHotKeySetting = settings.DefineSetting<KeyBinding>("Reset Active Timer", new KeyBinding(), (Func<string>)null, (Func<string>)null);
			_resetTimerHotKeySetting.get_Value().set_Enabled(true);
			_resetTimerHotKeySetting.get_Value().add_Activated((EventHandler<EventArgs>)ResetHotkey_Activated);
			SettingCollection _internalSettings = settings.AddSubCollection("Internal Settings", false);
			_resetTimerButtonLocationSetting = _internalSettings.DefineSetting<Point>("Reset Timer Button Location", new Point(150, 150), (Func<string>)null, (Func<string>)null);
			_resetTimerButtonSizeSetting = _internalSettings.DefineSetting<Point>("Reset Timer Button Size", new Point(64, 64), (Func<string>)null, (Func<string>)null);
			_keyBindSettings = new SettingEntry<KeyBinding>[5];
			for (int i = 0; i < 5; i++)
			{
				_keyBindSettings[i] = settings.DefineSetting<KeyBinding>("Trigger Key " + i, new KeyBinding(), "Trigger Key " + i, "For timers that require keys to trigger.", (SettingTypeRendererDelegate)null);
			}
			_alertSettingCollection = settings.AddSubCollection("AlertSetting", false);
			_lockAlertContainerSetting = _alertSettingCollection.DefineSetting<bool>("LockAlertContainer", false, (Func<string>)null, (Func<string>)null);
			_hideAlertsSetting = _alertSettingCollection.DefineSetting<bool>("HideAlerts", false, (Func<string>)null, (Func<string>)null);
			_hideDirectionsSetting = _alertSettingCollection.DefineSetting<bool>("HideDirections", false, (Func<string>)null, (Func<string>)null);
			_hideMarkersSetting = _alertSettingCollection.DefineSetting<bool>("HideMarkers", false, (Func<string>)null, (Func<string>)null);
			_hideSoundsSetting = _alertSettingCollection.DefineSetting<bool>("HideSounds", false, (Func<string>)null, (Func<string>)null);
			_centerAlertContainerSetting = _alertSettingCollection.DefineSetting<bool>("CenterAlertContainer", true, (Func<string>)null, (Func<string>)null);
			_alertSizeSetting = _alertSettingCollection.DefineSetting<AlertType>("AlertSize", AlertType.BigWigStyle, (Func<string>)null, (Func<string>)null);
			_alertDisplayOrientationSetting = _alertSettingCollection.DefineSetting<AlertFlowDirection>("AlertDisplayOrientation", AlertFlowDirection.TopToBottom, (Func<string>)null, (Func<string>)null);
			_alertContainerLocationSetting = _alertSettingCollection.DefineSetting<Point>("AlertContainerLocation", new Point(GameService.Graphics.get_WindowWidth() - GameService.Graphics.get_WindowWidth() / 4, GameService.Graphics.get_WindowHeight() / 2), (Func<string>)null, (Func<string>)null);
			_alertMoveDelaySetting = _alertSettingCollection.DefineSetting<float>("AlertMoveSpeed", 0.75f, (Func<string>)null, (Func<string>)null);
			_alertFadeDelaySetting = _alertSettingCollection.DefineSetting<float>("AlertFadeSpeed", 1f, (Func<string>)null, (Func<string>)null);
			_alertFillDirection = _alertSettingCollection.DefineSetting<bool>("FillDirection", true, (Func<string>)null, (Func<string>)null);
		}

		private void _showResetTimerButton_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			_resetButton?.ToggleVisibility();
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
			foreach (Encounter encounter in _encounters)
			{
				encounter.ShowAlerts = !_hideAlertsSetting.get_Value();
			}
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
				AlertContainer alertContainer2 = _alertContainer;
				if (alertContainer2 != null)
				{
					((Control)alertContainer2).Show();
				}
			}
		}

		private void SettingsUpdateHideDirections(object sender = null, EventArgs e = null)
		{
			foreach (Encounter encounter in _encounters)
			{
				encounter.ShowDirections = !_hideDirectionsSetting.get_Value();
			}
		}

		private void SettingsUpdateHideMarkers(object sender = null, EventArgs e = null)
		{
			foreach (Encounter encounter in _encounters)
			{
				encounter.ShowMarkers = !_hideMarkersSetting.get_Value();
			}
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
			_alertContainer.FlowDirection = _alertDisplayOrientationSetting.get_Value();
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
			_encounters = new HashSet<Encounter>();
			_activeEncounters = new HashSet<Encounter>();
			_invalidEncounters = new HashSet<Encounter>();
			_allTimerDetails = new List<TimerDetails>();
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
			_alertDisplayOrientationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<AlertFlowDirection>>)SettingsUpdateAlertDisplayOrientation);
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
					enc.Activate();
					_activeEncounters.Add(enc);
				}
				else
				{
					enc.Deactivate();
				}
			}
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
			await github.get_Repository().get_Release().GetLatest("QuitarHero", "Hero-Timers");
			await github.get_Repository().get_Release().GetAll("QuitarHero", "Hero-Timers");
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
					if (update.CreatedAt > _lastTimersUpdate.get_Value().CreatedAt)
					{
						_timersNeedUpdate = true;
						ScreenNotification.ShowNotification("New timers available. Go to settings to update!", (NotificationType)1, (Texture2D)null, 3);
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
			_tabPanel = BuildSettingsPanel(((Container)GameService.Overlay.get_BlishHudWindow()).get_ContentRegion());
			_onNewMapLoaded = delegate
			{
				ResetActivatedEncounters();
			};
			ResetButton resetButton = new ResetButton();
			((Control)resetButton).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)resetButton).set_Size(_resetTimerButtonSizeSetting.get_Value());
			((Control)resetButton).set_Location(_resetTimerButtonLocationSetting.get_Value());
			((Control)resetButton).set_Visible(_showResetTimerButton.get_Value());
			_resetButton = resetButton;
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
			_resetTimerButtonLocationSetting.set_Value(((Control)_resetButton).get_Location());
			_resetTimerButtonSizeSetting.set_Value(((Control)_resetButton).get_Size());
		}

		private void _resetButton_ButtonClicked(object sender, EventArgs e)
		{
			ResetActiveEncounters();
		}

		private void ShowTimerEntries(Panel timerPanel)
		{
			foreach (Encounter enc3 in _encounters)
			{
				TimerDetails timerDetails = new TimerDetails();
				((Control)timerDetails).set_Parent((Container)(object)timerPanel);
				timerDetails.Encounter = enc3;
				TimerDetails entry2 = timerDetails;
				entry2.Initialize();
				((Control)entry2).add_PropertyChanged((PropertyChangedEventHandler)delegate
				{
					ResetActivatedEncounters();
				});
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
							ScreenNotification.ShowNotification("Encounter <" + encounter4.Name + "> reloaded!", (NotificationType)0, AsyncTexture2D.op_Implicit(encounter4.Icon), 3);
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
							ScreenNotification.ShowNotification("Encounter <" + encounter3.Name + "> reloaded!", (NotificationType)0, AsyncTexture2D.op_Implicit(encounter3.Icon), 3);
						}, enc.TimerFile);
					}
				};
			}
			foreach (Encounter enc2 in _invalidEncounters)
			{
				TimerDetails timerDetails2 = new TimerDetails();
				((Control)timerDetails2).set_Parent((Container)(object)timerPanel);
				timerDetails2.Encounter = enc2;
				TimerDetails entry = timerDetails2;
				entry.Initialize();
				((Control)entry).add_PropertyChanged((PropertyChangedEventHandler)delegate
				{
					ResetActivatedEncounters();
				});
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
							ScreenNotification.ShowNotification("Encounter <" + encounter2.Name + "> reloaded!", (NotificationType)0, AsyncTexture2D.op_Implicit(encounter2.Icon), 3);
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
							ScreenNotification.ShowNotification("Encounter <" + encounter.Name + "> reloaded!", (NotificationType)0, AsyncTexture2D.op_Implicit(encounter.Icon), 3);
						}, enc.TimerFile);
					}
				};
			}
		}

		public void ShowCustomTimerCategories()
		{
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
					timerPanel.FilterChildren<TimerDetails>((Func<TimerDetails, bool>)((TimerDetails db) => string.Equals(db.Encounter.Category, category.Key)));
					_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => string.Equals(db.Encounter.Category, category.Key)).ToList();
				});
			}
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
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Expected O, but got Unknown
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected O, but got Unknown
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Expected O, but got Unknown
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Expected O, but got Unknown
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Expected O, but got Unknown
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Expected O, but got Unknown
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Expected O, but got Unknown
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Expected O, but got Unknown
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Expected O, but got Unknown
			//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_062c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_0691: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_0715: Expected O, but got Unknown
			//IL_0732: Unknown result type (might be due to invalid IL or missing references)
			//IL_0737: Unknown result type (might be due to invalid IL or missing references)
			//IL_0743: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0759: Unknown result type (might be due to invalid IL or missing references)
			//IL_075f: Unknown result type (might be due to invalid IL or missing references)
			//IL_076a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0779: Expected O, but got Unknown
			//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0808: Expected O, but got Unknown
			//IL_0836: Unknown result type (might be due to invalid IL or missing references)
			//IL_083b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_0852: Unknown result type (might be due to invalid IL or missing references)
			//IL_085d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0863: Unknown result type (might be due to invalid IL or missing references)
			//IL_087d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0888: Unknown result type (might be due to invalid IL or missing references)
			//IL_0897: Expected O, but got Unknown
			//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_090c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0917: Unknown result type (might be due to invalid IL or missing references)
			//IL_0926: Expected O, but got Unknown
			//IL_0954: Unknown result type (might be due to invalid IL or missing references)
			//IL_0959: Unknown result type (might be due to invalid IL or missing references)
			//IL_0965: Unknown result type (might be due to invalid IL or missing references)
			//IL_0970: Unknown result type (might be due to invalid IL or missing references)
			//IL_097b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0981: Unknown result type (might be due to invalid IL or missing references)
			//IL_099b: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b5: Expected O, but got Unknown
			//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a44: Expected O, but got Unknown
			//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad3: Expected O, but got Unknown
			//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b11: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b5a: Expected O, but got Unknown
			//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b60: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b71: Expected O, but got Unknown
			//IL_0c01: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c57: Expected O, but got Unknown
			//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c88: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c97: Expected O, but got Unknown
			//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d95: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0db9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dde: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dfa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e05: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e38: Expected O, but got Unknown
			//IL_0e4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e67: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e92: Expected O, but got Unknown
			//IL_0ed9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f22: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f55: Expected O, but got Unknown
			//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f71: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f8c: Expected O, but got Unknown
			//IL_0fb1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fcd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fdf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1006: Unknown result type (might be due to invalid IL or missing references)
			//IL_1012: Expected O, but got Unknown
			//IL_1013: Unknown result type (might be due to invalid IL or missing references)
			//IL_1018: Unknown result type (might be due to invalid IL or missing references)
			//IL_1024: Unknown result type (might be due to invalid IL or missing references)
			//IL_102f: Unknown result type (might be due to invalid IL or missing references)
			//IL_103e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1048: Unknown result type (might be due to invalid IL or missing references)
			//IL_1057: Unknown result type (might be due to invalid IL or missing references)
			//IL_1064: Unknown result type (might be due to invalid IL or missing references)
			//IL_1089: Expected O, but got Unknown
			//IL_108a: Unknown result type (might be due to invalid IL or missing references)
			//IL_108f: Unknown result type (might be due to invalid IL or missing references)
			//IL_109b: Unknown result type (might be due to invalid IL or missing references)
			//IL_10a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_10cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_10f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1101: Unknown result type (might be due to invalid IL or missing references)
			//IL_111a: Unknown result type (might be due to invalid IL or missing references)
			//IL_112f: Expected O, but got Unknown
			//IL_115d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1162: Unknown result type (might be due to invalid IL or missing references)
			//IL_116e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1179: Unknown result type (might be due to invalid IL or missing references)
			//IL_1184: Unknown result type (might be due to invalid IL or missing references)
			//IL_118b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1191: Unknown result type (might be due to invalid IL or missing references)
			//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_11b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_11be: Expected O, but got Unknown
			//IL_11bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_11db: Unknown result type (might be due to invalid IL or missing references)
			//IL_11ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1203: Unknown result type (might be due to invalid IL or missing references)
			//IL_1210: Unknown result type (might be due to invalid IL or missing references)
			//IL_1235: Expected O, but got Unknown
			//IL_1236: Unknown result type (might be due to invalid IL or missing references)
			//IL_123b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1247: Unknown result type (might be due to invalid IL or missing references)
			//IL_1252: Unknown result type (might be due to invalid IL or missing references)
			//IL_125d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1268: Unknown result type (might be due to invalid IL or missing references)
			//IL_1279: Unknown result type (might be due to invalid IL or missing references)
			//IL_1280: Unknown result type (might be due to invalid IL or missing references)
			//IL_1291: Unknown result type (might be due to invalid IL or missing references)
			//IL_12a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12db: Expected O, but got Unknown
			//IL_1309: Unknown result type (might be due to invalid IL or missing references)
			//IL_130e: Unknown result type (might be due to invalid IL or missing references)
			//IL_131a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1327: Expected O, but got Unknown
			//IL_135c: Unknown result type (might be due to invalid IL or missing references)
			//IL_136f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1399: Unknown result type (might be due to invalid IL or missing references)
			//IL_139e: Unknown result type (might be due to invalid IL or missing references)
			//IL_13a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_13a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_13a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_13b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_13bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_13c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ce: Expected O, but got Unknown
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
			timerPanel = val4;
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
			if (!Directory.EnumerateFiles(DirectoriesManager.GetFullDirectoryPath("timers")).Any() || _timersNeedUpdate)
			{
				Panel val8 = new Panel();
				((Control)val8).set_Parent((Container)(object)mainPanel);
				((Control)val8).set_Location(new Point(((Control)menuSection).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
				val8.set_ShowBorder(true);
				((Control)val8).set_Size(((Control)timerPanel).get_Size());
				Panel noTimersPanel = val8;
				Label val9 = new Label();
				val9.set_HorizontalAlignment((HorizontalAlignment)1);
				val9.set_VerticalAlignment((VerticalAlignment)2);
				((Control)val9).set_Parent((Container)(object)noTimersPanel);
				((Control)val9).set_Size(new Point(((Control)noTimersPanel).get_Width(), ((Control)noTimersPanel).get_Height() / 2 - 64));
				((Control)val9).set_ClipsBounds(false);
				Label notice = val9;
				if (_timersNeedUpdate)
				{
					notice.set_Text("Your timers are outdated!\nDownload some now!");
				}
				else
				{
					notice.set_Text("You don't have any timers!\nDownload some now!");
				}
				FlowPanel val10 = new FlowPanel();
				((Control)val10).set_Parent((Container)(object)noTimersPanel);
				val10.set_FlowDirection((ControlFlowDirection)0);
				FlowPanel downloadPanel = val10;
				((Control)downloadPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
				{
					//IL_002f: Unknown result type (might be due to invalid IL or missing references)
					((Control)downloadPanel).set_Location(new Point(((Control)noTimersPanel).get_Width() / 2 - ((Control)downloadPanel).get_Width() / 2, ((Control)notice).get_Bottom() + 24));
				});
				((Control)downloadPanel).set_Width(196);
				StandardButton val11 = new StandardButton();
				val11.set_Text("Download Hero's Timers");
				((Control)val11).set_Parent((Container)(object)downloadPanel);
				((Control)val11).set_Width(196);
				StandardButton val12 = new StandardButton();
				val12.set_Text("Manual Download");
				((Control)val12).set_Parent((Container)(object)downloadPanel);
				((Control)val12).set_Width(196);
				StandardButton manualDownload = val12;
				((Control)manualDownload).set_Visible(false);
				StandardButton val13 = new StandardButton();
				val13.set_Text("Open Timers Folder");
				((Control)val13).set_Parent((Container)(object)noTimersPanel);
				((Control)val13).set_Width(196);
				((Control)val13).set_Location(new Point(((Control)noTimersPanel).get_Width() / 2 - 200, ((Control)downloadPanel).get_Bottom() + 4));
				StandardButton openTimersFolder = val13;
				StandardButton val14 = new StandardButton();
				val14.set_Text("Skip for now");
				((Control)val14).set_Parent((Container)(object)noTimersPanel);
				((Control)val14).set_Width(196);
				((Control)val14).set_Location(new Point(((Control)openTimersFolder).get_Right() + 4, ((Control)downloadPanel).get_Bottom() + 4));
				StandardButton skipUpdate = val14;
				Label val15 = new Label();
				val15.set_Text("Once done, restart this module or Blish HUD to enable them.");
				val15.set_HorizontalAlignment((HorizontalAlignment)1);
				val15.set_VerticalAlignment((VerticalAlignment)0);
				((Control)val15).set_Parent((Container)(object)noTimersPanel);
				val15.set_AutoSizeHeight(true);
				((Control)val15).set_Width(((Control)notice).get_Width());
				((Control)val15).set_Top(((Control)skipUpdate).get_Bottom() + 4);
				Label restartBlishHudAfter = val15;
				bool isDownloading = false;
				((Control)val11).add_Click((EventHandler<MouseEventArgs>)async delegate
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
							restartBlishHudAfter.set_Text("Downloading latest version of timers, please wait...");
							webClient.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs eventArgs)
							{
								//IL_0093: Unknown result type (might be due to invalid IL or missing references)
								//IL_0099: Expected O, but got Unknown
								if (File.Exists(DirectoriesManager.GetFullDirectoryPath("timers") + "/Hero-Timers.zip"))
								{
									File.Delete(DirectoriesManager.GetFullDirectoryPath("timers") + "/Hero-Timers.zip");
								}
								string[] files = Directory.GetFiles(DirectoriesManager.GetFullDirectoryPath("timers"), "*.bhtimer", SearchOption.AllDirectories);
								DirectoryReader val41 = new DirectoryReader(DirectoriesManager.GetFullDirectoryPath("timers"));
								PathableResourceManager pathableResourceManager = new PathableResourceManager((IDataReader)(object)val41);
								string[] array = files;
								foreach (string text in array)
								{
									if (Directory.GetLastWriteTimeUtc(text) <= DateTimeOffset.Parse("5/2/2022 5:28:21 PM +00:00"))
									{
										Encounter encounter = ParseEncounter(new TimerStream(val41.GetFileStream(text), pathableResourceManager, text));
										if (encounter.Author == "QuitarHero.1645" && File.Exists(text))
										{
											File.Delete(text);
										}
										encounter.Dispose();
									}
								}
								val41.Dispose();
								pathableResourceManager.Dispose();
								if (eventArgs.Error != null)
								{
									notice.set_Text("Download failed: " + eventArgs.Error.Message);
									Logger.Error("Download failed: " + eventArgs.Error.Message);
									ScreenNotification.ShowNotification("Failed to download timers: " + eventArgs.Error.Message, (NotificationType)2, (Texture2D)null, 3);
									restartBlishHudAfter.set_Text("Wait and try downloading again\nOr manually download and place them in your timers folder.");
									((Control)downloadPanel).set_Width(400);
									((Control)manualDownload).set_Visible(true);
									((Control)manualDownload).add_Click((EventHandler<MouseEventArgs>)delegate
									{
										Process.Start("https://github.com/QuitarHero/Hero-Timers/releases/latest/download/Hero.Timer.Pack.zip");
										_lastTimersUpdate.set_Value(update);
									});
									((Control)downloadPanel).RecalculateLayout();
								}
								else
								{
									notice.set_Text("Your timers have been updated!");
									restartBlishHudAfter.set_Text("Download complete, click Continue to enable them.");
									ScreenNotification.ShowNotification("Timers updated!", (NotificationType)0, (Texture2D)null, 3);
									_lastTimersUpdate.set_Value(update);
									((Control)downloadPanel).Dispose();
									skipUpdate.set_Text("Continue");
								}
								isDownloading = false;
							};
						}
					}
					catch (Exception)
					{
						ScreenNotification.ShowNotification("Failed to download timers: try again later...", (NotificationType)2, (Texture2D)null, 3);
						isDownloading = false;
					}
				});
				((Control)openTimersFolder).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start("explorer.exe", "/open, \"" + DirectoriesManager.GetFullDirectoryPath("timers") + "\\\"");
				});
				((Control)skipUpdate).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (!_encountersLoaded)
					{
						string fullDirectoryPath = DirectoriesManager.GetFullDirectoryPath("timers");
						timerLoader = new TimerLoader(fullDirectoryPath);
						timerLoader.LoadFiles(AddEncounter);
						_encountersLoaded = true;
						((Control)noTimersPanel).Dispose();
						ShowTimerEntries((Panel)(object)timerPanel);
						ShowCustomTimerCategories();
					}
				});
			}
			((Control)searchBox).set_Width(((Control)menuSection).get_Width());
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetails>((Func<TimerDetails, bool>)((TimerDetails db) => ((DetailsButton)db).get_Text().ToLower().Contains(((TextInputBase)searchBox).get_Text().ToLower())));
			});
			((Control)val5).set_Location(new Point(((Control)menuSection).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			((Control)enableAllButton).set_Location(new Point(((Control)timerPanel).get_Right() - ((Control)enableAllButton).get_Width() - ((Control)disableAllButton).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X * 2, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			((Control)disableAllButton).set_Location(new Point(((Control)enableAllButton).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)timerPanel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardWindow val16 = new StandardWindow(Resources.AlertSettingsBackground, new Rectangle(24, 17, 505, 390), new Rectangle(38, 38, 472, 350));
			((Control)val16).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val16).set_Title("Alert Settings");
			((WindowBase2)val16).set_Emblem(Resources.TextureTimerEmblem);
			((WindowBase2)val16).set_SavesPosition(true);
			((WindowBase2)val16).set_Id("TimersAlertSettingsWindow");
			_alertSettingsWindow = val16;
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
			Checkbox val17 = new Checkbox();
			((Control)val17).set_Parent((Container)(object)_alertSettingsWindow);
			val17.set_Text("Lock Alerts Container");
			((Control)val17).set_BasicTooltipText("When enabled, the alerts container will be locked and cannot be moved.");
			((Control)val17).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0));
			Checkbox lockAlertsWindowCB = val17;
			lockAlertsWindowCB.set_Checked(_lockAlertContainerSetting.get_Value());
			lockAlertsWindowCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_lockAlertContainerSetting.set_Value(lockAlertsWindowCB.get_Checked());
			});
			Checkbox val18 = new Checkbox();
			((Control)val18).set_Parent((Container)(object)_alertSettingsWindow);
			val18.set_Text("Center Alerts Container");
			((Control)val18).set_BasicTooltipText("When enabled, the location of the alerts container will always be set to the center of the screen.");
			((Control)val18).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)lockAlertsWindowCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox centerAlertsWindowCB = val18;
			centerAlertsWindowCB.set_Checked(_centerAlertContainerSetting.get_Value());
			centerAlertsWindowCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_centerAlertContainerSetting.set_Value(centerAlertsWindowCB.get_Checked());
			});
			Checkbox val19 = new Checkbox();
			((Control)val19).set_Parent((Container)(object)_alertSettingsWindow);
			val19.set_Text("Hide Alerts");
			((Control)val19).set_BasicTooltipText("When enabled, alerts on the screen will be hidden.");
			((Control)val19).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)centerAlertsWindowCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideAlertsCB = val19;
			hideAlertsCB.set_Checked(_hideAlertsSetting.get_Value());
			hideAlertsCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideAlertsSetting.set_Value(hideAlertsCB.get_Checked());
			});
			Checkbox val20 = new Checkbox();
			((Control)val20).set_Parent((Container)(object)_alertSettingsWindow);
			val20.set_Text("Hide Directions");
			((Control)val20).set_BasicTooltipText("When enabled, directions on the screen will be hidden.");
			((Control)val20).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideAlertsCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideDirectionsCB = val20;
			hideDirectionsCB.set_Checked(_hideDirectionsSetting.get_Value());
			hideDirectionsCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideDirectionsSetting.set_Value(hideDirectionsCB.get_Checked());
			});
			Checkbox val21 = new Checkbox();
			((Control)val21).set_Parent((Container)(object)_alertSettingsWindow);
			val21.set_Text("Hide Markers");
			((Control)val21).set_BasicTooltipText("When enabled, markers on the screen will be hidden.");
			((Control)val21).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideDirectionsCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideMarkersCB = val21;
			hideMarkersCB.set_Checked(_hideMarkersSetting.get_Value());
			hideMarkersCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideMarkersSetting.set_Value(hideMarkersCB.get_Checked());
			});
			Checkbox val22 = new Checkbox();
			((Control)val22).set_Parent((Container)(object)_alertSettingsWindow);
			val22.set_Text("Mute Text to Speech");
			((Control)val22).set_BasicTooltipText("When enabled, text to speech will be muted.");
			((Control)val22).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideMarkersCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox hideSoundsCB = val22;
			hideSoundsCB.set_Checked(_hideSoundsSetting.get_Value());
			hideSoundsCB.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_hideSoundsSetting.set_Value(hideSoundsCB.get_Checked());
			});
			Checkbox val23 = new Checkbox();
			((Control)val23).set_Parent((Container)(object)_alertSettingsWindow);
			val23.set_Text("Invert Alert Fill");
			((Control)val23).set_BasicTooltipText("When enabled, alerts fill up as time passes.\nWhen disabled, alerts drain as time passes.");
			((Control)val23).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)hideSoundsCB).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Checkbox fillDirection = val23;
			fillDirection.set_Checked(_alertFillDirection.get_Value());
			fillDirection.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_alertFillDirection.set_Value(fillDirection.get_Checked());
			});
			Label val24 = new Label();
			((Control)val24).set_Parent((Container)(object)_alertSettingsWindow);
			val24.set_Text("Alert Size");
			val24.set_AutoSizeWidth(true);
			((Control)val24).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)fillDirection).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertSizeLabel = val24;
			Dropdown val25 = new Dropdown();
			((Control)val25).set_Parent((Container)(object)_alertSettingsWindow);
			Dropdown alertSizeDropdown = val25;
			alertSizeDropdown.get_Items().Add("Small");
			alertSizeDropdown.get_Items().Add("Medium");
			alertSizeDropdown.get_Items().Add("Large");
			alertSizeDropdown.get_Items().Add("BigWig Style");
			alertSizeDropdown.set_SelectedItem(_alertSizeSetting.get_Value().ToString());
			alertSizeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_alertSizeSetting.set_Value((AlertType)Enum.Parse(typeof(AlertType), alertSizeDropdown.get_SelectedItem().Replace(" ", ""), ignoreCase: true));
			});
			Label val26 = new Label();
			((Control)val26).set_Parent((Container)(object)_alertSettingsWindow);
			val26.set_Text("Alert Display Orientation");
			val26.set_AutoSizeWidth(true);
			((Control)val26).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertSizeLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertDisplayOrientationLabel = val26;
			Dropdown val27 = new Dropdown();
			((Control)val27).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val27).set_Location(new Point(((Control)alertDisplayOrientationLabel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationLabel).get_Top()));
			Dropdown alertDisplayOrientationDropdown = val27;
			alertDisplayOrientationDropdown.get_Items().Add("Left to Right");
			alertDisplayOrientationDropdown.get_Items().Add("Right to Left");
			alertDisplayOrientationDropdown.get_Items().Add("Top to Bottom");
			alertDisplayOrientationDropdown.get_Items().Add("Bottom to Top");
			switch (_alertDisplayOrientationSetting.get_Value())
			{
			case AlertFlowDirection.LeftToRight:
				alertDisplayOrientationDropdown.set_SelectedItem("Left to Right");
				break;
			case AlertFlowDirection.RightToLeft:
				alertDisplayOrientationDropdown.set_SelectedItem("Right to Left");
				break;
			case AlertFlowDirection.TopToBottom:
				alertDisplayOrientationDropdown.set_SelectedItem("Top to Bottom");
				break;
			case AlertFlowDirection.BottomToTop:
				alertDisplayOrientationDropdown.set_SelectedItem("Bottom to Top");
				break;
			}
			alertDisplayOrientationDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				switch (alertDisplayOrientationDropdown.get_SelectedItem())
				{
				case "Left to Right":
					_alertDisplayOrientationSetting.set_Value(AlertFlowDirection.LeftToRight);
					break;
				case "Right to Left":
					_alertDisplayOrientationSetting.set_Value(AlertFlowDirection.RightToLeft);
					break;
				case "Top to Bottom":
					_alertDisplayOrientationSetting.set_Value(AlertFlowDirection.TopToBottom);
					break;
				case "Bottom to Top":
					_alertDisplayOrientationSetting.set_Value(AlertFlowDirection.BottomToTop);
					break;
				}
			});
			((Control)alertSizeDropdown).set_Location(new Point(((Control)alertDisplayOrientationDropdown).get_Left(), ((Control)alertSizeLabel).get_Top()));
			Label val28 = new Label();
			((Control)val28).set_Parent((Container)(object)_alertSettingsWindow);
			val28.set_Text("Alert Preview");
			val28.set_AutoSizeWidth(true);
			((Control)val28).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton val29 = new StandardButton();
			((Control)val29).set_Parent((Container)(object)_alertSettingsWindow);
			val29.set_Text("Add Test Alert");
			((Control)val29).set_Location(new Point(((Control)alertDisplayOrientationDropdown).get_Left(), ((Control)alertDisplayOrientationDropdown).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			StandardButton addTestAlertButton = val29;
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
			});
			StandardButton val30 = new StandardButton();
			((Control)val30).set_Parent((Container)(object)_alertSettingsWindow);
			val30.set_Text("Clear Test Alerts");
			((Control)val30).set_Location(new Point(((Control)addTestAlertButton).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)addTestAlertButton).get_Top()));
			StandardButton clearTestAlertsButton = val30;
			((Control)clearTestAlertsButton).set_Width((int)((double)((Control)clearTestAlertsButton).get_Width() * 1.15));
			((Control)clearTestAlertsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)_alertContainer).ClearChildren();
				_testAlertPanels.ForEach(delegate(IAlertPanel panel)
				{
					panel.Dispose();
				});
				_testAlertPanels.Clear();
			});
			((Control)alertSizeDropdown).set_Width(((Control)addTestAlertButton).get_Width() + ((Control)clearTestAlertsButton).get_Width() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			((Control)alertDisplayOrientationDropdown).set_Width(((Control)alertSizeDropdown).get_Width());
			Label val31 = new Label();
			((Control)val31).set_Parent((Container)(object)_alertSettingsWindow);
			val31.set_Text("Alert Container Position");
			val31.set_AutoSizeWidth(true);
			((Control)val31).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)clearTestAlertsButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertContainerPositionLabel = val31;
			StandardButton val32 = new StandardButton();
			((Control)val32).set_Parent((Container)(object)_alertSettingsWindow);
			val32.set_Text("Reset Position");
			((Control)val32).set_Location(new Point(((Control)addTestAlertButton).get_Left(), ((Control)alertContainerPositionLabel).get_Top()));
			StandardButton resetAlertContainerPositionButton = val32;
			((Control)resetAlertContainerPositionButton).set_Width(((Control)alertSizeDropdown).get_Width());
			((Control)resetAlertContainerPositionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				_alertContainerLocationSetting.set_Value(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - ((Control)_alertContainer).get_Width() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 - ((Control)_alertContainer).get_Height() / 2));
			});
			Label val33 = new Label();
			((Control)val33).set_Parent((Container)(object)_alertSettingsWindow);
			val33.set_Text("Alert Animation Duration");
			((Control)val33).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val33.set_AutoSizeWidth(true);
			((Control)val33).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)resetAlertContainerPositionButton).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertMoveDelayLabel = val33;
			TextBox val34 = new TextBox();
			((Control)val34).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val34).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			((Control)val34).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val34).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val34).set_Height(((Control)alertMoveDelayLabel).get_Height());
			((TextInputBase)val34).set_Text($"{_alertMoveDelaySetting.get_Value():0.00}");
			TextBox alertMoveDelayTextBox = val34;
			TrackBar val35 = new TrackBar();
			((Control)val35).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val35).set_BasicTooltipText("How many seconds alerts will take to reposition itself.");
			val35.set_MinValue(0f);
			val35.set_MaxValue(3f);
			val35.set_Value(_alertMoveDelaySetting.get_Value());
			val35.set_SmallStep(true);
			((Control)val35).set_Location(new Point(((Control)alertMoveDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Top()));
			((Control)val35).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertMoveDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertMoveDelaySlider = val35;
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
			Label val36 = new Label();
			((Control)val36).set_Parent((Container)(object)_alertSettingsWindow);
			val36.set_Text("Alert Fade In/Out Delay");
			((Control)val36).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val36.set_AutoSizeWidth(true);
			((Control)val36).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertMoveDelayLabel).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			Label alertFadeDelayLabel = val36;
			TextBox val37 = new TextBox();
			((Control)val37).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val37).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			((Control)val37).set_Location(new Point(((Control)resetAlertContainerPositionButton).get_Left(), ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val37).set_Width(((Control)resetAlertContainerPositionButton).get_Width() / 5);
			((Control)val37).set_Height(((Control)alertFadeDelayLabel).get_Height());
			((TextInputBase)val37).set_Text($"{_alertFadeDelaySetting.get_Value():0.00}");
			TextBox alertFadeDelayTextBox = val37;
			TrackBar val38 = new TrackBar();
			((Control)val38).set_Parent((Container)(object)_alertSettingsWindow);
			((Control)val38).set_BasicTooltipText("How many seconds alerts will take to appear/disappear.");
			val38.set_MinValue(0f);
			val38.set_MaxValue(3f);
			val38.set_Value(_alertFadeDelaySetting.get_Value());
			val38.set_SmallStep(true);
			((Control)val38).set_Location(new Point(((Control)alertFadeDelayTextBox).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)alertFadeDelayLabel).get_Top()));
			((Control)val38).set_Width(((Control)resetAlertContainerPositionButton).get_Width() - ((Control)alertFadeDelayTextBox).get_Width() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			TrackBar alertFadeDelaySlider = val38;
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
			StandardButton val39 = new StandardButton();
			((Control)val39).set_Parent((Container)(object)_alertSettingsWindow);
			val39.set_Text("Close");
			StandardButton closeAlertSettingsButton = val39;
			((Control)closeAlertSettingsButton).set_Location(new Point((((Control)_alertSettingsWindow).get_Left() + ((Control)_alertSettingsWindow).get_Right()) / 2 - ((Control)closeAlertSettingsButton).get_Width() / 2, ((Control)_alertSettingsWindow).get_Bottom() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y - ((Control)closeAlertSettingsButton).get_Height()));
			((Control)closeAlertSettingsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_alertSettingsWindow).Hide();
			});
			ShowTimerEntries((Panel)(object)timerPanel);
			Menu val40 = new Menu();
			Rectangle contentRegion = ((Container)menuSection).get_ContentRegion();
			((Control)val40).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val40.set_MenuItemHeight(40);
			((Control)val40).set_Parent((Container)(object)menuSection);
			val40.set_CanSelect(true);
			timerCategories = val40;
			MenuItem obj = timerCategories.AddMenuItem("All Timers", (Texture2D)null);
			obj.Select();
			_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => true).ToList();
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetails>((Func<TimerDetails, bool>)((TimerDetails db) => true));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => true).ToList();
			});
			MenuItem enabledTimers = timerCategories.AddMenuItem("Enabled Timers", (Texture2D)null);
			((Control)enabledTimers).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetails>((Func<TimerDetails, bool>)((TimerDetails db) => db.Encounter.Enabled));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => db.Encounter.Enabled).ToList();
			});
			((Control)timerCategories.AddMenuItem("Current Map", (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetails>((Func<TimerDetails, bool>)((TimerDetails db) => db.Encounter.Map == GameService.Gw2Mumble.get_CurrentMap().get_Id()));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => db.Encounter.Map == GameService.Gw2Mumble.get_CurrentMap().get_Id()).ToList();
			});
			((Control)timerCategories.AddMenuItem("Invalid Timers", (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				timerPanel.FilterChildren<TimerDetails>((Func<TimerDetails, bool>)((TimerDetails db) => db.Encounter.State == Encounter.EncounterStates.Error));
				_displayedTimerDetails = _allTimerDetails.Where((TimerDetails db) => db.Encounter.State == Encounter.EncounterStates.Error).ToList();
			});
			ShowCustomTimerCategories();
			((Control)enableAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (timerCategories.get_SelectedMenuItem() != enabledTimers)
				{
					_displayedTimerDetails.ForEach(delegate(TimerDetails db)
					{
						if (db.Encounter.State != 0)
						{
							db.Enabled = true;
						}
					});
				}
			});
			((Control)disableAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_displayedTimerDetails.ForEach(delegate(TimerDetails db)
				{
					if (db.Encounter.State != 0)
					{
						db.Enabled = false;
					}
				});
				if (timerCategories.get_SelectedMenuItem() == enabledTimers)
				{
					timerPanel.FilterChildren<TimerDetails>((Func<TimerDetails, bool>)((TimerDetails db) => db.Enabled));
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
			timerLoader?.Dispose();
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged(_onNewMapLoaded);
			_showDebugSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateShowDebug);
			_lockAlertContainerSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateLockAlertContainer);
			_centerAlertContainerSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateCenterAlertContainer);
			_hideAlertsSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideAlerts);
			_hideDirectionsSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideDirections);
			_hideMarkersSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingsUpdateHideMarkers);
			_alertSizeSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<AlertType>>)SettingsUpdateAlertSize);
			_alertDisplayOrientationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<AlertFlowDirection>>)SettingsUpdateAlertDisplayOrientation);
			_alertMoveDelaySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingsUpdateAlertMoveDelay);
			_alertFadeDelaySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingsUpdateAlertFadeDelay);
			ResetButton resetButton = _resetButton;
			if (resetButton != null)
			{
				((Control)resetButton).Dispose();
			}
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_timersTab);
			((Control)_tabPanel).Dispose();
			_allTimerDetails.ForEach(delegate(TimerDetails de)
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
			foreach (Encounter encounter in _encounters)
			{
				encounter.Dispose();
			}
			_encounters.Clear();
			_activeEncounters.Clear();
			foreach (Encounter invalidEncounter in _invalidEncounters)
			{
				invalidEncounter.Dispose();
			}
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
