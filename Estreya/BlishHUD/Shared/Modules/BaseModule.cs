using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Exceptions;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.MumbleInfo.Map;
using Estreya.BlishHUD.Shared.Net;
using Estreya.BlishHUD.Shared.Security;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Services.Audio;
using Estreya.BlishHUD.Shared.Services.GameIntegration;
using Estreya.BlishHUD.Shared.Services.TradingPost;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Threading.Events;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.UI.Views.Settings;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Flurl.Http.Configuration;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using SemVer;

namespace Estreya.BlishHUD.Shared.Modules
{
	public abstract class BaseModule<TModule, TSettings> : Module where TModule : class where TSettings : BaseModuleSettings
	{
		private static TimeSpan _checkBackendInterval = TimeSpan.FromMinutes(2.0);

		private AsyncRef<double> _lastBackendCheck = new AsyncRef<double>(0.0);

		protected const string LIVE_FILE_SERVER_HOSTNAME = "files.estreya.de";

		protected const string DEV_FILE_SERVER_HOSTNAME = "files.estreya.dev";

		protected const string LIVE_API_HOSTNAME = "api.estreya.de";

		protected const string DEV_API_HOSTNAME = "api.estreya.dev";

		protected const string GITHUB_OWNER = "Tharylia";

		protected const string GITHUB_REPOSITORY = "Blish-HUD-Modules";

		private const string GITHUB_CLIENT_ID = "Iv1.9e4dc29d43243704";

		private string ErrorStateText;

		private ModuleSettingsView _defaultSettingView;

		private FlurlClient _flurlClient;

		private readonly ConcurrentDictionary<string, string> _loadingTexts = new ConcurrentDictionary<string, string>();

		private readonly ConcurrentDictionary<ModuleErrorStateGroup, string> _errorStates = new ConcurrentDictionary<ModuleErrorStateGroup, string>();

		private LoadingSpinner _loadingSpinner;

		private readonly AsyncLock _servicesLock = new AsyncLock();

		private readonly SynchronizedCollection<ManagedService> _services = new SynchronizedCollection<ManagedService>();

		private CancellationTokenSource _cancellationTokenSource;

		protected Logger Logger { get; }

		protected string FILE_ROOT_URL => "https://" + (ModuleSettings.UseDevelopmentAPI.get_Value() ? "files.estreya.dev" : "files.estreya.de");

		protected string FILE_BLISH_ROOT_URL => FILE_ROOT_URL + "/blish-hud";

		protected string MODULE_FILE_URL => FILE_BLISH_ROOT_URL + "/" + UrlModuleName;

		protected string API_ROOT_URL => "https://" + (ModuleSettings.UseDevelopmentAPI.get_Value() ? "api.estreya.dev" : "api.estreya.de") + "/blish-hud";

		private string API_HEALTH_URL => API_ROOT_URL + "/health";

		protected string MODULE_API_URL => API_ROOT_URL + "/v" + API_VERSION_NO + "/" + UrlModuleName;

		protected GitHubHelper GithubHelper { get; private set; }

		protected PasswordManager PasswordManager { get; private set; }

		protected abstract string UrlModuleName { get; }

		protected abstract string API_VERSION_NO { get; }

		protected virtual bool NeedsBackend => false;

		protected virtual bool EnableMetrics => false;

		protected ModuleState ModuleState
		{
			get
			{
				if (!_errorStates.Any((KeyValuePair<ModuleErrorStateGroup, string> e) => !string.IsNullOrWhiteSpace(e.Value)))
				{
					return ModuleState.Normal;
				}
				return ModuleState.Error;
			}
		}

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


		protected TSettings ModuleSettings { get; private set; }

		protected CornerIcon CornerIcon { get; set; }

		protected MessageContainer MessageContainer { get; private set; }

		protected TabbedWindow SettingsWindow { get; private set; }

		protected virtual BitmapFont Font => GameService.Content.get_DefaultFont16();

