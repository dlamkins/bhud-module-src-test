using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neokain.GW2.AllianceManager.Controls.Spam;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Repositories;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.AllianceManager.Services.Authentication;
using Neokain.GW2.AllianceManager.Services.Connection;
using Neokain.GW2.AllianceManager.Services.Logging;
using Neokain.GW2.AllianceManager.Services.Spam;
using Neokain.GW2.AllianceManager.Windows;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Accounts;

namespace Neokain.GW2.AllianceManager
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private const int CORNER_ICON_ASSET_ID = 155052;

		private const int MAIN_WINDOW_ASSET_ID = 155985;

		private const int WALLET_UPDATE_INTERVAL_MINUTES = 5;

		private CornerIcon _cornerIcon;

		private MainWindow _mainWindow;

		private CurrencyWindow _currencyWindow;

		private FavoritesWindow _favoritesWindow;

		private SpamFavoriteRepository _sharedFavoriteRepository;

		private Guid _cachedAccountId = Guid.Empty;

		private volatile Gw2WebClient _webClient;

		private SpamClient _spamClient;

		private ISpamOrchestrator _spamOrchestrator;

		private CooldownOverridePrompt _cooldownOverridePrompt;

		private Gw2ApiKeyProvider _keyProvider;

		private CancellationTokenSource _connectCts;

		private readonly SemaphoreSlim _connectGate = new SemaphoreSlim(1, 1);

		private readonly object _connectStateLock = new object();

		private volatile bool _unloading;

		private static readonly TimeSpan[] _transientBackoff = new TimeSpan[5]
		{
			TimeSpan.FromSeconds(2.0),
			TimeSpan.FromSeconds(5.0),
			TimeSpan.FromSeconds(10.0),
			TimeSpan.FromSeconds(20.0),
			TimeSpan.FromSeconds(30.0)
		};

		private readonly ConnectionStatus _connectionStatus = new ConnectionStatus();

		private List<AccountCurrency> _wallet;

		private List<Currency> _currencies;

		private System.Timers.Timer _walletUpdateTimer;

		private readonly SemaphoreSlim _walletUpdateSemaphore = new SemaphoreSlim(1, 1);

		private readonly TokenPermission[] _requiredPermissions;

		private SettingEntry<bool> _openOnStartup;

		private SettingEntry<bool> _makeUnclosable;

		private SettingEntry<bool> _makeNonResizable;

		private SettingEntry<EndpointSelection> _endpointSelection;

		private SettingEntry<string> _customApiKey;

		private SettingEntry<MessageDelayPreset> _spamMessageDelay;

		private SettingEntry<KeyBinding> _favoritesWindowKeybind;

		private SettingEntry<List<string>> _broadcastHistory;

		private double _countdownTickAccumulatorMs;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public IFontService FontService { get; private set; }

		public SettingEntry<EndpointSelection> EndpointSelectionSetting => _endpointSelection;

		public SettingEntry<string> CustomApiKey => _customApiKey;

		public SettingEntry<bool> MakeUnclosable => _makeUnclosable;

		public SettingEntry<bool> MakeNonResizable => _makeNonResizable;

		public SettingEntry<MessageDelayPreset> SpamMessageDelay => _spamMessageDelay;

		public SettingEntry<List<string>> BroadcastHistory => _broadcastHistory;

		public Gw2WebClient WebClient => _webClient;

		public SpamClient SpamClient => _spamClient;

		public ISpamOrchestrator SpamOrchestrator => _spamOrchestrator;

		public ISpamFavoriteRepository FavoriteRepository => _sharedFavoriteRepository;

		public static Module Instance { get; private set; }

		public ConnectionStatus ConnectionStatus => _connectionStatus;

		public bool IsConnected => _connectionStatus.State == ModuleConnectionState.Connected;

		public IReadOnlyList<AccountCurrency> Wallet => _wallet?.AsReadOnly();

		public IReadOnlyList<Currency> Currencies => _currencies?.AsReadOnly();

		public bool IsCurrencyDataLoaded { get; private set; }

		public event EventHandler ConnectionStatusChanged;

		public event EventHandler WalletUpdated;

		public Task<bool> ShowCooldownOverridePromptAsync(string reason)
		{
			return _cooldownOverridePrompt?.ShowAsync(reason) ?? Task.FromResult(result: false);
		}

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
		{
			TokenPermission[] array = new TokenPermission[3];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			_requiredPermissions = (TokenPermission[])(object)array;
			((Module)this)._002Ector(moduleParameters);
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Expected O, but got Unknown
			settings.DefineSetting<List<int>>("visibleCurrencies", (List<int>)null, (Func<string>)null, (Func<string>)null);
			SettingCollection currencyViewSettingCollection = settings.AddSubCollection("currencyView", true, (Func<string>)(() => "Currency View"));
			_openOnStartup = currencyViewSettingCollection.DefineSetting<bool>("openOnStartup", false, (Func<string>)(() => "Open on Startup"), (Func<string>)null);
			_makeUnclosable = currencyViewSettingCollection.DefineSetting<bool>("makeUnclosable", false, (Func<string>)(() => "Make unclosable"), (Func<string>)null);
			_makeNonResizable = currencyViewSettingCollection.DefineSetting<bool>("makeNonResizable", false, (Func<string>)(() => "Make non-resizable"), (Func<string>)null);
			SettingCollection authSettingCollection = settings.AddSubCollection("authentication", true, (Func<string>)(() => "Authentication"));
			_endpointSelection = authSettingCollection.DefineSetting<EndpointSelection>("endpointSelection", EndpointSelection.Live, (Func<string>)(() => "Server"), (Func<string>)(() => "Which Alliance Manager server to connect to."));
			MigrateEndpointSetting(authSettingCollection);
			_customApiKey = authSettingCollection.DefineSetting<string>("customApiKey", string.Empty, (Func<string>)(() => "API Key"), (Func<string>)(() => "Your Alliance Manager API key (am_…)"));
			SettingCollection spamSettingCollection = settings.AddSubCollection("spam", true, (Func<string>)(() => "Spam Settings"));
			_spamMessageDelay = spamSettingCollection.DefineSetting<MessageDelayPreset>("messageDelay", MessageDelayPreset.Normal, (Func<string>)(() => "Message Delay"), (Func<string>)(() => "Delay between spam messages. Lower = faster but may drop messages."));
			_favoritesWindowKeybind = spamSettingCollection.DefineSetting<KeyBinding>("favoritesKeybind", new KeyBinding((Keys)119), (Func<string>)(() => "Favorites Window"), (Func<string>)(() => "Keybind to toggle the Spam Favorites window."));
			_favoritesWindowKeybind.get_Value().set_Enabled(true);
			_favoritesWindowKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnFavoritesKeybindActivated);
			_broadcastHistory = spamSettingCollection.DefineSetting<List<string>>("broadcastHistory", new List<string>(), (Func<string>)(() => "Broadcast History"), (Func<string>)(() => "Previously sent alliance broadcast messages."));
			_endpointSelection.add_SettingChanged((EventHandler<ValueChangedEventArgs<EndpointSelection>>)OnConnectionSettingChanged<EndpointSelection>);
			_customApiKey.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnConnectionSettingChanged<string>);
		}

		private void OnConnectionSettingChanged<T>(object sender, ValueChangedEventArgs<T> e)
		{
			Logger.Info("Connection setting changed → triggering reconnect.");
			RequestReconnect();
		}

		private void MigrateEndpointSetting(SettingCollection authSettingCollection)
		{
			SettingEntry<string> legacy = authSettingCollection.DefineSetting<string>("apiEndpoint", string.Empty, (Func<string>)null, (Func<string>)null);
			if (!string.IsNullOrWhiteSpace(legacy.get_Value()))
			{
				EndpointSelection migrated = EndpointResolver.MigrateLegacyEndpoint(legacy.get_Value());
				if (_endpointSelection.get_Value() != migrated)
				{
					Logger.Info($"Migrating endpoint setting: '{legacy.get_Value()}' -> {migrated}");
					_endpointSelection.set_Value(migrated);
				}
			}
			authSettingCollection.UndefineSetting("apiEndpoint");
		}

		private string GetModuleVersion()
		{
			return ((object)((Module)this).get_Version())?.ToString() ?? "0.0.0";
		}

		public void RequestReconnect()
		{
			CancellationTokenSource cts;
			lock (_connectStateLock)
			{
				if (_unloading)
				{
					return;
				}
				try
				{
					_connectCts?.Cancel();
				}
				catch (ObjectDisposedException)
				{
				}
				cts = (_connectCts = new CancellationTokenSource());
			}
			RunConnectLoopAsync(cts);
		}

		private async Task RunConnectLoopAsync(CancellationTokenSource cts)
		{
			CancellationToken ct = cts.Token;
			try
			{
				await _connectGate.WaitAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (OperationCanceledException)
			{
				DisposeCts(cts);
				return;
			}
			try
			{
				await ConnectAndAuthenticateAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				try
				{
					_connectGate.Release();
				}
				catch (ObjectDisposedException)
				{
				}
				DisposeCts(cts);
			}
		}

		private void DisposeCts(CancellationTokenSource cts)
		{
			lock (_connectStateLock)
			{
				if (_connectCts == cts)
				{
					_connectCts = null;
				}
			}
			try
			{
				cts.Dispose();
			}
			catch
			{
			}
		}

		private async Task ConnectAndAuthenticateAsync(CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(_customApiKey.get_Value()?.Trim() ?? string.Empty))
			{
				Logger.Info("No AM key configured — not connecting. Configure an AM key to connect.");
				SetStatus(ModuleConnectionState.NoKey, "No API key configured.");
				return;
			}
			if (!_keyProvider.HasApiKey())
			{
				Logger.Info("AM key is malformed (expected am_ + 32-hex GUID) — not connecting.");
				SetStatus(ModuleConnectionState.BadKeyFormat, "API key is malformed. Expected an Alliance Manager key (am_…).");
				return;
			}
			string envOverride = Environment.GetEnvironmentVariable("AM_HUB_ENDPOINT");
			string endpoint = EndpointResolver.ResolveHub(_endpointSelection.get_Value(), envOverride);
			if (!string.IsNullOrWhiteSpace(envOverride))
			{
				Logger.Info("AM_HUB_ENDPOINT override active → " + endpoint);
			}
			string moduleVersion = GetModuleVersion();
			string apiKey = _keyProvider.GetApiKey();
			int attempt = 0;
			while (!ct.IsCancellationRequested)
			{
				try
				{
					if (_webClient?.IsConnected ?? false)
					{
						await _webClient.DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				catch (Exception ex3)
				{
					Logger.Warn(ex3, "Disconnect before reconnect failed — continuing.");
				}
				if (ct.IsCancellationRequested)
				{
					break;
				}
				SetStatus(ModuleConnectionState.Connecting, (attempt == 0) ? "Connecting…" : $"Connecting… (retry {attempt})", endpoint);
				string keyPrefix = ((apiKey.Length >= 8) ? (apiKey.Substring(0, 8) + "…") : "(short)");
				Logger.Info("Connecting → endpoint=" + endpoint + ", key=" + keyPrefix + ", version=" + moduleVersion);
				try
				{
					await _webClient.InitializeAndAuthenticateAsync(endpoint, apiKey, moduleVersion).ConfigureAwait(continueOnCapturedContext: false);
					if (!ct.IsCancellationRequested)
					{
						Logger.Info("Connect/auth established — handing reconnect ownership to the WebClient.");
					}
					return;
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (IncompatibleVersionException ex2)
				{
					Logger.Error((Exception)ex2, "Version incompatible with server — terminal, not retrying.");
					if (!ct.IsCancellationRequested)
					{
						SetStatus(ModuleConnectionState.FailedTerminal, "Module version is incompatible with the server.", endpoint);
					}
					return;
				}
				catch (Exception ex)
				{
					if (ct.IsCancellationRequested)
					{
						return;
					}
					ConnectionFailure failure = ConnectionFailureClassifier.Classify(ex);
					if (failure.Kind == FailureKind.Terminal)
					{
						Logger.Error(ex, "Connect/auth failed terminally: " + failure.Reason);
						SetStatus(ModuleConnectionState.FailedTerminal, failure.Reason, endpoint);
						return;
					}
					Logger.Warn(ex, "Connect/auth failed transiently: " + failure.Reason);
					TimeSpan delay = _transientBackoff[Math.Min(attempt, _transientBackoff.Length - 1)];
					attempt++;
					_connectionStatus.NextRetryUtc = DateTime.UtcNow + delay;
					SetStatus(ModuleConnectionState.FailedTransient, failure.Reason, endpoint);
					try
					{
						await Task.Delay(delay, ct).ConfigureAwait(continueOnCapturedContext: false);
					}
					catch (OperationCanceledException)
					{
						return;
					}
				}
			}
		}

		private void SetStatus(ModuleConnectionState state, string reason, string endpoint = null, string accountName = null)
		{
			lock (_connectStateLock)
			{
				_connectionStatus.State = state;
				_connectionStatus.Reason = reason ?? string.Empty;
				if (endpoint != null)
				{
					_connectionStatus.Endpoint = endpoint;
				}
				_connectionStatus.AccountName = ((state == ModuleConnectionState.Connected) ? accountName : null);
				if (state != ModuleConnectionState.FailedTransient)
				{
					_connectionStatus.NextRetryUtc = null;
				}
				_connectionStatus.UpdatedUtc = DateTime.UtcNow;
			}
			RaiseConnectionStatusChanged();
		}

		private void RaiseConnectionStatusChanged()
		{
			try
			{
				this.ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "ConnectionStatusChanged subscriber threw — ignored.");
			}
		}

		private void OnAuthenticated(object sender, AuthenticatedEventArgs e)
		{
			if (_sharedFavoriteRepository == null || _cachedAccountId != e.AccountId)
			{
				_cachedAccountId = e.AccountId;
				_sharedFavoriteRepository = new SpamFavoriteRepository(_webClient, e.AccountId);
			}
			Logger.Info($"Authenticated as {e.AccountName} (Guilds: {e.Guilds?.Count ?? 0}, Alliances: {e.Alliances?.Count ?? 0})");
			SetStatus(ModuleConnectionState.Connected, "Connected.", null, e.AccountName);
		}

		private void OnCompatibilityFailed(object sender, CompatibilityFailedEventArgs e)
		{
			ScreenNotification.ShowNotification("INCOMPATIBLE VERSION: " + e.CompatibilityMessage, (NotificationType)2, (Texture2D)null, 60);
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).set_BasicTooltipText("⚠\ufe0f Alliance Manager - UPDATE REQUIRED");
			}
			SetStatus(ModuleConnectionState.FailedTerminal, e.CompatibilityMessage);
		}

		private void OnWebClientError(object sender, Exception ex)
		{
			Logger.Warn(ex, "WebClient error event.");
			bool attemptInFlight;
			lock (_connectStateLock)
			{
				attemptInFlight = _connectCts != null;
			}
			if (!attemptInFlight && _connectionStatus.State == ModuleConnectionState.Connected)
			{
				ConnectionFailure failure = ConnectionFailureClassifier.Classify(ex);
				SetStatus((failure.Kind == FailureKind.Terminal) ? ModuleConnectionState.FailedTerminal : ModuleConnectionState.FailedTransient, failure.Reason);
			}
		}

		private void OnWebClientConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
		{
			bool attemptInFlight;
			lock (_connectStateLock)
			{
				attemptInFlight = _connectCts != null;
			}
			if (!attemptInFlight && (e.State == ConnectionState.Reconnecting || e.State == ConnectionState.Disconnected || e.State == ConnectionState.Connecting) && _connectionStatus.State == ModuleConnectionState.Connected)
			{
				SetStatus(ModuleConnectionState.FailedTransient, "Connection lost — reconnecting…");
			}
		}

		private void OnWebClientRetryScheduled(object sender, RetryScheduledEventArgs e)
		{
			bool attemptInFlight;
			lock (_connectStateLock)
			{
				attemptInFlight = _connectCts != null;
			}
			if (!attemptInFlight && (_connectionStatus.State == ModuleConnectionState.Connected || _connectionStatus.State == ModuleConnectionState.FailedTransient))
			{
				_connectionStatus.NextRetryUtc = e.NextAttemptUtc.UtcDateTime;
				SetStatus(ModuleConnectionState.FailedTransient, "Connection lost — reconnecting…");
			}
		}

		protected override void Initialize()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_cornerIcon.set_LoadingMessage((string)null);
			if (_openOnStartup.get_Value())
			{
				((Control)_currencyWindow).Show();
			}
			((Module)this).OnModuleLoaded(e);
		}

		protected override async Task LoadAsync()
		{
			try
			{
				LoadFonts();
				LoadCornerIcon();
				LoadCurrencyWindow();
				_keyProvider = new Gw2ApiKeyProvider(_customApiKey);
				Gw2WebClient.Logger = new WebClientLoggerAdapter();
				LogSignalRInternalizationStatus();
				_webClient = new Gw2WebClient();
				_webClient.Authenticated += new EventHandler<AuthenticatedEventArgs>(OnAuthenticated);
				_webClient.CompatibilityFailed += new EventHandler<CompatibilityFailedEventArgs>(OnCompatibilityFailed);
				_webClient.Error += new EventHandler<Exception>(OnWebClientError);
				_webClient.ConnectionStateChanged += new EventHandler<ConnectionStateChangedEventArgs>(OnWebClientConnectionStateChanged);
				_webClient.RetryScheduled += new EventHandler<RetryScheduledEventArgs>(OnWebClientRetryScheduled);
				_spamClient = new SpamClient(_webClient);
				_spamOrchestrator = new SpamOrchestrator(_spamClient, new SpamSender(), _webClient, SpamSender.GetMessageDelay(this), () => GameService.Gw2Mumble.get_CurrentMap().get_Id(), delegate(string msg)
				{
					ScreenNotification.ShowNotification(msg, (NotificationType)1, (Texture2D)null, 4);
				});
				_cooldownOverridePrompt = new CooldownOverridePrompt();
				if (_mainWindow == null)
				{
					LoadMainWindow();
				}
				RequestReconnect();
				await _003C_003En__0();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load module");
				throw;
			}
		}

		public async Task<bool> LoadCurrencyDataAsync()
		{
			if (IsCurrencyDataLoaded)
			{
				return true;
			}
			try
			{
				await InitializeCurrencyDataAsync();
				InitializeWalletUpdateTimer();
				IsCurrencyDataLoaded = true;
				Logger.Info("Currency data loaded successfully");
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load currency data");
				return false;
			}
		}

		private async Task InitializeCurrencyDataAsync()
		{
			_currencies = ((IEnumerable<Currency>)(await ((IAllExpandableClient<Currency>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Currencies()).AllAsync(default(CancellationToken)))).ToList();
			_currencies.Sort((Currency c1, Currency c2) => c1.get_Order().CompareTo(c2.get_Order()));
			await UpdateWalletAsync();
		}

		private void InitializeWalletUpdateTimer()
		{
			_walletUpdateTimer = new System.Timers.Timer(300000.0);
			_walletUpdateTimer.Elapsed += OnWalletUpdateTimerElapsed;
			_walletUpdateTimer.AutoReset = true;
			_walletUpdateTimer.Start();
		}

		private void OnWalletUpdateTimerElapsed(object sender, ElapsedEventArgs e)
		{
			Task.Run(async delegate
			{
				await UpdateWalletAsync();
			});
		}

		private async Task UpdateWalletAsync()
		{
			await _walletUpdateSemaphore.WaitAsync();
			try
			{
				_wallet = ((IEnumerable<AccountCurrency>)(await ((IBlobClient<IApiV2ObjectList<AccountCurrency>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Wallet()).GetAsync(default(CancellationToken)))).ToList();
				this.WalletUpdated?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to update wallet data.");
			}
			finally
			{
				_walletUpdateSemaphore.Release();
			}
		}

		private void LoadFonts()
		{
			FontService = new FontService(ContentsManager);
		}

		private void LoadCurrencyWindow()
		{
			_currencyWindow = new CurrencyWindow(this);
		}

		private void LoadMainWindow()
		{
			MainWindow mainWindow = new MainWindow(this, WebClient);
			((Control)mainWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_mainWindow = mainWindow;
		}

		private void LoadCornerIcon()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.FromAssetId(155052));
			((Control)val).set_BasicTooltipText("Alliance & Guild Manager");
			val.set_Priority(1645672343);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_LoadingMessage("Initializing...");
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconOnClick);
		}

		private void OnCornerIconOnClick(object s, MouseEventArgs args)
		{
			if (_mainWindow == null)
			{
				ScreenNotification.ShowNotification("Module still loading...", (NotificationType)0, (Texture2D)null, 4);
			}
			if (((Control)_mainWindow).get_Visible())
			{
				((Control)_mainWindow).Hide();
			}
			else
			{
				((Control)_mainWindow).Show();
			}
		}

		private void OnFavoritesKeybindActivated(object sender, EventArgs e)
		{
			ToggleFavoritesWindowAsync();
		}

		private async Task ToggleFavoritesWindowAsync()
		{
			if (_webClient == null || !_webClient.IsConnected || !_webClient.IsVerified)
			{
				ScreenNotification.ShowNotification("Not connected — reconnect on the Connection tab.", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			if (_favoritesWindow == null)
			{
				try
				{
					if (_cachedAccountId == Guid.Empty)
					{
						AccountDataDto account = await _webClient.GetMyAccount();
						if (account == null)
						{
							ScreenNotification.ShowNotification("Account data not available yet", (NotificationType)1, (Texture2D)null, 4);
							return;
						}
						_cachedAccountId = account.Id;
					}
					if (_sharedFavoriteRepository == null)
					{
						_sharedFavoriteRepository = new SpamFavoriteRepository(_webClient, _cachedAccountId);
					}
					_favoritesWindow = new FavoritesWindow(this, _sharedFavoriteRepository, _webClient);
				}
				catch (Exception ex)
				{
					ScreenNotification.ShowNotification("Failed to create favorites window: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
					return;
				}
			}
			if (((Control)_favoritesWindow).get_Visible())
			{
				((Control)_favoritesWindow).Hide();
			}
			else
			{
				((Control)_favoritesWindow).Show();
			}
		}

		public void ToggleCurrencyWindow()
		{
			if (_currencyWindow != null)
			{
				if (((Control)_currencyWindow).get_Visible())
				{
					((Control)_currencyWindow).Hide();
				}
				else
				{
					((Control)_currencyWindow).Show();
				}
			}
		}

		public async void ToggleFavoritesWindow()
		{
			if (_webClient == null || !_webClient.IsConnected || !_webClient.IsVerified)
			{
				ScreenNotification.ShowNotification("Not connected — reconnect on the Connection tab.", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			if (_favoritesWindow == null)
			{
				try
				{
					if (_cachedAccountId == Guid.Empty)
					{
						AccountDataDto account = await _webClient.GetMyAccount();
						if (account == null)
						{
							ScreenNotification.ShowNotification("Account data not available yet", (NotificationType)1, (Texture2D)null, 4);
							return;
						}
						_cachedAccountId = account.Id;
					}
					if (_sharedFavoriteRepository == null)
					{
						_sharedFavoriteRepository = new SpamFavoriteRepository(_webClient, _cachedAccountId);
					}
					_favoritesWindow = new FavoritesWindow(this, _sharedFavoriteRepository, _webClient);
				}
				catch (Exception ex)
				{
					ScreenNotification.ShowNotification("Failed to create favorites window: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
					return;
				}
			}
			if (((Control)_favoritesWindow).get_Visible())
			{
				((Control)_favoritesWindow).Hide();
			}
			else
			{
				((Control)_favoritesWindow).Show();
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (_connectionStatus.State != ModuleConnectionState.FailedTransient || !_connectionStatus.NextRetryUtc.HasValue)
			{
				_countdownTickAccumulatorMs = 0.0;
				return;
			}
			_countdownTickAccumulatorMs += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_countdownTickAccumulatorMs >= 1000.0)
			{
				_countdownTickAccumulatorMs = 0.0;
				RaiseConnectionStatusChanged();
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void LogSignalRInternalizationStatus()
		{
			try
			{
				AssemblyName asm = typeof(Activity).Assembly.GetName();
				Logger.Info($"DiagnosticSource/Activity resolved from: {asm.Name} {asm.Version} " + ((asm.Name == "Neokain.GW2.AllianceManager") ? "(ILRepack VERIFIED: internalized)" : "(WARNING: NOT internalized — expect SignalR TypeLoadException)"));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Activity type could not be resolved — this build is unmerged and SignalR will fail inside BlishHUD. Build with /p:Configuration=Release /p:EnableILRepack=true.");
			}
		}

		protected override void Unload()
		{
			_walletUpdateTimer?.Stop();
			_walletUpdateTimer?.Dispose();
			CancellationTokenSource ctsToDispose;
			lock (_connectStateLock)
			{
				_unloading = true;
				ctsToDispose = _connectCts;
				_connectCts = null;
			}
			if (ctsToDispose != null)
			{
				try
				{
					ctsToDispose.Cancel();
				}
				catch
				{
				}
				try
				{
					ctsToDispose.Dispose();
				}
				catch
				{
				}
			}
			_walletUpdateSemaphore?.Dispose();
			_connectGate?.Dispose();
			FontService?.Dispose();
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			MainWindow mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			CurrencyWindow currencyWindow = _currencyWindow;
			if (currencyWindow != null)
			{
				((Control)currencyWindow).Dispose();
			}
			FavoritesWindow favoritesWindow = _favoritesWindow;
			if (favoritesWindow != null)
			{
				((Control)favoritesWindow).Dispose();
			}
			_spamClient?.Dispose();
			CooldownOverridePrompt cooldownOverridePrompt = _cooldownOverridePrompt;
			if (cooldownOverridePrompt != null)
			{
				((Control)cooldownOverridePrompt).Dispose();
			}
			if (_webClient != null)
			{
				_webClient.Authenticated -= new EventHandler<AuthenticatedEventArgs>(OnAuthenticated);
				_webClient.CompatibilityFailed -= new EventHandler<CompatibilityFailedEventArgs>(OnCompatibilityFailed);
				_webClient.Error -= new EventHandler<Exception>(OnWebClientError);
				_webClient.ConnectionStateChanged -= new EventHandler<ConnectionStateChangedEventArgs>(OnWebClientConnectionStateChanged);
				_webClient.RetryScheduled -= new EventHandler<RetryScheduledEventArgs>(OnWebClientRetryScheduled);
			}
			WebClient?.DisconnectAsync().Wait(TimeSpan.FromSeconds(5.0));
			if (_favoritesWindowKeybind?.get_Value() != null)
			{
				_favoritesWindowKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnFavoritesKeybindActivated);
			}
			if (_endpointSelection != null)
			{
				_endpointSelection.remove_SettingChanged((EventHandler<ValueChangedEventArgs<EndpointSelection>>)OnConnectionSettingChanged<EndpointSelection>);
			}
			if (_customApiKey != null)
			{
				_customApiKey.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnConnectionSettingChanged<string>);
			}
			Instance = null;
			((Module)this).Unload();
		}
	}
}
