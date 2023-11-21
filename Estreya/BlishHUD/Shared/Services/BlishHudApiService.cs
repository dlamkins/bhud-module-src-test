using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Models.BlishHudAPI;
using Estreya.BlishHUD.Shared.Security;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Jose;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Services
{
	public class BlishHudApiService : ManagedService
	{
		private const string API_PASSWORD_KEY = "estreyaBlishHudAPI";

		private static TimeSpan _checkAPITokenInterval = TimeSpan.FromMinutes(5.0);

		private readonly string _apiRootUrl;

		private readonly string _apiVersion;

		private readonly AsyncRef<double> _lastAPITokenCheck = new AsyncRef<double>(0.0);

		private IFlurlClient _flurlClient;

		private PasswordManager _passwordManager;

		private SettingEntry<string> _usernameSetting;

		private APITokens? APITokens { get; set; }

		public string AccessToken => APITokens?.AccessToken;

		public event EventHandler RefreshedLogin;

		public event EventHandler NewLogin;

		public event EventHandler LoggedOut;

		public BlishHudApiService(ServiceConfiguration configuration, SettingEntry<string> usernameSetting, PasswordManager passwordManager, IFlurlClient flurlClient, string apiRootUrl, string apiVersion)
			: base(configuration)
		{
			_usernameSetting = usernameSetting;
			_passwordManager = passwordManager;
			_flurlClient = flurlClient;
			_apiRootUrl = apiRootUrl;
			_apiVersion = apiVersion;
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			_usernameSetting = null;
			_passwordManager = null;
			_flurlClient = null;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(CheckAPITokenExpiration, gameTime, _checkAPITokenInterval.TotalMilliseconds, _lastAPITokenCheck);
		}

		protected override async Task Load()
		{
			await APILogin();
		}

		public string GetAPIUsername()
		{
			return _usernameSetting.get_Value();
		}

		public string SetAPIUsername(string username)
		{
			string result;
			_usernameSetting.set_Value(result = username);
			return result;
		}

		public async Task<string> GetAPIPassword()
		{
			if (_passwordManager == null)
			{
				throw new ArgumentNullException("_passwordManager");
			}
			byte[] passwordData = await _passwordManager.Retrive("estreyaBlishHudAPI", silent: true);
			return (passwordData == null) ? null : Encoding.UTF8.GetString(passwordData);
		}

		public async Task SetAPIPassword(string password)
		{
			if (_passwordManager == null)
			{
				throw new ArgumentNullException("_passwordManager");
			}
			if (password == null)
			{
				_passwordManager.Delete("estreyaBlishHudAPI");
			}
			else
			{
				await _passwordManager.Save("estreyaBlishHudAPI", Encoding.UTF8.GetBytes(password));
			}
		}

		private ApiJwtPayload? GetTokenPayload(string token = null)
		{
			if (token == null)
			{
				token = APITokens?.AccessToken;
			}
			if (!string.IsNullOrWhiteSpace(token))
			{
				return JWT.Payload<ApiJwtPayload>(token);
			}
			return null;
		}

		public async Task TestLogin(string username = null, string password = null)
		{
			await APILogin(username, password, dryRun: true, throwException: true);
		}

		public async Task Login()
		{
			await APILogin(null, null, dryRun: false, throwException: true);
		}

		public void Logout()
		{
			APITokens = null;
			this.LoggedOut?.Invoke(this, EventArgs.Empty);
		}

		private async Task APILogin(string username = null, string password = null, bool dryRun = false, bool throwException = false)
		{
			_ = 2;
			try
			{
				if (username == null)
				{
					username = _usernameSetting.get_Value();
				}
				if (password == null)
				{
					password = await GetAPIPassword();
				}
				if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
				{
					Logger.Info("Credentials not available.");
					if (throwException)
					{
						throw new ArgumentNullException("Credentials");
					}
					return;
				}
				APITokens tokens = JsonConvert.DeserializeObject<APITokens>(await (await _flurlClient.Request(_apiRootUrl, "v" + _apiVersion, "auth", "login").PostJsonAsync(new { username, password }, default(CancellationToken), (HttpCompletionOption)0)).get_Content().ReadAsStringAsync());
				if (!dryRun)
				{
					ApiJwtPayload? priorPayload = GetTokenPayload();
					APITokens = new APITokens
					{
						AccessToken = tokens.AccessToken,
						RefreshToken = tokens.RefreshToken
					};
					ApiJwtPayload? currentPayload = GetTokenPayload();
					if (!priorPayload.HasValue || (currentPayload.HasValue && priorPayload.Value.Id != currentPayload.Value.Id))
					{
						this.NewLogin?.Invoke(this, EventArgs.Empty);
					}
					else
					{
						this.RefreshedLogin?.Invoke(this, EventArgs.Empty);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "API Login failed:");
				if (!dryRun)
				{
					APITokens = null;
				}
				if (throwException)
				{
					throw;
				}
			}
		}

		private async Task RefreshAPILogin()
		{
			_ = 1;
			try
			{
				if (!APITokens.HasValue || string.IsNullOrWhiteSpace(APITokens.Value.RefreshToken))
				{
					throw new ArgumentNullException("Refresh API Token");
				}
				APITokens tokens = JsonConvert.DeserializeObject<APITokens>(await (await _flurlClient.Request(_apiRootUrl, "v" + _apiVersion, "auth", "refresh").PostJsonAsync(new
				{
					refreshToken = APITokens.Value.RefreshToken
				}, default(CancellationToken), (HttpCompletionOption)0)).get_Content().ReadAsStringAsync());
				ApiJwtPayload? priorPayload = GetTokenPayload();
				APITokens = new APITokens
				{
					AccessToken = tokens.AccessToken,
					RefreshToken = tokens.RefreshToken
				};
				ApiJwtPayload? currentPayload = GetTokenPayload();
				if (!priorPayload.HasValue || (currentPayload.HasValue && priorPayload.Value.Id != currentPayload.Value.Id))
				{
					this.NewLogin?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					this.RefreshedLogin?.Invoke(this, EventArgs.Empty);
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Refresh API Login failed:");
				APITokens = null;
			}
		}

		private bool IsTokenExpired(string token)
		{
			ApiJwtPayload? payload = GetTokenPayload(token);
			if (!payload.HasValue)
			{
				return true;
			}
			DateTimeOffset expiresAt = DateTimeOffset.FromUnixTimeSeconds(payload.Value.Expiration);
			return DateTime.UtcNow > expiresAt;
		}

		private async Task CheckAPITokenExpiration()
		{
			if (APITokens.HasValue && IsTokenExpired(APITokens.Value.AccessToken))
			{
				if (IsTokenExpired(APITokens.Value.RefreshToken))
				{
					await APILogin();
				}
				else
				{
					await RefreshAPILogin();
				}
			}
		}
	}
}