		protected IconService IconService { get; private set; }

		protected TranslationService TranslationService { get; private set; }

		protected SettingEventService SettingEventService { get; private set; }

		protected NewsService NewsService { get; private set; }

		protected WorldbossService WorldbossService { get; private set; }

		protected MapchestService MapchestService { get; private set; }

		protected PointOfInterestService PointOfInterestService { get; private set; }

		protected AccountService AccountService { get; private set; }

		protected SkillService SkillService { get; private set; }

		protected PlayerTransactionsService PlayerTransactionsService { get; private set; }

		protected TransactionsService TransactionsService { get; private set; }

		protected ItemService ItemService { get; private set; }

		protected ArcDPSService ArcDPSService { get; private set; }

		protected BlishHudApiService BlishHUDAPIService { get; private set; }

		protected AchievementService AchievementService { get; private set; }

		protected AccountAchievementService AccountAchievementService { get; private set; }

		protected MetricsService MetricsService { get; private set; }

		protected AudioService AudioService { get; private set; }

		protected ChatService ChatService { get; private set; }

		protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

		protected abstract int CornerIconPriority { get; }

		protected event AsyncEventHandler BackendConnectionRestored;

		protected event AsyncEventHandler BackendConnectionLost;

		protected IFlurlClient GetFlurlClient()
		{
			if (_flurlClient == null)
			{
				_flurlClient = new FlurlClient();
				_flurlClient.WithHeader("User-Agent", $"{((Module)this).get_Name()} {((Module)this).get_Version()}").WithHeader("Accept-Encoding", "gzip, delate").Configure(delegate(ClientFlurlHttpSettings c)
				{
					c.HttpClientFactory = new FlurlHttpClientFactory();
					c.OnErrorAsync = async delegate(HttpCall call)
					{
						if (call.Response != null && call.FlurlRequest.Url.ToUri().AbsoluteUri.StartsWith(API_ROOT_URL))
						{
							IEnumerable<string> headers = default(IEnumerable<string>);
							((HttpHeaders)call.Response.get_Headers()).TryGetValues("API-TraceId", ref headers);
							string apiTraceId = headers?.FirstOrDefault();
							if (apiTraceId != null)
							{
								Logger.Warn("Intercepted estreya api error. Please provide trace id \"" + apiTraceId + "\" for support.");
							}
						}
					};
				});
			}
			return _flurlClient;
		}

		protected BaseModule(ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Logger = Logger.GetLogger(((object)this).GetType());
		}

		protected sealed override void DefineSettings(SettingCollection settings)
		{
			ModuleSettings = DefineModuleSettings(settings) as TSettings;
		}

		protected abstract BaseModuleSettings DefineModuleSettings(SettingCollection settings);

