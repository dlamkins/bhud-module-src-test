using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.MumbleInfo.Map;
using Estreya.BlishHUD.Shared.Security;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.UI.Views.Settings;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using SemVer;

namespace Estreya.BlishHUD.Shared.Modules
{
	public abstract class BaseModule<TModule, TSettings> : Module where TModule : class where TSettings : BaseModuleSettings
	{
		protected const string FILE_ROOT_URL = "https://files.estreya.de";

		protected const string FILE_BLISH_ROOT_URL = "https://files.estreya.de/blish-hud";

		protected string API_ROOT_URL = "https://blish-hud.api.estreya.de";

		protected const string GITHUB_OWNER = "Tharylia";

		protected const string GITHUB_REPOSITORY = "Blish-HUD-Modules";

		private const string GITHUB_CLIENT_ID = "Iv1.9e4dc29d43243704";

		protected static TModule Instance;

		private ModuleSettingsView _defaultSettingView;

		private FlurlClient _flurlClient;

		private LoadingSpinner _loadingSpinner;

		private readonly AsyncLock _stateLock = new AsyncLock();

		private Collection<ManagedState> _states = new Collection<ManagedState>();

		protected Logger Logger { get; }

		protected GitHubHelper GithubHelper { get; private set; }

		protected PasswordManager PasswordManager { get; private set; }

		public string WEBSITE_MODULE_FILE_URL => "https://files.estreya.de/blish-hud/" + WebsiteModuleName;

		public string API_URL => API_ROOT_URL + "/v" + API_VERSION_NO + "/" + WebsiteModuleName;

		public abstract string WebsiteModuleName { get; }

		protected abstract string API_VERSION_NO { get; }

		public bool IsPrerelease
		{
			get
			{
				Version version = ((Module)this).get_Version();
				return !string.IsNullOrWhiteSpace((version != null) ? version.get_PreRelease() : null);
			}
		}

		protected SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		protected ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		protected DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		protected Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		protected bool Debug => false;

		protected bool ShowUI { get; private set; } = true;


		public TSettings ModuleSettings { get; private set; }

		protected CornerIcon CornerIcon { get; set; }

		protected TabbedWindow2 SettingsWindow { get; private set; }

		public virtual BitmapFont Font => GameService.Content.get_DefaultFont16();

		public IconState IconState { get; private set; }

		public TranslationState TranslationState { get; private set; }

		public SettingEventState SettingEventState { get; private set; }

		public NewsState NewsState { get; private set; }

		public WorldbossState WorldbossState { get; private set; }

		public MapchestState MapchestState { get; private set; }

		public PointOfInterestState PointOfInterestState { get; private set; }

		public AccountState AccountState { get; private set; }

		public SkillState SkillState { get; private set; }

		public TradingPostState TradingPostState { get; private set; }

		public ItemState ItemState { get; private set; }

		public ArcDPSState ArcDPSState { get; private set; }

		public BlishHudApiState BlishHUDAPIState { get; private set; }

		protected IFlurlClient GetFlurlClient()
		{
			if (_flurlClient == null)
			{
				_flurlClient = new FlurlClient();
				_flurlClient.WithHeader("User-Agent", $"{((Module)this).get_Name()} {((Module)this).get_Version()}");
			}
			return _flurlClient;
		}

		public BaseModule(ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Logger = Logger.GetLogger(((object)this).GetType());
			Instance = this as TModule;
		}

		protected sealed override void DefineSettings(SettingCollection settings)
		{
			ModuleSettings = DefineModuleSettings(settings) as TSettings;
		}

		protected abstract BaseModuleSettings DefineModuleSettings(SettingCollection settings);

		protected override void Initialize()
		{
			string directoryName = GetDirectoryName();
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				string directoryPath = DirectoriesManager.GetFullDirectoryPath(directoryName);
				PasswordManager = new PasswordManager(directoryPath);
				PasswordManager.InitializeEntropy(Encoding.UTF8.GetBytes(((Module)this).get_Namespace()));
			}
		}

