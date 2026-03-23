using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Settings;
using SongbookOfTyria.Models.Api;
using SongbookOfTyria.Services;

namespace SongbookOfTyria.Settings
{
	public sealed class ModuleSettings : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

		private readonly SettingEntry<bool> _enableGuildAuthSetting;

		private readonly SettingEntry<string> _gw2ApiKeySetting;

		private readonly SettingCollection _settingsForView;

		private readonly object _verifyLock = new object();

		private TabsCacheService _tabsCacheService;

		private TextureService _textureService;

		private GuildAuthService _guildAuthService;

		private volatile bool _isVerifyingApiKey;

		public bool EnableGuildAuth => _enableGuildAuthSetting.get_Value();

		public string Gw2ApiKey => _gw2ApiKeySetting.get_Value();

		public SettingCollection SettingsForView => _settingsForView;

		public event EventHandler<StatusChangedEventArgs> CacheStatusChanged;

		public event EventHandler<StatusChangedEventArgs> AuthStatusChanged;

		public void SetEnableGuildAuth(bool value)
		{
			_enableGuildAuthSetting.set_Value(value);
		}

		public void SetGw2ApiKey(string value)
		{
			_gw2ApiKeySetting.set_Value(value);
		}

		public ModuleSettings(SettingCollection settings)
		{
			_settingsForView = settings.AddSubCollection("GuildAuth", false);
			_enableGuildAuthSetting = _settingsForView.DefineSetting<bool>("EnableGuildAuth", false, (Func<string>)(() => "Enable OPUS Guild Authentication"), (Func<string>)(() => "When enabled, uses your GW2 API key to verify OPUS guild membership and unlock private tabs."));
			_gw2ApiKeySetting = _settingsForView.DefineSetting<string>("Gw2ApiKey", string.Empty, (Func<string>)(() => "GW2 API Key"), (Func<string>)(() => "Enter your GW2 API key with 'account' permission. Get one at https://account.arena.net/applications"));
			_enableGuildAuthSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableGuildAuthSettingChanged);
			_gw2ApiKeySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnGw2ApiKeySettingChanged);
		}

		public void InitializeServices(TabsCacheService tabsCacheService, TextureService textureService, GuildAuthService guildAuthService)
		{
			_tabsCacheService = tabsCacheService;
			_textureService = textureService;
			_guildAuthService = guildAuthService;
		}

		public async Task InitializeGuildAuthAsync()
		{
			try
			{
				if (_enableGuildAuthSetting.get_Value() && !string.IsNullOrWhiteSpace(_gw2ApiKeySetting.get_Value()))
				{
					await VerifyGuildMembershipAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "InitializeGuildAuthAsync: Error");
			}
		}

		public async Task RefreshDataAsync()
		{
			if (_tabsCacheService != null)
			{
				await _tabsCacheService.RefreshTabsAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async void OnEnableGuildAuthSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				if (string.IsNullOrWhiteSpace(_gw2ApiKeySetting.get_Value()))
				{
					RaiseAuthStatus("Please enter your GW2 API key to enable guild authentication.", StatusType.Warning);
				}
				else
				{
					await VerifyGuildMembershipAsync();
				}
			}
			else
			{
				_guildAuthService?.ClearAuth();
				RaiseAuthStatus("Guild authentication disabled.", StatusType.Info);
			}
		}

		private async void OnGw2ApiKeySettingChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (_enableGuildAuthSetting.get_Value() && !string.IsNullOrWhiteSpace(e.get_NewValue()))
			{
				await VerifyGuildMembershipAsync();
			}
			else if (string.IsNullOrWhiteSpace(e.get_NewValue()))
			{
				_guildAuthService?.ClearAuth();
				RaiseAuthStatus("", StatusType.Info);
			}
		}

		private async Task VerifyGuildMembershipAsync()
		{
			lock (_verifyLock)
			{
				if (_isVerifyingApiKey || _guildAuthService == null)
				{
					return;
				}
				_isVerifyingApiKey = true;
			}
			RaiseAuthStatus("Verifying API key...", StatusType.Info);
			try
			{
				string apiKey = _gw2ApiKeySetting.get_Value();
				if (string.IsNullOrWhiteSpace(apiKey))
				{
					Logger.Warn("VerifyGuildMembershipAsync: No API key configured");
					RaiseAuthStatus("No API key configured.", StatusType.Warning);
					return;
				}
				AuthVerifyResponse response = await _guildAuthService.VerifyApiKeyAsync(apiKey);
				if (response == null)
				{
					RaiseAuthStatus("Failed to verify API key.", StatusType.Error);
				}
				else if (!response.Valid)
				{
					RaiseAuthStatus(response.Message ?? "Invalid API key.", StatusType.Error);
				}
				else if (response.InOpusGuild)
				{
					RaiseAuthStatus("Welcome, " + response.AccountName + "! Private tabs unlocked.", StatusType.Success);
				}
				else
				{
					RaiseAuthStatus("Verified as " + response.AccountName + ", but not an OPUS guild member.", StatusType.Warning);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "VerifyGuildMembershipAsync: Error");
				RaiseAuthStatus("Error verifying guild membership.", StatusType.Error);
			}
			finally
			{
				_isVerifyingApiKey = false;
			}
		}

		private void RaiseAuthStatus(string message, StatusType type)
		{
			this.AuthStatusChanged?.Invoke(this, new StatusChangedEventArgs(message, type));
		}

		public void RaiseCacheStatus(string message, StatusType type)
		{
			this.CacheStatusChanged?.Invoke(this, new StatusChangedEventArgs(message, type));
		}

		public void Dispose()
		{
			if (_enableGuildAuthSetting != null)
			{
				_enableGuildAuthSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableGuildAuthSettingChanged);
			}
			if (_gw2ApiKeySetting != null)
			{
				_gw2ApiKeySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnGw2ApiKeySettingChanged);
			}
		}
	}
}