		protected override void Initialize()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			TEMP_FIX_SetTacOAsActive();
			string directoryName = GetDirectoryName();
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				string directoryPath = DirectoriesManager.GetFullDirectoryPath(directoryName);
				PasswordManager = new PasswordManager(directoryPath);
				PasswordManager.InitializeEntropy(Encoding.UTF8.GetBytes(((Module)this).get_Namespace()));
			}
		}

		private void TEMP_FIX_SetTacOAsActive()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			if (DateTime.UtcNow.Date >= new DateTime(2023, 8, 22, 0, 0, 0, DateTimeKind.Utc) && Program.get_OverlayVersion() < new Version(1, 1, 0, (string)null, (string)null))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.get_TacO(), new object[1] { true });
				}
				catch
				{
				}
			}
		}

		protected override async Task LoadAsync()
		{
			await Task.Factory.StartNew((Func<Task>)InitializeEssentialServices, TaskCreationOptions.LongRunning).Unwrap();
			if (ModuleSettings.UseDevelopmentAPI.get_Value())
			{
				Logger.Info("User configured module to use development api: " + MODULE_API_URL);
				await MessageContainer.Add((Module)(object)this, MessageContainer.MessageType.Warning, "Using Development API");
			}
			await CheckBackendHealth();
			try
			{
				await VerifyModuleState();
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to verify module. Disabling module.");
				DisableSelf();
				await Task.Delay(1000);
				return;
			}
			await Task.Factory.StartNew((Func<Task>)InitializeServices, TaskCreationOptions.LongRunning).Unwrap();
			if (EnableMetrics)
			{
				await MetricsService.AskMetricsConsent();
			}
			GithubHelper = new GitHubHelper("Tharylia", "Blish-HUD-Modules", "Iv1.9e4dc29d43243704", ((Module)this).get_Name(), PasswordManager, IconService, TranslationService, ModuleSettings);
			ModuleSettings.UpdateLocalization(TranslationService);
			ModuleSettings.RegisterCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)RegisterCornerIcon_SettingChanged);
			BackendConnectionRestored += BaseModule_BackendConnectionRestored;
		}

		private async Task BaseModule_BackendConnectionRestored(object sender)
		{
			try
			{
				await BlishHUDAPIService.Reload();
			}
			catch (Exception)
			{
			}
		}

		private async Task VerifyModuleState()
		{
			if (HasErrorState(ModuleErrorStateGroup.BACKEND_UNAVAILABLE))
			{
				return;
			}
			Func<HttpResponseMessage, bool> isBackendUnavailable = delegate(HttpResponseMessage response)
			{
				bool flag = response == null;
				if (!flag)
				{
					HttpStatusCode statusCode = response.get_StatusCode();
					bool flag2 = (((uint)(statusCode - 502) <= 1u) ? true : false);
					flag = flag2;
				}
				return flag;
			};
			IFlurlRequest request = GetFlurlClient().Request(MODULE_API_URL, "validate").AllowAnyHttpStatus();
			ModuleValidationRequest moduleValidationRequest = default(ModuleValidationRequest);
			moduleValidationRequest.Version = ((Module)this).get_Version();
			ModuleValidationRequest data = moduleValidationRequest;
			HttpResponseMessage response2 = null;
			try
			{
				response2 = await request.PostJsonAsync(data, default(CancellationToken), (HttpCompletionOption)0);
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to validate module.");
			}
			if ((response2 != null && (response2.get_IsSuccessStatusCode() || response2.get_StatusCode() == HttpStatusCode.NotFound)) || isBackendUnavailable(response2))
			{
				return;
			}
			if (response2.get_StatusCode() != HttpStatusCode.Forbidden)
			{
				string content2 = await response2.get_Content().ReadAsStringAsync();
				await MessageContainer.Add((Module)(object)this, MessageContainer.MessageType.Error, "The module \"" + ((Module)this).get_Name() + "\" could not verify itself. Please check the latest log for more information.");
				Logger.Error($"Module validation failed with unexpected status code {response2.get_StatusCode()}: {content2}");
				ReportErrorState(ModuleErrorStateGroup.MODULE_VALIDATION, "Module validation failed. Check latest log for more information.");
				return;
			}
			ReportErrorState(ModuleErrorStateGroup.MODULE_VALIDATION, null);
			ModuleValidationResponse validationResponse;
			try
			{
				validationResponse = await response2.GetJsonAsync<ModuleValidationResponse>();
			}
			catch (Exception)
			{
				string content2 = await response2.get_Content().ReadAsStringAsync();
				await MessageContainer.Add((Module)(object)this, MessageContainer.MessageType.Error, "The module \"" + ((Module)this).get_Name() + "\" could not verify itself. Please check the latest log for more information.");
				throw new ModuleInvalidException("Could not read module validation response: " + content2);
			}
			List<string> messages = new List<string>
			{
				"[" + ((Module)this).get_Name() + "]",
				"The current module version is invalid!"
			};
			if (!string.IsNullOrWhiteSpace(validationResponse.Message) || !string.IsNullOrWhiteSpace(response2.get_ReasonPhrase()))
			{
				messages.Add(validationResponse.Message ?? response2.get_ReasonPhrase());
			}
			await MessageContainer.Add((Module)(object)this, MessageContainer.MessageType.Error, "\n" + string.Join("\n", messages));
			throw new ModuleInvalidException(validationResponse.Message);
		}

		protected void DisableSelf()
		{
			GameService.Module.get_Modules().ToList().Find((ModuleManager m) => m.get_ModuleInstance() == this)
				.Disable();
		}

		private async Task CheckBackendHealth()
		{
			if (!NeedsBackend)
			{
				return;
			}
			IFlurlRequest request = GetFlurlClient().Request(API_HEALTH_URL);
			HttpResponseMessage response = null;
			Stopwatch sw = Stopwatch.StartNew();
			try
			{
				response = await request.GetAsync(default(CancellationToken), (HttpCompletionOption)0);
			}
			catch (Exception ex2)
			{
				Logger.Warn(ex2, "Failed to validate backend health.");
			}
			sw.Stop();
			Logger logger = Logger;
			object[] obj = new object[4]
			{
				request.Url,
				(int)((response == null) ? ((HttpStatusCode)999) : response.get_StatusCode()),
				null,
				null
			};
			HttpResponseMessage obj2 = response;
			obj[2] = ((obj2 != null) ? obj2.get_ReasonPhrase() : null) ?? string.Empty;
			obj[3] = sw.Elapsed.TotalMilliseconds;
			logger.Debug(string.Format("Checked API backend at \"{0}\". Response: {1} - {2} | Duration: {3}ms", obj));
			bool backendOnline = response != null && response.get_IsSuccessStatusCode();
			bool wasUnavailable = HasErrorState(ModuleErrorStateGroup.BACKEND_UNAVAILABLE);
			if (!wasUnavailable && !backendOnline)
			{
				ReportErrorState(ModuleErrorStateGroup.BACKEND_UNAVAILABLE, "Backend unavailable.");
				await MessageContainer.Add((Module)(object)this, MessageContainer.MessageType.Error, "The backend for \"" + ((Module)this).get_Name() + "\" is unavailable. Check Estreya BlishHUD Discord for news.");
				await (this.BackendConnectionLost?.Invoke(this) ?? Task.CompletedTask);
			}
			else if (wasUnavailable && backendOnline)
			{
				ReportErrorState(ModuleErrorStateGroup.BACKEND_UNAVAILABLE, null);
				try
				{
					await VerifyModuleState();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to verify module. Disabling module.");
					DisableSelf();
					return;
				}
				await MessageContainer.Add((Module)(object)this, MessageContainer.MessageType.Info, "The backend for \"" + ((Module)this).get_Name() + "\" is back online.");
				await (this.BackendConnectionRestored?.Invoke(this) ?? Task.CompletedTask);
			}
		}

		private void RegisterCornerIcon_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			HandleCornerIcon(ModuleSettings.RegisterCornerIcon.get_Value());
		}

		protected abstract string GetDirectoryName();

		protected virtual Task OnAfterEssentialsServicesInitialized()
		{
			MessageContainer = new MessageContainer(Gw2ApiManager, ModuleSettings, TranslationService, IconService);
			return Task.CompletedTask;
		}

		private async Task InitializeEssentialServices()
		{
			using (await _servicesLock.LockAsync())
			{
				ServiceConfigurations configurations = new ServiceConfigurations();
				ConfigureServices(configurations);
				IconService = new IconService(new APIServiceConfiguration
				{
					Enabled = true,
					AwaitLoading = false
				}, ContentsManager);
				_services.Add(IconService);
				TranslationService = new TranslationService(new ServiceConfiguration
				{
					Enabled = true,
					AwaitLoading = true
				}, GetFlurlClient(), MODULE_FILE_URL);
				_services.Add(TranslationService);
				SettingEventService = new SettingEventService(new ServiceConfiguration
				{
					Enabled = true,
					AwaitLoading = false,
					SaveInterval = Timeout.InfiniteTimeSpan
				});
				_services.Add(SettingEventService);
				NewsService = new NewsService(new ServiceConfiguration
				{
					AwaitLoading = true,
					Enabled = true
				}, GetFlurlClient(), MODULE_FILE_URL);
				_services.Add(NewsService);
				MetricsService = new MetricsService(new ServiceConfiguration
				{
					Enabled = true,
					AwaitLoading = true
				}, GetFlurlClient(), API_ROOT_URL, ((Module)this).get_Name(), ((Module)this).get_Namespace(), ModuleSettings, IconService);
				_services.Add(MetricsService);
				ChatService = new ChatService(new ServiceConfiguration
				{
					Enabled = true,
					AwaitLoading = true
				});
				_services.Add(ChatService);
				await OnAfterEssentialsServicesInitialized();
			}
			await StartServices();
		}

		private async Task InitializeServices()
		{
			Logger.Debug("Initialize services");
			string directoryName = GetDirectoryName();
			string directoryPath = null;
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				directoryPath = DirectoriesManager.GetFullDirectoryPath(directoryName);
			}
			using (await _servicesLock.LockAsync())
			{
				ServiceConfigurations configurations = new ServiceConfigurations();
				ConfigureServices(configurations);
				if (configurations.BlishHUDAPI.Enabled)
				{
					if (PasswordManager == null)
					{
						throw new ArgumentNullException("PasswordManager");
					}
					BlishHUDAPIService = new BlishHudApiService(configurations.BlishHUDAPI, ModuleSettings.BlishAPIUsername, PasswordManager, GetFlurlClient(), API_ROOT_URL);
					_services.Add(BlishHUDAPIService);
				}
				if (configurations.Account.Enabled)
				{
					AccountService = new AccountService(configurations.Account, Gw2ApiManager);
					_services.Add(AccountService);
				}
				if (configurations.Audio.Enabled)
				{
					AudioService = new AudioService(configurations.Audio, directoryPath);
					_services.Add(AudioService);
				}
				if (configurations.Items.Enabled)
				{
					ItemService = new ItemService(configurations.Items, Gw2ApiManager, directoryPath, GetFlurlClient(), FILE_ROOT_URL);
					_services.Add(ItemService);
				}
				if (configurations.PlayerTransactions.Enabled)
				{
					PlayerTransactionsService = new PlayerTransactionsService(configurations.PlayerTransactions, ItemService, Gw2ApiManager);
					_services.Add(PlayerTransactionsService);
				}
				if (configurations.Transactions.Enabled)
				{
					TransactionsService = new TransactionsService(configurations.Transactions, ItemService, Gw2ApiManager);
					_services.Add(TransactionsService);
				}
				if (configurations.Worldbosses.Enabled)
				{
					if (configurations.Account.Enabled)
					{
						WorldbossService = new WorldbossService(configurations.Worldbosses, Gw2ApiManager, AccountService);
						_services.Add(WorldbossService);
					}
					else
					{
						Logger.Debug(typeof(WorldbossService).Name + " is not available because " + typeof(AccountService).Name + " is deactivated.");
						configurations.Worldbosses.Enabled = false;
					}
				}
				if (configurations.Mapchests.Enabled)
				{
					if (configurations.Account.Enabled)
					{
						MapchestService = new MapchestService(configurations.Mapchests, Gw2ApiManager, AccountService);
						_services.Add(MapchestService);
					}
					else
					{
						Logger.Debug(typeof(MapchestService).Name + " is not available because " + typeof(AccountService).Name + " is deactivated.");
						configurations.Mapchests.Enabled = false;
					}
				}
				if (configurations.PointOfInterests.Enabled)
				{
					if (string.IsNullOrWhiteSpace(directoryPath))
					{
						throw new ArgumentNullException("directoryPath", "Module directory is not specified.");
					}
					PointOfInterestService = new PointOfInterestService(configurations.PointOfInterests, Gw2ApiManager, directoryPath, GetFlurlClient(), FILE_ROOT_URL);
					_services.Add(PointOfInterestService);
				}
				if (configurations.Skills.Enabled)
				{
					if (string.IsNullOrWhiteSpace(directoryPath))
					{
						throw new ArgumentNullException("directoryPath", "Module directory is not specified.");
					}
					SkillService = new SkillService(configurations.Skills, Gw2ApiManager, IconService, directoryPath, GetFlurlClient(), FILE_ROOT_URL);
					_services.Add(SkillService);
				}
				if (configurations.ArcDPS.Enabled)
				{
					if (configurations.Skills.Enabled)
					{
						ArcDPSService = new ArcDPSService(configurations.ArcDPS, SkillService);
						_services.Add(ArcDPSService);
					}
					else
					{
						Logger.Debug(typeof(ArcDPSService).Name + " is not available because " + typeof(SkillService).Name + " is deactivated.");
						configurations.ArcDPS.Enabled = false;
					}
				}
				if (configurations.Achievements.Enabled)
				{
					AchievementService = new AchievementService(Gw2ApiManager, configurations.Achievements, directoryPath, GetFlurlClient(), FILE_ROOT_URL);
					_services.Add(AchievementService);
				}
				if (configurations.AccountAchievements.Enabled)
				{
					AccountAchievementService = new AccountAchievementService(Gw2ApiManager, configurations.AccountAchievements);
					_services.Add(AccountAchievementService);
				}
				Collection<ManagedService> customServices = GetAdditionalServices(directoryPath);
				if (customServices != null && customServices.Count > 0)
				{
					foreach (ManagedService customService in customServices)
					{
						_services.Add(customService);
					}
				}
				OnBeforeServicesStarted();
			}
			await StartServices();
		}

		private async Task StartServices()
		{
			using (await _servicesLock.LockAsync())
			{
				foreach (ManagedService state2 in _services.Where((ManagedService state) => !state.Running))
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

		protected virtual void ConfigureServices(ServiceConfigurations configurations)
		{
		}

		protected virtual void OnBeforeServicesStarted()
		{
		}

		protected virtual Collection<ManagedService> GetAdditionalServices(string directoryPath)
		{
			return null;
		}

		private void HandleCornerIcon(bool show)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (show)
			{
				if (CornerIcon == null)
				{
					CornerIcon val = new CornerIcon();
					val.set_IconName(((Module)this).get_Name());
					val.set_Priority(CornerIconPriority);
					CornerIcon = val;
					UpdateCornerIcon();
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

		private void UpdateCornerIcon()
		{
			if (CornerIcon != null)
			{
				CornerIcon.set_Icon((ModuleState == ModuleState.Error) ? GetErrorCornerIcon() : GetCornerIcon());
				((Control)CornerIcon).set_BasicTooltipText(ErrorStateText);
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
				SettingsWindow.ToggleWindow();
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
				SettingsWindow.ToggleWindow();
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
				_defaultSettingView = new ModuleSettingsView(IconService, TranslationService);
				_defaultSettingView.OpenClicked += DefaultSettingView_OpenClicked;
				_defaultSettingView.CreateGithubIssueClicked += DefaultSettingView_CreateGithubIssueClicked;
				_defaultSettingView.OpenMessageLogClicked += DefaultSettingView_OpenMessageLogClicked;
			}
			return (IView)(object)_defaultSettingView;
		}

		private void DefaultSettingView_OpenMessageLogClicked(object sender, EventArgs e)
		{
			MessageContainer?.Show();
		}

		private void DefaultSettingView_CreateGithubIssueClicked(object sender, EventArgs e)
		{
			GithubHelper.OpenIssueWindow();
		}

		private void DefaultSettingView_OpenClicked(object sender, EventArgs e)
		{
			SettingsWindow.ToggleWindow();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			((Module)this).OnModuleLoaded(e);
			Logger.Debug("Start building settings window.");
			if (SettingsWindow == null)
			{
				TabbedWindow tabbedWindow2 = (SettingsWindow = WindowUtil.CreateTabbedWindow(ModuleSettings, ((Module)this).get_Name(), ((object)this).GetType(), Guid.Parse("6bd04be4-dc19-4914-a2c3-8160ce76818b"), IconService, GetEmblem()));
			}
			SettingsWindow.Tabs.Add(new Tab(IconService.GetIcon("482926.png"), (Func<IView>)(() => (IView)(object)new NewsView(GetFlurlClient(), Gw2ApiManager, IconService, TranslationService, NewsService)
			{
				DefaultColor = ModuleSettings.DefaultGW2Color
			}), "News", (int?)null));
			OnSettingWindowBuild(SettingsWindow);
			SettingsWindow.Tabs.Add(new Tab(IconService.GetIcon("156331.png"), (Func<IView>)(() => (IView)(object)new DonationView(GetFlurlClient(), Gw2ApiManager, IconService, TranslationService)
			{
				DefaultColor = ModuleSettings.DefaultGW2Color
			}), "Donations", (int?)null));
			if (Debug)
			{
				SettingsWindow.Tabs.Add(new Tab(IconService.GetIcon("155052.png"), (Func<IView>)(() => (IView)(object)new ServiceSettingsView(_services, Gw2ApiManager, IconService, TranslationService, SettingEventService)
				{
					DefaultColor = ModuleSettings.DefaultGW2Color
				}), "Debug", (int?)null));
			}
			Logger.Debug("Finished building settings window.");
			HandleCornerIcon(ModuleSettings.RegisterCornerIcon.get_Value());
			MetricsService.SendMetricAsync("loaded");
		}

		protected abstract AsyncTexture2D GetEmblem();

		protected abstract AsyncTexture2D GetCornerIcon();

		protected virtual AsyncTexture2D GetErrorEmblem()
		{
			return GetEmblem();
		}

		protected virtual AsyncTexture2D GetErrorCornerIcon()
		{
			return GetCornerIcon();
		}

		protected virtual void OnSettingWindowBuild(TabbedWindow settingWindow)
		{
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(CheckBackendHealth, gameTime, _checkBackendInterval.TotalMilliseconds, _lastBackendCheck, doLogging: false);
			ShowUI = CalculateUIVisibility();
			using (_servicesLock.Lock())
			{
				List<string> stateLoadingTexts = new List<string>();
				foreach (ManagedService state in _services)
				{
					state.Update(gameTime);
					APIService apiService = state as APIService;
					if (apiService != null && apiService.Loading)
					{
						if (!string.IsNullOrWhiteSpace(apiService.ProgressText))
						{
							stateLoadingTexts.Add(state.GetType().Name + ": " + apiService.ProgressText);
						}
						else
						{
							stateLoadingTexts.Add(state.GetType().Name);
						}
					}
				}
				string stateTexts = ((stateLoadingTexts.Count == 0) ? null : ("Services:\n" + new string(' ', 4) + string.Join("\n" + new string(' ', 4), stateLoadingTexts)));
				ReportLoading("states", stateTexts);
			}
			StringBuilder loadingTexts = new StringBuilder();
			foreach (KeyValuePair<string, string> loadingText in _loadingTexts)
			{
				if (loadingText.Value != null)
				{
					loadingTexts.AppendLine(loadingText.Value);
				}
			}
			HandleLoadingSpinner(loadingTexts.Length > 0, loadingTexts.ToString().Trim());
		}

		protected void ReportLoading(string group, string loadingText)
		{
			_loadingTexts.AddOrUpdate(group, loadingText, (string key, string oldVal) => loadingText);
		}

		protected void ReportErrorState(ModuleErrorStateGroup group, string errorText)
		{
			_errorStates.AddOrUpdate(group, errorText, (ModuleErrorStateGroup key, string oldVal) => errorText);
			StringBuilder errorStates = new StringBuilder();
			bool hasMultipleErrorStates = _errorStates.Where((KeyValuePair<ModuleErrorStateGroup, string> e) => e.Value != null).Count() > 1;
			foreach (KeyValuePair<ModuleErrorStateGroup, string> errorState in _errorStates)
			{
				if (errorState.Value != null)
				{
					if (hasMultipleErrorStates)
					{
						errorStates.AppendLine("- " + errorState.Value.Trim());
					}
					else
					{
						errorStates.AppendLine(errorState.Value.Trim());
					}
				}
			}
			ErrorStateText = errorStates.ToString().Trim();
			UpdateCornerIcon();
		}

		protected bool HasErrorState(ModuleErrorStateGroup group)
		{
			return _errorStates.Any((KeyValuePair<ModuleErrorStateGroup, string> e) => e.Key == group && !string.IsNullOrWhiteSpace(e.Value));
		}

		protected virtual bool CalculateUIVisibility()
		{
			if (!ModuleSettings.GlobalDrawerVisible.get_Value())
			{
				return false;
			}
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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			show &= CornerIcon != null && ((Control)CornerIcon).get_Visible();
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

		protected async Task ReloadServices()
		{
			List<Task> tasks = new List<Task>();
			using (await _servicesLock.LockAsync())
			{
				tasks.AddRange(_services.Select((ManagedService s) => s.Reload()));
			}
			await Task.WhenAll(tasks);
		}

		protected override void Unload()
		{
			_cancellationTokenSource?.Cancel();
			BackendConnectionRestored -= BaseModule_BackendConnectionRestored;
			Logger.Debug("Unload settings...");
			if (ModuleSettings != null)
			{
				ModuleSettings.RegisterCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)RegisterCornerIcon_SettingChanged);
				ModuleSettings.Unload();
				ModuleSettings = null;
			}
			Logger.Debug("Unloaded settings.");
			Logger.Debug("Unload default settings view...");
			if (_defaultSettingView != null)
			{
				_defaultSettingView.OpenClicked -= DefaultSettingView_OpenClicked;
				_defaultSettingView.CreateGithubIssueClicked -= DefaultSettingView_CreateGithubIssueClicked;
				_defaultSettingView.OpenMessageLogClicked -= DefaultSettingView_OpenMessageLogClicked;
				((View<IPresenter>)(object)_defaultSettingView).DoUnload();
				_defaultSettingView = null;
			}
			Logger.Debug("Unloaded default settings view.");
			Logger.Debug("Unload settings window...");
			TabbedWindow settingsWindow = SettingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Hide();
			}
			TabbedWindow settingsWindow2 = SettingsWindow;
			if (settingsWindow2 != null)
			{
				((Control)settingsWindow2).Dispose();
			}
			SettingsWindow = null;
			Logger.Debug("Unloaded settings window.");
			Logger.Debug("Unloading states...");
			using (_servicesLock.Lock())
			{
				_services?.ToList().ForEach(delegate(ManagedService state)
				{
					state?.Dispose();
				});
				_services?.Clear();
				AccountService = null;
				ArcDPSService = null;
				IconService = null;
				ItemService = null;
				MapchestService = null;
				WorldbossService = null;
				PointOfInterestService = null;
				SettingEventService = null;
				SkillService = null;
				PlayerTransactionsService = null;
				TransactionsService = null;
				TranslationService = null;
			}
			Logger.Debug("Unloaded states.");
			Logger.Debug("Unload flurl client...");
			_flurlClient?.Dispose();
			_flurlClient = null;
			Logger.Debug("Unloaded flurl client.");
			Logger.Debug("Unload corner icon...");
			_loadingTexts?.Clear();
			HandleCornerIcon(show: false);
			LoadingSpinner loadingSpinner = _loadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
			}
			_loadingSpinner = null;
			Logger.Debug("Unloaded corner icon.");
			MessageContainer?.Dispose();
			MessageContainer = null;
		}
	}
}