		protected override async Task LoadAsync()
		{
			Logger.Debug("Initialize states");
			await Task.Factory.StartNew((Func<Task>)InitializeStates, TaskCreationOptions.LongRunning).Unwrap();
			GithubHelper = new GitHubHelper("Tharylia", "Blish-HUD-Modules", "Iv1.9e4dc29d43243704", ((Module)this).get_Name(), PasswordManager, IconState, TranslationState);
			ModuleSettings.UpdateLocalization(TranslationState);
			ModuleSettings.ModuleSettingsChanged += ModuleSettings_ModuleSettingsChanged;
		}

		private void ModuleSettings_ModuleSettingsChanged(object sender, BaseModuleSettings.ModuleSettingsChangedEventArgs e)
		{
			if (e.Name == "RegisterCornerIcon")
			{
				HandleCornerIcon(ModuleSettings.RegisterCornerIcon.get_Value());
			}
		}

		protected abstract string GetDirectoryName();

		private async Task InitializeStates()
		{
			string directoryName = GetDirectoryName();
			string directoryPath = null;
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				directoryPath = DirectoriesManager.GetFullDirectoryPath(directoryName);
			}
			using (await _stateLock.LockAsync())
			{
				StateConfigurations configurations = new StateConfigurations();
				ConfigureStates(configurations);
				if (configurations.BlishHUDAPI.Enabled)
				{
					if (PasswordManager == null)
					{
						throw new ArgumentNullException("PasswordManager");
					}
					BlishHUDAPIState = new BlishHudApiState(configurations.BlishHUDAPI, ModuleSettings.BlishAPIUsername, PasswordManager, GetFlurlClient(), API_ROOT_URL, API_VERSION_NO);
					_states.Add(BlishHUDAPIState);
				}
				if (configurations.Account.Enabled)
				{
					AccountState = new AccountState(configurations.Account, Gw2ApiManager);
					_states.Add(AccountState);
				}
				IconState = new IconState(new StateConfiguration
				{
					Enabled = true,
					AwaitLoading = false
				}, ContentsManager);
				_states.Add(IconState);
				TranslationState = new TranslationState(new StateConfiguration
				{
					Enabled = true,
					AwaitLoading = true
				}, GetFlurlClient(), WEBSITE_MODULE_FILE_URL);
				_states.Add(TranslationState);
				SettingEventState = new SettingEventState(new StateConfiguration
				{
					Enabled = true,
					AwaitLoading = false,
					SaveInterval = Timeout.InfiniteTimeSpan
				});
				_states.Add(SettingEventState);
				NewsState = new NewsState(new StateConfiguration
				{
					AwaitLoading = true,
					Enabled = true
				}, GetFlurlClient(), WEBSITE_MODULE_FILE_URL);
				_states.Add(NewsState);
				if (configurations.Items.Enabled)
				{
					ItemState = new ItemState(configurations.Items, Gw2ApiManager, directoryPath);
					_states.Add(ItemState);
				}
				if (configurations.TradingPost.Enabled)
				{
					TradingPostState = new TradingPostState(configurations.TradingPost, Gw2ApiManager, ItemState);
					_states.Add(TradingPostState);
				}
				if (configurations.Worldbosses.Enabled)
				{
					if (configurations.Account.Enabled)
					{
						WorldbossState = new WorldbossState(configurations.Worldbosses, Gw2ApiManager, AccountState);
						_states.Add(WorldbossState);
					}
					else
					{
						Logger.Debug(typeof(WorldbossState).Name + " is not available because " + typeof(AccountState).Name + " is deactivated.");
						configurations.Worldbosses.Enabled = false;
					}
				}
				if (configurations.Mapchests.Enabled)
				{
					if (configurations.Account.Enabled)
					{
						MapchestState = new MapchestState(configurations.Mapchests, Gw2ApiManager, AccountState);
						_states.Add(MapchestState);
					}
					else
					{
						Logger.Debug(typeof(MapchestState).Name + " is not available because " + typeof(AccountState).Name + " is deactivated.");
						configurations.Mapchests.Enabled = false;
					}
				}
				if (configurations.PointOfInterests.Enabled)
				{
					if (string.IsNullOrWhiteSpace(directoryPath))
					{
						throw new ArgumentNullException("directoryPath", "Module directory is not specified.");
					}
					PointOfInterestState = new PointOfInterestState(configurations.PointOfInterests, Gw2ApiManager, directoryPath);
					_states.Add(PointOfInterestState);
				}
				if (configurations.Skills.Enabled)
				{
					if (string.IsNullOrWhiteSpace(directoryPath))
					{
						throw new ArgumentNullException("directoryPath", "Module directory is not specified.");
					}
					SkillState = new SkillState(configurations.Skills, Gw2ApiManager, IconState, directoryPath, GetFlurlClient(), "https://files.estreya.de/blish-hud");
					_states.Add(SkillState);
				}
				if (configurations.ArcDPS.Enabled)
				{
					if (configurations.Skills.Enabled)
					{
						ArcDPSState = new ArcDPSState(configurations.ArcDPS, SkillState);
						_states.Add(ArcDPSState);
					}
					else
					{
						Logger.Debug(typeof(ArcDPSState).Name + " is not available because " + typeof(SkillState).Name + " is deactivated.");
						configurations.ArcDPS.Enabled = false;
					}
				}
				Collection<ManagedState> customStates = GetAdditionalStates(directoryPath);
				if (customStates != null && customStates.Count > 0)
				{
					foreach (ManagedState customState in customStates)
					{
						_states.Add(customState);
					}
				}
				OnBeforeStatesStarted();
				foreach (ManagedState state2 in _states.Where((ManagedState state) => !state.Running))
				{
					if (state2.AwaitLoading)
					{
						try
						{
							await state2.Start();
						}
						catch (Exception ex)
						{
							Logger.Error(ex, "Failed starting state \"{0}\"", new object[1] { state2.GetType().Name });
						}
						continue;
					}
					Task.Run((Func<Task>)state2.Start).ContinueWith(delegate(Task task)
					{
						if (task.IsFaulted)
						{
							Logger.Error((Exception)task.Exception, "Not awaited state start failed for \"{0}\"", new object[1] { state2.GetType().Name });
						}
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		protected virtual void ConfigureStates(StateConfigurations configurations)
		{
		}

		protected virtual void OnBeforeStatesStarted()
		{
		}

		protected virtual Collection<ManagedState> GetAdditionalStates(string directoryPath)
		{
			return null;
		}

		private void HandleCornerIcon(bool show)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			if (show)
			{
				if (CornerIcon == null)
				{
					CornerIcon val = new CornerIcon();
					val.set_IconName(((Module)this).get_Name());
					val.set_Icon(GetCornerIcon());
					val.set_Priority(1289351278);
					CornerIcon = val;
					OnCornerIconBuild();
				}
			}
			else if (CornerIcon != null)
			{
				OnCornerIconDispose();
				((Control)CornerIcon).Dispose();
				CornerIcon = null;
			}
		}

		protected virtual void OnCornerIconBuild()
		{
			((Control)CornerIcon).add_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
			((Control)CornerIcon).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)CornerIcon_RightMouseButtonPressed);
		}

		protected virtual void OnCornerIconDispose()
		{
			((Control)CornerIcon).remove_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
			((Control)CornerIcon).remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)CornerIcon_RightMouseButtonPressed);
		}

		private void CornerIcon_Click(object sender, MouseEventArgs e)
		{
			switch (ModuleSettings.CornerIconLeftClickAction.get_Value())
			{
			case CornerIconClickAction.Settings:
				((WindowBase2)SettingsWindow).ToggleWindow();
				break;
			case CornerIconClickAction.Visibility:
				ModuleSettings.GlobalDrawerVisible.set_Value(!ModuleSettings.GlobalDrawerVisible.get_Value());
				break;
			}
		}

		private void CornerIcon_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			switch (ModuleSettings.CornerIconRightClickAction.get_Value())
			{
			case CornerIconClickAction.Settings:
				((WindowBase2)SettingsWindow).ToggleWindow();
				break;
			case CornerIconClickAction.Visibility:
				ModuleSettings.GlobalDrawerVisible.set_Value(!ModuleSettings.GlobalDrawerVisible.get_Value());
				break;
			}
		}

		public override IView GetSettingsView()
		{
			if (_defaultSettingView == null)
			{
				_defaultSettingView = new ModuleSettingsView(IconState, TranslationState);
				_defaultSettingView.OpenClicked += DefaultSettingView_OpenClicked;
				_defaultSettingView.CreateGithubIssueClicked += DefaultSettingView_CreateGithubIssueClicked;
			}
			return (IView)(object)_defaultSettingView;
		}

		private void DefaultSettingView_CreateGithubIssueClicked(object sender, EventArgs e)
		{
			GithubHelper.OpenIssueWindow();
		}

		private void DefaultSettingView_OpenClicked(object sender, EventArgs e)
		{
			((WindowBase2)SettingsWindow).ToggleWindow();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			((Module)this).OnModuleLoaded(e);
			Logger.Debug("Start building settings window.");
			if (SettingsWindow == null)
			{
				TabbedWindow2 val2 = (SettingsWindow = WindowUtil.CreateTabbedWindow(((Module)this).get_Name(), ((object)this).GetType(), Guid.Parse("6bd04be4-dc19-4914-a2c3-8160ce76818b"), IconState, GetEmblem()));
			}
			SettingsWindow.get_Tabs().Add(new Tab(IconState.GetIcon("482926.png"), (Func<IView>)(() => (IView)(object)new NewsView(GetFlurlClient(), Gw2ApiManager, IconState, TranslationState, NewsState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = ModuleSettings.DefaultGW2Color
			}), "News", (int?)null));
			OnSettingWindowBuild(SettingsWindow);
			SettingsWindow.get_Tabs().Add(new Tab(IconState.GetIcon("156331.png"), (Func<IView>)(() => (IView)(object)new DonationView(GetFlurlClient(), Gw2ApiManager, IconState, TranslationState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = ModuleSettings.DefaultGW2Color
			}), "Donations", (int?)null));
			if (Debug)
			{
				SettingsWindow.get_Tabs().Add(new Tab(IconState.GetIcon("155052.png"), (Func<IView>)(() => (IView)(object)new StateSettingsView(_states, Gw2ApiManager, IconState, TranslationState, SettingEventState, Font)
				{
					DefaultColor = ModuleSettings.DefaultGW2Color
				}), "Debug", (int?)null));
			}
			Logger.Debug("Finished building settings window.");
			HandleCornerIcon(ModuleSettings.RegisterCornerIcon.get_Value());
		}

		protected abstract AsyncTexture2D GetEmblem();

		protected abstract AsyncTexture2D GetCornerIcon();

		protected virtual void OnSettingWindowBuild(TabbedWindow2 settingWindow)
		{
		}

		protected override void Update(GameTime gameTime)
		{
			ShowUI = CalculateUIVisibility();
			using (_stateLock.Lock())
			{
				bool anyStateLoading = false;
				string loadingText = null;
				foreach (ManagedState state in _states)
				{
					state.Update(gameTime);
					APIState apiState = state as APIState;
					if (apiState == null || !apiState.Loading)
					{
						continue;
					}
					anyStateLoading = true;
					if (!string.IsNullOrWhiteSpace(apiState.ProgressText))
					{
						if (loadingText == null)
						{
							loadingText = state.GetType().Name + ": " + apiState.ProgressText;
						}
					}
					else if (loadingText == null)
					{
						loadingText = state.GetType().Name;
					}
				}
				HandleLoadingSpinner(anyStateLoading, loadingText);
			}
		}

		protected virtual bool CalculateUIVisibility()
		{
			bool show = true;
			if (ModuleSettings.HideOnOpenMap.get_Value())
			{
				show &= !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			if (ModuleSettings.HideOnMissingMumbleTicks.get_Value())
			{
				show &= GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds < 0.5;
			}
			if (ModuleSettings.HideInCombat.get_Value())
			{
				show &= !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
			}
			if (ModuleSettings.HideInPvE_OpenWorld.get_Value())
			{
				MapType[] array = new MapType[4];
				RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				MapType[] pveOpenWorldMapTypes = (MapType[])(object)array;
				show &= GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pveOpenWorldMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type()) || MapInfo.MAP_IDS_PVE_COMPETETIVE.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			}
			if (ModuleSettings.HideInPvE_Competetive.get_Value())
			{
				MapType[] pveCompetetiveMapTypes = (MapType[])(object)new MapType[1] { (MapType)4 };
				show &= GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pveCompetetiveMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type()) || !MapInfo.MAP_IDS_PVE_COMPETETIVE.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			}
			if (ModuleSettings.HideInWvW.get_Value())
			{
				MapType[] array2 = new MapType[5];
				RuntimeHelpers.InitializeArray(array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				MapType[] wvwMapTypes = (MapType[])(object)array2;
				show &= !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !wvwMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type());
			}
			if (ModuleSettings.HideInPvP.get_Value())
			{
				MapType[] pvpMapTypes = (MapType[])(object)new MapType[2]
				{
					(MapType)2,
					(MapType)6
				};
				show &= !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pvpMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type());
			}
			return show;
		}

		protected void HandleLoadingSpinner(bool show, string text = null)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			show &= CornerIcon != null;
			if (_loadingSpinner == null)
			{
				LoadingSpinner val = new LoadingSpinner();
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				CornerIcon cornerIcon = CornerIcon;
				((Control)val).set_Size((Point)((cornerIcon != null) ? ((Control)cornerIcon).get_Size() : new Point(0, 0)));
				((Control)val).set_Visible(false);
				_loadingSpinner = val;
			}
			if (CornerIcon != null)
			{
				((Control)_loadingSpinner).set_Location(new Point(((Control)CornerIcon).get_Location().X, ((Control)CornerIcon).get_Location().Y + ((Control)CornerIcon).get_Height() + 5));
			}
			((Control)_loadingSpinner).set_BasicTooltipText(text);
			((Control)_loadingSpinner).set_Visible(show);
		}

		protected override void Unload()
		{
			Logger.Debug("Unload settings...");
			if (ModuleSettings != null)
			{
				ModuleSettings.ModuleSettingsChanged -= ModuleSettings_ModuleSettingsChanged;
				ModuleSettings.Unload();
				ModuleSettings = null;
			}
			Logger.Debug("Unloaded settings.");
			Logger.Debug("Unload default settings view...");
			if (_defaultSettingView != null)
			{
				_defaultSettingView.OpenClicked -= DefaultSettingView_OpenClicked;
				_defaultSettingView.CreateGithubIssueClicked -= DefaultSettingView_CreateGithubIssueClicked;
				((View<IPresenter>)(object)_defaultSettingView).DoUnload();
				_defaultSettingView = null;
			}
			Logger.Debug("Unloaded default settings view.");
			Logger.Debug("Unload settings window...");
			TabbedWindow2 settingsWindow = SettingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Hide();
			}
			TabbedWindow2 settingsWindow2 = SettingsWindow;
			if (settingsWindow2 != null)
			{
				((Control)settingsWindow2).Dispose();
			}
			SettingsWindow = null;
			Logger.Debug("Unloaded settings window.");
			Logger.Debug("Unload corner icon...");
			HandleCornerIcon(show: false);
			LoadingSpinner loadingSpinner = _loadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
			}
			_loadingSpinner = null;
			Logger.Debug("Unloaded corner icon.");
			Logger.Debug("Unloading states...");
			using (_stateLock.Lock())
			{
				_states.ToList().ForEach(delegate(ManagedState state)
				{
					state?.Dispose();
				});
				_states.Clear();
				AccountState = null;
				ArcDPSState = null;
				IconState = null;
				ItemState = null;
				MapchestState = null;
				WorldbossState = null;
				PointOfInterestState = null;
				SettingEventState = null;
				SkillState = null;
				TradingPostState = null;
				TranslationState = null;
			}
			Logger.Debug("Unloaded states.");
			Logger.Debug("Unload flurl client...");
			_flurlClient?.Dispose();
			_flurlClient = null;
			Logger.Debug("Unloaded flurl client.");
			Logger.Debug("Unload module instance...");
			Instance = null;
			Logger.Debug("Unloaded module instance.");
		}
	}
}
